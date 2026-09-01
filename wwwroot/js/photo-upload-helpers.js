window.PhotoUpload = (function () {
    "use strict";

    const MAX_BYTES = 20 * 1024 * 1024; // mirrors AllowedFileAttribute server-side
    const ALLOWED_TYPES = ["image/jpeg", "image/png"];

    // Validates a file and, if valid, previews it. Deliberately does nothing to
    // $preview when no file is given — "no file" means different things on
    // different pages, so that decision is left to the caller.
    function validateAndPreview(file, $preview, $error) {
        if (!file) {
            $error.text("").addClass("d-none");
            return true;
        }

        if (file.size > MAX_BYTES) {
            $error.text("File must be smaller than 20MB.").removeClass("d-none");
            return false;
        }
        if (!ALLOWED_TYPES.includes(file.type)) {
            $error.text("Only JPEG and PNG images are allowed.").removeClass("d-none");
            return false;
        }

        $error.text("").addClass("d-none");
        const reader = new FileReader();
        reader.onload = ev => $preview.attr("src", ev.target.result).removeClass("d-none");
        reader.readAsDataURL(file);
        return true;
    }

    return { validateAndPreview, MAX_BYTES, ALLOWED_TYPES };
})();