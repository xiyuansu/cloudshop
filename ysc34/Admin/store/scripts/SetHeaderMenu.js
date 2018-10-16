var showbox, dataurl;

//页面初始
$(function () {
    showbox = $("#datashow");
    dataurl = $("#dataurl").val();
    var urldata = { action: "getlist" };
    //初始数据显示
    showbox.QWRepeater({
        tmplId: "#datatmpl", url: dataurl, urlParameter: urldata,
        event_drawed: function (t) {
            var data = t.options.data;
            $("#txtCategoryNum").val(data.CategoryNum);
        }
    });

    $("#btnSaveNum").click(function () {
        var num = $("#txtCategoryNum").val();
        Post_SetShowNum(num);
    });
});

function ReloadPageData() {
    ResetCheckAll();
    showbox.QWRepeater("reload");
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

function ResetCheckAll() {
    try {
        var chkall = $("#checkall");
        if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); }
    } catch (e) { }
}

function Post_Delete(id) {
    if (id.length < 1) {
        ShowMsg("错误的参数", false);
        return;
    }
    var candel = false;
    candel = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (candel) {
        var pdata = {
            id: id, action: "Delete"
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

function Post_SetShow(id) {
    if (id.length < 1) {
        ShowMsg("错误的参数", false);
        return;
    }
    var candel = true;
    if (candel) {
        var pdata = {
            id: id, action: "SetShow"
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

function Post_Sort(id,sort) {
    if (id.length < 1 || sort.length<1) {
        ShowMsg("错误的参数", false);
        return;
    }
    var candel = true;
    if (candel) {
        var pdata = {
            id: id,sort:sort, action: "Sort"
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

function Post_SetShowNum(num) {
    //if (num.length < 1) {
    //    ShowMsg("错误的参数", false);
    //    return;
    //}
    var candel = true;
    if (candel) {
        var pdata = {
            num: num, action: "SetShowNum"
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