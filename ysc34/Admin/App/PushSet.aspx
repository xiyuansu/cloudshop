<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="PushSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.App.PushSet" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="dataarea mainwidth databody">
   
    </div>
    <div class="areacolumn clearfix">

        <div class="columnright">
            <div class="datafrom">
            <div class="formitem validator2"  style="padding-top:0px;">
                <ul>                    
                    <li>
                        <h2 class="colorE">配置</h2>
                        </li>
                    <li><span class="formitemtitle">AppId：</span> 
                        <asp:TextBox ID="txtAppId" runat="server" CssClass="forminput form-control"></asp:TextBox>    
                    </li>
                    <li><span class="formitemtitle">AppKey：</span>
                        <asp:TextBox ID="txtAppKey" runat="server" CssClass="forminput form-control"></asp:TextBox>   
                        <span style="padding:0 5px;"><a href="https://dev.getui.com/" target="_blank">获取AppKey Master Secret</a></span>                     
                    </li>
                    <li><span class="formitemtitle">Master Secret：</span>
                        <asp:TextBox ID="txtMasterSecret" runat="server" CssClass="forminput form-control"></asp:TextBox>                        
                    </li>
                     <li>
                        <h2 class="colorE">订单推送设置</h2>
                        </li>
                    <li><span class="formitemtitle">订单推送：</span>
                        <asp:CheckBox ID="cbEnableAppPushSetOrderSend" runat="server"  CssClass="icheck" Text="&nbsp;订单发货&nbsp;" />
                        <asp:CheckBox ID="cbEnableAppPushSetOrderRefund" runat="server" CssClass="icheck" Text="&nbsp;退款订单&nbsp;" />                        
                        <asp:CheckBox ID="cbEnableAppPushSetOrderReturn" runat="server" CssClass="icheck" Text="&nbsp;退货订单&nbsp;" />                        
                    </li>
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn btn-primary float" OnClick="btnSave_Click" />
                    </li>
                </ul>
            </div>
                </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

