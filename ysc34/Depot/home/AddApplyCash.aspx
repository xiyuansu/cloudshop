<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="AddApplyCash.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.AddApplyCash" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a class="hover">申请提现</a></li>
                <li><a href="ApplyCashManage.aspx">提现明细</a></li>
            </ul>
        </div>

        <div class="datafrom">
            <div class="formitem validator1 depot-p">
                <ul>
                    <li class="clearfix"><span class="formitemtitle">银行卡：</span>
                         <p>
                            账户名:<i id="iBankName"></i>
                        </p>
                        <p>
                            账户名:<i id="iBankAccountName"></i>
                        </p>
                        <p>
                            银行卡号:<i id="iBankCardNo"></i>
                        </p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>提现金额：</span>
                        <input type="text" id="txtRequestAmount" class="forminput form-control" style="width: 160px" />
                        <p id="ctl00_contentHolder_txtUserNameTip">
                            可用余额<i id="iBalance">0</i>元
                        </p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>交易密码：</span>
                        <input type="password" id="txtPassword" class="forminput form-control" style="width: 160px" />
                    </li>
                    <li class="clearfix"><span class="formitemtitle">备注：</span>
                        <input type="text" id="txtRemark" class="forminput form-control" style="width: 160px" maxlength="10" />
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


    <script src="/Depot/home/scripts/AddApplyCash.js?v=3.1" type="text/javascript"></script>
</asp:Content>
