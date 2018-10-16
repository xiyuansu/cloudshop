<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="RequestConfirm.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.Balance.RequestConfirm" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
    $(document).ready(function () {
        var DrawRequestType = $("#DrawRequestType").html();
        if (DrawRequestType == "银行卡转账") {
            $(".RInfo").hide(); $(".IsDefault").show();
        }       
        else if (DrawRequestType == "支付宝支付") {
            $(".RInfo").hide(); $(".IsAlipay").show();
        }

    });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
        <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li>
                        <h2 class="colorE">申请提现确认</h2>
                    </li>
                            <li><span class="formitemtitle">用 户 名：</span><asp:Literal ID="litUserName" runat="server" /></li>
                            <li><span class="formitemtitle">提现金额：</span><em><Hi:FormatedMoneyLabel ID="lblDrawBanlance" runat="server" /></em></li>
                             <li><span class="formitemtitle">提现方式：</span><em id="DrawRequestType"><asp:Literal ID="lblDrawRequestType" runat="server" /></em></li>
                            <li class="RInfo IsAlipay"><span class="formitemtitle">真实姓名：</span><em><asp:Literal ID="litAlipayRealName" runat="server" /></em></li>
                            <li class="RInfo IsAlipay"><span class="formitemtitle">收款账号：</span><em><asp:Literal ID="litAlipayCode" runat="server" /></em></li>

                            <li class="RInfo IsDefault"><span class="formitemtitle">开户银行：</span><em><asp:Literal ID="litBankName" runat="server" /></em></li>
                            <li class="RInfo IsDefault"><span class="formitemtitle">银行开户名：</span><em><asp:Literal ID="litAccountName" runat="server" /></em></li>
                            <li class="RInfo IsDefault"><span class="formitemtitle">账号信息：</span><em><asp:Literal ID="litMerchantCode" runat="server" /></em></li>

                            <li style="height:auto;"><span class="formitemtitle">备 注：</span><em><asp:Literal ID="litRemark" runat="server" /></em></li>
                        </ul>
                        <div class="xiayibu"><asp:Button ID="btnDrawConfirm" runat="server" Text="确认提现" OnClick="btnDrawConfirm_Click"  CssClass="btn btn-primary ml_198" /></div>
                    </div>
             </div>
            </div>
</asp:Content>
