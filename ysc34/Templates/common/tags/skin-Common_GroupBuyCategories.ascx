<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<span><a href='<%# "GroupBuyList.aspx?categoryId=" + Eval("CategoryId") %> '><%# Eval("Name") %></a></span>