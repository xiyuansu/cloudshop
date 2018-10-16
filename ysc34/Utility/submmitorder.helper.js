var bg = "<div class='modal_qt'></div>";
function setTradePassword() {
    var password = $("#txtTradePassword").val().trim();
    var confirmPassword = $("#txtTradePasswordAgain").val().trim();
    if (password.length == 0) {
        //alert_h("请输入交易密码！"); return false;
        alert("请输入交易密码");
        return false;
    }
    if (password.length < 6 || password.length > 20) {
        //alert_h("交易密码限制为6-20个字符！"); return false;
        alert("交易密码限制为6-20个字符");
        return false;
    }
    if (confirmPassword.length == 0) {
        //alert_h("请确认交易密码！"); return false;
        alert("请确认交易密码");
        return false;
    }
    if (password != confirmPassword) {
        //alert_h("两次输入的交易密码不一致！"); return false;
        alert("两次输入的交易密码不一致");
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
                        $("#divSetPassword").hide();
                    }
                    else if (data.Status == "OK") {
                        $(".sub_dialog").hide();
                        $(".modal_qt").remove();
                        $("#divSetPassword").hide();
                        $("#hidIsValidTradePassword").val("true");
                        $("#hidTradePassword").val(confirmPassword);
                        $("#hidHasTradePassword").val("1");
                        SetUseBalance();
                    }
                    else {
                        $("#errormsg").html(data.Message);
                        $("#errormsg").show();
                        $("#btnSetTradePassword").text("确定");
                        $("#btnSetTradePassword").show();
                        $("#btnSetTradePassword").removeAttr("disabled");
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

function CalculationTax() {
    var totalPrice = parseFloat($("#hidTotalPrice").val());
    if (isNaN(totalPrice) || totalPrice < 0) { totalPrice = 0; }
    var couponPrice = parseFloat($("#SubmmitOrder_litCouponAmout").html());
    if (isNaN(couponPrice) || couponPrice < 0) { couponPrice = 0; }

    var taxRate = parseFloat($("#SubmmitOrder_litTaxRate").html());
    if (isNaN(taxRate) || taxRate < 0) { taxRate = 0; }
    var pointPrice = parseFloat($("#SubmmitOrder_litPointAmount").html());
    if (isNaN(pointPrice) || pointPrice < 0) { pointPrice = 0; }

    var tax = (totalPrice.toSub(couponPrice).toSub(pointPrice)).toMul(taxRate).toDiv(100);
    if (tax < 0) { tax = 0; }
    $("#SubmmitOrder_lblTax").html(tax.toFixed(2));
    if (tax > 0) {
        $("#divTaxRate").show();
    }
}

$(function () {
    $("#advancepay").click(function () {
        if ($("#hidHasTradePassword").val() == "0") {
            $("#divSetPassword").show();
            $(".sub_dialog").hide();
            return false;
        }
        $("#errormsg").hide();
        $(this).hide();
        //$("#submitloading").show();
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
                    $("#divSetPassword").hide();
                }
                else if (data.Status == "OK") {//密码验证成功
                    $(".sub_dialog").hide();
                    $("#hidTradePassword").val(AdvancePayPass);
                    $("#divSetPassword").hide();
                    $("#hidIsValidTradePassword").val("true");
                    $(".modal_qt").remove();
                    SetUseBalance();
                } else {
                    if (data.Status == "003") {

                        $("#errormsg").html("<a href=\"/User/ForgotTradePassword\" style=\"color:red;\">密码错误,<span style=\"color:#000\">忘记交易密码，</span><span style=\"color:blue\">点击去重置</span></a>");
                        $("#errormsg").show();
                        $("#advancepay").show();
                        $("#submitloading").hide();
                        $("#advancepay").text("确定");
                        $("#advancepay").removeAttr("disabled");
                    }
                    else if (data.Status == "NoTradePassword") {
                        $(".sub_dialog").hide();
                        $("#divSetPassword").show();

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
    $("#btnSetTradePassword").bind("click", function () { setTradePassword() });
    $("#tradePassword_close,#setTradePassword_close,#canclepay,#canclepay1").click(function (e) {
        $(".modal_qt").remove();
        $(".sub_dialog").hide();
        $("#advancepay").show();
        $("#errormsg").hide();
        // $("#submitloading").hide();
        $("#divSetPassword").hide();
        $("#advancepay").text("确定");
        $("#advancepay").removeAttr("disabled");
        $("#chkIsUseBalance").iCheck("uncheck");
        SetUseBalance(false);
    });
    var strform = "";
    if (getParam("from") != "") {
        strform = getParam("from").toLowerCase();
    }

    //隐藏 购物车编辑
    if (strform == "combinationbuy" || strform == "groupbuy" || strform == "countdown"
        || strform == "fightgroup" || strform == "signbuy" || strform == "presale") {
        $("#backToShoppingCart").hide();
    }

    $("#user_shippingaddress").hide();
    if ($("#SubmmitOrder_hidIsAnonymous").val() == "1") {
        $("#user_shippingaddress").show();
    }
    if ($("#hidHasSupplierProduct").val() == "1" || $("#hidIsGiftOrder").val() == "1") {
        $(".payment_way ul li").eq(0).show();
        $(".payment_way ul li").eq(1).hide();
        $(".payment_way ul li").eq(2).hide();
        $(".payment_way ul li").eq(3).hide();
    }
    //如果开启了线下付款
    var paymentId_PayOffline = parseInt($("#hidPaymentId_Offline").val());
    if (!isNaN(paymentId_PayOffline) && paymentId_PayOffline > 0 && $("#hidHasSupplierProduct").val() != "1" && $("#hidIsGiftOrder").val() != "1") {
        $(".payment_way ul li").eq(3).show();
    }

    if (($("#hidGetgoodsOnStores").val() != "false" || $("#hidIsCloseStoreButGetGoods").val() != "false") && $("#divProductList") && $("#hidHasSupplierProduct").val() != "1" && $("#hidIsGiftOrder").val() != "1") {
        $(".deliver_way ul li").eq(1).show();
        btnziti();
        var paymentId_PayAfterGetGoods = parseInt($("#hidPaymentId_Podrequest").val());
        //如果后台配置了货到付款方式则显示货到付款，否则不显示
        if (!isNaN(paymentId_PayAfterGetGoods) && paymentId_PayAfterGetGoods > 0 && $("#hidHasSupplierProduct").val() != "1" && $("#hidIsGiftOrder").val() != "1") {
            $(".payment_way ul li").eq(1).show();
        }
        else {
            $(".payment_way ul li").eq(1).hide();
        }
    } else {
        btnDeliverWay(0);
        $(".deliver_way ul li").eq(1).hide();

        var paymentId_PayAfterGetGoods = parseInt($("#hidPaymentId_Podrequest").val());
        //如果后台配置了货到付款方式则显示货到付款，否则不显示
        if (!isNaN(paymentId_PayAfterGetGoods) && paymentId_PayAfterGetGoods > 0 && $("#hidHasSupplierProduct").val() != "1" && $("#hidIsGiftOrder").val() != "1") {
            $(".payment_way ul li").eq(1).show();
        }
        else {
            $(".payment_way ul li").eq(1).hide();
        }
        $(".payment_way ul li").eq(2).hide();
        //var selectedRegionId = GetSelectedRegionId();
        //ResetShipingModel(selectedRegionId);
    }
    if ($("#hidIsCloseStoreButGetGoods").val() != "false") {
        $("#divGetGoodsRemark").hide();
    }
    //默认支付方式为在线支付
    var onlinePayCount = parseInt($("#hidOnlinePayCount").val());
    if (onlinePayCount > 0) {
        $(".payment_way ul li").eq(0).show();
        $(".payment_way ul li").eq(0).addClass("select");
        $("#SubmmitOrder_inputPaymentModeId").val("0");

    }
    else {
        if ($(".payment_way ul li").eq(1).is(":visible")) {
            $("#SubmmitOrder_inputPaymentModeId").val($("#hidPaymentId_Podrequest").val());
            $(".payment_way ul li").eq(1).addClass("select");
        }
        else if ($(".payment_way ul li").eq(2).is(":visible")) {
            $("#SubmmitOrder_inputPaymentModeId").val("-3");
            $(".payment_way ul li").eq(2).addClass("select");
        }
        else if ($(".payment_way ul li").eq(3).is(":visible")) {
            $("#SubmmitOrder_inputPaymentModeId").val($("#hidPaymentId_Offline").val());
            $(".payment_way ul li").eq(3).addClass("select");
        }
        else {
            $("#SubmmitOrder_inputPaymentModeId").val("");
            // $("#SubmmitOrder_btnCreateOrder").attr("disabled", true);
        }
    }
    //默认配送方式为平台快递配送
    $(".deliver_way ul li").eq(0).addClass("select");
    $("#SubmmitOrder_inputShippingModeId").val("0");
    $('.step ul li').click(function () {
        if ($(this).parents(".deliver_way").length == 0) {
            $(this).addClass('select').siblings().removeClass('select');
        }
    })

    if ($("#hidCanUsePoint").val() == "false") {
        $("#divPoint").hide();
    }
    //匿名用户隐藏优惠券，促销信息，和可得积分 等信息
    if ($("#SubmmitOrder_hidIsAnonymous").val() == "1") {//GroupBuy  CountDown  Bundling
        $("#divPoint").hide();
        $("#divPromtion").hide();
        $("#divCoupons").hide();
        $("#selectCoupon .list").hide();
        $("#divGetPoints").hide();
        $("#setDefaultRow").hide();
    }

    //如果没有可以选择的优惠券则隐藏选择,捆绑销售不能使用优惠券
    if ($("#SubmmitOrder_CmbCoupCode option").length == 1) {
        $("#divCoupons").hide();
        $("#selectCoupon .list").hide();
    }

    //支付方式点击
    $(".payment_way ul li").click(function (e) {
        var index = $(".payment_way ul li").index($(this));
        if (index == 0) {
            $("#SubmmitOrder_inputPaymentModeId").val("0");
        }
        else if (index == 1) {
            $("#SubmmitOrder_inputPaymentModeId").val($("#hidPaymentId_Podrequest").val());
        }
        else if (index == 3) {
            $("#SubmmitOrder_inputPaymentModeId").val($("#hidPaymentId_Offline").val());
        }
        else if (index == 2) {
            //到店支付
            $("#SubmmitOrder_inputPaymentModeId").val("-3");
        }
    });
    //配送方式点击
    $(".deliver_way ul li").click(function (e) {
        var index = $(".deliver_way ul li").index($(this));
        if (index != 0) {
            $("#SubmmitOrder_lblShippModePrice").html("0.00");
        }
        btnDeliverWay(index);
        if (index == 0) {
            var selectedRegionId = GetSelectedRegionId();
            CalculateFreight(selectedRegionId);
        }
        SetUseBalance();
    });

    $("#SubmmitOrder_txtUsePoints").change(function (e) {
        onPointChange(this);
    });
    //选择了使用积分
    $("#chkIsUsePoints").on('ifClicked', function (event) {
        onPointCheck(true);
    });
    //取消了使用积分
    $("#chkIsUsePoints").on('ifUnchecked', function (event) {
        onPointCheck(false);
    });

    $("#SubmmitOrder_CmbCoupCode").combobox({ editable: false });
    $("#SubmmitOrder_drpShipToDate").combobox({ editable: false });

})
$(document).ready(function () {
    //###############门店自提################
    $('.ziti_btn').click(function () {
        //自动选中上门自提
        if ($('.ziti').length == 0) { return false; }//按钮被禁用
        $("#SubmmitOrder_inputShippingModeId").val("-2");
        if ($("#SubmmitOrder_inputShippingModeId").val() != "-2") {
            $("#SubmmitOrder_hidStoreId").val("");
        }

        $(".deliver_way ul li").eq(index).addClass('select').siblings().removeClass('select');
        $(".payment_way ul li").eq(1).hide();
        $(".payment_way ul li").eq(3).hide();
        $(".Order_deliver_time").hide();

        if ($.trim($(".ziti_con").html()) == "")
            $("#SubmmitOrder_hidStoreId").val("");
        if ($("#hidGetgoodsOnStores").val() != "false" && $("#hidHasSupplierProduct").val() != "1" && $("#hidIsGiftOrder").val() != "1") {
            $(".payment_way ul li").eq(2).show();
        }
        else {
            $(".payment_way ul li").eq(2).hide();
            if ($("#SubmmitOrder_inputPaymentModeId").val() == "-2") {
                $("#SubmmitOrder_inputPaymentModeId").val("");
            }
        }
        if ($("#hidIsCloseStoreButGetGoods").val() == "false") {
            $('.mendian').css('display', 'block');
            $('.zhezhao').css('display', 'block');
        }
        btnziti();

    });

    //################-END-##################
    //if ($("#regionSelectorValue").val().trim().length > 0) {
    //    ResetShipingModel($("#regionSelectorValue").val());
    //}
    if ($("#SubmmitOrder_hidCanPointUseWithCoupon").val() == "False" && $("#SubmmitOrder_CmbCoupCode").val() != "" && $("#SubmmitOrder_CmbCoupCode").val() != "0") {
        $("#divPoint").hide();
    }

    // 如果默认选中了一个收货地址
    if ($("#addresslist .list").length > 0) {

        $("#user_shippingaddress").css("display", "none");
        var firstshippId = $("#addresslist .list:first").addClass("select").find("input[type='hidden']").eq(0).val();
        if (firstshippId != "" && firstshippId != "undefined" && parseInt(firstshippId) > 0) {
            ResetAddress(firstshippId);
        }
    } else {
        $("#btnaddr").hide();
        $("#setDefaultRow").hide();
        $("#user_shippingaddress").show();
    }

    var orderDeductibleMoney = parseFloat($("#SubmmitOrder_lblDeductibleMoney").html());
    if (isNaN(orderDeductibleMoney) || orderDeductibleMoney <= 0) {
        $("#divPromtion").hide();
    }
    var orderCouponAmout = parseFloat($("#SubmmitOrder_litCouponAmout").html());
    if (isNaN(orderCouponAmout) || orderCouponAmout <= 0) {
        $("#divCoupons").hide();
    }


    //var orderShippModePrice = parseFloat($("#SubmmitOrder_lblShippModePrice").html());
    //if (isNaN(orderShippModePrice) || orderShippModePrice <= 0) {
    //    $("#divFreight").hide();
    //}


    //根据订单金额设置支付列表是否显示
    // SetPayMentList();

    $("#addresslist .list").live("click", function () {
        $(this).addClass("select").siblings().removeClass("select");
        $("#regionSelectorValue").val($(this).attr("regionid"));
        btnziti();
        //if ($("#SubmmitOrder_inputShippingModeId").val() == "0")//平台配送时才自动分配门店
        //{
        //    ResetShipingModel($(this).attr("regionid"));
        //}
        if ($(this).find("input[type='hidden']") != null && $(this).find("input[type='hidden']") != "" && $(this).find("input[type='hidden']") != "undefined") {
            $("#SubmmitOrder_hidShipperId").val($(this).find("input[type='hidden']").eq(0).val());

            ResetAddress($(this).find("input[type='hidden']").eq(0).val());
        }
    });
    // 收获地址列表选择触发事件
    $("input[type='radio'][name='SubmmitOrder$Common_ShippingAddressesRadioButtonList']").bind('click', function () {
        var shippingId = $(this).attr("value");
        ResetAddress(shippingId);
    })

    //3级收货地址选择触发事件
    $("#ddlRegions1").bind("change", function () {
        CalculateFreight($("#ddlRegions1").val());
    })
    //取消选中需要发票
    $("#SubmmitOrder_chkTax").on('ifUnchecked', function (event) {
        $("#SubmmitOrder_lblTax").html("0.00");
        $("#divTaxRate").hide();
        $("#div_fapiao").hide();
        //resetPoint();
        SetUseBalance();
    });
    //选择需要发票
    $("#SubmmitOrder_chkTax").on('ifChecked', function (event) {
        CalculationTax();
        $(".order_tax").show();
        $("body").append(bg);
        $("#div_fapiao").show();
        //resetPoint();
        SetUseBalance();
    });
    //清除使用优惠券
    function ClearCouponCode() {
        // $("#SubmitOrder_CouponName").html("");
        $("#SubmmitOrder_litCouponAmout").html("0");
        $("#SubmmitOrder_htmlCouponCode").val("");
        resetPoint();
        if ($("#SubmmitOrder_chkTax").is(':checked')) {
            CalculationTax();
        }
        SetUseBalance();
        // if ($("#SubmmitOrder_hidCanPointUseWithCoupon").val() == "False") {
        var myPoints = parseInt($("#SubmmitOrder_hidMyPoints").val());
        if (myPoints > 0) {
            $("#SubmmitOrder_txtUsePoints").val("0");
            onPointChange($("#SubmmitOrder_txtUsePoints"), false);
            $("#SubmmitOrder_tbPoint").show();
            //$("#SubmmitOrder_divJoinView").show();
            $("#divPoint").show();
            onPointCheck(false);
        }
        else {
            $("#SubmmitOrder_txtUsePoints").val("0");
            onPointChange($("#SubmmitOrder_txtUsePoints"), false);
            $("#SubmmitOrder_tbPoint").hide();
            $("#divPoint").hide();
        }
        //}

        $("#divCoupons").hide();
    }
    $("#SubmmitOrder_CmbCoupCode").combobox({

        onChange: function (n, o) {
            //var couponCode = $("#SubmmitOrder_CmbCoupCode").combobox("getValue");
            var couponCode = n;
            //如果选择了不使用优惠券
            if (couponCode == "0") {
                ClearCouponCode();
                return false;
            }
            else if (couponCode == undefined || couponCode == "" || couponCode == null) {
                couponCode = $("#SubmmitOrder_CmbCoupCode").combobox("getText");
                if (couponCode == "0" || couponCode == "") {
                    ClearCouponCode();
                    return false;
                }
            }
            $("#divCoupons").show();
            var cartTotal = $("#SubmmitOrder_lblTotalPrice").html();
            $.ajax({
                url: "/Handler/SubmmitOrderHandler.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { Action: "ProcessorUseCoupon", CartTotal: cartTotal, CouponCode: couponCode },
                cache: false,
                success: function (resultData) {
                    if (resultData.Status == "OK") {
                        //   $("#SubmitOrder_CouponName").html(resultData.CouponName);
                        $("#SubmmitOrder_litCouponAmout").html(resultData.DiscountValue);
                        $("#SubmmitOrder_htmlCouponCode").val(couponCode);
                        resetPoint();
                        if ($("#SubmmitOrder_chkTax").is(':checked')) {
                            CalculationTax();
                        }
                        SetUseBalance();
                        //如果设置了积分不与优惠券同时使用，则隐藏积分抵扣相关
                        if ($("#SubmmitOrder_hidCanPointUseWithCoupon").val() == "False") {
                            $("#SubmmitOrder_txtUsePoints").val("0");
                            onPointChange($("#SubmmitOrder_txtUsePoints"), false);
                            $("#divPoint").hide();
                        }
                    }
                    else {
                        $("#divCoupons").hide();
                        alert("您的优惠券编号无效或者不满足使用条件");
                    }
                }
            });
        }

    });

    // 使用优惠券
    $("#btnCoupon").click(function () {



    })


    function GetOrderTotal() {
        var OrderTotal = parseInt($("#SubmmitOrder_lblOrderTotal").html());
        if (isNaN(OrderTotal) || OrderTotal <= 0) {
            return 0;
        }
        return OrderTotal;
    }
    var lastSubmitTime = new Date();
    var submitTimes = 0;
    var hasCheckStock = false;
    // 创建订单
    $("#SubmmitOrder_btnCreateOrder").click(function () {
        var tempDate = new Date();
        if ((tempDate.getTime() - lastSubmitTime.getTime()) < 10000 && submitTimes > 0 && !hasCheckStock) {
            lastSubmitTime = new Date();
            submitTimes += 1;
            return false;
        }
        lastSubmitTime = new Date();
        submitTimes += 1;
        //代销模式
        if ($("#user_shippingaddress:hidden").length == 0) {
            $("#SubmmitOrder_hidShipperId").val(0);
        }
        if ($(this).attr("triggerevent") != undefined && $(this).attr("triggerevent") == "false") {
            return false;
        }
        var str = $("#SubmmitOrder_txtShipTo").val();
        var reg = new RegExp("[\u4e00-\u9fa5a-zA-Z ]+[\u4e00-\u9fa5_a-zA-Z0-9]*");
        if (str == "" || !reg.test(str)) {
            ShowMsg("请输入正确的收货人姓名", false);
            return false;
        }

        if ($("#SubmmitOrder_txtAddress").val() == "") {
            ShowMsg("请输入收货人详细地址", false);
            return false;
        }
        if ($("#SubmmitOrder_txtTelPhone").val() == "" && $("#SubmmitOrder_txtCellPhone").val() == "") {
            ShowMsg("请输入电话号码或手机号码", false);
            return false;
        }
        // 验证配送地区选择了没有
        var selectedRegionId = GetSelectedRegionId();
        var depth = parseInt($("#regionSelectorValue").attr("depth"));

        if (!isNaN(depth) && depth < 2 && $("#addresslist .list").length == 0) {
            ShowMsg("请收货人地址请至少精确到市区", false);
            return false;
        }
        if (selectedRegionId == null || selectedRegionId.length == "" || selectedRegionId == "0") {
            ShowMsg("请选择您的收货人地址", false);
            return false;
        }

        if (!PageIsValid()) {
            ShowMsg("部分信息没有通过检验，请查看页面提示", false);
            return false;
        }
        if ($("#SubmmitOrder_inputShippingModeId").val() == "") {
            ShowMsg("请选择配送方式", false);
            return false;
        }
        if ($("#SubmmitOrder_inputPaymentModeId").val() == "" && GetOrderTotal() > 0) {

            if ($("#payment_way").find("li:visible").length <= 0) {
                ShowMsg("商城暂未配置支付方式，请稍后提交", false);
            }
            else {
                ShowMsg("请选择支付方式", false);
            }
            return false;
        }
        if ($("#SubmmitOrder_inputShippingModeId").val() == "-2" && $("#SubmmitOrder_hidStoreId").val() == "" && $("#hidIsCloseStoreButGetGoods").val() == "false") {
            ShowMsg("请选择上门自提的门店", false);
            return false;
        }
        //如果需要发票，但是没有填写发票开头
        if ($("#SubmmitOrder_chkTax").prop("checked") == true && $("#SubmmitOrder_txtInvoiceTitle").val() == "") {
            ShowMsg("请填写发票开头", false);
            return false;
        }
        if ($("#SubmmitOrder_chkTax").prop("checked") == true && $("#SubmmitOrder_radInvoiceType2").prop("checked") == true && $("#SubmmitOrder_txtInvoiceTaxpayerNumber").val() == "") {
            ShowMsg("请填写企业纳税人识别号", false);
            return false;
        }
        //如果开启实名验证 且从页面获取
        if ($("#hidIsOpenCertification").val() == '1' && $("#hidIsGetIDInfo").val() == '1') {
            var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
            if (reg.test($("#SubmmitOrder_txtIDNumber").val()) === false) {
                ShowMsg("身份证输入不合法");
                return false;
            }
            //判定是否上传图片 开启图片验证
            if ($("#hidCertificationModel").val() == '2') {
                var idImage1 = $('#IDJust span[name="siteLogo"]').hishopUpload("getImgSrc");
                if (idImage1 == "" || idImage1.indexOf('upload_ID_Just.jpg') >= 0) {
                    ShowMsg("请上传证件正面照");
                    return false;
                }
                $("#hididCardJustImg").val($('#IDJust span[name="siteLogo"]').hishopUpload("getImgSrc"));
                var idImage2 = $('#IDAnti span[name="siteLogo"]').hishopUpload("getImgSrc");
                if (idImage2 == "" || idImage2.indexOf('upload_ID_Anti.jpg') >= 0) {
                    ShowMsg("请上传证件号反面照");
                    return false;
                }
                $("#hididCardAntiImg").val($('#IDAnti span[name="siteLogo"]').hishopUpload("getImgSrc"));
            }
        }

        if (!hasCheckStock) {
            CheckProductStock();
            return false;
        }
        $(this).attr({ "enabled": "false" });
        hasCheckStock = false;
    });

    ///提交订单前检查订单库存
    function CheckProductStock() {
        var from = getParam("from");
        var prodcutSku = getParam("productSku");
        var buyAmount = getParam("buyAmount");
        var bundlingid = getParam("Bundlingid");
        var combinationId = getParam("CombinationId");
        $.ajax({
            url: "/Handler/SubmmitOrderHandler.ashx",
            type: "post",
            dataType: "json",
            timeout: 10000,
            data: { action: 'CheckStock', from: from, productSku: productSku, buyAmount: buyAmount, Bundlingid: bundlingid, CombinationId: combinationId },
            success: function (data) {

                if (data.Status != 'SUCCESS') {
                    alert(data.Message);
                    document.location.href = "ShoppingCart";
                }
                else {
                    hasCheckStock = true;
                    $("#SubmmitOrder_btnCreateOrder").trigger("click");
                }
            }
        });
    }

    $("#btnaddaddress").click(function () {

        $("#tab_address").show();
        $("#user_shippingaddress").addClass("cart_Order_address2").show();
        $("body").append(bg);

        //$("#user_shippingaddress").css({ 'position': 'relative' });

    });
    $("#imgCloseLogin").click(function () {
        $("#user_shippingaddress").hide();
        $("#tab_address").hide();
        $("#user_shippingaddress").attr("class", "cart_Order_address");
        $(".modal_qt").remove();
    });



    $("#a_salemode").toggle(function () {
        $("#tab_pasteaddress").css("display", "block");
        $("#user_shippingaddress").show();

        $(this).text("切换到普通模式");


        if ($("#addresslist") != null && $("#addresslist") != "undefined" && $("#addresslist") != "" && $("#addresslist").size() > 0) {
            $("#addresslist").hide();
            $("#addresslist .list").removeClass("select");
        }
        $("#btnaddr").hide();

        ClearAddress();

    }, function () {
        $("#user_shippingaddress").attr("class", "cart_Order_address");
        $("#tab_pasteaddress").css("display", "none");
        if ($("#addresslist") != null && $("#addresslist") != "undefined" && $("#addresslist") != "" && $("#addresslist").size() > 0) {
            $("#user_shippingaddress").hide();
        }


        $(this).text("切换到代销模式");

        if ($("#addresslist") != null && $("#addresslist") != "undefined" && $("#addresslist") != "" && $("#addresslist").size() > 0) {
            $("#addresslist").show();
            $("#addresslist .list").removeClass("select");

        }
    });

    var shoppingDeduction = $("#SubmmitOrder_hidShoppingDeduction").val();
    shoppingDeduction = parseFloat(shoppingDeduction);

    if (shoppingDeduction <= 0) {
        $("#SubmmitOrder_tbPoint").hide();
    }

    //如果只提交礼品或者订单金额为0，则不显示可得积分项
    var productSku = getParam('productSku');
    var total = GetOrderTotal();
    if (productSku == "" || productSku == undefined || total <= 0) {
        $("#divGetPoints").hide();
    }


});

function btnziti() {
    if ($("#hidIsCloseStoreButGetGoods").val() != "false") {
        $('.ziti_btn').hide();
        var storeRemark = $("#hidGetGoodsRemark").val();
        $("#divGetGoodsRemark").html(storeRemark);
        return false;
    }
    //判断是否选中收货地址
    var selectedRegionId = GetSelectedRegionId();

    if (selectedRegionId == null || selectedRegionId.length == "" || selectedRegionId == "0") {
        //alert("请选择您的收货人地址");
        $(".deliver_way").find("li").eq(1).find("div").eq(0).removeClass("ziti");
        $(".deliver_way").find("li").eq(1).find("div").eq(0).addClass("noused");

        $('.ziti_con').html("请填写您的收货地址");
        $('.ziti_btn').hide();
        $("#SubmmitOrder_inputShippingModeId").val("0");
        $(".deliver_way").find("li").eq(0).addClass("select");
        return false;
    }

    if (regionid != selectedRegionId) {
        regionid = selectedRegionId;
        productSku = $.trim(getParam('productSku'));
        var buyAmount = $.trim(getParam('buyAmount'));
        $('.mendian').find(".Store").remove();
        $('.mendian').find(".info").append('<div class="info1 Store">加载中……</div>');
        $('.mendian').find(".info1").eq(0).find("select option").remove();
        $('.mendian').find(".info1").eq(0).find("select").append("<option value='0' >全部区域</option>");


        $(".deliver_way").find("li").eq(1).find("div").eq(0).addClass("ziti");
        $(".deliver_way").find("li").eq(1).find("div").eq(0).removeClass("noused");
        $('.ziti_con').html("");
        $('.ziti_btn').show();
        $('.mendian').find(".info1").eq(0).find("select option").remove();
        $('.mendian').find(".info1").eq(0).find("select").append("<option value='0' >全部区域</option>");
        $('.mendian').find(".Store").remove();
        $('.mendian').find(".info").append('<div class="info1 Store">加载中……</div>');

        //清空
        $("#SubmmitOrder_hidStoreId").val("");
        if ($('.ziti').length > 0) {
            $(".ziti_con").html("");
            $(".ziti_btn").html("<a href=\"javascript:;\">&nbsp;选择自提门店</a>");
        }
        var combinationId = getParam("CombinationId");
        $.ajax({
            url: "/Handler/SubmmitOrderHandler.ashx",
            type: "post",
            dataType: "json",
            timeout: 10000,
            data: { action: 'GetStores', regionid: regionid, productSku: productSku, buyAmount: buyAmount, CombinationId: combinationId },
            async: false,
            success: function (data) {
                $('.mendian').find(".Store").remove();
                if (data.status == 'true') {
                    if (data.Data && data.Data.length > 0) {

                        for (var i = 0; i < data.Data.length; i++) {
                            if (i == 0) {
                                $('.mendian').find(".info").append(getReviewHtml(true, (data.Data[i].Error != ""), data.Data[i]));
                            } else {
                                $('.mendian').find(".info").append(getReviewHtml(false, (data.Data[i].Error != ""), data.Data[i]));
                            }
                        }
                    } else {
                        $('.mendian').find(".info").append('<div class="info1 Store">本市无可自提的门店</div>');
                    }
                } else {
                    $(".deliver_way").find("li").eq(1).removeClass("select");
                    $(".deliver_way").find("li").eq(1).find("div").eq(0).removeClass("ziti");
                    $(".deliver_way").find("li").eq(1).find("div").eq(0).addClass("noused");

                    $('.ziti_con').html(data.msg);
                    $('.mendian').find(".info").append('<div class="info1 Store">' + data.msg + '</div>');
                    $('.ziti_btn').hide();
                    $("#SubmmitOrder_inputShippingModeId").val("0");
                    $(".deliver_way").find("li").eq(0).addClass("select");
                    btnDeliverWay(0);
                }
            }
        });
        //获取区域
        $.ajax({
            url: "/Handler/SubmmitOrderHandler.ashx",
            type: "post",
            dataType: "json",
            timeout: 10000,
            data: { action: 'GetCountys', regionid: regionid },
            async: false,
            success: function (data) {
                if (data.status == 'true') {
                    if (data.Data && data.Data.length > 0) {
                        for (var i = 0; i < data.Data.length; i++) {
                            $('.mendian').find(".info1").eq(0).find("select").append("<option value='" + data.Data[i].id + "'>" + data.Data[i].name + "</option>");
                        }
                    }
                    $('.mendian').find(".info1").eq(0).find("select").change(function () {
                        var CountyId = parseInt($("option:selected", this).val());
                        $(".Store").find("h2").html("");
                        $(".Store .Error").remove();
                        if (CountyId == 0) {
                            $(".Store").show();
                            $(".Error").hide();
                            $(".Store").find("h2").eq(0).html("门店：");
                        } else {
                            $(".Store").hide();
                            if ($(".CountyId_" + CountyId).length > 0) {
                                $(".CountyId_" + CountyId).show();
                                $(".CountyId_" + CountyId).find("h2").eq(0).html("门店：");
                            } else {
                                $('.mendian').find(".info").append('<div class="info1 Store Error">该区域无可自提的门店</div>');
                            }
                        }
                    });
                } else {
                    alert(data.msg);
                }
            }
        });
    }



}
function getReviewHtml(ish1, isdisabled, obj) {
    var arr = new Array();
    arr.push('<div class="info1 Store CountyId_' + obj.CountyId + '" >');
    if (ish1)
        arr.push('<h2>门店：</h2>');
    else
        arr.push('<h2>&nbsp;</h2>');

    if (isdisabled)
        arr.push('<div class="addr-detail disabled">');
    else
        arr.push('<div class="addr-detail">');

    arr.push('<span class="name skin-flat ">');
    if (isdisabled) {
        arr.push('<input type="radio" disabled="" name="flat-radio" ><b>' + obj.Name + '</b>');
        arr.push('<em><i>!</i>' + obj.Error + '</em>');

    }
    else
        arr.push('<input type="radio" onclick="checkStore(' + obj.storeId + ', \'' + obj.Name + '\', \'' + obj.Addr + '\', \'' + obj.Tel + '\',' + obj.IsOnlinePay + ',' + obj.IsOfflinePay + ')" name="flat-radio"><b>' + obj.Name + '</b>');
    arr.push('</span>');
    arr.push('<span>地址：' + obj.Addr + '</span>');
    arr.push('<span>联系电话：' + obj.Tel + '</span>');
    arr.push('</div>');
    arr.push('</div>');
    return arr.join("");
}
function btnDeliverWay(index) {
    if (index == 0) {
        if ($("#SubmmitOrder_inputShippingModeId").val() == "-2") {
            $("#SubmmitOrder_hidStoreId").val("");
            if ($('.ziti').length > 0) {
                $(".ziti_con").html("");
                $(".ziti_btn").html("<a href=\"javascript:;\">&nbsp;选择自提门店</a>");
            }
        }
        $("#SubmmitOrder_inputShippingModeId").val("0");
        //自动配送
        var selectedRegionId = GetSelectedRegionId();
        //ResetShipingModel(selectedRegionId);

        //$(".deliver_way ul li").eq(index).trigger("click");
        $(".deliver_way ul li").eq(index).addClass('select').siblings().removeClass('select');
        var paymentId_PayAfterGetGoods = parseInt($("#hidPaymentId_Podrequest").val());
        //如果后台配置了货到付款方式则显示货到付款，否则不显示
        if (!isNaN(paymentId_PayAfterGetGoods) && paymentId_PayAfterGetGoods > 0 && $("#hidHasSupplierProduct").val() != "1" && $("#hidIsGiftOrder").val() != "1") {
            $(".payment_way ul li").eq(1).show();
        }
        else {
            $(".payment_way ul li").eq(1).hide();
        }
        $(".payment_way ul li").eq(2).hide();
        if ($("#SubmmitOrder_inputPaymentModeId").val() == "-2") {
            $("#SubmmitOrder_inputPaymentModeId").val("");
        }
        var paymentId_PayOffline = parseInt($("#hidPaymentId_Offline").val());
        if (!isNaN(paymentId_PayOffline) && paymentId_PayOffline > 0 && $("#hidHasSupplierProduct").val() != "1" && $("#hidIsGiftOrder").val() != "1") {
            $(".payment_way ul li").eq(3).show();
        }
        else {
            $(".payment_way ul li").eq(3).hide();
        }
        $(".Order_deliver_time").show();
        if ($("#hidIsCloseStoreButGetGoods").val() != "false") {
            $("#divGetGoodsRemark").hide();
        }
    }//上门自提
    else if (index == 1 && $(".ziti").length > 0) {
        if ($('.ziti').length == 0) { return false; }//按钮被禁用
        btnziti();
        if ($("#hidIsCloseStoreButGetGoods").val() != "false") {
            $("#divGetGoodsRemark").show();
        }

        if ($("#SubmmitOrder_inputShippingModeId").val() != "-2") {
            $("#SubmmitOrder_hidStoreId").val("");
        }
        $("#SubmmitOrder_inputShippingModeId").val("-2");
        $(".deliver_way ul li").eq(index).addClass('select').siblings().removeClass('select');
        //  $(".deliver_way ul li").eq(index).trigger("click");
        $(".payment_way ul li").eq(1).hide();
        $(".payment_way ul li").eq(3).hide();
        $(".Order_deliver_time").hide();

        if ($.trim($(".ziti_con").html()) == "")
            $("#SubmmitOrder_hidStoreId").val("");
        var isOfflinePay = !isNaN(paymentId_PayOffline) && paymentId_PayOffline > 0;//是否开启了线下支付
        if (isOfflinePay && ($("#hidIsCloseStoreButGetGoods").val() != "false" || $("#hidGetgoodsOnStores").val() != "false") && $("#hidHasSupplierProduct").val() != "1" && $("#hidIsGiftOrder").val() != "1") {
            $(".payment_way ul li").eq(2).show();
        }
        else {
            $(".payment_way ul li").eq(2).hide();
            if ($("#SubmmitOrder_inputPaymentModeId").val() == "-2") {
                $("#SubmmitOrder_inputPaymentModeId").val("");
            }
        }
        if ($("#hidIsCloseStoreButGetGoods").val() == "false") {
            $('.mendian').css('display', 'block');
            $('.zhezhao').css('display', 'block');
        }
    }
}

var regionid = "", productSku = "";
function checkStore(id, Name, Addr, Tel, isOnlinePay, isOfflinePay) {
    $("#SubmmitOrder_hidStoreId").val(id);
    $(".ziti_con").html("自提地点： " + Name + " &nbsp;" + Addr + " &nbsp;" + Tel + "");
    $(".ziti_btn").html("<a href=\"javascript:;\">&nbsp;修改自提门店</a>");
    $('.mendian').css('display', 'none');
    $('.zhezhao').css('display', 'none');
    $("#SubmmitOrder_lblShippModePrice").html("0.00");
    $(".payment_way ul li").removeClass("select");
    if (isOnlinePay) {
        $(".payment_way ul li").eq(0).addClass("select");
        $("#SubmmitOrder_inputPaymentModeId").val("0");
        $(".payment_way ul li").eq(0).show();
        $(".payment_way ul li").eq(2).hide();
    }
    if (isOfflinePay) {
        if (!isOnlinePay) {
            $(".payment_way ul li").eq(0).hide();
            $(".payment_way ul li").eq(2).addClass("select");
            $("#SubmmitOrder_inputPaymentModeId").val("-3");
        }
        $(".payment_way ul li").eq(2).show();
    }


    SetUseBalance();
}



function ClearAddress() {
    $("#SubmmitOrder_txtShipTo").val('');
    $("#SubmmitOrder_txtAddress").val('');
    $("#SubmmitOrder_txtZipcode").val('');
    $("#SubmmitOrder_txtCellPhone").val('');
    $("#SubmmitOrder_txtTelPhone").val('');
    $("#ddlRegions1").val("");
    $("#ddlRegions2").val(null);
    $("#ddlRegions3").val(null);
}


function AddShippAddress() {
    if (!PageIsValid()) {
        ShowMsg("部分信息没有通过检验，请查看页面提示", false);
        return false;
    }
    var depth = parseInt($("#regionSelectorValue").attr("depth"));
    if (!isNaN(depth) && depth < 2) {
        ShowMsg("收货人地址请至少精确到市区", false);
        return false;
    }
    $.ajax({
        url: "/Handler/SubmmitOrderHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: {
            Action: "AddShippingAddress", ShippingTo: $("#SubmmitOrder_txtShipTo").val().replace(/\s/g, ""),
            RegionId: $("#regionSelectorValue").val(),
            AddressDetails: $("#SubmmitOrder_txtAddress").val().replace(/\s/g, ""),
            ZipCode: $("#SubmmitOrder_txtZipcode").val().replace(/\s/g, ""),
            TelPhone: $("#SubmmitOrder_txtTelPhone").val().replace(/\s/g, ""),
            CellHphone: $("#SubmmitOrder_txtCellPhone").val().replace(/\s/g, ""),
            IsDefault: $("#chkIsDefault").is(':checked')
        },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                var divlist = "<div class=\"list select\" regionid=\"" + resultData.Result.Id + "\">";
                divlist += "<div class=\"inner\">";
                divlist += "<div class=\"addr-hd\" title=\"" + resultData.Result.RegionId + "(" + resultData.Result.ShipTo + "收)" + "\">";
                divlist += "<span class=\"name\">" + resultData.Result.ShipTo + "</span>(" + resultData.Result.RegionId + ")";
                divlist += "</div>";
                divlist += "<div class=\"addr-bd\" title=\"" + resultData.Result.ShippingAddress + "\">";
                divlist += "<span class=\"street\">" + resultData.Result.ShippingAddress + "</span><span class=\"phone\">" + resultData.Result.CellPhone + "</span>";
                divlist += "<span class=\"last\">&nbsp;</span>";
                divlist += "</div>";
                divlist += "</div>";
                divlist += "<em class=\"curmarker\"></em><input type=\"hidden\" value=\"" + resultData.Result.ShippingId + "\"/>";
                divlist += "</div>";
                if ($(".address_tab .list").length > 0) {
                    $(".address_tab .list").removeClass("select");
                    $(".address_tab .list:last").after(divlist);
                    $("#user_shippingaddress").hide();
                    btnziti();
                }
                else {
                    $("#noaddressDiv").show();
                    $(".address_tab").html(divlist);
                    $("#user_shippingaddress").hide();
                }
                $(".modal_qt").remove();
                ResetAddress(resultData.Result.ShippingId);

            }
            else {
                ShowMsg(resultData.Result, false);
            }
        }
    });
}

// 重置收货地址
function ResetAddress(shippingId) {
    var ConsigneeName = $("#SubmmitOrder_txtShipTo");
    var ConsigneeAddress = $("#SubmmitOrder_txtAddress");
    var ConsigneePostCode = $("#SubmmitOrder_txtZipcode");
    var ConsigneeTel = $("#SubmmitOrder_txtTelPhone");
    var ConsigneeHandSet = $("#SubmmitOrder_txtCellPhone");
    $("#SubmmitOrder_hidShipperId").val(shippingId);
    $.ajax({
        url: "/Handler/SubmmitOrderHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: "GetUserShippingAddress", ShippingId: shippingId },
        async: false,
        success: function (resultData) {

            if (resultData.Status == "OK") {
                $(ConsigneeName).val(resultData.ShipTo);
                ConsigneeName.focus();

                $(ConsigneeAddress).val(resultData.Address);
                ConsigneeAddress.focus();

                $(ConsigneePostCode).val(resultData.Zipcode);
                ConsigneePostCode.focus();

                $(ConsigneeTel).val(resultData.TelPhone);
                ConsigneeTel.focus();

                $(ConsigneeHandSet).val(resultData.CellPhone);
                ConsigneeHandSet.focus();
                ResetSelectedRegion(resultData.RegionId);
                CalculateFreight(resultData.RegionId);
                //是否开启实名验证
                //当商品列表中有跨境商品时
                OpenNameVerify(resultData);
            }
            else {

                ShowMsg("收货地址选择出错，请重试!", false);
            }
        }
    });
}

////// 根据选择的地址，刷新门店
//function ResetShipingModel(regionId) {
//    $("input:radio[name=shippButton]").attr("checked", false);
//    var buyType = $("#SubmmitOrder_hdbuytype").val();
//    var isAnonymous = $("#SubmmitOrder_hidIsAnonymous").val();
//    var productSku = getParam("productSku");
//    if (buyType == "GroupBuy" || buyType == "CountDown" || buyType == "Bundling" || isAnonymous == "1") {
//        return;
//    }

//    var buyAmount = $.trim(getParam('buyAmount'));

//    var combinationId = getParam("CombinationId");
//    $.ajax({
//        url: "/SubmmitOrderHandler",
//        type: 'post', dataType: 'json', timeout: 10000,
//        data: { Action: "GetCanShipStoresForOrder", regionId: regionId, productSku: productSku, buyAmount: buyAmount, CombinationId: combinationId },
//        async: false,
//        success: function (resultData) {
//            $("#SubmmitOrder_hidStoreCount").val(resultData.length);
//            $("#SubmmitOrder_hidStoreId").val("");
//            if (resultData.length > 0 && $("#SubmmitOrder_inputShippingModeId").val() == "0") {
//                if (resultData.length == 1) {
//                    $("#SubmmitOrder_hidStoreId").val(resultData[0].StoreId);
//                }
//            }
//        }
//    })
//}


// 绑定选中的门店ID
function bindStoreId(storeId) {
    $("#SubmmitOrder_hidStoreId").val(storeId);
    $("#SubmmitOrder_Common_StoreShippingModeList___radOnStoreTake").click();
}

// 总金额
function CalculateTotalPrice() {
    var useBalance = 0;
    var cartTotalPrice = parseFloat($("#hidTotalPrice").val());
    if (isNaN(cartTotalPrice) || cartTotalPrice < 0) { cartTotalPrice = 0; }

    var shippmodePrice = parseFloat($("#SubmmitOrder_lblShippModePrice").html());
    if (isNaN(shippmodePrice) || shippmodePrice < 0) { shippmodePrice = 0; }
    var couponPrice = parseFloat($("#SubmmitOrder_litCouponAmout").html());
    if (isNaN(couponPrice) || couponPrice < 0) { couponPrice = 0; }

    var tax = parseFloat($("#SubmmitOrder_lblTax").html());
    if (isNaN(tax) || tax < 0) { tax = 0; }
    var pointPrice = parseFloat($("#SubmmitOrder_litPointAmount").html());
    if (isNaN(pointPrice) || pointPrice < 0) { pointPrice = 0; }

    //var total = (cartTotalPrice * 100 - couponPrice * 100) / 100 + tax;
    var total = cartTotalPrice.toSub(couponPrice).toAdd(tax);

    if (total < 0) { total = 0; }

    if (getParam("from").toLowerCase() == "presale") {
        var deposit = parseFloat($("#SubmmitOrder_lblDeposit").html());
        //total = total - deposit;
        total = total.toSub(deposit);

        if (total < 0) { total = 0; }

        if ($("#SubmmitOrder_hlkFeeFreight").html() == "") {
            //total = (parseFloat(total) + parseFloat(shippmodePrice)).toFixed(2); // 19.98+18=37.980000000000004  js bug
            total = total.toAdd(shippmodePrice).toFixed(2);
        }

    
        if (pointPrice) {
            //total = (total * 100) - (pointPrice * 100) / 100;
            total = total.toSub(pointPrice);
        }

        if (total < 0) { total = 0; }
        $("#SubmmitOrder_lblFinalPayment").html(total.toFixed(2));//尾款
        //total = (deposit + total);
        total = deposit.toAdd(total);
        useBalance = parseFloat($("#hidUseBalance").val())
        if (isNaN(useBalance)) { useBalance = 0; }
        $("#SubmmitOrder_lblOrderTotal").html(total.toFixed(2));//合计
        $("#SubmmitOrder_lblPreSaleOrderTotal").html(total.toFixed(2));//订单总计
        if (deposit - useBalance < 0) {//如果定金-余额小于0，则更新使用余额为定金
            //total = (total * 100 - deposit * 100) / 100;
            total = total.toSub(deposit);
            useBalance = deposit;
            $("#hidUseBalance").val(useBalance);
        }
        else {
            //total = (total * 100 - deposit * 100 - useBalance * 100) / 100;
            total = total.toSub(deposit, useBalance)
        }

    }
    else {
        if ($("#SubmmitOrder_hlkFeeFreight").html() == "") {
            //total = (parseFloat(total) + parseFloat(shippmodePrice)).toFixed(2); // 19.98+18=37.980000000000004  js bug
            total = total.toAdd(shippmodePrice).toFixed(2);
        }       
        //alert(total+"+1+2+3+4+5="+total.toAdd(1, 2, 3, 4, 5));        
        //alert((parseFloat("1").toAdd(2)).toMul(3));
        //alert(parseFloat("3").toMul(3));
        if (pointPrice) {
            //total = (total * 100 - pointPrice * 100) / 100;
            total = total.toSub(pointPrice);
        }
        if (total < 0) { total = 0; }
        useBalance = parseFloat($("#hidUseBalance").val())
        if (isNaN(useBalance)) { useBalance = 0; }
        if (total - useBalance < 0) {
            useBalance = total;
            total = 0;
            $("#hidUseBalance").val(useBalance.toFixed(2));
            $("#spanUseBalance").html(useBalance.toFixed(2));
        }
        else {
            //total = (total * 100 - useBalance * 100) / 100;
            total = total.toSub(useBalance);
        }
        $("#SubmmitOrder_lblOrderTotal").html(total.toFixed(2));
    }
    var pointRate = parseFloat($("#hidPointRate").val());
    if (isNaN(pointRate)) { pointRate = 1; }
    var timePoint = parseFloat($("#hidTimePoint").val());
    //var point = parseInt((total + (useBalance * 100 - shippmodePrice * 100) / 100) * timePoint / pointRate);
    point = parseInt((total.toAdd(useBalance).toSub(shippmodePrice)).toMul(timePoint).toDiv(pointRate));
    if (isNaN(point)) {
        point = 0;
    }
    $("#litPoint").html(point < 0 ? 0 : point);
}
//计算积分可计算的总金额
function CalculatePointTotalPrice() {

    var cartTotalPrice = parseFloat($("#hidTotalPrice").val());
    if (isNaN(cartTotalPrice) || cartTotalPrice < 0) { cartTotalPrice = 0; }
    //var shippmodePrice = parseFloat($("#SubmmitOrder_lblShippModePrice").html());
    //if (isNaN(shippmodePrice) || shippmodePrice < 0) { shippmodePrice = 0; }
    var couponPrice = parseFloat($("#SubmmitOrder_litCouponAmout").html());
    if (isNaN(couponPrice) || couponPrice < 0) { couponPrice = 0; }

    // var tax = parseFloat($("#SubmmitOrder_lblTax").html());
    // if (isNaN(tax) || tax < 0) { tax = 0; }

    //var total = (cartTotalPrice * 100 - couponPrice * 100) / 100;// tax;
    var total = cartTotalPrice.toSub(couponPrice);
    if (total < 0) { total = 0; }
    if (getParam("from").toLowerCase() == "presale") {
        var deposit = parseFloat($("#SubmmitOrder_lblDeposit").html());
        if (isNaN(deposit) || deposit < 0) { deposit = 0; }
        //total = (total * 100 - deposit * 100) / 100;
        total = total.toSub(deposit);
        if (total < 0) { total = 0; }
        //if ($("#SubmmitOrder_hlkFeeFreight").html() == "")
        //    total = total + shippmodePrice;
    }
    //else {
    //    if ($("#SubmmitOrder_hlkFeeFreight").html() == "")
    //        total = total + shippmodePrice;
    //}
    return total.toFixed(2);

}
// 重新计算运费
function CalculateFreight(regionId) {
    var shippingModeId = $("#SubmmitOrder_inputShippingModeId").val();
    // 不是上门自提的都要计算邮费
    var weight = $("#SubmmitOrder_litAllWeight").html();
    var productSku = getParam('productSku');
    var buyAmount = parseInt(getParam('buyAmount'));
    var Bundlingid = parseInt(getParam('Bundlingid'));
    var combinationId = getParam("CombinationId");
    var from = getParam('from');
    if (isNaN(buyAmount) || buyAmount < 1) buyAmount = 1;
    if (isNaN(Bundlingid) || Bundlingid < 1) Bundlingid = 0;
    $.ajax({
        url: "/Handler/SubmmitOrderHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: 'CalculateFreight', ModeId: shippingModeId, Weight: weight, RegionId: regionId, productSku: productSku, buyAmount: buyAmount, Bundlingid: Bundlingid, from: from, CombinationId: combinationId },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                if ($("#SubmmitOrder_hlkFeeFreight").html() == "") {
                    $("#SubmmitOrder_lblShippModePrice").html(resultData.Price);
                    $("#divFreight").show();
                    for (var i = 0; i < resultData.SupplierPrice.length; i++) {
                        var item = resultData.SupplierPrice[i];
                        var giftstr = "";
                        if ($(".spanFreight" + item.SupplierId).length > 0) {
                            if ($(".spanFreight" + item.SupplierId).html().indexOf("含礼品") > -1) {
                                giftstr = "(含礼品)";
                            }
                            $(".spanFreight" + item.SupplierId).html("￥" + item.FreightPrice + giftstr);
                        }
                    }
                }
                else {
                    $("#SubmmitOrder_lblShippModePrice").html("0.00");
                    // $("#divFreight").hide();
                }
                //resetPoint();
                SetUseBalance();
            }
        }
    });
}


//点击使用积分抵扣checkbox事件
function onPointCheck(isCheck) {
    var maxPoints = CalculateMaxDeductionPoint();
    $("#SubmmitOrder_lblMaxPoints").text(maxPoints);
    if (isCheck == undefined) {
        isCheck = !$("#chkIsUsePoints").is(":checked");
    }
    if (isCheck) {
        $("#SubmmitOrder_txtUsePoints").val(maxPoints);
        onPointChange($("#SubmmitOrder_txtUsePoints"), true);
    }
    else {
        $("#SubmmitOrder_txtUsePoints").val("0");
        onPointChange($("#SubmmitOrder_txtUsePoints"), false);
        SetUseBalance();

    }
    // CalculateTotalPrice();
}
//改变使用的抵扣积分触发事件
function onPointChange(obj, isUsePoint) {
    //如果未选中使用商城积分
    //alert($("#chkIsUsePoints").is(":checked"));
    if (isUsePoint == undefined) {
        isUsePoint = $("#chkIsUsePoints").is(":checked");
    }
    if (!isUsePoint) {
        $("#SubmmitOrder_litPointAmount").html("0.00")
        if ($("#SubmmitOrder_chkTax").is(':checked')) {
            CalculationTax();
        }
        SetUseBalance();
        return false;
    }
    var currentPoints = $(obj).val();
    var ex = /^[1-9]\d*$/;
    if (isNaN(currentPoints) || (!ex.test(currentPoints) && currentPoints != "0")) {
        ShowMsg("积分请输入整数", false);
        $(obj).val("0");
        //   $("#SubmmitOrder_lblDeductibleMoney").text("0");
        $("#SubmmitOrder_litPointAmount").html("0");
        if ($("#SubmmitOrder_chkTax").is(':checked')) {
            CalculationTax();
        }
        SetUseBalance();
        return false;
    }
    if (currentPoints == "" || currentPoints == null || currentPoints == undefined || currentPoints == "0") {
        $(obj).val("0");
        //    $("#SubmmitOrder_lblDeductibleMoney").text("0");
        $("#SubmmitOrder_litPointAmount").html("0");
        //CalculateTotalPrice();
        if ($("#SubmmitOrder_chkTax").is(':checked')) {
            CalculationTax();
        }
        SetUseBalance();
        return false;
    }
    var maxPoints = $("#SubmmitOrder_lblMaxPoints").text();
    var maxPoints = parseInt(maxPoints);
    var currentPoints = parseInt(currentPoints);
    if (currentPoints > maxPoints) {
        $(obj).val("0");
        // $("#SubmmitOrder_lblDeductibleMoney").text("0");
        $("#SubmmitOrder_litPointAmount").html("0");
        if ($("#SubmmitOrder_chkTax").is(':checked')) {
            CalculationTax();
        }
        SetUseBalance();
        ShowMsg("您输入的积分大于可用积分，请重新输入", false);
        return false;
    }
    var shoppingDeduction = $("#SubmmitOrder_hidShoppingDeduction").val();
    var shoppingDeduction = parseFloat(shoppingDeduction);
    if (isNaN(shoppingDeduction) || shoppingDeduction < 0) {
        shoppingDeduction = 100;
    }
    //var deductionMoney = currentPoints / shoppingDeduction;
    var deductionMoney = currentPoints.toDiv(shoppingDeduction);
    deductionMoney = deductionMoney.toFixed(2);
    //  $("#SubmmitOrder_lblDeductibleMoney").html(deductionMoney);
    $("#SubmmitOrder_litPointAmount").html(deductionMoney);
    if ($("#SubmmitOrder_chkTax").is(':checked')) {
        CalculationTax();
    }
    SetUseBalance();
}

//最大可抵扣积分
function CalculateMaxDeductionPoint() {
    var total = CalculatePointTotalPrice();
    var deducationRate = $("#SubmmitOrder_hidShoppingDeductionRatio").val();
    if (isNaN(deducationRate) || deducationRate < 0) { deducationRate = 100; }
    var shoppingDeduction = $("#SubmmitOrder_hidShoppingDeduction").val();
    if (isNaN(shoppingDeduction) || shoppingDeduction < 0) { shoppingDeduction = 0; }
    var myPoints = $("#SubmmitOrder_hidMyPoints").val();
    if (isNaN(myPoints) || myPoints < 0) { myPoints = 0; }
    //var maxPoints = parseInt(deducationRate) * parseFloat(total) * parseFloat(shoppingDeduction) / 100;
    var maxPoints = parseInt(deducationRate).toMul(parseFloat(total), parseFloat(shoppingDeduction)).toDiv(100);
    myPoints = parseInt(myPoints);
    maxPoints = parseInt(maxPoints);
    var lastPoint = myPoints > maxPoints ? maxPoints : myPoints;
    return lastPoint;
}

function resetPoint() {
    if ($("#chkIsUsePoints") && !$("#divPoint").is(":hidden")) {
        $("#chkIsUsePoints").iCheck("uncheck");
        onPointCheck(false);
    }
}

//开启实名验证
function OpenNameVerify(obj) {
    if ($("#hidIsOpenCertification").val() == '1') {
        //有无参数
        $("#OpenCertification").show();
        if (typeof (obj) == "object" && obj != null) {
            if (obj.IsGetIDInfo == '0') {
                //如果从收货地址获取就隐藏
                $("#OpenCertification").hide();
                $("#OpenCertification li[class='Certification2'],li[class='id_card']").hide();
            } else {
                $("#trShipTo").show();
                $("#SubmmitOrder_lblShipTo").text(obj.ShipTo);
                if ($("#hidCertificationModel").val() == '2' && obj.IDNumber != '') {
                    $("#SubmmitOrder_txtIDNumber").val(obj.IDNumber);
                }
                OpenIDImgUpload();
            }
            $("#hidIsGetIDInfo").val(obj.IsGetIDInfo);
            $("#hidCertificationModel").val(obj.CertificationModel);
        }
        else {
            //hidIsGetIDInfo = 1页面 0地址
            $("#trShipTo").hide();
            $("#SubmmitOrder_lblShipTo").text();
            $("#SubmmitOrder_txtIDNumber").val();
            $("#hidIsGetIDInfo").val("1");//没有开启默认从订单获取
            OpenIDImgUpload();
        }
    } else {
        $("#OpenCertification").hide();
    }
}

//开启图片上传
function OpenIDImgUpload() {
    if ($("#hidCertificationModel").val() == '2') {
        //$("#OpenCertification2").show();
        $("#OpenCertification li[class='Certification2'],li[class='id_card']").show();
        //证件上传
        var url = ImageUploadPath;
        var imgJustSrc = ImageServerUrl + $("#fieldIDCardJust").val();
        $('#IDJust span[name="siteLogo"]').hishopUpload(
        {
            title: '缩略图',
            url: url,
            imageDescript: '',
            imgFieldName: "siteLogo",
            pictureSize: '',
            imagesCount: 1,
            displayImgSrc: imgJustSrc,
            foldName: 'user'
        });
        var imgAntiSrc = ImageServerUrl + $("#fieldIDCardAnti").val();
        $('#IDAnti span[name="siteLogo"]').hishopUpload(
        {
            title: '缩略图',
            url: url,
            imageDescript: '',
            imgFieldName: "siteLogo",
            pictureSize: '',
            imagesCount: 1,
            displayImgSrc: imgAntiSrc,
            foldName: 'user'
        });
    } else {
        $("#OpenCertification li[class='Certification2'],li[class='id_card']").hide();
    }
}
