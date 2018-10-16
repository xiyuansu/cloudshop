<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<a href="#" onclick="javascript:location.href='./Favorites.aspx?tags='+escape('<%#Eval("TagName") %>')"><%#Eval("TagName") %>(<%#Eval("ProductNum") %>)</a>  