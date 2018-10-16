<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditArticle" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
     <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="areacolumn clearfix databody">
            <div class="title">
                <ul class="title-nav">
                    <li><a href="ArticleList.aspx">文章管理</a></li>
                    <li class="hover"><a href="javascript:void">编辑</a></li>
                </ul>
            </div>
        <div class="columnright">
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle"><em>*</em>所属分类：</span><abbr class="formselect">
                        <Hi:ArticleCategoryDropDownList ID="dropArticleCategory" AllowNull="true" runat="server" CssClass="iselect"  NullToDisplay="请选择所属分类"></Hi:ArticleCategoryDropDownList>
                    </abbr>
                        <a class="colorBlue ml_10" href="javascript:DialogFrame('comment/AddArticleCategory.aspx?source=add','添加文章分类',null,null,function(e){location.reload();});">添加文章分类 </a>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>主题：</span>
                        <asp:TextBox ID="txtArticleTitle" runat="server" CssClass="form_input_m form-control" placeholder="限制在60个字符以内"></asp:TextBox>
                         <p id="ctl00_contentHolder_txtArticleTitleTip"></p>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>是否立即发布：</span>
                        <Hi:OnOff runat="server" ID="ooRelease"></Hi:OnOff>
                    </li>
                    <li><span class="formitemtitle">搜索描述：</span>
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtMetaDescription" placeholder="限制在260个字符以内" />
                    </li>
                    <li><span class="formitemtitle">搜索关键字：</span>
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtMetaKeywords" placeholder="限制在160个字符以内" />
                    </li>

                    <li>
                        <span class="formitemtitle">文章图标：</span>
                        <div id="imageContainer">
                            <span name="articleImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </div>
                    </li>

                    <li><span class="formitemtitle">摘要：</span>
                        <asp:TextBox ID="txtShortDesc" TextMode="MultiLine" CssClass="form_input_l form-control" Height="70" runat="server"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtShortDescTip"></p>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>内容：</span>
                        <span>
                            <Hi:Ueditor ID="fcContent" runat="server" Width="660" />
                        </span>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnAddArticle" runat="server" OnClientClick="return getUploadImages();" Text="保 存" CssClass="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

     <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    
    <script type="text/javascript" language="javascript">
        // 初始化图片上传控件
        function initImageUpload() {
            var imgSrc = '<%=hidOldImages.Value%>';
            $('#imageContainer span[name="articleImage"]').hishopUpload(
                           {
                               title: '文章图标',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: imgSrc,
                               imgFieldName: "articleImage",
                               defaultImg: '',
                               pictureSize: '220*130',
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

            initValid(new InputValidator('ctl00_contentHolder_txtArticleTitle', 1, 60, false, null, '文章标题不能为空，长度限制在60个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtShortDesc', 0, 300, true, null, '文章摘要的长度限制在300个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 1, 260, true, null, '搜索描述不能为空，长度限制在260个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtMetaKeywords', 0, 160, true, null, '搜索关键字的长度限制在160个字符以内'))
        }
        $(document).ready(function () { InitValidators(); initImageUpload(); });
    </script>

</asp:Content>

