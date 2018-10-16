<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CommissionCashSetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CommissionCashSetting" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <asp:HiddenField ID="hidIsDemoSite" runat="server" Value="0" ClientIDMode="Static" />
        <div class="title">
            <ul class="title-nav">
                <li><a href="SplittinDrawRequest.aspx">银行卡结算</a></li>
                <li><a href="SplittinDrawRequestWeixin.aspx">微信结算</a></li>
                <li><a href="SplittinDrawRequestAlipay.aspx">支付宝结算</a></li>
                <li class="hover"><a href="javascript:void">佣金提现设置</a></li>
                <asp:Literal runat="server" ID="litUserName" />
            </ul>
        </div>

        <div class="datafrom">
            <div class="formitem validator1 p-100 setorder">
                <ul>
                    <li><span class="formitemtitle">预存款：</span>
                        <div class="input-group" style="font-size: 14px; width: 298px;">
                            <Hi:OnOff runat="server" ID="OnOffToDeposit" ClientIDMode="Static"></Hi:OnOff>
                        </div>
                    </li>
                    <li runat="server" id="wxbox"><span class="formitemtitle">微信支付：</span>
                        <div class="input-group" style="font-size: 14px; width: 398px;">
                            <Hi:OnOff runat="server" ID="OnOffToWeiXin" ClientIDMode="Static"></Hi:OnOff>
                            <span style="color: #AEAEAE;" id="spanWeiXin" runat="server" visible="false">&nbsp;&nbsp;&nbsp;&nbsp;您还未配置微信提现支付,<a href="../sales/cashsetting.aspx">去配置微信支付</a></span>
                        </div>
                    </li>
                    <li runat="server" id="alibox"><span class="formitemtitle">支付宝：</span>
                        <div class="input-group" style="font-size: 14px; width: 398px;">
                            <Hi:OnOff runat="server" ID="OnOffToALiPay" ClientIDMode="Static"></Hi:OnOff>
                            <span style="color: #AEAEAE;" id="spanAlipay" runat="server" visible="false">&nbsp;&nbsp;&nbsp;&nbsp;您还未配置支付宝提现支付,<a href="../sales/cashsetting.aspx">去配置支付宝支付</a></span>
                        </div>
                    </li>
                    <li><span class="formitemtitle">银行卡：</span>
                        <div class="input-group" style="font-size: 14px; width: 298px;">
                            <Hi:OnOff runat="server" ID="OnOffToBankCard" ClientIDMode="Static"></Hi:OnOff>
                        </div>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button ID="btnOK" runat="server" Text="保存" CssClass="btn btn-primary" OnClick="btnOK_Click" />
                    </li>
                </ul>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
