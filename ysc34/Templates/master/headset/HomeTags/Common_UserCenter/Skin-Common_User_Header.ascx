<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    
    <script src="/Utility/ClientJump.js"></script>
    <Hi:PageTitle ID="PageTitle1" runat="server" />
    <Hi:MetaTags ID="MetaTags1" runat="server" />
    <script src="/Utility/star/es5-shim.js"></script>
    <link rel="stylesheet" type="text/css" href="/utility/bootflat/bootstrap.min.css" />
    <Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/user.css"></Hi:TemplateStyle>
    <link rel="stylesheet" type="text/css" href="/templates/master/default/style/style.css" />
    <link href="/Utility/validate/pagevalidator.css" rel="stylesheet" type="text/css" />
    <link href="/Utility/iconfont/iconfont.css" rel="Stylesheet" />
    <script type="text/javascript" src="/templates/master/default/script/jquery.js"></script>
    <Hi:Script ID="Script1" runat="server" Src="/utility/globals.js" />
    <asp:Literal runat="server" ID="MeiQia_OnlineServer"></asp:Literal>

</head>

<style>
    body { background: #f3f3f3; }

    * { box-sizing: content-box; }

    .col-sm-12 { width: auto; margin: 0px; padding: 0px; }
    .top-search .search-box #txt_Search_Keywords {
        float: left;
        width: 351px;
        height: 31px;
        border: 2px solid #fe5722;
        color: #D1D1D1;
        background-image: url("/templates/master/default/images/common/iconArray.png");
        background-repeat: no-repeat;
        background-position: 10px -80px;
        padding-left: 35px;
    }

    .top-search .search-box .sbtn {
        float: left;
        width: 93px;
        height: 35px;
        background-color: #fe5722;
        color: #fff;
        text-align: center;
        line-height: 35px;
        font-size: 16px;
        cursor: pointer;
        border: none;
    }
</style>

<body>

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
                                <Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" />
                            </li>
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
                                <a href='#'>全部商品分类</a>
                            </h3>
                        </li>
                        <li><a href="/"><span>首页</span></a></li>
                        <Hi:Common_PrimaryClass ID="Common_PrimaryClass1" runat="server" />
                        <Hi:Common_HeaderMune ID="Common_HeaderMune1" runat="server" />
                    </ul>
                    <div class="left-nav">
                        <div class="pro-menu">
                            <div class="menu-inner">
                                <Hi:Common_CategoriesWithWindow ID="Common_CategoriesWithWindow1" SkinName="/Ascx/tags/Skin-CategoriesWithWindow1.ascx" MaxCNum="12" runat="server" />
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
        if (currenturl != "/" && currenturl != null && currenturl.indexOf("desig_templete.aspx?skintemp=default") == -1) {
            $(".left-nav").css({ "display": "none" });
            var height = $(".top-nav .side-nav").height();
            $(".top-nav .side-nav").css("height", "auto");

            $(".side-nav .title").hover(function () {
                $(".top-nav .side-nav").css("height", "498px");
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
            var hidIsLogin = $("#hidIsLogin");
            if (hidIsLogin.val() == "1") {
                $("#liLinkLogin").addClass("fore3");
            } else {
                $("#liLinkLogin").removeClass("fore3");
            }
        });
        $(function () {
            $(" .top-search .search-box .sbtn").val("搜索");
        })
    </script>
