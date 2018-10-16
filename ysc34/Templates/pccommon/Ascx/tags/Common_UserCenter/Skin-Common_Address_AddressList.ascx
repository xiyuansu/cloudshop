<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<style type="text/css">
    span.default{width:auto; background:#ff6a00;padding:2px 5px; color:#ffffff;}
    span.noverify{font-size:12px;width: auto;padding: 2px 5px;color: #ff6a00;}
</style>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
  <tr id="spqingdan_title">
  <td width="15%" align="center">收货人</td>
    <td align="center">详细地址</td>
    <td width="15%" align="center">电话号码</td>
    <td width="15%" align="center">手机号码</td>
    <td width="15%" align="center">操作</td>
  </tr>
<asp:Repeater ID="repeaterRegionsSelect" runat="server">
     <ItemTemplate>
     
     <tr>
     <td align="center"><asp:Label ID="lblShipTo" runat="server" Text='<%#Bind("ShipTo") %>'></asp:Label><%# Eval("IDStatus").ToInt()==1?"<span class=\"noverify\">未实名验证</span>":"" %></td>
     <td align="center" style="line-height: 25px; padding: 0px 10px;"> <asp:Label ID="lblAddress" runat="server" Text='<%#Bind("FullAddress") %>'></asp:Label></td>
     <td align="center"><asp:Label ID="lblTellPhone" runat="server" Text='<%#Bind("TelPhone")%>'></asp:Label></td>
     <td align="center"><asp:Label ID="lblPhone" runat="server" Text='<%#Bind("CellPhone") %>'></asp:Label></td>
     <td align="center">
         <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("ShippingId") %>' CommandName="Edit" Text="编辑" />
         <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("ShippingId") %>' OnClientClick="return window.confirm('确认要删除该收货地址吗?')" CommandName="Delete" Text="删除" />
         <asp:LinkButton ID="btnSetDefault" runat="server" Visible='<%# !Eval("IsDefault").ToBool() %>' CommandArgument='<%# Eval("ShippingId") %>' CommandName="SetDefault" Text="设为默认" />
         <%# Eval("IsDefault").ToBool()?"<span class=\"default\">默认地址</span>":"" %>
     </td>
     </tr>
    </ItemTemplate>
</asp:Repeater>
