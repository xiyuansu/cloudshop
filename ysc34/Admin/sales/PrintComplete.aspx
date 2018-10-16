<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintComplete.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.PrintComplete" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="style/layout.css" />
<script src="../../Utility/jquery-1.6.4.min.js"></script>
<script src="../../Utility/jquery.artDialog.js"></script>
<script src="../../Utility/iframeTools.js"></script>
    <style type="text/css">
      
        .submit_DAqueding{ width:100px; height:38px;background:url(../images/icon.gif) no-repeat -159px -362px; color:#FFF; border:0;font-size:14px; font-weight:700;cursor:pointer;}
    </style>
<script>
    function closeclicks() {
        art.dialog.close();
    }
    $(function () {
        
    })
</script>
</head>
<body>
<%= script%>
<div style=" margin-left:400px; margin-top:100px;"><input type="button" value="关闭" class="btn btn-primary" id="printBtn" onclick="closeclicks()"/></div>
</body>
</html>
