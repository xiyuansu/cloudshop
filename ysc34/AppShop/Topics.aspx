<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Topics.aspx.cs" Inherits="Hidistro.UI.Web.AppShop.Topics" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.CodeBehind" Assembly="Hidistro.UI.SaleSystem.CodeBehind" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="H2" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<!DOCTYPE html>

<html lang="zh-CN">
<head>
    <title><%=pageTitle %></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1" name="viewport" />
    <meta http-equiv="content-script-type" content="text/javascript">
    <meta name="format-detection" content="telephone=no" />
    <meta name="screen-orientation" content="portrait">
    <meta name="x5-orientation" content="portrait">
    <link rel="stylesheet" href="/Admin/Shop/PublicMob/css/dist/style.css">

    <link rel="stylesheet" href="../Templates/common/style/index.css">
    <link rel="stylesheet" href="/templates/common/style/fonts/style.css" type="text/css">
    <style>
        html { font-size: 20px; /*display:flex;*/ }
        /*Note3*/
        @media only screen and (min-width: 360px) {
            html { font-size: 22px !important; }
        }

        /*iPhone6*/
        @media only screen and (min-width: 375px) {
            html { font-size: 23px !important; }
        }

        /*iPhone6 plus*/
        @media only screen and (min-width: 414px) {
            html { font-size: 25px !important; }
        }

        /*big Resolution*/
        @media only screen and (min-width: 641px) {
            html { font-size: 25px !important; }
        }

        .menuNav { display: inline-block; position: fixed; bottom: 0; left: 0; width: 100%; height: 2.4rem; z-index: 102; background: rgba(255,255,255,.95); border-top: 0.05rem solid #ddd; }

        .fulltext p span { background: none; font-size: 0.6rem !important; }

        .menuNav ul li { height: 100%; float: left; }

        .navcontent img { height: 1rem !important; width: 1rem !important; }

        .navcontent a { font-size: 0.5rem; color: #333; }

        .navcontent img { display: block; margin: 0 auto 2px; }

        .navcontent { padding: 0.4rem; text-align: center; }

        .nav-bar { position: absolute; right: 0; padding: 0 8px; border-radius: 4px; background: #eee; z-index: 999; border: 1px #E0E0E0 solid; display: none; top: 0; }

            .nav-bar ul li { border-bottom: 1px #E0E0E0 solid; text-align: center; width: 80px; font-size: 14px; line-height: 22px; padding: 4px 0px; color: #212121; }

                .nav-bar ul li:last-child { border-bottom: none; }

                .nav-bar ul li i { margin-right: 0.21rem; }
    </style>
</head>
<body>
    <asp:Literal ID="litImageServerUrl" runat="server"></asp:Literal>
    <asp:Literal ID="MeiQia_OnlineServer" runat="server"></asp:Literal>
    <div class="membersbox pad50">
        <asp:Panel ID="PanelTheme" runat="server"></asp:Panel>
        <script type="text/javascript" src="/Utility/jquery-1.11.0.min.js"></script>
        <p style="display: none">
            <H2:CnzzShow runat="server" />
        </p>
    </div>
    <script src="/Admin/Shop/PublicMob/plugins/swipe/swipe.js"></script>
    <script src="/Utility/jquery.scrollLoading.min.js"></script>
    <script type="text/javascript" src="../Templates/appshop/script/main.js"></script>
    <script type="text/javascript" src="../Templates/appshop/script/mui.min.js"></script>
    <link rel="stylesheet" href="/templates/common/style/css.css?v=3.0" type="text/css" />

    <script type="text/javascript" src="/Utility/common.js?v=3.0"></script>
    <script src="/Utility/listPageAddToCart.js" type="text/javascript"></script>
    <script type="text/javascript">

        function goSerachResult(keyword, cid) {
            OpenUrl("search-result", '{\"keywords\":\"' + keyword + '\",\"pid\":\"' + cid + '\"}');
        }
         function updateshortcart(){ 
				mui.fire(plus.webview.getWebviewById("shopcart.html"),"updateData");
			}
        
        //购物车
        function goToShoppingCart() { 
            OpenUrl("shopcart");
        }
        //首页
        function goHomePage() {
            OpenUrl("home");
        }
        //分类
        function goCategoryPage() {
            OpenUrl("category");
        }
        //个人中心
        function goUserCenterPage() {
            OpenUrl("userhome")
        }

        //去商品详情页
        function showProductDetail(id) {
            OpenUrl("product-detail", '{"productid":' + id + '}');
        }
        //分类导航
        function goCategory() {
            OpenUrl("category");
        }

        //会员中心
        function goMerberCenter() {
            OpenUrl("userhome")
        }

        //商品列表
        function goProductList() {
            OpenUrl("search-result");
        }
        //火拼团列表
        function goFightGroupList() {
            OpenUrl("rushgrouplist");
        }
        //摇一摇
        function goShake() {
            var type = GetAgentType();
            // 设置标题
            if (type == 2)
                window.HiCmd.webShowShake();
            else if (type == 1)
                loadIframeURL("hishop://webShowShake/null/");
        }

        function OpenUrl(pagename, param) {
            var openwebview = plus.webview.getWebviewById("appauto");
//          if (openwebview == null) {
//              openwebview = plus.webview.getWebviewById("appauto");
//          }
            mui.fire(openwebview, "OpenUrl", { 'pagename': pagename, 'extend': param });
        }

        $(function () {
            $("#navbar li").on("click", function () {
                var pagename = this.dataset.page;

                switch (pagename) {
                    case "home":
                        goHomePage();
                        break;
                    case "category":
                        goCategoryPage();
                        break;
                    case "shopcart":
                        goToShoppingCart();
                        break;
                    default:
                        goUserCenterPage();
                        break;
                };
            });

        })


        //监听"..."打开导航菜单        
        document.addEventListener("showAppNav", function (e) {
            var display = document.getElementById('navbar').style.display;
            if (display == "none" || display == '') {
                document.getElementById('navbar').style.display = 'block';
            } else if (display == "block") {
                document.getElementById('navbar').style.display = 'none';
            }



        });




    </script>
    <script type="text/javascript">

        $(function () {
            $('.j-swipe').each(function (index, el) {
                var me = $(this);
                me.attr('id', 'Swiper' + index); setScrollLoading();
                var id = me.attr('id');
                // alert(id)
                var elem = document.getElementById(id);
                window.mySwipe = Swipe(elem, {
                    startSlide: 0,
                    auto: 3000,
                    callback: function (m) {
                        $(elem).find('.members_flash_time').children('span').eq(m).addClass('cur').siblings().removeClass('cur')
                    },
                });
            });

            ////图片比例处理
            var imgw = $('body').width();
            $(".members_goodspic ul .b_mingoods a ").width(imgw / 2 - 20);
            $(".members_goodspic ul .b_mingoods a ").height(imgw / 2 - 20);
            setScrollLoading();

        });

        function setScrollLoading() {
            $("img").scrollLoading();
        }

    </script>

    <script type="text/javascript">
        // 商品列表高度
        $(document).ready(function () {
            $('.mingoods').width();
            var k = $('.mingoods').width();
            // $('.mingoods').height(k * 1.1);
            $('.mingoods div a img').height(k * 1.1 - 45);
        })
    </script>
    <script src="/Utility/traffic.js"></script>

    <div class="nav-bar" id="navbar" style="display: none;">
        <ul>
            <li data-page="home"><i class="icon-icon_home"></i>首页</li>
            <li data-page="category"><i class="icon-icon_category-03"></i>分类</li>
            <li data-page="shopcart"><i class="icon-icon_cart"></i>购物车</li>
            <li data-page="user"><i class="icon-icon_account"></i>个人中心</li>
        </ul>
    </div>


</body>
</html>
