var urlPageIndex = parseInt(getParam("page"));
var urlPageSize = parseInt(getParam("rows"));
if (isNaN(urlPageIndex) || urlPageIndex <= 0) {
    urlPageIndex = 1;
}
else {
    $("#curpageindex").val(urlPageIndex)
}
if (isNaN(urlPageSize) || urlPageSize <= 0) {
    urlPageSize = 10;
}
else {
    $("#pagesize_dropdown").val(urlPageSize);
}
var curpagesize = urlPageSize, curpageindex = urlPageIndex, total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;
var MoveToStoreurl;
var queryParameters;
function getReturnUrl() {
    var listurl = "/admin/product/ProductOnSales";
    var url = listurl + "?";
    if (queryParameters) {
        for (var item in queryParameters) {
            url += item + "=" + queryParameters[item] + "&";
        }
    }
    if (url.lastIndexOf("&") == url.length - 1) {
        url = url.substring(0, url.length - 1);
    }
    return url;
}
///商品编辑页面
function ToEdit(productId) {
    location.href = "EditProduct?productId=" + productId + "&returnUrl=" + encodeURIComponent(getReturnUrl());
}


//页面初始
$(function () {
    //setQuery();
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;


    InitPage();
    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
    //搜索
    $("#btnSearch").on("click", function () {
        curpagesize = parseInt($("#pagesize_dropdown").val());
        ShowListData(showbox, 1, curpagesize, true);
    });
});

function InitPage() {

    //每页显示数量
    $("#pagesize_dropdown").on("change", function () {
        var _t = $(this);
        curpagesize = parseInt(_t.val());
        ShowListData(showbox, curpageindex, curpagesize, false);
        showpager.HiPaginator("option", { pageSize: curpagesize });
        showpager.HiPaginator("redraw");
    });
    FilterTab();
}


//Tab过滤器
function FilterTab() {
    var tab_status = $(".tab_status");
    var filterdom = $("#hidFilter");
    var FILTER_DATA_NAME = "status", ACTION_CLASS_NAME = "hover";
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
    var categoryId = $("#dropCategories").val();
    if (categoryId.length > 0) {
        obj.categoryId = categoryId;
    }
    var productCode = $("#txtSKU").val();
    if (productCode.length > 0) {
        obj.productCode = productCode;
    }

    var BrandId = $("#dropBrandList").val();
    if (BrandId.length > 0) {
        obj.BrandId = BrandId;
    }
    var TagId = $("#dropTagList").val();
    if (TagId.length > 0) {
        obj.TagId = TagId;
    }
    var TypeId = $("#dropType").val();
    if (TypeId.length > 0) {
        obj.TypeId = TypeId;
    }
    var ShippingTemplateId = parseInt($("#dropShippingTemplateId").val());
    if (!isNaN(ShippingTemplateId)) {
        obj.ShippingTemplateId = ShippingTemplateId;
    }
    if ($("#chkIsWarningStock").is(':checked')) {
        obj.isWarning = 1;
    }

    obj.ProductType = $("#ddlProductType").val();

    obj.saleStatus = $("#hidFilter").val();


    return obj;
}
function DataRedraw(databox) {
    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red' });

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
                    if(isChkAll){
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
            loadingobj.close();
            ShowMsg("系统内部异常", false);
        }
    });
}

function OperationButton() {
    var btn = $("#bacthOnsale");
    if (btn.length < 1) {
        $("table tr", showbox).find("th:eq(5)").hide();
        $("table tr", showbox).find("td:eq(5)").hide();
        $("table tr", showbox).find("th:eq(0)").hide();
        $("table tr", showbox).find("td:eq(0)").hide();
    }
}

function ReloadPageData(pageindex) {    /*复位*/    try { var chkall = $("#checkall");  if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { alert(e); }
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

//打开确认移入页面 //打开商品移入窗口
function openMoveFrame(productIds) {

    OpenDialogFrame(MoveToStoreurl + "?ProductIds=" + productIds + "&callback=CloseDialogAndReloadData", {
        title: "商品上架", width: 1000, height: 400, button: [{
            name: '关闭'
        }, {
            name: '保存',
            callback: function () {
                var iframe = this.iframe.contentWindow;
                $("#SaveStock", iframe.document).trigger("click");  //触发保存
                return false;  //关闭动作由callback操作
            },
            focus: true
        }],
        init: function () {
            var iframe = this.iframe.contentWindow;
            //var top = art.dialog.top;
            $("#SaveStock", iframe.document).hide();
        }
    });
}

// 批量上架
function batchMoveToStore() {
    var productIds = GetProductId();
    if (productIds.length == 0) return;
    openMoveFrame(productIds);
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



function bat_delete() {
    var ids = GetProductId();
    if (ids.length < 1) {
        ShowMsg("请选择至少一个商品！", false);
        return;
    }
    post_delete(ids);

}
function deleteModel(id) {
    post_delete(id);
}
function post_delete(ids) {
    var candel = false;
    candel = confirm("确定将商品移入回收站？");
    if (candel) {
        var pdata = {
            ids: ids, action: "delete"
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
var saleOffPdId = 0;
function DownStorePd(pdId) {
    saleOffPdId = pdId;
    formtype = "unsale";
    DialogShow("商品入库", "productOff", "divUnSaleOffFromStore", "btnSaleOff");
   
}
function SaleOff() {
        var loading;
        try {
            loading = showCommonLoading();
        } catch (e) { }
        $.ajax({
            type: "post",
            url: dataurl,
            data: { id: saleOffPdId, action: "dowdPdInStore" },
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
            error: function (msg) {
                ShowMsg(msg, false);
            }
        });
}

$(document).ready(function () {
    $("#dropBatchOperation").bind("change", function () { SelectOperation(); });
});

function confirmimport() {
    if (confirm("同步后门店所有商品的库存会保持和平台一致，是否确认同步?")) {
        $("#bg,.loading").show();
        $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2");
    }
}

function SelectOperation() {
    var Operation = $("#dropBatchOperation").val();
    var productIds = GetProductId();
    if (productIds.length > 0) {
        switch (Operation) {
            case "1":
                formtype = "onsale";
                arrytext = null;
                DialogShow("商品上架", "productonsale", "divOnSaleProduct", "btnUpSale");
                break;
            case "2":
                formtype = "unsale";
                arrytext = null;
                DialogShow("商品下架", "productunsale", "divUnSaleProduct", "btnUnSale");
                break;
            case "3":
                formtype = "instock";
                arrytext = null;
                DialogShow("商品入库", "productinstock", "divInStockProduct", "btnInStock");
                break;
                //case "5":
                //    formtype = "setFreeShip";
                //    arrytext = null;
                //    DialogShow("设置包邮", "setFreeShip", "divSetFreeShip", "ctl00_contentHolder_btnSetFreeShip");
                //    break;
                //case "6":
                //    formtype = "cancelFreeShip";
                //    arrytext = null;
                //    DialogShow("取消包邮", "cancelFreeShip", "divCancelFreeShip", "ctl00_contentHolder_btnCancelFreeShip");
                //    break;
            case "10":
                DialogFrame("product/EditBaseInfo.aspx?callback=CloseDialogAndReloadData&ProductIds=" + productIds, "调整商品基本信息", 1000, null, function (e) { ReloadPageData(); });
                break;
            case "11":
                DialogFrame("product/EditSaleCounts.aspx?callback=CloseDialogAndReloadData&ProductIds=" + productIds, "调整前台显示的销售数量", null, null, function (e) { ReloadPageData(); });
                break;
            case "12":
                DialogFrame("product/EditStocks.aspx?callback=CloseDialogAndReloadData&ProductIds=" + productIds, "调整库存", 880, null, function (e) { ReloadPageData(); });
                break;
            case "13":
                DialogFrame("product/EditMemberPrices.aspx?callback=CloseDialogAndReloadData&ProductIds=" + productIds, "调整会员零售价", 1200, null, function (e) { ReloadPageData(); });
                break;
            case "15":
                DialogFrame("product/EditProductTags.aspx?callback=CloseDialogAndReloadData&ProductIds=" + productIds, "调整商品关联标签", 800, null, function (e) { ReloadPageData(); });
                break;
            case "16":
                formtype = "deduct";
                DialogShow("设置商品分销佣金", "productdeduct", "divDeduct", "ctl00_contentHolder_btnUpdateProductDeducts");
                break;
            case "17":
                DialogFrame("product/EditWarningStocks.aspx?callback=CloseDialogAndReloadData&ProductIds=" + productIds, "调整警戒库存", 880, null, function (e) { ReloadPageData(); });
                break;
            case "18":
                formtype = "instock";
                arrytext = null;
                DialogShow("设置商品跨境", "productinstock", "divOnSetCross_border", "btnSetCrossborder");
                break;
            case "19":
                formtype = "instock";
                arrytext = null;
                DialogShow("取消商品跨境", "productunsale", "divOnOffCross_border", "btnOffCrossborder");
                break;
            case "20":
                formtype = "setshippingtempalte";
                arrytext = null;
                DialogFrame("product/SetTemplates.aspx?callback=CloseDialogAndReloadData&ProductIds=" + productIds, "批量设置运费模板", 600, 300, function (e) { ReloadPageData(); });
                break;
        }
    }
    $("#dropBatchOperation").val("");
}



function CollectionProduct(url) {
    DialogFrame("product/" + url, "相关商品");
}

function ShowStoreStock(url) {
    DialogFrame("product/" + url, "门店库存", 900, 600);
}

function validatorForm() {
    switch (formtype) {
        case "tag":
            if ($("#ctl00_contentHolder_txtProductTag").val().replace(/\s/g, "") == "") {
                alert("请选择商品标签");
                return false;
            }
            break;
        case "onsale":
        case "unsale":
        case "instock":
        case "setFreeShip":
        case "cancelFreeShip":
            setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
            break;
        case "deduct":
            if ($("#ctl00_contentHolder_txtSecondLevelDeduct").val() == "") {
                alert("请输入会员上二级抽佣比例");
                return false;
            }
            if ($("#ctl00_contentHolder_txtSubMemberDeduct").val() == "") {
                alert("请输入会员直接上级抽佣比例");
                return false;
            }
            if ($("#ctl00_contentHolder_txtThreeLevelDeduct").val() == "") {
                alert("请输入会员上三级抽佣比例");
                return false;
            }
            setArryText('ctl00_contentHolder_txtSecondLevelDeduct', $("#ctl00_contentHolder_txtSecondLevelDeduct").val());
            setArryText('ctl00_contentHolder_txtSubMemberDeduct', $("#ctl00_contentHolder_txtSubMemberDeduct").val());
            setArryText('ctl00_contentHolder_txtThreeLevelDeduct', $("#ctl00_contentHolder_txtThreeLevelDeduct").val());
            break;
    };
    return true;
}



function post_ChageStatus(status, action) {
    var ids = GetProductId();
    if (ids.length < 1) {
        ShowMsg("请选择至少一个商品！", false);
        return;
    }
    var pdata = {
        ids: ids, action: action, SaleStatus: status
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
//批量上下架入库
function ChageStatus(status) {
    post_ChageStatus(status, "chagestatus");
}

//批量设置商品是否跨境
function UpdateCrossborder(flag) {
    var ids = GetProductId();
    if (ids.length < 1) {
        ShowMsg("请选择至少一个商品！", false);
        return;
    }
    var pdata = {
        ids: ids, action: "updateCrossborder", Crossborderstatus: flag
    }
    var loading;
    try {
        loading = showCommonLoading();
    } catch (e) { }
    $.ajax({
        url: dataurl,
        type: 'post',
        data: pdata,
        dataType: 'json',
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
    })
}

