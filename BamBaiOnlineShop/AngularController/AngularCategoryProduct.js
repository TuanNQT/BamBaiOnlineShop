var app = angular.module("CategoryApp", ['ngFileUpload', 'angularUtils.directives.dirPagination']);
app.controller("CategoryCtrl", function ($scope, $http) {
    debugger;
    var url = "https://bambai.online";
/*    var url = "https://localhost:44393";*/
    $scope.clear = function () {
        $scope.CategoryName = "";
        document.getElementById("btnSave").setAttribute("value", "Submit");
        document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
        document.getElementById("spn").innerHTML = "Thêm mới danh mục sản phẩm";
    }
    $scope.InsertData = function () {
        if ($scope.myForm.$valid) {
            var Action = document.getElementById("btnSave").getAttribute("value");
            if (Action == "Submit") {
                $scope.Category = {};
                $scope.Category.CategoryName = $scope.CategoryName;
                $http({
                    method: "post",
                    url: url + "/Admin/CategoryProduct/Insert",
                    datatype: "json",
                    data: JSON.stringify($scope.Category)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                    $scope.Category.CategoryName = "";
                })
            } else {
                $scope.Category = {};
                $scope.Category.CategoryName = $scope.CategoryName;
                $scope.Category.CategoryID = document.getElementById("CaID").value;
                $http({
                    method: "post",
                    url: url + "/Admin/CategoryProduct/Update",
                    datatype: "json",
                    data: JSON.stringify($scope.Category)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                    $scope.CategoryName = "";
                    document.getElementById("btnSave").setAttribute("value", "Submit");
                    document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
                    document.getElementById("spn").innerHTML = "Thêm mới danh mục sản phẩm";
                })
            }
        } else {

        }
    }
    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: url + '/Admin/CategoryProduct/Get_AllCategory'
        }).then(function (response) {
            $scope.Categorys = response.data;
            $("#myModal").modal('hide');

        }, function () {
            alert("Error", "Lỗi tải dữ liệu", "error");
        })
    };
    $scope.Delete = function (Category) {
        swal({
            title: "Are you sure?",
            text: "Bạn đang xóa danh mục " + Category.CategoryName + " ?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $http({
                        method: "post",
                        url: url + "/Admin/CategoryProduct/Delete",
                        datatype: "json",
                        data: JSON.stringify(Category)
                    }).then(function (response) {
                        swal("Thông báo", response.data);
                        $scope.GetAllData();
                    })
                } else {

                }
            });

    };
    $scope.Update = function (Category) {
        document.getElementById("CaID").value = Category.CategoryID;
        $scope.CategoryName = Category.CategoryName;
        document.getElementById("btnSave").setAttribute("value", "Update");
        document.getElementById("btnSave").style.backgroundColor = "Yellow";
        document.getElementById("spn").innerHTML = "Chỉnh sửa danh mục sản phẩm";
        console.log('ffffffff');
    };
});