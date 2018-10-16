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
    return obj;
}

function ReloadPageData() {
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

function Post_Delete(BackupName) {
    if (BackupName.length < 1) {
        ShowMsg("错误的参数", false);
        return;
    }
    var candel = false;
    candel = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (candel) {
        var pdata = {
            BackupName: BackupName, action: "Delete"
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

function Post_Restore(BackupName) {
    if (BackupName.length < 1) {
        ShowMsg("错误的参数", false);
        return;
    }
    var candel = false;
    candel = confirm("您确定要执行恢复操作吗？执行后数据将恢复到备份时的状态！");
    if (candel) {
        var pdata = {
            BackupName: BackupName, action: "Restore"
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