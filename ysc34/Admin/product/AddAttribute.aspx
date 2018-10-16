<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddAttribute.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.AddAttribute" %>

<%@ Register TagPrefix="cc1" TagName="AttributeView" Src="~/Admin/product/ascx/AttributeView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="ProductTypes.aspx">商品类型</a></li>
                <li class="hover"><a href="javascript:void">添加新的商品类型</a></li>
            </ul>

        </div>
        <div class="columnright">
         <ul class="step_p_f">
                <li class="step_p_f_active_after">第一步：添加类型名称</li>
                <li class="step_p_f_active">第二步：添加扩展属性</li>
                <li>第三步：添加规格</li>
            </ul>
          
            <cc1:AttributeView runat="server" ID="attributeView" />
            <div class="Pg_15">
                <a class="btn btn-primary " href="<%=Hidistro.Core.Globals.GetAdminAbsolutePath("/product/AddSpecification.aspx?typeId=" + typeId.ToString()) %>">下一步</a>
            </div>
            <div class="left_from"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_attributeView_txtName', 1, 15, false, null, '扩展属性的名称，最多15个字符。'));
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>
