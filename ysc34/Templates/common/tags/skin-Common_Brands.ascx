<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a href="BrandDetail.aspx?BrandId=<%# Eval("BrandId")%>">
    <div class="well">
        <Hi:HiImage ID="imgLogo" runat="server" CssClass="img-responsive" DataField="Logo" />
        <div class="name">
            <%# Eval("BrandName")%></div>
    </div>
</a>