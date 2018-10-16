var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;

var DEFAULT_PAGE_INDEX = 1, DEFAULT_PAGESIZE = 10;


//页面初始
$(function () {
    InitParameters();
    InitPage();
    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
});

///参数处理
function InitParameters() {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;
    
    try {
        curpageindex = parseInt($("#curpageindex").val());
        if (isNaN(curpageindex)) {
            curpageindex = DEFAULT_PAGE_INDEX;
        }
    } catch (e) {
        curpageindex = DEFAULT_PAGE_INDEX;
    }
    try {
        curpagesize = parseInt($("#pagesize_dropdown").val());
        if (isNaN(curpagesize)) {
            curpagesize = DEFAULT_PAGESIZE;
        }
    } catch (e) {
        curpagesize = DEFAULT_PAGESIZE;
    }
}
//页面初始
function InitPage() {
    //搜索
    $("#btnSearch").on("click", function () {
        ShowListData(showbox, 1, curpagesize, true);
    });
    //每页显示数量
    $("#pagesize_dropdown").on("change", function () {
        var _t = $(this);
        curpagesize = parseInt(_t.val());
        ShowListData(showbox, curpageindex, curpagesize, false);
        showpager.HiPaginator("option", { pageSize: curpagesize });
        showpager.HiPaginator("redraw");
    });

    $("#btnSendMessage").on("click", function () {
        var ids = GetSelectId();
        var content = $("#txtmsgcontent").val();
        Post_SendSMS(ids, content);
    });
    $("#btnSendEmail").on("click", function () {
        var ids = GetSelectId();
        var content = $("#txtemailcontent").val();
        Post_SendEmail(ids, content);
    });
    $("#btnsitecontent").on("click", function () {
        var ids = GetSelectId();
        var content = $("#txtsitecontent").val();
        Post_SendSiteMsg(ids, content);
    });

}

//Tab过滤器
function FilterTab() {
    var tab_status = $(".tab_status");
    var filterdom = $("#hidFilter");
    var FILTER_DATA_NAME = "filter", ACTION_CLASS_NAME = "hover";
    if (filterdom && tab_status.length > 0) {
        tab_status.on("click", function () {
            var _t = $(this);
            var _d = _t.data(FILTER_DATA_NAME);
            filterdom.val(_d);
            tab_status.removeClass(ACTION_CLASS_NAME);
            _t.addClass(ACTION_CLASS_NAME);
            ShowListData(showbox, 1, curpagesize, true);
        });
        var curstatus = filterdom.val();
        if (curstatus && curstatus.length > 0) {
            tab_status.removeClass(ACTION_CLASS_NAME);
            tab_status.each(function () {
                var _t = $(this);
                if (_t.data(FILTER_DATA_NAME) == curstatus) {
                    _t.addClass(ACTION_CLASS_NAME);
                }
            });
        } else {
            tab_status.eq(0).addClass(ACTION_CLASS_NAME);
        }
    }
}

//搜索参数
function initQuery(obj) {
    var LastConsumeTime = $("#hidTime").val();
    if (LastConsumeTime.length > 0) {
        obj.LastConsumeTime = LastConsumeTime;
    }
    var CustomConsumeStartTime = $("#calendarStartDate").val();
    if (CustomConsumeStartTime.length > 0) {
        obj.CustomConsumeStartTime = CustomConsumeStartTime;
    }
    var CustomConsumeEndTime = $("#calendarEndDate").val();
    if (CustomConsumeEndTime.length > 0) {
        obj.CustomConsumeEndTime = CustomConsumeEndTime;
    }
    var ConsumeTimes = $("#hidNum").val();
    if (ConsumeTimes.length > 0) {
        obj.ConsumeTimes = ConsumeTimes;
    }
    var CustomStartTimes = $("#txtCustomStartTimes").val();
    if (CustomStartTimes.length > 0) {
        obj.CustomStartTimes = CustomStartTimes;
    }
    var CustomEndTimes = $("#txtCustomEndTimes").val();
    if (CustomEndTimes.length > 0) {
        obj.CustomEndTimes = CustomEndTimes;
    }
    var ConsumePrice = $("#hidPrice").val();
    if (ConsumePrice.length > 0) {
        obj.ConsumePrice = ConsumePrice;
    }
    var CustomStartPrice = $("#txtStartPrice").val();
    if (CustomStartPrice.length > 0) {
        obj.CustomStartPrice = CustomStartPrice;
    }
    var CustomEndPrice = $("#txtEndPrice").val();
    if (CustomEndPrice.length > 0) {
        obj.CustomEndPrice = CustomEndPrice;
    }
    var OrderAvgPrice = $("#hidAvgPrice").val();
    if (OrderAvgPrice.length > 0) {
        obj.OrderAvgPrice = OrderAvgPrice;
    }
    var CustomStartAvgPrice = $("#txtStartAvgPrice").val();
    if (CustomStartAvgPrice.length > 0) {
        obj.CustomStartAvgPrice = CustomStartAvgPrice;
    }
    var CustomEndAvgPrice = $("#txtEndAvgPrice").val();
    if (CustomEndAvgPrice.length > 0) {
        obj.CustomEndAvgPrice = CustomEndAvgPrice;
    }
    var ProductCategory = $("#hidCategoryId").val();
    if (ProductCategory.length > 0) {
        obj.ProductCategory = ProductCategory;
    }
    var MemberTag = $("#hidTagId").val();
    if (MemberTag.length > 0) {
        obj.MemberTag = MemberTag;
    }
    var UserGroupType = $("#hidUserGroupType").val();
    if (UserGroupType.length > 0) {
        obj.UserGroupType = UserGroupType;
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
    var DATA_TEMPLATE_DOM_NAME = "datatmpl";
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
                    databox.html(template(DATA_TEMPLATE_DOM_NAME, data));
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
                            curpagesize = showpager.HiPaginator("getPagesize");
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
//后续处理
function DataRedraw(databox) {
    try {
        $('[data-toggle="tooltip"]').tooltip();
        $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red', });
    } catch (e) { }
}

function ReloadPageData(pageindex) {
    ResetCheckAll();
    var _page = showpager;
    var opts = _page.HiPaginator("getOption");
    curpagesize = opts.pageSize;
    curpageindex = pageindex || opts.currentPage;
    ShowListData(showbox, curpageindex, curpagesize, true);
}

//复位全选
function ResetCheckAll() {
    try {
        var chkall = $("#checkall");
        if (chkall) {
            if (chkall.iCheck) {
                chkall.iCheck('uncheck');
            }
            chkall.prop("checked", false);
        }
    } catch (e) { }
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

function ShowSuccessAndReloadData(msg) {
    msg = msg || "操作成功"
    ShowMsg(msg, true);
    CloseDialogAndReloadData();
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

function bat_delete() {
    var ids = GetSelectId();
    if (ids.length == 0) {
        alert("请选择数据项");
        return;
    }
    Post_Delete(ids);
}

function Post_Delete(ids) {
    if (ids.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    var canoper = false;
    canoper = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (canoper) {
        var pdata = {
            ids: ids, action: "Delete"
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

function Post_SendSMS(ids, content) {
    if (ids.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    if (content.length < 1) {
        ShowMsg("请先填写发送的内容信息", false);
        return;
    }
    var canoper = true;
    if (canoper) {
        var pdata = {
            ids: ids,content:content, action: "SendSMS"
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

function Post_SendEmail(ids, content) {
    if (ids.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    if (content.length < 1) {
        ShowMsg("请先填写发送的内容信息", false);
        return;
    }
    var canoper = true;
    if (canoper) {
        var pdata = {
            ids: ids, content: content, action: "SendEmail"
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

function Post_SendSiteMsg(ids, content) {
    if (ids.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    if (content.length < 1) {
        ShowMsg("请先填写发送的内容信息", false);
        return;
    }
    var canoper = true;
    if (canoper) {
        var pdata = {
            ids: ids, content: content, action: "SendSiteMsg"
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

function Post_ExportExcel() {
    var urldata = {
        action: "exportexcel"
    }
    initQuery(urldata);

    //处理导出参数
    var format = $(":radio[name*='exportFormatRadioButtonList']:checked").val();
    urldata.FileFormat = format;
    var fields = "", headers = "";
    $(":checkbox[name*='exportFieldsCheckBoxList']:checked").each(function () {
        var _t = $(this);
        var _d = _t.parents("span[data-type='field']");
        var _vf = _d.data("field"), _vh = _d.data("header");
        if (_vf.length > 0 && _vh.length > 0) {
            fields += _vf + ",";
            headers += _vh + ",";
        }
    });
    urldata.Fields = fields;
    urldata.Headers = headers;

    if (urldata.Fields.length < 1 || urldata.Headers.length < 1 || urldata.FileFormat.length < 1) {
        ShowMsg("请选择需要导出的信息头", false);
        return;
    }
    var url = dataurl + "?";
    for (var item in urldata) {
        url += item + "=" + urldata[item] + "&";
    }
    window.open(url);
}