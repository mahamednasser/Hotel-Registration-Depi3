
$(document).on("click", ".previewable", function () {
    const src = $(this).attr("src");
    if (!src) return;

    $("#previewImage").attr("src", src);

    const modalEl = document.getElementById("imagePreviewModal");
    if (modalEl) {
        const modal = new bootstrap.Modal(modalEl);
        modal.show();
    }
});
