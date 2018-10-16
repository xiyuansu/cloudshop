<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li><span><a href="StoreListQuery?TagId=<%# Eval("TagId") %>" class="p-img">
<img alt="<%# Eval("TagName") %>" style="width:100%;height:auto;" src="<%# Eval("TagImgSrc") %>"></a></span><a href="StoreListQuery?TagId=<%# Eval("TagId") %>"><%#Eval("TagName") %></a></li>