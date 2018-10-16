<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.AppAliPaySet" CodeBehind="AppAliPaySet.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Register Src="~/Admin/Ascx/AccountNumbersTextBox.ascx" TagPrefix="uc1" TagName="AccountNumbersTextBox" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">
        function fuCheckEnableZFBPay(event, state) {
            if (state) {
                $("#divContent1").show();
            }
            else {
                $("#divContent1").hide();
            }
        }
        function fuCheckEnableZFBPage(event, state) {
            if (state) {
                $("#divContent2").show();
            }
            else {
                $("#divContent2").hide();
            }
        }
        $(document).ready(function () {
            if ($("#ctl00_contentHolder_radEnableAppAliPay input").is(':checked')) {
                $("#divContent1").show();
            }
            else {
                $("#divContent1").hide();
            }

            if ($("#ctl00_contentHolder_radEnableWapAliPay input").is(':checked')) {
                $("#divContent2").show();
            }
            else {
                $("#divContent2").hide();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <%--<script>
        //支付宝手机应用内支付
        function fuCheckEnableZFBPay(event, state) {

            if (state) {                
                $("#ctl00_contentHolder_txtAppPartner,#ctl00_contentHolder_txtAppKey,#ctl00_contentHolder_txtAppAccount").parent().show();
            }
            else {
                
                $("#ctl00_contentHolder_txtAppPartner,#ctl00_contentHolder_txtAppKey,#ctl00_contentHolder_txtAppAccount").parent().hide();
            }
        }
        //支付宝手机网页支付
        function fuCheckEnableZFBPage(event, state) {
            if (state) {
                $("#ctl00_contentHolder_txtPartner,#ctl00_contentHolder_txtKey,#ctl00_contentHolder_txtAccount").parent().show();
            }
            else {
                $("#ctl00_contentHolder_txtPartner,#ctl00_contentHolder_txtKey,#ctl00_contentHolder_txtAccount").parent().hide();
            }
        }
    </script>--%>
    <div class="dataarea mainwidth databody">
         <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">手机支付宝</a></li>
                <%--<li><a href="WeixinPay.aspx">微信支付设置</a></li>--%>
                <li><a href="ShengPaySet.aspx">盛付通支付</a></li>
                <li><a href="AppBankUnionSet.aspx">银联全渠道支付</a></li>
            </ul>
        </div>
        <!-- <span>建议您尽量使用支付宝手机应用内支付。如果您已经申请了支付宝手机网页支付，并且只想使用该支付方式，您可以点击
        <a href="javascript:void(0)" onclick="showWapPay()">这里</a> 进行配置支付宝手机网页支付。</span> -->
        <div class="datafrom">
            <div class="formitem validator1 mt_10">
                <ul>
                    <li>
                        <h2 class="colorE">支付宝app支付</h2>
                    </li>
                    <li class="clearfix"><span>请设置好您的支付宝信息。 还没开通快捷支付(无线)？ <a target="_blank" href="https://b.alipay.com/order/productDetail.htm?productId=2014110308142133">立即免费申请开通支付宝快捷支付(无线)接口</a></span></li>

                    <li class="clearfix"><span class="formitemtitle">是否开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radEnableAppAliPay"></Hi:OnOff>

                        </abbr>

                    </li>
                </ul>
                <ul id="divContent1">
                    <li><span class="formitemtitle">商家号：</span>
                        <uc1:AccountNumbersTextBox ID="txtAppPartner" CssClass="forminput form-control" runat="server" />
                    </li>
                    <li><span class="formitemtitle">商家密钥：</span>
                        <uc1:AccountNumbersTextBox ID="txtAppKey" CssClass="forminput form-control" runat="server" />
                    </li>
                    <li><span class="formitemtitle">收款账户：</span>
                        <uc1:AccountNumbersTextBox ID="txtAppAccount" CssClass="forminput form-control" runat="server" />
                    </li>
                </ul>
                <ul>
                    <li>
                        <h2 class="colorE">支付宝H5网页支付</h2>
                    </li>
                    <li class="clearfix"><span>请设置好您的支付宝信息。 还没开通支付宝？ <a target="_blank" href="https://b.alipay.com/order/productDetail.htm?productId=2013080604609688">立即免费申请开通支付宝接口</a></span></li>
                    <li class="clearfix"><span class="formitemtitle">是否开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radEnableWapAliPay"></Hi:OnOff>

                        </abbr>

                    </li>
                </ul>
                <ul id="divContent2">
                    <li><span class="formitemtitle">商家号/AppID：</span>
                        <uc1:AccountNumbersTextBox ID="txtPartner" CssClass="forminput form-control" runat="server" />
                    </li>
                   <li><span class="formitemtitle">商家私钥：</span>
                        <uc1:AccountNumbersTextBox ID="txtKey" CssClass="forminput form-control" runat="server" />
                    </li>
                    <li><span class="formitemtitle">商家公钥：</span>
                        <uc1:AccountNumbersTextBox ID="txtPublicKey" CssClass="forminput form-control" runat="server" />
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClick="btnOK_Click" CssClass="btn btn-primary inbnt" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            if ($("input[name='ctl00$contentHolder$radEnableWapAliPay']:checked").val() == "False")
                $("#ulwapAlipay").hide();
        });

        function showWapPay() {
            $("#ulwapAlipay").show();
        }
    </script>
</asp:Content>

