
function logEvent(eventName) {
    var logList = $("#events ul"),
        newItem = $("<li>", { text: eventName });

    logList.prepend(newItem);
}
debugger;
$("#sTgridContainer").dxDataGrid({
 
        dataSource: staionparam,
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
           
            allowUpdating: true,
            
            editEnabled: true,
            searchPanel: true,
            mode: "batch"
        },
        columns: [
            {
                dataField: "SensorName",
                caption: "Sensor Name",
                validationRules: [{ type: "required" }],
                width: 200,
            },
            {
                dataField: "Gain",
                caption: "Gain",
                //validationRules: [{ type: "required" }],
                width: 200,
            },
            {
                dataField: "Offset",
                caption: "Offset",
                //validationRules: [{ type: "required" }],
                width: 200
            },
            {
                dataField: "SerialNumber",
                caption: "Serial Number",
                //validationRules: [{ type: "required" }],
                width: 200
            }
        ],
        onEditorPreparing(e) {
            e.editorOptions.disabled = e.parentType == "dataRow" && e.dataField == "SensorName" && !e.row.inserted;
        },
        onEditingStart: function (e) {
            logEvent("EditingStart");
        },
        onInitNewRow: function (e) {
            logEvent("InitNewRow");
        },
        onRowUpdating: function (e) {
            debugger;
            var newData = JSON.stringify(e.newData);
            var olddata = JSON.stringify(e.oldData);
            $.ajax({
                url: "/Admin/Station/Update",
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
            swal({
                title: "Role!",
                text: "Update successfully!",
                icon: "success"
            });
            logEvent("RowUpdated");
        },
        onRowRemoving: function (e) {
            $.ajax({
                url: "/Admin/Station/Remove",
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