<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QRTakeCode.aspx.cs" Inherits="Hidistro.UI.Web.QRTakeCode" %>

<html>
<head>
    <script src="Utility/jquery-1.8.3.min.js"></script>
    <script>
        $(function () {
            var url = "<%=this.takeCode%>";
            $("#imgQrCode").attr("src", "/API/VshopProcess.ashx?action=GenerateTwoDimensionalImage&url=" + url);

        })
    </script>

</head>
<body>
    <div style="text-align:center; padding:50px;">
        <img id="imgQrCode" width="250" height="250" src="">
    </div>

</body>
</html>
