<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditAttribute.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.EditAttribute" %>

<%@ Register TagPrefix="cc1" TagName="AttributeView" Src="~/Admin/product/ascx/AttributeView.ascx" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody areacolumn clearfix">

        <div class="title">
            <ul class="title-nav">
                <li><a href="ProductTypes.aspx">商品类型</a></li>
                <li><a href='<%= Globals.GetAdminAbsolutePath("/product/EditProductType.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>基本设置</a></li>
                <li  class="hover"><a href="javascript:void">扩展属性</a></li>
                <li><a href='<%= Globals.GetAdminAbsolutePath("/product/EditSpecification.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>规 格</a></li>
            </ul>           
        </div>
        <div class="columnright">
            <cc1:AttributeView runat="server" ID="attributeView" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
