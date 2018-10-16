<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="goods-box">
    <a href="<%# "GroupBuyProductDetails.aspx?groupbuyId=" + Eval("GroupBuyId") %>" >
        <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl180" Style="position: absolute; left: 0px; top: 0px;" />
        <div class="info">
            <div class="name font-xl">
                <%# Eval("ProductName") %>
            </div>
            <%--<div class="intro_1 fs_22">
                <%# Eval("ShortDescription")%>
            </div>--%>
            <div class="price text-danger">
                ¥<%# Eval("Price", "{0:F2}") %><del class="font-s text-muted">¥<%# Eval("SalePrice", "{0:F2}") %></del><span
                    class="sales text-muted">已团<%# Eval("ProdcutQuantity") == DBNull.Value ? 0 : Eval("ProdcutQuantity")%>件</span>
            </div>
        </div>
    </a>
</div>
