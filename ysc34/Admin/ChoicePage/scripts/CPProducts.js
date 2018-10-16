var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom, datanulldom;
var DEFAULT_PAGE_INDEX = 1, DEFAULT_PAGESIZE = 10;


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
    //每页显示数量
    $("#pagesize_dropdown").on("change", function () {
        var _t = $(this);
        curpagesize = parseInt(_t.val());
        ShowListData(showbox, curpageindex, curpagesize, false);
        showpager.HiPaginator("option", { pageSize: curpagesize });
        showpager.HiPaginator("redraw");
    });
}

//Tab过滤器
function FilterTab() {
    var tab_status = $(".tab_status");
    var filterdom = $("#hidFilter");
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
    var TagId = $("#dropTagList").val();
    if (TagId.length > 0) {
        obj.TagId = TagId;
    }
    var IsHasStock = $("#hidIsHasStock").val();
    if (IsHasStock && IsHasStock.length > 0) {
        obj.IsHasStock = IsHasStock;
    }
    var ProductCode = $("#hidProductCode ").val();
    if (ProductCode && ProductCode.length > 0) {
        obj.ProductCode = ProductCode;
    }
    var StartDate = $("#hidStartDate").val();
    if (StartDate.length > 0) {
        obj.StartDate = StartDate;
    }
    var TypeId = $("#hidTypeId").val();
    if (TypeId.length > 0) {
        obj.TypeId = TypeId;
    }
    var SaleStatus = $("#hidSaleStatus").val();
    if (SaleStatus.length > 0) {
        obj.SaleStatus = SaleStatus;
    }
    var EndDate = $("#hidEndDate").val();
    if (EndDate.length > 0) {
        obj.EndDate = EndDate;
    }
    var IsWarningStock = $("#hidIsWarningStock").val();
    if (IsWarningStock.length > 0) {
        obj.IsWarningStock = IsWarningStock;
    }
    var FilterProductIds = $("#hidFilterProductIds").val();
    if (FilterProductIds.length > 0) {
        obj.FilterProductIds = FilterProductIds;
    }
    var IsFilterFightGroupProduct = $("#hidIsFilterFightGroupProduct").val();
    if (IsFilterFightGroupProduct.length > 0) {
        obj.IsFilterFightGroupProduct = IsFilterFightGroupProduct;
    }
    var IsFilterBundlingProduct = $("#hidIsFilterBundlingProduct").val();
    if (IsFilterBundlingProduct.length > 0) {
        obj.IsFilterBundlingProduct = IsFilterBundlingProduct;
    }
    var IsFilterPromotionProduct = $("#hidIsFilterPromotionProduct").val();
    if (IsFilterPromotionProduct.length > 0) {
        obj.IsFilterPromotionProduct = IsFilterPromotionProduct;
    }
    var IsFilterCountDownProduct = $("#hidIsFilterCountDownProduct").val();
    if (IsFilterCountDownProduct.length > 0) {
        obj.IsFilterCountDownProduct = IsFilterCountDownProduct;
    }
    var IsFilterGroupBuyProduct = $("#hidIsFilterGroupBuyProduct").val();
    if (IsFilterGroupBuyProduct.length > 0) {
        obj.IsFilterGroupBuyProduct = IsFilterGroupBuyProduct;
    }
    var NotInCombinationMainProduct = $("#hidNotInCombinationMainProduct").val();
    if (NotInCombinationMainProduct.length > 0) {
        obj.NotInCombinationMainProduct = NotInCombinationMainProduct;
    }
    var NotInPreSaleProduct = $("#hidNotInPreSaleProduct").val();
    if (NotInPreSaleProduct.length > 0) {
        obj.NotInPreSaleProduct = NotInPreSaleProduct;
    }
    var NotInCombinationOtherProduct = $("#hidNotInCombinationOtherProduct").val();
    if (NotInCombinationOtherProduct.length > 0) {
        obj.NotInCombinationOtherProduct = NotInCombinationOtherProduct;
    }

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
    var DATA_TEMPLATE_DOM_NAME = "datatmpl";
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
                    databox.html(template(DATA_TEMPLATE_DOM_NAME, data));
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
                            curpagesize = showpager.HiPaginator("getPagesize");
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
    try {
        $('[data-toggle="tooltip"]').tooltip();
        $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red', });
    } catch (e) { }
}

function ReloadPageData(pageindex) {
    ResetCheckAll();
    var _page = showpager;
    var opts = _page.HiPaginator("getOption");
    curpagesize = opts.pageSize;
    curpageindex = pageindex || opts.currentPage;
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