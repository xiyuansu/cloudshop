var curpagesize = 10, curpageindex = 1, total = 0, tagId;
var showbox, dataurl, showpager, datanullbox, datanulldom;


//页面初始
$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;

    tagId = GetQueryString("tagId");
    InitPage();
    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
    //搜索
    $("#btnSearch").on("click", function () {
        curpagesize = parseInt($("#pagesize_dropdown").val());
        tagId = 0;
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

}

//搜索参数
function initQuery(obj) {
    var StoreName = $("#txtStoresName").val();
    if (StoreName.toString().length > 0) {
        obj.StoreName = StoreName;
    }
    var UserName = $("#txtUserName").val();
    if (UserName.toString().length > 0) {
        obj.UserName = UserName;
    }
    var RegionId = $("#regionSelectorValue").val();
    if (RegionId.toString().length > 0) {
        obj.RegionId = RegionId;
    }

    var CategoryId = $("#dropArticleCategory").val();
    if (CategoryId > 0) {
        obj.CategoryId = CategoryId;
    }
    if (tagId > 0) {
        obj.tagId = tagId;
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
                if (total == 0) {
                    isshowpage = false;
                }
                if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));
                    DataRedraw(databox);
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
            try {
                loadingobj.close();
            } catch (e) { }
            ShowMsg("系统内部异常", false);
        }
    });
}




function ReloadPageData(pageindex) {    /*复位*/    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }
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
function DataRedraw(databox) {
    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red' });

}
//全选按钮关闭
function setCheckbox() {

    if ($("#checkall").attr('checked') == 'checked') {
        $("#checkall").iCheck('uncheck');
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
//关闭并刷新页面数据
function CloseDialogAndReloadData(id) {
    if (id) {
        art.dialog({ id: id }).close();
    } else {
        CloseDialogFrame("", false);
    }
    ReloadPageData();
}
function ShowSuccessAndCloseReload() {
    ShowMsg("批量操作成功!", true);
    CloseDialogAndReloadData();
}


function batchEdit() {
    var userIds = GetSelectId();
    if (userIds.length > 0) {
        DialogFrame("depot/BatchEditCommissionRate.aspx?StoresIds=" + userIds + "&callback=ShowSuccessAndCloseReload", "批量设置佣金比例", 480, 220, null);
    }
}

function SetStoreState(storeId) {
    var urldata = {
        StoreId: storeId, action: "SetStoreState"
    }
    $.ajax({
        type: "POST",
        url: dataurl,
        data: urldata,
        dataType: "json",
        success: function (data) {
            if (data.success) {
                ShowMsg("设置成功!", true);
                ReloadPageData();
            }
            else {
                ShowMsg("设置失败!", false);
            }
        }
    });
}


function Post_ExportExcel() {
    var storeIds = GetSelectId();
    var urldata = {
        action: "exportexcel"
    }
    if (storeIds.length > 0) {
        urldata.storeIds = storeIds;
    } else {
        initQuery(urldata);
    }

    var url = dataurl + "?";
    for (var item in urldata) {
        url += item + "=" + urldata[item] + "&";
    }
    window.open(url);
}