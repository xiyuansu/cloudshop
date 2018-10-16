<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="O2OXCXPaySetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Applet.O2OXCXPaySetting" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Register Src="~/Admin/Ascx/AccountNumbersTextBox.ascx" TagPrefix="uc1" TagName="AccountNumbersTextBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript">
        function fuCheckEnableWXPay(event, state) {
            if (state) {
                $("#divContent").show();
            }
            else {
                $("#divContent").hide();
            }
        }

        $(document).ready(function () {
            if ($("#ctl00_contentHolder_ooEnableHtmRewrite input").is(':checked')) {
                $("#divContent").show();
            }
            else {
                $("#divContent").hide();
            }
        });
    </script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">微信支付设置</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem validator2">
                <ul>
                    <li>
                        <span class="formitemtitle">开启微信付款：</span>
                        <Hi:OnOff runat="server" ID="ooEnableHtmRewrite"></Hi:OnOff>
                    </li>
                </ul>
            </div>
            <div class="formitem validator2">
                <ul id="divContent">
                    <li><span class="formitemtitle">AppId：</span>
                        <uc1:AccountNumbersTextBox ID="txtAppId" CssClass="form_input_l form-control" runat="server" />
                    </li>
                    <li><span class="formitemtitle">AppSecret：</span>
                        <uc1:AccountNumbersTextBox ID="txtAppSecret" CssClass="form_input_l form-control" runat="server" />
                    </li>
                    <li><span class="formitemtitle">mch_id：</span>
                        <uc1:AccountNumbersTextBox ID="txtPartnerID" CssClass="form_input_l form-control" runat="server" />
                    </li>
                    <li><span class="formitemtitle">Key：</span>
                        <uc1:AccountNumbersTextBox ID="txtPartnerKey" CssClass="form_input_l form-control" runat="server" placeholder="" />
                    </li>
                   
                    <li><span class="formitemtitle">微信证书：</span>
                        <asp:FileUpload ID="file_UploadCert" runat="server" />
                        <p id="ctl00_contentHolder_UploadCertTip">证书用于企业帐号支付以及退款原路返回，请使用扩展名为p12的证书文件</p>
                    </li>
                    <li id="liCertFileName" runat="server"><span class="formitemtitle">证书文件名：</span>
                        <Hi:TrimTextBox ID="txtCerFileName" CssClass="forminput form-control" runat="server" ReadOnly="true" />
                    </li>
                    <li style="display: none;"><span class="formitemtitle">证书密码：</span>
                        <uc1:AccountNumbersTextBox ID="txtCertPassword" CssClass="form_input_l form-control" runat="server" placeholder="证书和证书密码用于微信退款,默认证书密码为商户号" />

                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnOK" runat="server" Text="保 存" CssClass="btn btn-primary"
                        OnClientClick="return PageIsValid();" OnClick="btnOK_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>



