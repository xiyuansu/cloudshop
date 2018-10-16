<%@ Control Language="C#"%>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<div class="tab_sales">
<div class="tr_head title">
<div class="name">买家</div><div class="num">数量</div>
<div class="size">款式和型号</div><div class="time">付款时间</div>
</div>
<asp:Repeater ID="rp_productsales" runat="server">
<ItemTemplate>
<div class="tr_head">
<div class="name"><img style="width:40px;border-radius:100%;" src="/templates/pccommon/images/test.jpg" /> <%#Eval("Username") %></div>
<div class="num"><%#Eval("Quantity") %></div>
<div class="size"><%# Eval("SKUContent") %>&nbsp;</div>
<div class="time"><%#Eval("PayDate")%></div>
</div>
</ItemTemplate>
</asp:Repeater>
</div>