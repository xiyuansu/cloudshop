var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;
var adjustStockurl, adjustWarningStockurl, storeStockAdjustLogurl, adjustSalePriceurl;

var canModifyPrice = false, canModifyStock = false;

//页面初始
$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;

    adjustStockurl = $("#adjustStockurl").val();
    adjustWarningStockurl = $("#adjustWarningStockurl").val();
    adjustSalePriceurl = $("#adjustSalePriceurl").val();
    storeStockAdjustLogurl = $("#storeStockAdjustLogurl").val();
    curpagesize = parseInt($("#pagesize_dropdown").val());
    canModifyPrice = $("#hidIsModifyPrice").val() == "1";
    canModifyStock = $("#hidIsShelvesProduct").val() == "1";
    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
    //每页显示数量
    $("#pagesize_dropdown").on("change", function () {
        var _t = $(this);
        curpagesize = parseInt(_t.val());
        ShowListData(showbox, curpageindex, curpagesize, false);
        showpager.HiPaginator("option", { pageSize: curpagesize });
        showpager.HiPaginator("redraw");
    });

    FilterTab();

    //搜索
    $("#btnSearch").on("click", function () {
        curpagesize = parseInt($("#pagesize_dropdown").val());
        ShowListData(showbox, 1, curpagesize, true);
    });
    //批量下架
    $("#btnBatDelete").click(function () {
        var proids = GetProductId();
        if (proids.length > 0) {
            DeleteProduct(proids);
        }
    });

    //下架操作
    showbox.on("click", ".downshelf", function () {
        var _t = $(this);
        var id = _t.data("id");
        DeleteProduct(id);
    })
});

//Tab过滤器
function FilterTab() {
    var tab_status = $(".tab_status");
    var filterdom = $("#hidIsWarning");
    var FILTER_DATA_NAME = "filter", ACTION_CLASS_NAME = "hover";
    if (filterdom && tab_status.length > 0) {
        tab_status.on("click", function () {
            var _t = $(this);
            var _d = _t.data(FILTER_DATA_NAME);
            filterdom.val(_d);
            tab_status.removeClass(ACTION_CLASS_NAME);
            _t.addClass(ACTION_CLASS_NAME);
            ShowListData(showbox, 1, curpagesize, true);
        });
        var curstatus = filterdom.val();
        if (curstatus && curstatus.length > 0) {
            tab_status.removeClass(ACTION_CLASS_NAME);
            tab_status.each(function () {
                var _t = $(this);
                if (_t.data(FILTER_DATA_NAME) == curstatus) {
                    _t.addClass(ACTION_CLASS_NAME);
                }
            });
        } else {
            tab_status.eq(0).addClass(ACTION_CLASS_NAME);
        }
    }
}

//搜索参数
function initQuery(obj) {
    var ProductName = $("#txtSearchText").val();
    if (ProductName.length > 0) {
        obj.ProductName = ProductName;
    }
    var productCode = $("#txtProductCode").val();
    if (productCode.length > 0) {
        obj.productCode = productCode;
    }
    var categoryId = $("#dropCategories").val();
    if (categoryId.length > 0) {
        obj.categoryId = categoryId;
    }
    var isWarning = $("#hidIsWarning").val();
    if (isWarning.length > 0) {
        obj.isWarning = isWarning;
    }
    obj.ProductType = $("#ddlProductType").val();
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
                    ShowOperationButton();
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


function ReloadPageData(pageindex) {    /*复位*/    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }
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
function ShowOperationButton() {
    if (canModifyPrice) {
        $(".btb_saleprice").removeClass("hidden");
    }
    if (canModifyStock) {
        $(".btb_stock").removeClass("hidden");
        $(".btb_warningstock").removeClass("hidden");
    }
}

//商品下架
function DeleteProduct(ids) {
    if (confirm('确定要把这些商品下架吗？')) {
        $.ajax({
            type: "post",
            url: dataurl,
            data: { action: "delete", productids: ids },
            dataType: "json",
            success: function (data) {
                if (data) {
                    if (data.success) {
                        ShowMsg("下架操作完成", true);
                    } else {
                        ShowMsg(data.message, false);
                    }
                }
                ReloadPageData();
            },
            error: function () {
                ShowMsg("系统内部异常", false);
            }
        });
    }
}

//批量修改库存
function batchAdjustStore() {
    var productIds = GetProductId();
    if (productIds.length == 0) {
        return;
    }
    openAdjustStockPage(productIds);
}
//调整库存页面
function openAdjustStockPage(productIds) {
    OpenDialogFrame(adjustStockurl + "?ProductIds=" + productIds + "&callback=SuccessAndCloseReload", {
        title: "调整库存", width: 880, height: 300, button: [{
            name: '关闭'
        }, {
            name: '保存',
            callback: function () {
                var iframe = this.iframe.contentWindow;
                $("#btnSaveStock", iframe.document).trigger("click");  //触发保存
                return false;  //关闭动作由callback操作
            },
            focus: true
        }],
        init: function () {
            var iframe = this.iframe.contentWindow;
            //var top = art.dialog.top;
            $("#btnSaveStock", iframe.document).hide();
        }
    });
}

//批量修改警戒库存
function batchAdjustWarningStore() {
    var productIds = GetProductId();
    if (productIds.length == 0) {
        return;
    }

    openAdjustWarningStockPage(productIds);
}

function batchAdjustSalePrice() {
    var productIds = GetProductId();
    if (productIds.length == 0) {
        return;
    }

    openAdjustSalePrice(productIds);
}

function openAdjustSalePrice(productIds) {
    OpenDialogFrame(adjustSalePriceurl + "?ProductIds=" + productIds + "&callback=SuccessAndCloseReload", {
        title: "调整门店售价", width: 880, height: 300, button: [{
            name: '关闭'
        }, {
            name: '保存',
            callback: function () {
                var iframe = this.iframe.contentWindow;
                $("#btnSaveSalePrice", iframe.document).click();  //触发保存
                return false;  //关闭动作由callback操作
            },
            focus: true
        }],
        init: function () {
            var iframe = this.iframe.contentWindow;
            //var top = art.dialog.top;
            $("#btnSaveSalePrice", iframe.document).hide();
        }
    });
}

//调整库存页面
function openAdjustWarningStockPage(productIds) {
    OpenDialogFrame(adjustWarningStockurl + "?ProductIds=" + productIds + "&callback=SuccessAndCloseReload", {
        title: "调整警戒库存", width: 880, height: 300, button: [{
            name: '关闭'
        }, {
            name: '保存',
            callback: function () {
                var iframe = this.iframe.contentWindow;
                $("#btnSaveWarningStock", iframe.document).trigger("click");  //触发保存
                return false;  //关闭动作由callback操作
            },
            focus: true
        }],
        init: function () {
            var iframe = this.iframe.contentWindow;
            //var top = art.dialog.top;
            $("#btnSaveWarningStock", iframe.document).hide();
        }
    });
}

//打开库存明细页面
function openAdjustStockLog(productId) {
    DialogFrame(storeStockAdjustLogurl + "?ProductId=" + productId, "库存明细", 880, 560, null);
}

//获取选中的商品ID
function GetProductId() {
    var v_str = "";

    $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
        v_str += $(rowItem).attr("value") + ",";
    });

    if (v_str.length == 0) {
        alert("请选择商品");
        return "";
    }
    return v_str.substring(0, v_str.length - 1);
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