<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddIbeaconPage.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.AddIbeaconPage" %>

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
            InitValidators();
            $('#logoContainer span[name="siteLogo"]').hishopUpload(
                          {
                              title: '缩略图',
                              url: "/admin/UploadHandler.ashx?action=newupload",
                              imageDescript: '',
                              imgFieldName: "siteLogo",
                              pictureSize: '',
                              imagesCount: 1
                          });

            $("[id$=ddlSystemUrl]").change(function () {
                var value = $(":selected", this).val();
                $("#ctl00_contentHolder_txtURL").val(value);
            });
        })
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 1, 6, false, null, '主标题不能为空，长度必须小于或等于6个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtSubtitle', 1, 7, false, null, '副标题不能为空，长度必须小于或等于7个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtRemark', 1, 15, false, null, '备注不能为空，长度必须小于或等于15个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtURL', 0, 300, false, '(http)|(https)://.*', '跳转URL必须以http://或者https://开头，长度必须小于或等于300个字符'));
        }
        function doSubmit() {
            getUploadImages();
            var strTitle = $("#ctl00_contentHolder_txtTitle").val().trim();
            var strSubTitle = $("#ctl00_contentHolder_txtSubtitle").val().trim();
            var thumbnail = $("#ctl00_contentHolder_hidUploadLogo").val();
            var strUrl = $("#ctl00_contentHolder_txtURL").val().trim();
            var strRemark = $("#ctl00_contentHolder_txtRemark").val().trim();

            if (strTitle == "" || strTitle.length > 6) {
                alert("主标题不能为空，长度必须小于或等于6个字符！");
                return false;
            }
            if (strSubTitle == "" || strSubTitle.length > 7) {
                alert("副标题不能为空，长度必须小于或等于7个字符！");
                return false;
            }
            if (thumbnail == "") {
                alert("请上传缩略图！");
                return false;
            }
            if (strUrl == "" || strUrl.length > 300) {
                alert("跳转URL必须以http://或者https://开头，长度必须小于或等于300个字符");
                return false;
            }
            var regex = new RegExp("^(http)|(https)://.*");
            if (!regex.test(strUrl)) {
                alert("跳转URL必须以http://或者https://开头，长度必须小于或等于300个字符");
                return false;
            }
            if (strRemark == "" || strRemark.length > 15) {
                alert("备注不能为空，长度必须小于或等于15个字符！");
                return false;
            }

            return true;
        }
        //获取上传成功后的图片路径
        function getUploadImages() {
            var srcLogo = $('#logoContainer span[name="siteLogo"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadLogo.ClientID%>").val(srcLogo);
            return true;
        }
    </script>
  
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="IbeaconPageList.aspx">管理</a></li>
                <li  class="hover"><a>新增</a></li>
               
            </ul>
        </div>
        <input type="hidden" id="txtRegionId" value="" />
        <div class="datafrom">
            <div class="formitem validator1 ">
                <ul>
                    <li class="mb_0"><span class="formitemtitle  "><em>*</em>主标题：</span>
                        <input style="display: none" />
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="forminput form-control" MaxLength="6"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtTitleTip">
                        </p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle "><em>*</em>副标题：</span>
                        <input style="display: none" />
                        <asp:TextBox ID="txtSubtitle" runat="server" CssClass="forminput form-control" MaxLength="7"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtSubtitleTip">
                        </p>
                    </li>
                    <li class="m_none"><span class="formitemtitle  m_none"><em>*</em>缩略图：</span>
                        <div id="logoContainer">
                            <span name="siteLogo" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadLogo" runat="server" />                           
                        </div>
                         <p>图片大小建议120px*120 px，限制不超过200 px *200 px，图片需为正方形，不能是二维码</p>
                    </li>


                    <li class="mb_0"><span class="formitemtitle "><em>*</em>跳转URL：</span>
                        <asp:DropDownList ID="ddlSystemUrl" runat="server" CssClass="iselect droptype" Width="100">
                            <asp:ListItem Text="链接" Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtURL" CssClass="forminput form-control ml_10" runat="server" MaxLength="300"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtURLTip">
                        </p>
                    </li>

                    <li class="mb_0"><span class="formitemtitle "><em>*</em>备注：</span>
                        <asp:TextBox ID="txtRemark" CssClass="forminput form-control" runat="server" MaxLength="15"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtRemarkTip">
                        </p>
                    </li>

                </ul>
                <div class="ml_198">
                    <asp:Button runat="server" ID="btnAdd" Text="保 存" CssClass="Ml_100 btn btn-primary" OnClientClick=" return doSubmit();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
