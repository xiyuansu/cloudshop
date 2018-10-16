<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="app_wxpay_submit.aspx.cs" Inherits="Hidistro.UI.Web.pay.app_wxpay_submit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <script type="text/javascript" src="../Templates/appshop/script/jquery-1.11.0.min.js"></script>
     <script type="text/javascript" src="../Templates/appshop/script/mui.min.js"></script>
    <script type="text/javascript" src="../Utility/globals.js"></script>
    <script type="text/javascript" src="../Templates/appshop/script/main.js"></script>
     <script type="text/javascript" src="../Templates/appshop/script/AppAuto.js"></script>
    <script type="text/javascript">
var isFightGroup = "<%=isFightGroup%>";
        var OrderId = GetQueryString("orderId");
        var isFightGroup = "<%=isFightGroup%>";
        var type = "wxpay";
        var payUrl = "";
           $(function () {
               payUrl = '<%= pay_json %>';
               //document.write(payUrl);
        });

        function paymentReturn(ret) { // ret 1 为支付成功，ret 0为失败
            if (ret == 0) {
//                alert("支付失败,参数配置错误或者用户取消支付");
                location.replace("/AppShop/MemberOrderDetails?OrderId=" + OrderId);
            }
            var isrecharge = parseInt(GetQueryString("isrecharge"));
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
                location.href = "/AppShop/MyAccountSummary";
            } else {
                location.href = "/AppShop/MemberOrderDetails?orderId=" + OrderId;
            }
        }

    </script>
</body>
</html>

