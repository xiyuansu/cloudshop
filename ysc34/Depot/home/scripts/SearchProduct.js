var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;



//页面初始
$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;

    curpagesize = parseInt($("#pagesize_dropdown").val());
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
    //搜索
    $("#btnSearch").on("click", function () {
        //  curpagesize = parseInt($("#pagesize_dropdown").val());
        ShowListData(showbox, 1, curpagesize, true);
    });

});

function ReloadPageData(pageindex) {/*复位*/    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }
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


//搜索参数
function initQuery(obj) {
    var Keywords = $("#txtSearchText").val();
    if (Keywords.length > 0) {
        obj.Keywords = Keywords;
    }
    var CategoryId = $("#dropCategories").val();
    if (CategoryId.length > 0) {
        obj.CategoryId = CategoryId;
    }
    var BrandId = $("#dropBrandList").val();
    if (BrandId.length > 0) {
        obj.BrandId = BrandId;
    }
    var filterProductIds = $("#hidFilterProductIds").val();
    if (filterProductIds.length > 0) {
        obj.FilterProductIds = filterProductIds;
    }
    var IsIncludeHomeProduct = $("#hidIsIncludeHomeProduct").val();
    if (IsIncludeHomeProduct.length > 0) {
        obj.IsIncludeHomeProduct = IsIncludeHomeProduct;
    }
    var IsIncludeAppletProduct = $("#hidIsIncludeAppletProduct").val();
    if (IsIncludeAppletProduct.length > 0) {
        obj.IsIncludeAppletProduct = IsIncludeAppletProduct;
    }
    var ProductType = $("#hidProductType").val();
    if (ProductType.length > 0) {
        obj.ProductType = ProductType;
    }
    return obj;
}

function GetAllProductIds() {
    var urldata = {
        action: "getAllProducts"
    }
    initQuery(urldata);
    $.ajax({
        type: "GET",
        url: dataurl,
        data: urldata,
        dataType: "json",
        success: function (data) {
            var proStr = "";
            var origin = artDialog.open.origin;
            if (data.Products.length > 0) {
                $.each(data.Products, function (idx, obj) {
                    proStr += obj.ProductId + "|||" + obj.ProductName + (idx == data.Products.length - 1 ? "" : ",,,");
                });
            }
            if (origin.document.getElementById("ctl00_contentHolder_hidSelectProducts") != null && origin.document.getElementById("ctl00_contentHolder_hidSelectProducts") != undefined) {
                $(origin.document.getElementById("ctl00_contentHolder_hidSelectProducts")).val(proStr);
            }
            if (origin.document.getElementById("hidSelectProducts") != null && origin.document.getElementById("hidSelectProducts") != undefined) {
                $(origin.document.getElementById("hidSelectProducts")).val(proStr);
            }
            art.dialog.close();
        },
        error: function () {
            loadingobj.close();
            ShowMsg("系统内部异常", false);
        }
    });
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
            DataNullHelper.hide(datanullbox);
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
                        first: '',
                        prev: '<a href="javascript:;" class="page-prev">上一页</a>',
                        next: '<a href="javascript:;" class="page-next">下一页</a>',
                        page: '',
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
        error: function (r) {
            loadingobj.close();
            ShowMsg(r.responseText, false);
        }
    });
}



function DataRedraw(databox) {
    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red' });

}


