var pluginContainer, templateRow;
var dropPlugins, selectedNameCtl, configDataCtl;
var InputType = { "TextBox": 0, "TextArea": 1, "DropDownList": 2, "CheckBox": 3, "Password": 5, "File": 6 };

function SelectPlugin(pluginType) {
    $("#licert").hide();
    var gateway = $(dropPlugins).val().toLowerCase();
    if (gateway == "hishop.plugins.payment.podrequest" || gateway == "hishop.plugins.payment.bankrequest" || gateway == "hishop.plugins.payment.advancerequest") {
        $("#liUsePrePay").hide();
    }
    else {
        $("#liUsePrePay").show();
    }
    ResetContainer($(dropPlugins).val(), pluginType);
}

function ResetContainer(name, pluginType) {
    if (pluginContainer.length == 0)
        return;

    $.each($(pluginContainer).find("[rowType=attributeContent]"), function (i, item) {
        $(item).remove();
    });

    if (name.length == 0)
        return;
    //alert("/PluginHandler?type=" + pluginType + "&name=" + name + "&action=getmetadata");
    // 如果选择了插件，则根据选择的插件获取对应的配置信息
    $.ajax({
        url: "/PluginHandler?type=" + pluginType + "&name=" + name + "&action=getmetadata",
        type: 'GET',
        dataType: 'xml',
        timeout: 10000,
        success: function (xml) {
            CreateContainer(xml, name);
        }
    });
}



function CreateContainer(meta, typename) {
    var dataXml;
    var hasValue = false;

    if ($(configDataCtl).val().length > 0) {
        var s = $(configDataCtl).val().replace("<xml>", "<root>");
        s = s.replace("</xml>", "</root>");

        if ($.browser.msie) {
            dataXml = new ActiveXObject("Microsoft.XMLDOM");
            dataXml.async = false;
            dataXml.loadXML("<xml>" + s + "</xml>");
        }
        else {
            dataXml = new DOMParser().parseFromString("<xml>" + s + "</xml>", "text/xml");
        }

        hasValue = true;
    }

    $.each($(meta).find("att"), function (i, att) {
        var attributeRow = templateRow.clone();
        var t = attributeRow.html();
        t = t.replace("$Name$", $(att).attr("Name"));
        t = t.replace("$Description$", $(att).attr("Description"));
        $("#licert").hide();
        var strInputInnerHtml = "";
        var strLink = "";
        var strItem = "";
        var strData = "";
        switch (typename) {
            case "hishop.plugins.payment.alipayassure.assurerequest"://担保交易
                strLink = "　<a href='https://b.alipay.com/order/pidKey.htm?pid=2088101600118305&product=escrow' target='_blank'>获取PID、Key</a>";
                break;
            case "hishop.plugins.payment.alipaydirect.directrequest"://即时交易
                strLink = "　<a href='https://b.alipay.com/order/pidKey.htm?pid=2088101600118305&product=fastpay' target='_blank'>获取PID、Key</a>";
                break;
            case "hishop.plugins.payment.alipay.standardrequest"://双接口
                strLink = "　<a href='https://b.alipay.com/order/pidKey.htm?pid=2088101600118305&product=dualpay' target='_blank'>获取PID、Key</a>";
                break;
            case "hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest":  //银联网关支付
                strLink = "<a href='https://merchant.unionpay.com' target='_blank'>&nbsp;&nbsp;获取商户号</a>";
                $("#licert").show();
                break;
            default:
                strLink = "";
                break;
        };

        if (hasValue) {
            strData = $(dataXml).find($(att).attr("Property")).text();
        }
        var description = $(att).attr("Description");
        if (description != undefined && description != "") {
            description = "<br><p style=\"color:#aaa;\">" + $(att).attr("Description") + "</p>";
        }
        switch (Number($(att).attr("InputType"))) {
            case InputType.TextBox:
                if ($(att).attr("Property") == "SignCertFileName") {
                    strInputInnerHtml = "<input class='forminput form-control' style='width:230px;' readonly='readonly' name='" + $(att).attr("Property") + "' id='" + $(att).attr("Property") + "' value='" + strData + "'\/>";
                }
                else {
                    strInputInnerHtml = "<input class='forminput form-control' style='width:230px;' name='" + $(att).attr("Property") + "' id='" + $(att).attr("Property") + "' value='" + strData + "'\/>";
                }
                if ($(att).attr("Property") == "Partner" || $(att).attr("Property") == "Vmid") {
                    strInputInnerHtml += strLink;
                }
                break;

            case InputType.Password:
                strInputInnerHtml = "<input  class='forminput form-control' style='width:230px;' name='" + $(att).attr("Property") + "' type='password' id='" + $(att).attr("Property") + "' value='" + strData + "'\/>";
                break;

            case InputType.TextArea:
                strInputInnerHtml = "<textarea style='width:230px;' name='" + $(att).attr("Property") + "' id='" + $(att).attr("Property") + "'>" + strData + "<\/textarea>";
                break;

            case InputType.CheckBox:
                var check = strData;
                if (strData.toLowerCase() == "true") {
                    strData = "checked=checked";
                }

                strInputInnerHtml = "<input  type='checkbox' onclick='this.value=this.checked;' name='" + $(att).attr("Property") + "' id='" + $(att).attr("Property") + "' " + strData + " value='" + check + "'\/>";
                
                break;

            case InputType.DropDownList:
                strInputInnerHtml = "<select name='" + $(att).attr("Property") + "' id='" + $(att).attr("Property") + "'>";
                $.each($(att).find("Options").find("Item"), function (j, item) {
                    if (strData == $(item).text())
                        strItem = "<Option selected='selected' value='" + $(item).text() + "'>" + $(item).text() + "<\/Option>";
                    else
                        strItem = "<Option value='" + $(item).text() + "'>" + $(item).text() + "<\/Option>";
                    strInputInnerHtml += strItem;
                });
                strInputInnerHtml += "<\/select>";
                break;
            case InputType.File:
                if (strData != "" && strData != null) {
                    strInputInnerHtml = "<input class='forminput form-control' style='width:230px;display:none;' type='file' name='file_" + $(att).attr("Property") + "' id='file_" + $(att).attr("Property") + "' \/>" + "<span>&nbsp;&nbsp;<input type='text'  name='" + $(att).attr("Property") + "' style='display:none;' id='" + $(att).attr("Property") + "' value='" + strData + "' /><a href=\'" + strData + "'>" + strData + "&nbsp;&nbsp;<a href='javascript:void(0)' fileUrl='" + strData + "' class='delCertFile'>删除重传</a></span>";
                }
                else {
                    strInputInnerHtml = "<input class='forminput form-control' style='width:230px;' type='file' name='" + $(att).attr("Property") + "' id='" + $(att).attr("Property") + "' \/>";
                }
                //                strInputInnerHtml = "<input type='file' style='width:230px;' name='" + $(att).attr("Property") + "' id='" + $(att).attr("Property") + "' />" + strData + "";
                break;
            default:
                break;
        }
        strInputInnerHtml += description;

        t = t.replace("$Input$", strInputInnerHtml);
        attributeRow.html(t);
        attributeRow.attr("rowType", "attributeContent");
        attributeRow.css('display', '');
        attributeRow.appendTo(pluginContainer);
    });
}

$(document).ready(function (e) {
    $(".delCertFile").live("click", function (e) {
        var fileUrl = $(this).attr("fileUrl");
        var fileObj = $(this);
        $.ajax({
            url: '/Admin/Admin.ashx?action=delCertFile',
            type: 'post', dataType: 'json', timeout: 20000,
            data: { action: "delCertFile", FilePath: fileUrl },
            error: function (xm, msg) {
                alert(msg);
            },
            success: function (data) {
                if (data.status == "0") {
                    alert(data.msg);
                }
                $(fileObj).parent().prev().show();
                $(fileObj).parent().hide();
                $(fileObj).parent().find("input[type='text']").remove();
            }
        });
    });
});