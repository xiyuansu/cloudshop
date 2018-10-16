<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetRedEnvelopeCode.aspx.cs" Inherits="Hidistro.UI.Web.User.GetRedEnvelopeCode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <div style="text-align:center;width:100%">
        <br />
        <br />
        <div>
            恭喜您获得<asp:Label runat="server" ID="labRedEnvelopeNum"></asp:Label>个红包<br />
            快去把他分享给好友吧<br /><br /><br /><br />
        </div>
        <div>
            <asp:Image runat="server" ID="imgRedEnvelopeCode" />
        </div>
        <br />
        <div>请使用微信扫描二维码,在打开的页面选择分享</div>
    </div>
</body>
</html>
