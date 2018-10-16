<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Context" %>
<a href='/AppShop/Articles.aspx?CategoryId=<%# Eval("CategoryId") %>' class="list-group-item ">
    <%# Eval("Name") %>
    <span class="glyphicon glyphicon-eye-open"></span></a>

<%--   <div class="well goods-box">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl100" />
        <div class="info">
            <div class="name font-xl">
                <%# Eval("ProductName") %></div>
            <div class="intro font-m text-muted">
                <%# Eval("ShortDescription") %></div>
            <div class="price text-danger">
                ¥<%# Eval("SalePrice", "{0:F2}") %><span class="sales font-s text-muted">已售<%#Eval("ShowSaleCounts")%>件</span></div>
        </div>
    </div>--%>