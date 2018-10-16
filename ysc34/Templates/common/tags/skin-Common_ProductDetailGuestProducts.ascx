<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
    <a href="ProductDetails.aspx?productId=<%# Eval("ProductId") %>">
    <img data-url="<%#Eval("ProdImg").ToNullString()%>" />
        </a>
    <a class="guest_list_title" href="ProductDetails.aspx?productId=<%# Eval("ProductId") %>">
        <%#Eval("ProductName") %>
    </a>
    <span class="guest_list_price">￥<Hi:FormatedMoneyLabel Money='<%# Eval("SalePrice") %>' runat="server" /></span>
</li>
