<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="StoreAppDonwload.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.StoreAppDonwload" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
     <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="Config.aspx">门店设置</a></li>
                <li><a href="MarketingImageList.aspx">营销图库</a></li>
                <li><a href="MarktingList">营销图标设置</a></li>
                <li><a href="TagList.aspx">门店标签设置</a></li>
                <li><a href="../StoreSetting.aspx">门店APP推送设置</a></li>
                <li  class="hover"><a href="#">门店APP下载设置</a></li>
                <li><a href="../../store/DadaLogistics.aspx">达达物流设置</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem validator1">
                <ul class="content">
                    <li><span class="formitemtitle">版本号：</span>
                        <asp:Literal ID="litVersion" runat="server" Text="1.00" />
                        <input type="hidden" id="hidIsForcibleUpgrade" runat="server" />
                    </li>
                    <li><span class="formitemtitle">版本描述：</span>
                        <asp:Literal ID="litDescription" runat="server" Text="初始版本" />
                    </li>
                    <li><span class="formitemtitle">安卓APP：</span>
                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="input-file" />
                        <p runat="server" class="pl0">上传了升级包之后将会覆盖原来的版本，上传前请确认上传的更新包版本比当前版本新</p>
                        <p class="downloadurl" id="txtAndroidDownloadUrl" runat="server"></p>
                    </li>

                    <li><span class="formitemtitle"><em></em>苹果APP：</span>
                        <asp:TextBox ID="txtIosDownloadUrl" runat="server" CssClass="forminput form-control" />
                        <p runat="server">苹果版本下载地址</p>
                    </li>
                </ul>
                <ul>
                    <li><span class="formitemtitle">&nbsp;</span>
                        <asp:Button ID="btnUpoad" runat="server" Text="保   存" CssClass="btn btn-primary" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
