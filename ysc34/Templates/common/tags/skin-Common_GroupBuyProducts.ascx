<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<div class="goods-box">
    <a href="<%# "GroupBuyProductDetails.aspx?groupbuyId=" + Eval("GroupBuyId") %>" >
        <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl180" Style="position: absolute; left: 0; top: 0;" />
        <div class="info">
            <div class="name">
                <%# Eval("ProductName") %>
            </div>
           <%-- <div class="intro_1">
                <%# Eval("ShortDescription")%>
            </div>--%>
            <div class="price text-danger">
                ¥<%# Eval("Price", "{0:F2}") %><del class="text-muted">¥<%# Eval("SalePrice", "{0:F2}") %></del><span
                    class="sales text-muted">已团<%# Eval("ProdcutQuantity") == DBNull.Value ? 0 : Eval("ProdcutQuantity")%>件</span>
            </div>
        </div>
    </a>
</div>

