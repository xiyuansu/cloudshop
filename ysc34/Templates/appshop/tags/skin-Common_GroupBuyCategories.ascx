<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<span><a href='<%#  "/AppShop/GroupBuyList.aspx?categoryId=" + Eval("CategoryId") %> '><%# Eval("Name") %></a></span>
