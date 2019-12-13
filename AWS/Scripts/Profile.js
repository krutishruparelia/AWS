$(function () {
    //$("#pName").dxTextBox({
    //    placeholder: "Parameters",
    //    Name: "ProfileName",
    //    ID:"profileName"
    //});
    //$("#deli").dxTextBox({
        
    //    placeholder: "Delimiter Eg: ,#"
    //});
    //$("#dateformat").dxSelectBox({
    //    items: [
    //        "dd/MM/yyy",
    //        "mm/dd/yyy",
    //        "yyy/mm/dd"
    //    ],
    //    placeholder: "Select Date Format",
    //    showClearButton: true
    //});
    $("#normal-contained").dxButton({
        stylingMode: "contained",
        text: "Submit",
        type: "success",
        width: 120,
        onClick: function () {
            //DevExpress.ui.notify("The Contained button was clicked");
            var profileName = $("#pName").val();
            var deli = $("#deli").val();
            var dateformat = $("#dateformat option:selected").text();
          //  var url = '@Url.Action("insertProfileName", "Profile",new {area="" })';
            $.ajax({
                cache: false,
                type: "POST",
                url: "Profile/insertProfileName",
                data: { "profilename": profileName, "deli": deli, "dateformat": dateformat },
                success: function (Json) {
                    //alert(Json);
                    //swal("Profile Added!", "Profile Successfuly Configured", "success");
                    DevExpress.ui.notify("Profile Added!", "Profile Successfuly Configured", "success");
                    //window.location = "Index";
                }
            });
        }
    });
    function logEvent(eventName) {
        var logList = $("#events ul"),
            newItem = $("<li>", { text: eventName });
        logList.prepend(newItem);
    }
    $("#gridContainer").dxDataGrid({
        dataSource: sensord,
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
            //allowUpdating: true,
            //allowDeleting: true,
            allowAdding: true,
            editEnabled: true,
            searchPanel: true,
            allowDeleting: true,
            
            selectTextOnEditStart: true,
            startEditAction: "click"
        },
        columns: [
            {
                dataField: "Name",
                caption: "Sensor Name",
                validationRules: [{ type: "required" }],
                width: 200,
                lookup: {
                    dataSource: sensordLookup,
                    displayExpr: "Name",
                    valueExpr: "Name"
                }
            },
            {
                dataField: "Type",
                caption: "Type",
                //validationRules: [{ type: "required" }],
                width: 200,
                lookup: {
                    dataSource: sensordLookup,
                    displayExpr: "Type",
                    valueExpr: "Type"
                }
            },
            {
                dataField: "Make",
                caption: "Make",
                //validationRules: [{ type: "required" }],
                width: 200,
                lookup: {
                    dataSource: sensordLookup,
                    displayExpr: "Make",
                    valueExpr: "Make"
                }
            },
            {
                dataField: "Model",
                caption: "Model",
                //validationRules: [{ type: "required" }],
                width: 200,
                lookup: {
                    dataSource: sensordLookup,
                    displayExpr: "Model",
                    valueExpr: "Model"
                }
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
                url: "/Admin/Profile/InsertTemp",
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
            //$.ajax({
            //    url: "/Admin/Profile/Insert",
            //    dataType: "json",
            //    data: { "value": "value" },
            //    success: function (result) {
            //        deferred.resolve(result.data, {
            //            totalCount: result.totalCount,
            //            summary: result.summary,
            //            groupCount: result.groupCount
            //        });
            //    }
            //});  
            swal({
            title: "Role!",
                text: "Inserted successfully!",
                    icon: "success"
        });
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
                url: "/Admin/Profile/Remove",
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
                    if(!isUpdateCanceled) {
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