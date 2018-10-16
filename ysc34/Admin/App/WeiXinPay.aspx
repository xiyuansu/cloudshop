<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="WeiXinPay.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXinPay" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register Src="~/Admin/Ascx/AccountNumbersTextBox.ascx" TagPrefix="uc1" TagName="AccountNumbersTextBox" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">
        //是否开启服务商模式
        function funCheckOnOffServiceMode(event, state) {
            if (state) {
                $(".servicemode").show();
            }
            else {
                $(".servicemode").hide();
            }
        }
        function fuCheckEnableWXPay(event, state) {
            if (state) {
                $("#divContent").show();
            }
            else {
                $("#divContent").hide();
            }
        }

        $(document).ready(function () {
            if ($("#ctl00_contentHolder_radEnableHtmRewrite input").is(':checked')) {
                $("#divContent").show();
            }
            else {
                $("#divContent").hide();
            }
            if ($("#ctl00_contentHolder_OnOffServiceMode input").is(':checked')) {
                $(".servicemode").show();
            }
            else {
                $(".servicemode").hide();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <div class="dataarea mainwidth databody">
         <div class="title">
            <ul class="title-nav">
                <li ><a href="AppAliPaySet.aspx">手机支付宝</a></li>
                <li class="hover"><a >微信支付设置</a></li>
                <li><a href="ShengPaySet.aspx">盛付通支付</a></li>
                <li><a href="AppBankUnionSet.aspx">银联全渠道支付</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                    <li><span class="formitemtitle">开启微信付款：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radEnableHtmRewrite"></Hi:OnOff>
                        </abbr>
                    </li>
                </ul>
            </div>
            <div class="formitem validator1 pd_0">
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
                        <uc1:AccountNumbersTextBox ID="txtPartnerKey" CssClass="form_input_l form-control" runat="server" placeholder="填写服务商代理申请提供的Key" />
                    </li>
                    <li><span class="formitemtitle">服务商模式开启：</span>
                        <Hi:OnOff runat="server" ID="OnOffServiceMode"></Hi:OnOff>
                    </li>
                    <li class="servicemode"><span class="formitemtitle">服务商商户ID：</span>
                        <uc1:AccountNumbersTextBox ID="txtMainMchID" CssClass="form_input_l form-control" runat="server" placeholder="由服务商代理申请的微信支付时填写" />
                    </li>
                    <li class="servicemode"><span class="formitemtitle">服务商AppID：</span>
                        <uc1:AccountNumbersTextBox ID="txtMainAppID" CssClass="form_input_l form-control" runat="server" placeholder="由服务商代理申请的微信支付时填写" />
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
                    <asp:Button ID="btnOK" runat="server" Text="保 存" CssClass="btn btn-primary" OnClientClick="return PageIsValid();" OnClick="btnOK_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
