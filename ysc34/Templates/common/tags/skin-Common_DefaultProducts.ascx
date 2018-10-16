<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li class="col-xs-6">
    <div class="padding">
        <div class="inner">
            <div class="img">
                <a href="<%# "ProductDetails.aspx?ProductId=" + Eval("ProductId") %>"><Hi:ListImage runat="server" DataField="ThumbnailUrl310" /></a>
            </div>
            <div class="shop-info">
                <div class="info-name">
                    <a href="<%# "ProductDetails.aspx?ProductId=" + Eval("ProductId") %>" title="<%# Eval("ProductName") %>"><%# Eval("ProductName") %></a>
                </div>
                <div class="info-price">
                    <i>¥</i>
                    <%# Eval("SalePrice", "{0:F2}") %>
                </div>
            </div>
        </div>
    </div>
</li>