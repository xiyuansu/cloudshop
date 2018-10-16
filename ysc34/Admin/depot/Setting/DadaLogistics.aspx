<%@ Page Title="达达物流设置" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DadaLogistics.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.Setting.DadaLogistics" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Register Src="~/Admin/Ascx/AccountNumbersTextBox.ascx" TagPrefix="uc1" TagName="AccountNumbersTextBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <link rel="stylesheet" href="../../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../../js/bootstrap-switch.js"></script>
    <script type="text/javascript">
        function fuCheckEnableDaDa(event, state) {
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
                <li><a href="Config.aspx">门店设置</a></li>
                <li><a href="MarketingImageList.aspx">营销图库</a></li>
                <li><a href="MarktingList.aspx">营销图标设置</a></li>
                <li><a href="TagList.aspx">门店标签设置</a></li>
                <li><a href="../StoreSetting.aspx">门店APP推送设置</a></li>
                <li><a href="StoreAppDonwload.aspx">门店APP下载设置</a></li>
                <li  class="hover"><a href="javascript:return false;">达达物流设置</a></li>
            </ul>
        </div>

         <div class="datafrom">
            <div class="formitem validator2">
                <ul>
                    <li>
                        <span class="formitemtitle">开启达达物流：</span>
                        <Hi:OnOff runat="server" ID="ooEnableHtmRewrite" ></Hi:OnOff>
                        <a  href="http://newopen.imdada.cn/#/helpCenter/file/dock?_k=riqns0" target="_blank" style=" margin-left:30px; margin-right:1px; cursor:pointer;">点击查看</a>已经开通的城市
                    </li>
                </ul>
            </div>
            <div class="formitem validator2">
                <ul id="divContent">
                    <li><span class="formitemtitle">app_key：</span>
                        <uc1:AccountNumbersTextBox ID="txtAppKey" CssClass="form_input_l form-control" runat="server" />
                    </li>
                    <li><span class="formitemtitle">app_secret：</span>
                        <uc1:AccountNumbersTextBox ID="txtAppSecret" CssClass="form_input_l form-control float" runat="server"/>
                        <a  href="http://newopen.imdada.cn/#/register?_k=sfilmi" target="_blank" style=" margin-left:5px; margin-right:1px; cursor:pointer;">注册开发者</a>，获取app_key和app_secret
                    </li>
                    <li><span class="formitemtitle">source_id：</span>
                        <uc1:AccountNumbersTextBox ID="txtSourceID" CssClass="form_input_l form-control float" runat="server" />
                        <a  href="http://newopen.imdada.cn/#/merchantRegister?_k=ze6t5y" target="_blank" style=" margin-left:5px; margin-right:1px; cursor:pointer;">注册商户</a>，获取商户ID
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button runat="server" ID="btnSave" Text="保 存" OnClick="btnSave_Click" OnClientClick="return getUploadImages();" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
