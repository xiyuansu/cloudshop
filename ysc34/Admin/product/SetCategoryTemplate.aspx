<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetCategoryTemplate.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetCategoryTemplate" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="managecategories.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">分类模版</a></li>
                <li><a href="AddCategory.aspx">添加</a></li>
            </ul>
        </div>
        <div class="searcharea bd_0 mb_0">
            <ul>
                <li style="width: 100%;">
                    <span style="float: left">上传分类模板文件：</span>
                    <asp:FileUpload ID="fileThame" runat="server" Width="180px" CssClass="forminput" Style="_margin-top: 5px; *margin-top: 5px;" />
                    <asp:Button runat="server" ID="btnUpload" Style="margin-left: 5px;" CssClass="btn btn-primary ml_20 fl" Text="上传"></asp:Button>
                    <span style="color: Red; margin-left: 5px;">(上传文件必须为html格式)</span>
                </li>
                <li>
                    <span style="float: left">删除分类模板文件：</span>
                    <asp:DropDownList runat="server" ID="dropThmes" CssClass="iselect" />
                    <Hi:ImageLinkButton runat="server" ID="btnDelete" Text="删除" CssClass="btn btn-danger ml_20" DeleteMsg="请确认是否删除模板" />
                </li>
            </ul>
        </div>



        <div>
            <table class="table table-striped" cellspacing="0" border="0" style="width: 100%; border-collapse: collapse;">
                <thead>
                    <tr>
                        <th scope="col" style="height: 50px; width: 40%;">分类名称</th>
                        <th scope="col">套用的模板</th>
                        <th scope="col">操作</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rptCategries" OnItemCommand="rptCategries_ItemCommand" OnItemDataBound="rptCategries_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                            <td height="50"><%# Eval("Name") %></td>
                            <td>
                                <asp:TextBox ID="hidCategoryId" runat="server" CssClass="hide" Text='<%#Eval("CategoryId") %>'></asp:TextBox>
                                <asp:DropDownList runat="server" ID="dropTheme" CssClass="iselect" /></td>
                            <td><span class="submit_shanchu">
                                <asp:LinkButton ID="lblSave" CommandName="Save" runat="server" Text="保存模板设置" /></span></td>
                                </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <div class="Pw_198">
            <asp:Button ID="btnSaveAll" runat="server" Text="批量保存设置" CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Content>
