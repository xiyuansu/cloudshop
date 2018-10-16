<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>

<div class="hyzxconterl">
    <div class="info"></div>
    <div class="hyzxconterl1">
        <ul>
            <li class="lijsbg">
                <div class="dian"></div>
                <h2>我的交易记录</h2>
                <ul>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl4" UrlName="user_UserOrders" runat="server">订单管理</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl5" UrlName="user_UserRefundApply" runat="server">退款申请单</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl6" UrlName="user_UserReturnsApply" runat="server">退货申请单</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl7" UrlName="user_UserReplaceApply" runat="server">换货申请单</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl1" UrlName="user_UserPoints" runat="server">我的积分</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl2" UrlName="user_MyCoupons" runat="server">我的优惠券</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl21" UrlName="user_MyCoupons_WeiXinRedEnvelope" runat="server">我的代金红包</Hi:SiteUrl></li>
                </ul>
                <div class="lixuxian"></div>
            </li>
            <li class="lijsbg">
                <div class="dian"></div>
                <h2>商品收藏与评论</h2>
                <ul>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl8" UrlName="user_Favorites" runat="server">收藏夹</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl9" UrlName="user_UserConsultations" runat="server">咨询/回复</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl10" UrlName="user_UserProductReviews" runat="server">我参与的评论</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl11" UrlName="user_UserBatchBuy" runat="server">商品批量购买</Hi:SiteUrl></li>
                </ul>
                <div class="lixuxian"></div>
            </li>
            <li class="lijsbg">
                <div class="dian"></div>
                <h2>预付款账户</h2>
                <ul>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl12" UrlName="user_MyAccountSummary" runat="server">预付款账户</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl13" UrlName="user_RechargeRequest" runat="server">预付款充值</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl14" UrlName="user_Myaccount" runat="server">账户安全</Hi:SiteUrl></li>
                </ul>
                <div class="lixuxian"></div>
            </li>
            <li class="lijsbg">
                <div class="dian"></div>
                <h2>推广员</h2>
                <ul>
                    <li id="liReferralRegisterAgreement" runat="server">
                        <Hi:SiteUrl ID="SiteUrl3" UrlName="user_ReferralRegisterAgreement" runat="server">立即成为推广员</Hi:SiteUrl></li>
                    <li id="liReferralLink" runat="server">
                        <Hi:SiteUrl UrlName="user_PopularizeGift" runat="server">分享赚奖励</Hi:SiteUrl></li>
                    <li id="liReferralSplittin" runat="server">
                        <Hi:SiteUrl ID="url" UrlName="user_SplittinDetails" runat="server">我的奖励</Hi:SiteUrl></li>
                    <li id="liSubReferral" runat="server">
                        <Hi:SiteUrl ID="SiteUrl22" UrlName="user_SubReferrals" runat="server">我推广的会员</Hi:SiteUrl></li>
                </ul>
            </li>
            <li class="lijsbg">
                <div class="dian"></div>
                <h2>个人设置</h2>
                <ul>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl15" UrlName="user_UserProfile" runat="server">个人信息</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl16" UrlName="user_UpdatePassword" runat="server">修改密码</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl17" UrlName="user_UserShippingAddresses" runat="server">我的收货地址</Hi:SiteUrl></li>
                </ul>
            </li>
            <li class="lijsbg">
                <div class="dian"></div>
                <h2>站内消息</h2>
                <ul>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl18" UrlName="user_UserReceivedMessages" runat="server">收件箱(<em><asp:Literal runat="server" ID="messageNum"></asp:Literal></em>)</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl19" UrlName="user_UserSendedMessages" runat="server">发件箱</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl20" UrlName="user_UserSendMessage" runat="server">给管理员发消息</Hi:SiteUrl></li>
                </ul>
            </li>
        </ul>

    </div>
    <div class="hyzxconterl2"><a href="/logout.aspx">安全退出</a></div>
</div>
<input type="hidden" runat="server" clientidmode="Static" id="hidAction" />
<!--<script>
$(function(){
	$('.hover_action').click(function(){
		$(this).parents('li').addClass('action');
		})
})
</script>-->

<script language="javascript" type="text/javascript">
    $(document).ready(function (e) {
        if (!IsOpenReferral) {
            $(".lijsbg3").hide();
        }

        var action = $("#hidAction").val();
        $(".hyzxconterl a").each(function () {
            var href = $(this).attr("href");
            if (href.indexOf(action) >= 0) {
                $(this).parent().addClass("hover_action").siblings().removeClass("hover_action");
                $(this).parents('.lijsbg').addClass("action").siblings().removeClass("action");
            }
        });
        if(action=="OpenBalance")
        {
            var returnUrl = GetQueryString("ReturnUrl");
            var url = returnUrl.split("aspx")[0].split("/");
            var theUrl = url[url.length - 1];
            $(".hyzxconterl a").each(function () {
                var href = $(this).attr("href");
                if (href.indexOf(theUrl) >= 0) {
                    $(this).parent().addClass("hover_action").siblings().removeClass("hover_action");
                    $(this).parents('.lijsbg').addClass("action").siblings().removeClass("action");
                }
            });
        }
    });

    function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
</script>

