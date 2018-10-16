$(document).ready(function () {
    $("input[name='inputQuantity']").bind("blur", function () { chageQuantity(this); }); //立即购买
    $("[name='iDelete']").bind("click", function () {
        var obj = this;
        myConfirm('询问', '确定要从购物车里删除该商品吗？', '确认删除', function () {
            deleteCartProduct(obj);
        });
    }); //立即购买
    $("#selectShippingType").bind("change", function () { chageShippingType() });
    $("#selectCoupon").bind("change", function () { chageCoupon() });
    $("#aSubmmitorder").bind("click", function () { submmitorder() });
    $("#btn_gotoPay").bind("click", function () { gotoPay() });
    $("#btnSetTradePassword").bind("click", function () { setTradePassword() });
    $("#divChoosePayType").bind("click", function () {
        event.stopPropagation();
        return true;
    });
    $("#payment_close").click(function (e) {
        if ($(this).attr("id") != "payment_close")
            return false;
        var parentOrderId = $("#hidParentOrderId").val();
        if (parentOrderId == "-1") {
            document.location.href = "MemberOrders?ParentOrderId=" + $("#hidOrderId").val();
        }
        else {
            document.location.href = "MemberOrderDetails?orderId=" + $("#hidOrderId").val();
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
                    $("#btnSetTradePassword").text("确定");
                    $("#btnSetTradePassword").removeAttr("disabled");
                    $("#divSetPassword").removeClass('is-visible');
                    $("#hidIsValidTradePassword").val("true");
                    $("#hidHasTradePassword").val("1");
                    $("#hidIsValidTradePassword").val("true");
                    $("#textfieldpassword").val(confirmPassword);
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
                        $("#divSetPassword").removeClass('is-visible');
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
    });

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
                        $("#errormsg").html("参数错误！");
                        $("#errormsg").show();
                        $("#advancepay").text("确定");
                        $("#advancepay").removeAttr("disabled");
                        $("#advancepay").show();
                        $("#submitloading").hide();
                    }
                    else if (data.Status == "OK") {//密码验证成功
                        $("#divSetPassword").hide();
                        $("#divSetPassword").removeClass('is-visible');
                        $("#hidIsValidTradePassword").val("true");
                        $("#textfieldpassword").val(confirmPassword);
                        $("#hidHasTradePassword").val("1");
                        SetUseBalance(true);
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


function advancePayPassVerify(orderId, advancePayPass, parentOrderId, btnType) {

}
function deleteCartProduct(obj) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "DeleteCartProduct", skuId: $(obj).attr("skuId") },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                location.href = "ShoppingCart";
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
    if ($("#selectShipTo").val() == "" || $("#selectShipTo").val() == undefined) {
        alert_h("请选择或添加收货地址");
        return false;
    }
    if ($("#inputShippingModeId").val() == "" || $("#inputShippingModeId").val() == undefined) {
        alert_h("请选择配送方式");
        return false;
    }
    if ($("#hidDeliveryTime").val() == "" || $("#hidDeliveryTime").val() == undefined) {
        alert_h("请选择送货时间");
        return false;
    }
    var paymentId = $("#inputPaymentModeId").val();
    if (paymentId != "0" && paymentId != "1" && paymentId != "-3" && paymentId != "2") {
        if ($("#ulPayType").find("li:visible").length <= 0) {
            alert_h("商城暂未配置支付方式，请稍后提交");
        }
        else {
            alert_h("请选择支付方式");
        }
        return false;
    }

    if (($("#selectShipLatLng").val() == "" || $("#selectShipLatLng").val() == undefined) && $("#hidIsMultiStore").val() == "1") {
        $("#divUpdateAddressTip").show().delay(2000).fadeOut();
        return false;
    }

    if ($("#inputShippingModeId").val() == "-2" && $("#hidHasStoresInCity").val() == "true" && $("#hidStoreId").val() == "0" && ($("#hidChooseStoreId").val() == "0" || $("#hidChooseStoreId").val() == "")) {
        alert_h("您选择了上门自提，请选择自提的门店");
        return false;
    }
    if ($("#inputShippingModeId").val() == "-2" && $("#hidHasStoresInCity").val() == "false" && $("#hidStoreId").val() == "0" && $("#hidIsMultiStore").val() == "1") {
        alert_h("您选择了上门自提，但本市无可自提的门店，请选择其他配送方式");
        return false;
    }

    var needInvoice = false;
    var invoiceTitle = "";
    if ($('#chkIsNeedInvoice').is(':checked')) {
        invoiceTitle = $("#invoiceTitle").val().Trim();
        needInvoice = true;
    }
    var invoiceType = $("#hidInvoiceType").val();
    var invoiceTaxpayerNumber = $("#invoiceTaxpayerNumber").val();
    var invoiceId = parseInt($("#hidInvoiceId").val());
    if (isNaN(invoiceId) || invoiceId < 0) {
        invoiceId = 0;
    }
    if (needInvoice && invoiceType != "0" && invoiceId == 0) {
        alert_h("请填写正确的发票信息,并且保存");
        return false;
    }
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
    if ((tempDate.getTime() - lastSubmitTime.getTime()) < 10000 && submitTimes > 0) {
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
        action: "Submmitorder", shippingType: $("#inputShippingModeId").val(), paymentType: $("#inputPaymentModeId").val(), storeId: storeId, chooseStoreId: chooseStoreId, storeCount: $("#hidstorecount").val(), couponCode: $("#htmlCouponCode").val(), shippingId: $('#selectShipTo').val(),
        productSku: productSku, buyAmount: getParam("buyAmount"), from: getParam("from"), shiptoDate: $("#hidDeliveryTime").val(), remark: $('#remark').val(),
        orderSource: GetOrderSouce(), deductionPoints: usePoints, needInvoice: needInvoice, invoiceTitle: invoiceTitle, invoiceType: invoiceType, invoiceId: invoiceType == "0" ? 0 : $("#hidInvoiceId").val(), invoiceTaxpayerNumber: invoiceTaxpayerNumber, UseBalance: $("#hidUseBalance").val(), AdvancePayPass: $("#textfieldpassword").val()
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
                            document.location.replace("MemberOrderDetails?orderId=" + resultData.OrderId);//线下支付
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

