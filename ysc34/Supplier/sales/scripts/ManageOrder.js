var curpagesize = 10, curpageindex = 1, total = 0;
var DEFAULT_PAGE_INDEX = 1, DEFAULT_PAGESIZE = 10;
var showbox, dataurl, showpager, datanullbox, datanulldom;
var queryParameters;

function ToDetail(orderId) {
    var listurl = "/Supplier/Sales/ManageOrder";
    var url = listurl + "?";
    if (queryParameters) {
        for (var item in queryParameters) {
            url += item + "=" + queryParameters[item] + "&";
        }
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
    $("#anchors" + $("#ordStatus").val()).addClass("hover");

    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
    //搜索
    $("#btnSearch").on("click", function () {
        curpagesize = parseInt($("#pagesize_dropdown").val());
        ShowListData(showbox, 1, curpagesize, true);
    });
    //导出
    $("#btnExportExcel").on("click", function () {
        Post_ExportExcel();
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

function Post_ExportExcel() {
    var orderIds = "";
    $("input:checked[name='CheckBoxGroup']").each(function () {
        orderIds += $(this).val() + ",";
    });
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

//搜索参数
function initQuery(obj) {
    var OrderId = $("#txtOrderId").val();
    if (OrderId.length > 0) {
        obj.OrderId = OrderId;
    }
    var ProductName = $("#txtProductName").val();
    if (ProductName.length > 0) {
        obj.ProductName = ProductName;
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
    var OrderStatus = $("#ordStatus").val();
    if (OrderStatus.length > 0) {
        obj.OrderStatus = OrderStatus;
    }
    var region = $("#regionSelectorValue").val();
    if (region.length > 0) {
        obj.region = region;
    }
   // obj.InvoiceType = $("#dropInvoiceType").val();
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
    queryParameters = urldata;
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


function ReloadPageData(pageindex) {/*复位*/    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }/*复位*/    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }
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

function SuccessAndCloseReload(msg) {
    msg = msg || "操作成功";
    ShowMsg(msg, true);
    CloseDialogAndReloadData();
}

function DataRedraw(databox) {

    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red' });
}