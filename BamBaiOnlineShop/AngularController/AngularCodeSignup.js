$(document).ready(function () {
    var app = angular.module("SignupApp", []);
    app.controller("SignupCtrl", function ($scope, $http) {
        debugger;
        var url = "https://bambai.online";
/*    var url = "https://localhost:44393";*/
        $scope.Signup = function () {
            if ($scope.reg.$valid) {
                if ($scope.matkhau != $scope.matkhauxn) {
                    alert("Mật khẩu xác nhận không khớp");
                } else {
                    $http({
                        method: "post",
                        url: url+"/Admin/LoginSigninAdmin/Reg",
                        params: {
                            taikhoan: $scope.taikhoan,
                            matkhau: $scope.matkhau
                        },
                    }).then(function (response) {
                        $scope.taikhoan = "";
                        $scope.matkhau = "";
                        console.log(response.data);
                        if (response.data === "Đăng ký thành công") {
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
                }
            } else {

            }
        }
    });
});

