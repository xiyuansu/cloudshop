<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>

<div>
<span class="<%# Eval("IsFightGroupHead").ToBool()?"leader":""%>">
    <i></i>
    <img style='<%#Eval("UserId").ToInt()>0?"":"display:none;"%>' src="<%#String.IsNullOrEmpty(Eval("Picture").ToNullString())?"/templates/common/images/headerimg.png":Globals.GetImageServerUrl("http://",Eval("Picture").ToNullString()) %>">
</span>
</div>
