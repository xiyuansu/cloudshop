

function CheckInputItem() {
    var itemsJson = $.parseJSON($("#hidInputItemsJson").val());
    if (itemsJson.list.length > 0) {
        submitItemJson = itemsJson;
        var itemvalue = "";
        for (var i = 0; i < repcount; i++) {
            for (var j = 0; j < itemsJson.list.length; j++) {
                var idx = j;
                var obj = itemsJson.list[idx];
                if (i == 0) {
                    submitItemJson.list[idx].InputFileValues.length = 0;//清空
                }
                var isRequired = obj.IsRequired;
                if (obj.InputFieldType != 6) {
                    itemvalue = $("#item_" + i + "_" + idx).val().Trim();
                }
                else {
                    var tempvalue = $('#item_imageupload_' + i + ' span[name="imageupload"]').hishopUpload("getImgSrc");
                    if (tempvalue.length > 0) {
                        itemvalue = tempvalue.join(",");
                    }
                    else {
                        itemvalue = "";
                    }
                }
                if (isRequired && itemvalue == "") {
                    ShowMsg("第" + (i + 1) + "表单的 " + obj.InputFieldTitle + ",不能为空.", false);
                    //$("#item_" + i + "_" + idx).focus();
                    return false;
                }
                if (obj.InputFieldType == 2) {
                    if (!itemvalue.IsDate() && itemvalue != "") {
                        ShowMsg("第" + (i + 1) + "表单的 " + obj.InputFieldTitle + ",请输入正确的日期格式.", false);
                        // $("#item_" + i + "_" + idx).focus();
                        return false;
                    }
                }
                if (obj.InputFieldType == 3) {
                    if (!itemvalue.IsCardNo() && itemvalue != "") {
                        ShowMsg("第" + (i + 1) + "表单的 " + obj.InputFieldTitle + ",请输入正确的身份证号码.", false);
                        // $("#item_" + i + "_" + idx).focus();
                        return false;
                    }
                }
                if (obj.InputFieldType == 4) {
                    if (itemvalue != "" && !Reg_mobbile.test(itemvalue) && !Reg_phoneWithArea.test(itemvalue) && !Reg_phoneRegNoArea.test(itemvalue)) {
                        ShowMsg("第" + (i + 1) + "表单的 " + obj.InputFieldTitle + ",请输入正确的手机号码.", false);
                        // $("#item_" + i + "_" + idx).focus();
                        return false;
                    }
                }
                if (obj.InputFieldType == 5) {
                    if (itemvalue != "") {
                        var intvalue = parseInt(itemvalue);
                        var floatvalue = parseFloat(itemvalue);
                        if (isNaN(intvalue) && isNaN(floatvalue)) {
                            ShowMsg("第" + (i + 1) + "表单的 " + obj.InputFieldTitle + ",请输入正确数字类型.", false);
                            // $("#item_" + i + "_" + idx).focus();
                            return false;
                        }
                    }
                }
                submitItemJson.list[idx].InputFileValues.push(itemvalue);//保存值
            }
        }
    }

    return true;
}

$(document).ready(function () {
    $("input[name='inputQuantity']").bind("blur", function () { chageQuantity(this); }); //立即购买
    $("#selectCoupon").bind("change", function () { chageCoupon() });
    $("#aSubmmitorder").bind("click", function () { submmitorder() });
    $("#btn_gotoPay").bind("click", function () { gotoPay() });
    $("#btnSetTradePassword").bind("click", function () { setTradePassword() });
    $("#payment_close").click(function (e) {
        if ($(this).attr("id") != "payment_close")
            return false;
        var parentOrderId = $("#hidParentOrderId").val();
        if (parentOrderId == "-1") {
            document.location.href = "MemberOrders?ParentOrderId=" + $("#hidOrderId").val();
        }
        else {
            document.location.href = "ServiceMemberOrderDetails?orderId=" + $("#hidOrderId").val();
        }
    });
    $("#tradePassword_close").click(function (e) {
        $(".sub_dialog").hide();
        $("#advancepay").show();
        $("#errormsg").hide();
        $("#submitloading").hide();
        $("#advancepay").text("确定");
        $("#advancepay").removeAttr("disabled");
        $("#chkIsUseBalance").prop("checked", false);
        SetUseBalance(false);
    });
    $('.sub_order').on('click', function () {
        $('#divChoosePayType').addClass('is-visible');
    });

    var strform = "";
    if (getParam("from") != "") {
        strform = getParam("from").toLowerCase();
    }
    //隐藏 购物车编辑
    if (strform != "") {
        $("#cartEdit").hide();
        if (strform == "presale") {
            $("#spordertxt").text("定金：")
        }
        if (strform == "prize") {
            $("#i_jiang").show();
        }

    }

    $("#advancepay").click(function () {
        if ($("#hidHasTradePassword").val() == "0") {
            $("#divSetPassword").addClass('is-visible');
            $(".sub_dialog").hide();
            return false;
        }
        $("#errormsg").hide();
        $(this).hide();
        $("#submitloading").show();
        $(this).attr("disabled", "disabled");
        var AdvancePayPass = $("#textfieldpassword").val();
        if (!AdvancePayPass) {
            $("#errormsg").html("请输入交易密码!");
            $("#errormsg").show();
            $(this).show();
            $("#submitloading").hide();
            return;
        }
        $(this).text("确认中..");

        $.ajax({
            url: "/API/VshopProcess.ashx",
            type: "post",
            dataType: "json",
            timeout: 10000,
            data: {
                action: "AdvancePayPassVerify",
                PayAmount: CalculateUseBalance,
                AdvancePayPass: AdvancePayPass
            },
            async: false,
            success: function (data) {
                if (data.Status == undefined) {
                    $("#btnSetTradePassword").text("确定");
                    $("#btnSetTradePassword").removeAttr("disabled");
                    $("#divSetPassword").removeClass('is-visible');
                    $("#divPublicTip").find("span").html("参数错误");
                    $("#divPublicTip").show().delay(2000).fadeOut();
                }
                else if (data.Status == "OK") {//密码验证成功
                    $(".sub_dialog").hide();

                    $("#divSetPassword").removeClass('is-visible');
                    $("#hidIsValidTradePassword").val("true");
                    SetUseBalance();
                } else {
                    if (data.Status == "003") {

                        $("#errormsg").html("<a href=\"ForgotTradePassword\" style=\"color:red;\">密码错误,<span style=\"color:#000\">忘记交易密码，</span><span style=\"color:blue\">点击去重置</span></a>");
                        $("#errormsg").show();
                        $("#advancepay").show();
                        $("#submitloading").hide();
                        $("#advancepay").text("确定");
                        $("#advancepay").removeAttr("disabled");
                    }
                    else if (data.Status == "NoTradePassword") {
                        $(".sub_dialog").hide();
                        $("#divSetPassword").addClass('is-visible');

                    }
                    else {
                        $("#errormsg").html(data.Message);
                        $("#errormsg").show();
                        $("#advancepay").text("确定");
                        $("#advancepay").show();
                        $("#submitloading").hide();
                        $("#advancepay").removeAttr("disabled");
                    }
                }
            }
        });
    });
    //取消支付则关闭余额支付
    $("#canclepay").click(function () {
        $(".sub_dialog").hide();
        $("#advancepay").show();
        $("#errormsg").hide();
        $("#submitloading").hide();
        $("#advancepay").text("确定");
        $("#advancepay").removeAttr("disabled");
        $("#chkIsUseBalance").prop("checked", false);
        SetUseBalance(false);
    })

});

function setTradePassword() {
    var password = $("#txtTradePassword").val().trim();
    var confirmPassword = $("#txtTradePasswordAgain").val().trim();
    if (password.length == 0) {
        //alert_h("请输入交易密码！"); return false;
        $("#divSetPasswordTitle").html("请输入交易密码");
        return false;
    }
    if (password.length < 6 || password.length > 20) {
        //alert_h("交易密码限制为6-20个字符！"); return false;
        $("#divSetPasswordTitle").html("交易密码限制为6-20个字符");
        return false;
    }
    if (confirmPassword.length == 0) {
        //alert_h("请确认交易密码！"); return false;
        $("#divSetPasswordTitle").html("请确认交易密码");
        return false;
    }
    if (password != confirmPassword) {
        //alert_h("两次输入的交易密码不一致！"); return false;
        $("#divSetPasswordTitle").html("两次输入的交易密码不一致");
        return false;
    }
    var data = {};
    data.password = password;
    data.confirmPassword = confirmPassword;
    $("#btnSetTradePassword").attr("disabled", "disabled");
    $.post("/api/VshopProcess.ashx?action=OpenBalance", data, function (json) {
        if (json.success || json.success == "true") {
            $("#btnSetTradePassword").text("确认中..");
            var orderId = $("#hidOrderId").val();
            var parentOrderId = $("#hidParentOrderId").val();
            $.ajax({
                url: "/API/VshopProcess.ashx",
                type: "post",
                dataType: "json",
                timeout: 10000,
                data: {
                    action: "AdvancePayPassVerify",
                    PayAmount: CalculateUseBalance,
                    AdvancePayPass: password
                },
                async: false,
                success: function (data) {
                    if (data.Status == undefined) {
                        $("#btnSetTradePassword").text("确定");
                        $("#btnSetTradePassword").removeAttr("disabled");
                        $("#divSetPassword").removeClass('is-visible');
                        $("#divPublicTip").find("span").html("参数错误");
                        $("#divPublicTip").show().delay(2000).fadeOut();
                    }
                    else if (data.Status == "OK") {
                        $(".sub_dialog").hide();

                        $("#divSetPassword").removeClass('is-visible');
                        $("#hidIsValidTradePassword").val("true");
                        $("#textfieldpassword").val(confirmPassword);
                        SetUseBalance();
                    }
                    else {
                        $("#errormsg").html(data.Message);
                        $("#errormsg").show();
                        $("#btnSetTradePassword").text("确定");
                        $("#btnSetTradePassword").show();
                        $("#btnSetTradePassword").removeAttr("disabled");
                        $("#divPublicTip").find("span").html(data.Message);
                        $("#divPublicTip").show().delay(2000).fadeOut();
                    }
                }
            });
        }
        else {
            //alert_h(json.msg);
            $("#divSetPasswordTitle").html(json.msg);
            return false;
        }
    });
}

function gotoPay() {
    var orderId = $("#hidOrderId").val();
    var parentOrderId = $("#hidParentOrderId").val();
    if (orderId == "") {
        alert_h("错误的订单信息");
        //$(".att-popup-close").trigger("click");
        $("#payment_close").trigger("click");
        return false;
    }
    if ($("input[name='chk_paymentlist']:checked").length == 0) {
        alert_h("请选择支付方式");
        return false;
    }
    var paymentTypeId = $("input[name='chk_paymentlist']:checked").val();

    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "UpdatePaymentType", paymentTypeId: paymentTypeId, orderId: orderId },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                location.replace(resultData.ToUrl);
            }
            else if (resultData.Status == "NoLogined") {
                alert_h(resultData.Message, function (e) {
                    location.replace("MemberCenter");
                });
            }
            else {
                alert_h(resultData.Message, function (e) {
                    //$(".att-popup-close").trigger("click");
                    $("#payment_close").trigger("click");
                });
            }
        }
    });

}




function chageQuantity(obj) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "ChageQuantity", skuId: $(obj).attr("skuId"), quantity: parseInt($(obj).val()) },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                location.href = "ShoppingCart";
            }
            else {
                alert_h("最多只可购买" + resultData.Status + "件", function () {
                    location.href = "ShoppingCart";
                });

            }
        }
    });
}


var lastSubmitTime = new Date();
var submitTimes = 0;
function submmitorder() {
    var isCheck = CheckInputItem();
    if (!CheckInputItem()) {
        return false;
    }
    var paymentId = $("#inputPaymentModeId").val();
    if (paymentId != "0" && paymentId != "1" && paymentId != "-3" && paymentId != "2") {
        if ($("#ulPayType").find("li:visible").length <= 0) {
            ShowMsg("商城暂未配置支付方式，请稍后提交", false);
        }
        else {
            ShowMsg("请选择支付方式", false);
        }
        return false;
    }


    var needInvoice = false;
    var invoiceTitle = "";
    if ($('#chkIsNeedInvoice').is(':checked')) {
        invoiceTitle = $("#invoiceTitle").val().Trim();
        if (invoiceTitle == "") {
            ShowMsg("请填写发票抬头", false);
            return false;
        }
        needInvoice = true;
    }
    var invoiceType = $("#hidInvoiceType").val();
    var invoiceTaxpayerNumber = $("#invoiceTaxpayerNumber").val();
  
    var usePoints = 0;
    if ($('#chkIsUsePoints').is(':checked')) {
        var usePoints = parseInt($('#txtUsePoints').val());
        var maxPoints = parseInt($("#lblMaxPoints").html());
        if (isNaN(maxPoints) || maxPoints < 0) { maxPoints = 0; }
        if (isNaN(usePoints) || usePoints < 0 || usePoints > maxPoints) {
            alert_h("请输入正确的抵扣积分数" + usePoints + "-" + maxPoints);
            location.href = "#anchor_usepoint"
            return false;
        }
    }
    //10秒内重复点击直接返回false
    var tempDate = new Date();
    if ((tempDate.getTime() - lastSubmitTime.getTime()) < 3000 && submitTimes > 0) {
        lastSubmitTime = new Date();
        submitTimes += 1;
        return false;
    }
    lastSubmitTime = new Date();
    submitTimes += 1;
    try {
        if (stockError != undefined && stockError == true) {
            alert_h(stockErrorInfo);
            return false;
        }
    }
    catch (e) { }

    var productSku;
    productSku = $("#hdCurrentckIds").val();
    //计算门店Id
    var submitStoreId = 0;
    var storeId = $("#hidStoreId").val();
    var chooseStoreId = $("#hidChooseStoreId").val();
    //storeId = parseInt(storeId);
    //if (!isNaN(storeId) && storeId > 0) {
    //    submitStoreId = storeId;
    //}
    //else {
    //    storeId = $("#hidChooseStoreId").val();
    //    storeId = parseInt(storeId);
    //    if (!isNaN(storeId) && storeId > 0) {
    //        submitStoreId = storeId;
    //    }
    //}
    var params = {
        action: "ServiceProductSubmitorder", paymentType: $("#inputPaymentModeId").val(), storeId: storeId, chooseStoreId: chooseStoreId, storeCount: $("#hidstorecount").val(), couponCode: $("#htmlCouponCode").val(),
        productSku: productSku, buyAmount: getParam("buyAmount"), from: getParam("from"), remark: $('#remark').val(),
        orderSource: GetOrderSouce(), deductionPoints: usePoints, needInvoice: needInvoice, invoiceTitle: invoiceTitle, invoiceType: invoiceType, invoiceId: $("#hidInvoiceId").val(), invoiceTaxpayerNumber: invoiceTaxpayerNumber, InputItems: JSON.stringify(submitItemJson.list), UseBalance: $("#hidUseBalance").val(), AdvancePayPass: $("#textfieldpassword").val()
    };
    if (params.from == 'countDown')
        params.countDownId = $('#countdownHiddenBox').val();
    else if (params.from == 'groupBuy')
        params.groupbuyId = $('#groupbuyHiddenBox').val();
    else if (params.from == 'fightGroup') {
        params.fightGroupId = $('#fightGroupHiddenBox').val();
        params.fightGroupActivityId = $('#fightGroupActivityHiddenBox').val();
    }
    else if (params.from == "combinationbuy") {
        params.combinaid = getParam("combinaid");
    }
    else if (params.from == "preSale") {
        params.presaleid = getParam("presaleid");
    }
    else if (params.from == "prize") {
        params.recordid = getParam("recordid");
    }
    $("#aSubmmitorder").attr("disabled", "disabled");

    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 20000,
        data: params,
        success: function (resultData) {
            if (resultData.ErrorMsg != "" && resultData.ErrorMsg != undefined) {
                alert_h(resultData.ErrorMsg, function (e) {
                    $("#aSubmmitorder").removeAttr("disabled");
                    if (resultData.ErrorUrl != "" && resultData.ErrorUrl != undefined) {
                        document.location.href = resultData.ErrorUrl;
                    }
                });
                return;
            }
            $("#aSubmmitorder").hide();
            $("#hidOrderId").val(resultData.OrderId);
            $("#hidParentOrderId").val(resultData.ParentOrderId);
            if (resultData.paymentType == "HASPAY") {
                alert_h("订单支付成功。", function (e) {
                    if (GetOrderSouce() == 3 && resultData.FightGroupId != "0") {
                        document.location.replace("FightGroupSuccess?orderId=" + resultData.OrderId);
                    } else {
                        if (resultData.ParentOrderId == "-1")
                            document.location.replace("MemberOrders?ParentOrderId=" + resultData.OrderId);
                        else
                            document.location.replace("ServiceMemberOrderDetails?orderId=" + resultData.OrderId);//线下支付
                    }
                });
            } else {
                //如果是在线支付方式则显示支付方式选择
                if ($("#inputPaymentModeId").val() == "0") {//在线支付
                    //$('.att-popup').addClass('is-visible');
                    $("#divChoosePayType").addClass('is-visible');
                    $("#aChoicePayment").show();
                }
                else if ($("#inputPaymentModeId").val() != "-3") {
                    if ($("#inputPaymentModeId").val() != "1")
                        document.location.replace("MemberOrders?status=1");//线下支付
                    else {
                        //货到付款
                        var isOpenStore = $("#hidIsMultiStore").val();
                        if (isOpenStore == "0") {
                            document.location.replace("MemberOrders?status=2");
                        } else {
                            document.location.replace("MemberOrders");
                        }
                    }
                }
                else {//到店支付
                    document.location.replace("MemberOrders?Status=999");
                }
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#aSubmmitorder").attr("disabled", "");
        }
    });
}


function is_weixn() {
    var ua = navigator.userAgent.toLowerCase();
    if (ua.match(/MicroMessenger/i) == "micromessenger") {
        return true;
    } else {
        return false;
    }
}

function GetOrderSouce() {
    var iSourceOrder = 1;
    url = document.location.href.toLowerCase();
    if (url.indexOf("/vshop/") > -1) iSourceOrder = 3;
    if (url.indexOf("/wapshop/") > -1) iSourceOrder = 4;
    if (url.indexOf("/appshop/") > -1) iSourceOrder = 6;
    if (url.indexOf("/alioh/") > -1) iSourceOrder = 5;
    return iSourceOrder;
}

