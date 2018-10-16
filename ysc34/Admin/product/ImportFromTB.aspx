<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ImportFromTB.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.ImportFromTB" Title="无标题页" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">从淘宝数据包导入</a></li>
                <li><a href="ImportFromLocal.aspx">从本地导入数据包</a></li>
                <%--<li><a href="ImportFromPP.aspx">从拍拍数据包导入</a></li>--%>
            </ul>

        </div>
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li>
                        <h2 class="colorE">数据包信息</h2>
                    </li>
                    <li>
                        <span class="formitemtitle">导入插件版本： </span>
                        <span class="formselect">
                            <asp:DropDownList runat="server" ID="dropImportVersions" CssClass="iselect"></asp:DropDownList></span>
                    </li>
                    <li>
                        <span class="formitemtitle">选择导入数据包文件： </span>
                        <span class="formselect">
                            <asp:DropDownList runat="server" ID="dropFiles" CssClass="iselect"></asp:DropDownList></span>

                        <p class="c-666" style="line-height: 1.4; margin-top: 5px; padding: 0;">
                            导入之前需要先将数据包文件上传到服务器上；
                如果上面的下拉框中没有您要导入的数据包文件，请先上传。
                        </p>

                    </li>
                    <li><span class="formitemtitle">&nbsp;</span>
                        <asp:FileUpload runat="server" ID="fileUploader" CssClass="forminput" />&nbsp;&nbsp;<asp:Button runat="server" ID="btnUpload" Text="上传" OnClientClick="return checkfrom()" CssClass="btn btn-primary" />

                        <p class="c-666" style="line-height: 1.4; margin-top: 5px; padding: 0;">
                            上传数据包须小于40M，否则可能上传失败，
                                您还可以使用FTP工具先将数据包上传到网站的/App_Data/data/taobao目录以后，再重新打开此页面操作。
                        </p>

                    </li>
                </ul>
                <ul>
                    <li>
                        <h2 class="colorE">导入选项</h2>
                    </li>
                    <li>
                        <span class="formitemtitle">商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="请选择商品分类" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li><span class="formitemtitle">商品品牌：</span>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="请选择品牌" CssClass="iselect"></Hi:BrandCategoriesDropDownList>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle">商品导入状态：</span>
                        <div class="icheck_group ">
                            <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus" Checked="true" Text="出售中" CssClass="icheck"></asp:RadioButton>
                            <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus" Text="下架区" CssClass="icheck"></asp:RadioButton>
                            <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus" Text="仓库中" CssClass="icheck"></asp:RadioButton>
                        </div>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnImport" runat="server" OnClientClick="return doImport();" CssClass="btn btn-primary" Text="导 入" />
                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">
        function checkform() {
            var version = $("#ctl00_contentHolder_dropImportVersions").val();
            if (version == "") {
                alert("请选择导入插件的版本");
                return false;
            }
            var file = $("#ctl00_contentHolder_fileUploader").val();
            if (file == null || file == "") {
                alert("请选择要上传的文件");
                return false;
            }
            return true;

        }
    </script>
</asp:Content>
