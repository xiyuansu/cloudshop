var databox, dataurl;
$(function () {
    databox = $("#datashow");
    dataurl = $("#dataurl").val();
    databox.QWRepeater({
        tmplId: "#datatmpl",
        url: dataurl,
        urlParameter: { action: "getlist" },
        event_beforedraw: function (t) {
            var opts = t.options;
            if (opts.data.rows) {
                if (opts.data.rows.length < 1) {
                    opts.data = null;
                }
            } else {
                opts.data = null;
            }
            //    t.container.append("<tr><td colspan=\"4\">界面绘制之前</td></tr>");
        },
        event_drawed: function (t) {
            //    t.container.append("<tr><td colspan=\"4\">界面绘制之后</td></tr>");
            DataRedraw(databox);
        }
    });

});


function DataRedraw(databox) {
    

}

 

function Post_Deletes(ids) {
    
    var candel = false;
    candel = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (candel) {
        var pdata = {
            ids: ids, action: "delete"
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
                    databox.QWRepeater("reload");

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

 
$("#datashow").on("blur", ".txtdisplay", function () {
    var _v = $(this).val();
    if (_v.length<=0) {
        ShowMsg("排序不能为空！", false);
        return;
    }
    var _old = $(this).data("oldvalue");
    if (_old==_v) {
        return;
    }
    var ServiceId = $(this).data("id");
    var pdata = {
        ServiceId: ServiceId, action: "saveorder", Value: _v
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
                databox.QWRepeater("reload");
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



    });

//关闭并刷新页面数据
function CloseDialogAndReloadData(id) {
    if (id) {
        art.dialog({ id: id }).close();
    } else {
        CloseDialogFrame("", false);
    }
    databox.QWRepeater("reload");
}

//是否显示
function IsShow(CategoryId) {
    var pdata = {
        CategoryId: CategoryId, action: "isshow"
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
              databox.QWRepeater("reload");
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