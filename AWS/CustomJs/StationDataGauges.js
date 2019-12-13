$(function () {
    var temperatureGauge = $("#temperatureGauge").dxLinearGauge({
        title: {
            text: "Temperature (°C)",
            font: {
                size: 16
            }
        },
        geometry: { orientation: "vertical" },
        scale: {
            startValue: -40,
            endValue: 40,
            tickInterval: 40
        },
        rangeContainer: {
            backgroundColor: "none",
            ranges: [
                { startValue: -40, endValue: 0, color: "#679EC5" },
                { startValue: 0, endValue: 40 }
            ]
        },
        value: cities[0].data.temperature
    }).dxLinearGauge("instance");



    $("#selectbox").dxSelectBox({
        dataSource: cities,
        onSelectionChanged: function (e) {
            var weatherData = e.selectedItem.data;

            temperatureGauge.option("value", weatherData.temperature);
            humidityGauge.option("value", weatherData.humidity);
            pressureGauge.option("value", weatherData.pressure);
        },
        displayExpr: "name",
        value: cities[0]
    });
});