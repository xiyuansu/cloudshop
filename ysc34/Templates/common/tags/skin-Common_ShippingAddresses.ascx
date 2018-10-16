<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="address-box">
    <a onclick='UpdateShipping(<%#Eval("ShippingId") %>)' class="address_left" style="padding-right: 3.0rem;">
        <span><%#Eval("ShipTo")%>,<%#Eval("CellPhone")%></span>
        <div class="color_6">
            <Hi:RegionAllName ID="regionname" runat="server" RegionId='<%#Eval("RegionId") %>'></Hi:RegionAllName>
            <%#Eval("Address")+" "+Eval("BuildingNumber")%>
        </div>
        <asp:Literal ID="ltlUpgrade" runat="server" Text='<%# Eval("LatLng")%>'></asp:Literal>
    </a>
        <div class="address_default">
         <label class="label-checkbox item-content">
            <input type="radio"  IsDefault='<%# Eval("IsDefault").ToBool()%>' AddressId='<%# Eval("ShippingId") %>' name="radIsDefault" <%# Eval("IsDefault").ToBool()?"checked='checked'":"" %> style="-webkit-appearance: checkbox;" />
            <div class="item-media"><i class="icon icon-form-checkbox"></i></div>
        </label>        
        <span class="setDefTxt" style='color:<%# Eval("IsDefault").ToBool()?"#ff5722":"#333;"%>'><%# Eval("IsDefault").ToBool()?"默认地址":"设为默认" %></span>
            <span class="del_address">
            <a href="javascript:void(0)" class="icon_trash  icon-icon_trash_48" onclick='DeleteShippingAddress(<%#Eval("ShippingId") %>,this)'></a><i>删除</i>
                </span>
    </div>    
</div>

