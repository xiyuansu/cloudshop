<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core.Urls" %>
<li><a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%> </a></li>
