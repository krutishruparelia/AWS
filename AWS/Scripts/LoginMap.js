$(function () {
    var mapJson = mapd;
    var markerUrl = "https://js.devexpress.com/Demos/RealtorApp/images/map-marker.png",
        markersData = mapJson;

    var mapWidget = $("#map").dxMap({
        provider: "bing",
        zoom: 5,
        height: 940,
        width: "100%",
        controls: true,
        markerIconSrc: markerUrl,
        markers: markersData
    }).dxMap("instance");

    $("#use-custom-markers").dxCheckBox({
        value: true,
        text: "Use custom marker icons",
        onValueChanged: function (data) {
            mapWidget.option("markers", markersData);
            mapWidget.option("markerIconSrc", data.value ? markerUrl : null);
        }
    });

    $("#show-tooltips").dxButton({
        text: "Show all tooltips",
        onClick: function () {
            var newMarkers = $.map(markersData, function (item) {
                return $.extend(true, {}, item, { tooltip: { isShown: true } });
            });

            mapWidget.option("markers", newMarkers);
        }
    });
});