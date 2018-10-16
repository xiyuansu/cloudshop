<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core.Urls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
  <div class="icon"><img src="<%#Eval("Icon")%>"></div><h2><a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%> </a></h2><div class="c">
<asp:Repeater ID="rptSubCategries" runat="server" >
    <ItemTemplate>
    <span>
    <a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><asp:Literal ID="litName" runat="server" Text='<%# Eval("Name")%>'></asp:Literal></a></span>
    </ItemTemplate>
</asp:Repeater>
</div></li>
