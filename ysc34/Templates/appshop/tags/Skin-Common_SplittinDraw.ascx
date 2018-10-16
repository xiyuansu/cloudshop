<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
    <span>提现</span>
    <br>
    <em><%# Eval("AccountDate") %></em>
    <div>+<%#Eval("Amount", "{0:F2}") %></div>
</li>