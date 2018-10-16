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
    ShowMsg("操作成功！");
}

function Post_Deletes(id) {

    var candel = false;
    candel = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (candel) {
        var pdata = {
            id: id, action: "delete"
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

 
function EditValue()
{

    var valueId = $("#hidvalueId").val();
    var OldValue = $("#txtOldValue").val();
  

    var pdata = {
        valueId: valueId, OldValue: OldValue, action: "editvalue"
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
    $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
}


function AddValue() {
    var Value = $("#txtValue").val();
    var pdata = {
        Value: Value, AttributeId: AttributeId, action: "addvalue"
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
                $("#txtName").val("");
                $("#chkIsImg").prop("checked", false);

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
    $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
}
