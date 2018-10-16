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
    <script type="text/javascript" src="/utility/jquery-1.8.3.min.js?v=3.4"></script>
    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    <script type="text/javascript">
        //如果是网站首页直接跳转到WAP端首页，如果是其它页面，且该页面对应WAP端有相应的页面对直接中转到对应的WAP页面，如果没有则不跳转
        var pageurl = document.location.href.toLowerCase();
        var sUserAgent = navigator.userAgent.toLowerCase();
        var bIsIpad = sUserAgent.match(/ipad/i) == "ipad";
        var bIsIphoneOs = sUserAgent.match(/iphone os/i) == "iphone os";
        var bIsMidp = sUserAgent.match(/midp/i) == "midp";
        var bIsUc7 = sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4";
        var bIsUc = sUserAgent.match(/ucweb/i) == "ucweb";
        var bIsAndroid = sUserAgent.match(/android/i) == "android";
        var bIsCE = sUserAgent.match(/windows ce/i) == "windows ce";
        var bIsWM = sUserAgent.match(/windows mobile/i) == "windows mobile";
        var bIsWX = sUserAgent.match(/micromessenger/i) == "micromessenger";
        //bIsWM = true;

        if ((bIsIpad || bIsIphoneOs || bIsMidp || bIsUc7 || bIsUc || bIsAndroid || bIsCE || bIsWM || bIsWX) && (HasWapRight || HasVshopRight)) {
            //解决分享到微信时，浏览器跳转到网站首页的问题
            DirectUrl = GetWapUrl();
            if (DirectUrl != "") {
                if (bIsWX && HasVshopRight)
                    location.href = "/vShop/" + DirectUrl;
                else if (HasWapRight)
                    location.href = "/Wapshop/" + DirectUrl;
            }
        }
        ///判断是否需要跳转，如果当前页面为首页或者帮助页，文单页则跳转，其它的不需要跳转
        function IsDirect() {

            if (pageurl.indexOf("/default.aspx") > -1) return true;
            if (pageurl.indexOf("/article/") >= -1 && pageurl.indexOf("/help/") >= -1) return false;
            return true;
        }
        ///获取PC端对应Wap端的页面地址，如果没有对应地址则返回空
        function GetWapUrl() {
            var PageKey = "";
            var port = document.location.port;
            var domain = document.domain;
            var param = document.location.search;
            if (port != "80" && port != "") domain = domain + ":" + port;
            domain = document.location.protocol + "//" + domain;
            if ((pageurl.length == domain.length || (pageurl.length - 1) == domain.length) && pageurl.indexOf(domain) == 0) { return "default.aspx"; }
            pageurl = pageurl.replace(domain.toLowerCase(), "");
            if (pageurl.indexOf("/default") > -1) { return "/default"; }
            if (pageurl.indexOf("/groupbuyproductdetails") > -1) { return "groupbuyproductdetails" + param }
            if (pageurl.indexOf("/countdownproductsdetails") > -1) { return "countdownproductsdetails" + param }
            if (pageurl.indexOf("/product_detail") > -1) { return "ProductDetails?productId=" + pageurl.replace("/product_detail/", "").replace(param.toLowerCase(), "") + param.replace("?", "&"); }
            if (pageurl.indexOf("/productdetails") > -1) { return "ProductDetails" + param; }
            if (pageurl.indexOf("/presaleproductdetails") > -1) { return "presaleproductdetails" + pageurl.replace("/presaleproductdetails", ""); }
            if (pageurl.indexOf("/brand_detail") > -1) { return "BrandDetail?brandId=" + pageurl.replace("/brand/brand_detail/", ""); }
            if (pageurl.indexOf("/brand") > -1) { return "brandlist" + pageurl.replace("/brand", ""); }
            if (pageurl.indexOf("/browse/category") > -1) { return "ProductList?categoryId=" + pageurl.replace("/browse/category/", ""); }
            if (pageurl.indexOf("/category") > -1) { return "productsearch"; }
            if (pageurl.indexOf("/subcategory") > -1) { return "ProductList" + pageurl.replace("/subcategory", ""); }
            if (pageurl.indexOf("/groupbuyproducts") > -1) { return "GroupBuyList"; }
            if (pageurl.indexOf("/countdownproducts") > -1) { return "CountDownProducts"; }
            if (pageurl.indexOf("/login") > -1) { return "login"; }
            if (pageurl.indexOf("userdefault") > -1) { return "MemberCenter.aspx"; }
            if (pageurl.indexOf("userorders") > -1) { return "MemberOrders.aspx"; }
            if (pageurl.indexOf("usershippingaddresses") > -1) { return "ShippingAddresses.aspx" }
            if (pageurl.indexOf("user/referralregisteragreement") > -1) { return "ReferralRegisterAgreement.aspx"; }
            if (pageurl.indexOf("user/referralregisterresults") > -1) { return "referralregisterresults"; }
            if (pageurl.indexOf("/user/") > -1) { return "MemberCenter.aspx"; }
            if (pageurl.indexOf("articles") > -1) { return "Articles.aspx" + param; }
            if (pageurl.indexOf("articledetails") > -1) { return "ArticleDetails" + param; }
            if (pageurl.indexOf("storeappdownload") > -1) { return ""; }
            if (pageurl.indexOf("appdownload") > -1) { return "appdownload"; }
            if (pageurl.indexOf("registeredcoupons") > -1) { return "registeredcoupons"; }//解决扫码登录无法跳转的bug
            if (pageurl.indexOf("referralagreement") > -1) { return "ReferralAgreement" + param; }
            if (pageurl.indexOf("register") > -1) { return "login?action=register"; }
            if (pageurl.indexOf("/un_product_detail") > -1) { return "ProductDetails?productId=" + pageurl.replace("/un_product_detail/", "").replace(param.toLowerCase(), "") + param.replace("?", "&"); }
            return "";
        }

        function Resize() {
            var width = $(window).width(),
                height = $(window).height();
            $("body").css({
                "width": width,
                "height": height,
            });
            if (width <= 1190) {
                $("body").css({
                    "overflow": "auto"
                });
            }
        };

        function GetPageKey(pagepre, IsEnd) {
            if (pageurl.indexOf(pagepre) > -1) {
                var endIndex = 0;
                if (!IsEnd) {
                    endIndex = pageurl.indexOf(".aspx");
                    if (endIndex <= -1) endIndex = pageurl.indexOf(".htm")
                }
                else {
                    endIndex = pageurl.length;
                }
                var startIndex = pageurl.indexOf(pagepre) + pagepre.length;
                if (startIndex >= 0 && endIndex > startIndex)
                    return pageurl.substring(startIndex, endIndex);
            }
            return "0";
        }</script>
    <Hi:PageTitle runat="server" />
    <Hi:MetaTags runat="server" />
    <script type="text/javascript" defer="defer" src="/Utility/icheck/icheck.min.js"></script>
    <script type="text/javascript" src="/templates/pccommon/script/nav.js"></script>
    <script type="text/javascript" src="/templates/pccommon/script/swiper.min.js"></script>
    <script type="text/javascript" src="/utility/globals.js?v=3.0"></script>
    <script type="text/javascript" src="/utility/region.helper.js?v=3.0"></script>
    <script type="text/javascript" src="/utility/traffic.js?v=3.0"></script>
    <Hi:TemplateStyle ID="Style2" runat="server" Href="/style/nav.css?v=3.35"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/style.css?v=3.35"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Style1" runat="server" Href="/style/menu.css?v=3.35"></Hi:TemplateStyle>
    <link href="/Utility/iconfont/iconfont.css?v=3.35" rel="Stylesheet" />
    <Hi:TemplateStyle ID="RegionStyle" runat="server" Href="/style/region.css?v=3.35"></Hi:TemplateStyle>
    <script type="text/javascript" defer="defer" src="/Utility/windows.js?v=3.35"></script>
    <link href="/Admin/css/windows.css?v=3.35" rel="stylesheet" />
    <Hi:TemplateStyle ID="Style3" runat="server" Href="/style/home.css?v=3.35"></Hi:TemplateStyle> 
    <asp:Literal runat="server" ID="MeiQia_OnlineServer"></asp:Literal>
    <link href="/templates/master/default/plugins/jquery.cxscroll-1.2.2/css/layout.css" rel="stylesheet" />
    <link rel="stylesheet" href="/admin/css/bootstrap-datetimepicker.css?v=3.35" type="text/css" media="screen" />
    <link rel="stylesheet" href="/templates/master/xiaomi/style/swiper.min.css?v=3.35" type="text/css" media="screen" />
    <script type="text/javascript" src="/admin/js/bootstrap-datetimepicker.min.js"></script>
    <script type="text/javascript" src="/admin/js/bootstrap-datetimepicker.zh-CN.js"></script>  

</head>
<style>
    * {
        box-sizing: content-box;
    }
</style>



<body onResize="Resize()">
    <Hi:Common_OnlineServer ID="Common_OnlineServer1" runat="server"></Hi:Common_OnlineServer>
<div id="header">     
    <div class="site-topbar">
			<div class="container">
				<div class="topbar-nav">
					<a href="/" class="home"><img src="/templates/master/xiaomi/images/home.png"></a>
					<ul>
						<li class="fore3">
							<Hi:Common_VshopQRCode ID="Common_VshopQRCode1" runat="server" />
						</li>
						<li>
							<Hi:Common_AppDownloadQRCode ID="Common_AppDownloadQRCode1" runat="server" />
						</li>
					</ul>
					
				</div>
				<div class="topbar-cart">
					<a class="cart-mini" href="shoppingCart.aspx"><img src="/templates/master/xiaomi/images/cart.png" class="img_cart">购物车</a>
				</div>
				<div class="topbar-info" id="J_userInfo">
					<div class="fr top1_r">
                        <ul>
                            <li>
                                <Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" /></li>
                            <li id="liLinkLogin">
                                <Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" /></li>
                        </ul>
                    </div>
				</div>
			</div>
		</div>


<div class="site-header">
			<div class="container">
				<div class="header-logo">
					<Hi:Common_Logo ID="Common_Logo1" runat="server" />
				</div>
				<div class="header-nav">
					<ul class="nav-list J_navMainList clearfix">
						<Hi:Common_PrimaryClass ID="Common_PrimaryClass1" runat="server" />
                        <Hi:Common_HeaderMune ID="Common_HeaderMune1" runat="server" />
					</ul>
				</div>

				<div class="header-search">
					<Hi:Common_Search ID="Common_Search" runat="server" skinName="/HomeTags/Common_Search/Skin-Common_DefaultSearch.ascx" />
				</div>
			</div>
		</div>

<!--轮播图跟分类 -->
		<div class="container home-hero">
			<div class="slider">
				<div class="container">
						<div class="span10 offset1">
							<div class="flexslider">
								<Hi:Common_CustomAd runat="server" AdId="34" />								
							</div>
						</div>
				</div>
			</div>

			<!--分类菜单 -->
			<div class="left-nav">
				<div class="pro-menu">
					<Hi:Common_CategoriesWithWindow ID="Common_CategoriesWithWindow1" MaxCNum="10" runat="server" skinName="/HomeTags/Skin-DefaultCategoriesWithWindow.ascx" />
				</div>
			</div>

		</div>
  </div>      

    <script>
        var currenturl = window.location.pathname;
        currenturl = currenturl.toLowerCase();
        var waittime;
        if ((currenturl != "/" && currenturl != null && currenturl.indexOf("default.aspx") == -1 && currenturl.indexOf("default") == -1 && currenturl.indexOf("desig_templete.aspx?skintemp=default") == -1) ||
            ((currenturl == "/" || currenturl == null || currenturl.indexOf("default.aspx") >= 0 || currenturl.indexOf("default") >= 0 || currenturl.indexOf("desig_templete.aspx?skintemp=default") >= 0) && homePageTopicId > 0)
            ) {
            $(".pro-menu").hover(function () {
                $(".left-nav").addClass("left-nav01");
                $(".left-nav").css({ "display": "block" });

            }, function () {
                $(".left-nav").removeClass("left-nav01");
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
