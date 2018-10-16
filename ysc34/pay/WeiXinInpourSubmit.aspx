<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeiXinInpourSubmit.aspx.cs" Inherits="Hidistro.UI.Web.pay.WeiXinInpourSubmit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<script type="text/javascript">
    document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
        WeixinJSBridge.invoke('getBrandWCPayRequest', <%= pay_json %>, function(res){
            if(res.err_msg == "get_brand_wcpay_request:ok" ) {
                alert("订单支付成功!点击确认进入我的订单中心");
                location.href = "/vshop/MyAccountSummary.aspx";
            }
            else
            {
                alert("支付取消或者失败");
                location.href = "/vshop/MyAccountSummary.aspx";
            }
            //alert("订单支付成功!点击确认进入我的订单中心");
        });
    });
</script>
</body>
</html>