<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="AccountSettings.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.AccountSettings" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a class="hover">提现账户绑定</a></li>
            </ul>
        </div>

        <div class="datafrom">
            <div class="formitem validator1 depot-p">
                <ul>
                    <li>
                        <h2 class="colorE">银行卡绑定</h2>
                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>开户银行：</span>
                        <input type="text" id="txtBankName" class="forminput form-control" style="width: 160px" />
                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>账户名：</span>
                        <input type="text" id="txtBankAccountName" class="forminput form-control" style="width: 160px" />
                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>银行卡号：</span>
                        <input type="text" id="txtBankCardNo" class="forminput form-control" style="width: 160px" />
                    </li>
                </ul>
                <ul id="ulAlipay">
                    <li>
                        <h2 class="colorE">支付宝绑定</h2>
                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>支付宝帐号：</span>
                        <input type="text" id="txtAlipayAccount" class="forminput form-control" style="width: 160px" />
                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>真实姓名：</span>
                        <input type="text" id="txtAlipayRealName" class="forminput form-control" style="width: 160px" />
                    </li>
                </ul>
                <ul>
                    <li>
                        <h2 class="colorE">输入交易密码</h2>
                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>交易密码：</span>
                        <input type="password" id="txtPassword" class="forminput form-control" style="width: 160px" />
                    </li>
                </ul>
                <ul class="btntf Pa_198 clear">
                    <li>
                        <input type="button" value="保 存" class="btn btn-primary inbnt" id="btnAdd" onclick="doSubmit();" /></li>
                </ul>
            </div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hidSelectProducts" />
    <asp:HiddenField runat="server" ID="hidProductIds" />
    <asp:HiddenField runat="server" ID="hidAllSelectedProducts" />


    <script src="/Depot/home/scripts/AccountSettings.js?v=3.35" type="text/javascript"></script>
</asp:Content>
