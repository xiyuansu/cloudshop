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
                <h2><a style="color: #575757;font-size: 13px;" href="/User/UserDefault.aspx">个人中心</a></h2>
              <%--  <ul>
                    <li>
                       <a href="/User/UserDefault.aspx">个人中心</a></li>
                    <li>
                      
                </ul>--%>
            </li>
            <li class="lijsbg">
                <div class="dian"></div>         
                <h2>我的交易记录</h2>
                <ul>
                    <li>
                        <a href="/User/UserOrders.aspx">订单管理</a></li>
                    <li>
                        <a href="/User/UserRefundApply.aspx">退款管理</a></li>
                    <li>
                        <a href="/User/UserReturnsApply.aspx">退货管理</a></li>
                    <li>
                       <a href="/User/UserReplaceApply.aspx">换货管理</a></li>
                    <li>
                        <a href="/User/UserPoints.aspx">我的积分</a></li>
                    <li>
                       <a href="/User/MyCoupons.aspx"> 我的优惠券</a></li>
                    <li>
                       <a href="/User/MyWeiXinRedEnvelope.aspx"> 我的代金红包</a></li>
                </ul>
                <div class="lixuxian"></div>
            </li>
            <li class="lijsbg">
                <div class="dian"></div>
                <h2>商品收藏与评论</h2>
                <ul>
                    <li>
                        <a href="/User/Favorites.aspx">收藏夹</a></li>
                    <li>
                       <a href="/User/UserConsultations.aspx">咨询/回复</a></li>
                    <li>
                       <a href="/User/UserProductReviews.aspx"> 我参与的评论</a></li>
                    <li>
                       <a href="/User/UserBatchBuy.aspx">商品批量购买</a></li>
                </ul>
                <div class="lixuxian"></div>
            </li>
            <li class="lijsbg">
                <div class="dian"></div>
                <h2>预付款账户</h2>
                <ul>
                    <li>
                        <a href="/User/MyAccountSummary.aspx">预付款账户</a></li>
                    <li>
                        <a href="/User/RechargeRequest.aspx">预付款充值</a></li>
                    <li>
                        <a href="/User/MyAccount.aspx">账户安全</a></li>
                </ul>
                <div class="lixuxian"></div>
            </li>
            <li class="lijsbg" id="liListReferral" runat="server">
                <div class="dian"></div>
                <h2>分销员</h2>
                <ul>
                    <li id="liReferralRegisterAgreement" runat="server">
                       <a href="/User/ReferralRegisterAgreement.aspx">立即成为分销员</a></li>
                    <li id="liReferralLink" runat="server">
                       <a href="/User/PopularizeGift.aspx">分享赚奖励</a></li>
                    <li id="liReferralSplittin" runat="server">
                       <a href="/User/SplittinDetails.aspx"> 我的奖励</a></li>
                    <li id="liSubReferral" runat="server">
                       <a href="/User/SubReferrals.aspx">我分销的会员</a></li>
                </ul>
            </li>
            <li class="lijsbg">
                <div class="dian"></div>
                <h2>个人设置</h2>
                <ul>
                    <li>
                        <a href="/User/UserProfile.aspx">个人信息</a></li>
                    <li>
                        <a href="/User/UpdatePassword.aspx">修改密码</a></li>
                    <li>
                        <a href="/User/UserShippingAddresses.aspx">我的收货地址</a></li>
                </ul>
            </li>
            <li class="lijsbg">
                <div class="dian"></div>
                <h2>站内消息</h2>
                <ul>
                    <li>
                        <a href="/User/UserReceivedMessages.aspx">收件箱(<em><asp:Literal runat="server" ID="messageNum"></asp:Literal></em>)</a></li>
                    <li>
                        <a href="/User/UserSendedMessages.aspx"> 发件箱</a></li>
                    <li>
                        <a href="/User/UserSendMessage.aspx"> 给管理员发消息</a></li>
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

