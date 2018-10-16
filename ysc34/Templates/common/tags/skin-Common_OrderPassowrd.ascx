<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="order-password">
    <input name="chkorderpassword" type="checkbox" checked="checked" value="<%# Eval("Password") %>" />
    <div class="pw_desc">
        <span class="password-num">密码<%# Eval("Index") %></span>
        <span class="password-txt"><%# Eval("Password") %></span>
    </div>
</div>
