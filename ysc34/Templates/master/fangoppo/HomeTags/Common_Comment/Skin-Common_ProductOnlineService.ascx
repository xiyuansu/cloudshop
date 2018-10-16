<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>


<div class="list">
    <Hi:OnlineServiceShow Account='<%# Eval("Account") %>' ImageType='<%# Eval("ImageType") %>' NickName='<%# Eval("NickName") %>' ServiceType='<%# Eval("ServiceType") %>' runat="server"></Hi:OnlineServiceShow>
    <%# Eval("NickName") %>

</div>
