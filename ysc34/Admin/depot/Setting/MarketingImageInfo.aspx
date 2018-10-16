<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MarketingImageInfo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.MarketingImageAdd" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        html { background: #fff !important; }
        body { padding: 0; width: 100%; }
        #mainhtml { margin: 0; padding: 20px 20px 60px 20px; width: 100% !important; }
        .carat { margin-right: 0 !important; }
    </style>
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
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
                               pictureSize: '640*400',
                               imagesCount: 1,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            var srcImg = $('#imageContainer span[name="articleImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);
            if (srcImg == "") {
                ShowMsg("请上传图片", false);
                return false;
            }
            if ($("#txtImageName").val() == "" || $("#txtImageName").val().trim().length > 20) {
                ShowMsg("请输入图片名称，并且名称长度不能大于20个字符", false);
                return false;
            }

            if ($("#txtDescription").val() != "" && $("#txtDescription").val().trim().length > 100) {
                ShowMsg("使用说明的长度不能大于100个字符", false);
                return false;
            }
            return true;
        }
        $(document).ready(function () { initImageUpload(); });
    </script>
    <asp:HiddenField ID="hidImageId" runat="server" />
    <div class="areacolumn clearfix" style="padding: 0 25px;">
        <div class="columnright">
            <div class="formitem validator2">
                <ul>
                    <li><span><em>*</em>图片名称：</span>
                        <Hi:TrimTextBox runat="server" ID="txtImageName" CssClass="form_input_m form-control" MaxLength="20" placeholder="图片名称20个字符以内" ClientIDMode="static"></Hi:TrimTextBox>
                    </li>
                    <li>
                        <span class=""><em>*</em>上传图片：</span>
                        <div id="imageContainer" style="width: 200px;">
                            <span name="articleImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </div>
                    </li>
                    <li><span>使用说明：</span>
                        <asp:TextBox TextMode="MultiLine" Rows="5" ID="txtDescription" MaxLength="100" runat="server" placeholder="使用说明长度100个字符以内" ClientIDMode="static" CssClass="forminput form-control"></asp:TextBox>
                    </li>

                </ul>

            </div>
        </div>
    </div>
    <div class="modal_iframe_footer">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return getUploadImages();" Text="添加" CssClass="btn btn-primary" />
    </div>
</asp:Content>
