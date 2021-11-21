var app = angular.module("CustomerApp", ['angularUtils.directives.dirPagination']);
app.controller("CustomerCtrl", function ($scope, $http) {
    debugger;
    var url = "https://bambai.online";
/*    var url = "https://localhost:44393";*/
    $scope.clear = function () {
        $scope.FullName = "";
        $scope.EmailAddress = "";
        $scope.Password = "";
        $scope.PhoneNumber = " ";
        $scope.Gender = "";
        $scope.BirthDay = "";
        $scope.ResetCode = "";
        document.getElementById("btnSave").setAttribute("value", "Submit");
        document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
        document.getElementById("spn").innerHTML = "Thêm mới khách hàng";
    }
    $scope.CheckOut = function (Customer) {
        if (confirm("Bạn muốn check out cho khách hàng" + Customer.FullName +" ?? ")) {
            $http({
                method: "post",
                url: url+"/Admin/Customer/addOrder",
                params: {
                    cusid: Customer.CustomerID
                },
            }).then(function (response) {
                swal("Thông báo", response.data);
                $scope.GetAllData();
            })
        }
    };
    $scope.InsertData = function () {
        if ($scope.myForm.$valid) {
            var Action = document.getElementById("btnSave").getAttribute("value");
            if (Action == "Submit") {
                $scope.Customer = {};
                $scope.Customer.FullName = $scope.FullName;
                $scope.Customer.EmailAddress = $scope.EmailAddress;
                $scope.Customer.Password = $scope.Password;
                $scope.Customer.PhoneNumber = $scope.PhoneNumber;
                $scope.Customer.Gender = $scope.Gender;
                $scope.Customer.BirthDay = $scope.BirthDay;
                $scope.Customer.ResetCode = $scope.ResetCode;
                $http({
                    method: "post",
                    url: url+"/Admin/Customer/Insert_Customer",
                    datatype: "json",
                    data: JSON.stringify($scope.Customer)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                    $scope.FullName = "";
                    $scope.EmailAddress = "";
                    $scope.Password = "";
                    $scope.PhoneNumber = " ";
                    $scope.Gender = "";
                    $scope.BirthDay = "";
                    $scope.ResetCode = "";
                })
            } else {
                $scope.Customer = {};
                $scope.Customer.FullName = $scope.FullName;
                $scope.Customer.EmailAddress = $scope.EmailAddress;
                $scope.Customer.Password = $scope.Password;
                $scope.Customer.PhoneNumber = $scope.PhoneNumber;
                $scope.Customer.Gender = $scope.Gender;
                $scope.Customer.BirthDay = $scope.BirthDay;
                $scope.Customer.ResetCode = $scope.ResetCode;
                $scope.Customer.CustomerID = document.getElementById("CusId").value;
                $http({
                    method: "post",
                    url: url+"/Admin/Customer/Update_Customer",
                    datatype: "json",
                    data: JSON.stringify($scope.Customer)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                    $scope.FullName = "";
                    $scope.EmailAddress = "";
                    $scope.Password = "";
                    $scope.PhoneNumber = " ";
                    $scope.Gender = "";
                    $scope.BirthDay = "";
                    $scope.ResetCode = "";
                    document.getElementById("btnSave").setAttribute("value", "Submit");
                    document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
                    document.getElementById("spn").innerHTML = "Thêm mới khách hàng";
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
            url: url+"/Admin/Customer/Get_AllCustomer"
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
            $scope.customers = response.data;
            $("#myModal").modal('hide');
        }, function () {
            alert("Error Occur");
        })
    };
    $scope.DeleteCus = function (Customer) {
        swal({
            title: "Are you sure?",
            text: "Bạn đang xóa khách hàng " + Customer.FullName + " ?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $http({
                    method: "post",
                    url: url+"/Admin/Customer/Delete_Customer",
                    datatype: "json",
                    data: JSON.stringify(Customer)
                }).then(function (response) {
                    swal("Thông báo", response.data);
                    $scope.GetAllData();
                })
            } else {
            }
        });
    };
    $scope.UpdateCus = function (Customer) {
        document.getElementById("CusId").value = Customer.CustomerID;
        $scope.FullName = Customer.FullName;
        $scope.EmailAddress = Customer.EmailAddress;
        $scope.Password = Customer.Password;
        $scope.PhoneNumber = Customer.PhoneNumber;
        $scope.Gender = Customer.Gender;
        $scope.BirthDay = Customer.BirthDay;
        $scope.ResetCode = Customer.ResetCode;
        document.getElementById("btnSave").setAttribute("value", "Update");
        document.getElementById("btnSave").style.backgroundColor = "Yellow";
        document.getElementById("spn").innerHTML = "Chỉnh sửa thông tin khách hàng";
        console.log('ffffffff');
        console.log(product.Status);
        if (Customer.Gender == true) {
            document.getElementById("statusget").innerHTML = "Nữ";
        }
        else {
            document.getElementById("statusget").innerHTML = "Nam";
        }


    };
});