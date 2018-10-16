<%@ Control Language="C#"%>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>

<tr>
<td><%#Eval("Username") %></td>
<td><%#Eval("Quantity") %></td>
<td><%# Eval("SKUContent") %>&nbsp;</td>
    <td><%#Eval("PayDate")%></td>
</tr>
