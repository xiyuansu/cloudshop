<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<tr>
    <td style=" padding-left: 15px;"><%# Eval("ShowUserName")%></td>
    <td><%# ((DateTime)Eval("CreateDate")).ToString("yyyy-MM-dd HH:mm:ss") %></td>
    <td>￥<%#((Decimal)Eval("SubSumOrderTotal")).F2ToString("f2")%></td>
    <td><%# Eval("SubSumOrderNumber") %></td>
    <td><%#((Decimal)Eval("SubMemberAllSplittin")).F2ToString("f2")%></td>
</tr>