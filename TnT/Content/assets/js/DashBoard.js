jQuery(document).ready(function ($) {
    
    var exist = false;
  
    
    strSel = "<option value=''> " + ResourcesKey.SelectYr + "</option>";
    //strSel = "<option value=''> "+ SelectYr +"</option>";
    //strSel = "<option value=''/>".html(SelectYr);
  
    var thisYear = (new Date()).getFullYear();
    var thisMonth = (new Date()).getMonth() + 1;
    var dateDIff = (new Date()).getFullYear() - 2017;
    for (var i = 0; i <= dateDIff; i++) {
        var year = thisYear - i;
        strSel += "<option value='" + year + "'>" + year + "</option>";
    }

    $("#year").html(strSel);
    $("#year").val(thisYear);
    if (thisMonth < 10) {
        $("#month").val("0" + thisMonth);
    }
    else {
        $("#month").val(thisMonth);
    }

    var month = $("#month").val();
    var year = $("#year").val();
    if (month == "") {
        month = (new Date()).getMonth() + 1;
    }

    if (year == null) {
        year = (new Date()).getFullYear();
    }
    getdata(month, year);
});

$("#month").change(function () {
    debugger;
    var month = $("#month").val();
    var year = $("#year").val();
    if (month == "") {
        month = (new Date()).getMonth()+1;
    }

    if (year == "") {
        year = (new Date()).getFullYear();
    }
  
    getdata(month, year);
})
$("#year").change(function () {
    debugger;
    var month = $("#month").val();
    var year = $("#year").val();
    if (month == "") {
        month = (new Date()).getMonth()+1;
    }

    if (year == "") {
        year = (new Date()).getFullYear();
    }

    getdata(month, year);
})

function emptyChanrt()
{
    $("#chart6").empty();
    $("#chart3").empty();
    $("#chart4").empty();
    $("#chart8").empty();
}
function getdata(month, year) {
    
    emptyChanrt();

    $.ajax({
        type: 'POST',
        url: 'getDashboardDetailsPichart',
        dataType: 'json',
        data: { "month": month, "year": year },
        aysync: true,
        success: function (data) {
          
            var CloseBatch1 = ResourcesKey.CloseBatch;
            var strBatchStatus = data[0][0];
          Morris.Donut({
                element: 'chart6',
                data: [{ label: strBatchStatus[0].x, value: strBatchStatus[0].y },
                      { label: strBatchStatus[1].x, value: strBatchStatus[1].y },
                      { label: strBatchStatus[2].x, value: strBatchStatus[2].y }],
                labelColor: '#303641',
                colors: ['#00a651', '#f26c4f', '#00bff3', '#0072bc']
            });

            var struser = data[1][0];
            var mychartBar = Morris.Bar({
                element: 'chart3',
                axes: true,
                data: struser,
                xkey: 'x',
                ykeys: ['y'],
                labels: [ResourcesKey.Batch],
                labelDisplay: "wrap",
                xLabelAngle: 80,
                resize: true,
                barColors: ['rgba(124, 181, 236, 0.75)']
            });

            var strbatch = data[2][0];
            var mychartBar2 = Morris.Bar({
                element: 'chart4',
                axes: true,
                data: strbatch,
                xkey: 'month',
                ykeys: ['batch', 'prod'],
                labels: [ResourcesKey.Batch, ResourcesKey.product],
                xLabelAngle: 60,
                resize: true,
                barColors: ['rgba(124, 181, 236, 0.75)', 'rgba(144, 237, 125, 0.75)']
            });

            var strlinewise = data[3][0];


            var mychartArea = Morris.Area({
                element: 'chart8',
                height: '400px',
                data: strlinewise,
                xkey: 'line',
                ykeys: ['batchCount'],
                labels: [ResourcesKey.BatchCnt],
                xLabelAngle: 70,
                resize: true,
                parseTime: false,
                lineColors: ['rgba(67, 67, 72, 0.75)']
            });
        }
    });
}
