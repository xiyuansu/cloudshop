<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<li>
    <span class="coupons_price" ><i>￥</i><%# Eval("Price") %></span>
    <input type="hidden" value="<%# Eval("ClaimCode") %>" class="claimcode" />
    <input type="hidden" value="<%# Eval("CouponName") %>" class="couponname" />
    <input type="hidden" value="<%# Eval("Price") %>" class="disprice" />

    <div class="coupons_tips">
        <div class="coupons_tips_left" style="padding-left:20px;">
            <span><i class="icon_tips"></i>&nbsp;<%# Eval("CouponName") %></span>
            <span><i class="icon_clock"></i>&nbsp;<%# Eval("StartTime").ToDateTime().Value.ToString("yyyy.MM.dd") %>-<%# Eval("ClosingTime").ToDateTime().Value.ToString("yyyy.MM.dd") %></span>
        </div>
        <a class="btn_receive" style="position:relative;"  href="javascript:;" ClaimCode="<%# Eval("ClaimCode") %>" >选取</a>
    </div>
</li>