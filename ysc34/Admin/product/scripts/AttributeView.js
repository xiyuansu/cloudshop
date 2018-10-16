var databox, dataurl, typeId;
$(function () {
    databox = $("#datashow");
    dataurl = $("#dataurl").val();
    typeId = $("#hidProductTypeId").val();
    databox.QWRepeater({
        tmplId: "#datatmpl",
        url: dataurl,
        urlParameter: { action: "getlist", typeId: typeId },
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
}

function SuccessAndCloseReload(msg) {
    msg = msg || "操作成功！";
    ShowMsg(msg, true);
    CloseDialogAndReloadData();
}

function Post_Deletes(id) {
    var candel = false;
    candel = confirm("当前操作将彻底删该除属性及下属的所有值，确定吗？");
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

function Post_DeleteValue(id) {
    var candel = false;
    candel = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (candel) {
        var pdata = {
            id: id, action: "DeleteValue"
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

function Post_Multi(id) {
    var pdata = {
        id: id, typeId: typeId, action: "IsMulti"
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

function Post_EditName(id, dom) {
    var _t = $(dom);
    var namedom = _t.parent().find(".ipt_AttrName");
    var name = namedom.val();
    if (name.length < 1) {
        ShowMsg("请填写属性名称", false);
        return;
    }
    var pdata = {
        id: id,name:name, typeId: typeId, action: "EditName"
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


function SetOrder(Type, DisplaySequence, AttributeId, index) {
    var replaceAttributeId;
    var num;
    if (Type == "Fall") {
        num = parseInt(index) + 1;
    } else {
        num = parseInt(index) - 1;

    }
    replaceAttributeId = $("#DisplaySequence_" + num).val();
    var pdata = {
        Type: Type, DisplaySequence: DisplaySequence, AttributeId: AttributeId, replaceAttributeId: replaceAttributeId, action: "setorder"
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


