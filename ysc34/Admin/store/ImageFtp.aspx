<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ImageFtp" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <style>
        html {
            background: #fff !important;
        }

        body {
            padding: 0;
            width: 100%;
        }

        #mainhtml {
            margin: 0;
            padding: 20px 20px 70px 20px;
            width: 100% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <!--面包屑-->
    <div class="dataarea mainwidth databody">
        <div class="blockquote-default blockquote-tip mb_20">
            1.您一次最多可以上传10张图片。<br />
            2.请勿重复选择同一个图片文件。<br />
            3.图片文件的大小建议控制在500KB以内，图片太大会影响网站打开速度
        </div>

        <div class="datalist clearfix">
            <div class="searcharea clearfix br_search">
                <ul>
                    <li><span>上传到：</span><Hi:ImageDataGradeDropDownList ID="dropImageFtp" runat="server" CssClass="iselect" /></li>
                </ul>
            </div>
            <%--<div class="imageDataRight">
                <div class="borderthin">
                    <ul class="RightHead">图片分类:</ul>
                    <Hi:ImageTypeLabel runat="server" ID="ImageTypeID" />
                    <ul class="pad10"><a href="<%= Globals.GetAdminAbsolutePath("/store/ImageType.aspx")%>" class="btn btn-primary" style="display: block; text-align: center;">分类管理</a></ul>
                </div>
            </div>--%>
            <div class="imageDataLeft " style="width:100%;">
                 <div id="imageContainer" style="margin-top: 30px">
                                <span name="articleImage" class="imgbox"></span>
                                <asp:HiddenField ID="hidUploadImages" runat="server" />
                                <asp:HiddenField ID="hidOldImages" runat="server" />
                            </div>
         

            </div>
        </div>
        <div class="modal_iframe_footer">
            <asp:Button ID="btnSaveImageFtp" OnClientClick="return getUploadImages()" runat="server" Text="确定上传" CssClass="btn btn-primary" />

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            initImageUpload();
        });
        // 初始化图片上传控件
        function initImageUpload() {
            var imgSrc = '<%=hidOldImages.Value%>';
            $('#imageContainer span[name="articleImage"]').hishopUpload(
                           {
                               title: '',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: imgSrc,
                               imgFieldName: "articleImage",
                               defaultImg: '',
                               pictureSize: '',
                               imagesCount: 10,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            var aryImgs = $('#imageContainer span[name="articleImage"]').hishopUpload("getImgSrc");
            var imgSrcs = "";
            $(aryImgs).each(function () {
                imgSrcs += this + ",";
            });
            $("#<%=hidUploadImages.ClientID%>").val(imgSrcs);
            return true;
        }

    </script>
</asp:Content>
