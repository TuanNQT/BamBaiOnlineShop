var app = angular.module("TypeBlogApp", ['ngFileUpload', 'angularUtils.directives.dirPagination']);
app.controller("TypeBlogCtrl", function ($scope, $http) {
    debugger;
    var url = "https://bambai.online";
/*    var url = "https://localhost:44393";*/
    $scope.clear = function () {
        $scope.TypeName = "";
        document.getElementById("btnSave").setAttribute("value", "Submit");
        document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
        document.getElementById("spn").innerHTML = "Thêm mới danh mục bài viết";
    }
    $scope.InsertData = function () {
        if ($scope.myForm.$valid) {
            var Action = document.getElementById("btnSave").getAttribute("value");
            if (Action == "Submit") {
                $scope.TypeBlog = {};
                $scope.TypeBlog.TypeName = $scope.TypeName;
                $http({
                    method: "post",
                    url: url+"/Admin/BlogType/Insert",
                    datatype: "json",
                    data: JSON.stringify($scope.TypeBlog)
                }).then(function (response) {
                   swal("Thông báo", response.data);
                    $scope.GetAllData();
                    $scope.TypeBlog.TypeName = "";
                })
            } else {
                $scope.TypeBlog = {};
                $scope.TypeBlog.TypeName = $scope.TypeName;
                $scope.TypeBlog.Typeid = document.getElementById("TyID").value;
                $http({
                    method: "post",
                    url: url+"/Admin/BlogType/Update",
                    datatype: "json",
                    data: JSON.stringify($scope.TypeBlog)
                }).then(function (response) {
                   swal("Thông báo", response.data);
                    $scope.GetAllData();
                    $scope.TypeName = "";
                    document.getElementById("btnSave").setAttribute("value", "Submit");
                    document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
                    document.getElementById("spn").innerHTML = "Thêm mới danh mục bài viết";
                })
            }
        } else {

        }
    }
    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: url+'/Admin/BlogType/Get_AllTypeBlog'
        }).then(function (response) {
            $scope.TypeBlogs = response.data;
            $("#myModal").modal('hide');

        }, function () {
            alert("Error Occur");
        })
    };
    $scope.Delete = function (TypeBlog) {
        swal({
            title: "Are you sure?",
            text: "Bạn đang xóa danh mục bài viết " + TypeBlog.TypeName + " ?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $http({
                    method: "post",
                    url: url+"/Admin/BlogType/Delete",
                    datatype: "json",
                    data: JSON.stringify(TypeBlog)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                })
            } else {
            }
        });
    };
    $scope.Update = function (TypeBlog) {
        document.getElementById("TyID").value = TypeBlog.Typeid;
        $scope.TypeName = TypeBlog.TypeName;
        document.getElementById("btnSave").setAttribute("value", "Update");
        document.getElementById("btnSave").style.backgroundColor = "Yellow";
        document.getElementById("spn").innerHTML = "Chỉnh sửa danh mục bài viết";
        console.log('ffffffff');
    };
});