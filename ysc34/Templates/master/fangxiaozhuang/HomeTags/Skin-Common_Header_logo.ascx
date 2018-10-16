<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    <script src="/Utility/ClientJump.js"></script>
    <Hi:PageTitle runat="server" />
    <Hi:MetaTags runat="server" />
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" />
    <script src="/Utility/icheck/icheck.min.js"></script>
    <script type="text/javascript" src="/templates/master/default/script/nav.js"></script>
    <Hi:Script ID="Script1" runat="server" Src="/utility/globals.js" />
    <Hi:Script ID="Script3" runat="server" Src="/utility/region.helper.js" />
    <link rel="stylesheet" type="text/css" href="/templates/master/default/style/nav.css" />
    <link rel="stylesheet" href="/Utility/bootflat/bootstrap.min.css" rev="stylesheet" type="text/css">
    <Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/style.css"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Style1" runat="server" Href="/style/menu.css"></Hi:TemplateStyle>
    <link href="/Utility/iconfont/iconfont.css" rel="Stylesheet" />
    <Hi:TemplateStyle ID="RegionStyle" runat="server" Href="/style/region.css"></Hi:TemplateStyle>

</head>
<style>
    * { box-sizing: content-box; }
    .header_logo { border-bottom: none; }
</style>
<body>
    <Hi:Common_OnlineServer ID="Common_OnlineServer1" runat="server"></Hi:Common_OnlineServer>
    <div id="header" class="top">
        <div class="header_logo">
            <div class="top1">
                <div class="logo1">
                    <Hi:Common_Logo ID="Common_Logo1" runat="server" />
                </div>
                <div class="xyonghu">欢迎登录</div>
            </div>


        </div>


    </div>
