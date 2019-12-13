$(function () {
    function logEvent(eventName) {
        var logList = $("#events ul"),
            newItem = $("<li>", { text: eventName });

        logList.prepend(newItem);
    }
    $("#gridContainer").dxDataGrid({
        dataSource: Sensord,
        keyExpr: "ID",
        showBorders: true,
        paging: {
            pageSize: 10
        },
        headerFilter: {
            visible: true,
            allowSearch: true
        },


        searchPanel: { visible: true },
        editing: {
            mode: "popup",
            allowUpdating: true,
            allowDeleting: true,
            allowAdding: true,
            editEnabled: true,
            searchPanel: true
        },
        columns: [
            {
                dataField: "Name",
                caption: "Name",
                validationRules: [{ type: "required" }],
                width: 200,
            },
            {
                dataField: "Type",
                caption: "Type",
                validationRules: [{ type: "required" }],
                width: 200,
            },
            {
                dataField: "Make",
                caption: "Make",
                validationRules: [{ type: "required" }],
                width: 200,
            },
            {
                dataField: "Model",
                caption: "Model",
                validationRules: [{ type: "required" }],
                width: 200,
            }
        ],

        onEditingStart: function (e) {
            logEvent("EditingStart");
        },
        onInitNewRow: function (e) {
            logEvent("InitNewRow");
        },
        onRowInserting: function (e) {
            var newData = JSON.stringify(e.data);
            $.ajax({
                url: "/Admin/Sensor/Insert",
                dataType: "json",
                data: { "value": newData },
                success: function (result) {
                    deferred.resolve(result.data, {
                        totalCount: result.totalCount,
                        summary: result.summary,
                        groupCount: result.groupCount
                    });
                }
            });
            logEvent("RowInserting");
        },
        onRowInserted: function (e) {

            DevExpress.ui.dialog.alert("Inserted successfully", "Parameter")
            logEvent("RowInserted");
        },
        onRowUpdating: function (e) {
            var newData = JSON.stringify(e.newData);
            var olddata = JSON.stringify(e.oldData);
            $.ajax({
                url: "/Admin/Sensor/Update",
                dataType: "json",
                data: { "newdata": newData, "olddata": olddata },
                success: function (result) {
                    deferred.resolve(result.data, {
                        totalCount: result.totalCount,
                        summary: result.summary,
                        groupCount: result.groupCount
                    });
                }
            });

            logEvent("RowUpdating");
        },
        onRowUpdated: function (e) {
            DevExpress.ui.dialog.alert("Updated successfully", "Parameter")
            logEvent("RowUpdated");
        },
        onRowRemoving: function (e) {
            $.ajax({
                url: "/Admin/Sensor/Remove",
                dataType: "json",
                data: { "ID": e.data.ID },
                success: function (result) {
                    deferred.resolve(result.data, {
                        totalCount: result.totalCount,
                        summary: result.summary,
                        groupCount: result.groupCount
                    });
                }
            });
            logEvent("RowRemoving");
        },
        onRowRemoved: function (e) {
            logEvent("RowRemoved");
        },
        onContentReady: function (e) {
            var saveButton = $(".dx-button[aria-label='Save']");
            if (saveButton.length > 0)
                saveButton.click(function (event) {
                    if (!isUpdateCanceled) {
                        DoSomething(e.component);
                        event.stopPropagation();
                    }
                });
        }
    });
    $("#clear").dxButton({
        text: "Clear",
        onClick: function () {
            $("#events ul").empty();
        }
    });
});