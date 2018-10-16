<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<h4><a href='<%# GetRouteUrl("Helps", new {CategoryId =Eval("CategoryId")})%>'><%#Eval("Name")%></a></h4>