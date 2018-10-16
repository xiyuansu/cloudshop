<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<li name="<%# Eval("RechargeMoney").ToDecimal().F2ToString("f2")%>"><%# Eval("RechargeMoney").ToDecimal().F2ToString("f2") %>元<br />送<%# Eval("GiftMoney").ToDecimal().F2ToString("f2") %>元</li>
