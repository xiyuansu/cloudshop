<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="H5WxPay_Submit.aspx.cs" Inherits="Hidistro.UI.Web.pay.H5WxPay_Submit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>H5微信支付</title>
    <meta charset="UTF-8" />
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1" name="viewport" />
    <meta http-equiv="content-script-type" content="text/javascript" />
    <meta name="format-detection" content="telephone=no" />
    <!-- uc强制竖屏 -->
    <meta name="screen-orientation" content="portrait" />
    <!-- QQ强制竖屏 -->
    <meta name="x5-orientation" content="portrait" />
    <script src="/Utility/jquery-1.11.0.min.js" id="jquery" type="text/javascript"></script>
    <script src="/Utility/common.js?v=3.0" type="text/javascript"></script>
    <link rel="stylesheet" href="/templates/common/style/index.css?v=3.3" type="text/css" />
    <link rel="stylesheet" href="/templates/common/style/css.css?v=3.2" type="text/css" />
    <link rel="stylesheet" href="/templates/common/style/fonts/style.css?v=3.0" type="text/css" />
    <script type="text/javascript" src="/Utility/bootflat/bootstrap.min.js"></script>
    <script type="text/javascript" src="/templates/common/script/jquery.slides.min.js"></script>
    <script type="text/javascript" src="/templates/common/script/main.js"></script>
    <script type="text/javascript" src="/templates/common/script/iscroll.js"></script>
    <style type="text/css">
        a[class*="btn"] {
            display: block;
            height: 42px;
            line-height: 42px;
            color: #FFFFFF;
            text-align: center;
            border-radius: 5px;
            width: 90%;
            margin-left: 5%;
        }

        .btn-blue {
            background: #3D87C3;
            border: 1px solid #1C5E93;
        }
        .error h2{width:100%; text-align:center; color:#ff0000; font-size:larger; margin-top:45%;padding:0px;}
        .popbox{ background:#fff; }
        .popbox h3{color:#000000; font-size:larger; line-height:60px; width:100%; text-align:center; }
        .popbox ul{margin:0px; padding:0px;}
        .popbox ul li{border-top:1px solid #000; color:#ff0000; list-style:none; width:100%; font-size:large; text-align:center;  line-height:40px;}
        .popbox ul li a{ color:#ff0000; }
        .popbox ul li.repay a{color:#666;}
    </style>
    <script type="text/javascript">
        var payurl = "<%=pay_uri%>";
        $(document).ready(function (e) {
            if ($(".error h2").html() == "") {
                $(".error").hide();
                document.location.href = payurl;
                $(".popbox").addClass('is-visible');
            }
            else {
                $(".error").show();
                $(".popbox").hide();
            }
            
        });
        function FinishPay() {
            var source = parseInt($("#hidOrderSource").val());
            if (isNaN(source)) {
                source = 1;
            }
            var orderId = $("#hidOrderId").val();
            if (source == 5) {
                document.location.href = "/AliOH/MemberOrderDetails?OrderId=" + orderId;
            }
            else {
                document.location.href = "/WapShop/MemberOrderDetails?OrderId=" + orderId;
            }
        }
        function ErrorPay() {
            document.location.href = payurl;
        }
    </script>
</head>
<body>
    <input type="hidden" id="hidOrderId" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidOrderSource" runat="server" clientidmode="Static" />
    <div class="error">
        <h2><asp:Literal ID="litError" runat="server"></asp:Literal></h2>
    </div>
    <div class="att-popup popbox">
        <h3>请确认微信支付是否已完成</h3>
        <ul>
            <li><a href="javascript:void(0)" onclick="FinishPay()">已完成支付</a></li>
            <li class="repay"><a href="javascript:void(0)" onclick="ErrorPay()">支付遇到问题,重新支付</a></li>
        </ul>
    </div>
</body>
</html>

