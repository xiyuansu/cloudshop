var databox, dataurl, typeId;
$(function () {
    databox = $("#datashow");
    dataurl = $("#dataurl").val();
      typeId = getParam('typeId');
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
            //    t.container.append("<tr><td colspan=\"4\">界面绘制之前</td></tr>");
        },
        event_drawed: function (t) {
            //    t.container.append("<tr><td colspan=\"4\">界面绘制之后</td></tr>");
           
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
    ShowMsg("操作成功！",true);
}

function Post_Deletes(ids) {

    var candel = false;
    candel = confirm("当前操作将彻底删该除规格及下属的所有规格值，确定吗？");
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

function IsUplod(AttributeId,UseAttributeImage)
{
    var pdata = {
        AttributeId: AttributeId,UseAttributeImage:UseAttributeImage,typeId:typeId, action: "isupload"
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

function EditSku()
{
    var attributeId=  $("#hidEditId").val();
    var SkuName = $("#txtName").val();
    var UseAttributeImage = $("#chkIsImg").is(':checked');
    var pdata = {
        typeId:typeId, attributeId: attributeId, SkuName: SkuName, UseAttributeImage: UseAttributeImage, action: "editsku"
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
                debugger;
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


function AddSku() {
    var SkuName = $("#txtName").val();
    var UseAttributeImage = $("#chkIsImg").is(':checked');
    var pdata = {
        SkuName: SkuName, UseAttributeImage: UseAttributeImage,typeId:typeId, action: "addsku"
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

function SetOrder(Type, DisplaySequence, AttributeId, index)
{
    var replaceAttributeId;
    var num;
    if (Type == "Fall")
    {
        num = parseInt(index) + 1;
    } else {
        num = parseInt(index) - 1;
      
    }
    replaceAttributeId = $("#DisplaySequence_" + num).val();
    var pdata = {
        Type: Type, DisplaySequence: DisplaySequence, AttributeId: AttributeId,replaceAttributeId:replaceAttributeId, action: "setorder"
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


