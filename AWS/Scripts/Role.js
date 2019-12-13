$(function () {
    function logEvent(eventName) {
        var logList = $("#events ul"),
            newItem = $("<li>", { text: eventName });

        logList.prepend(newItem);
    }
    $("#gridContainer").dxDataGrid({
        dataSource: Roled,
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
                dataField: "RoleName",
                caption: "Role Name",
                validationRules: [{ type: "required" }],
                width: 200,
               
            },
            {
                dataField: "Code",
                caption: "Code",
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
                url: "/Admin/Role/Insert",
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
          
            swal({
                title: "Role!",
                text: "Added successfully!",
                icon: "success"
            });
            $(".swal2-modal").css('background-color', '#FFF');
            logEvent("RowInserted");
        },
        onRowUpdating: function (e) {
            var newData = JSON.stringify(e.newData);
            var olddata = JSON.stringify(e.oldData);
            $.ajax({
                url: "/Admin/Role/Update",
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
                url: "/Admin/Role/Remove",
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