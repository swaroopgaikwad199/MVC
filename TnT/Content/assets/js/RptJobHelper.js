

function ResetDivisionCode() {
    $("#DivisionCode").html("");
    var optionhtml1 = '<option value="' + 0 + '">' + $("#DivCode").val() + '</option>';
    $("#DivisionCode").append(optionhtml1);
}
function ResetPlantCode() {
    $("#PlantCode").html("");
    var optionhtml2 = '<option value="' + 0 + '">' + $("#PCode").val() + '</option>';
    $("#PlantCode").append(optionhtml2);
}
function ResetLineCode() {
    $("#LineCode").html("");
    var optionhtml3 = '<option value="' + 0 + '">' + $("#LCode").val() + '</option>';
    $("#LineCode").append(optionhtml3);
}
function ResetProductName() {
    $("#ProductName").html("");
    var optionhtml4 = '<option value="' + 0 + '">' + $("#PName").val() + '</option>';
    $("#ProductName").append(optionhtml4);
}
function ResetJobStatus() {
    $("#JobStatus").html("");
    var optionhtml7 = '<option value="' + 0 + '">' + $("#BStatus").val() + '</option>';
    $("#JobStatus").append(optionhtml7);
}
function ResetIdBatchNo() {
    $("#IdBatchNo").html("");
    var optionhtml5 = '<option value="' + 0 + '">' + $("#BNumber").val() + '</option>';
    $("#IdBatchNo").append(optionhtml5);
}
function ResetIdJobName() {
    $("#IdJobName").html("");
    var optionhtml6 = '<option value="' + 0 + '">' + $("#BName").val() + '</option>';
    $("#IdJobName").append(optionhtml6);
}

//Binding DropDown Functions
function ResetBindings() {

    $('#LocationCode').val(0);

    ResetDivisionCode();
    ResetPlantCode();
    ResetLineCode();
    ResetProductName();
    ResetJobStatus();
    ResetIdBatchNo();
    ResetIdJobName();

    $("#FilteredJob").html("");
}

function ResetPieChart() {

    $("#dataProductName").html('-');
    $("#dataGTIN").html('-');
    $("#dataGoodCount").html('-');
    $("#dataTotal").html('-');
    $("#dataDecomm").html('-');
    $("#dataEXP").html('-');
    $("#dataBQty").html('-');
    $("#dataBadCount").html('-');
    $("#dataMFG").html('-');
    $('.chart').hide();
    $('#chart3').hide();
}

$('#LocationCode').on('change', function () {
    $("#JidVAL").val("");
    ResetPieChart();
    var selIndex = this.selectedIndex;

    if (selIndex !== 0) {
        $(".loading").show();
        var LocationCode = this.value;

        $.ajax({
            url: 'getDivisionCodes',
            type: "POST",
            dataType: 'json',
            data: { "LocationCode": LocationCode },
            success: function (data) {
                $(".loading").hide();
                if (data !== null) {
                    var vdata = data[0];
                    var JobTbl = data[1];

                    ResetDivisionCode();

                    $.each(vdata, function (i, div) {
                        $("#DivisionCode").append(
                            $('<option></option>').val(div.DivisionCode).html(div.DivisionCode));
                    });

                    $("#FilteredJob").html("");
                    $("#FilteredJob").html(JobTbl);

                }
            },
            error: function (data) {

                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        ResetBindings();

    }
});

$('#DivisionCode').on('change', function () {
    ResetPieChart();
    $("#JidVAL").val("");
    $(".loading").show();
    var selIndex = this.selectedIndex;

    if (selIndex !== 0) {
        // var LocationCode = $('#LocationCode').val;
        var LocationCode = $('#LocationCode').val();

        var DivisionCode = this.value;

        $.ajax({
            url: 'getPlantCodes',
            type: "POST",
            dataType: 'json',
            data: { "LocationCode": LocationCode, "DivisionCode": DivisionCode },
            success: function (data) {
                $(".loading").hide();
                if (data !== null) {
               
                    var vdata = data[0];
                    var JobTbl = data[1];

                    ResetPlantCode();
                    //$.each(vdata, function (i, div) {
                    //    $("#PlantCode").append(
                    //        $('<option></option>').val(div.PlantCode).html(div.PlantCode));
                    //});
                    for (var i = 0; i < vdata.length; i++)
                    {
                        $("#PlantCode").append(
                        $('<option></option>').val(vdata[i].PlantCode).html(vdata[i].PlantCode));
                    }
                    $("#FilteredJob").html("");
                    $("#FilteredJob").html(JobTbl);
                }
            },
            error: function (data) {

                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        ResetBindings();
    }
});

$('#PlantCode').on('change', function () {
    ResetPieChart();
    $("#JidVAL").val("");
    var selIndex = this.selectedIndex;
    $(".loading").show();
    if (selIndex !== 0) {
        var PlantCode = this.value;
        var LocationCode = $('#LocationCode').val();
        var DivisionCode = $('#DivisionCode').val();
        $.ajax({
            url: 'getLineCodes',
            type: "POST",
            dataType: 'json',
            data: { "LocationCode": LocationCode, "DivisionCode": DivisionCode, "PlantCode": PlantCode },
            success: function (data) {
                if (data !== null) {
                    $(".loading").hide();
                    var vdata = data[0];
                    var JobTbl = data[1];

                    ResetLineCode();

                    $.each(vdata, function (i, div) {
                        $("#LineCode").append(
                            $('<option></option>').val(div.LineCode).html(div.LineCode));
                    });

                    $("#FilteredJob").html("");
                    $("#FilteredJob").html(JobTbl);
                }
            },
            error: function (data) {

                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        ResetBindings();
    }
});

$('#LineCode').on('change', function () {
    ResetPieChart();
    $("#JidVAL").val("");
    $(".loading").show();
    var selIndex = this.selectedIndex;
    if (selIndex !== 0) {
        var LineCode = this.value;
        var LocationCode = $('#LocationCode').val();
        var DivisionCode = $('#DivisionCode').val();
        var PlantCode = $('#PlantCode').val();

        $.ajax({
            url: 'getProductsStatusBatchesJobNames',
            type: "POST",
            dataType: 'json',
            data: { "LocationCode": LocationCode, "DivisionCode": DivisionCode, "PlantCode": PlantCode, "LineCode": LineCode },
            success: function (data) {
                $(".loading").hide();
                if (data !== null) {

                    var product = data[0];
                    var jobs = data[1];
                    var JobTbl = data[2];
                    var status = data[3];

                    ResetProductName();
                    ResetIdJobName();
                    ResetIdBatchNo();
                    ResetJobStatus();

                    //bind Product
                    $.each(product, function (i, prod) {
                        $("#ProductName").append(
                            $('<option></option>').val(prod.PAID).html(prod.Name));
                    });

                    //bind JobName
                    $.each(jobs, function (i, jb) {
                        $("#IdJobName").append(
                            $('<option></option>').val(jb.JID).html(jb.JobName));
                    });

                    //bind BatchNo
                    $.each(jobs, function (i, jb) {
                        $("#IdBatchNo").append(
                            $('<option></option>').val(jb.JID).html(jb.BatchNo));
                    });

                    //bind Status
                    $.each(status, function (i, sts) {
                        $("#JobStatus").append(
                            $('<option></option>').val(sts.Status).html(sts.Status));
                    });

                    $("#FilteredJob").html("");
                    $("#FilteredJob").html(JobTbl);
                }
            },
            error: function (data) {

                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        ResetBindings();
    }
});

$('#ProductName').on('change', function () {
    ResetPieChart();
    
    $("#JidVAL").val("");
    $(".loading").show();
    var selIndex = this.selectedIndex;
    if (selIndex !== 0) {
        var PAID = this.value;
        var LineCode = $("#LineCode").val();
        $.ajax({
            url: 'getProductWiseJobs',
            type: "POST",
            dataType: 'json',
            data: { "PAID": PAID, "LineCode": LineCode },
            success: function (data) {

                $(".loading").hide();
                if (data !== null) {
                    var vdata = data[1];
                    var jobs = data[0];
                   
                    $("#FilteredJob").html("");
                    $("#FilteredJob").html(vdata);
                    //bind JobName
                    ResetIdBatchNo();
                    ResetIdJobName();
                    for (var i = 0; i < jobs.length; i++) {
                        $("#IdBatchNo").append(
                            $('<option></option>').val(jobs[i].JID).html(jobs[i].JobName));
                    }

                    //bind BatchNo
                    $.each(jobs, function (i, jb) {
                        $("#IdJobName").append(
                            $('<option></option>').val(jb.JID).html(jb.BatchNo));
                    });

                    $('#JobStatus').val(0);
                   
                    $('#IdJobName').val(0);




                }
            },
            error: function (data) {

                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        ResetBindings();

    }
});

$('#IdBatchNo').on('change', function () {
    ResetPieChart();
    $("#JidVAL").val("");
    $(".loading").show();
    var selIndex = this.selectedIndex;
    if (selIndex !== 0) {
        var BatchNo = this.value;

        $.ajax({
            url: 'getBatchWiseJobs',
            type: "POST",
            dataType: 'json',
            data: { "BatchNo": BatchNo },
            success: function (data) {
                if (data !== null) {
                    $(".loading").hide();
                    var vdata = data[1];
                    $("#FilteredJob").html("");
                    $("#FilteredJob").html(vdata);
                    var jobs = data[0];
                    ResetIdJobName();
                    $.each(jobs, function (i, jb) {
                        $("#IdJobName").append(
                            $('<option></option>').val(jb.JID).html(jb.BatchNo));
                    });
                    $('#JobStatus').val(0);

                   
                    $('#ProductName').val(0);

                }
            },
            error: function (data) {

                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        ResetBindings();

    }
});

$('#IdJobName').on('change', function () {
    ResetPieChart();
    $("#JidVAL").val("");
    $(".loading").show();
    var selIndex = this.selectedIndex;
    var batchno = $('#IdBatchNo').val();
    if (selIndex !== 0) {
        var JobName = this.value;

        $.ajax({
            url: 'getJobNameWiseJobs',
            type: "POST",
            dataType: 'json',
            data: { "JobName": JobName, "batchno": batchno },
            success: function (data) {
                $(".loading").hide();

                if (data !== null) {
                    debugger;
                    var vdata = data[1];
                    var job = data[0];
                    $("#FilteredJob").html("");
                    $("#FilteredJob").html(vdata);
                    ResetIdBatchNo();
                    $.each(job, function (i, jb) {
                        $("#IdBatchNo").append(
                            $('<option></option>').val(jb.JID).html(jb.JobName));
                    });

                    $('#JobStatus').val(0);
                   
                    $('#ProductName').val(0);

                }
            },
            error: function (data) {

                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        ResetBindings();

    }
});

$('#JobStatus').on('change', function () {
    ResetPieChart();
    $("#JidVAL").val("");
    $(".loading").show();
    var selIndex = this.selectedIndex;
    if (selIndex !== 0) {
        var LineCode = $('#LineCode').val();
        var LocationCode = $('#LocationCode').val();
        var DivisionCode = $('#DivisionCode').val();
        var PlantCode = $('#PlantCode').val();
        var Status = this.value;
        $.ajax({
            url: 'getStatusWiseJobs',
            type: "POST",
            dataType: 'json',
            data: { "LocationCode": LocationCode, "DivisionCode": DivisionCode, "PlantCode": PlantCode, "LineCode": LineCode, "Status": Status },
            success: function (data) {
                $(".loading").hide();
                if (data !== null) {
                    var JobTbl = data[1];
                 
                    $("#FilteredJob").html("");
                    $("#FilteredJob").html(JobTbl);
                 
                    var jobs = data[0];
                    ResetIdJobName();
                    ResetIdBatchNo();
                    for (var i = 0; i < jobs.length; i++) {
                        $("#IdBatchNo").append(
                            $('<option></option>').val(jobs[i].JID).html(jobs[i].JobName));
                    }

                    for (var i = 0; i < jobs.length; i++) {
                        $("#IdJobName").append(
                            $('<option></option>').val(jobs[i].JID).html(jobs[i].JobName));
                    }
                    $('#IdBatchNo').val(0);
                    $('#IdJobName').val(0);
                    $('#ProductName').val(0);
                }
            },
            error: function (data) {

                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        ResetBindings();
    }
});

///////////////////////

// Date wise search Jobs
$("#btnMfgWise").click(function () {
    $("#JidVAL").val("");
    ResetBindings();
    ResetPieChart();
    $(".loading").show();
    var frmDt = $("#MfgDateWiseFrom").datepicker().val();
    var toDt = $("#MfgDateWiseTo").datepicker().val();
    if (frmDt !== "" && toDt !== "") {
        $.ajax({
            url: 'getMfgWiseJobs',
            type: "POST",
            dataType: 'json',
            data: { "FrmDt": frmDt, "ToDt": toDt },
            success: function (data) {
                $(".loading").hide();
                if (data !== null) {
                    var JobTbl = data;
                    $("#FilteredJob").html("");
                    $("#FilteredJob").html(JobTbl);
                } else {
                    console.log("Write some code in - RptJobHelper.js line 385");
                }
            },
            error: function (data) {

                toastr.warning(ErrorOccr);

            }
        });
    } else {

        $("#errmsg").html(SelectDate +"!");
        $(".loading").hide();
    }
});

$("#btnExpWise").click(function () {
    $("#JidVAL").val("");
    ResetBindings();
    ResetPieChart();
    $(".loading").show();
    var frmDt = $("#ExpDateWiseFrom").datepicker().val();
    var toDt = $("#ExpDateWiseTo").datepicker().val();
    if (frmDt !== "" && toDt !== "") {
        $.ajax({
            url: 'getExpgWiseJobs',
            type: "POST",
            dataType: 'json',
            data: { "FrmDt": frmDt, "ToDt": toDt },
            success: function (data) {
                $(".loading").hide();
                if (data !== null) {
                    var JobTbl = data;
                    $("#FilteredJob").html("");
                    $("#FilteredJob").html(JobTbl);
                }
            },
            error: function (data) {

                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $("#errmsg2").html(SelectDate);
        $(".loading").hide();
    }
});

function RemoveDisable() {
    $(".ddCSS").removeAttr("disabled");
    $(".icheck").removeAttr("disabled");
    $("#MfgDateWiseFrom").removeAttr("disabled");
    $("#MfgDateWiseTo").removeAttr("disabled");
    $("#btnMfgWise").removeAttr("disabled");
    $("#ExpDateWiseFrom").removeAttr("disabled");
    $("#ExpDateWiseTo").removeAttr("disabled");
    $("#btnExpWise").removeAttr("disabled");


}
$("#JobFromCreatedDate").on('change', function () {

    var frmDt = $("#JobFromCreatedDate").datepicker().val();
    if (frmDt == "") {
        $(".ddCSS").attr("disabled", "disabled");
        $(".icheck").attr("disabled", "disabled");
        $("#MfgDateWiseFrom").attr("disabled", "disabled");
        $("#MfgDateWiseTo").attr("disabled", "disabled");
        $("#btnMfgWise").attr("disabled", "disabled");
        $("#ExpDateWiseFrom").attr("disabled", "disabled");
        $("#ExpDateWiseTo").attr("disabled", "disabled");
        $("#btnExpWise").attr("disabled", "disabled");
        ResetPieChart();
    }
});
$("#JobToCreatedDate").on('change', function () {
    var toDt = $("#JobToCreatedDate").datepicker().val();
    if (toDt == "") {
        $(".ddCSS").attr("disabled", "disabled");
        $(".icheck").attr("disabled", "disabled");
        $("#MfgDateWiseFrom").attr("disabled", "disabled");
        $("#MfgDateWiseTo").attr("disabled", "disabled");
        $("#btnMfgWise").attr("disabled", "disabled");
        $("#ExpDateWiseFrom").attr("disabled", "disabled");
        $("#ExpDateWiseTo").attr("disabled", "disabled");
        $("#btnExpWise").attr("disabled", "disabled");
        ResetPieChart();
    }
});
$("#btnCreatedWise").click(function () {
    $("#JidVAL").val("");
    ResetBindings();
    ResetPieChart();
    $("#JidVAL").val("");
    var frmDt = $("#JobFromCreatedDate").datepicker().val();
    var toDt = $("#JobToCreatedDate").datepicker().val();
    if (frmDt === "" || toDt === "") {
        $('#modal-msg').html(SelectDate);
        $("#showMSGBx").trigger("click");
        return false;
    }
    ////

    if (frmDt !== "" && toDt !== "") {
        RemoveDisable();
        $(".loading").show();
        $.ajax({
            url: 'getCreatedDtWiseJobs',
            type: "POST",
            dataType: 'json',
            data: { "FrmDt": frmDt, "ToDt": toDt },
            success: function (data) {
                $(".loading").hide();

                if (data !== null) {

                    if (data === "No Data") {
                        $('#modal-msg').html(NodataAvilable);
                        $("#showMSGBx").trigger("click");
                        $("#dataProductName").html('-');
                        $("#dataGTIN").html('-');
                        $("#dataGoodCount").html('-');
                        $("#dataTotal").html('-');
                        $("#dataDecomm").html('-');
                        $("#dataEXP").html('-');
                        $("#dataBQty").html('-');
                        $("#dataBadCount").html('-');
                        $("#dataMFG").html('-');
                        $('.chart').hide();
                        $('#chart3').hide();
                        $("#FilteredJob").html("");
                        $("#ReportLink").hide();
                        return false;
                    }
                    else
                    {
                        $("#ReportLink").show();
                    }

                    var JobTbl = data;
                    $("#FilteredJob").html("");
                    $("#FilteredJob").html(JobTbl);
                }

            },
            error: function (data) {

                toastr.warning(ErrorOccr);

            }
        });
    } else {

        $("#errmsg3").html(SelectDate + " !");
        $(".loading").hide();
    }
});
///////////////////////

// Generate Reports

function genReport(rptMethod) {

    $(".loading").show();
    switch (rptMethod) {

        case 'RptProductWise':
            performRptProductWise();
          
            break;
        case 'RptUIDList':
            performRptUIDList();
           
            break;
        case 'RptBadUIDList':
            performRptBadUIDList();
           
            break;
        case 'RptSummary':
            performRptSummary();
          
            break;
        case 'RptDetail':
            performDetailedJobsRpt();
           
            break;
        case 'RptDetailWithOperator':
            performRptDtlsWithOprtr();
           
            break;
        case 'RptOperatorStatistics':
            performRptOprtrStats();
          
            break;
        case 'RptDetailedJobInfo':
            performRptDetailedJobInfo();
          
            break;
        case 'RptParentChildRelationships':
            performRptPCRelationships();
            
            break;
        case 'RptJobWiseSSCC':
            performRptJobWiseSSCC();
           
            break;
        case 'RptDecommisionedUID':
            performRptDecommisionedUIds();
           
            break;

        default:
            $(".loading").hide();
            $('#modal-msg').html(NodataAvilable);
            $("#showMSGBx").trigger("click");
    }
}

function performRptUIDList() {
    var FromDate = $('#JobFromCreatedDate').datepicker().val();
    var ToDate = $('#JobToCreatedDate').datepicker().val();


 
    if (IsDatesValid(FromDate, ToDate)) {
        var selJID = $("#JidVAL").val();
        if (selJID === "") {
            $(".loading").hide();
            $('#modal-msg').html(TnT.LangResource.GlobalRes.RptUidListSelectRecord);
            $("#showMSGBx").trigger("click");
            return false;
        } else {
            $.ajax({
                url: 'getUIDList',
                type: "POST",
                dataType: 'json',
                data: { "JID": selJID },
                success: function (data) {
                    $(".loading").hide();
                    if (data === "No Data") {

                        $('#modal-msg').html(NodataAvilable);
                        $("#showMSGBx").trigger("click");

                    } else {
                        $('#modal-rpt-data').html(data);
                        $("#showRPTBx").trigger("click");
                    }
                },
                error: function (data) {
                    $(".loading").hide();
                    toastr.warning(ErrorOccr);

                }
            });
        }
    }
    else {
        $(".loading").hide();
        $('#modal-msg').html(InvalidDateFormat + ' !<br/> <h5>Format: dd/mm/yyyy </h5>');
        $("#showMSGBx").trigger("click");
    }
}


function performRptBadUIDList() {

    var FromDate = $('#JobFromCreatedDate').datepicker().val();
    var ToDate = $('#JobToCreatedDate').datepicker().val();



    if (IsDatesValid(FromDate, ToDate)) {
        var selJID = $("#JidVAL").val();
        if (selJID === "") {
            $(".loading").hide();
            //var Record : '<%=TnT.LangResource.GlobalRes.RptUidListSelectRecord%>';
            // $('#modal-msg').html('Please Select Record'); SelectRecord
            $('#modal-msg').html(SelectRecord);
            $("#showMSGBx").trigger("click");
            return false;
        } else {
            $.ajax({
                url: 'getBadUIDList',
                type: "POST",
                dataType: 'json',
                data: { "JID": selJID },
                success: function (data) {
                    $(".loading").hide();
                    if (data === "No Data") {

                        $('#modal-msg').html(NodataAvilable);
                        $("#showMSGBx").trigger("click");

                    } else {
                        $('#modal-rpt-data').html(data);
                        $("#showRPTBx").trigger("click");
                    }
                },
                error: function (data) {
                    $(".loading").hide();
                    toastr.warning(ErrorOccr);

                }
            });
        }
    }
    else {
        $(".loading").hide();
        $('#modal-msg').html(InvalidDateFormat +' !<br/> <h5>Format: dd/mm/yyyy </h5>');
        $("#showMSGBx").trigger("click");
    }
}

function performRptProductWise() {
   
    var FromDate = $('#JobFromCreatedDate').datepicker().val();
    var ToDate = $('#JobToCreatedDate').datepicker().val();



    if (IsDatesValid(FromDate, ToDate)) {

        $.ajax({
            url: 'getProductWiseReport',
            type: "POST",
            dataType: 'json',
            data: { "FromDate": FromDate, "ToDate": ToDate },
            success: function (data) {
                $(".loading").hide();
                if (data === "No Data") {

                    $('#modal-msg').html(NodataAvilable);
                    $("#showMSGBx").trigger("click");

                } else {
                    $('#modal-rpt-data').html(data);
                    $("#showRPTBx").trigger("click");
                }

            },
            error: function (data) {
                $(".loading").hide();
                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        $('#modal-msg').html(InvalidDateFormat+'!<br/> <h5>Format: dd/mm/yyyy </h5>');
        $("#showMSGBx").trigger("click");
    }
}

function performRptSummary() {

    var FromDate = $('#JobFromCreatedDate').datepicker().val();
    var ToDate = $('#JobToCreatedDate').datepicker().val();

    if (IsDatesValid(FromDate, ToDate)) {

        $.ajax({
            url: 'getSummaryReport',
            type: "POST",
            dataType: 'json',
            data: { "FromDate": FromDate, "ToDate": ToDate },
            success: function (data) {
                $(".loading").hide();
                if (data === "No Data") {

                    $('#modal-msg').html(NodataAvilable);
                    $("#showMSGBx").trigger("click");

                } else {

                    $('#modal-rpt-data').html(data);
                    $("#showRPTBx").trigger("click");
                }

            },
            error: function (data) {
                $(".loading").hide();
                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        $('#modal-msg').html(InvalidDateFormat+' !<br/> <h5>Format: dd/mm/yyyy </h5>');
        $("#showMSGBx").trigger("click");
    }
}

function performDetailedJobsRpt() {

    var FromDate = $('#JobFromCreatedDate').datepicker().val();
    var ToDate = $('#JobToCreatedDate').datepicker().val();

    if (IsDatesValid(FromDate, ToDate)) {

        $.ajax({
            url: 'getDetailedReport',
            type: "POST",
            dataType: 'json',
            data: { "FromDate": FromDate, "ToDate": ToDate },
            success: function (data) {
                $(".loading").hide();
                if (data === "No Data") {

                    $('#modal-msg').html(NodataAvilable);
                    $("#showMSGBx").trigger("click");

                } else {

                    $('#modal-rpt-data').html(data);
                    $("#showRPTBx").trigger("click");
                }

            },
            error: function (data) {
                $(".loading").hide();
                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        $('#modal-msg').html(InvalidDateFormat+' !<br/> <h5>Format: dd/mm/yyyy </h5>');
        $("#showMSGBx").trigger("click");
    }

}

function performRptDtlsWithOprtr() {

    var FromDate = $('#JobFromCreatedDate').datepicker().val();
    var ToDate = $('#JobToCreatedDate').datepicker().val();
    var level = "NA";
    if (IsDatesValid(FromDate, ToDate)) {

        $.ajax({
            url: 'getDetailsWithOprtrRpt',
            type: "POST",
            dataType: 'json',
            data: { "FromDate": FromDate, "ToDate": ToDate, "Level": level },
            success: function (data) {
                $(".loading").hide();
                if (data === "No Data") {

                    $('#modal-msg').html(NodataAvilable);
                    $("#showMSGBx").trigger("click");

                } else {

                    $('#modal-rpt-data').html(data);
                    $("#showRPTBx").trigger("click");
                }

            },
            error: function (data) {
                $(".loading").hide();
                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        $('#modal-msg').html(InvalidDateFormat+'!<br/> <h5>Format: dd/mm/yyyy </h5>');
        $("#showMSGBx").trigger("click");
    }
}

function performRptOprtrStats() {

    var FromDate = $('#JobFromCreatedDate').datepicker().val();
    var ToDate = $('#JobToCreatedDate').datepicker().val();
    var level = "NA";
    if (IsDatesValid(FromDate, ToDate)) {

        $.ajax({
            url: 'getOprtrStatsRpt',
            type: "POST",
            dataType: 'json',
            data: { "FromDate": FromDate, "ToDate": ToDate, "Level": level },
            success: function (data) {
                $(".loading").hide();
                if (data === "No Data") {

                    $('#modal-msg').html(NodataAvilable);
                    $("#showMSGBx").trigger("click");

                } else {

                    $('#modal-rpt-data').html(data);
                    $("#showRPTBx").trigger("click");
                }

            },
            error: function (data) {
                $(".loading").hide();
                toastr.warning(ErrorOccr);

            }
        });
    } else {
        $(".loading").hide();
        $('#modal-msg').html( InvalidDateFormat+'!<br/> <h5>Format: dd/mm/yyyy </h5>');
        $("#showMSGBx").trigger("click");
    }
}

function performRptDetailedJobInfo() {

    var FromDate = $('#JobFromCreatedDate').datepicker().val();
    var ToDate = $('#JobToCreatedDate').datepicker().val();

    if (IsDatesValid(FromDate, ToDate)) {
        var selJID = $("#JidVAL").val();
        if (selJID === "") {
            $(".loading").hide();
            $('#modal-msg').html(SelectRecord);
            $("#showMSGBx").trigger("click");
            return false;
        } else {
            $.ajax({
                url: 'getDetailedJobInfo',
                type: "POST",
                dataType: 'json',
                data: { "JID": selJID },
                success: function (data) {
                    $(".loading").hide();
                    if (data === "No Data") {

                        $('#modal-msg').html(NodataAvilable);
                        $("#showMSGBx").trigger("click");

                    } else if (data === "Batch is not verified") {
                        $('#modal-msg').html('Batch Is Not Verified');
                        $("#showMSGBx").trigger("click");
                    }
                    else {
                        $('#modal-rpt-data').html(data);
                        $("#showRPTBx").trigger("click");
                    }
                },
                error: function (data) {
                    $(".loading").hide();
                    toastr.warning(ErrorOccr);

                }
            });
        }
    }
    else {
        $('#modal-msg').html(InvalidDateFormat+' !<br/> <h5>Format: dd/mm/yyyy </h5>');
        $("#showMSGBx").trigger("click");
    }
}

function performRptDecommisionedUIds() {

    var FromDate = $('#JobFromCreatedDate').datepicker().val();
    var ToDate = $('#JobToCreatedDate').datepicker().val();

    if (IsDatesValid(FromDate, ToDate)) {
        var selJID = $("#JidVAL").val();
        if (selJID === "") {
            $(".loading").hide();
            $('#modal-msg').html(SelectRecord);
            $("#showMSGBx").trigger("click");
            return false;
        } else {
            $.ajax({
                url: 'getDecommUIDList',
                type: "POST",
                dataType: 'json',
                data: { "JID": selJID },
                success: function (data) {
                    $(".loading").hide();
                    if (data === "No Data") {

                        $('#modal-msg').html(NodataAvilable);
                        $("#showMSGBx").trigger("click");

                    } else {
                        $('#modal-rpt-data').html(data);
                        $("#showRPTBx").trigger("click");
                    }
                },
                error: function (data) {
                    $(".loading").hide();
                    toastr.warning(ErrorOccr);

                }
            });
        }
    }
    else {
        $(".loading").hide();
        $('#modal-msg').html(InvalidDateFormat+' !<br/> <h5>Format: dd/mm/yyyy </h5>');
        $("#showMSGBx").trigger("click");
    }
}

function performRptPCRelationships() {
    $(".loading").show();
    var FromDate = $('#JobFromCreatedDate').datepicker().val();
    var ToDate = $('#JobToCreatedDate').datepicker().val();

    if (IsDatesValid(FromDate, ToDate)) {

        var selJID = $("#JidVAL").val();
        if (selJID === "") {
            $(".loading").hide();
            $('#modal-msg').html(SelectRecord);
            $("#showMSGBx").trigger("click");
            return false;
        } else {
            $.ajax({

                url: 'getPCRelationshipReport',
                type: "POST",
                dataType: 'json',
                data: { "JID": selJID },
                success: function (data) {
                   
                    if (data === "No Data") {
                        $(".loading").hide();
                        $('#modal-msg').html(NodataAvilable);
                        $("#showMSGBx").trigger("click");

                    } else {
                        $(".loading").hide();
                        $('#modal-rpt-data').html(data);
                        $("#PCRela").rowspanizer({ vertical_align: 'middle' });
                        $("#showRPTBx").trigger("click");
                    }
                    $(".loading").hide();
                },
                error: function (data) {
                    $(".loading").hide();
                    toastr.warning(ErrorOccr);

                }
            });
        }
    }
    else {
        $(".loading").hide();
        $('#modal-msg').html(InvalidDateFormat+' !<br/> <h5>Format: dd/mm/yyyy </h5>');
        $("#showMSGBx").trigger("click");
    }
}

function performRptJobWiseSSCC() {
    var FromDate = $('#JobFromCreatedDate').datepicker().val();
    var ToDate = $('#JobToCreatedDate').datepicker().val();

    if (IsDatesValid(FromDate, ToDate)) {

        var selJID = $("#JidVAL").val();
        if (selJID === "") {
            $(".loading").hide();
            $('#modal-msg').html(SelectRecord);
              $("#showMSGBx").trigger("click");
            return false;
        } else {
            $.ajax({
                url: 'getJobwiseSSCCReport',
                type: "POST",
                dataType: 'json',
                data: { "JID": selJID },
                success: function (data) {
                    $(".loading").hide();
                    if (data === "No Data") {

                        $('#modal-msg').html(NodataAvilable);
                        $("#showMSGBx").trigger("click");

                    } else {
                        $('#modal-rpt-data').html(data);
                        $("#showRPTBx").trigger("click");
                    }
                },
                error: function (data) {
                    $(".loading").hide();
                    toastr.warning(ErrorOccr);

                }
            });
        }
    }
    else {
        $(".loading").hide();
        $('#modal-msg').html('Invalid Date format !<br/> <h5>Format: dd/mm/yyyy </h5>');
        $("#showMSGBx").trigger("click");
    }
}

function IsDatesValid(FromDate, ToDate) {
    if ((FromDate === "") || (ToDate === "")) {

        return false;
    }
    var JFVal = FromDate;

    JFvalues = JFVal.split('/');
    JFDay = JFvalues[0];
    JFMonth = JFvalues[1];
    JFYear = JFvalues[2];
    FinalJFDate = JFMonth + '/' + JFDay + '/' + JFYear;
    //var startDate = new Date(FinalJFDate);


    var JtVal = ToDate;

    Jtvalues = JtVal.split('/');
    JtDay = Jtvalues[0];
    JtMonth = Jtvalues[1];
    JtYear = Jtvalues[2];
    FinalJtDate = JtMonth + '/' + JtDay + '/' + JtYear;
    var startDate = new Date(FinalJFDate);

    var endDate = new Date(FinalJtDate);

    var delta = endDate - startDate;
    x = delta / 1000
    seconds = x % 60
    x /= 60
    minutes = x % 60
    x /= 60
    hours = x % 24
    x /= 24
    days = x
    if (days < 0) {
        alert("To Date is less than from date");
        return false;
    } else {
        return true;
    }

}

