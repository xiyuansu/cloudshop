var curpagesize = 10, curpageindex = 1, total = 0, recordcount = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;


//页面初始
$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;

    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
    //搜索
    $("#btnSearch").on("click", function () {
        ShowListData(showbox, 1, curpagesize, true);
    });
});



//搜索参数
function initQuery(obj) {
    var StoreName = $("#txtStoreName").val();
    if (StoreName.length > 0) {
        obj.StoreName = StoreName;
    }
    var startDate = $("#caStartDate").val();
    if (startDate.length > 0) {
        obj.startDate = startDate;
    }
    var endDate = $("#caEndDate").val();
    if (endDate.length > 0) {
        obj.endDate = endDate;
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
            databox.empty(); DataNullHelper.hide(datanullbox);
            if (data) {
                total = data.total;
                recordcount = data.recordcount;
                if (recordcount == 0) {
                    isshowpage = false;
                }
                if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));

                    $("#lblSumTransactions").html(data.sum_amount);
                    $("#lblNumberTransactions").html(data.total);

                    $("#TabOrders tr:gt(0)").not(".table_title,.c_hidden").click(function () {
                        if ($(this).next("tr").is(":hidden")) {
                            $(this).next("tr").removeClass("c_hidden");
                        } else {
                            $(this).next("tr").addClass("c_hidden");
                        }

                    });

                    $("[name=tbItem]").css("border", "0px");
                    $("[name=tbItem] td").css("border", "0px");
                } else {
                    initpagination = true;  //强制更新分页组件
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
                        totalCounts: recordcount,
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
            try {
                loadingobj.close();
            } catch (e) { }
            ShowMsg("系统内部异常", false);
        }
    });
}




function ReloadPageData(pageindex) {
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
    setCheckbox();
}



function Link(storeId, systemStoreId, deviceId) {
    var startDate = $("#caStartDate").val();
    var endDate = $("#caEndDate").val();

    if (deviceId > 0) {
        window.location = "HiPOSDetailsList.aspx?storeId=" + storeId + "&startDate=" + startDate + "&endDate=" + endDate + "&deviceId=" + deviceId + "&systemStoreId=" + systemStoreId + "";
    } else {
        window.location = "HiPOSDetailsList.aspx?storeId=" + storeId + "&startDate=" + startDate + "&endDate=" + endDate + "&systemStoreId=" + systemStoreId + "";
    }
}




