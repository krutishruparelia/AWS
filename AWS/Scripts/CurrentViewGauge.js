$(function () {
    $("#gauge").dxCircularGauge({
        scale: {
            startValue: 0,
            endValue: 230,
            tickInterval: 20,
            label: {
                customizeText: function (arg) {
                    return arg.valueText + " v";
                }
            }
        },
        rangeContainer: {
            ranges: [
                { startValue: 0, endValue: 20, color: "#CE2029" },
                { startValue: 20, endValue: 50, color: "#FFD700" },
                { startValue: 50, endValue: 100, color: "#228B22" }
            ]
        },
        tooltip: { enabled: true },
        "export": {
            enabled: true
        },
        title: {
            text: "Battery Charge",
            font: { size: 28 }
        },
        value: batteryvoltage
    });
});