$(document).ready(function () {
    var app = angular.module("LoginApp", []);
    app.controller("LoginCtrl", function ($scope, $http) {
        debugger;
        var url = "https://bambai.online";
/*    var url = "https://localhost:44393";*/
        $scope.login = function () {
            if ($scope.formlogin.$valid) {
                onLoading(true)
                $http({
                    method: "post",
                    url: url+"/Admin/LoginSigninAdmin/Login",
                    params: {
                        taikhoan: $scope.taikhoan,
                        matkhau: $scope.matkhau
                    },
                }).then(function (response) {
                    onLoading(false)
                    $scope.taikhoan = "";
                    $scope.matkhau = "";
                    if (response.data === "Đăng nhập thành công") {
                        swal({
                            title: "Thanks You!",
                            text: response.data,
                            icon: "success",
                            button: "OK",
                            timer: 2500,
                        }).then(function () {
                            window.location = url+"/Admin/Default/Index";
                        });
                    } else {
                        swal({
                            title: "Error!",
                            text: response.data,
                            icon: "error",
                            button: "OK",
                            timer: 2000,
                        });
                    }
                })
            } else {

            }
        }
        $scope.Signin = function () {

        }
    });
    function onLoading(isStatus) {
        // Disable the button and hide the other image here
        // or you can hide the whole div like below
        swal({
            title: "Vui lòng chờ đợi",
            text: "Waiting......",
            icon: "/Content/images/loading.gif",
            buttons: false,
            closeOnClickOutside: false,
            onclose: isStatus
        });
    }
});