<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_ChargesList.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Order_ChargesList" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<style>
    .total {
        width: 50%;
        float: right;
    }
</style>
<style>
    .total_table tr td{
        line-height:35px;
    }
</style>
<table width="300" border="0" cellspacing="0" style="float: right;" class="total_table">
    <tr>
        <td align="right">购物车小计：</td>
        <td>
            <Hi:FormatedMoneyLabel ID="lblTotalPrice" runat="server" /></td>
    </tr>
    <tr id="tdWeight" runat="server" visible="false">
        <td align="right">商品重量：</td>
        <td>
            <asp:Literal runat="server" ID="litWeight" />g</td>
    </tr>
    <tr id="tdFreight" runat="server">
        <td style="width: 120px; " align="right" >运费： </td>
        <td style="color:#666;width:180px;" class="text-ellipsis float">
            <asp:Literal ID="litFreight" runat="server" />&nbsp;<asp:Literal ID="litShippingMode" runat="server"></asp:Literal><%--&nbsp;<asp:HyperLink ID="hlkFreightFreePromotion" runat="server" Target="_blank" />--%></td>
    </tr>
    <tr>
        <td align="right">支付方式：</td>
        <td style="color:#666;">
            <asp:Literal ID="litPayMode" runat="server"></asp:Literal></td>
    </tr>
    <tr id="tdBalanceAmount" runat="server" visible="false">
        <td align="right">余额抵扣：</td>
        <td colspan="2"><span style="color:#666;">
            -<asp:Literal ID="liBalanceAmount" runat="server" Visible="true" /></span></td>
    </tr>
    <tr id="tdCouponValue" runat="server" visible="false">
        <td align="right">优惠折扣：</td>
        <td colspan="2"><span style="color:#666;">
            <asp:Literal ID="litCoupon" runat="server" /><asp:Literal ID="litCouponValue" runat="server" Visible="false" /></span></td>
    </tr>
    <tr id="tdDeductionMoney" runat="server" visible="false">
        <td align="right">积分抵扣：</td>
        <td colspan="2"><span style="color:#666;">
            <asp:Literal ID="lblDeductionMoney" runat="server" /></span></td>
    </tr>
    <tr id="tdDiscount" runat="server" visible="false">
        <td align="right">涨价或减价：</td>
        <td><span style="color:#666;">
            <asp:Literal ID="litDiscount" runat="server" /></span></td>
    </tr>
    <asp:Literal ID="litTax" runat="server" />
    <asp:Literal ID="litInvoiceTitle" runat="server" />
    <tr>
        <td align="right">订单可得积分：</td>
        <td colspan="2" style="color:#666;width:180px;" class="text-ellipsis float">
            <asp:Literal ID="litPoints" runat="server" /><%--&nbsp;<asp:HyperLink ID="hlkSentTimesPointPromotion" runat="server" Target="_blank" />--%></td>
    </tr>
    <tr id="tdRefundPoint" runat="server" visible="false">
        <td align="right">退款扣除积分：</td>
        <td colspan="2" style="color:#666;">
            <asp:Literal ID="litRefundPoint" runat="server" />&nbsp;</td>
    </tr>
    <tr id="tdReducedPromotion" runat="server" visible="false">
        <td align="right">订单促销：</td>
        <td style="color:#666;width:180px;" class="text-ellipsis float">
            <asp:Literal ID="litPromotionPrice" runat="server" />&nbsp;
            <asp:HyperLink ID="hlkReducedPromotion" runat="server" Target="_blank" /></td>
    </tr>
    <tr id="tdBundlingPrice" runat="server" visible="false">
        <td align="right">捆绑价格：</td>
        <td style="color:#666;">
            <asp:Literal runat="server" ID="lblBundlingPrice" /></td>
    </tr>
    <tr class="bg" style="border-top:1px solid #ddd;">
        <td align="right" class="colorG">订单实收款：</td>
        <td colspan="2"><strong style="color:#333;font-weight:700;">
            <asp:Literal ID="litTotalPrice" runat="server" Text="0" />元</strong></td>
    </tr>
    <tr class="bg" id="tdRefundAmout" runat="server" visible="false">
        <td align="right" class="colorG">退款金额：</td>
        <td colspan="2"><strong style="color:#666;">
            <asp:Literal ID="litRefundAmount" runat="server" Text="0" /></strong></td>
    </tr>
</table>
