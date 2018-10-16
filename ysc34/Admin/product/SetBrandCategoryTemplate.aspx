<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetBrandCategoryTemplate.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetBrandCategoryTemplate" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="brandcategories.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">模板设置</a></li>
                <li><a href="AddBrandCategory.aspx">添加</a></li>
            </ul>

        </div>

        <div class="searcharea bd_0 mb_0">
            <ul>
                <li>
                    <span style="float: left">上传分类模板文件：</span>
                    <asp:FileUpload ID="fileThame" runat="server" CssClass="forminput" />
                    <asp:Button runat="server" ID="btnUpload" Text="上传" CssClass="btn btn-primary fl ml_20"></asp:Button>
                    <span style="color: Red; float: left; margin-left: 5px;">(上传文件必须为html格式)</span>
                </li>
                <li>
                    <span style="float: left">删除分类模板文件：</span>
                    <asp:DropDownList runat="server" ID="dropThmes" CssClass="iselect" />
                    <Hi:ImageLinkButton runat="server" ID="btnDelete" Text="删除" DeleteMsg="请确认是否删除模板" CssClass="btn btn-danger ml_20" />
                </li>
            </ul>
        </div>
        <div class="datalist clearfix">

            <table class="table table-striped" cellspacing="0" border="0" style="width: 100%; border-collapse: collapse;">
                <thead>
                    <tr>
                        <th scope="col" style="height: 50px; width: 40%;">品牌名称</th>
                        <th scope="col">套用的模板</th>
                        <th scope="col">操作</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rptBrandes" OnItemCommand="rptBrandes_ItemCommand" OnItemDataBound="rptBrandes_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td height="50"><%# Eval("BrandName") %></td>
                                <td>
                                    <asp:TextBox ID="hidBrandId" runat="server" CssClass="hide" Text='<%#Eval("BrandId") %>'></asp:TextBox>
                                    <asp:DropDownList runat="server" ID="dropTheme" CssClass="iselect" /></td>
                                <td><span class="submit_shanchu">
                                    <asp:LinkButton ID="lblSave" CommandName="Save" runat="server" Text="保存模板设置" /></span></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <div style="padding-left: 12px">
            <asp:Button ID="btnSaveAll" runat="server" Text="批量保存设置" CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Content>
