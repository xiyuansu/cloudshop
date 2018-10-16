<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li class="mingoods">
    <div class="b_mingoods_wrapper">
        <a href="ProductDetails.aspx?productId=<%# Eval("ProductId") %>  ">
            <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl180" CustomToolTip="ProductName" />
        </a>
        <div class="text-ellipsis"><%# Eval("ProductName") %></div>        
        <span class="replace">
            ￥<Hi:FormatedMoneyLabel Money='<%# Eval("SalePrice") %>' runat="server" />
        </span>
    </div>
</li>
