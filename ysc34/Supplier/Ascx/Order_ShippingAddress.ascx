<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_ShippingAddress.ascx.cs" Inherits="Hidistro.UI.Web.Supplier.Order_ShippingAddress" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%--<h1 style="font-size:14px;">物流信息</h1>--%>


<div id="updatetag_div" style="display: none;">
    <div class="frame-content">
        <p>
            <span class="frame-span frame-input90">物流公司：<em>*</em> </span>
            <Hi:ExpressDropDownList ClientIDMode="Static" runat="server" CssClass="forminput form-control order-ShippingAddress-select" ID="expressRadioButtonList"></Hi:ExpressDropDownList>
        </p>
        <p>
            <span class="frame-span frame-input90">发货单号：<em>*</em> </span>
            <asp:TextBox ID="txtpost" ClientIDMode="Static" CssClass="forminput form-control" runat="server" />
        </p>
        <input type="hidden" id="txt_expressCompanyName" runat="server" clientidmode="Static" />
        <input type="hidden" id="OrderId" runat="server" clientidmode="Static" />
    </div>
</div>

<div style="display: none">
    <asp:Button ID="btnupdatepost" runat="server" Text="修 改" CssClass="btn btn-primary" />
    <input type="hidden" id="hdtagId" runat="server" />
</div>




<!--物流信息-->
<asp:Panel ID="plExpress" runat="server" Visible="false" Style="widht: 730px; margin-bottom: 10px;">
    <h1 style="font-size: 16px; margin-top: 20px; padding-bottom: 5px;">快递单物流信息</h1>
    <div style="clear: both"></div>
    <ul class="express_title">
        <li>
            <span>物流公司：</span><%=Order.ExpressCompanyName %>

        </li>
        <li>
            <span>物流单号：</span><%=Order.ShipOrderNumber %>            
        </li>
        <div class="clear"></div>
    </ul>
    <div style="clear: both"></div>
    <div id="expressInfo" style="line-height: 1.8;">正在加载中....</div>
</asp:Panel>
<script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>
<script>
    function ShowPurchaseOrder() {
        formtype = "changeorder";
        arrytext = null;
        DialogShow("修改发货单号", 'changepurcharorder', 'updatetag_div', 'ctl00_contentHolder_shippingAddress_btnupdatepost');
    }

    $(function () {
        var orderId = $('#OrderId').val();
        if (orderId) {
            $('#expressInfo').expressInfo(orderId);
        }
    });



</script>
