<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<a name="<%# Eval("HasChildren") %>" value="<%# Eval("CategoryId") %>" class="list-group-item categoryItem">
    <%# Eval("Name") %>
    <span class="glyphicon glyphicon-eye-open" onclick="goUrl('ProductList.aspx?categoryId=<%# Eval("CategoryId") %>');event.cancelBubble = true;"></span></a>
