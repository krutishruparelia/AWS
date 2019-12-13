$(function () {
    $("#statinonid").dxTextBox({
       
        name: "stationid"
    });

    $("#staionname").dxTextBox({
        name: "stationname"
    });
    $("#latitude").dxTextBox({

        name: "latitude"
    });
    $("#longnitude").dxTextBox({
        name: "longnitude"
    });
    $("#city").dxTextBox({
        name: "city"
    });
    $("#district").dxTextBox({
        name: "LastName"
    });
    $("#state").dxTextBox({
        name: "state"
    });
    $("#tahesil").dxTextBox({
        name: "tahesil"
    });
    $("#Block").dxTextBox({
        name: "Block"
    });
    $("#village").dxTextBox({
        name: "village"
    });
    $("#image").dxFileUploader({
        selectButtonText: "Select photo",
        labelText: "",
        accept: "image/*",
        name : "photo",
        uploadMode: "UseForm"
    });

    $("#address").dxTextBox({
        name: "address"
    });
    $("#bank").dxTextBox({
        name: "bank"
    });
    $("#busstand").dxTextBox({
        name: "busstand"
    });
    $("#airport").dxTextBox({
        name: "airport"
    });
    $("#otherinfo").dxTextBox({
        name: "otherinfo"
    });
    $("#profile").dxSelectBox({
        dataSource:statonprofile,
        valueExpr: "value",
        displayExpr: "name",
        value: "*",
        onValueChanged: function (e) {

            $.ajax({
                url: "/Admin/Station/getDrodownvalue",
                dataType: "json",
                data: { "value": e.value },
                success: function (result) {
                    //$("#partialview").append("@Html.Action('GridViewPartial')");  
                    $("#partialview").load('Station/GridViewPartial/');
                }
            });
        }
    });

    $("#button").dxButton({
        text: "Create Station",
        type: "success",
        onClick: function () {
            //DevExpress.ui.dialog.alert("Uncomment the line to enable sending a form to the server.", "Click Handler");
            $("#form").submit();
        }
    });

   
});