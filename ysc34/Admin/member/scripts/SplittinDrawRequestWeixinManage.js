var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager,datanullbox,datanulldom;



//页面初始
$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
datanullbox = showbox;
    
    curpagesize = parseInt($("#pagesize_dropdown").val());
    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
    DataRedraw(showbox);
    //每页显示数量
    $("#pagesize_dropdown").on("change", function () {
        var _t = $(this);
        curpagesize = parseInt(_t.val());
        ShowListData(showbox, curpageindex, curpagesize, false);
        showpager.HiPaginator("option", { pageSize: curpagesize });
        showpager.HiPaginator("redraw");
    });
    //搜索
    $("#btnSearch").on("click", function () {
        curpagesize = parseInt($("#pagesize_dropdown").val());
        ShowListData(showbox, 1, curpagesize, true);
    });

});

function ReloadPageData(pageindex) {
    /*复位*/
    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }
    var _page = showpager;
    var opts = _page.HiPaginator("getOption");
    curpagesize = opts.pageSize;
    curpageindex = pageindex || opts.currentPage;
    ShowListData(showbox, curpageindex, curpagesize, true);
}


//搜索参数
function initQuery(obj) {
    var DateStart = $("#calendarStart").val();
    if (DateStart.length > 0) {
        obj.DateStart = DateStart;
    }

    var DateEnd = $("#calendarEnd").val();
    if (DateEnd.length > 0) {
        obj.DateEnd = DateEnd;
    }
    
    var UserName = $("#txtUserName").val();
    if (UserName.length > 0) {
        obj.UserName = UserName;
    }
    obj.AuditStatus = 1;
    obj.DrawRequestType =2;
    return obj;
}



function ShowListData(target, page, pagesize, initpagination) {
    page = page || 1;
    pagesize = pagesize || 10;
    if (typeof (initpagination) == "undefined") { initpagination = true; }
    var urldata = {
        page: page, rows: pagesize, action: "getSplittinDraws"
    }
    initQuery(urldata);
    var loadingobj = showLoading(showbox);
    target.empty();
DataNullHelper.hide(datanullbox);

    $.ajax({
        type: "GET",
        url: dataurl,
        data: urldata,
        dataType: "json",
        success: function (data) {
            var isnodata = true;
            var isshowpage = true;
            var databox = $(target);
            loadingobj.close();
            databox.empty();DataNullHelper.hide(datanullbox);
            if (data) {
                total = data.total;
                if (total == 0) {
                    isshowpage = false;
                }
                if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));
                    DataRedraw(databox);
                }
            }
            if (isnodata) {
                //total = 0;
                DataNullHelper.show(datanullbox);
            }
            curpageindex = page;
            if (initpagination) {
                //初始分页组件
                if (isshowpage) {
                    showpager.HiPaginator({
                        totalCounts: total,
                        pageSize: curpagesize,
                        currentPage: curpageindex,
                        first: '',
                        prev: '<a href="javascript:;" class="page-prev">上一页</a>',
                        next: '<a href="javascript:;" class="page-next">下一页</a>',
                        page: '<a href="javascript:;">{{page}}</a>',
                        last: '',
                        visiblePages: 10,
                        disableClass: 'hide',
                        activeClass: 'page-cur',
                        onPageChange: function (pageindex, type) {
                            //分页回调
                            curpagesize = $("#showpager").HiPaginator("getPagesize");
                            if (type != "init") {
                                ShowListData(showbox, pageindex, curpagesize, false);
                            }
                        }
                    });
                } else {
                    if (showpager.HiPaginator("isExist")) {
                        showpager.HiPaginator("destroy");
                    }
                }
            }
        },
        error: function () {
            loadingobj.close();
            ShowMsg("系统内部异常", false);
        }
    });
}
//后续处理
function DataRedraw(databox) {
    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red'});
}

function DrawRequestAlipay() {
    if ($("#hidIsDemoSite").val() == "1") {
        ShowMsg("演示站点，无法真实付款", false);
        return;
    }
    var ids = "", UserName = "", CodeNum = 0;
    $('input[name="CheckBoxGroup"]:checked').each(function () {
        ids += $(this).val() + ",";
        UserName += $(this).attr("uname") + ";";
        CodeNum++;
    })
    if (ids == "") {
        alert("请至少选择一项"); return false;
    }
    if (UserName.length > 50)
        UserName = UserName.substr(0, 50) + "...";
    UserName += "(" + CodeNum + "个会员)";
    $('#JournalNumber').val(ids);
    $('#lblUserName').html(UserName);

    DialogShow("确认付款", 'balancedrawrequest', 'divunlinerecharge', 'btnMustRequest');
    return false;
}
function ShowUnLineReCharge(opers, id, UserName) {
    if ($("#hidIsDemoSite").val() == "1") {
        ShowMsg("演示站点，无法真实付款", false);
        return;
    }
    $('#ChargeType').val('UnLineReCharge');
    $('#JournalNumber').val(id);
    $('#lblUserName').html(UserName);

    DialogShow(opers, 'balancedrawrequest', 'divunlinerecharge', 'btnRequest');
}
function ShowRefuseRequest(opers, id, UserName) {

    $('#ChargeType').val('RefuseRequest');
    $('#JournalNumber').val(id);
    $('#lblRefuseName').html(UserName);
    setArryText('txtReason', '');

    DialogShow(opers, 'balancedrawrequest', 'divrefuserequest', 'btnRequest');
}
function validatorForm() {
    if ($('#ChargeType').val() == "RefuseRequest")//拒绝必填原因
    {
        if ($("#txtReason").val() == "") {
            alert('请填写拒绝原因');
            $("#txtReason").focus();
            return false;
        }
        if ($("#txtReason").val().length > 50) {
            alert('拒绝原因不可超过50字');
            $("#txtReason").focus();
            return false;
        }
    }
    return true;
}
 

function DrawRequest() {
    var ChargeType = $("#ChargeType").val();
    var JournalNumber = $("#JournalNumber").val();
    var Reason = $("#txtReason").val();

    var pdata = {
        ChargeType: ChargeType, JournalNumber: JournalNumber, Reason: Reason, action: "drawRequest"
    }
    var loading = showCommonLoading();
    $.ajax({
        type: "post",
        url: dataurl,
        data: pdata,
        dataType: "json",
        success: function (data) {
            loading.close();
            if (data.success) {
                ShowMsg(data.message, true);
                ReloadPageData();
            } else {
                ShowMsg(data.message, false);
            }
        },
        error: function () {
            loading.close();
        }
    });  
}

//生成报告
function ExportToExcel() {
    var url = dataurl + "?";
    var urldata = {
        action: "exporttoexcel"
    }
    initQuery(urldata);
    for (var item in urldata) {
        url += item + "=" + urldata[item] + "&";
    }
    window.open(url);
}


function MoreDrawRequest() {

    if ($("#hidIsDemoSite").val() == "1")
    {
        ShowMsg("演示站点,无法真实付款", false);
        return;
    }
    var ChargeType = $("#ChargeType").val();
    var JournalNumber = $("#JournalNumber").val();
    var Reason = $("#txtReason").val();

    var pdata = {
        ChargeType: ChargeType, JournalNumber: JournalNumber, Reason: Reason, action: "moredrawRequest"
    }
    var loading = showCommonLoading();
    $.ajax({
        type: "post",
        url: dataurl,
        data: pdata,
        dataType: "json",
        success: function (data) {
            loading.close();
            if (data.success) {
                ShowMsg(data.message, true);
                ReloadPageData();
            } else {
                ShowMsg(data.message, false);
            }
        },
        error: function () {
            loading.close();
        }
    });
}

