<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core.Urls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
	 <h3><a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%> </a></h3>
    <div><b style="display:none;"><a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'>全部</a></b> 
     <asp:Repeater ID="rptSubCategries" runat="server">
        <ItemTemplate>
		   <a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><asp:Literal ID="litName" runat="server" Text='<%# Eval("Name")%>'></asp:Literal></a> 
        </ItemTemplate>
    </asp:Repeater>
   </div>
</li>
 