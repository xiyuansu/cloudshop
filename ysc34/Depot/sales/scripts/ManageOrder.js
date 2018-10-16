var curpagesize = 10, curpageindex = 1, total = 0;
var DEFAULT_PAGE_INDEX = 1, DEFAULT_PAGESIZE = 10;
var showbox, dataurl, showpager, datanullbox, datanulldom;
function ToDetail(orderId) {
    var urldata = {
        pagesize: curpagesize,
        pageindex: curpageindex
    }

    initQuery(urldata);

    var listurl = "/Depot/Sales/ManageOrder";
    var url = listurl + "?";
    for (var item in urldata) {
        url += item + "=" + urldata[item] + "&";
    }
    location.href = "OrderDetails?OrderId=" + orderId + "&returnUrl=" + encodeURIComponent(url);
}

$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;

    try {
        curpageindex = parseInt($("#curpageindex").val());
        if (isNaN(curpageindex)) {
            curpageindex = DEFAULT_PAGE_INDEX;
        }
    } catch (e) {
        curpageindex = DEFAULT_PAGE_INDEX;
    }
    try {
        curpagesize = parseInt($("#pagesize_dropdown").val());
        if (isNaN(curpagesize)) {
            curpagesize = DEFAULT_PAGESIZE;
        }
    } catch (e) {
        curpagesize = DEFAULT_PAGESIZE;
    }
    InitPage();
    $("#anchors" + $("#ordStatus").val()).find("a").addClass("hover");
    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
    //搜索
    $("#btnSearch").on("click", function () {
        curpagesize = parseInt($("#pagesize_dropdown").val());
        ShowListData(showbox, 1, curpagesize, true);
    });
    //订单备注
    $("#btnRemark").on("click", function () {
        Post_RemarkOrder();
    });
    //关闭订单
    $("#btnCloseOrder").on("click", function () {
        Post_CloseOrder();
    });
});

function InitPage() {
    $('#orderStartDate').datetimepicker({
        minView: 2, format: 'yyyy-mm-dd', language: 'zh-CN', weekStart: 1,
        todayBtn: 1, autoclose: 1, todayHighlight: 1, startView: 2,
        forceParse: 0, showMeridian: 1,
    });
    $('#orderEndDate').datetimepicker({
        minView: 2, format: 'yyyy-mm-dd',
        language: 'zh-CN', weekStart: 1, todayBtn: 1,
        autoclose: 1, todayHighlight: 1, startView: 2,
        forceParse: 0, showMeridian: 1,
    });
    //每页显示数量
    $("#pagesize_dropdown").on("change", function () {
        var _t = $(this);
        curpagesize = parseInt(_t.val());
        ShowListData(showbox, curpageindex, curpagesize, false);
        showpager.HiPaginator("option", { pageSize: curpagesize });
        showpager.HiPaginator("redraw");
    });
    $('.statusanchors').on("click", function () {
        var _t = $(this);
        $(".statusanchors").removeClass("hover");
        _t.addClass("hover");
        $("#ordStatus").val(_t.data("staus"));
        curpagesize = parseInt($("#pagesize_dropdown").val());
        $("#txtOrderId").val("");
        ShowListData(showbox, 1, curpagesize, true);
    });
}

function Post_CloseOrder() {
    var CloseReason = $("#ddlCloseReason").val();
    var OrderId = $("#hidOrderId").val();
    var pdata = {
        orderId: OrderId, closeReason: CloseReason, action: "closeorder"
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
                ShowMsg("关闭订单成功", true);
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

function Post_RemarkOrder() {
    var RemarkTxt = $("#txtRemark").val();
    var OrderId = $("#hidOrderId").val();
    var orderRemark = $("#orderRemarkImageForRemark [type='radio']:checked").val();
    var pdata = {
        orderId: OrderId, remarkFlag: orderRemark, remarkTxt: RemarkTxt, action: "remark"
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
                ShowMsg("保存备忘录成功", true);
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

function Post_ConfirmOrder(OrderId) {
    var pdata = {
        orderId: OrderId, action: "confirmorder"
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
                ShowMsg("成功确认了该订单", true);
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

function Post_FinishTrade(OrderId) {
    var pdata = {
        orderId: OrderId, action: "finishtrade"
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
                ShowMsg("成功的完成了该订单", true);
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
function Post_ConfirmPay(OrderId) {
    var pdata = {
        orderId: OrderId, action: "confirmpay"
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
                ShowMsg("成功的确认了订单收款", true);
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

//搜索参数
function initQuery(obj) {
    var OrderId = $("#txtOrderId").val();
    if (OrderId.length > 0) {
        obj.OrderId = OrderId;
    }
    var ShipTo = $("#txtShopTo").val();
    if (ShipTo.length > 0) {
        obj.ShipTo = ShipTo;
    }
    var StartDate = $("#orderStartDate").val();
    if (StartDate.length > 0) {
        obj.StartDate = StartDate;
    }
    var EndDate = $("#orderEndDate").val();
    if (EndDate.length > 0) {
        obj.EndDate = EndDate;
    }
    var orderType = $("#ddlOrderType").val();
    if (orderType.length > 0) {
        obj.OrderType = orderType;
    }
    var takecode = $("#txtCode").val();
    if (takecode.length > 0) {
        obj.takecode = takecode;
    }
    var OrderStatus = $("#ordStatus").val();
    if (OrderStatus.length > 0) {
        obj.OrderStatus = OrderStatus;
    }
    var isTickit = $("#ddlIsOpenReceipt").val();
    if (isTickit.length > 0) {
        obj.isTickit = isTickit;
    }
    var invoiceType = $("#dropInvoiceType").val();
    if (invoiceType.length > 0) {
        obj.InvoiceType = invoiceType;
    }
    return obj;
}



function ShowListData(target, page, pagesize, initpagination) {
    page = page || 1;
    pagesize = pagesize || 10;
    if (typeof (initpagination) == "undefined") { initpagination = true; }
    var urldata = {
        page: page, rows: pagesize, action: "getlist"
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
            databox.empty(); DataNullHelper.hide(datanullbox);
            if (data) {
                total = data.total;
                if (total == 0) {
                    isshowpage = false;
                }
                if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));
                    $('[data-toggle="tooltip"]', databox).tooltip();
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
                        prev: '<a href="javascript:;" class="page-prev">上一页</a>',
                        next: '<a href="javascript:;" class="page-next">下一页</a>',
                        page: '<a href="javascript:;">{{page}}</a>',
                        first: '',
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


function ReloadPageData(pageindex) {
    /*复位*/
    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }
    var _page = showpager;
    var opts = _page.HiPaginator("getOption");
    curpagesize = opts.pageSize;
    curpageindex = pageindex || opts.currentPage;
    ShowListData(showbox, curpageindex, curpagesize, true);
}
//关闭并刷新页面数据
function CloseDialogAndReloadData(id) {
    if (id) {
        art.dialog({ id: id }).close();
    } else {
        CloseDialogFrame("", false);
    }
    ReloadPageData();
}

function ExportToExcel() {
    var orderIds = GetSelectId();
    var urldata = {
        action: "exportexcel"
    }
    if (orderIds.length > 0) {
        urldata.orderIds = orderIds;
    } else {
        initQuery(urldata);
    }

    var url = dataurl + "?";
    for (var item in urldata) {
        url += item + "=" + urldata[item] + "&";
    }

    window.open(url);
}

//获取选中的ID
function GetSelectId() {
    var v_str = "";
    $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
        if (rowIndex > 0) {
            v_str += ",";
        }
        v_str += $(rowItem).attr("value");
    });
    return v_str;
}