<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
  <tr id="spqingdan_title">
  <td width="14%" align="center">收货人</td>
    <td width="10%" align="center">邮政编码</td>
    <td width="31%" align="center">详细地址</td>
    <td width="15%" align="center">电话号码</td>
    <td width="15%" align="center">手机号码</td>
    <td width="15%" align="center">操作</td>
  </tr>
<asp:Repeater ID="repeaterRegionsSelect" runat="server">
     <ItemTemplate>
     
     <tr>
     <td align="center"><asp:Label ID="lblShipTo" runat="server" Text='<%#Bind("ShipTo") %>'></asp:Label></td>
     <td align="center"><asp:Label ID="lblZipcode" runat="server" Text='<%#Bind("ZipCode") %>'></asp:Label></td>
     <td align="center" style="line-height: 25px; padding: 0px 10px;"><Hi:RegionAllName ID="RegionAllName1" RegionId='<%# Eval("RegionId") %>' runat="server"></Hi:RegionAllName><asp:Label ID="lblAddress" runat="server" Text='<%#Bind("Address") %>'></asp:Label></td>
     <td align="center"><asp:Label ID="lblTellPhone" runat="server" Text='<%#Bind("TelPhone")%>'></asp:Label></td>
     <td align="center"><asp:Label ID="lblPhone" runat="server" Text='<%#Bind("CellPhone") %>'></asp:Label></td>
     <td align="center"><asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("ShippingId") %>' CommandName="Edit" Text="编辑" />&nbsp;&nbsp;<asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("ShippingId") %>' OnClientClick="return window.confirm('确认要删除该收货地址吗?')" CommandName="Delete" Text="删除" /></td>
     </tr>
    </ItemTemplate>
</asp:Repeater>
