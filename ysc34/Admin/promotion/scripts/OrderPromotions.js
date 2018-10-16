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
    showbox.QWRepeater({tmplId: "#datatmpl",url: dataurl,urlParameter:urldata});
});
//搜索参数
function initQuery(obj) {
    var isWholesale = $("#hidIsWholesale").val();
    if (isWholesale.length > 0) {
        obj.isWholesale = isWholesale;
    }
    return obj;
}

function Post_Delete(id) {
    if (id.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    var candel = false;
    candel = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (candel) {
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