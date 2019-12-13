
$.ajax({
    type: "GET",
    url: "/Index/StationlocationALL",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    cache: false,
    processData: false,
    success: function (response) {

        var markers = response;
        window.onload = function () {

            var mapOptions = {
                center: new google.maps.LatLng(markers[0].Latitude, markers[0].Longitude),
                zoom: 1,
                mapTypeId: 'roadmap'
            };
           
            var infoWindow = new google.maps.InfoWindow();
            var latlngbounds = new google.maps.LatLngBounds();
            var map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
            var i = 0;
            var interval = setInterval(function () {
                var data = markers[i];
                var myLatlng = new google.maps.LatLng(data.Latitude, data.Longitude);
                var icon = "";
                switch (data.ID) {
                    case 1:
                        icon = "red";
                        break;
                    case 2:
                        icon = "blue";
                        break;
                    case 3:
                        icon = "yellow";
                        break;
                    case 4:
                        icon = "green";
                        break;
                    default:
                        icon = "yellow";
                }
                icon = "http://maps.google.com/mapfiles/ms/icons/" + icon + ".png";
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: data.Name,
                    animation: google.maps.Animation.DROP,
                    icon: new google.maps.MarkerImage(icon)
                });
                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function (e) {
                        infoWindow.setContent(data.Name);
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
                latlngbounds.extend(marker.position);
                i++;
                if (i === markers.length) {
                    clearInterval(interval);
                    var bounds = new google.maps.LatLngBounds();
                    map.setCenter(latlngbounds.getCenter());
                    map.fitBounds(latlngbounds);
                    map.setTilt(45);
                }
            }, 10);
        };
        
    },
    failure: function (response) {
        //alert(response.responseText);
        console.log(response.responseText);
    },
    error: function (response) {
        //alert(response.responseText);
        console.log(response.responseText);
    }
});

function show_popup() {
    $('#myModal').modal({
        show: true,
        keyboard: false,
        backdrop: 'static'
    });
}
function fullScreen() {
    var doc = window.document;
    var docEl = doc.documentElement;

    var requestFullScreen = docEl.requestFullscreen || docEl.mozRequestFullScreen || docEl.webkitRequestFullScreen || docEl.msRequestFullscreen;
    var cancelFullScreen = doc.exitFullscreen || doc.mozCancelFullScreen || doc.webkitExitFullscreen || doc.msExitFullscreen;

    if (!doc.fullscreenElement && !doc.mozFullScreenElement && !doc.webkitFullscreenElement && !doc.msFullscreenElement) {
        requestFullScreen.call(docEl);
    }
    else {
        cancelFullScreen.call(doc);
    }
}
window.setTimeout(show_popup, 3000);