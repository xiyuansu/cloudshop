<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="RegisterHiPOSFinished.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.RegisterHiPOSFinished" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript">
        window.history.go(1);
    </script>
    <div class="dataarea mainwidth databody">
 

        <div class="datafrom">
            <div class="formitem validator1">
                 <div class="step" style="margin-bottom: 30px;">
                    <span>1.注册商户</span>
                    <span >2.设置支付</span>
                    <span class="setp_hover mr0">3.完成</span>
                </div>
                <p class="sethipos_f">基本设置已完成<a href="BindHiPOS.aspx">现在去绑定设备</a></p>
            </div>
        </div>
    </div>



</asp:Content>
