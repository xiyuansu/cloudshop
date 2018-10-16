<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MarkingInfo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.Setting.MarkingInfo" %>
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
        }
        .carat { margin-right: 0!important; }
    </style>
     <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script> 
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
                               pictureSize: '40*40',
                               imagesCount: 1,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {            
            var srcImg = $('#imageContainer span[name="articleImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);
            if (srcImg == "") {
                ShowMsg("请上传图标");
                return false;
            }            
            if ($("#ctl00_contentHolder_ddlType").prop('selectedIndex')== 0) {
                ShowMsg("请选择跳转类型");
                return false;
            }
            return true;
        }
        $(document).ready(function () { initImageUpload();});
        </script>
     <asp:HiddenField ID="hidDisplaySequence" runat="server" />
     <asp:HiddenField ID="hidId" runat="server" />
    
 <div class="areacolumn clearfix" style="padding:0 25px;">
     <div class="columnright">
            <div class="formitem validator2">
                <ul>  
                    <li>
                        <span class=""><em>*</em>图标：</span>
                        <div id="imageContainer" style="width:200px;">
                            <span name="articleImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </div>
                    </li>      
                    <li class="mb_0"><span class=""><em>*</em>跳转至：</span>
                       <asp:DropDownList ID="ddlType" runat="server" CssClass="iselect">
                    </asp:DropDownList>
                        <p id="ctl00_contentHolder_txtTagName"></p>
                    </li>
                               
                </ul>
                
            </div>
         </div>
    </div>
          <div class="modal_iframe_footer">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return getUploadImages();" Text="添加" CssClass="btn btn-primary" />
                </div>
</asp:Content>
