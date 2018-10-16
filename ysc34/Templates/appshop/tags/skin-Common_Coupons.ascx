<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li><span class="coupons_price"><i>￥</i><%# Eval("Price", "{0:f2}")%><em><%# string.IsNullOrEmpty(Eval("CanUseProducts").ToString())?"全场商品可用":"部分商品可用" %></em></span><div class="coupons_tips">
    <div class="coupons_tips_left"><span><i class="icon_tips"></i>&nbsp;<%# decimal.Parse(Eval("OrderUseLimit").ToString())>0?"订单金额满"+Eval("OrderUseLimit","{0:f2}")+"元可用":"订单无限制" %> </span><span><i class="icon_clock"></i>&nbsp; <%# Convert.ToDateTime(Eval("StartTime")).ToString("yyyy.MM.dd") %>-<%# Convert.ToDateTime(Eval("ClosingTime")).ToString("yyyy.MM.dd") %></span></div>
    <%# (string.IsNullOrEmpty(Eval("UsedTime").ToString()) && DateTime.Parse(Eval("StartTime").ToString())<=DateTime.Parse(DateTime.Now.ToString("D")) && DateTime.Parse(Eval("ClosingTime").ToString())>DateTime.Now)?"<a class='btn_receive' href='javascript:goToUseCoupon(\""+Eval("CanUseProducts")+"\","+Eval("CouponId")+")'>去使用</a>":"" %>
    </div>
</li>

<script type="text/javascript">
    function goToUseCoupon(productIds, id) {
        var type = GetAgentType();
        if (productIds == "") {
            if (type == 2) {
                window.HiCmd.webGoHome(0)
            } else if (type == 1) {
                loadIframeURL("hishop://webGoHome/null/0");
            }
        } else {
            // 设置标题
            if (type == 2)
                window.HiCmd.webFindProductByCouponId(id);
            else if (type == 1)
                loadIframeURL("hishop://webFindProductByCouponId/null/" + id);
        }
    }
</script>
