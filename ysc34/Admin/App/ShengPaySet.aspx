<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ShengPaySet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.App.ShengPaySet" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
     <script type="text/javascript">
         function fuCheckEnableShengPay(event, state) {
             if (state) {
                 $("#divContent").show();
             }
             else {
                 $("#divContent").hide();
             }
         }

         $(document).ready(function () {
             if ($("#ctl00_contentHolder_radEnableAppShengPay input").is(':checked')) {
                 $("#divContent").show();
             }
             else {
                 $("#divContent").hide();
             }
         });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="AppAliPaySet.aspx">手机支付宝</a></li>
                <%--<li ><a  href="WeixinPay.aspx">微信支付设置</a></li>--%>
                <li class="hover"><a >盛付通支付</a></li>
                <li><a href="AppBankUnionSet.aspx">银联全渠道支付</a></li>
            </ul>
        </div>
       
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                    <li class="clearfix"><span class="formitemtitle">是否开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radEnableAppShengPay"></Hi:OnOff>
                        </abbr>

                    </li>
                </ul>
            </div>
            <div class="formitem validator1">
                <ul id="divContent">
                    
                    <li><span class="formitemtitle">商户号：</span>
                        <asp:TextBox ID="txtPartner" CssClass="forminput formwidth form-control" runat="server" />
                    </li>
                    <li><span class="formitemtitle">商家密钥：</span>
                        <asp:TextBox ID="txtKey" CssClass="forminput formwidth form-control" runat="server" />
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnOK" runat="server" Text="保 存" CssClass="btn btn-primary inbnt" OnClientClick="return PageIsValid();" OnClick="btnOK_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
