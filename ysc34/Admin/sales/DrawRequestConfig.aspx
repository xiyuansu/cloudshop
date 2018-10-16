<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DrawRequestConfig.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.DrawRequestConfig" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <link href="/admin/css/Hishopv5.css" rel="stylesheet" />
    <script type="text/javascript" src="/admin/js/Hishopv5.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div id="rechargeGift" runat="server" style=" font-size:14px; text-align:center;">因进行过充值赠送金额活动，为了您的账户安全，预存款账户将<br/>永久关闭提现功能！</div>
            <div id="formitem" runat="server" class="formitem">
                <ul>
                       <li><span class="formitemtitle os-switch-text">是否开启预存款提现：</span>
                        <Hi:OnOff runat="server" ID="ooEnableBulkPaymentAdvance"></Hi:OnOff>
                        <span class="ml_10"><asp:Literal ID="litMsg" runat="server" Text="开启后，用户可以申请预存款提现。"></asp:Literal></span>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnSave" runat="server" OnClientClick="return IsFlagDate();" CssClass="btn btn-primary" Text="保存" />
                </div>
            </div>
        </div>
    </div>
       </div>
</asp:Content>
