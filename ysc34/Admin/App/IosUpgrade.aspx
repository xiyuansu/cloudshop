<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="IosUpgrade.aspx.cs" Inherits="Hidistro.UI.Web.Admin.IosUpgrade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
       <div class="title">
            <ul class="title-nav">
                <li ><a href="AndroidUpgrade.aspx">安卓升级</a></li>
                <li><a  class="hover">ios升级</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                   
                    <li><span class="formitemtitle"><em>*</em>下载地址：</span>
                        <asp:TextBox ID="txtDownloadUrl" runat="server" CssClass="forminput form-control" />
                        <p runat="server">请填写APP的下载地址</p>
                    </li>
                    <li><span class="formitemtitle">&nbsp;</span>
                        <asp:Button ID="btnUpoad" runat="server" Text="保   存" CssClass="btn btn-primary" />
                    </li>
                    <li class="hidden">
                        <h2 class="colorE">当前版本信息</h2>
                    </li>
                    <li class="hidden"><span class="formitemtitle">版本号：</span>
                        <asp:Literal ID="litVersion" runat="server" Text="1.00" />
                        <input type="hidden" id="hidIsForcibleUpgrade" runat="server" />
                    </li>
                    <li class="hidden"><span class="formitemtitle">版本描述：</span>
                        <asp:Literal ID="litDescription" runat="server" Text="初始版本" />
                    </li>
                    <li class="hidden"><span class="formitemtitle">链接地址：</span>
                        <asp:Literal ID="litUpgradeUrl" runat="server" Text="" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
