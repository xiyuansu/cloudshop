var showbox, dataurl, showpager, datanullbox, datatmpname, pagesize = 10, listurl;
var showbox2, dataurl2, showpager2, datanullbox2, datatmpname2, pagesize2 = 10, listurl2;
var curArticId;


//页面初始
$(function () {
    InitParameters();
    InitPage();
    //初始数据显示
    ShowListData(showbox, listurl, datatmpname, showpager, datanullbox, 1, pagesize, true);

    ShowListData(showbox2, listurl2, datatmpname2, showpager2, datanullbox2, 1, pagesize2, true);
});

///参数处理
function InitParameters() {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;//.parents(".datalist");
    datatmpname = "datatmpl";
    listurl = dataurl + "?action=getproductlist"

    curArticId = $("#hidCurArticId").val();
    showbox2 = $("#datashow2");
    showpager2 = $("#showpager2");
    dataurl2 = $("#dataurl2").val();
    datanullbox2 = showbox2;//.parents(".datalist");
    datatmpname2 = "datatmpl2";
    listurl2 = dataurl2 + "?action=getrelatedlist&id=" + curArticId;
}
//页面初始
function InitPage() {
    //搜索
    $("#btnSearch").on("click", function () {
        ShowListData(showbox, listurl, datatmpname, showpager, datanullbox, 1, pagesize, true);
    });
    $("#txtSearchText").on("keypress", function (event) {
        if (event.keyCode == 13) {
            $("#btnSearch").trigger("click");
            event.returnValue = false;
        }
    });
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
    var FilterProductIds = $("#hidFilterProductIds").val();
    if (FilterProductIds.length > 0) {
        obj.FilterProductIds = FilterProductIds;
    }
    return obj;
}




function ShowListData(target, url, tmpname, pageContainer, nullContainer, page, pagesize, initpagination) {
    page = page || 1;
    pagesize = pagesize || 10;
    if (typeof (initpagination) == "undefined") { initpagination = true; }
    var urldata = {
        page: page, rows: pagesize
    }
    initQuery(urldata);

    var loadingobj;
    loadingobj = showLoading(target);
    target.empty();
    DataNullHelper.hide(nullContainer);

    $.ajax({
        type: "post",
        url: url,
        data: urldata,
        dataType: "json",
        success: function (data) {
            var total = 0;
            var isnodata = true;
            var isshowpage = true;
            var databox = $(target);
            loadingobj.close();
            if (data) {
                total = data.total;
                if (total == 0) {
                    isshowpage = false;
                }
                if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template(tmpname, data));
                }
            }
            if (isnodata) {
                //total = 0;
                DataNullHelper.show(nullContainer);
            }
            if (initpagination) {
                //初始分页组件
                if (isshowpage) {
                    pageContainer.HiPaginator({
                        totalCounts: total,
                        pageSize: pagesize,
                        currentPage: page,
                        first: '',
                        prev: '<a href="javascript:;" class="page-prev">上一页</a>',
                        next: '<a href="javascript:;" class="page-next">下一页</a>',
                        page: '',
                        last: '',
                        visiblePages: 0,
                        disableClass: 'hide',
                        activeClass: 'page-cur',
                        onPageChange: function (pageindex, type) {
                            return function (t, u, tn, p, n) {
                                //分页回调
                                curpagesize = p.HiPaginator("getPagesize");
                                if (type != "init") {
                                    ShowListData(t, u, tn, p, n, pageindex, curpagesize, false);
                                }
                            }(target, url, tmpname, pageContainer, nullContainer)
                        }
                    });
                } else {
                    if (pageContainer.HiPaginator("isExist")) {
                        pageContainer.HiPaginator("destroy");
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
    var _page = showpager;
    var pgsize = 10;
    if (_page.HiPaginator("isExist")) {
        var opts = _page.HiPaginator("getOption");
        pageindex = pageindex || opts.currentPage;
        pgsize = opts.pageSize || 10;
    } else {
        pageindex = pageindex || 1;
        isinitpage = true;
    }
    ShowListData(showbox, listurl, datatmpname, _page, datanullbox, pageindex, pgsize, true);
}

function ReloadPageData2(pageindex) {
    var _page = showpager2;
    var pgsize = 10;
    if (_page.HiPaginator("isExist")) {
        var opts = _page.HiPaginator("getOption");
        pageindex = pageindex || opts.currentPage;
        pgsize = opts.pageSize || 10;
    } else {
        pageindex = pageindex || 1;
    }
    ShowListData(showbox2, listurl2, datatmpname2, _page, datanullbox2, pageindex, pgsize, true);
}

function ReloadPageAllListData(pageindex) {
    ReloadPageData(pageindex);
    ReloadPageData2(pageindex);
}

function Post_Delete(rid) {
    if (rid.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    var pdata = {
        id: curArticId, relatedId: rid, action: "Delete"
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
        id: curArticId, action: "Clear"
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
        id: curArticId, relatedId: rid, action: "Add"
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
        id: curArticId, action: "AddSearch"
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
        id: curArticId, action: "GetRelatedIds"
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