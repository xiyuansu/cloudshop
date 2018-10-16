<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<a href='Articles.aspx?CategoryId=<%# Eval("CategoryId") %>' class="list-group-item ">
    <%# Eval("Name") %>
</a>

