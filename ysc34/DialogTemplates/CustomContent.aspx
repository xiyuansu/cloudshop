<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomContent.aspx.cs" Inherits="Hidistro.UI.Web.DialogTemplates.CustomContent" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register src="/Admin/Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/utility/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="/utility/jquery.artDialog.js"></script>
    <script type="text/javascript" src="/utility/jquery.cookie.js"></script>
    <link href="css/design.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        var UEEditor;
       
        $(document).ready(function (e) {
            UEEditor = UE.getEditor('editDescription');
        });
        function HTMLEncode(input) {
            return String(input).replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
        }

        function HTMLDecode(input) {
            return String(value).replace(/&quot;/g, '"').replace(/&#39;/g, "'").replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&amp;/g, '&');
        }

        function GetContetnsHtml() {
            return UEEditor.getContent();
        }
    </script>
    <script type="text/javascript" src="/Admin/js/HiShopComPlugin.js"></script>
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="/admin/css/style.css" type="text/css" media="screen" />
</head>
<body>
    <style type="text/css">
        #albums, #icon-container {
    position: fixed;
    top: 50%;
    left: 50%;
    margin: -375px 0 0 -450px;
    width: 900px;
    height: 550px;
    background: #fff;
    padding-bottom: 100px;
    overflow: hidden;
    border: 1px solid rgba(0,0,0,.2);
    border-radius: 6px;
    box-shadow: 0 5px 15px rgba(0,0,0,.5);
    z-index: 999999;
}
    </style>
    <form id="form1" runat="server">
    <div>
        <Hi:Ueditor ID="editDescription" FilterMode="false" runat="server" Width="100%" />
    </div>
    <uc1:ImageList ID="ImageList" runat="server" />
    </form>
</body>
</html>
