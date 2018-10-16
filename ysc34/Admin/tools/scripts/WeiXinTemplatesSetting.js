$(function () {
    dataurl = $("#dataurl").val();
    //一键设置
    $("#btnQuickSet").click(function (e) {
        var pdata = {
            action: "QuickSetWeiXinTemplates"
        }
        var loading = showCommonLoading();
        $.ajax({
            type: "post",
            url: dataurl,
            data: pdata,
            dataType: "json",
            success: function (data) {
                loading.close();
                if (data.Status == "SUCCESS") {
                    ShowMsg(data.Message, true, function (e) {
                        document.location.reload();
                    });

                } else {
                    ShowMsg(data.Message, false);
                }
            },
            error: function () {
                loading.close();
            }
        });
    })
    //手动批量设置
    $("#btnSaveSendSetting").click(function (e) {
        var templates = $("#grdWxTempletsNew tr td input[type='hidden']");
        var data = "{\"TemplatePostData\":[";
        $("#grdWxTempletsNew tr td input[type='hidden']").each(function () {
            var templateItem = "{";
            templateItem += "\"MessageType\":" + "\"" + $(this).val() + "\",";
            templateItem += "\"TemplateId\":" + "\"" + $("input[messagetype='" + $(this).val() + "']").val() + "\"";
            templateItem += "},";
            data += templateItem;
        });
        data = data.substring(0, data.length - 1);
        data += "]}";
        var pdata = {
            action: "SaveWeiXinTemplates",
            TemplateData: data
        }
        var loading = showCommonLoading();
        $.ajax({
            type: "post",
            url: dataurl,
            data: pdata,
            dataType: "json",
            success: function (data) {
                loading.close();
                if (data.Status =="SUCCESS") {
                    ShowMsg(data.Message, true, function (e) {
                        document.location.reload();
                    });
                    
                } else {
                    ShowMsg(data.Message, false);
                }
            },
            error: function () {
                loading.close();
            }
        });
    })

    $("#btnAppletSaveSendSetting").click(function (e) {
        var templates = $("#grdWxTempletsNew tr td input[type='hidden']");
        var data = "{\"TemplatePostData\":[";
        $("#grdWxTempletsNew tr td input[type='hidden']").each(function () {
            var templateItem = "{";
            templateItem += "\"MessageType\":" + "\"" + $(this).val() + "\",";
            templateItem += "\"TemplateId\":" + "\"" + $("input[messagetype='" + $(this).val() + "']").val() + "\"";
            templateItem += "},";
            data += templateItem;
        });
        data = data.substring(0, data.length - 1);
        data += "]}";
        var pdata = {
            action: "SaveAppletTemplates",
            TemplateData: data
        }
        var loading = showCommonLoading();
        $.ajax({
            type: "post",
            url: dataurl,
            data: pdata,
            dataType: "json",
            success: function (data) {
                loading.close();
                if (data.Status == "SUCCESS") {
                    ShowMsg(data.Message, true, function (e) {
                        document.location.reload();
                    });

                } else {
                    ShowMsg(data.Message, false);
                }
            },
            error: function () {
                loading.close();
            }
        });
    })
});