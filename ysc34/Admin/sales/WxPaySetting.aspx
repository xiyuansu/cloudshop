<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="WxPaySetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WxPaySetting" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register Src="~/Admin/Ascx/AccountNumbersTextBox.ascx" TagPrefix="uc1" TagName="AccountNumbersTextBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">

        function funCheckEnableWxH5Pay(event, state) {
            if (state) {//如果选中则显示
                $("#divContent2").show();
            }
            else {
                //否则判断其它两个是否选中或者可用
                if ($("#ctl00_contentHolder_radEnableWxPay input").is(':checked') || $("#ctl00_contentHolder_radEnableWxQrcodePay input").is(':checked') || $("#ctl00_contentHolder_radEnableAppWxPay input").is(':checked'))
                    $("#divContent2").show();
                else
                    $("#divContent2").hide();
            }
        }
        //是否开启服务商模式
        function funCheckOnOffServiceMode(event, state) {
            if (state) {
                $(".servicemode").show();
            }
            else {
                $(".servicemode").hide();
            }
        }
        function funCheckEnableWxPay(event, state) {
            if (state) {
                $("#divContent2").show();
            }
            else {
                if ($("#ctl00_contentHolder_radEnableWxH5Pay input").is(':checked') || $("#ctl00_contentHolder_radEnableWxQrcodePay input").is(':checked') || $("#ctl00_contentHolder_radEnableAppWxPay input").is(':checked'))
                    $("#divContent2").show();
                else
                    $("#divContent2").hide();
            }
        }
        function funCheckEnableAppWxPay(event, state) {
            if (state) {
                $("#divContent2").show();
            }
            else {
                if ($("#ctl00_contentHolder_radEnableWxH5Pay input").is(':checked') || $("#ctl00_contentHolder_radEnableWxQrcodePay input").is(':checked') || $("#ctl00_contentHolder_radEnableWxPay input").is(':checked'))
                    $("#divContent2").show();
                else
                    $("#divContent2").hide();
            }
        }
        function funCheckEnableWxQrcodePay(event, state) {
            if (state) {
                $("#divContent2").show();
            }
            else {
                if ($("#ctl00_contentHolder_radEnableWxH5Pay input").is(':checked') || $("#ctl00_contentHolder_radEnableWxPay input").is(':checked') || $("#ctl00_contentHolder_radEnableAppWxPay input").is(':checked'))
                    $("#divContent2").show();
                else
                    $("#divContent2").hide();
            }
        }
        $(document).ready(function () {

            if ($("#ctl00_contentHolder_radEnableWxPay input").is(':checked') || $("#ctl00_contentHolder_radEnableWxQrcodePay input").is(':checked') || $("#ctl00_contentHolder_radEnableWxH5Pay input").is(':checked') || $("#ctl00_contentHolder_radEnableAppWxPay input").is(':checked')) {
                $("#divContent2").show();
            }
            else {
                $("#divContent2").hide();
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
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="PaymentTypes">PC端</a></li>
                <li><a href="MobilePaymentSet">移动端</a></li>
                <li class="hover"><a href="javascript:void">微信支付</a></li>
                <li><a href="BalancePaySetting">余额支付</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem validator1 mt_10">
                <ul>
                    <li class="btline">
                        <h2 class="colorE">微信支付(支持微商城、触屏版、APP、生活号（原支付宝服务窗）)</h2>
                    </li>
                    <li class="clearfix">
                        <span class="formitemtitle" style="width:160px">微商城支付开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radEnableWxPay"></Hi:OnOff>
                        </abbr>
                        <span class="formitemtitle" style="width:160px">微信H5支付开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radEnableWxH5Pay"></Hi:OnOff>
                        </abbr>
                        <span class="formitemtitle" style="width:160px">APP微信支付开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radEnableAppWxPay"></Hi:OnOff>
                        </abbr>
                        <span class="formitemtitle" style="width:160px">微信扫码支付开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radEnableWxQrcodePay"></Hi:OnOff>
                        </abbr>
                    </li>
                </ul>
                <ul id="divContent2">
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

                    <li style="display: none;"><span class="formitemtitle ">PaySignKey：</span>
                        <uc1:AccountNumbersTextBox ID="txtPaySignKey" CssClass="form_input_l form-control" runat="server" placeholder="" />
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
                    <li class="clearfix">
                        <span class="formitemtitle">PC端预付款充值是否开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="OnOffPcInpour"></Hi:OnOff>
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
