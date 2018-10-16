<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li><span><a href="<%# Eval("RedirectTo")+"?StoreId="+Eval("StoreId")%>" class="p-img"><img src="<%# Eval("IconUrl") %>"></a></span><a href="<%# Eval("RedirectTo")+"?StoreId="+Eval("StoreId")%>"><%# Eval("MarktingTypeText") %></a></li>
