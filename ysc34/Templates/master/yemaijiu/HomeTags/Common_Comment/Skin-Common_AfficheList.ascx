<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
	<li><a href='<%# GetRouteUrl("AffichesDetails", new {afficheId =Eval("AfficheId")})%>' target="_blank" Title='<%#Eval("Title") %>'>
<Hi:SubStringLabel ID="SubStringLabel" Field="Title" StrLength="14" runat="server"  /></a></li>
