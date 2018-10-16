<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<a href="<%#Eval("LoctionUrl")%>">
    <i class="icon" name="icon">
        <Hi:HiImage runat="server" Width="35px" Height="35px;" />
    </i>
    <span>
        <%#Eval("ShortDesc")%>
    </span>
</a>

