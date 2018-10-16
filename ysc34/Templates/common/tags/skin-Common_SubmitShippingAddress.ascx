<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<div style=" position: relative;" onclick="goChoiceAddress()">
    <input type="hidden" id="selectShipTo" value="<%# Eval("ShippingId") %>" />
    <input type="hidden" id="selectShipLatLng" value="<%# Eval("LatLng") %>" clientidmode="static" />
    <div class="step1_left" style="left: -1.85rem; top: 10%">
        <span class="icon_adress icon-icon5" id="spanTagAddress"></span>
        <i class="icon-update2" id="iUpdateAddress" style="display:none;">升级</i>
    </div>
    <div class="m step1-in ">
        <a href="javascript:void(0)">
            <div class="s1-name">
                <span><%# Eval("ShipTo") %>，<%# Eval("CellPhone") %></span>
            </div>
            <div class="slnamebt">
                <%# Eval("FullAddress") %>
            </div>
        </a>
    </div>
    <div class="step1_right icon-icon_right2" style="right: -1.25rem; top: 30%">
        <span class="icon_viewdetial"></span>
    </div>
</div>
<div style="position: relative; padding-top: 0.5rem; border-top: 1px solid #e0e0e0;display:none" id="idCertification">
    <div class="step1_left" style="left: -1.75rem; top: 30%">
        <span class="icon_id"></span>
    </div>
    <div class="m step1-in " style="padding-right: 1.6rem">
        <a href="javascript:void(0)" onclick="GoCertification()">
            <div class="s1-name">
                <span>补充实名认证信息</span>
            </div>
            <div>
                根据海关要求，购买跨境商品需要实名认证
            </div>
        </a>
    </div>
    <div class="step1_right">
        <span class="icon_viewdetial"></span>
    </div>
</div>
<b class="s1-borderB"></b>

<script language="javascript" type="text/javascript">
    //跳转实名验证
    function GoCertification() {
        var url = "SubmitIDInfo.aspx?ShippingId=" + $("#selectShipTo").val() + "&Source=SubmmitOrder&returnUrl=" + document.location.href;
        location.href = url;
    }
</script>
