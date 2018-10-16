var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl;


//页面初始
$(function () {
    showbox = $("#datashow");
    dataurl = $("#dataurl").val();

    var urldata = {
        action: "getlist", ClientType: $("#hidClientType").val()
    }
    //初始数据显示
    showbox.QWRepeater({ tmplId: "#datatmpl", url: dataurl, urlParameter: urldata });
});

function ReloadPageData() {
    showbox.QWRepeater("reload");
}

function Post_Delete(id) {
    if (id.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    if (confirm("确定要执行该删除操作吗？删除后将不可以恢复！")) {
        var pdata = {
            id: id, action: "Delete"
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

function Post_Sort(id, sort) {
    if (id.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    var pdata = {
        id: id, sort: sort, action: "Sort"
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
