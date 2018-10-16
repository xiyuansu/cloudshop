<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<li <%# Request.QueryString["CategoryId"].ToInt()==Eval("CategoryId").ToInt()?"class=\"current\"":"" %>>
    <a href='Articles?CategoryId=<%# Eval("CategoryId") %>'>
        <%# Eval("Name") %>
    </a>
</li>

