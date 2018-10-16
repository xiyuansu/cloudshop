var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;
var MoveToStoreurl;


//页面初始
$(function () {
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

    var StartDate = $("#calendarStartDate").val();
    if (StartDate.length > 0) {
        obj.StartDate = StartDate;
    }
    var EndDate = $("#calendarEndDates").val();
    if (EndDate.length > 0) {
        obj.EndDate = EndDate;
    }
    var IsMakeTaobao = $("#dpTaoBao").val();
    if (IsMakeTaobao.length > 0) {
        obj.IsMakeTaobao = IsMakeTaobao;
    }

    if ($("#chkOnSales").is(':checked')) {
        obj.includeOnSales = true;
    }
    if ($("#chkInStock").is(':checked')) {
        obj.IncludeInStock = true;
    }
    if ($("#chkUnSales").is(':checked')) {
        obj.IncludeUnSales = true;
    }

    if (!obj.includeOnSales && !obj.IncludeInStock & !obj.IncludeUnSales) {
        ShowMsg("至少选择一种商品状态！", false);
    }

    obj.RemoveProductId = $("#RemoveProductId").val();

    return obj;
}
function DataRedraw(databox) {
    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red' });
    $("#lblTotals").html(total);
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





function post_delete(ids) {

    var RemoveProductId = $("#RemoveProductId").val();
    if (RemoveProductId.length <= 0) {
        RemoveProductId = ids;
    } else {

        RemoveProductId += "," + ids;
    }
    $("#RemoveProductId").val(RemoveProductId);

    ReloadPageData();

}





//导出
function Export() {
    var url = dataurl + "?";

    var adapterName = $("#dropExportVersions").val();
    var exportStock = $("#ctl00_contentHolder_chkExportStock input").is(":checked");
    if (adapterName.length <= 0) {

        ShowMsg("请选择一个导出版本", false);
        return;
    }
    var urldata = {
        action: "export", adapterName: adapterName, exportStock: exportStock
    }
    initQuery(urldata);
    for (var item in urldata) {
        url += item + "=" + urldata[item] + "&";
    }
    window.open(url);
}