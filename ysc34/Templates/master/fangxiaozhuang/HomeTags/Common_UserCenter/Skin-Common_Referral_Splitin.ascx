<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<tr>
    <td style=" padding-left: 15px;"><%# Eval("OrderId") %></td>
    <td><%# Eval("FromUserName") %></td>
    <td>￥<%# Eval("TradeType").ToInt() == (int)Hidistro.Entities.Members.SplittingTypes.DrawRequest ? "-" + Eval("Expenses", "{0:F2}") : Eval("Income", "{0:F2}") %></td>
    <td><Hi:SplittingTypeNameLabel ID="lblTradeType" runat="server" SplittingType='<%# Eval("TradeType") %>'></Hi:SplittingTypeNameLabel></td>
    <td><%# ((bool)Eval("IsUse")) ? "已结算" : "未结算" %></td>
    <td>
       <%# ((DateTime)Eval("TradeDate")).ToString("yyyy-MM-dd HH:mm:ss") %>
    </td>
</tr>
