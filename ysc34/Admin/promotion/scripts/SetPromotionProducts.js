var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl;


//页面初始
$(function () {
    showbox = $("#datashow");
    dataurl = $("#dataurl").val();

    var urldata = {
        action: "getlist"
    }
    initQuery(urldata);
    //初始数据显示
    showbox.QWRepeater({ tmplId: "#datatmpl", url: dataurl, urlParameter: urldata });
});
//搜索参数
function initQuery(obj) {
    var activityId = $("#hdactivy").val();
    if (activityId.length > 0) {
        obj.activityId = activityId;
    }
    var isWholesale = $("#hidIsWholesale").val();
    if (isWholesale.length > 0) {
        obj.isWholesale = isWholesale;
    }
    var isMobileExclusive = $("#hdIsMobileExclusive").val();
    if (isMobileExclusive.length > 0) {
        obj.isMobileExclusive = isMobileExclusive == "1";
    }
    return obj;
}

function ReloadPageData(pageindex) {
    ResetCheckAll();
    showbox.QWRepeater("reload");
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

function Post_Delete(id) {
    if (id.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    var activityId = $("#hdactivy").val();
    var candel = false;
    candel = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (candel) {
        var pdata = {
            activityId: activityId, id: id, action: "Delete"
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
                    ShowMsg(data.message, true);
                    showbox.QWRepeater("reload");
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

function Post_Clear() {
    var activityId = $("#hdactivy").val();
    var candel = false;
    candel = confirm("确定要清空这些促销商品吗！");
    if (candel) {
        var pdata = {
            activityId: activityId, action: "Clear"
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
                    ShowMsg(data.message, true);
                    showbox.QWRepeater("reload");
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