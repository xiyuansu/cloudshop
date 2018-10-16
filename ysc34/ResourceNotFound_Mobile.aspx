<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResourceNotFound.aspx.cs" Inherits="Hidistro.UI.Web.ResourceNotFound" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title></title>
    <style>
        body { }
        .notexit { padding: 15px; font-weight: normal; font-size: 14px; }
        .notexit b { display: block; font-weight: normal; background: #fff; color: #ff0000; height: 20px; line-height: 20px; font-size: 16px; }
    </style>
</head>
<body>
    <div>
        <h5 class="notexit"><b>
            <asp:Literal runat="server" ID="litMsg" /></b></h5>
    </div>
</body>
</html>
