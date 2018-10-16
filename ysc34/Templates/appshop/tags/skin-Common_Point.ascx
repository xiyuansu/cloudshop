<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<li>
    <span><%# Eval("TradeTypeName") %></span>
    <i><%# Eval("TradeDate") %></i>
    <b><%# Eval("Increased").ToString() == "0" ? "-" + Eval("Reduced"):"+" + Eval("Increased") %></b>
</li>