<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Topics.aspx.cs" Inherits="Hidistro.UI.Web.Vshop.Topics" %>

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
    <link rel="stylesheet" href="/templates/common/style/fonts/style.css?v=3.2" type="text/css" />
    <link rel="stylesheet" href="../Templates/common/style/index.css">
    <link rel="stylesheet" href="/templates/common/style/fonts/style.css" type="text/css">
    <style>
        html {
            font-size: 20px;
            /*display:flex;*/
        }
        /*Note3*/
        @media only screen and (min-width: 360px) {
            html {
                font-size: 22px !important;
            }
        }

        /*iPhone6*/
        @media only screen and (min-width: 375px) {
            html {
                font-size: 23px !important;
            }
        }

        /*iPhone6 plus*/
        @media only screen and (min-width: 414px) {
            html {
                font-size: 25px !important;
            }
        }

        /*big Resolution*/
        @media only screen and (min-width: 641px) {
            html {
                font-size: 25px !important;
            }
        }

        .menuNav {
            display: inline-block;
            position: fixed;
            bottom: 0;
            left: 0;
            width: 100%;
            height: 2.4rem;
            z-index: 102;
            background: rgba(255,255,255,.95);
            border-top: 0.05rem solid #ddd;
        }

        .fulltext p span {
            background: none;
            font-size: 0.6rem !important;
        }

        .menuNav ul li {
            height: 100%;
            float: left;
        }

        .navcontent img {
            height: 1rem !important;
            width: 1rem !important;
        }

        .navcontent a {
            font-size: 0.5rem;
            color: #333;
        }

        .navcontent img {
            display: block;
            margin: 0 auto 2px;
        }

        .navcontent {
            padding: 0.4rem;
            text-align: center;
        }
            body{ padding: 0px;}
        .members_nav2 li {
        margin-right:0;
        }
    </style>
</head>
<body  style="padding-bottom:0">
    <asp:Literal ID="litImageServerUrl" runat="server"></asp:Literal>
        <asp:Literal ID="MeiQia_OnlineServer" runat="server"></asp:Literal>
    <div class="membersbox">       
        <asp:Panel ID="PanelTheme" runat="server"></asp:Panel>
        <script type="text/javascript" src="/Utility/jquery-1.11.0.min.js"></script>
        <Hi:Common_ContextMenu id="ContextMenu" runat="server" />
    </div>
    <input type="hidden" runat="server" ClientIDMode="Static" id="hidTitle" />
    <input type="hidden" runat="server" ClientIDMode="Static" id="hidDescription" />

    <input type="hidden" id="hdAppId" runat="server" ClientIDMode="Static" /> 
    <input type="hidden" id="hdTitle" runat="server" ClientIDMode="Static" />
    <input type="hidden" id="hdDesc" runat="server" ClientIDMode="Static" />
    <input type="hidden" id="hdImgUrl" runat="server" ClientIDMode="Static" />
    <input type="hidden" id="hdLink" runat="server" ClientIDMode="Static" />
    <script src="/Admin/Shop/PublicMob/plugins/swipe/swipe.js"></script>
    <script src="/Utility/jquery.scrollLoading.min.js"></script>
    <script src="../Templates/common/script/main.js"></script>

    <link rel="stylesheet" href="/templates/common/style/css.css?v=3.0" type="text/css" />
       
    <script type="text/javascript" src="/Utility/common.js?v=3.0"></script>
    <script src="/Utility/listPageAddToCart.js" type="text/javascript"></script>
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
            $('.mingoods').height(k * 1.1);
            $('.mingoods div a img').height(k * 1.1 - 45);

            $(".members_nav2 li").each(function () {
                $(this).css({ 'width': '100%', 'text-align': 'left' });
            });
        })
    </script>
    <script src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script> 
    <script src="../Utility/wxShare.js"></script>
<script src="/Utility/traffic.js"></script>
</body>
</html>
