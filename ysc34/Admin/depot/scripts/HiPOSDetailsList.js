var curpagesize=10,total = 0;
var showbox, dataurl, showpager;


//页面初始
$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();

    var urldata = {
        page: 1, rows: curpagesize, action: "getlist"
    }
    //初始数据显示
    showbox.QWRepeater({
        tmplId: "#datatmpl",
        url: dataurl,
        urlParameter: urldata,
        external: true,
        call_beforeLoadData: function (o) {
            o.urlParameter = initQuery(o.urlParameter);
        },
        event_beforedraw: function (t) {
        },
        event_drawed: function (t) {
            var opts = t.options;
            var data = opts.data;
            $("#lblSumTransactions").html(data.sum_amount);
            $("#lblNumberTransactions").html(data.total);
            var exdata = showbox.QWRepeater("external");
            if (exdata) {
                //初始分页组件
                if (opts.data.total > 0) {
                    showpager.HiPaginator({
                        totalCounts: opts.data.total,
                        pageSize: curpagesize,
                        currentPage: opts.urlParameter.page,
                        first: '',
                        prev: '<a href="javascript:;" class="page-prev">上一页</a>',
                        next: '<a href="javascript:;" class="page-next">下一页</a>',
                        page: '<a href="javascript:;">{{page}}</a>',
                        last: '',
                        visiblePages: 10,
                        disableClass: 'hide',
                        activeClass: 'page-cur',
                        onPageChange: function (pageindex, type) {
                            if (type != "init") {
                                showbox.QWRepeater("external", false);
                                showbox.QWRepeater("reloadUrl", { page: pageindex});
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

    //搜索
    $("#btnSearch").on("click", function () {
        showbox.QWRepeater("external", true);
        showbox.QWRepeater("reloadUrl", { page: 1 });
    });
});



//搜索参数
function initQuery(obj) {
    obj.storeId = $("#hidstoreId").val();
    obj.deviceId = $("#ddlPOS").val();
    obj.isSystemOrder = $("#cbxHishopOnly").is(':checked') ? true : false;
    obj.orderId = $("#txtOrderId").val();
    return obj;
}
//参数检测
function checkQuery(obj) {
    var result = true;

    return result;
}

function ReloadPageData(pageindex, initpage) {
    var urldata = {};
    if (pageindex) {
        urldata.page = pageindex;
    }
    showbox.QWRepeater("external", true);
    showbox.QWRepeater("reloadUrl", urldata);
    showpager.HiPaginator("redraw");
}


