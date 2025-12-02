$(document).ready(function () {

    // Base Image Preview
    $("#baseImageInput").change(function (event) {
        const file = event.target.files[0];
        const $baseImageDisplay = $("#baseImageDisplay");
        const $coverPlaceholder = $("#coverPlaceholder");

        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $baseImageDisplay.attr("src", e.target.result).show();
                $coverPlaceholder.hide();
            }
            reader.readAsDataURL(file);
        } else {
            $baseImageDisplay.hide().attr("src", "");
            $coverPlaceholder.show();
        }
    });

    // Static slots setup
    const $slots = [
        { display: $("#slot1ImageDisplay"), icon: $("#slot1Icon"), wrapper: $("#slot1Placeholder") },
        { display: $("#slot2ImageDisplay"), icon: $("#slot2Icon"), wrapper: $("#slot2Placeholder") },
        { display: $("#slot3ImageDisplay"), icon: $("#slot3Icon"), wrapper: $("#slot3Placeholder") }
    ];

    function resetAdditionalPlaceholders() {
        $slots.forEach(slot => {
            slot.display.hide().attr("src", "");
            slot.icon.show();
        });
        $("#dynamicAdditionalImages").empty().hide();
    }

    resetAdditionalPlaceholders();

    // Additional images logic
    $("#additionalImagesInput").change(function (event) {
        const files = Array.from(event.target.files);

        resetAdditionalPlaceholders();

        if (files.length > 0) {
            files.slice(0, 3).forEach((file, index) => {
                const slot = $slots[index];
                const reader = new FileReader();

                reader.onload = function (e) {
                    slot.display.attr("src", e.target.result).show();
                    slot.icon.hide();
                }
                reader.readAsDataURL(file);
            });

            if (files.length > 3) {
                const dynamicFiles = files.slice(3);
                const $dynamicWrapper = $("#dynamicAdditionalImages");

                $dynamicWrapper.show();

                dynamicFiles.forEach(file => {
                    const reader = new FileReader();
                    reader.onload = function (e) {
                        const imageUrl = e.target.result;
                        const $htmlSlot = $(`
                            <div class="additional-image-slot border rounded-3 overflow-hidden"
                                 style="width: 120px; height: 90px; background-color: #f8f9fa;">
                                <img src="${imageUrl}"
                                     class="img-fluid w-100 h-100 object-fit-cover previewable"
                                     style="cursor: pointer;">
                            </div>
                        `);
                        $dynamicWrapper.append($htmlSlot);
                    }
                    reader.readAsDataURL(file);
                });
            }
        }
    });

    // Image Preview Modal
    $(document).on("click", ".previewable", function () {
        let src = $(this).attr("src");
        if (src) {
            $("#previewImage").attr("src", src);
            let modal = new bootstrap.Modal(document.getElementById("imagePreviewModal"));
            modal.show();
        }
    });

});
