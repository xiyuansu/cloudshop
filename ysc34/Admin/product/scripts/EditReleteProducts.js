var showbox, dataurl, showpager, pagesize = 5;
var showbox2, showpager2, pagesize2 = 5;
var curProductId;

//页面初始
$(function () {
    InitPage();
});

//页面初始
function InitPage() {

    showbox = $("#datashow");
    showpager = $("#showpager");

    showbox2 = $("#datashow2");
    showpager2 = $("#showpager2");

    dataurl = $("#dataurl").val();
    curProductId = $("#hidCurProductId").val();

    //初始数据显示
    showbox.QWRepeater({
        tmplId: "#datatmpl",
        url: dataurl,
        urlParameter: {
            page: 1, rows: pagesize, action: "getproductlist"
        },
        external: true,
        call_beforeLoadData: function (o) {
            o.urlParameter = initQuery(o.urlParameter);
        },
        event_drawed: function (t) {
            var opts = t.options;
            var exdata = showbox.QWRepeater("external");
            if (exdata) {
                //初始分页组件
                if (opts.data.total > 0) {
                    showpager.HiPaginator({
                        totalCounts: opts.data.total,
                        pageSize: pagesize,
                        currentPage: opts.urlParameter.page,
                        first: '',
                        prev: '<a href="javascript:;" class="page-prev">上一页</a>',
                        next: '<a href="javascript:;" class="page-next">下一页</a>',
                        page: '',
                        last: '',
                        visiblePages: 0,
                        disableClass: 'hide',
                        activeClass: 'page-cur',
                        onPageChange: function (pageindex, type) {
                            if (type != "init") {
                                showbox.QWRepeater("external", false);
                                showbox.QWRepeater("reloadUrl", { page: pageindex });
                            }
                        }
                    });
                } else {
                    if (showpager.HiPaginator("isExist")) {
                        showpager.HiPaginator("destroy");
                    }
                }
            }
        }
    });

    showbox2.QWRepeater({
        tmplId: "#datatmpl2",
        url: dataurl,
        urlParameter: {
            page: 1, rows: pagesize2, action: "getrelatedlist", id: curProductId
        },
        external: true,
        event_drawed: function (t) {
            var opts = t.options;
            var exdata = showbox2.QWRepeater("external");
            if (exdata) {
                //初始分页组件
                if (opts.data.total > 0) {
                    showpager2.HiPaginator({
                        totalCounts: opts.data.total,
                        pageSize: pagesize2,
                        currentPage: opts.urlParameter.page,
                        first: '',
                        prev: '<a href="javascript:;" class="page-prev">上一页</a>',
                        next: '<a href="javascript:;" class="page-next">下一页</a>',
                        page: '',
                        last: '',
                        visiblePages: 0,
                        disableClass: 'hide',
                        activeClass: 'page-cur',
                        onPageChange: function (pageindex, type) {
                            if (type != "init") {
                                showbox2.QWRepeater("external", false);
                                showbox2.QWRepeater("reloadUrl", { page: pageindex });
                            }
                        }
                    });
                } else {
                    if (showpager2.HiPaginator("isExist")) {
                        showpager2.HiPaginator("destroy");
                    }
                }
            }
        }
    });

    //搜索
    $("#btnSearch").on("click", function () {
        showbox.QWRepeater("external", true);
        showbox.QWRepeater("reloadUrl", { page: 1 });
    });
    $("#txtSearchText").on("keypress", function (event) {
        if (event.keyCode == 13) {
            $("#btnSearch").trigger("click");
            event.returnValue = false;
            event.stopPropagation();
            event.preventDefault();
            return false;
        }
    });
}

function ReloadPageData(pageindex) {
    var urldata = {};
    if (pageindex) {
        urldata.page = pageindex;
    }
    showbox.QWRepeater("external", true);
    showbox.QWRepeater("reloadUrl", urldata);
}

function ReloadPageData2(pageindex) {
    var urldata = {};
    if (pageindex) {
        urldata.page = pageindex;
    }
    showbox2.QWRepeater("external", true);
    showbox2.QWRepeater("reloadUrl", urldata);
}

function ReloadPageAllListData(pageindex) {
    ReloadPageData(pageindex);
    ReloadPageData2(pageindex);
}
function initQuery(obj) {
    obj.Keywords = $("#txtSearchText").val();
    obj.CategoryId = $("#dropCategories").val();
    obj.FilterProductIds = $("#hidFilterProductIds").val();
    return obj;
}

function Post_Delete(rid) {
    if (rid.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    var pdata = {
        id: curProductId, relatedId: rid, action: "Delete"
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
                InitRelatedIds();
                ShowMsg(data.message, true);
                ReloadPageAllListData();
            } else {
                ShowMsg(data.message, false);
            }
        },
        error: function () {
            loading.close();
        }
    });
}

function Post_Clear() {
    var pdata = {
        id: curProductId, action: "Clear"
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
                InitRelatedIds();
                ShowMsg(data.message, true);
                ReloadPageAllListData();
            } else {
                ShowMsg(data.message, false);
            }
        },
        error: function () {
            loading.close();
        }
    });
}


function Post_Add(rid) {
    if (rid.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    var pdata = {
        id: curProductId, relatedId: rid, action: "Add"
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
                InitRelatedIds();
                ShowMsg(data.message, true);
                ReloadPageAllListData();
            } else {
                ShowMsg(data.message, false);
            }
        },
        error: function () {
            loading.close();
        }
    });
}

function Post_AddSearch() {
    var pdata = {
        id: curProductId, action: "AddSearch"
    }
    initQuery(pdata);
    var loading = showCommonLoading();
    $.ajax({
        type: "post",
        url: dataurl,
        data: pdata,
        dataType: "json",
        success: function (data) {
            loading.close();
            if (data.success) {
                InitRelatedIds();
                ShowMsg(data.message, true);
                ReloadPageAllListData();
            } else {
                ShowMsg(data.message, false);
            }
        },
        error: function () {
            loading.close();
        }
    });
}

function InitRelatedIds() {
    var pdata = {
        id: curProductId, action: "GetRelatedIds"
    }
    initQuery(pdata);
    var loading = showCommonLoading();
    $.ajax({
        type: "post",
        url: dataurl,
        data: pdata,
        async: false,
        success: function (data) {
            loading.close();
            if (data) {
                $("#hidFilterProductIds").val(data);
            } else {
                $("#hidFilterProductIds").val("");

            }
        },
        error: function () {
            loading.close();
        }
    });
}