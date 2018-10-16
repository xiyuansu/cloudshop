<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a href="<%# "ProductDetails.aspx?ProductId=" + Eval("ProductId") %> ">
    <div class="well goods-box">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl100" />
        <div class="info">
            <div class="name">
                <%# Eval("ProductName") %></div>
            <div class="price text-danger">
                ¥<%# Eval("SalePrice", "{0:F2}") %><span class="sales text-muted">已售<%#Eval("ShowSaleCounts")%>件</span></div>
        </div>
    </div>
</a>