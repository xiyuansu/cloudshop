<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeBehind="PromotionAggregation.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.PromotionAggregation" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">
        function ShowSecondMenuLeft(secondurl, ItemName) {
            window.parent.ShowMenuLeft("营销", secondurl, null, ItemName);
        }

        $(document).ready(function (e) {
            $.ajax({
                url: "/API/AuthValid.ashx",
                type: 'post',
                dataType: 'json',
                timeout: 10000,
                data: {
                    action: "all"
                },
                async: false,
                success: function (resultData) {
                    var ServerStatus = resultData.status;
                    if (ServerStatus != "") {
                        var OpenTaobao = 0,
                            OpenMobbile = 0,
                            OpenAliho = 0,
                            OpenWap = 0,
                            OpenVstore = 0,
                            OpenReferral = 0,
                            OpenMultiStore = 0;

                        var StatusArr = ServerStatus.split(',');

                        OpenTaobao = parseInt(StatusArr[0]);
                        if (StatusArr.length >= 2) OpenVstore = parseInt(StatusArr[1]);
                        if (StatusArr.length >= 3) OpenMobbile = parseInt(StatusArr[2]);;
                        if (StatusArr.length >= 4) OpenWap = parseInt(StatusArr[3]);
                        if (StatusArr.length >= 5) OpenAliho = parseInt(StatusArr[4]);
                        if (StatusArr.length >= 6) OpenReferral = parseInt(StatusArr[5]);
                        if (StatusArr.length >= 7) OpenMultiStore = parseInt(StatusArr[6]);
                        if (StatusArr.length >= 9) OpenApplet = parseInt(StatusArr[8]);
                        if (StatusArr.length >= 10) OpenPcShop = parseInt(StatusArr[9]);

                        if (OpenAliho == 0 && OpenWap == 0 && OpenVstore == 0 && OpenMobbile == 0 && OpenPcShop == 0) {
                            $("#lirechargesend").hide();
                            $("#licombinationbuy").hide();
                            $("#ligroupbuy").hide();
                            $("#lipresale").hide();
                        }
                        if (OpenMobbile == 0) {
                            $("#app_apppromotecoupons").hide();
                        }
                        if (OpenVstore == 0 && OpenMobbile == 0) {
                            $("#v_divfdtitil").hide();
                            $("#v_fd").hide();
                            $("#v_bm").hide();
                            if (OpenWap == 0 && OpenAliho == 0)
                                $("#m_zxj").hide();
                        }
                        else {
                            if (OpenVstore == 0) {
                                $("#v_bm").hide();
                                $("#v_dzp").hide();
                                $("#v_zjd").hide();
                                $("#v_ggk").hide();
                                $("#v_cj").hide();
                                $("#v_djhb").hide();
                                $("#v_vote").hide();
                            }
                            if (OpenMobbile == 0) {
                                $("#app_yyy").hide();
                            }
                        }

                    }
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="hd_1">常用促销</div>
        <ul class="list_1">
            <li>
                <a href="javascript:ShowSecondMenuLeft('promotion/productpromotions.aspx','有买有送')">
                    <font>送</font>
                    <strong>有买有送</strong>
                    <span>设置购物送礼和买几送几</span>
                </a>
            </li>
            <li>
                <a href="javascript:ShowSecondMenuLeft('promotion/orderpromotions.aspx','满额优惠')">
                    <font>满</font>
                    <strong>满额优惠</strong>
                    <span>设置订单满指定金额享受优惠</span>
                </a>
            </li>
            <li>
                <a href="javascript:ShowSecondMenuLeft('promotion/countdowns.aspx','限时抢购')">
                    <font>抢</font>
                    <strong>限时抢购</strong>
                    <span>设置商品限时打折促销</span>
                </a>
            </li>
            <li id="licombinationbuy">
                <a href="javascript:ShowSecondMenuLeft('promotion/CombinationBuy.aspx','组合购')">
                    <font>组</font>
                    <strong>组合购</strong>
                    <span>设置多件商品搭配销售</span>
                </a>
            </li>
            <li id="ligroupbuy">
                <a href="javascript:ShowSecondMenuLeft('promotion/groupbuys.aspx','经典团购')">
                    <font>团</font>
                    <strong>经典团购</strong>
                    <span>设置团体购买商品优惠</span>
                </a>
            </li>

            <li>
                <a href="javascript:ShowSecondMenuLeft('promotion/orderpromotions.aspx?isWholesale=true','混合批发')">
                    <font>混</font>
                    <strong>混合批发</strong>
                    <span>设置混合商品批发优惠方案</span>
                </a>
            </li>
            <li>
                <a href="javascript:ShowSecondMenuLeft('promotion/productpromotions.aspx?isWholesale=true','单品批发')">
                    <font>单</font>
                    <strong>单品批发</strong>
                    <span>设置单件商品批发优惠方案</span>
                </a>
            </li>
            <li>
                <a href="javascript:ShowSecondMenuLeft('promotion/newcoupons.aspx','优惠券')">
                    <font>券</font>
                    <strong>优惠券</strong>
                    <span>向客户发放店铺优惠券</span>
                </a>
            </li>

            <li id="m_zxj">
                <a href="javascript:ShowSecondMenuLeft('promotion/productpromotions.aspx?IsMobileExclusive=true','手机专享价')">
                    <font>享</font>
                    <strong>手机专享价</strong>
                    <span>设置移动端专享价格</span>
                </a>
            </li>
            <li id="lipresale">
                <a href="javascript:ShowSecondMenuLeft('promotion/ProductPreSale.aspx','预售')">
                    <font>预</font>
                    <strong>预售</strong>
                    <span>设置产品预售</span>
                </a>
            </li>
        </ul>

        <div class="hd_1" id="v_divfdtitil">互动营销</div>
        <ul class="list_1 bg_1" id="v_fd">
            <li id="v_dzp">
                <a href="javascript:ShowSecondMenuLeft('vshop/NewLotteryActivity.aspx?type=1','幸运大转盘')">
                    <font>转</font>
                    <strong>幸运大转盘</strong>
                    <span>通过滚动转盘进行抽奖</span>
                </a>
            </li>
            <li id="v_ggk">
                <a href="javascript:ShowSecondMenuLeft('vshop/NewLotteryActivity.aspx?type=2','刮刮卡')">
                    <font>刮</font>
                    <strong>刮刮卡</strong>
                    <span>通过刮开卡片进行抽奖</span>
                </a>
            </li>
            <li id="v_zjd">
                <a href="javascript:ShowSecondMenuLeft('vshop/NewLotteryActivity.aspx?type=3','砸金蛋')">
                    <font>砸</font>
                    <strong>砸金蛋</strong>
                    <span>通过砸开金蛋进行抽奖</span>
                </a>
            </li>
            <li id="v_fpt">
                <a href="javascript:ShowSecondMenuLeft('promotion/FightGroupActivitiyList.aspx','火拼团')">
                    <font>团</font>
                    <strong>火拼团</strong>
                    <span>引导好友一起购买</span>
                    <div class="icon_1"><i class="icon_wechat"></i><i class="icon_app"></i></div>
                </a>
            </li>
            <li id="v_cj">
                <a href="javascript:ShowSecondMenuLeft('vshop/ManageLotteryTicket.aspx','微抽奖')">
                    <font>抽</font>
                    <strong>微抽奖</strong>
                    <span>用户先报名后开奖</span>
                    <div class="icon_1"><i class="icon_wechat"></i></div>
                </a>
            </li>
            <li id="v_djhb">
                <a href="javascript:ShowSecondMenuLeft('vshop/ManageRedEnvelope.aspx','代金红包')">
                    <font>红</font>
                    <strong>代金红包</strong>
                    <span>用户下单后的裂变红包</span>
                    <div class="icon_1"><i class="icon_wechat"></i></div>
                </a>
            </li>
            <li id="app_yyy">
                <a href="javascript:ShowSecondMenuLeft('App/LotteryDrawSet.aspx','摇一摇抽奖')">
                    <font>摇</font>
                    <strong>摇一摇抽奖</strong>
                    <span>通过摇一摇手机进行抽奖</span>
                    <div class="icon_1"><i class="icon_app"></i></div>
                </a>
            </li>
        </ul>

        <div class="hd_1">运营工具</div>
        <ul class="list_1 bg_2">
            <li id="v_bm">
                <a href="javascript:ShowSecondMenuLeft('vshop/ManageActivity.aspx','微报名')">
                    <font>报</font>
                    <strong>微报名</strong>
                    <span>活动报名设置及管理</span>
                    <div class="icon_1"><i class="icon_wechat"></i></div>
                </a>
            </li>
            <li id="v_vote">
                <a href="javascript:ShowSecondMenuLeft('store/votes.aspx','微投票')">
                    <font>投</font>
                    <strong>微投票</strong>
                    <span>报名调查设置及管理</span>
                    <div class="icon_1"><i class="icon_wechat"></i></div>
                </a>
            </li>
            <li>
                <a href="javascript:ShowSecondMenuLeft('promotion/gifts.aspx','礼品中心')">
                    <font>礼</font>
                    <strong>礼品中心</strong>
                    <span>添加和管理商城礼品</span>
                </a>
            </li>
            <li>
                <a href="javascript:ShowSecondMenuLeft('promotion/giftcouponsconfig.aspx','注册送优惠券')">
                    <font>券</font>
                    <strong>注册送优惠券</strong>
                    <span>用户注册时获得优惠券</span>
                </a>
            </li>
            <li id="app_apppromotecoupons">
                <a href="javascript:ShowSecondMenuLeft('promotion/AppPromoteCoupons.aspx','APP推广红包')">
                    <font>推</font>
                    <strong>APP推广红包</strong>
                    <span>用户下载APP并且第一次登录时获得优惠券</span>
                </a>
            </li>
            <li id="lirechargesend">
                <a href="javascript:ShowSecondMenuLeft('promotion/RechargeGift.aspx','充值赠送')">
                    <font>充</font>
                    <strong>充值赠送</strong>
                    <span>用户充值后额外赠送指定金额</span>
                </a>
            </li>
        </ul>
    </div>

</asp:Content>

