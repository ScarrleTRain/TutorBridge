(function () {
    "use strict";

    const MAX_PHOTO_BYTES = 20 * 1024 * 1024; // mirrors AllowedFileAttribute server-side
    const ALLOWED_PHOTO_TYPES = ["image/jpeg", "image/png"];
    const MIN_AGE_YEARS = 5; // mirrors MinAgeAttribute server-side

    const $form = $("#registerForm");

    let currentStep = 1;
    let maxUnlockedStep = 1;
    let emailChecked = false;
    let emailAvailable = true;

    function showStep(step) {
        $(".reg-step").each(function () {
            $(this).toggleClass("d-none", parseInt($(this).data("step"), 10) !== step);
        });
        $(".step-pill").each(function () {
            const s = parseInt($(this).data("step"), 10);
            $(this)
                .toggleClass("active", s === step)
                .toggleClass("locked", s > maxUnlockedStep)
                .attr("aria-disabled", s > maxUnlockedStep ? "true" : "false");
        });
        currentStep = step;
    }

    function validateField(selector) {
        return $form.validate().element(selector);
    }

    async function checkEmailAvailability() {
        const email = $("#Input_Email").val();
        if (!email) return;

        const response = await fetch(`?handler=CheckEmail&email=${encodeURIComponent(email)}`);
        const data = await response.json();

        emailAvailable = data.available;
        emailChecked = true;

        if (!emailAvailable) {
            $form.validate().showErrors({ "Input.Email": "This email is already registered." });
        } else {
            validateField("#Input_Email"); // re-runs standard rules, clearing the message above if now clean
        }
    }

    function isOldEnough(dobValue, years) {
        if (!dobValue) return false;
        const dob = new Date(dobValue);
        const cutoff = new Date();
        cutoff.setFullYear(cutoff.getFullYear() - years);
        return dob <= cutoff;
    }

    async function validateStep1() {
        const fields = ["#Input_Email", "#Input_Password", "#Input_ConfirmPassword", "#Input_NameFirst", "#Input_NameLast", "#Input_Phone"];
        let allValid = fields.map(f => validateField(f)).every(Boolean);

        if (!emailChecked) {
            await checkEmailAvailability();
        }
        if (!emailAvailable) allValid = false;

        return allValid;
    }

    function validateStep2() {
        if (!validateField("#Input_BirthDate")) return false;

        if (!isOldEnough($("#Input_BirthDate").val(), MIN_AGE_YEARS)) {
            $form.validate().showErrors({ "Input.BirthDate": `You must be at least ${MIN_AGE_YEARS} years old.` });
            return false;
        }
        return true;
    }

    async function goNext() {
        let valid = false;
        if (currentStep === 1) valid = await validateStep1();
        else if (currentStep === 2) valid = validateStep2();

        if (valid) {
            maxUnlockedStep = Math.max(maxUnlockedStep, currentStep + 1);
            showStep(currentStep + 1);
        }
    }

    function goBack() {
        showStep(currentStep - 1);
    }

    function goToStep(step) {
        if (step <= maxUnlockedStep) {
            showStep(step);
        }
    }

    function handlePhotoChange(e) {
        const file = e.target.files[0];
        const $preview = $("#photoPreview");
        const $error = $("#photoError");

        $error.text("").addClass("d-none");

        if (!file) {
            $preview.addClass("d-none").attr("src", "");
            return;
        }

        if (file.size > MAX_PHOTO_BYTES) {
            $error.text("File must be smaller than 5MB.").removeClass("d-none");
            e.target.value = "";
            $preview.addClass("d-none").attr("src", "");
            return;
        }
        if (!ALLOWED_PHOTO_TYPES.includes(file.type)) {
            $error.text("Only JPEG and PNG images are allowed.").removeClass("d-none");
            e.target.value = "";
            $preview.addClass("d-none").attr("src", "");
            return;
        }

        const reader = new FileReader();
        reader.onload = ev => $preview.attr("src", ev.target.result).removeClass("d-none");
        reader.readAsDataURL(file);
    }

    $(document).ready(function () {
        showStep(1);

        $("#Input_Email").on("blur", checkEmailAvailability);
        $("#Input_Email").on("input", () => { emailChecked = false; });

        $(".step-next").on("click", goNext);
        $(".step-back").on("click", goBack);
        $(".step-pill").on("click", function () {
            goToStep(parseInt($(this).data("step"), 10));
        });

        $("#Input_ProfilePhoto").on("change", handlePhotoChange);
    });
})();