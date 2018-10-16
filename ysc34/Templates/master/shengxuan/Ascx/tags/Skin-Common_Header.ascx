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
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.8.3.min.js" />
    <script src="/Utility/icheck/icheck.min.js"></script>
    <script type="text/javascript" src="/templates/pccommon/script/nav.js"></script>
    <Hi:Script ID="Script1" runat="server" Src="/utility/globals.js" />
    <Hi:Script ID="Script3" runat="server" Src="/utility/region.helper.js" />
    <link rel="stylesheet" type="text/css" href="/Admin/css/bootstrap.min.css">
    <Hi:TemplateStyle ID="Style2" runat="server" Href="/style/nav.css"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/style.css"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Style1" runat="server" Href="/style/menu.css"></Hi:TemplateStyle>
    <link href="/Utility/iconfont/iconfont.css" rel="Stylesheet" />
    <Hi:TemplateStyle ID="RegionStyle" runat="server" Href="/style/region.css"></Hi:TemplateStyle>
    <script src="/Utility/windows.js"></script>
    <link href="/Admin/css/windows.css" rel="stylesheet" />
    <Hi:TemplateStyle ID="Style3" runat="server" Href="/style/home.css"></Hi:TemplateStyle>
    <asp:Literal runat="server" ID="MeiQia_OnlineServer"></asp:Literal>

</head>
<style>
    * {
        box-sizing: content-box;
    }
</style>



<body>
    <Hi:Common_OnlineServer ID="Common_OnlineServer1" runat="server"></Hi:Common_OnlineServer>
    <div id="header" class="top">

        <div class="top-w">
            <div class="top-main">
                <div class="header_top1">
                    <div class="fl">
                        <Hi:Common_CurrentUser runat="server" ID="lblCurrentUser" />
                    </div>
                    <div class="fr top1_r">
                        <ul>
                            <li>
                                <Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" /></li>
                            <li id="liLinkLogin">
                                <Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" /></li>
                            <li class="fore3">
                                <Hi:Common_VshopQRCode ID="Common_VshopQRCode1" runat="server" />
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div class="top-search clearfix">
            <div class="logo">
                <Hi:Common_Logo ID="Common_Logo1" runat="server" />
            </div>
            <div class="search-box">

                <Hi:Common_Search ID="Common_Search" runat="server" />
            </div>
            <div class="cart-box">
                <a href="/ShoppingCart.aspx">
                    <Hi:Common_ShoppingCart_Info ID="Common_ShoppingCart_Info1" runat="server" />
                </a>
            </div>
        </div>

        <div class="top-nav">
            <div class="nav-wrap">
                <div class="side-nav">
                    <ul class="clearfix">
                        <li id="liall">
                            <h3 class="title">
                                商品分类
                            </h3>
                        </li>
                        <li><a href="/"><span>首页</span></a></li>
                        <Hi:Common_PrimaryClass ID="Common_PrimaryClass1" runat="server" />
                        <Hi:Common_HeaderMune ID="Common_HeaderMune1" runat="server" />
                    </ul>
                    <div class="left-nav">
                        <div class="pro-menu">
                            <div class="menu-inner">
                                <Hi:Common_CategoriesWithWindow ID="Common_CategoriesWithWindow1" MaxCNum="6" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>




    <script>
        var currenturl = window.location.pathname;
        currenturl = currenturl.toLowerCase();
        var waittime;
        if (currenturl != "/" && currenturl != null && currenturl.indexOf("default.aspx") == -1 && currenturl.indexOf("default") == -1 && currenturl.indexOf("desig_templete.aspx?skintemp=default") == -1) {
            $(".left-nav").css({ "display": "none" });
            var height = $(".top-nav .side-nav").height();
            $(".top-nav .side-nav").css("height", "auto");

            $(".side-nav .title").hover(function () {
                $(".left-nav").css({ "display": "block" });
                $(".left-nav").addClass("left-nav01");
            }, function () {
                $(".left-nav").css({ "display": "none" });
            });

            $(".pro-menu").hover(function () {
                $(".left-nav").css({ "display": "block" });

            }, function () {
                $(".left-nav").css("display", "none");
            });
        }


        $(document).ready(function () {
            $('.dialog_title_r').click(function () {
                $('.login_tan').hide();
                $('.modal_qt').hide();
            })
            var hidIsLogin = $("#hidIsLogin");
            if (hidIsLogin.val() == "1") {
                $("#liLinkLogin").addClass("fore3");
            } else {
                $("#liLinkLogin").removeClass("fore3");
            }
        });

    </script>
