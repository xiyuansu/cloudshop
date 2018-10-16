var curpagesize = 10, curpageindex = 1, total = 0;
var DEFAULT_PAGE_INDEX = 1, DEFAULT_PAGESIZE = 10;
var showbox, dataurl, showpager, datanullbox, datanulldom;

function ToDetail(orderId) {
    var urldata = {
        pagesize: curpagesize,
        pageindex: curpageindex
    }

    initQuery(urldata);

    var listurl = "/admin/Supplier/Order/ManageOrder";
    var url = listurl + "?";
    for (var item in urldata) {
        url += item + "=" + urldata[item] + "&";
    }
    location.href = "/admin/Supplier/OrderDetails?OrderId=" + orderId + "&returnUrl=" + encodeURIComponent(url);
}

//页面初始
$(function () {
    InitParameters();
    InitPage();
    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
});

///参数处理
function InitParameters() {
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
}
//页面初始
function InitPage() {
    //搜索
    $("#btnSearch").on("click", function () {
        ShowListData(showbox, 1, curpagesize, true);
    });

    //导出
    $("#btnExportExcel").on("click", function () {
        Post_ExportExcel();
    });

    //每页显示数量
    $("#pagesize_dropdown").on("change", function () {
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
        $("#hidStatus").val(_d);
        tab_status.removeClass("hover");
        _t.addClass("hover");
        ShowListData(showbox, 1, curpagesize, true);
    });
    var curstatus = $("#hidStatus").val();
    if (curstatus.toString().length > 0) {
        tab_status.removeClass("hover");
        tab_status.each(function () {
            var _t = $(this);
            if (_t.data("status") == curstatus) {
                _t.addClass("hover");
            }
        });
    } else {
        tab_status.eq(0).addClass("hover");
    }


    //关闭订单
    $("#btnCloseOrder").on("click", function () {
        Post_CloseOrder();
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
    var ddlSuppliers = $("#ddlSuppliers");
    if (ddlSuppliers.length > 0) {
        var SupplierId = ddlSuppliers.val();
        if (SupplierId.length > 0) {
            obj.SupplierId = SupplierId;
        }
    }
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
}


function ReloadPageData(pageindex) {
    ResetCheckAll();
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
//复位全选
function ResetCheckAll() {
    try {
        var chkall = $("#checkall");
        if (chkall) {
            if (chkall.iCheck) {
                chkall.iCheck('uncheck');
            }
            chkall.prop("checked", false);
        }
    } catch (e) { }
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