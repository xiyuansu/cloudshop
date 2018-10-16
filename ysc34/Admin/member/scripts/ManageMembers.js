var urlPageIndex = parseInt(getParam("page"));
var urlPageSize = parseInt(getParam("rows"));
if (isNaN(urlPageIndex) || urlPageIndex <= 0) {
    urlPageIndex = 1;
}
else {
    $("#curpageindex").val(urlPageIndex)
}
if (isNaN(urlPageSize) || urlPageSize <= 0) {
    urlPageSize = 10;
}
else {
    $("#pagesize_dropdown").val(urlPageSize);
}
var curpagesize = urlPageSize, curpageindex = urlPageIndex, total = 0;
var showbox, dataurl, showpager;
var DEFAULT_PAGE_INDEX = 1, DEFAULT_PAGESIZE = 10;
var queryParameters;
function getReturnUrl() {
    var listurl = "/admin/member/managemembers";
    var url = listurl + "?";
    if (queryParameters) {
        for (var item in queryParameters) {
            url += item + "=" + queryParameters[item] + "&";
        }
    }
    if (url.lastIndexOf("&") == url.length - 1) {
        url = url.substring(0, url.length - 1);
    }
    return url;
}
///会员详情页
function ToDetail(userId) {
    location.href = "/admin/member/MemberDetails?userId=" + userId + "&returnUrl=" + encodeURIComponent(getReturnUrl());
}
///会员编辑页
function ToEdit(userId) {
    location.href = "/admin/member/EditMember?userId=" + userId + "&returnUrl=" + encodeURIComponent(getReturnUrl());
}

function GoMemberSearch() {
    location.href = "/admin/member/MemberSearch?returnUrl=" + encodeURIComponent(getReturnUrl());
}
//生日提醒设置
function GoMemberBirthDaySetting() {
    location.href = "/admin/member/MemberBirthDaySetting?returnUrl=" + encodeURIComponent(getReturnUrl());
}

function ToPointDetails(userId, userName, points) {
    location.href = "/admin/member/MemberPointDetails?userId=" + userId + "&userName=" + userName + "&points=" + points + "&returnUrl=" + encodeURIComponent(getReturnUrl());
}
//页面初始
$(function () {
    setQuery();
    InitParameters();
    InitPage();

    var urldata = {
        page: curpageindex, rows: curpagesize, action: "getlist"
    }

    //初始数据显示
    showbox.QWRepeater({
        tmplId: "#datatmpl",
        url: dataurl,
        urlParameter: urldata,
        external: true,
        call_beforeLoadData: function (o) {
            o.urlParameter = initQuery(o.urlParameter);
            queryParameters = o.urlParameter;
        },
        event_beforedraw: function (t) {
        },
        event_drawed: function (t) {
            var isChkAll = $("#checkall").is(':checked');
            if (isChkAll) {
                $("input[name='CheckBoxGroup']").iCheck('check');
            }
            else {
                $("input[name='CheckBoxGroup']").iCheck('uncheck');
            }
            var opts = t.options;
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
                                curpagesize = showpager.HiPaginator("getPagesize");
                                showbox.QWRepeater("reloadUrl", { page: pageindex, rows: curpagesize });
                            }
                        }
                    });
                } else {
                    if (showpager.HiPaginator("isExist")) {
                        showpager.HiPaginator("destroy");
                    }
                }
            }
            $('[data-toggle="tooltip"]', t.container).tooltip();
            $('.icheck', t.container).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red', });
        }
    });
});

///参数处理
function InitParameters() {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();

    try {
        //curpageindex = parseInt($("#curpageindex").val());
        if (curpageindex <= 0) {
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
        showbox.QWRepeater("external", true);
        showbox.QWRepeater("reloadUrl", { page: 1 });
    });
    var pagesizedom = $("#pagesize_dropdown");
    pagesizedom.find("option").each(function () {
        var _t = $(this);
        if (_t.val() == curpagesize) {
            _t.prop("selected", true);
        }
    });
    //每页显示数量
    pagesizedom.on("change", function () {
        var _t = $(this);
        curpagesize = parseInt(_t.val());
        showbox.QWRepeater("reloadUrl", { rows: curpagesize });
        showpager.HiPaginator("redraw", { pageSize: curpagesize });
    });

    $("#dropSortBy").change(function (e) {
        ReloadPageData();
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

    //回车查询事件在Admin.Master母版中有定义，此处再定义可能导致重复触发事件
    //$(".searcharea input[type='text']").on("keypress", function (event) {
    //    if (event.keyCode == 13) {
    //        $("#btnSearch").trigger("click");
    //        event.returnValue = false;
    //    }
    //});

}

//搜索参数
function initQuery(obj) {
    obj.UserName = $("#txtUserName").val();
    obj.RealName = $("#txtRealName").val();
    obj.StartTime = $("#cldStartDate").val();
    obj.EndTime = $("#cldEndDate").val();
    obj.GradeId = $("#rankList").val();
    obj.RegisteredSource = $("#sourceList").val();
    obj.TagsId = $("#ddlMemberTags").val();
    obj.UserGroupType = $("#hidUserGroupType").val();
    obj.OrderBy = $("#dropSortBy").val();
    //obj.CkMembrtBrithDay = $("#CkMembrtBrithDay").is(':checked');
    return obj;
}


function setQuery() {
    if (getParam("UserName") != "") {
        $("#txtUserName").val(getParam("UserName"));
    }
    if (getParam("RealName") != "") {
        $("#txtRealName").val(getParam("RealName"));
    }
    if (getParam("StartTime") != "") {
        $("#cldStartDate").val(getParam("StartTime"));
    }
    if (getParam("EndTime") != "") {
        $("#cldEndDate").val(getParam("EndTime"));
    }
    //if (getParam("GradeId") != "") {
    //    $("#rankList").val(getParam("GradeId"));
    //}
    //if (getParam("RegisteredSource") != "") {
    //    $("#sourceList").val(getParam("RegisteredSource"));
    //}
    //if (getParam("TagsId") != "") {
    //    $("#ddlMemberTags").val(getParam("TagsId"));
    //}
    if (getParam("UserGroupType") != "") {
        $("#hidUserGroupType").val(getParam("UserGroupType"));
    }
    //if (getParam("OrderBy") != "") {
    //    $("#dropSortBy").val(getParam("OrderBy"));
    //}
}

function ReloadPageData(pageindex) {
    ResetCheckAll();
    var urldata = {};
    if (pageindex) {
        urldata.page = pageindex;
    }
    showbox.QWRepeater("external", true);
    showbox.QWRepeater("reloadUrl", urldata);
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
            ids: ids, content: content, action: "SendSMS"
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
                    ShowMsg("发送短信成功", true);
                    $("$litsmscount").html(data.message);
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
                    ShowMsg("发送邮件成功", true);
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