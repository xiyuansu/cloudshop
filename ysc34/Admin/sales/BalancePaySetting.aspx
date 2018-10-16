<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BalancePaySetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.BalancePaySetting" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="PaymentTypes">PC端</a></li>
                <li><a href="MobilePaymentSet">移动端</a></li>
                <li><a href="WxPaySetting">微信支付</a></li>
                <li class="hover"><a href="javascript:void">余额支付</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem validator1 mt_10">
               
                <ul>
                    <li class="btline">
                        <h2 class="colorE">余额支付(支持微商城、触屏版、生活号（原支付宝服务窗）、APP、PC端)</h2>
                    </li>
                    <li class="clearfix"><span>注意：开启了余额支付抵扣，将会直接使用用户帐户的余额抵扣订单金额。</span></li>

                    <li class="clearfix"><span class="formitemtitle">是否开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="OnoffBalancePay"></Hi:OnOff>

                        </abbr>

                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClick="btnOK_Click" CssClass="btn btn-primary inbnt" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
