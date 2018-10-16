<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>

<li>
    <div class="user_img">
        <img src="<%#String.IsNullOrEmpty(Eval("Picture").ToNullString())?"/templates/common/images/headerimg.png":Globals.GetImageServerUrl("http://",Eval("Picture").ToNullString()) %>">
        <i class="icon_xx" style="display: <%# Eval("IsFightGroupHead").ToBool()?"block":"none" %>"></i>
    </div>
    <span><%# Eval("Name").ToString() %></span>
</li>
