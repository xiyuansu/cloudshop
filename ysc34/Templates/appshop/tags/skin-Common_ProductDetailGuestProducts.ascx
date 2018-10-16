<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
	<a class="AppProductDetail" data-pid="<%# Eval("ProductId") %>">
    <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl180" CustomToolTip="ProductName" />
    </a>
    <a class="AppProductDetail guest_list_title" data-pid="<%# Eval("ProductId") %>">
        <%#Eval("ProductName") %>
    </a>
    <span class="guest_list_price">￥<Hi:FormatedMoneyLabel Money='<%# Eval("SalePrice") %>' runat="server" /></span>
</li>
