<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="StoreSetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.StoreSetting" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a  href="Setting/Config.aspx">门店设置</a></li>
                <li><a  href="Setting/MarketingImageList.aspx">营销图库</a></li>
                <li><a  href="Setting/MarktingList.aspx">营销图标设置</a></li>
                <li><a  href="Setting/TagList.aspx">门店标签设置</a></li>
                <li class="hover"><a  href="javascript:return false;">门店APP推送设置</a></li>
                <li><a href="Setting/StoreAppDonwload.aspx">门店APP下载设置</a></li>
                <li><a href="../store/DadaLogistics.aspx">达达物流设置</a></li>
            </ul>             
        </div>

        <div class="datafrom">
            <div class="formitem">
                <%--<ul>
                    <li>
                        <h2 class="colorE">订单分配设置</h2>
                        </li>
                    <li><span class="formitemtitle">订单自动分配到门店：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radAutoAllotOrder"></Hi:OnOff>
                        </abbr>
                        <p>开启时，订单根据配送范围自动匹配到门店。<br />若关闭，则订单由平台手动匹配。</p>
                    </li>
                </ul>--%>
                  <ul>
                    <li>
                        <h2 class="colorE">门店APP推送设置</h2>
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
                </ul>
                <asp:Button runat="server" ID="btnSave" Text="保 存" OnClick="btnSave_Click" CssClass="btn btn-primary ml_198" />
            </div>
        </div>
    </div>
</asp:Content>
