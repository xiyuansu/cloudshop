<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AppBankUnionSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.App.AppBankUnionSet" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">
        function fuCheckEnableUnionPay(event, state) {
            if (state) {
                $("#divContent").show();
            }
            else {
                $("#divContent").hide();
            }
        }

        $(document).ready(function () {
            if ($("#ctl00_contentHolder_radEnableWapBankUnionPay input").is(':checked')) {
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
                <li><a href="ShengPaySet.aspx">盛付通支付</a></li>
                <li class="hover"><a href="">银联全渠道支付</a></li>
            </ul>
        </div>
      <div class="datafrom">
          <div class="formitem validator1">
                <ul>
            <%--<li><h2 class="colorE">银联全渠道支付</h2></li>--%>
                    <li class="clearfix"><span class="formitemtitle">是否开启：</span>
                        <abbr class="formselect">
                      <Hi:OnOff runat="server" ID="radEnableWapBankUnionPay"></Hi:OnOff>
                      </abbr>
                    </li>
                </ul>
            </div>
        <div class="formitem validator1">
          <ul id="divContent">
            <li><span class="formitemtitle">商户号：</span>
              <asp:TextBox ID="txtPartner" CssClass="forminput form-control" runat="server" />
            </li>
               <li><span class="formitemtitle">上传证书：</span>
                  <asp:FileUpload ID="fileBankUnionCert" runat="server" CssClass="input-file" />
            </li>
             <li><span class="formitemtitle">证书文件名：</span>
              <asp:TextBox ID="txtCerFileName" CssClass="forminput form-control" runat="server" ReadOnly="true"/>
            </li> 
            <li><span class="formitemtitle">证书密码：</span>
              <asp:TextBox ID="txtKey" CssClass="forminput form-control" runat="server" />
            </li> 
<%--            <li><span class="formitemtitle">设置须知：</span>
                填写完成以上配置后，盛付通接口将自动启用
            </li>--%>
          </ul>
            <div class="ml_198">
                <asp:Button ID="btnOK" runat="server" Text="保 存" CssClass="btn btn-primary inbnt" OnClientClick="return PageIsValid();" onclick="btnOK_Click" />
            </div>
        </div>
      </div>
</div>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>



