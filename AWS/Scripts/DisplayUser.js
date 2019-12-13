$(function () {
    function logEvent(eventName) {
        var logList = $("#events ul"),
            newItem = $("<li>", { text: eventName });

        logList.prepend(newItem);
    }
    $("#gridContainer").dxDataGrid({
        dataSource: Userd,
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
                dataField: "FirstName",
                caption: "First Name",
                validationRules: [{ type: "required" }],
                width: 150

            },
            {
                dataField: "LastName",
                caption: "Last Name",
                validationRules: [{ type: "required" }],
                width: 150

            },
            {
                dataField: "EmailID",
                caption: "Email ID",
                validationRules: [{ type: "required" }, { type: "email" }],
                width: 150

            },
            {
                dataField: "PhoneNumber",
                caption: "Phone N0.",
                validationRules: [{ type: "required" }],
                width: 150

            },
            {
                dataField: "Address",
                caption: "Address",
                validationRules: [{ type: "required" }],
                width: 150

            },
            {
                dataField: "Username",
                caption: "Username",
                validationRules: [{ type: "required" }],
                width: 150

            },
            {
                dataField: "Password",
                caption: "Password",
                validationRules: [{ type: "required" }],
                width: 150

            },
            {
                dataField: "RoleID",
                caption: "Role Name",
                width: 125,
                lookup: {
                    dataSource: rdata,
                    displayExpr: "RoleName",
                    valueExpr: "ID"
                }
            },
            {
                dataField: "IsActive",
                caption: "IsActive",
                width: 150

            },
            {
                dataField: "IsAdmin",
                caption: "IsAdmin",
                width: 150

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
                url: "/Admin/DisplayUsers/Insert",
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
            logEvent("RowInserted");
        },
        onRowUpdating: function (e) {
            var newData = JSON.stringify(e.newData);
            var olddata = JSON.stringify(e.oldData);
            $.ajax({
                url: "/Admin/DisplayUsers/Update",
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