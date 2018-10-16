$(document).ready(function () {

    $("#imgCloseLogin").bind("click", function () {
        closePayPop();
    });

    $('.skin-flat input').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    });

    $("#textfieldpassword").on("focus", function () {
        var _t = $(this);
        _t.prop("type","password");
    });
   
});



function checkPayment() {
    var modeId = parseInt($("#paymentModeId").val())
    $("#windowsdialog").remove();
    if (isNaN(modeId) || modeId <= 0) {
        ShowMsg("请选择支付方式");
        return false;
    }

    if (modeId > 0) {
        var AdvanceId = parseInt($("#AdvanceId").val())
        if (AdvanceId == modeId) {
            if ($("#hidHasTradePassword").val() != "1") {
                window.location.href = "/user/OpenBalance.aspx";
                return;
            }
            else {
                $("#paymodel").show();
                $("#loginForBuy").show();
                $("#loginForBuy").removeClass("hide");
                $("#btnLoginAndBuy").click(function () {
                    $("#windowsdialog").remove();
                    $(this).attr("disabled", "disabled");
                    var OrderId = $("#hidOrderid").val();
                    var AdvancePayPass = $("#textfieldpassword").val().trim();
                    if (!AdvancePayPass) {
                        ShowMsg("请输入交易密码!");
                        $(this).val("支 付 ");
                        $(this).removeAttr("disabled");
                        return;
                    }
                    $(this).val("请求支付中...");
                    $.ajax({
                        url: "/API/VshopProcess.ashx",
                        type: "post",
                        dataType: "json",
                        timeout: 10000,
                        data: {
                            action: "AdvancePayPassVerify",
                            OrderId: OrderId,
                            AdvancePayPass: AdvancePayPass
                        },
                        async: false,
                        success: function (data) {
                            if (data.Status == undefined) {
                                ShowMsg("参数错误！", false);
                                $("#btnLoginAndBuy").val("支 付 ");
                                $("#btnLoginAndBuy").removeAttr("disabled");
                            } else if (data.Status == "OK") {
                                window.location.reload(true);
                            } else {
                                if (data.Status == "001" || data.Status == "002") {
                                    $("#loginForBuy").hide();
                                    $("#paymodel").hide();
                                    var w = $("body").width();
                                    $("#divshow").css("top", 450 + "px");
                                    $("#divshow").css("left", w / 2 - 180 + "px");
                                    if ($(document).scrollTop() <= 145) {
                                        $("#divshow").css("top", "125px");
                                    }
                                    $(".btn-continue").bind("click", function () { $("#divshow").css('display', 'none'); $(".modal_qt").remove(); });
                                    $("#divshow").css('display', 'block');
                                    var bg = "<div class='modal_qt'></div>";
                                    $('body').append(bg);
                                    if (data.Status == "001") {
                                        $("#spcounttype").text("您来晚了，订单中有商品已删除或已下架。");
                                    }
                                    else {
                                        $("#spcounttype").text("您来晚了，订单中有商品库存不足，请联系卖家处理。");
                                    }

                                    $(".btn-continue").bind("click", function () {
                                        $("#divshow").css('display', 'none');
                                        $(".modal_qt").remove();
                                        window.location.href = "/user/UserOrders.aspx";
                                    });
                                }
                                else {
                                    if (data.Status == "003") {
                                        $("#pwderrnotice").show();
                                    }
                                    else {
                                        $("#pwderrnotice").hide();
                                    }
                                    ShowMsg(data.Message, false);
                                    $("#btnLoginAndBuy").val("支 付 ");
                                    $("#btnLoginAndBuy").removeAttr("disabled");
                                }
                            }
                        }
                    });
                });
                $("#textfieldpassword").keydown(function (event) {

                    if (event.keyCode == 13) {
                        $("#btnLoginAndBuy").click();
                        return false;
                    }

                });
                return false;
            }
        }

        $("#paymodel").show();
        $("#loginToPay").removeClass("hide");
        return true;
    }
}

function closePayPop() {
    $("#loginForBuy").hide();
    $(".modal_qt").remove();
    var modeId = parseInt($("#paymentModeId").val());
    var AdvanceId = parseInt($("#AdvanceId").val())
    if (AdvanceId != modeId) {
        var d = new Date();
        window.location.href = window.location.href + "?t=1";
    }
}

function validatePay() {
    var modeId = parseInt($("#paymentModeId").val());
    var AdvanceId = parseInt($("#AdvanceId").val());
    if ($("#hidHasTradePassword").val() != "1" && modeId == AdvanceId) {
        return false;
    }
    var OrderId = $("#hidOrderid").val();
    $.ajax({
        url: "/FinishOrder",
        type: "post",
        dataType: "json",
        timeout: 10000,
        data: {
            isCallback: true,
            action: "ToPay",
            orderId: OrderId,
            paymentModeId: modeId
        },
        success: function (result) {
            if (result.Status == 0) {
                ShowMsg(result.Msg, false);
                return false;
            } else {
                var aOrderPay = document.getElementById("aOrderPay");
                aOrderPay.href = result.Msg;               
            }
        }
    })
}