<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<li>
    <span>
        <label class="label-checkbox item-content">
            <input type="radio" id="chk_list" name="ShippingId" value='<%# Eval("ShippingId") %>' onclick="onAddressChoose()" />
            <div class="item-media"><i class="icon icon-form-checkbox" id="test"></i></div>
        </label>
    </span>
    <samp><strong><%# Eval("ShipTo") %>，<%# Eval("CellPhone") %></strong><br />
        <%# Eval("FullAddress") %></samp>
    <div class="step1_right" id="iUpdateAddress" onclick='gotoEdit(<%# Eval("ShippingId") %>)' style='display:<%# string.IsNullOrEmpty(Eval("LatLng").ToNullString().Replace(",",""))?"inline":"none" %>'>
        <i class="icon-update2">升级</i>
    </div>
    <div class="step1_right icon-icon_right2" id="spanTagAddress" onclick='gotoEdit(<%# Eval("ShippingId") %>)' style='display:<%# string.IsNullOrEmpty(Eval("LatLng").ToNullString().Replace(",",""))?"none":"inline" %>'>
        <span class="icon_viewdetial"></span>
    </div>
</li>
