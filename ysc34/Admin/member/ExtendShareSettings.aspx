<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ExtendShareSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ExtendShareSettings" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript" language="javascript">
        // 初始化图片上传控件
        function initImageUpload() {
            var shareSrc = '<%=hidUploadLogo.Value%>';
            $('#logoContainer span[name="imgExtendSharePic"]').hishopUpload(
                           {
                               title: '分享图片',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: shareSrc,
                               imgFieldName: "imgExtendSharePic",
                               defaultImg: '',
                               pictureSize: '',
                               imagesCount: 1,
                               dataWidth: 9
                           });
        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!PageIsValid()) return false;
            var shareSrc = $('#logoContainer span[name="imgExtendSharePic"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadLogo.ClientID%>").val(shareSrc);
            return true;
        }

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtExtendShareTitle', 0, 12, false, null, '分享分销标题长度限制在12字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtExtendShareDetail', 0, 20, false, '', '分享详情长度限制在20个字符以内'))
        }
        $(document).ready(function () { InitValidators(); initImageUpload(); });


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="DeductSettings.aspx">分佣规则设置</a></li>
                <li><a href="ReferralSettings.aspx">分销员申请设置</a></li>
                <li><a href="ReferralPosterSet.aspx">分销海报设置</a></li>
                <li class="hover"><a href="javascript:void">分销分享设置</a></li>
                <li><a href="RecruitPlanSettings.aspx">招募计划设置</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li class="mb_0"><span class="formitemtitle">分享分销标题：</span>
                        <asp:TextBox ID="txtExtendShareTitle" CssClass="forminput form-control formwidth" runat="server" placeholder="不超过12个字符" />
                        <p id="txtExtendShareTitleTip" runat="server"></p>
                    </li>
                </ul>
                <ul>
                    <li class="m_none" style="height: 90px"><span class="formitemtitle">分享图片：</span>
                        <div id="logoContainer">
                            <span name="imgExtendSharePic" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadLogo" runat="server" />
                            <span class="fi-help-text">建议尺寸：120 * 120 像素</span>
                        </div>
                    </li>
                </ul>
                <ul>
                    <li class="mb_0"><span class="formitemtitle">分享详情：</span>
                        <asp:TextBox ID="txtExtendShareDetail" CssClass="forminput form-control formwidth" runat="server" placeholder="不超过20个字符" />
                        <p id="txtExtendShareDetailTip" runat="server"></p>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="btn btn-primary" OnClientClick="return getUploadImages();" />
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
