var app = angular.module("OrderApp", ['ngFileUpload', 'angularUtils.directives.dirPagination']);
app.controller("OrderCtrl", function ($scope, $http) {
    debugger;
    var url = "https://bambai.online";
/*    var url = "https://localhost:44393";*/
    $scope.beforeaction = function (Order) {
        document.getElementById("OrId").value = Order.OrderID;
    }
    $scope.checkOrder = function () {
            $http({
                method: "post",
                url: url+"/Admin/Order/checkOrder",
                params: {
                    Orid: document.getElementById("OrId").value
                },
            }).then(function (response) {
                swal("Thông báo", response.data);
                $scope.GetAllData();
                $("#myModal").modal('hide');
            })
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
            url: url+'/Admin/Order/Get_AllOrder'
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
            $scope.orders = response.data;
            $("#myModal").modal('hide');

        }, function () {
            alert("Error Occur");
        })
    };
    $scope.GetOrderDetail = function () {
        $http({
            method: "get",
            url: url+'/Admin/Order/Get_OrderDetailById',
            params: {
                Orid: document.getElementById("OrId").value
            },
        }).then(function (response) {
            $scope.orderdetails = response.data;
        }, function () {
            alert("Error Occur");
        });
        $scope.getTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.orderdetails.length; i++) {
                var orderdetail = $scope.orderdetails[i];
                total += orderdetail.thanhtien;
            }
            return total;
        }
    };
});