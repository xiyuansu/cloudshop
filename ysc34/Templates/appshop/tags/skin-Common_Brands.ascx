<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a href="BrandDetail.aspx?BrandId=<%# Eval("BrandId")%>">
    <div class="well">
        <Hi:HiImage DataField="Logo" class="img-responsive" runat="server" />      
        <div class="name">
            <%# Eval("BrandName")%></div>
    </div>
</a>