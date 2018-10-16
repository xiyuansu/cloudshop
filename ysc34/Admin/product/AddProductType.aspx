<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddProductType.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.AddProductType" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">

        <div class="title">
            <ul class="title-nav">
                <li><a href="ProductTypes.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">添加</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <ul class="step_p_f">
                <li class="step_p_f_active">第一步：添加类型名称</li>
                <li >第二步：添加扩展属性</li>
                <li>第三步：添加规格</li>
            </ul>
           
            <div class="formitem">
                <ul>
                    <li><span class="formitemtitle"><em>*</em>商品类型名称：</span>
                        <asp:TextBox ID="txtTypeName" CssClass="form-control forminput" runat="server" Width="320" placeholder="长度限制在1-30个字符之间"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle">关联品牌：</span>
                        <span>
                            <Hi:BrandCategoriesCheckBoxList runat="server" ID="chlistBrand" CssClass="icheck icheck-label-5-10" /></span>
                    </li>
                    <li><span class="formitemtitle">备注：</span>
                        <asp:TextBox ID="txtRemark" TextMode="MultiLine" Width="320" Height="90" CssClass="fl form-control" runat="server" placeholder="备注的长度限制在0-100个字符之间"></asp:TextBox>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnAddProductType" runat="server" OnClientClick="return PageIsValid();" Text="下一步" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtTypeName', 1, 30, false, null, '商品类型名称不能为空，长度限制在1-30个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtRemark', 0, 300, true, null, '备注的长度限制在0-100个字符之间'))
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>
