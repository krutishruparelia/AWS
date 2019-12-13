$(function () {
    var mapJson = mapd;
    var markerUrl = "https://js.devexpress.com/Demos/RealtorApp/images/map-marker.png",
        markersData = mapJson;

    var mapWidget = $("#map").dxMap({
        provider: "google",
        zoom: 5,
        height: 445,
        width: "100%",
        controls: true,
        markerIconSrc: markerUrl,
        markers: markersData,
        key: {
            // NOTE: Specify your map API keys for every used provider
            //bing: "YOUR_BING_MAPS_API_KEY",
            google: "AIzaSyCI1lbHfNJ3XTbX0eWfzE7uqj5o2mATel4"
        }
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