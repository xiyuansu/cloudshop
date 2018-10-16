<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<a href='<%# string.IsNullOrEmpty(Eval("LoctionUrl").ToString())?"javascript:;":Eval("LoctionUrl")%>'>
    <Hi:HiImage runat="server" DataField="ImageUrl" />
</a>
