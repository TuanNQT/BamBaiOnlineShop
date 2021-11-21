var app = angular.module("myApp", ['ngFileUpload', 'angularUtils.directives.dirPagination']);
app.controller("myCtrl", function ($scope, $http,Upload) {
    debugger;
    var url = "https://bambai.online";
/*    var url = "https://localhost:44393";*/
    $scope.clear = function () {
        $scope.CategoryID = "";
        $scope.CategoryName = "";
        $scope.ModelNumber = "";
        $scope.ModelName = "";
        $scope.ProductImage = " ";
        $scope.UnitCost = "";
        $scope.Description = "";
        $scope.Status = "";
        document.getElementById("categoryName").style.display = "none";
        document.getElementById("btnSave").setAttribute("value", "Submit");
        document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
        document.getElementById("spn").innerHTML = "Thêm mới sản phẩm";
    }
    $scope.addReview = function () {
        if ($scope.formaddReview.$valid) {
            $http({
                method: "post",
                url: url+"/Admin/Product/Insert_review",
                params: {
                    Email: $scope.Email,
                    Comment: $scope.Comment,
                    Rating: $scope.Rating,
                    PrID: document.getElementById("PrId").value
                },
            }).then(function (response) {
                swal("Thông báo", response.data);
                $scope.GetAllData();
                $("#myModalAddReview").modal('hide');
                $scope.Email = "";
                $scope.Comment = "";
                $scope.Rating = "";
            })
        } else {

        }
    };
    $scope.addCart = function () {
        if ($scope.formaddCart.$valid) {
            $http({
                method: "post",
                url: url+"/Admin/Product/Insert_Cart",
                params: {
                    CustomerID: $scope.CustomerID,
                    Quantity: $scope.Quantity,
                    ProductID: document.getElementById("PrId").value
                },
            }).then(function (response) {
                swal("Thông báo", response.data);
                $scope.GetAllData();
                $("#myModalAddCart").modal('hide');
                $scope.CustomerID = "";
                $scope.Quantity = "";
            })
        } else {

        }
    };
    $scope.InsertData = function () {
        if ($scope.myForm.$valid) {
        var Action = document.getElementById("btnSave").getAttribute("value");
        if (Action == "Submit") {
            $scope.Product = {};
            $scope.Product.CategoryID = $scope.CategoryID;
            $scope.Product.ModelNumber = $scope.ModelNumber;
            $scope.Product.ModelName = $scope.ModelName;
            $scope.Product.ProductImage = $scope.ProductImage;
            $scope.Product.UnitCost = $scope.UnitCost;
            $scope.Product.Description = $scope.Description;
            $scope.Product.Status = $scope.Status;
            $http({
                method: "post",
                url: url+"/Admin/Product/Insert_Product",
                datatype: "json",
                data: JSON.stringify($scope.Product)
            }).then(function (response) {
                swal("Thông báo", response.data);
                $scope.GetAllData();
                $scope.CategoryID = "";
                $scope.ModelNumber = "";
                $scope.ModelName = "";
                $scope.ProductImage = " ";
                $scope.UnitCost = "";
                $scope.Description = "";
                $scope.Status = "";
            })
        } else {
            $scope.Product = {};
            $scope.Product.CategoryID = $scope.CategoryID;
            $scope.Product.ModelNumber = $scope.ModelNumber;
            $scope.Product.ModelName = $scope.ModelName;
            $scope.Product.ProductImage = $scope.ProductImage;
            $scope.Product.UnitCost = $scope.UnitCost;
            $scope.Product.Description = $scope.Description;
            $scope.Product.Status = $scope.Status;
            $scope.Product.ProductID = document.getElementById("PrId").value;
            $http({
                method: "post",
                url: url+"/Admin/Product/Update_Product",
                datatype: "json",
                data: JSON.stringify($scope.Product)
            }).then(function (response) {
                swal("Thông báo", response.data);
                $scope.GetAllData();
                $scope.CategoryID = "";
                $scope.CategoryName = "";
                $scope.ModelNumber = "";
                $scope.ModelName = "";
                $scope.ProductImage = " ";
                $scope.UnitCost = "";
                $scope.Description = "";
                $scope.Status = "";
                document.getElementById("btnSave").setAttribute("value", "Submit");
                document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
                document.getElementById("spn").innerHTML = "Thêm mới sản phẩm";
            })
            }
        } else {
           
        }
    }
    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: url+'/Admin/Product/Get_AllProduct'
        }).then(function (response) {
            $scope.products = response.data;
            document.getElementById("categoryName").style.display = "none";
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
        console.log('aaaaaab');
        $http({
            method: "get",
            url: url+'/Admin/Product/Get_Category'
        }).then(function (response) {
            console.log('avaaaabc');
            $scope.data = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
    $scope.DeleteEmp = function (product) {
        swal({
            title: "Are you sure?",
            text: "Bạn đang xóa vật phẩm " + product.ModelName + " ?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $http({
                    method: "post",
                    url: url+"/Admin/Product/Delete_Product",
                    datatype: "json",
                    data: JSON.stringify(product)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                })
            } else {
            }
        });

    };
    $scope.UpdateEmp = function (product) {
        document.getElementById("PrId").value = product.ProductID;
        $scope.CategoryID = product.CategoryID;
        $scope.CategoryName = product.CategoryName;
        $scope.ModelNumber = product.ModelNumber;
        $scope.ModelName = product.ModelName;
        $scope.ProductImage = product.ProductImage;
        $scope.UnitCost = product.UnitCost;
        $scope.Description = product.Description;
        $scope.Status = product.Status;
        document.getElementById("btnSave").setAttribute("value", "Update");
        document.getElementById("btnSave").style.backgroundColor = "Yellow";
        document.getElementById("spn").innerHTML = "Chỉnh sửa sản phẩm";
        document.getElementById("categoryName").style.display = "block";
        console.log('ffffffff');
        console.log(product.Status);
        if (product.Status==true) {
            document.getElementById("statusget").innerHTML = "Còn hàng";
        }
        else {
            document.getElementById("statusget").innerHTML = "Hết hàng";
        }
    
    
    };
    $scope.UpdateEmpCart = function (product) {
        document.getElementById("PrId").value = product.ProductID;
    };
    $scope.UploadFiles = function (files)  {
        console.log('ddddddd')
        $scope.SelectedFiles = files;
        if ($scope.SelectedFiles && $scope.SelectedFiles.length) {
            Upload.upload({
                method: "post",
                url: url+'/Admin/Product/Upload',
                data: {
                    files: $scope.SelectedFiles
                }
            }).then(function (response) {
                $scope.Result = response.data;
                console.log(response.data);
                $scope.ProductImage = response.data;
            }, function (response) {
                if (response.status > 0) {
                    var errorMsg = response.status + ': ' + response.data;
                    alert(errorMsg);
                }
            });
        }
    };
});