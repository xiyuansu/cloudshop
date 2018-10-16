<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddBrandCategory.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddBrandCategory" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
   <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="brandcategories.aspx">管理</a></li>        
                <%--<li><a href="SetBrandCategoryTemplate.aspx">模板设置</a></li>--%>
                <li class="hover"><a href="javascript:void" >添加</a></li>   
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li><span class="formitemtitle"><em>*</em>品牌名称：</span>
                        <asp:TextBox ID="txtBrandName" CssClass="form_input_l form-control" runat="server" MaxLength="15" placeholder="品牌名称不能为空，长度限制在15个字符以内" />
                    </li>
                    <li>
                        <span class="formitemtitle"><em>*</em>品牌Logo：</span>
                        <div id="imageContainer">
                            <span name="articleImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </div>
                    </li>
                    <li><span class="formitemtitle">品牌官方地址：</span>
                        <asp:TextBox ID="txtCompanyUrl" CssClass="form_input_l form-control" runat="server" placeholder="以http://开头，长度限制在100个字符以内" />
                    </li>
                    <li style="margin-bottom: 20px;"><span class="formitemtitle">URL重写名称：</span>
                        <asp:TextBox ID="txtReUrl" CssClass="form_input_l form-control" runat="server" />
                    </li>
                    <li ><span class="formitemtitle">搜索关键字：</span>
                        <asp:TextBox ID="txtkeyword" CssClass="form_input_l form-control" runat="server" placeholder="搜索关键字限制在160字符以内" />
                    </li>
                    <li ><span class="formitemtitle">关键字描述：</span>
                        <asp:TextBox ID="txtMetaDescription" CssClass="form_input_l form-control" runat="server" placeholder="描述限制在260字符以内" />
                    </li>
                    <li><span class="formitemtitle">品牌介绍：</span>
                        <span class="fl">
                        <Hi:Ueditor ID="fckDescription" runat="server" Width="660" />
                            </span>
                    </li>
                    <li><span class="formitemtitle">关联商品类型：</span>
                        <span style="float: left;" class="errortd">
                            <Hi:ProductTypesCheckBoxList runat="server" ID="chlistProductTypes" Width="100%" CssClass="icheck" /></span>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnSave" OnClientClick="return getUploadImages();" Text="保 存" CssClass="btn btn-primary" runat="server" />
                    <asp:Button ID="btnAddBrandCategory" OnClientClick="return getUploadImages();" Text="保存并继续添加" CssClass="btn btn-primary ml_20" runat="server" />
                </div>
            </div>

        </div>
    </div>
    <!-- start ImgPicker -->
     <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" language="javascript">
        // 初始化图片上传控件
        function initImageUpload() {
            var imgSrc = '<%=hidOldImages.Value%>';
            $('#imageContainer span[name="articleImage"]').hishopUpload(
                           {
                               title: '品牌logo',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: imgSrc,
                               imgFieldName: "articleImage",
                               defaultImg: '',
                               pictureSize: '450*150',
                               imagesCount: 1,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!PageIsValid()) return false;
            var srcImg = $('#imageContainer span[name="articleImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);
            return true;
        }

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtBrandName', 1, 15, false, null, '品牌名称不能为空，长度限制在15个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtCompanyUrl', 0, 100, true, '^(http)://.*', '品牌官方网站的网址必须以http://开头，长度限制在100个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtReUrl', 0, 60, true, '([a-zA-Z])+(([a-zA-Z_-])?)+', 'URL重写长度限制在60个字符以内，必须为字母开头且只包含字母-和_'))
            initValid(new InputValidator('ctl00_contentHolder_txtkeyword', 0, 160, true, null, '让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在160个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 1, 260, true, null, '长度限制在260个字符以内'));
            //initValid(new InputValidator('ctl00_contentHolder_txtDescription', 0, 300, true, null, '品牌介绍的长度限制在300个字符以内'));
        }
        $(document).ready(function () { InitValidators(); initImageUpload(); });
    </script>
</asp:Content>
