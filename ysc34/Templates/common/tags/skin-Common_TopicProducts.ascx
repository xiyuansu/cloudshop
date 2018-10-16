<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a href="<%# "ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
    <div class="zt">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl100" />
     <div class="info">
            <div class="name">
                <%# Eval("ProductName") %>
            </div>
            <div class="intro font-m text-muted">
                <%# Eval("ShortDescription")%>
            </div>
            <div class="price text-danger" style="float: left; margin-top: 0.5rem; width:100%;padding:0;">
                ¥<%# Eval("SalePrice", "{0:F2}") %><span class="sales font-s text-muted" style="padding:0;margin:0;float:right">已售<%# Eval("ShowSaleCounts")%>件</span>
            </div>
        </div>
    </div>
</a>