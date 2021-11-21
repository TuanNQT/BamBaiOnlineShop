var app = angular.module("AdminAccountApp", ['ngFileUpload', 'angularUtils.directives.dirPagination']);
app.controller("AdminAccountCtrl", function ($scope, $http) {
    debugger;
    var url = "https://bambai.online";
/*    var url = "https://localhost:44393";*/
    $scope.clear = function () {
        $scope.TaiKhoan = "";
        $scope.MatKhau = "";
        document.getElementById("btnSave").setAttribute("value", "Submit");
        document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
        document.getElementById("spn").innerHTML = "Add New AdminAccount";
    }
    $scope.InsertData = function () {
        if ($scope.myForm.$valid) {
            var Action = document.getElementById("btnSave").getAttribute("value");
            if (Action == "Submit") {
                $scope.AdminAccount = {};
                $scope.AdminAccount.TaiKhoan = $scope.TaiKhoan;
                $scope.AdminAccount.MatKhau = $scope.MatKhau;
                $http({
                    method: "post",
                    url: url + "/Admin/AdminAccount/Insert",
                    datatype: "json",
                    data: JSON.stringify($scope.AdminAccount)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                    $scope.TaiKhoan = "";
                    $scope.MatKhau = "";
                })
            } else {
                $scope.AdminAccount = {};
                $scope.AdminAccount.MatKhau = $scope.MatKhau;
                $scope.AdminAccount.TaiKhoan = document.getElementById("AcID").value;
                $http({
                    method: "post",
                    url: url + "/Admin/AdminAccount/Update",
                    datatype: "json",
                    data: JSON.stringify($scope.AdminAccount)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                    $scope.TaiKhoan = "";
                    $scope.MatKhau = "";
                    document.getElementById("btnSave").setAttribute("value", "Submit");
                    document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
                    document.getElementById("spn").innerHTML = "Add New AdminAccount";
                })
            }
        } else {

        }
    }
    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: url + '/Admin/AdminAccount/Get_AllAdminAccount'
        }).then(function (response) {
            $scope.AdminAccounts = response.data;
            $("#myModal").modal('hide');

        }, function () {
            swal("Cảnh báo", "Lỗi tải dữ liệu", "error");

        })
    };
    $scope.Delete = function (AdminAccount) {
        swal({
            title: "Are you sure?",
            text: "Bạn đang xóa tài khoản " + AdminAccount.TaiKhoan + " ?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $http({
                    method: "post",
                    url: url + "/Admin/AdminAccount/Delete",
                    datatype: "json",
                    data: JSON.stringify(AdminAccount)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                })
            } else {

            }
        });
    };
    $scope.Update = function (AdminAccount) {
        document.getElementById("AcID").value = AdminAccount.TaiKhoan;
        $scope.MatKhau = AdminAccount.MatKhau;
        document.getElementById("btnSave").setAttribute("value", "Update");
        document.getElementById("btnSave").style.backgroundColor = "Yellow";
        document.getElementById("spn").innerHTML = "Update AdminAccount Information";
        console.log('ffffffff');
    };
});