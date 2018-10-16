var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;



//页面初始
$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;

    // curpagesize = parseInt($("#pagesize_dropdown").val());
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
    FilterTab();
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

    var startDate = $("#calendarStartDate").val();
    if (startDate.length > 0) {
        obj.startDate = startDate;
    }

    var endDate = $("#calendarEndDate").val();
    if (endDate.length > 0) {
        obj.endDate = endDate;
    }
    obj.Type = $("#hidType").val();
    return obj;
}

function FilterTab() {
    var tab_status = $(".tab_status");
    var filterdom = $("#hidType");
    var FILTER_DATA_NAME = "filter", ACTION_CLASS_NAME = "hover";
    if (filterdom && tab_status.length > 0) {
        tab_status.on("click", function () {
            var _t = $(this);
            var _d = _t.data(FILTER_DATA_NAME);
            if (_d == true || _d == "true") {
                $(".functionHandleArea").hide();
            }
            else {
                $(".functionHandleArea").show();
            }
            filterdom.val(_d);
            tab_status.removeClass(ACTION_CLASS_NAME);
            _t.addClass(ACTION_CLASS_NAME);
            ShowListData(showbox, 1, curpagesize, true);
        });
        var curstatus = filterdom.val();
        console.log(curstatus);
        if (curstatus.length > 0) {
            tab_status.removeClass(ACTION_CLASS_NAME);
            tab_status.each(function () {
                var _t = $(this);
                if (_t.data(FILTER_DATA_NAME).toString() == curstatus) {
                    _t.addClass(ACTION_CLASS_NAME);
                }
            });
        } else {
            tab_status.eq(0).addClass(ACTION_CLASS_NAME);
        }
    }
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
    $(".rating").rating();

}


function post_delete(ReviewId) {


    var dataurl = "/admin/comment/ashx/ProductReviews.ashx";
    var candel = false;
    candel = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (candel) {
        var urldata = { ReviewId: ReviewId, action: "delete" }
        var loading = showCommonLoading();
        $.ajax({
            type: "POST",
            url: dataurl,
            data: urldata,
            dataType: "json",
            success: function (data) {
                loading.close();

                if (data.success) {
                    ShowMsg(data.message, true);
                    ReloadPageData();
                } else {
                    ShowMsg(data.message, false);
                }

            },
            error: function () {
                loading.close();

            }
        });
    }
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

function BatchReply() {
    var selectIds = GetSelectId();
    DialogFrame('comment/BatchReplyProductReviews?callback=CloseDialogAndReloadData&ReviewIds=' + selectIds, '回复客户评论', null, null, function (e) { ReloadPageData(); });
}
