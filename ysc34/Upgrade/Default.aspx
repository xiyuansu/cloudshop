<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Hidistro.UI.Web.Upgrade.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>3.1UpgradeTo3.2无用文件删除</title>
    <style type="text/css">
        .divmsg { padding-top: 30px; line-height: 30px; font-size: 16px; color: green; width: 1000px; text-align: center; margin: 0 auto; }
            .divmsg h2 { color: blue; font-size: 16px; }
            .divmsg b { color: red; }
            .divmsg strong{font-size:24px; color:red;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <div id="divmsg" class="divmsg" runat="server"></div>

            <p style="text-align: center; font-size: 20px; font-family: 'Microsoft YaHei'" runat="server" visible="false" id="clearFile">
                 进行此操作之前，请确认您已经执行了升级文档说明中的,点击下面的按钮将删除无效的文件。<br />
                <br />
                <asp:Button runat="server" Text="点击删除无效文件" ID="Button1" OnClick="Button1_Click" />
            </p>
        </div>
    </form>
</body>
</html>
