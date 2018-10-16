<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<li>
    <a href='<%# "ProductList.aspx?categoryId=" + Eval("CategoryId") %> '>
        
        <%# Eval("Name") %>
    </a>
</li>