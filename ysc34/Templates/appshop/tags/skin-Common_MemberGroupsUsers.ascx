<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>


<span class="<%# Eval("IsFightGroupHead").ToBool()?"leader":""%>" >
    <i></i>
    <img src="<%#String.IsNullOrEmpty(Eval("Picture").ToString())?"/templates/common/images/headerimg.png":Globals.GetImageServerUrl("http://",Eval("Picture").ToString()) %>"></span>

