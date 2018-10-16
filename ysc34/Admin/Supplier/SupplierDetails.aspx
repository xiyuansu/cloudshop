<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Admin/Admin.Master" CodeBehind="SupplierDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.SupplierDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .areacolumn .columnright .formdetail li{margin-bottom:15px;font-size:14px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="SupplierList.aspx">管理</a></li>
                <li  class="hover">
                    <a href="javascript:void">基本信息</a></li>
            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">

        <div class="columnright">


            <div class="formitem clearfix formdetail">
                <ul>
                    <li><span class="formitemtitle">用户名：</span><asp:Literal runat="server" ID="lblUserName" /></li>
                    <li><span class="formitemtitle">供应商名称：</span><asp:Literal runat="server" ID="lblSupplierName" /></li>
                    <li><span class="formitemtitle">状态：</span><asp:Literal runat="server" ID="lblStatus" /></li>
                    <li><span class="formitemtitle">联系人：</span><asp:Literal runat="server" ID="lblContactMan" /></li>
                    <li><span class="formitemtitle">联系电话：</span><asp:Literal runat="server" ID="lblTel" /></li>
                    <%--<li><span class="formitemtitle">所在区域：</span><asp:Literal runat="server" ID="lblRegions" /></li>--%>
                    <li><span class="formitemtitle">详细地址：</span><asp:Literal runat="server" ID="lblAddress" /></li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>