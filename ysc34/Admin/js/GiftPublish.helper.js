$(document).ready(function (e) {
    $(".shippingTemplates").bind("change", function (e) {

        var postUrl = "addproduct.aspx?isCallback=true&action=getShippingTemplatesValuationMethod&timestamp=";
        postUrl += new Date().getTime();
        postUrl += "&shippingTemplatesId=" + $(this).val();
        var valuationMethod = getValuationMethod();
        if (valuationMethod != "") {
            if (valuationMethod == "2") {
                $("#weightRow").show();
                $("#volumeRow").hide();
            }
            else if (valuationMethod == "3") {
                $("#volumeRow").show();
                $("#weightRow").hide();
            }
            else {
                $("#volumeRow").hide();
                $("#weightRow").hide();
            }
        }
        else {
            $("#volumeRow").hide();
            $("#weightRow").hide();
        }
    });
    $('.shippingTemplates').trigger("change");
});
//获取计价方式
function getValuationMethod() {
    var valuationMethod = "";
    var selectedIndex = $('option:selected', '.shippingTemplates').index();
    var valuationMethod = $(".shippingTemplates").attr("valuationmethodlist");
    if (selectedIndex != 0) {
        valuationMethod = valuationMethod.split(",")[selectedIndex - 1];
    }
    return valuationMethod;
}
///提交
function doSubmit() {
    if ($("#ctl00_contentHolder_onoffIsPointExchange input").is(':checked')) {
        var needPoint = parseInt($("#ctl00_contentHolder_txtNeedPoint").val());
        if (isNaN(needPoint) || needPoint <= 0) {
            alert("您开启了积分兑换，请输入兑换需要的积分数,必须大于0");
            return false;
        }
    }
    if ($(".shippingTemplates").val() == "") {
        alert("请选择运费模板");
        return false;
    }
    var valuationMethod = getValuationMethod();
    // if ($("#weightRow").is(":visible")) {
    if (valuationMethod == "2") {
        var weight = parseFloat($("#txtWeight").val());
        if (isNaN(weight) || weight < 0 || weight > 1000000) {
            alert("请输入商品重量,数值限制在0-1000000之间");
            $("#weightRow").show();
            return false;
        }
    }
    if (valuationMethod == "3") {
        var volume = parseFloat($("#txtVolume").val());
        if (isNaN(volume) || volume < 0 || volume > 1000000) {
            alert("请输入商品体积,数值限制在0-1000000之间");
            $("#volumeRow").show();
            return false;
        }
    }
    return true;
}