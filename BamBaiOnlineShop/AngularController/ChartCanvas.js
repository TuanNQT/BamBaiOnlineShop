$(document).ready(function () {
    var url = "https://bambai.online";
/*    var url = "https://localhost:44393";*/
    $.get(url+"/Admin/Default/ThongKeByMonth", addDatabymonth);
    $.get(url+"/Admin/Default/GetTop5Product", addDatatop5sp);
    $.get(url+"/Admin/Default/ThongKeByCustomer", addDatabyCustomer);
    $.get(url+"/Admin/Default/ThongKeByCategories", addDatabyCategories);

    var dataPointsbymonth = [];
    var dataPointstop5sp = [];
    var dataPointsbyCustomer = [];
    var dataPointsbyCategories = [];
    var chartbyMonth = new CanvasJS.Chart("chartByMonth", {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: "Doanh thu theo tháng"
        },
        axisY: {
            title: "Doanh thu",
            titleFontSize: 24
        },
        axisX: {
            title: "Tháng",
            titleFontSize: 20,
            intervalType: "month",
            interval: 1,
            valueFormatString: "M",
        },
        data: [{
            type: "line",
            yValueFormatString: "#,###",
            dataPoints: dataPointsbymonth
        }]
    });

    function addDatabymonth(data) {
        console.log(data);
        for (var i = 0; i < data.length; i++) {
            dataPointsbymonth.push({
                x: new Date(data[i].Thang),
                y: data[i].Doanh_thu
            });
        }
        chartbyMonth.render();

    }
    var charttopsp = new CanvasJS.Chart("chartTop5sp", {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: "Top 5 sản phẩm hot"
        },
        axisY: {
            title: "Doanh thu",
            titleFontSize: 24,
            includeZero: true
        },
        axisX: {
            title: "Tên sản phẩm",
            titleFontSize: 18,
        },
        data: [{
            type: "column",
            yValueFormatString: "#,###",
            dataPoints:dataPointstop5sp
        }]
    });

    function addDatatop5sp(data) {
        console.log(data);
        for (var i = 0; i < data.length; i++) {
            dataPointstop5sp.push({
                label: data[i].ModelName,
                y: data[i].Doanh_Thu
            });
        }
        charttopsp.render();
    }
   
    var chartbyCustomer = new CanvasJS.Chart("chartbyCustomer", {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: "Khách hàng tiềm năng"
        },
        axisY: {
            title: "Doanh thu",
            titleFontSize: 24,
            includeZero: true
        },
        axisX: {
            title: "Tên khách hàng",
            titleFontSize: 18,
        },
        data: [{
            type: "bar",
            yValueFormatString: "#,###",
            dataPoints: dataPointsbyCustomer
        }]
    });

    function addDatabyCustomer(data) {
        console.log(data);
        for (var i = 0; i < data.length; i++) {
            dataPointsbyCustomer.push({
                label: data[i].Label,
                y: data[i].Y
            });
        }
        chartbyCustomer.render();
    }

    var chartbyCategories = new CanvasJS.Chart("chartbyCategories", {
        animationEnabled: true,
        title: {
            fontFamily: "tahoma",
            text: "Xu thế tiêu dùng "
        },
        data: [{
            type: "pie",
            startAngle: 240,
            indexLabel: "{label} {y}",
            dataPoints: dataPointsbyCategories
        }]
    });

    function addDatabyCategories(data) {
        console.log(data);
        for (var i = 0; i < data.length; i++) {
            dataPointsbyCategories.push({
                label: data[i].Label,
                y: data[i].Y
            });
        }
        chartbyCategories.render();
    }
});