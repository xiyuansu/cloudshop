<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_Submit.aspx.cs" Inherits="Hidistro.UI.Web.Pay.wx_Submit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微信支付</title>
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
    <link rel="stylesheet" href="/templates/common/style/index.css" rev="stylesheet" type="text/css" />
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
        }

        .btn-blue {
            background: #3D87C3;
            border: 1px solid #1C5E93;
        }
    </style>
</head>
<body>
    <%if (isWeiXin)
        {%>
    <script type="text/javascript">
        var OrderId = "<%=Page.Request.QueryString["OrderId"]%>";
        var IsServiceOrder = "<%=IsServiceOrder%>";
        var isFightGroup = "<%=isFightGroup%>";
        var isOfflineOrder = "<%=isOfflineOrder%>";
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            WeixinJSBridge.invoke('getBrandWCPayRequest', <%= pay_json %>, function(res){
                if(res.err_msg == "get_brand_wcpay_request:ok" ) {
                
                    if(isOfflineOrder=="true"){
                        alert("恭喜您,支付成功!");
                    }else{
                        if(isFightGroup=="true"){
                            alert("订单支付成功!");
                            location.replace("/vshop/FightGroupSuccess.aspx?orderId="+OrderId);
                        }else{
                            $.ajax({
                                url: "/API/VshopProcess.ashx",
                                type: 'post', dataType: 'json', 
                                data: { action: "CheckSendRedEnvelope", OrderId: OrderId},
                                success: function (resultData) {
                        
                                    if (resultData.success=="true") {
                                        //跳转到发红包页面
                                        location.replace("/vshop/SendRedEnvelope");
                                    }
                                    else {                               
                                        alert("订单支付成功!点击确认进入我的订单中心");
                                        if(IsServiceOrder!="true"){
                                            location.replace("/vshop/MemberOrderDetails?OrderId="+OrderId);                                 
                                        }
                                        else{
                                            location.replace("/vshop/ServiceMemberOrderDetails?OrderId="+OrderId);                                 
                                        }
                                    }
                                }
                            });
                        }
                    }
                }
                else
                {
                    if(isOfflineOrder=="true"){
                        alert("支付取消或者失败!");
                    }
                    else{
                        alert("支付取消或者失败");
                        if(IsServiceOrder!="true"){
                            location.replace("/vshop/MemberOrderDetails?OrderId="+OrderId);                                 
                        }
                        else{
                            location.replace("/vshop/ServiceMemberOrderDetails?OrderId="+OrderId);                                 
                        }
                    }
                
                }
                //alert("订单支付成功!点击确认进入我的订单中心");
            });
        });
    </script>
    <%}
        else
        { %>
    <br />
    <br />
    <a class="btn-blue" href="<%=pay_uri %>">立即支付</a>
    <%} %>
</body>
</html>
