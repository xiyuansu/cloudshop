<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Entities" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="Hidistro.Context" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<div class="address_tab">
    <asp:Repeater ID="rp_shippgaddress" runat="server">
        <ItemTemplate>
            <div class="list" regionid="<%# Eval("RegionId") %>">
                <div class="inner ">
                    <div title="<%# Eval("FullAddress") %>" (<%# Eval("ShipTo") %>收)" class="addr-hd">
                    <span class="name"><%# Eval("ShipTo") %></span>
                        (<%# Eval("FullAddress") %>)</div>
                    <div class="addr-bd" title='<%# Eval("FullAddress")%>'>
                        <span class="street"><%# Eval("FullAddress")%></span>
                        <span class="phone"><%# Eval("CellPhone")%></span>
                        <span class="last">&nbsp;</span>
                    </div>
                </div>
                <em class="curmarker"></em>
                <input type="hidden" value='<%# Eval("ShippingId") %>' />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>