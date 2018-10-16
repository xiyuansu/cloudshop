<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.WAPShop.BasicConfig" CodeBehind="BasicConfig.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>支付宝收款信息设置</h1>
        <span>请设置好您的支付宝信息。 还没开通支付宝？ <a target="_blank" href="https://b.alipay.com/order/productDetail.htm?productId=2013080604609688">立即免费申请开通支付宝接口</a></span>
      </div>
      <div class="datafrom">
        <div class="formitem validator1">
          <ul>
            <li><span class="formitemtitle Pw_100">商家号：</span>
              <asp:TextBox ID="txtPartner" CssClass="forminput formwidth" runat="server" />
            </li>
            <li><span class="formitemtitle Pw_100">商家密钥：</span>
              <asp:TextBox ID="txtKey" CssClass="forminput formwidth" runat="server" />
            </li> 
            <li><span class="formitemtitle Pw_100">收款账户：</span>
              <asp:TextBox ID="txtAccount" CssClass="forminput formwidth" runat="server" />
            </li> 
            <li><span class="formitemtitle Pw_100">设置须知：</span>
                填写完成以上配置后，支付宝接口将自动启用
            </li>
          </ul>
          <div style="clear:both"></div>
           <ul class="btntf Pa_198">
            <asp:Button ID="btnOK" runat="server" Text="保 存" CssClass="submit_DAqueding inbnt" 
                   OnClientClick="return PageIsValid();" onclick="btnOK_Click" />
	       </ul>
        </div>
      </div>
</div>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

