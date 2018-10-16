var curpagesize = 10, curpageindex = 1, total = 0;
var DEFAULT_PAGE_INDEX = 1, DEFAULT_PAGESIZE = 10;
var showbox, dataurl, showpager, datanullbox, datanulldom;
var queryParameters,isshowstore;

function ToDetail(orderId) {
    var listurl = "/admin/sales/ManageOrder";
    var url = listurl + "?";
    if (queryParameters) {
        for (var item in queryParameters) {
            url += item + "=" + queryParameters[item] + "&";
        }
    }
    location.href = "OrderDetails?OrderId=" + orderId + "&returnUrl=" + encodeURIComponent(url);
}

function ToMemberDetail(userId) {
    var listurl = "/admin/sales/ManageOrder";
    var url = listurl + "?";
    if (queryParameters) {
        for (var item in queryParameters) {
            url += item + "=" + queryParameters[item] + "&";
        }
    }
    location.href = "/Admin/member/MemberDetails?userId= " + userId;
}

//页面初始
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

    isshowstore = $("#isshowstore").val() == "1";
    InitPage();
    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
    //搜索
    $("#btnSearch").on("click", function () {
        ShowListData(showbox, 1, curpagesize, true);
    });
    //关闭订单
    $("#btnCloseOrder").on("click", function () {
        Post_CloseOrder();
    });
    //导出
    $("#btnExportExcel").on("click", function () {
        Post_ExportExcel();
    });
});

function InitPage() {
    var pagesizedom = $("#pagesize_dropdown");
    pagesizedom.find("option").each(function () {
        var _t = $(this);
        if (_t.val() == curpagesize) {
            _t.prop("selected",true);
        }
    });
    //每页显示数量
    pagesizedom.on("change", function () {
        var _t = $(this);
        curpagesize = parseInt(_t.val());
        ShowListData(showbox, curpageindex, curpagesize, false);
        showpager.HiPaginator("option", { pageSize: curpagesize });
        showpager.HiPaginator("redraw");
    });
    //Tab
    var tab_status = $(".tab_status");
    tab_status.on("click", function () {
        var _t = $(this);
        var _d = _t.data("status");
        $("#txtOrderId").val("");
        $("#hidStatus").val(_d);
        tab_status.removeClass("hover");
        _t.addClass("hover");
        ShowListData(showbox, 1, curpagesize, true);
    });
    var curstatus = $("#hidStatus").val();
    if (curstatus.toString().length > 0) {
        tab_status.removeClass("hover");
        $(".tab_status[data-status='" + curstatus + "']").addClass("hover");
    } else {
        tab_status.eq(0).addClass("hover");
    }
}

//搜索参数
function initQuery(obj) {
    var OrderId = $("#txtOrderId").val();
    if (OrderId.toString().length > 0) {
        obj.OrderId = OrderId;
    }
    var OrderStatus = $("#hidStatus").val();
    if (OrderStatus.toString().length > 0) {
        obj.OrderStatus = OrderStatus;
    }
    var groupId = $("#hidGroupId").val();
    if (groupId.toString().length > 0) {
        obj.GroupBuyId = groupId;
    }

    var UserName = $("#txtUserName").val();
    if (UserName.length > 0) {
        obj.UserName = UserName;
    }
    var ProductName = $("#txtProductName").val();
    if (ProductName.length > 0) {
        obj.ProductName = ProductName;
    }
    var ShipTo = $("#txtShopTo").val();
    if (ShipTo.length > 0) {
        obj.ShipTo = ShipTo;
    }
    var StartDate = $("#cldStartDate").val();
    if (StartDate.length > 0) {
        obj.StartDate = StartDate;
    }
    var EndDate = $("#cldEndDate").val();
    if (EndDate.length > 0) {
        obj.EndDate = EndDate;
    }
    var ddlStoreDistribution = $("#ddlStoreDistribution");
    if (ddlStoreDistribution.length > 0) {
        var IsAllotStore = ddlStoreDistribution.val();
        if (IsAllotStore.toString().length > 0) {
            obj.IsAllotStore = IsAllotStore;
        }
    }
    var ddlSearchStore = $("#ddlSearchStore");
    if (ddlSearchStore.length > 0) {
        var StoreId = ddlSearchStore.val();
        if (StoreId.toString().length > 0) {
            obj.StoreId = StoreId;
        }
    }
    var SourceOrder = $("#dropsourceorder").val();
    if (SourceOrder.toString().length > 0) {
        obj.SourceOrder = SourceOrder;
    }

    var orderType = $("#ddlOrderType").val();
    if (orderType.length > 0) {
        obj.OrderType = orderType;
    }
    

    var IsPrinted = $("#ddlIsPrinted").val();
    if (IsPrinted.toString().length > 0) {
        obj.IsPrinted = IsPrinted;
    }
    var RegionId = $("#regionSelectorValue").val();
    if (RegionId.length > 0) {
        obj.RegionId = RegionId;
    }
    var IsMoreSearch = $("#so_more_none").is(":visible");
    obj.IsMoreSearch = IsMoreSearch;
    obj.InvoiceType = $("#dropInvoiceType").val();
    return obj;
}
//参数检测
function checkQuery(obj) {
    var result = true;

    return result;
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
    if (!checkQuery(urldata)) {
        return;
    }

    var loadingobj;
    try {
        target.empty();
        DataNullHelper.hide(datanullbox);
        loadingobj = showLoading(target);
    } catch (e) { }

    $.ajax({
        type: "GET",
        url: dataurl,
        data: urldata,
        dataType: "json",
        success: function (data) {
            var isnodata = true;
            var isshowpage = true;
            var databox = $(target);
            try {
                loadingobj.close();
            } catch (e) { }
            databox.empty();
            DataNullHelper.hide(datanullbox);
            if (data) {
                total = data.total;
                if (total == 0) {
                    isshowpage = false;
                }
                if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));
                    DataRedraw(databox);
                    var isChkAll = $("#checkall").is(':checked');
                    if (isChkAll) {
                        $("input[name='CheckBoxGroup']").iCheck('check');
                    }
                    else {
                        $("input[name='CheckBoxGroup']").iCheck('uncheck');
                    }
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
            try {
                loadingobj.close();
            } catch (e) { }
            ShowMsg("系统内部异常", false);
        }
    });
}
//后续处理
function DataRedraw(databox) {
    $('[data-toggle="tooltip"]').tooltip();
    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red', });
    if (isshowstore) {
        $(".order_title_store").show();
    }
}


function ReloadPageData(pageindex) {
    /*复位*/
    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }
    var _page = showpager;
    var hasPaginator = showpager.HiPaginator("isExist");
    if (hasPaginator) {
        var opts = _page.HiPaginator("getOption");
        curpagesize = opts.pageSize;
        curpageindex = pageindex || opts.currentPage;
    } else {
        curpagesize = 10;
        curpageindex = 1;
    }
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

function ShowMsgAndReloadData(data) {
    msg = "操作成功"
    success = true;
    if (data) {
        if (typeof data == "string") {
            msg = data;
        } else {
            msg = data.msg || msg;
            success = data.success || success;
        }
    }
    ShowMsg(msg, success);
    CloseDialogAndReloadData();
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

function bat_delete() {
    var ids = GetSelectId();
    if (ids.length == 0) {
        alert("请选择数据项");
        return;
    }
    Post_Delete(ids);
}

function Post_Delete(ids) {
    if (ids.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    var candel = false;
    candel = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (candel) {
        var pdata = {
            ids: ids, action: "Delete"
        }
        var loading;
        try {
            loading = showCommonLoading();
        } catch (e) { }
        $.ajax({
            type: "post",
            url: dataurl,
            data: pdata,
            dataType: "json",
            success: function (data) {
                try {
                    loading.close();
                } catch (e) { }
                if (data.success) {
                    ShowMsg(data.message, true);
                    ReloadPageData();
                } else {
                    ShowMsg(data.message, false);
                }
            },
            error: function () {
                try {
                    loading.close();
                } catch (e) { }
            }
        });
    }
}

function Post_ConfirmPay(id) {
    if (id.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    var canoper = false;
    canoper = confirm("如果客户已经通过其他途径支付了订单款项，您可以使用此操作修改订单状态\n\n此操作成功完成以后，订单的当前状态将变为已付款状态，确认客户已付款？");
    if (canoper) {
        var pdata = {
            id: id, action: "ConfirmPay"
        }
        var loading;
        try {
            loading = showCommonLoading();
        } catch (e) { }
        $.ajax({
            type: "post",
            url: dataurl,
            data: pdata,
            dataType: "json",
            success: function (data) {
                try {
                    loading.close();
                } catch (e) { }
                if (data.success) {
                    ShowMsg(data.message, true);
                    ReloadPageData();
                } else {
                    ShowMsg(data.message, false);
                }
            },
            error: function () {
                try {
                    loading.close();
                } catch (e) { }
            }
        });
    }
}

function Post_FinishTrade(OrderId) {
    var pdata = {
        id: OrderId, action: "finishtrade"
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
//取消达达发货订单
function Post_CancelSendGoods(orderId) {
    if (orderId == "") {
        alert("请选择要取消发货的订单");
        return;
    }
    DialogFrame('sales/CancelSendOrderGoods.aspx?OrderId=' + orderId, '取消订单发货', null, null, function () {
        ReloadPageData();
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

function Post_ExportExcel() {
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

function CheckVerification(code) {
    var pdata = {
        VerificationPassword: code, action: "checkverification"
    }
    var flag = true;
    $.ajax({
        type: "post",
        url: dataurl,
        data: pdata,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.success) {
                flag = true;
            } else {
                $("#codeErr").text(data.message);
                flag = false;
            }
        },
        error: function () {
            loading.close();
        }
    });
    return flag;
}