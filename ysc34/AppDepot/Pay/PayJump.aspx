<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayJump.aspx.cs" Inherits="Hidistro.UI.Web.AppDepot.PayJump" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <title>门店APP支付中转页面</title>

    <meta charset="UTF-8" />
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1" name="viewport" />
    <meta http-equiv="content-script-type" content="text/javascript">
    <meta name="format-detection" content="telephone=no" />
    <!-- uc强制竖屏 -->
    <meta name="screen-orientation" content="portrait">
    <!-- QQ强制竖屏 -->
    <meta name="x5-orientation" content="portrait">
    <script src="/Utility/jquery-1.11.0.min.js" id="jquery"></script>
    <script src="/Utility/common.js?v=3.0" type="text/javascript"></script>
    <link rel="stylesheet" href="/Utility/bootflat/bootstrap.min.css" rev="stylesheet"
        type="text/css">
    <link rel="stylesheet" href="/Utility/bootflat/bootflat.min.css" rev="stylesheet"
        type="text/css">
    <link rel="stylesheet" href="/templates/common/style/css.css" rev="stylesheet" type="text/css">
    <link rel="stylesheet" href="/templates/common/style/mycss.css" rev="stylesheet" type="text/css">
    <link rel="stylesheet" href="/templates/common/style/main.css" rev="stylesheet" type="text/css">
    <script src="/Utility/bootflat/bootstrap.min.js"></script>
    <script src="/templates/common/script/jquery.slides.min.js"></script>
    <script src="/templates/common/script/main.js"></script>
    <script src="/templates/common/script/iscroll.js"></script>

    <script language="javascript" type="text/javascript">
        var sUserAgent = navigator.userAgent.toLowerCase();
        var bIsWX = sUserAgent.match(/micromessenger/i) == "micromessenger";
        var sessionId = getParam("SessionId");
        $(document).ready(function (e) {
            if ($("#hidErrMsg").val() != "") {
                alert($("#hidErrMsg").val());
            } else {
                if ($("#inputPanel").length == 0) {
                    var orderId = getParam("OrderId");
                    var isOffline = getParam("IsOffline");
                    if (orderId != "") {
                        if (bIsWX) {
                            if ($("#hasWxPayRight").val() != "1") {
                                alert("平台未配置微信支付");
                            }
                        }
                        else {
                            if ($("#hasAliPayRight").val() != "1") {
                                alert("平台未配置支付宝网页支付");
                            }
                        }
                    }
                    else {
                        if (bIsWX) {
                            if ($("#hasWxPayRight").val() == "0") {
                                $("#inputPanel").hide();
                                alert("平台未配置微信支付");
                            }
                        }
                        else {
                            if ($("#hasAliPayRight").val() == "0") {
                                $("#inputPanel").hide();
                                alert("平台未配置支付宝网页支付");
                            }
                        }
                    }
                }
            }
            $("#toPay").click(function (e) {
                var orderId = getParam("OrderId");
                var isOffline = getParam("IsOffline");
                if (orderId != "" && isOffline != "") {
                    if (bIsWX) {
                        location.replace("/Vshop/StoreOrderPay.aspx?OrderId=" + orderId + "&IsOffline=" + isOffline);
                    }
                    else {
                        location.replace("/WapShop/StoreOrderPay.aspx?OrderId=" + orderId + "&IsOffline=" + isOffline);
                    }
                }
                var payAmount = parseFloat($("#txtPayAmount").val());
                if (isNaN(payAmount) || payAmount <= 0 || payAmount > 10000000) {
                    alert("请输入正确的支付金额,金额限制在0.01至10000000之间");
                    return false;
                }
                if ($.trim(sessionId) == "") {
                    alert("SessionId不能为空");
                    return false;
                }
                $.ajax({
                    url: "/AppDepot/StoreAppAPI.ashx",
                    type: 'post', dataType: 'json', timeout: 10000,
                    data: { action: "GenerateOfflineOrder", SessionId: sessionId, PayAmount: payAmount, Remark: "门店线下订单用户输入金额在线支付" },
                    success: function (resultData) {
                        if (resultData.ErrorResponse != undefined) {
                            alert("不正确的支付请求");
                        }
                        else {
                            var orderId = resultData.Result.SerialNumber;
                            var isOffline = "true";
                            if (orderId != "") {
                                if (bIsWX) {
                                    location.replace("/Vshop/StoreOrderPay.aspx?OrderId=" + orderId + "&IsOffline=" + isOffline);
                                }
                                else {
                                    location.replace("/WapShop/StoreOrderPay.aspx?OrderId=" + orderId + "&IsOffline=" + isOffline);
                                }
                            }

                        }
                    }
                });
            })
        });
    </script>
    <style type="text/css">
        body{
            background:#fff;
        }
        .invoice {
            background: #fff;
            float: left;
            width: 100%;
            padding: 100px 0;
            font-size: 1rem;
        }

        .item-content {
        }

        #txtPayAmount {
            border: 1px solid #e7e5ea;
            line-height: 32px;
            padding-left: 10px;
        }

        #top {
            height: 40px;
            padding: 0 2rem;
            background: #ff5722;
            display: inline-block;
            line-height: 40px;
            color: #fff;
            border-radius: 4px;
        }
        .item-title span{ float: left;color: #333;width:6.5rem; line-height: 32px;font-weight: bold;}
        .zhifu a{background: #ff5722;
color: #fff;
text-align: center;
border-radius: 0.2rem;
font-size:1rem;
height: 2.8rem;
line-height: 2.8rem;
width: 100%;
border: 0;
display: inline-block;}
    </style>
</head>

<body>
    <form id="myform" runat="server">
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hasWxPayRight" />
        <!--是否有微信授权-->
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hasAliPayRight" />
        <!--非微信端跳转端，如果为空则表示都未授权-->
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidErrMsg" Value="" />
        <div id="inputPanel" runat="server" clientidmode="static">
            <div class="list-block invoice mt_15" style="background: #fff;">
                <ul>
                    <li class="item-content">
                        <div class="item-inner pd_0 ">
                            <div class="item-title" style="overflow: visible;clear: both; margin-top: 10px; padding: 0 10px;">
                               <span>支付金额:</span> 
                                <input id="txtPayAmount" class="form-control" style="border: 0; font-size: 14px;" placeholder="请输入支付金额" type="number">
                            </div>
                            <div style="clear: both; margin-top: 30px; padding: 0 10px;" class="zhifu">
                                <a id="toPay" class="btn btn-warning btn-block" >立即支付</a>
                                <%--<a id="finishPay" class="btn btn-success btn-block" style="height: 40px; line-height: 1.8">完成支付</a>--%>
                            </div>
                        </div>
                    </li>

                </ul>
            </div>

        </div>
    </form>
</body>
</html>
