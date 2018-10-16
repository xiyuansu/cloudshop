<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="app_alipay_Submit.aspx.cs"
    Inherits="Hidistro.UI.Web.pay.app_alipay_Submit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-

transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <script type="text/javascript" src="../Templates/appshop/script/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="../Utility/globals.js"></script>
           <script type="text/javascript" src="../Templates/appshop/script/mui.min.js"></script>
    <script type="text/javascript" src="../Templates/appshop/script/main.js"></script>
       <script type="text/javascript" src="../Templates/appshop/script/AppAuto.js"></script>
    <script type="text/javascript">
        var OrderId = GetQueryString("orderId");
        var isFightGroup = "<%=isFightGroup%>";
        var type = "alipay";
        var payUrl = "";
        $(function () {
            payUrl = '<%= pay_json %>';
        });

        function paymentReturn(ret) { // ret 1 为支付成功，ret 0为失败
            if (ret == 0) {
                alert("支付失败,参数配置错误或者用户取消支付");
                location.replace("/AppShop/MemberOrderDetails.aspx?OrderId=" + OrderId);
            }
            var isrecharge = GetQueryString("isrecharge");
            if (isFightGroup == "true") {
                $.ajax({
                    url: "/AppShop/AppShopHandler.ashx",
                    type: 'post', dataType: 'json', timeout: 10000,
                    data: { action: "FightGroupShare", OrderId: OrderId },
                    success: function (resultData) {
                        var shareJson = "{\"Result\":{\"ShareImage\":\"" + resultData.Result.ShareImage + "\",\"ShareTitle\":\"" + resultData.Result.ShareTitle + "\",\"ShareContent\":\"" + resultData.Result.ShareContent + "\",\"ShareLink\":\"" + resultData.Result.ShareLink + "\"}}";
                        goFightGroupSuccess(resultData.Result.Status, resultData.Result.NeedJoinNumber, shareJson);
                    }
                });
            } else if (isrecharge == 1) {
                location.href = "/AppShop/MyAccountSummary.aspx";
            } else {
                location.href = "/AppShop/MemberOrderDetails.aspx?orderId=" + GetQueryString("orderId");
            }
        }

    </script>
</body>
</html>
