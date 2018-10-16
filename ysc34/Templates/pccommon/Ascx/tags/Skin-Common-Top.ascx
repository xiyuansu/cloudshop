<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <script type="text/javascript" src="/utility/jquery-1.8.3.min.js"></script>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />

    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    <Hi:PageTitle runat="server" />
    <Hi:MetaTags runat="server" />
    <script src="/Utility/icheck/icheck.min.js"></script>
    <link rel="stylesheet" href="/Admin/css/bootstrap.min.css" rev="stylesheet" type="text/css">
    <link rel="stylesheet" type="text/css" href="/templates/pccommon/style/_all.css" />
    <Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/style.css?v=3.0"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Style1" runat="server" Href="/style/menu.css?v=3.0"></Hi:TemplateStyle>
    <script src="/Utility/windows.js?v=3.0"></script>
    <link href="/Admin/css/windows.css?v=3.0" rel="stylesheet" />

</head>
<body>
    <style>
        * {
            box-sizing: content-box;
        }

        .main {
            margin: 0 auto;
            width: 1000px;
            clear: both;
            overflow: hidden;
        }

        .top .top-main {
            width: 1000px;
        }
    </style>
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
                                <Hi:Common_LoginLink ID="Common_LoginLink1" runat="server" /></li>
                            <li class="fore3">
                                <Hi:Common_VshopQRCode ID="Common_VshopQRCode1" runat="server" />
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(function () {
            $('.cart_chekout').click(function () {
                $('.modal_qt').css('display', 'none');
            })

           
			$('.dialog_title_r').click(function(){
        		$('.login_tan').hide();
        		$('.modal_qt').hide();
        	})
            var hidIsLogin = $("#hidIsLogin");
            if (hidIsLogin.val() == "1") {
                $("#liLinkLogin").addClass("fore3");
            } else {
                $("#liLinkLogin").removeClass("fore3");
            }
        }
        )
    </script>
