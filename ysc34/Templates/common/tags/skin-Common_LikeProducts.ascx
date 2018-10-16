<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
    <div>
        <a href="<%# "ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
            <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl180" CustomToolTip="ProductName" />
        </a>
    </div>
</li>

