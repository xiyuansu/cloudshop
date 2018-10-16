<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<li>
    <span class="coupons_price"><i>￥</i><%# Eval("Price") %></span>
    <div class="coupons_tips">
        <div class="coupons_tips_left">
            <span><i class="icon_tips  icon-icon_warning"></i>&nbsp;订单满<%# Eval("OrderUseLimit") %>元使用（不含运费）</span>
            <span><i class="icon_clock icon-icon_time"></i>&nbsp;<%# DateTime.Parse(Eval("StartTime").ToString()).ToString("yyyy.MM.dd") %>-<%# DateTime.Parse(Eval("ClosingTime").ToString()).ToString("yyyy.MM.dd") %></span>
        </div>
        <a class="btn_receive" href="javascript:onGetCouponClick(<%# Eval("CouponId") %>);">领取</a>
    </div>
</li>
