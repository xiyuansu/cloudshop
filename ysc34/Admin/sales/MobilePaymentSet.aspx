<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MobilePaymentSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.MobilePaymentSet" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Register Src="~/Admin/Ascx/AccountNumbersTextBox.ascx" TagPrefix="uc1" TagName="AccountNumbersTextBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">
        function funCheckEnableZFBPay(event, state) {
            if (state) {
                $("#divContent1").show();
            }
            else {
                $("#divContent1").hide();
            }
        }
       
        function funCheckEnableYLQQD(event, state) {
            if (state) {
                $("#divContent3").show();
            }
            else {
                $("#divContent3").hide();
            }
        }
        function funCheckEnableSFT(event, state) {
            if (state) {
                $("#divContent4").show();
            }
            else {
                $("#divContent4").hide();
            }
        }
        function funCheckEnableKJZFB(event, state) {
            if (state) {
                $("#divContent5").show();
            }
            else {
                $("#divContent5").hide();
            }
        }
        $(document).ready(function () {
            if ($("#ctl00_contentHolder_radEnableAppAliPay input").is(':checked')) {
                $("#divContent1").show();
            }
            else {
                $("#divContent1").hide();
            }

           
            if ($("#ctl00_contentHolder_OnoffYLQQD input").is(':checked')) {
                $("#divContent3").show();
            }
            else {
                $("#divContent3").hide();
            }
            if ($("#ctl00_contentHolder_OnoffSFT input").is(':checked')) {
                $("#divContent4").show();
            }
            else {
                $("#divContent4").hide();
            }
            if ($("#ctl00_contentHolder_OnoffKJZFB input").is(':checked')) {
                $("#divContent5").show();
            }
            else {
                $("#divContent5").hide();
            }
        });
    </script>
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
                <li class="hover"><a href="javascript:void">移动端</a></li>
                <li><a href="WxPaySetting">微信支付</a></li>
                <li><a href="BalancePaySetting">余额支付</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem validator1 mt_10">
                <ul>
                    <li class="btline">
                        <h2 class="colorE">支付宝H5网页支付(支持微商城、触屏版、生活号（原支付宝服务窗）)</h2>
                    </li>
                    <li class="clearfix"><span>请设置好您的支付宝信息。 还没开通支付宝？ <a target="_blank" href="https://b.alipay.com/order/productDetail.htm?productId=2013080604609688">立即免费申请开通支付宝接口</a></span></li>

                    <li class="clearfix"><span class="formitemtitle">是否开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radEnableAppAliPay"></Hi:OnOff>

                        </abbr>

                    </li>
                </ul>
                <ul id="divContent1">
                    <li><span class="formitemtitle">AppId：</span>
                        <uc1:AccountNumbersTextBox ID="txtPartner" CssClass="forminput form-control" runat="server" />
                    </li>
                    <li><span class="formitemtitle">商家私钥：</span>
                        <uc1:AccountNumbersTextBox ID="txtKey" CssClass="forminput form-control" runat="server" />
                    </li>
                    <li><span class="formitemtitle">商家公钥：</span>
                        <uc1:AccountNumbersTextBox ID="txtPublicKey" CssClass="forminput form-control" runat="server" />
                    </li>
                    <li style="display:none;"><span class="formitemtitle">收款账户：</span>
                        <uc1:AccountNumbersTextBox ID="txtAccount" CssClass="forminput form-control" runat="server" />
                    </li>
                </ul>
                <div id="divOnlyForWap" runat="server">
                    <ul>
                        <li class="btline">
                            <h2 class="colorE">银联全渠道支付(支持触屏版)</h2>
                        </li>
                        <li class="clearfix"><span class="formitemtitle">是否开启：</span>
                            <abbr class="formselect">
                                <Hi:OnOff runat="server" ID="OnoffYLQQD"></Hi:OnOff>

                            </abbr>

                        </li>
                    </ul>
                    <ul id="divContent3">
                        <li><span class="formitemtitle">商户号：</span>
                            <uc1:AccountNumbersTextBox ID="txtYLQQDPartner" CssClass="forminput form-control" runat="server" />
                        </li>
                        <li><span class="formitemtitle">上传证书：</span>
                            <asp:FileUpload ID="fileBankUnionCert" runat="server" CssClass="input-file" />
                        </li>
                        <li><span class="formitemtitle">证书文件名：</span>
                            <uc1:AccountNumbersTextBox ID="txtYLQQDCerFileName" CssClass="forminput form-control" runat="server" ReadOnly="true" />
                        </li>
                        <li><span class="formitemtitle">证书密码：</span>
                            <uc1:AccountNumbersTextBox ID="txtYLQQDKey" CssClass="forminput form-control" runat="server" />
                        </li>
                    </ul>
                    <ul>
                        <li class="btline">
                            <h2 class="colorE">跨境支付宝支付(支持触屏版)</h2>
                        </li>
                        <li class="clearfix"><span class="formitemtitle">是否开启：</span>
                            <abbr class="formselect">
                                <Hi:OnOff runat="server" ID="OnoffKJZFB"></Hi:OnOff>

                            </abbr>

                        </li>
                    </ul>
                    <ul id="divContent5">
                        <li><span class="formitemtitle">安全校验码(Key)：</span>
                            <uc1:AccountNumbersTextBox ID="txtKJZFBKey" CssClass="form_input_l form-control" runat="server" />
                        </li>
                        <li><span class="formitemtitle">收款支付宝账号：</span>
                            <uc1:AccountNumbersTextBox ID="txtSellerEmail" CssClass="form_input_l form-control" runat="server" />
                        </li>
                        <li><span class="formitemtitle">合作者身份(PID)：</span>
                            <uc1:AccountNumbersTextBox ID="txtKJZFBPartner" CssClass="form_input_l form-control" runat="server" />
                        </li>
                        <li><span class="formitemtitle">是否人民币收款：</span>
                            <Hi:OnOff runat="server" ID="ooRMB"></Hi:OnOff>
                        </li>
                        <li><span class="formitemtitle">币种：</span>
                            <uc1:AccountNumbersTextBox ID="txtCurrency" CssClass="form_input_l form-control" runat="server" />
                        </li>
                    </ul>
                </div>
                <ul>
                    <li class="btline">
                        <h2 class="colorE">盛付通支付(支持微商城、触屏版、生活号（原支付宝服务窗）)</h2>
                    </li>
                    <li class="clearfix"><span>注意：移动端盛付通支付申请“快捷支付H5支付接口”，PC端申请“网银B2C接口”，需前往官网找在线客服申请签约， <a target="_blank" href="https://www.shengpay.com/">立即前往</a></span></li>

                    <li class="clearfix"><span class="formitemtitle">是否开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="OnoffSFT"></Hi:OnOff>

                        </abbr>

                    </li>
                </ul>
                <ul id="divContent4">
                    <li><span class="formitemtitle">商户号：</span>
                        <uc1:AccountNumbersTextBox ID="txtSPPartner" CssClass="forminput form-control" runat="server" />
                    </li>
                    <li><span class="formitemtitle">商家密钥：</span>
                        <uc1:AccountNumbersTextBox ID="txtSPKey" CssClass="forminput form-control" runat="server" />
                    </li>
                </ul>

                <div class="ml_198">
                    <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClick="btnOK_Click" CssClass="btn btn-primary inbnt" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
