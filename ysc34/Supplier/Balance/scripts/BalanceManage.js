var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;
var DEFAULT_PAGE_INDEX = 1, DEFAULT_PAGESIZE = 10;

//页面初始
$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;
    
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


//搜索参数
function initQuery(obj) {
    var DateStart = $("#calendarStartDate").val();
    if (DateStart.length > 0) {
        obj.DateStart = DateStart;
    }

    var DateEnd = $("#calendarEndDate").val();
    if (DateEnd.length > 0) {
        obj.DateEnd = DateEnd;
    }

    var TradeType = $("#ddlType").val();
    if (TradeType.length > 0) {
        obj.TradeType = TradeType;
    }
    return obj;
}



function ShowListData(target, page, pagesize, initpagination) {
    page = page || 1;
    pagesize = pagesize || 10;
    if (typeof (initpagination) == "undefined") { initpagination = true; }
    var urldata = {
        page: page, rows: pagesize, action: "getlistBalance"
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
                    databox.html(template('datatmpl', data));
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


//生成报告
function ExportToExcelBalance() {
    var url = dataurl + "?";
    var urldata = {
        action: "exporttoexcelBalance"
    }
    initQuery(urldata);
    for (var item in urldata) {
        url += item + "=" + urldata[item] + "&";
    }
    window.open(url);
}