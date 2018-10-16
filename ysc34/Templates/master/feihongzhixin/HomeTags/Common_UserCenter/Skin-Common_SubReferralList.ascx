<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<tr>
    <td style=" padding-left: 15px;"><%# Eval("UserName")%></td>
    <td><%# ((DateTime)Eval("CreateDate")).ToString("yyyy-MM-dd hh:mm:ss") %></td>
    <td>￥<%#((Decimal)Eval("SubSumOrderTotal")).ToString("f2")%></td>
    <td><%# Eval("SubSumOrderNumber") %></td>
    <td><%#((Decimal)Eval("SubMemberAllSplittin")).ToString("f2")%></td>
</tr>