<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<li>
    <div class="related_pic">
        <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server" ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
            <img data-url="<%#(Eval("ThumbnailUrl160"))%>" />
        </Hi:ProductDetailsLink>
    </div>
    <div class="related_name">
         <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ProductName") %>'
            ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink>
    </div>
    <div class="related_price">
        <strong>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2"  Money='<%# Eval("SalePrice") %>' runat="server" /></strong>
    </div>
</li>
