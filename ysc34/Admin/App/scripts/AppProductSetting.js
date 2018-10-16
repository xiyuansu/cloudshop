var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;


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
        if (showpager.HiPaginator("isExist")) {
            showpager.HiPaginator("option", { pageSize: curpagesize });
            showpager.HiPaginator("redraw");
        }
    });

});

function ReloadPageData(pageindex) {/*复位*/    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }
    if (showpager.HiPaginator("isExist")) {
        var _page = showpager;
        var opts = _page.HiPaginator("getOption");
        curpagesize = opts.pageSize;
        curpageindex = pageindex || opts.currentPage;
    }
    ShowListData(showbox, curpageindex, curpagesize, true);
}

function ShowListData(target, page, pagesize, initpagination) {
    page = page || 1;
    pagesize = pagesize || 10;
    if (typeof (initpagination) == "undefined") { initpagination = true; }
    var urldata = {
        page: page, rows: pagesize, action: "getlist"
    }

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


function DataRedraw(databox) {
    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red' });
    /*复位*/
    try {
        var chkall = $("#checkall");
        if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); }
    } catch (e) { }

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


function bat_delete()
{
    var ids = GetSelectId();
    if (ids.length < 1) {
        ShowMsg("请选择至少一个商品！", false);
        return;
    }
    Post_Deletes(ids);
}


function Post_Deletes(ids) {
    
    var candel = false;
    candel = confirm("确定要执行该删除操作吗？");
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

 
$("#datashow").on("blur", ".txtdisplay", function () {
    var _v = $(this).val();
    if (_v.length<=0) {
        ShowMsg("排序不能为空！", false);
        return;
    }
    var _old = $(this).data("oldvalue");
    if (_old==_v) {
        return;
    }
    var ProductId = $(this).data("id");
    var pdata = {
        ProductId: ProductId, action: "saveorder", Value: _v
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
});

function AddProduct(ids) {
    if (ids.length < 1) {
        ShowMsg("请选择至少一个商品！", false);
        return;
    }
    var pdata = {
        ids: ids, action: "addproduct"
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

 