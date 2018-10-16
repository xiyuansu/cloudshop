var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager,datanullbox,datanulldom;



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
    
    var UserName = $("#txtUserName").val();
    if (UserName.length > 0) {
        obj.UserName = UserName;
    }
    return obj;
}



function ShowListData(target, page, pagesize, initpagination) {
    page = page || 1;
    pagesize = pagesize || 10;
    if (typeof (initpagination) == "undefined") { initpagination = true; }
    var urldata = {
        page: page, rows: pagesize
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
            databox.empty();DataNullHelper.hide(datanullbox);
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


function CheckReferral(userId, userName, requestReason) {
    $("#hidUserId").val(userId);
    //$("#ctl00_contentHolder_lblRealname").html(realName);
    $("#ctl00_contentHolder_lblUserName").html(userName);
    $("#ctl00_contentHolder_lblPersonMsg").html(requestReason);

    setArryText('ctl00_contentHolder_txtRefusalReason', '');

    ShowMessageDialog("分销员审核", "checkReferral", "CheckReferral");
}

//审核分销员
function acceptRequest() {
     
    var userID = $("#hidUserId").val();
    var dataurl = "/admin/member/ashx/MemberManage.ashx";
    var urldata = { userId: userID, action: "acceptRequest" }
    var loading = showCommonLoading();
    $.ajax({
        type: "GET",
        url: dataurl,
        data: urldata,
        dataType: "json",
        success: function(data){
            loading.close();
            CloseDialogFrame("", false);
            if (data.success) {
                ShowMsg("审核分销员成功", true);
                ReloadPageData();
            } else {
                ShowMsg(data.message, false);
            }
         
        },
        error: function () {
            loading.close();
            CloseDialogFrame("", false);
        }
 });
}

function refuse() {
    var refusalReason = $("#txtRefusalReason").val();
    if (refusalReason.length == 0) {
        alert("请输入拒绝理由！");
        return false;
    }
    refuseAjax(refusalReason);
  //  $("#hidRefusalReason").val(refusalReason);
  //  $("#ctl00_contentHolder_btnRefuse").trigger("click");
}

function refuseAjax(refusalReason) {

    var userID = $("#hidUserId").val();
    var dataurl = "/admin/member/ashx/MemberManage.ashx";
    var urldata = { userId: userID, action: "refuse", refusalReason: refusalReason }
    var loading = showCommonLoading();
    $.ajax({
        type: "POST",
        url: dataurl,
        data: urldata,
        dataType: "json",
        success: function (data) {
            loading.close();
            CloseDialogFrame("", false);
            if (data.success) {
                ShowMsg("拒绝分销员成功", true);
                ReloadPageData();
            } else {
                ShowMsg(data.message, false);
            }

        },
        error: function () {
            loading.close();
            CloseDialogFrame("", false);
        }
    });
}
 