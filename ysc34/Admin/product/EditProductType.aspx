<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditProductType.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.EditProductType" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody areacolumn clearfix">
        <div class="title">
            <ul class="title-nav">
                <li><a href="ProductTypes.aspx">商品类型</a></li>
                <li class="hover"><a href="javascript:void" >基本设置</a></li>
                <li><a href='<%= Globals.GetAdminAbsolutePath("/product/EditAttribute.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>扩展属性</a></li>
                <li><a href='<%= Globals.GetAdminAbsolutePath("/product/EditSpecification.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>规 格</a></li>
            </ul>          
        </div>
        <!--<ul class="step_p_f">
                <li class="step_p_f_active">第一步：添加类型名称</li>
                <li >第二步：添加扩展属性</li>
                <li>第三步：添加规格</li>
        </ul>-->
        <div class="columnright pt0">
            <div class="formitem validator2">
                <ul>
                    <li class="mb_0"><span class="formitemtitle">商品类型名称：</span>
                        <asp:TextBox ID="txtTypeName" CssClass="forminput form-control" runat="server" Width="320" placeholder="商品类型名称不能为空，长度限制在1-30个字符之间"></asp:TextBox>
                        <p id="txtTypeNameTip" runat="server"></p>
                    </li>
                    <li><span class="formitemtitle">关联品牌：</span>
                        <span>
                            <Hi:BrandCategoriesCheckBoxList runat="server" ID="chlistBrand" CssClass="icheck icheck-label-5-10" /></span>
                    </li>
                    <li><span class="formitemtitle">备注：</span>
                        <asp:TextBox ID="txtRemark" TextMode="MultiLine" Width="320" Height="90" runat="server" CssClass="fl form-control"></asp:TextBox>
                        
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnEditProductType" runat="server" OnClientClick="return PageIsValid();" Text="保 存" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtTypeName', 1, 30, false, null, '商品类型名称不能为空，长度限制在1-30个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtRemark', 0, 100, true, null, '备注的长度限制在0-100个字符之间'))
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>
