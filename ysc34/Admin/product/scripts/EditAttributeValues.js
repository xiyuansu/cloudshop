var databox, dataurl, AttributeId;
$(function () {
    databox = $("#datashow");
    dataurl = $("#dataurl").val();
    AttributeId = getParam('AttributeId');
    databox.QWRepeater({
        tmplId: "#datatmpl",
        url: dataurl,
        urlParameter: { action: "getlist", AttributeId: AttributeId },
        event_drawed: function (t) {
            var sortbox = $("div.sortbox", t.container);
            var sortmax = sortbox.length - 1;
            sortbox.each(function () {
                var sort_fall = $('<a href="###" class="sort-fall"><img src="../images/fall.gif" border="0"></a>');
                var sort_rise = $('<a href="###" class="sort-rise"><img src="../images/rise.gif" border="0"></a>');
                var _t = $(this);
                var _index = sortbox.index(_t);
                if (_index > 0) {
                    _t.append(sort_rise);
                    var preid = sortbox.eq(_index - 1).attr("val-id");
                    _t.attr("val-preid", preid);
                }
                if (_index < sortmax) {
                    _t.append(sort_fall);
                    var nextid = sortbox.eq(_index + 1).attr("val-id");
                    _t.attr("val-nextid", nextid);
                }
            });
        }
    });

    databox.on("click", ".sort-fall", function () {
        var _d = $(this).parent();
        var _id = _d.attr("val-id");
        var _nid = _d.attr("val-nextid");
        var _sort = _d.attr("val-sort");
        SetOrder("Fall", _sort, _id, _nid);
    });
    databox.on("click", ".sort-rise", function () {
        var _d = $(this).parent();
        var _id = _d.attr("val-id");
        var _pid = _d.attr("val-preid");
        var _sort = _d.attr("val-sort");
        SetOrder("Rise", _sort, _id, _pid);
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
    ShowMsg("操作成功！", true);
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


function EditValue() {

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

function SetOrder(Type, DisplaySequence, id, rid) {
    var pdata = {
        Type: Type, DisplaySequence: DisplaySequence, AttributeId: id, replaceAttributeId: rid, action: "setorder"
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


