$(document).ready(function () {
    $("input[name='inputQuantity']").bind("blur", function () { chageQuantity(this); }); //立即购买
    $("[name='iDelete']").bind("click", function () {
        var obj = this;
        myConfirm('操作提示', '确定要从购物车里删除该商品吗？', '确认删除', function () {
            deleteCartProduct(obj);
        });
    }); //立即购买
    $("#selectShippingType").bind("change", function () { chageShippingType() });
    $("#selectCoupon").bind("change", function () { chageCoupon() });
    $("#aSubmmitorder").bind("click", function () { submmitorder() });
    $("#btn_gotoPay").bind("click", function () { gotoPay() });
    $("#btnSetTradePassword").bind("click", function () { setTradePassword() });
    $("#payment_close").click(function (e) {
        if ($(this).attr("id") != "payment_close")
            return false;
        var parentOrderId = $("#hidParentOrderId").val();
        if (parentOrderId == "-1") {
            goOrderList();
        }
        else {
            document.location.href = "MemberOrderDetails.aspx?orderId=" + $("#hidOrderId").val();
        }
    })
    $("#tradePassword_close").click(function (e) {
        payment_close$(".sub_dialog").hide();
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
        $(".icon_edit").hide();
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
                    $("#btnSetTradePassword").removeClass('is-visible');;
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

                $("#advancepay").focus();
                document.activeElement.blur();
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

function goOrderList() {
    goToUrl("order-list");
}


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
                        $("#divSetPassword").removeClass('is-visible');;
                        $("#divPublicTip").find("span").html("参数错误");
                        $("#divPublicTip").show().delay(2000).fadeOut();
                    }
                    else if (data.Status == "OK") {
                        $("#divSetPassword").hide();
                        $("#btnSetTradePassword").text("确定");
                        $("#btnSetTradePassword").removeAttr("disabled");
                        $("#hidIsValidTradePassword").val("true");
                        $("#hidHasTradePassword").val("1");
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
    //var paymentTypeId = $("input[name='chk_paymentlist']:checked")

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
                    ToLogin();
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
function deleteCartProduct(obj) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "DeleteCartProduct", skuId: $(obj).attr("skuId") },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                goToShoppingCart();
                //location.href = "/AppShop/ShoppingCart.aspx";
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
                goToShoppingCart();
                //location.href = "/AppShop/ShoppingCart.aspx";
            }
            else {
                alert_h("最多只可购买" + resultData.Status + "件", function () {
                    goToShoppingCart();
                    //location.href = "/AppShop/ShoppingCart.aspx";
                });

            }
        }
    });
}






function chageShippingType() {
    var freight = 0;
    if ($("#selectShippingType").val() != "-1") {
        var selectedShippingType = $("#selectShippingType option:selected").text();
        freight = parseFloat(selectedShippingType.substring(selectedShippingType.lastIndexOf("￥") + 1));
    }

    var discountValue = 0;
    if ($("#selectCoupon").val() != undefined && $("#selectCoupon").val() != "") {
        var selectCoupon = $("#selectCoupon option:selected").text();
        discountValue = parseFloat(selectCoupon.substring(selectCoupon.lastIndexOf("￥") + 1));
    }

    var orderTotal = parseFloat($("#hiddenCartTotal").val()) + freight - discountValue;
    $("#strongTotal").html("¥" + orderTotal.toFixed(2));
}

function chageCoupon() {
    var freight = 0;
    if ($("#selectShippingType").val() != "-1") {
        var selectedShippingType = $("#selectShippingType option:selected").text();
        freight = parseFloat(selectedShippingType.substring(selectedShippingType.lastIndexOf("￥") + 1));
    }

    var discountValue = 0;
    if ($("#selectCoupon").val() != "") {
        var selectCoupon = $("#selectCoupon option:selected").text();
        discountValue = parseFloat(selectCoupon.substring(selectCoupon.lastIndexOf("￥") + 1));
    }

    var orderTotal = parseFloat($("#hiddenCartTotal").val()) + freight - discountValue;
    $("#strongTotal").html("¥" + orderTotal.toFixed(2));
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
            alert_h("请输入正确的抵扣积分数");
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
    //getParam("productSku")
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
        orderSource: 6, deductionPoints: usePoints, needInvoice: needInvoice, invoiceTitle: invoiceTitle, invoiceType: invoiceType, invoiceId: invoiceType == "0" ? 0 : $("#hidInvoiceId").val(), invoiceTaxpayerNumber: invoiceTaxpayerNumber, UseBalance: $("#hidUseBalance").val(), AdvancePayPass: $("#textfieldpassword").val()
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
                    if (resultData.ErrorUrl != undefined && resultData.ErrorUrl != "") {
                        if (resultData.ErrorUrl.toLowerCase() == "shoppingcart") {
                            goToShoppingCart();
                        }
                        else if (resultData.ErrorUrl.toLowerCase() == "default") {
                            goHomePage();
                        }
                        else if (resultData.ErrorUrl.toLowerCase() == "login") {
                            ToLogin();
                        }
                        else {
                            location.href.replace(resultData.ErrorUrl);
                        }
                    }
                    $("#aSubmmitorder").removeAttr("disabled");
                });
                return;
            }
            $("#aSubmmitorder").hide();
            $("#hidOrderId").val(resultData.OrderId);
            $("#hidParentOrderId").val(resultData.ParentOrderId);
            if (resultData.paymentType == "HASPAY") {
                alert_h("订单支付成功。", function (e) {
                    var fightgroupId = parseInt(resultData.FightGroupId);
                    if (!isNaN(fightgroupId) && fightgroupId > 0) {
                        $.ajax({
                            url: "/AppShop/AppShopHandler.ashx",
                            type: 'post', dataType: 'json', timeout: 10000,
                            data: { action: "FightGroupShare", OrderId: resultData.OrderId },
                            success: function (resultData) {
                                var shareJson = "{\"Result\":{\"ShareImage\":\"" + resultData.Result.ShareImage + "\",\"ShareTitle\":\"" + resultData.Result.ShareTitle + "\",\"ShareContent\":\"" + resultData.Result.ShareContent + "\",\"ShareLink\":\"" + resultData.Result.ShareLink + "\"}}";

                                goFightGroupSuccess(resultData.Result.Status, resultData.Result.NeedJoinNumber, shareJson);
                            }
                        });
                    }
                    else {
                        if (resultData.ParentOrderId == "-1")
                            goOrderList();
                        else
                            document.location.replace("MemberOrderDetails.aspx?orderId=" + resultData.OrderId);
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
                    //if ($("#inputPaymentModeId").val() != "1")
                    //    document.location.replace("MemberOrders.aspx?status=1");//线下支付
                    //else {
                    //    //货到付款
                    //    var isOpenStore = $("#hidIsMultiStore").val();
                    //    if (isOpenStore == "0") {
                    //        document.location.replace("MemberOrders.aspx?status=2");
                    //    } else {
                    //        document.location.replace("MemberOrders.aspx");
                    //    }
                    //}
                    goOrderList();
                }
                else {//到店支付
                    goOrderList();
                }
            }
        },
        complete: function () {
            RefreshUpdateUser();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#aSubmmitorder").attr("disabled", "");
            //alert(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus);
        }
    });
}

function getParam(paramName) {
    paramValue = "";
    isFound = false;
    if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
        arrSource = unescape(this.location.search).substring(1, this.location.search.length).split("&");
        i = 0;
        while (i < arrSource.length && !isFound) {
            if (arrSource[i].indexOf("=") > 0) {
                if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                    paramValue = arrSource[i].split("=")[1];
                    isFound = true;
                }
            }
            i++;
        }
    }
    return paramValue;
}
