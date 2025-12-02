$(document).ready(function () {

    $("#saveTypeBtn").on("click", function () {
        const form = $("#addTypeForm");
        const formData = {
            RoomTypeName: $("#RoomTypeName").val(),
            RoomTypeDescription: $("#RoomTypeDescription").val(),
        };

        $.ajax({
            url: '/Guest/RoomType/AddType',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    $("#room-type").append(
                        `<option value="${response.id}" selected>${response.name}</option>`
                    );
                    const modal = bootstrap.Modal.getInstance(document.getElementById('addTypeModal'));
                    modal.hide();
                    form[0].reset();
                }
            },
            error: function (xhr, status, error) {
                console.error("Error adding room type:", error);
            }
        });
    });

});
