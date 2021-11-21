var app = angular.module("BlogApp", ['ngFileUpload', 'angularUtils.directives.dirPagination']);
app.controller("BlogCtrl", function ($scope, $http, Upload) {
    debugger;
    var url = "https://bambai.online";
/*    var url = "https://localhost:44393";*/
    $scope.clear = function () {
        document.getElementById("ngaydang").required = true;
        document.getElementById("showdp").style.display = "none";
        document.getElementById("categoryName").style.display = "none";
        $scope.Title = "";
        document.getElementById("ngaydang").value = "";
        $scope.datepost = "";
        $scope.content = "";
        $scope.view = "";
        $scope.comment = "";
        $scope.Typeid = "";
        $scope.TypeName = ""
        $scope.image = "";
        $scope.writer = "";
        document.getElementById("btnSave").setAttribute("value", "Submit");
        document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
        document.getElementById("spn").innerHTML = "Thêm mới bài viết";
    }
    $scope.addComment = function () {
        if ($scope.formaddCart.$valid) {
            $http({
                method: "post",
                url:url+ "/Admin/Blog/Insert_Comment",
                params: {
                    ContentComment: $scope.ContentComment,
                    CusID: $scope.CustomerID,
                    Blogid: document.getElementById("PoId").value
                },
            }).then(function (response) {
                swal("Thông báo", response.data);
                $scope.GetAllData();
                $("#myModalAddComment").modal('hide');
                $scope.ContentComment = "";
                $scope.CustomerID = "";
            })
        } else {

        }
    }
    $scope.InsertData = function () {
        if ($scope.myForm.$valid) {
            var Action = document.getElementById("btnSave").getAttribute("value");
            if (Action == "Submit") {
                $scope.Blog = {};
                $scope.Blog.Title = $scope.Title;
                $scope.Blog.datepost = $scope.datepost;
                $scope.Blog.content = $scope.content;
                $scope.Blog.view = $scope.view;
                $scope.Blog.comment = $scope.comment;
                $scope.Blog.Typeid = $scope.Typeid;
                $scope.Blog.image = $scope.image;
                $scope.Blog.writer = $scope.writer;
                $http({
                    method: "post",
                    url:url+ "/Admin/Blog/Insert_Blog",
                    datatype: "json",
                    data: JSON.stringify($scope.Blog)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                    $scope.Blog.Title = "";
                    $scope.Blog.datepost = "";
                    $scope.Blog.content = "";
                    $scope.Blog.view = "";
                    $scope.Blog.comment = "";
                    $scope.Blog.Typeid = "";
                    $scope.Blog.image = "";
                    $scope.Blog.writer = "";
                })
            } else {
                $scope.Blog = {};
                $scope.Blog.Title = $scope.Title;
                $scope.Blog.datepost = $scope.datepost;
                $scope.Blog.content = $scope.content;
                $scope.Blog.view = $scope.view;
                $scope.Blog.comment = $scope.comment;
                $scope.Blog.Typeid = $scope.Typeid;
                $scope.Blog.image = $scope.image;
                $scope.Blog.writer = $scope.writer;
                $scope.Blog.Postid = document.getElementById("PoId").value;
                $http({
                    method: "post",
                    url: url+"/Admin/Blog/Update_Blog",
                    datatype: "json",
                    data: JSON.stringify($scope.Blog)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                    $scope.Title = "";
                    document.getElementById("ngaydang").value = "";
                    $scope.datepost = "";
                    $scope.content = "";
                    $scope.view = "";
                    $scope.comment = "";
                    $scope.Typeid = "";
                    $scope.TypeName=""
                    $scope.image = "";
                    $scope.writer = "";
                    document.getElementById("btnSave").setAttribute("value", "Submit");
                    document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
                    document.getElementById("spn").innerHTML = "Thêm mới bài viết";
                })
            }
        } else {

        }
    }
    function ConvertJsonDateToDate(date) {
        var parsedDate = new Date(parseInt(date.substr(6)));
        var newDate = new Date(parsedDate);
        var month = ('0' + (newDate.getMonth() + 1)).slice(-2);
        var day = ('0' + newDate.getDate()).slice(-2);
        var year = newDate.getFullYear();
        return day + "/" + month + "/" + year;
    }
    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: url+'/Admin/Blog/Get_AllBlog'
        }).then(function (response) {
            for (var i = 0; i < response.data.length; i++) {
                // Loop through each property in the Array.
                for (var property in response.data[i]) {
                    if (response.data[i].hasOwnProperty(property)) {
                        var resx = /Date\(([^)]+)\)/;
                        // Checking Date with regular expresion.
                        if (resx.test(response.data[i][property])) {
                            // Setting Date in dd/MM/yyyy format.
                            response.data[i][property] = ConvertJsonDateToDate(response.data[i][property]);
                        }
                    }
                }
            }
            $scope.blogs = response.data;
            $("#myModal").modal('hide');

        }, function () {
            alert("Error Occur");
        })
    };
    $scope.GetCustomers = function () {
        console.log('Customers get success');
        $http({
            method: "get",
            url: url+'/Admin/Product/Get_Customer'
        }).then(function (response) {
            $scope.data = response.data;
        }, function () {
            alert("Customers get error");
        })
    };
    $scope.GetCategory = function () {
        $http({
            method: "get",
            url: url+'/Admin/Blog/Get_Category'
        }).then(function (response) {
            $scope.data = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
    $scope.Delete = function (blog) {
        swal({
            title: "Are you sure?",
            text: "Bạn đang xóa bài viết " + blog.Title + " ?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $http({
                    method: "post",
                    url: url+"/Admin/Blog/Delete_Blog",
                    datatype: "json",
                    data: JSON.stringify(blog)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                })
            } else {

            }
        });
    };
    $scope.Update = function (blog) {
            function formatDate(date) {
                var d = new Date(date),
                    month = '' + (d.getMonth() + 1),
                    day = '' + d.getDate(),
                    year = d.getFullYear();

                if (month.length < 2)
                    month = '0' + month;
                if (day.length < 2)
                    day = '0' + day;

                return [year, month, day].join('-');
        }
        document.getElementById("ngaydang").required = false;
        document.getElementById("showdp").style.display = "";
        blog.datepost = formatDate(blog.datepost);
        document.getElementById("PoId").value = blog.Postid;
        $scope.Title = blog.Title;
        console.log(blog.datepost);
        /*       document.getElementById("ngaydang").value=blog.datepost;*/
        $scope.datepost = blog.datepost;
        $scope.content = blog.content;
        $scope.view = blog.view;
        $scope.comment = blog.comment;
        $scope.Typeid = blog.Typeid;
        $scope.TypeName = blog.TypeName;
        $scope.image = blog.image;
        $scope.writer = blog.writer;
        document.getElementById("btnSave").setAttribute("value", "Update");
        document.getElementById("btnSave").style.backgroundColor = "Yellow";
        document.getElementById("spn").innerHTML = "Chỉnh sửa bài viết";
        document.getElementById("categoryName").style.display = "block";
    };
    $scope.UpdateBFcomment = function (blog) {
        document.getElementById("PoId").value = blog.Postid;
    };
    $scope.UploadFiles = function (files) {
        console.log('ddddddd')
        $scope.SelectedFiles = files;
        if ($scope.SelectedFiles && $scope.SelectedFiles.length) {
            Upload.upload({
                method: "post",
                url: url+'/Admin/Blog/Upload',
                data: {
                    files: $scope.SelectedFiles
                }
            }).then(function (response) {
                $scope.Result = response.data;
                console.log(response.data);
                $scope.image = response.data;
            }, function (response) {
                if (response.status > 0) {
                    var errorMsg = response.status + ': ' + response.data;
                    alert(errorMsg);
                }
            });
        }
    };
});