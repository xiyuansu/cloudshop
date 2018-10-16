<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
<style>
.footer{ background:none; border:none; margin:0px; padding:0px;}
.footer .bottom{ border:none;margin:0px; padding:0px;}
</style>
<div class="bottom">
       <div class="footer_link">
        <Hi:Common_FriendLinks runat="server" TemplateFile="/ascx/tags/Common_Comment/Skin-Common_FriendLinks.ascx" />
    </div>
    <div class="Copyright">
        <Hi:PageFooter ID="PageFooter1" runat="server" />
         <Hi:CnzzShow ID="CnzzShow1" runat="server" /></div>
    </div>
</body>
</html> 