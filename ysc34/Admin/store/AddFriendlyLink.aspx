<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddFriendlyLink.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddFriendlyLink" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <div class="areacolumn clearfix databody">
            <div class="title">
                <ul class="title-nav">
                    <li><a href="FriendlyLinks.aspx">管理</a></li>
                    <li  class="hover"><a href="javascript:void">添加</a></li>
                </ul>
            </div>
        <div class="columnright">
            <div class="formitem">
                <ul>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>网站名称：</span>
                        <asp:TextBox ID="txtaddTitle" runat="server" CssClass="form_input_l form-control" placeholder="长度限制在60个字符以内"></asp:TextBox>
                        <p id="txtaddTitleTip" runat="server"></p>
                    </li>
                    <li>
                        <span class="formitemtitle">友情链接Logo：</span>
                         <div id="imageContainer">
                        <span name="articleImage" class="imgbox"></span>
                        <asp:HiddenField ID="hidUploadImages" runat="server" />
                        <asp:HiddenField ID="hidOldImages" runat="server" />
                    </div>
                    </li>
                   
                    <li><span class="formitemtitle">网站地址：</span>
                        <asp:TextBox ID="txtaddLinkUrl" runat="server" CssClass="form_input_l form-control" placeholder="请输入带http的完整格式的URL地址"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle">是否显示：</span>
                        <Hi:OnOff ID="ooShowLinks" runat="server"></Hi:OnOff>
                    </li>                    
                </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnSubmitLinks" runat="server" Text="添 加" OnClientClick="return getUploadImages()" CssClass="btn btn-primary" />
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
            initValid(new InputValidator('ctl00_contentHolder_txtaddTitle', 0, 60, false, null, '网站名称不可为空，长度限制在60个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtaddLinkUrl', 0, 255, true, '^(http://).*(\.).*', '请输入带http的完整格式的URL地址'))
        }
        $(document).ready(function () { InitValidators(); initImageUpload(); });
    </script>
</asp:Content>
