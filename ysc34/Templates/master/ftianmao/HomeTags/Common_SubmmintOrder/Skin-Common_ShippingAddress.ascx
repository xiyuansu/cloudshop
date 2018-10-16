<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Entities" %>
<%@ Import Namespace="System" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<div class="address_tab">
    <asp:Repeater ID="rp_shippgaddress" runat="server">
        <ItemTemplate>
            <div class="list" regionid="<%# Eval("RegionId") %>">
                <div class="inner ">
                    <div title="<%# RegionHelper.GetFullRegion(Convert.ToInt32(Eval("RegionId").ToString())," ") %>" (<%# Eval("ShipTo") %>收)" class="addr-hd">
                    <span class="name"><%# Eval("ShipTo") %></span>
                        (<%# RegionHelper.GetFullRegion(Convert.ToInt32(Eval("RegionId").ToString())," ") %>)</div>
                    <div class="addr-bd" title='<%# Eval("Address")%>'>
                        <span class="street"><%# Eval("Address")%></span>
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