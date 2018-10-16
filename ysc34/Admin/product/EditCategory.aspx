<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditCategory" MasterPageFile="~/Admin/Admin.Master" CodeBehind="~/Admin/product/EditCategory.aspx.cs" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody areacolumn">

        <div class="title">
            <ul class="title-nav">
                <li><a href="ManageCategories.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">编辑</a></li>
            </ul>
        </div>
        <div class="columnright">
            <div class="formitem validator1">
                <ul>
                    <li class="mb_0"><span class="formitemtitle "><em>*</em>分类名称：</span>
                        <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control  form_input_l" placeholder="分类名称不能为空，在1至20个字符之间" MaxLength="20" />
                        <p id="ctl00_contentHolder_txtCategoryNameTip"></p>
                    </li>
                    <li><span class="formitemtitle ">商品类型：</span>
                        <span class="formselect">
                            <Hi:ProductTypeDownList runat="server" ID="dropProductTypes" NullToDisplay="请选择类型" CssClass="iselect" /></span>
                    </li>

                    <li><span class="formitemtitle">分类图标：</span>
                        <div id="imageContainer" style="width: 100px;"><%--这里只有上传一个图标，固定宽度100--%>
                            <span name="articleImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </div>
                        <div id="divsmallTS" runat="server" style="float:left;line-height:55px;">应用于移动端分类模板一前台显示</div>
                    </li>
                    <li id="libigImage" style="display: none;" runat="server"><span class="formitemtitle">分类大图标：</span>
                        <div id="mobileImageContainer" style="width: 100px;">
                            <span name="mobileImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadMobileImages" runat="server" />
                            <asp:HiddenField ID="hidOldMobileImages" runat="server" />
                        </div>
                        <div id="divbigTS" runat="server" style="float:left;line-height:55px;">应用于移动端分类模板三和模板四的前台显示</div>
                        <%--<div class="Items">( 用于移动端商品分类页 )</div>--%>
                    </li>

                    <li runat="server" id="liParentCategroy"><span class="formitemtitle ">货号前缀：</span>
                        <asp:TextBox ID="txtSKUPrefix" runat="server" CssClass="form-control  form_input_m" placeholder="货号前缀长度限制在5个字符以内，必须为字母数字" />
                    </li>
                    <li runat="server" id="liRewriteName">
                        <span class="formitemtitle ">URL重写名称：</span>
                        <asp:TextBox ID="txtRewriteName" runat="server" CssClass="form-control  form_input_l" MaxLength="50" />
                    </li>
                    <li><span class="formitemtitle ">搜索标题：</span>
                        <asp:TextBox ID="txtPageKeyTitle" runat="server" CssClass="form-control  form_input_l" placeholder="长度限制在100字符以内"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle ">搜索关键字：</span>
                        <asp:TextBox ID="txtPageKeyWords" runat="server" CssClass="form-control  form_input_l" placeholder="长度限制在160字符以内" />
                    </li>
                    <li><span class="formitemtitle ">搜索描述：</span>
                        <asp:TextBox ID="txtPageDesc" runat="server" CssClass="form-control  form_input_l" placeholder="长度限制在240个字符以内" />
                    </li>

                    <li class="m_none"><span class="formitemtitle ">分类广告：</span>
                        <span class="tab">
                            <div class="status">
                                <ul>
                                    <li style="clear: none;"><a onclick="ShowNotes(1)" id="tip1" style="cursor: pointer">分类广告1</a></li>
                                    <li style="clear: none;"><a onclick="ShowNotes(2)" id="tip2" style="cursor: pointer">分类广告2</a></li>
                                    <li style="clear: none;"><a onclick="ShowNotes(3)" id="tip3" style="cursor: pointer">分类广告3</a></li>
                                </ul>
                            </div>
                        </span>
                        <span class="ml_198 mt_0">
                            <div id="notes1">
                                <Hi:Ueditor ID="fckNotes1" runat="server" Width="660" />
                            </div>
                            <div id="notes2" style="display: none;">
                                <Hi:Ueditor ID="fckNotes2" runat="server" Width="660" ImportLib="1" />
                            </div>
                            <div id="notes3" style="display: none;">
                                <Hi:Ueditor ID="fckNotes3" runat="server" Width="660" ImportLib="1" />
                            </div>
                        </span>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnSaveCategory" runat="server" OnClientClick="return getUploadImages();" Text="保 存" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>

    </div>
      <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
  
    <style type="text/css">
        .off {
            color: #741f0b;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript" language="javascript">
        // 初始化图片上传控件
        function initImageUpload() {
            var imgSrc = '<%=hidOldImages.Value%>';
            $('#imageContainer span[name="articleImage"]').hishopUpload(
                           {
                               title: '分类图标',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: imgSrc,
                               imgFieldName: "articleImage",
                               defaultImg: '',
                               pictureSize: '200*200',
                               imagesCount: 1,
                               dataWidth: 9
                           });

            var imgSrc2 = '<%=hidOldMobileImages.Value%>';
            $('#mobileImageContainer span[name="mobileImage"]').hishopUpload(
                           {
                               title: '分类大图标',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: imgSrc2,
                               imgFieldName: "mobileImage",
                               defaultImg: '',
                               pictureSize: '640*240',
                               imagesCount: 1,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!PageIsValid()) return false;
            var srcImg = $('#imageContainer span[name="articleImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);

            var srcImg2 = $('#mobileImageContainer span[name="mobileImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadMobileImages.ClientID%>").val(srcImg2);
            return true;
        }

        function ShowNotes(index) {

            document.getElementById("notes1").style.display = "none";
            document.getElementById("notes2").style.display = "none";
            document.getElementById("notes3").style.display = "none";
            var notesId = "notes" + index;
            document.getElementById(notesId).style.display = "block";

            document.getElementById("tip1").className = "";
            document.getElementById("tip2").className = "";
            document.getElementById("tip3").className = "";
            var tipId = "tip" + index;
            document.getElementById(tipId).className = "off"
        }

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtCategoryName', 1, 20, false, null, '分类名称不能为空，长度限制在20个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtSKUPrefix', 1, 5, true, '(?!_)(?!-)[a-zA-Z0-9_-]+', '货号前缀长度限制在5个字符以内，必须为字母数字'))
            initValid(new InputValidator('ctl00_contentHolder_txtRewriteName', 0, 60, true, '([a-zA-Z])+(([a-zA-Z_-])?)+', 'URL重写长度限制在60个字符以内，必须为字母开头且只包含字母-和_'))
            initValid(new InputValidator('ctl00_contentHolder_txtPageKeyTitle', 0, 100, true, null, '长度限制在100个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtPageKeyWords', 0, 160, true, null, '让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在160个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtPageDesc', 0, 240, true, null, '告诉搜索引擎此分类浏览页面的主要内容，长度限制在240个字符以内'))
        }
        $(document).ready(function () {
            InitValidators();
            initImageUpload();

            $("#ctl00_contentHolder_dropCategories").change(function () {
                var pid = $(this).find("option:selected").val();
                if (pid == "") {
                    $("#libigImage").show();
                } else {
                    $("#libigImage").hide();
                }
            });
        });
    </script>
</asp:Content>

