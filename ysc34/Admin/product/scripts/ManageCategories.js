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
        },
        event_drawed: function (t) {
            var span = $("span.b-subcate", t.container);
            span.each(function () {
                var _t = $(this);
                var _depth = _t.data("depth");
                var _basepadding = 25;
                var _padding = _basepadding * (_depth - 1);
                _t.css({ "padding-left": _padding });
            });
        }
    });

});

$("#datashow").on("blur", ".txtdisplay", function () {
    debugger;
    var _v = $(this).val();
    if (_v.length <= 0) {
        ShowMsg("排序不能为空！", false);
        return;
    }

    var _old = $(this).data("oldvalue");
    if (_old == _v) {
        return;
    }
    var CategoryId = $(this).data("id");
    var pdata = {
        CategoryId: CategoryId, DisplaySequence: _v, action: "setorder"
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
    candel = confirm("删除分类会级联删除其所有子分类，确定要删除选择的分类吗？");
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


