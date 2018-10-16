<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TagInfo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.Setting.TagInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
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
            overflow:hidden;
        }
        .carat { margin-right: 0!important; }
    </style>
       <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <script type="text/javascript">
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
                               pictureSize: '96*96',
                               imagesCount: 1,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!PageIsValid()) return false;
            var srcImg = $('#imageContainer span[name="articleImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);
            if (srcImg == "") {
                ShowMsg("请上传图标");
                return false;
            }
            return true;
        }
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtTagName', 1, 20, false, null, '标签名称不能为空，长度限制在20个字符以内'))
        }
        $(document).ready(function () { initImageUpload(); InitValidators(); });
        </script>
    <asp:HiddenField ID="hidTagId" runat="server" />
 <div class="areacolumn clearfix" style="padding:0 25px;">
     <div class="columnright">
            <div class="formitem validator2">
                <ul>  
                    <li>
                        <span class=""><em>*</em>标签图标：</span>
                        <div id="imageContainer" style="width:200px;">
                            <span name="articleImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </div>
                    </li>      
                    <li class="mb_0"><span class=""><em>*</em>标签名称：</span>
                        <asp:TextBox ID="txtTagName"  CssClass="form_input_m form-control"  MaxLength="20" runat="server" placeholder="长度限制在20个字符以内"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtTagName"></p>
                    </li>
                               
                </ul>
                
            </div>
         </div>
    </div>
          <div class="modal_iframe_footer">
                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAddTag" OnClientClick="return getUploadImages();" Text="添加" CssClass="btn btn-primary" />
                </div>
</asp:Content>
