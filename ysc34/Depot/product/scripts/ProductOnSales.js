var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager,datanullbox,datanulldom;
var MoveToStoreurl;


//页面初始
$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
datanullbox = showbox;
    
    MoveToStoreurl = $("#MoveToStoreurl").val();
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
    $('#startDate').datetimepicker({
        minView: 2, format: 'yyyy-mm-dd', language: 'zh-CN', weekStart: 1,
        todayBtn: 1, autoclose: 1, todayHighlight: 1, startView: 2,
        forceParse: 0, showMeridian: 1,
    });
    $('#endDate').datetimepicker({
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
    var startDate = $("#startDate").val();
    if (startDate.length > 0) {
        obj.StartDate = startDate;
    }
    var endDate = $("#endDate").val();
    if (endDate.length > 0) {
        obj.EndDate = endDate;
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
            databox.empty();DataNullHelper.hide(datanullbox);
            if (data) {
                total = data.total;
                if (total == 0) {
                    isshowpage = false;
                }
                if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));
                    OperationButton();
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

function OperationButton()
{
    var btn = $("#bacthOnsale");
    if (btn.length <1) {
        $("table tr", showbox).find("th:eq(5)").hide();
        $("table tr", showbox).find("td:eq(5)").hide();
        $("table tr", showbox).find("th:eq(0)").hide();
        $("table tr", showbox).find("td:eq(0)").hide();
    }
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

//打开确认移入页面 //打开商品移入窗口
function openMoveFrame(productIds) {

    OpenDialogFrame(MoveToStoreurl + "?ProductIds=" + productIds + "&callback=SuccessAndCloseReload", {
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

function SuccessAndCloseReload(msg) {
    msg = msg || "操作成功";
    ShowMsg(msg, true);
    CloseDialogAndReloadData();
}