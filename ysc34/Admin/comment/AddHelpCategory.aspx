<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddHelpCategory.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddHelpCategory" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>





<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
   <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
     <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
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
            padding: 20px 20px 60px 20px;
            width: 100% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="formitem  validator3">
                <ul>
                    <li class="mb_0"><span class="formitemtitle "><em>*</em>分类名称：</span>
                        <asp:TextBox ID="txtHelpCategoryName" CssClass="form_input_m form-control" runat="server" placeholder="长度限制在60个字符以内"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtHelpCategoryNameTip"></p>
                    </li>
                    <li>
                        <span class="formitemtitle ">分类图标：</span>
                        <div id="imageContainer" style="width:300px;">
                            <span name="articleImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </div>
                    </li>

                    <li><span class="formitemtitle ">显示在底部帮助：</span>
                        <Hi:OnOff runat="server" ID="ooShowFooter"></Hi:OnOff>
                    </li>
                    <li><span class="formitemtitle ">分类介绍：</span>
                        <asp:TextBox ID="txtHelpCategoryDesc" TextMode="MultiLine" CssClass="form_input_l form-control" Height="100"  runat="server" placeholder="最多只能输入300个字符"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtHelpCategoryDescTip"></p>
                    </li>
                </ul>

            </div>

        </div>
    </div>
    <div class="modal_iframe_footer">
        <asp:Button ID="btnSubmitHelpCategory" runat="server" OnClientClick="return getUploadImages();" Text="添加" CssClass="btn btn-primary" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
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
            initValid(new InputValidator('ctl00_contentHolder_txtHelpCategoryName', 1, 60, false, null, '分类名称不能为空，长度限制在60个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtHelpCategoryDesc', 0, 300, true, null, '分类介绍最多只能输入300个字符'))
        }
        $(document).ready(function () { InitValidators(); initImageUpload(); });
    </script>
</asp:Content>
