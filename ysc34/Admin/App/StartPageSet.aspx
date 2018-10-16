<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="StartPageSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.StartPageSet" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
     <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            var srcAndroidImage = $("#<%=hfAndroidImage.ClientID%>").val();
            var srcIOSImage = $("#<%=hfIOSImage.ClientID%>").val();            
            
            $('#logoContainer span[name="AndroidImage"]').hishopUpload(
                          {
                              title: '上传Android图片',
                              url: "/admin/UploadHandler.ashx?action=newupload",
                              displayImgSrc: srcAndroidImage,
                              imgFieldName: "AndroidImage",
                              pictureSize: '',
                              imagesCount: 1
                          });
            $('#logoContainer span[name="IOSImage"]').hishopUpload(
                        {
                            title: '上传IOS图片',
                            url: "/admin/UploadHandler.ashx?action=newupload",
                            displayImgSrc: srcIOSImage,
                            imgFieldName: "siteLogo",
                            pictureSize: '',
                            imagesCount: 1
                        });
            $("#<%=hfAndroidImage.ClientID%>").val("");
            $("#<%=hfIOSImage.ClientID%>").val("");            
        })

        function getUploadImages() {
            var srcAndroidImage = $('#logoContainer span[name="AndroidImage"]').hishopUpload("getImgSrc");
            $("#<%=hfAndroidImage.ClientID%>").val(srcAndroidImage);

            var srcIOSImage = $('#logoContainer span[name="IOSImage"]').hishopUpload("getImgSrc");
            $("#<%=hfIOSImage.ClientID%>").val(srcIOSImage);
            return true;
        }

        function doSubmit() {
            getUploadImages();
            return true;
        }
    </script>
    <div class="areacolumn clearfix">
         
        <div class="columnright">
      

            <div class="formitem validator2 startpage">
                <ul>
                    <li><span class="formitemtitle Pw_128">上传Android图片：</span>
                        <div id="logoContainer">
                            <span name="AndroidImage" class="imgbox"></span>                            
                            <p></p>
                        </div>
                    </li>                  
                    <li runat="server" id="liParent"><span class="formitemtitle Pw_128">上传IOS图片：</span>
                        <div id="logoContainer">
                            <span name="IOSImage" class="imgbox"></span>                            
                            <p></p>
                        </div>
                    </li>
                       <li>
                           <span class="formitemtitle Pw_128">&nbsp;</span>
                          <asp:Button runat="server" ID="btnAdd" Text="保 存" CssClass="btn btn-primary" OnClientClick=" return doSubmit();"  />
                      </li>
                </ul>
                
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfAndroidImage" runat="server" />
    <asp:HiddenField ID="hfIOSImage" runat="server" />
</asp:Content>
