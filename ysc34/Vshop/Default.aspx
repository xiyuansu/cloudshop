<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Hidistro.UI.Web.Vshop.Default" %>

<%@ OutputCache Duration="60" VaryByParam="none" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.CodeBehind" Assembly="Hidistro.UI.SaleSystem.CodeBehind" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="H2" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<!doctype html>
<html lang="zh-CN">
<head>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport" />
    <meta http-equiv="content-script-type" content="text/javascript">
    <meta name="format-detection" content="telephone=no" />
    <meta name="screen-orientation" content="portrait">
    <meta name="x5-orientation" content="portrait">

    <link rel="stylesheet" href="/Admin/Shop/PublicMob/css/dist/style.css">
    <script charset="utf-8" src="https://map.qq.com/api/js?v=2.exp&libraries=convertor"></script>
    <script src="https://3gimg.qq.com/lightmap/components/geolocation/geolocation.min.js"></script>
    <script src="../Utility/vPosition.js"></script>
    <script type="text/javascript">
        function restartPosition() {
            var fromsource = getParam("fromSource");
            var mapkey = $("#hdQQMapKey").val();
            var isOpenMultStore = $("#hidIsOpenMultStore").val();
            var hasPosition = $("#hidHasPosition").val();
            if (fromsource != "1" && fromsource != "2" && isOpenMultStore == "1" && hasPosition != "1" && $("#hidIsToPlatform").val().toLowerCase() != "platform") {
                //if (navigator.geolocation) {
                //    navigator.geolocation.getCurrentPosition(getPositionSuccess, ShowError);
                //}
                var geolocation = new qq.maps.Geolocation(mapkey, "myapp");
                if (geolocation) {
                    geolocation.getLocation(getPositionSuccess, ShowError)
                }
                else {
                    if ($("#hidIsToPlatform").val() != "Platform")
                        window.location.href = "NoPositionTip.aspx";
                }
                //临时测试
                //seachNearStore("28.18985,112.99767", "");
                //seachNearStore("30.18985,192.99767", "");
            }
            if (fromsource == "2") {
                $('#noTip2').show().delay(1000).fadeOut();
            }
        }
        function getPositionSuccess(position) {
            //var lat = position.coords.latitude;
            //var lng = position.coords.longitude;
            //qq.maps.convertor.translate(new qq.maps.LatLng(lat, lng), 1, function (res) {
            //    latlng = res[0];
            //    seachNearStore(latlng, "");
            //});
            var lat = position.lat;
            var lng = position.lng;
            seachNearStore(lat + "," + lng, "", false, $("#hidIsToPlatform").val(), true);
        }
    </script>
    <style>
        body { background: #fff; }

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

        /*modal*/
        .modal-open { overflow: hidden; }

        .modal { position: fixed; top: 0; right: 0; bottom: 0; left: 0; z-index: 1050; display: none; overflow: hidden; -webkit-overflow-scrolling: touch; outline: 0; }

            .modal.fade .modal-dialog { -webkit-transition: -webkit-transform .3s ease-out; -o-transition: -o-transform .3s ease-out; transition: transform .3s ease-out; -webkit-transform: translate(0, -25%); -ms-transform: translate(0, -25%); -o-transform: translate(0, -25%); transform: translate(0, -25%); }

            .modal.in .modal-dialog { -webkit-transform: translate(0, 0); -ms-transform: translate(0, 0); -o-transform: translate(0, 0); transform: translate(0, 0); }

        .modal-open .modal { overflow-x: hidden; overflow-y: auto; }

        .modal-dialog { position: relative; width: auto; margin: 15px; margin-top: 5rem; }

        .modal-content { position: relative; background-color: #fff; -webkit-background-clip: padding-box; background-clip: padding-box; border: 1px solid #999; border: 1px solid rgba(0, 0, 0, .2); border-radius: 0; outline: 0; -webkit-box-shadow: 0 3px 9px rgba(0, 0, 0, .5); box-shadow: 0 3px 9px rgba(0, 0, 0, .5); }

        .modal-backdrop { position: fixed; top: 0; right: 0; bottom: 0; left: 0; z-index: 1040; background-color: #000; }

            .modal-backdrop.fade { filter: alpha(opacity=0); opacity: 0; }

            .modal-backdrop.in { filter: alpha(opacity=50); opacity: .5; }

        .modal-header { padding: 0.75rem 0.5rem 0.25rem 0.5rem; font-size: 0.8rem; text-align: center; border: 0; }

            .modal-header .close { margin-top: -0.1rem; display: none; }

        .modal-title { margin: 0; line-height: 1.42857143; }

        .modal-body { padding: 0.75rem 0.5rem; padding-top: 0.2rem; text-align: center; border-bottom: 0px; border-radius: 0; font-size: 0.7rem; border-bottom: 0; }

        .modal-footer { height: 2.2rem; overflow: hidden; display: table; width: 100%; border-collapse: collapse; border-top: 1px solid #ebebeb; }

            .modal-footer .btn + .btn { margin-bottom: 0; margin-left: 0.25rem; }

            .modal-footer .btn-group .btn + .btn { margin-left: -1px; }

            .modal-footer .btn-block + .btn-block { margin-left: 0; }

        .modal-scrollbar-measure { position: absolute; top: -9999px; width: 2.5rem; height: 2.5rem; overflow: scroll; }

        .modal-footer button { width: 50%; float: right; height: 2.2rem; line-height: 2.2rem; border: 0; padding: 0; background: none; color: #666; font-size: 0.7rem; }

            .modal-footer button:first-child { position: absolute; left: 0; }

            .modal-footer button:last-child { border-left: 1px solid #dedede; }
        /*end*/

        /*专题样式*/
        .app_zt .tab { margin-left: 15px; }

            .app_zt .tab a { width: 110px; height: 74px; }

        .diy-actions { height: 700px; }

        .app_1 { background: url(../images/app_1.jpg); }

        .app_2 { background: url(../images/app_2.jpg); }

        .app_3 { background: url(../images/app_3.jpg); }

        .app_4 { background: url(../images/app_4.jpg); }

        .app_5 { background: url(../images/app_5.jpg); }

        .app_8 { background: url(../images/app_8.png); }

        .app_9 { background: url(../images/app_9.png); }

        .app_1:hover { background: url(../images/app_1_1.jpg) !important; }

        .app_2:hover { background: url(../images/app_2_1.jpg) !important; }

        .app_3:hover { background: url(../images/app_3_1.jpg) !important; }

        .app_4:hover { background: url(../images/app_4_1.jpg) !important; }

        .app_5:hover { background: url(../images/app_5_1.jpg) !important; }

        .app_8:hover { background: url(../images/app_8_1.png) !important; }

        .app_9:hover { background: url(../images/app_9_1.png) !important; }

        .diy .members_con { margin: 0 !important; }

        .members_nav1 { margin: 0 !important; padding: 0; }

            .members_nav1 ul li span { width: 100% !important; }

        .members_con .members_nav1 li { width: 100%; float: left; padding: 0px; text-align: center; margin-top: 0px; margin-bottom: 0px; }

        .title_img { width: 320px !important; height: 36px; border-top: 1px solid #e7e5ea; }

        .members_nav1 li.board6 { width: 50%; border-right: 1px solid #e7e5ea; border-top: 1px solid #e7e5ea; }

        .members_nav1 li.big1_img { width: 50%; border-right: 1px solid #e7e5ea; border-top: 1px solid #e7e5ea; }

        .members_nav1 ul li.big_img { width: 40%; border-right: 1px solid #e7e5ea; border-top: 1px solid #e7e5ea; }



        .members_nav1 ul li.mid_img { width: 60%; border-top: 1px solid #e7e5ea; }

        .members_nav1 ul li.mid1_img { width: 39.9%; border-top: 1px solid #e7e5ea; border-right: 1px solid #e7e5ea; }

        .members_nav1 ul li.small_img { width: 30%; border-right: 1px solid #e7e5ea; border-top: 1px solid #e7e5ea; }

        .members_nav1 ul li.small1_img { width: 30%; border-right: 1px solid #e7e5ea; border-top: 1px solid #e7e5ea; }

        .members_nav1 li.small2_img { width: 25%; border-right: 1px solid #e7e5ea; border-top: 1px solid #e7e5ea; }

        .small_img:last-child { border-right: 0; }

        .ad_img { width: 320px !important; height: 52px !important; }

        .members_con .members_nav2 { background: #424242; padding: 0px 10px; color: #e0e0e0; }

            .members_con .members_nav2 ul li { width: 100%; float: none; margin-right: 0; text-align: left; background: #424242; border: none; border-bottom: 1px solid #616161; }

            .members_con .members_nav2 a { color: #e0e0e0; }

        .members_nav2 ul li b { border: #919191 solid 1px; border-width: 1px 1px 0 0; }

        .members_goodspic ul li.mingoods { margin-bottom: 10px; }

        .members_nav1 ul li span img { width: 100%; }
    </style>
</head>
<body onload="restartPosition()">
    <!--提示-->
    <div id="noTip2">
        <div class="black">
            <div class="content">您当前位置周边没有可提供服务的门店，系统已为您推荐到“平台店”</div>
        </div>
    </div>

    <div id="noTip3">
        <div class="black">
            <div class="content">
                <div>您当前位置为【<i id="iAdrCity"></i>】，是否切换位置？</div>
                <div><em><a href="#" class="quxiao" id="aQuit">取消</a></em><em><i><a href="#" id="aTosee">去看看</a></i></em></div>
            </div>
        </div>
    </div>
    <div class="dingwei" id="divReferralInfo" runat="server" visible="false">
        <div class="filter-bg"></div>
        <div class="beijing"></div>
        <div class="plr11 ptb13 storeinfo" id="stores">
            <div class="pic">
                <img class="logo" src="/Storage/master/depot/201706201002486513670.png" runat="server" id="Referral_Logo">
                <div class="intro">
                    <asp:Literal ID="Referral_ShopName" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
    <asp:Literal ID="litImageServerUrl" runat="server"></asp:Literal>
    <div class="membersbox" style="margin: 0 auto;">
        <div style="height: 50px; display: none;" id="topemptydiv" runat="server"></div>
        <input type="hidden" id="Hidden1" runat="server" clientidmode="Static" />
        <Hi:HomePage runat="server" ID="H_Page" ClientType="VShop"></Hi:HomePage>
        <script type="text/javascript" src="/Utility/jquery-1.11.0.min.js"></script>
        <input type="hidden" id="hidPageTitle" runat="server" clientidmode="Static" />
        <input type="hidden" id="hdAppId" runat="server" clientidmode="Static">
        <input type="hidden" id="hdLink" runat="server" clientidmode="Static" />
        <input type="hidden" id="hdQQMapKey" runat="server" clientidmode="Static" />
        <input type="hidden" id="hidIsOpenMultStore" runat="server" clientidmode="Static" />
        <input type="hidden" id="hidIsToPlatform" runat="server" clientidmode="Static" />
        <input type="hidden" id="hidHasPosition" runat="server" clientidmode="Static" />
        <Hi:Common_WapFooter runat="server" />
    </div>
    <script type="text/javascript">
        if (document.getElementById("hidPageTitle").value != "") {
            document.title = document.getElementById("hidPageTitle").value;
        }
    </script>
    <script src="/Admin/Shop/PublicMob/plugins/swipe/swipe.js"></script>
    <script src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script language="javascript" type="text/javascript" src="/Utility/wxShare.js?v=3.0"></script>
    <script type="text/javascript" src="/templates/common/script/main.js"></script>
    <script type="text/javascript" src="/Utility/common.js?v=3.0"></script>
    <%--<script src="/Admin/Shop/PublicMob/js/dist/lib-min.js"></script>
    <script src="/Admin/shop/Public/js/dist/underscore.js"></script>--%>
    <%--<script src="/Admin/Shop/PublicMob/js/dist/main.js"></script>--%>
    <script src="/Utility/jquery.scrollLoading.min.js"></script>
    <script src="/templates/common/script/jquery.slides.min.js"></script>
    <link rel="stylesheet" href="/templates/common/style/fonts/style.css?v=3.2" type="text/css" />
    <link rel="stylesheet" href="/templates/common/style/index.css?v=3.2.1" type="text/css" />
    <link rel="stylesheet" href="/templates/common/style/css.css?v=3.0" type="text/css" />


    <script type="text/javascript" src="/templates/common/script/main.js?v=3.0"></script>
    <script type="text/javascript" src="/Utility/common.js?v=3.0"></script>
    <script>
        $(function () {
            $(".big1_img").height(($("body").width() - 20) / 4);
            $(".board6").height(($("body").width() - 20) / 4);
            $('.j-swipe').each(function (index, el) {
                var me = $(this);
                me.attr('id', 'Swiper' + index);
                var id = me.attr('id');
                // alert(id)
                var elem = document.getElementById(id);
                window.mySwipe = Swipe(elem, {
                    startSlide: 0,
                    auto: 4000,
                    callback: function (m) {
                        $(elem).find('.members_flash_time').children('span').eq(m).addClass('cur').siblings().removeClass('cur')
                    },
                });
            });

            $.each($(".lisw4 a"), function (i, link) {
                link.href = link.href.replace("/wapshop/StoreList", "/vshop/StoreList");
            })

            //图片比例处理
            var imgw = $('body').width();
            $(".members_goodspic ul .b_mingoods a img").width(imgw / 2 - 20);
            $(".members_goodspic ul .b_mingoods a img").height(imgw / 2 - 20);

            $(".members_con").each(function (index, el) {
                if (index <= 6) {
                    $(this).find("img").scrollLoading();
                }
            });

        });

        $(window).scroll(function () {
            setScrollLoading();
        });


        //图片可视区域加载
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
            if ($(".appcouponinfo").text() != "") {
                $("body").css("padding-top", "50px");
                $(".appcouponinfo em").click(function () {
                    $(this).parent().hide();
                    $("body").css("padding-top", "0px");
                });
            }

        })
    </script>

    <script src="/Utility/traffic.js"></script>

    <link rel="stylesheet" href="/templates/common/style/css.css">
    <Hi:Common_WAPGuanZhu runat="server"></Hi:Common_WAPGuanZhu>
    <script src="/Utility/listPageAddToCart.js" type="text/javascript"></script>

</body>
</html>
