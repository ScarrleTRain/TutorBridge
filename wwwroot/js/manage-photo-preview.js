$(document).ready(function () {
    $("#Input_ProfilePhoto").on("change", function (e) {
        const file = e.target.files[0];
        if (!window.PhotoUpload.validateAndPreview(file, $("#photoPreview"), $("#photoError"))) {
            e.target.value = "";
        }
    });
});