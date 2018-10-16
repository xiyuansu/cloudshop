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

    $("#datashow").on("change", ".dropAddToCategories", function () {
        var _t = $(this);
        var pid = _t.data("pid");
        var extendIndex = _t.parents("td").eq(0).find(".dropExtendIndex").val();
        var dropCategories =_t.val();
        if (extendIndex == 0) {
            ShowMsg("请选择要设置的扩展分类", false);
        }
        if (extendIndex < 1 || extendIndex > 5) {
            extendIndex = 1;
        }

        ChangeAddToCategorie(pid, extendIndex, dropCategories);

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
    var EndDate = $("#calendarEndDate").val();
    if (EndDate.length > 0) {
        obj.EndDate = EndDate;
    }
    var BrandId = $("#dropBrandList").val();
    if (BrandId.length > 0) {
        obj.BrandId = BrandId;
    }
    var TypeId = $("#dropType").val();
    if (TypeId.length > 0) {
        obj.TypeId = TypeId;
    }


    return obj;
}
function DataRedraw(databox) {

    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red' });
    $(".b_excate").each(function (index) {
        var _t = $(this);
        var pid = _t.data("pid");
        var _d = $("#dropCategories").clone();
        _d.removeAttr("id").removeAttr("name").attr("data-pid", pid);
        _d.addClass("iselect dropAddToCategories");
        _d.find("option[value='0']").remove();
        _t.append(_d);
    });
    $(".hidSelectIndex").each(function (index) {
        var _t = $(this);
        var _d = _t.parent().find(".dropExtendIndex");
        _d.val(_t.val());
    });
  
    $('select.iselect').easyDropDown();

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
            databox.empty();
            DataNullHelper.hide(datanullbox);
            if (data) {
                total = data.total;
                if (total == 0) {
                    isshowpage = false;
                }
                if (data.rows.length > 0) {
                    isnodata = false;
                    template.config("escape", false);
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
function deleteModel() {
    var id = $("#currentProductId").val();
    post_delete(id);
}


function post_delete(ids) {
    var candel = false;
    candel = confirm("确定要把商品移入回收站吗？");
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

function bat_AddToCategorie() {
    var ids = GetProductId();
    if (ids.length < 1) {
        ShowMsg("请选择至少一个商品！", false);
        return;
    }
    var extendIndex = $("#drop_ExtendIndex").val();
    var dropCategories = $("#dropAddToAllCategories").val();
    if (extendIndex == 0) {
        ShowMsg("请选择要设置的扩展分类", false);
    }
    if (extendIndex < 1 || extendIndex > 5) {
        extendIndex = 1;
    }
    ChangeAddToCategorie(ids, extendIndex, dropCategories);
}

function ChangeAddToCategorie(ids, extendIndex, dropCategories) {


    var pdata = {
        ids: ids, action: "addtocategorie", dropIndex: extendIndex, dropCategories: dropCategories
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

//删除扩展分类
$(document).ready(function (e) {
    $(".datalist").on("click", "samp.extend i", function (e) {
        var productId = parseInt($(this).parent().attr("productId"));
        var index = parseInt($(this).parent().attr("index"));
        var sampId = $(this).parent().attr("id");
        if (!isNaN(productId) && !isNaN(index)) {
            $.ajax({
                url: "/Admin/Admin.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { action: "delextendcategory", productId: productId, extendIndex: index },
                async: false,
                success: function (resultData) {

                    if (resultData.status == "1") {
                        var indexChinese = ["一", "二", "三", "四", "五"];
                        $("#" + sampId).html("第" + indexChinese[index - 1] + "分类");
                        $("#" + sampId).removeClass("extend" + index).addClass("nocontent");
                        //$(this).remove();
                        //$(this).parent().remove();
                        $("#" + sampId).remove();
                    }
                    else {
                        alert(resultData.msg);
                        return false;
                    }
                },
                error: function () {
                    // alert('操作错误,请与系统管理员联系!');
                }
            });
        }

    });
});


function MoveCategory() {
    var Categories = $("#dropMoveToCategories").val();
    var ids = GetProductId();
    if (ids.length <= 0) {
        ShowMsg("请选择要转移的商品", false);
        return;
    }
    var pdata = {
        ids: ids, action: "movecategory", Categories: Categories
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