<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Hidistro.UI.Web.wdgj_api.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        uCode:<asp:TextBox ID="uCodeTextBox" runat="server" Text="123456"/><br />
        secret:<asp:TextBox ID="secretTextBox" runat="server"/><br />
        操作:<asp:TextBox ID="actionTextBox" runat="server" Text="mGetGoods"/> mGetGoods / mOrderSearch / mGetOrder / mSndGoods / mSysGoods<br />
        TimeStamp:<asp:TextBox ID="timeStampTextBox" runat="server" /><br />
        Sign:<asp:TextBox ID="signTextBox" runat="server"/><br />
        查询商品：<asp:HyperLink ID="getProducts" runat="server" /><br />
        查询订单：<asp:HyperLink ID="getOrders" runat="server" /><br />
        <asp:Button ID="saveButton" runat="server" Text="生成" OnClick="saveButton_Click" />

    </div>

        <div>
            新网店管家测试地址:http://www.polyapi.com/Test/Index
        </div>
    </form>
</body>
</html>
