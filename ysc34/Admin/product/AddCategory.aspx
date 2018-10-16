<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.AddCategory" MasterPageFile="~/Admin/Admin.Master" CodeBehind="~/Admin/product/AddCategory.aspx.cs" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>



<asp:Content ID="Content3" ContentPlaceHolderID="headHolder" runat="server">
   <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="ManageCategories.aspx">管理</a></li>
                <%--<li><a href="setcategorytemplate.aspx">分类模版</a></li>--%>
                <li class="hover"><a href="javascript:void">添加</a></li>
            </ul>
        </div>
        <div class="columnright">

            <div class="datafrom">
                <div class="formitem spfl">
                    <ul>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>分类名称：</span>
                            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control form_input_l" placeholder="不超过20个字符" MaxLength="20" />
                            <p id="ctl00_contentHolder_txtCategoryNameTip"></p>
                        </li>
                        <li><span class="formitemtitle">选择上级分类：</span>
                            <span class="formselect">
                                <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" CssClass="iselect" NullToDisplay="请选择上级分类" /></span>
                        </li>
                        <li><span class="formitemtitle">商品类型：</span>
                            <span class="formselect">
                                <Hi:ProductTypeDownList runat="server" ID="dropProductTypes" CssClass="iselect" NullToDisplay="请选择商品类型" /></span>
                        </li>

                        <li><span class="formitemtitle">分类小图标：</span>
                            <div id="imageContainer" style="width: 100px;"><%--这里只有上传一个图标，固定宽度100--%>
                                <span name="articleImage" class="imgbox"></span>
                                <asp:HiddenField ID="hidUploadImages" runat="server" />
                                <asp:HiddenField ID="hidOldImages" runat="server" />
                            </div>
                            <div id="divsmallTS" style="float:left;line-height:55px;">应用于移动端分类模板一前台显示</div>
                        </li>
                        <li id="libigImage"><span class="formitemtitle">分类大图标：</span>
                            <div id="mobileImageContainer" style="width: 100px;">
                                <span name="mobileImage" class="imgbox"></span>
                                <asp:HiddenField ID="hidUploadMobileImages" runat="server" />
                                <asp:HiddenField ID="hidOldMobileImages" runat="server" />
                            </div>
                            <div id="divbigTS" style="float:left;line-height:55px;">应用于移动端分类模板三和模板四的前台显示</div>
                            <%--<p style="margin-top: 10px; color: red;">用于移动端商品分类页</p>--%>
                        </li>
                        <li><span class="formitemtitle">货号前缀：</span>
                            <asp:TextBox ID="txtSKUPrefix" runat="server" CssClass="form_input_m form-control" MaxLength="5" placeholder="不超过5个字符，前缀只能是字母数字-和_" />
                        </li>
                        <li id="liURL" runat="server">
                            <span class="formitemtitle">URL重写名称：</span>
                            <asp:TextBox ID="txtRewriteName" runat="server" CssClass="form_input_l form-control" MaxLength="50" placeholder="" />
                        </li>
                        <li><span class="formitemtitle">搜索标题：</span>
                            <asp:TextBox ID="txtPageKeyTitle" runat="server" CssClass="form_input_l form-control" placeholder="不超过100个字符"></asp:TextBox>
                        </li>
                        <li><span class="formitemtitle">搜索关键字：</span>
                            <asp:TextBox ID="txtPageKeyWords" runat="server" CssClass="form_input_l form-control" placeholder="不超过160个字符" />
                        </li>
                        <li><span class="formitemtitle">搜索描述：</span>
                            <asp:TextBox ID="txtPageDesc" runat="server" CssClass="form_input_l form-control" placeholder="不超过240个字符" />
                        </li>
                        <li class="m_none"><span class="formitemtitle">分类广告：</span>
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
                </div>
                <div class="ml_198">
                    <asp:Button ID="btnSaveCategory" runat="server" OnClientClick="return getUploadImages();" Text="确 定" CssClass="btn btn-primary" />
                    <asp:Button ID="btnSaveAddCategory" runat="server" OnClientClick="return getUploadImages();" Text="保存并继续添加" CssClass="btn btn-primary ml_20" />
                </div>
            </div>
        </div>

    </div>
    <!-- start ImgPicker -->
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
                               imageDescript: '主要用于PC端展示',
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

        function Callback(value) {
            var liURL = document.getElementById("ctl00_contentHolder_liURL");
            var txtRewriteName = document.getElementById("ctl00_contentHolder_txtRewriteName");
            var txtSKUPrefix = document.getElementById("ctl00_contentHolder_txtSKUPrefix");

            if (value.length > 0) {
                liURL.style.display = "none";
                txtRewriteName.value = "";

                $.ajax({
                    url: "AddCategory.aspx",
                    type: 'post', dataType: 'json', timeout: 10000,
                    data: {
                        isCallback: "true",
                        parentCategoryId: value
                    },
                    async: false,
                    success: function (resultData) {
                        txtSKUPrefix.value = resultData.SKUPrefix;
                    }
                });
            }
            else {
                liURL.style.display = "";
                txtSKUPrefix.value = "";
            }

            //alert(value);
            if (value == null || value == "") {//说明没选择上一级分类 当前它是一级分类，则上传图标提示显示
                $("#divsmallTS").show();
                $("#divbigTS").show();
            } else {
                $("#divsmallTS").hide();
                $("#divbigTS").hide();
            }
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
            initValid(new InputValidator('ctl00_contentHolder_txtSKUPrefix', 1, 5, true, '(?!_)(?!-)[a-zA-Z0-9_-]+', '货号前缀长度限制在5个字符以内，前缀只能是字母数字-和_'))
            initValid(new InputValidator('ctl00_contentHolder_txtRewriteName', 0, 60, true, '([a-zA-Z])+(([a-zA-Z_-])?)+', 'URL重写长度限制在60个字符以内，必须为字母开头且只包含字母-和_'))
            initValid(new InputValidator('ctl00_contentHolder_txtPageKeyTitle', 0, 100, true, null, '长度限制在100个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtPageKeyWords', 0, 160, true, null, '让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在160个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtPageDesc', 0, 240, true, null, '告诉搜索引擎此分类浏览页面的主要内容，长度限制在240个字符以内'))
        }

        $(document).ready(function () {
            if ($("#ctl00_contentHolder_dropCategories").val() != "" && $("#ctl00_contentHolder_dropCategories").val() != " ") {
                document.getElementById("ctl00_contentHolder_liURL").style.display = "none";
            }
            initImageUpload();
            InitValidators();
            $("#ctl00_contentHolder_dropCategories").bind("change", function () {
                Callback($(this).attr("value"));

            });

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

