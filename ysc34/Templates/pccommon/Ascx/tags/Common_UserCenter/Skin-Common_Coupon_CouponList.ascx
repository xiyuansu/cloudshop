<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<tr class="ddgl_1">
    <td align="center">
        <%# Eval("CouponName") %>
    <td align="center">
        <%# Eval("Price").ToDecimal().F2ToString("f2")%>
    </td>
    <td align="center">
        <%# (Eval("StartTime").ToDateTime().HasValue ? Eval("StartTime").ToDateTime().Value.ToString("yyyy.MM.dd") : "") + "至" + (Eval("ClosingTime").ToDateTime().HasValue ? Eval("ClosingTime").ToDateTime().Value.ToString("yyyy.MM.dd") : "") %></td>
    <td align="center">
        <%# Eval("OrderUseLimit").ToDecimal() == 0 ? "无限制" : "订单满" + Eval("OrderUseLimit").ToDecimal().F2ToString("f2") + "元使用" %></td>
    <td align="center">
        <%# string.IsNullOrEmpty(Eval("CanUseProducts").ToNullString().Trim()) ? "全场通用" : "部分商品可用" %>
    </td>
    <%# (!Eval("UsedTime").ToDateTime().HasValue && Eval("ClosingTime").ToDateTime().Value > DateTime.Now) ? (Eval("StartTime").ToDateTime().Value>DateTime.Now) ? "<td></td>" : string.IsNullOrEmpty(Eval("CanUseProducts").ToNullString().Trim()) ? "<td align=\"center\"><a href=\"/\">去使用</a></td>" : "<td align=\"center\"><a href=\"/SubCategory.aspx?CanUseProducts=" + Eval("CanUseProducts").ToNullString() + "&keywords=\">去使用</a></td>":"" %>
       
</tr>
