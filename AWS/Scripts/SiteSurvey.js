$(function () {
    $("#gridContainer").dxDataGrid({
        dataSource: employees,
        allowColumnReordering: true,
        allowColumnResizing: true,
        columnAutoWidth: true,
        showBorders: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
            enabled: true
        },
        editing: {
            mode: "popup",
            allowUpdating: true,
            allowDeleting: true,
            allowAdding: true,
            editEnabled: true,
            searchPanel: true
        },
        columns: [{
            caption: "ProjectName",
            width: 230,
            fixed: true,
            calculateCellValue: function (data) {
                return [data.projectName]
                    .join(" ");
            }
        },
"SiteName",
"State",
            "City",
            {
                dataField: "DateofVisit",
                dataType: "date"
            }, 
            "District",
            "Basin",
            "Village",
            "Latitude",
            "Longitude",
            "Altitude",
            "Landmark",
            "Address",
            "CoordinatorName",
            "CoordinatorContact",
            "CoordinatorAddress",
            "SiteInchargeName",
            "SiteInchargeContact",
            "SiteAddress",
            "WorkingWeekDays",
            {
                dataField: "StationType",
                caption: "Station Type",
                width: 125,
                lookup: {
                    dataSource: stationtype,
                    displayExpr: "Name",
                    valueExpr: "Name"
                }
            },
            "Site",
            "LandType",
            "ElectromagneticInterface",
            "UndergroundObstruction",
            "HighTensionsPowerLines",
            "HeatSources",
            "Grass",
            "SpeficProcedure",
            {
                dataField: "SiteCleaning",
                caption: "Site Cleaning/Leveling Requried",
                width: 125,
                lookup: {
                    dataSource: yesno,
                    displayExpr: "Name",
                    valueExpr: "Name"
                }
            },
            {
                dataField: "LaborAvailability",
                caption: "Labor Availability",
                width: 125,
                lookup: {
                    dataSource: yesno,
                    displayExpr: "Name",
                    valueExpr: "Name"
                }
            },
            {
                dataField: "CivilMaterial",
                caption: "Civil Material",
                width: 125,
                lookup: {
                    dataSource: yesno,
                    displayExpr: "Name",
                    valueExpr: "Name"
                }
            },
            "ListOfGSM",
            "DistanceFromPoint",
            "Remarks",
            "Accommodation",
            "Transportation",
            "Lodging",
            "ATMS",
            "SiteDistance",
            "LocalLanguage",
            "CustomerLanguage",
            {
                dataField: "Photo1",
                visible: false
            },
            {
                dataField: "Photo2",
                visible: false
            },
            {
                dataField: "Photo3",
                visible: false
            },
            {
                dataField: "Photo4",
                visible: false
            },
            {
                dataField: "Photo5",
                visible: false
            },
            "SiteSurveyName",
            "SiteIncharge",
            "SiteCareTaker"]
    });
});