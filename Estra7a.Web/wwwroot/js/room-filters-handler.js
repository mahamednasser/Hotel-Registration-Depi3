$(document).ready(function () {
    function filterRooms() {
        var price = $("#price_filter").val();
        var capacity = $("#capacity_filter").val();
        var roomType = $("#room_type_filter").val();

        $.ajax({
            url: "/Guest/Room/Filter",
            type: "GET",
            data: {
                priceRange: price,
                capacityRange: capacity,
                roomType: roomType
            },
            success: function (result) {
                $("#rooms_container").html(result);
            },
            error: function (xhr) {
                console.error("Filter error:", xhr.responseText);
            }
        });
    }

    $("#price_filter, #capacity_filter, #room_type_filter").change(filterRooms);
});
