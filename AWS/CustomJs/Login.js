//$(document).ready(function () {
//    alert(errorMsg);
//});
$("#login").click(function () {
    var username = $("#exampleInputEmail1").val();
    var password = $("#exampleInputPassword1").val();

    $.ajax({

        cache: false,
        type: "POST",
        url: url,
        data: { "username": username, "password": password},
        success: function (Json) {
            //alert(Json);
            if (Json !== "Invalid") {
                if (user !== "") {
                    window.top.location.href = returnUrl;
                }
                else {
                    window.top.location.href = userreturnUrl;
                }
                //window.location = returnUrl;
            }
            else {
              
                    swal("Sign In Failed!", "Invalid Username or Password!", "error");
            }
        },
        error: function (data) { // error callback 
         
            $('.error-login').on('click', function () {
                swal("Sign In Failed!", "Invalid Username or Password!", "warning");
            });
        }
    });
});