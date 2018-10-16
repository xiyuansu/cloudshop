<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.SiteContent" CodeBehind="SiteContent.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register Src="../Ascx/ImageList.ascx" TagName="ImageList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem validator1">
                <ul class="jichushezhi">
                    <li>
                        <h2>基本设置</h2>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>商城名称：</span>
                        <asp:TextBox ID="txtSiteName" CssClass="forminput form-control formwidth" runat="server" placeholder="不超过60个字符" />
                        <p id="txtSiteNameTip" runat="server"></p>
                    </li>
                    <li class="m_none" style="height: 90px"><span class="formitemtitle">商城标志：</span>
                        <div id="logoContainer">
                            <span name="siteLogo" class="imgbox"></span>
                            <asp:HiddenField ID="hidOldLogo" runat="server" />
                            <asp:HiddenField ID="hidUploadLogo" runat="server" />
                        </div>
                    </li>
                    <li class="mb_0"><span class="formitemtitle">客服电话：</span>
                        <asp:TextBox ID="txtServicePhone" CssClass="forminput form-control formwidth" runat="server" />
                    </li>
                    <li style="padding-top: 5px;" class="mb_0"><span class="formitemtitle"><em>*</em>网店授权域名：</span>
                        <asp:TextBox ID="txtDomainName" CssClass="form-control forminput" runat="server" placeholder="部分收费功能用此域名访问才能使用"></asp:TextBox>
                        <p id="txtDomainNameTip" runat="server"></p>
                    </li>

                    <li><span class="formitemtitle">自定义页尾：</span>
                        <span style="display: block; float: left;">
                            <Hi:Ueditor ID="fkFooter" runat="server" Width="660" />
                        </span>
                    </li>
                    <li><span class="formitemtitle">会员注册协议：</span>

                        <span style="display: block; float: left;">
                            <Hi:Ueditor ID="fckRegisterAgreement" ImportLib="1" runat="server" Width="660" />
                        </span>
                    </li>
                    <li>
                        <h2 class="clear">开放平台接口参数</h2>
                    </li>
                    <li><span class="formitemtitle">appkey：</span>
                        <asp:Literal ID="litKeycode" runat="server" />
                    </li>
                    <li><span class="formitemtitle">appsecret：</span>
                        <asp:Literal ID="litappsecret" runat="server" />
                    </li>
                    <li>
                        <h2 class="clear">网店管家接口参数</h2>
                    </li>
                    <li><span class="formitemtitle">接口地址：</span>
                        <asp:Literal ID="litwdgjapi" runat="server" />(旧)
                        <br />
                        <span class="formitemtitle">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        <asp:Literal ID="litwdgjapi_new" runat="server" />(新)
                    </li>
                    <li><span class="formitemtitle">appsecret：</span>
                        <asp:Literal ID="litappsecret2" runat="server" />
                    </li>
                     <li class="mb_0"><span class="formitemtitle"><em></em>接入码：</span>
                        <asp:TextBox ID="txtPolyapiAppId" CssClass="form_input_l form-control " runat="server" placeholder="请填写菠萝派提供的接入码"></asp:TextBox>
                        <p id="txtPolyapiAppIdTip" runat="server"></p>
                    </li>
                     <li class="mb_0"><span class="formitemtitle"><em></em>密钥：</span>
                        <asp:TextBox ID="txtPolyapiKey" CssClass="form_input_l form-control " runat="server" placeholder="请填写菠萝派提供的密钥"></asp:TextBox>
                        <p id="txtPolyapiKeyTip" runat="server"></p>
                    </li>
                    <li>
                        <h2 class="clear">商品设置</h2>
                    </li>
                    <li style="display: none;"><span class="formitemtitle">商品价格小数点位数：</span>
                        <span class="formselect">
                            <Hi:DecimalLengthDropDownList ID="dropBitNumber" runat="server" /></span>
                    </li>
                    <li><span class="formitemtitle">默认商品图片：</span>
                        <div id="imageContainer">
                            <span name="defaultImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </div>
                    </li>
                    <li>
                        <h2 class="clear">营销SEO设置</h2>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>简单介绍：</span>
                        <asp:TextBox ID="txtSiteDescription" CssClass="form_input_l form-control " runat="server" placeholder="长度限制在100字符以内"></asp:TextBox>
                        <p id="txtSiteDescriptionTip" runat="server"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>商城描述：</span>
                        <asp:TextBox ID="txtSearchMetaDescription" runat="server" CssClass="form_input_l form-control " placeholder="长度限制在260字符以内"></asp:TextBox>
                        <p id="txtSearchMetaDescriptionTip" runat="server"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle">搜索关键字：</span>
                        <asp:TextBox ID="txtSearchMetaKeywords" CssClass="form_input_l form-control " runat="server" placeholder="长度必须控制在160个字符以内" />
                        <p id="txtSearchMetaKeywordsTip" runat="server"></p>
                    </li>
                                        <li>
                        <h2 class="clear">定位API设置</h2>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>腾讯地图AppKey：</span>
                        <asp:TextBox ID="txtQQMapAppKey" CssClass="form_input_l form-control " runat="server" placeholder="请填写申请的腾讯地图AppKey"></asp:TextBox>
                        <p id="txtQQMapAppKeyTip" runat="server">系统内置AppKey,如果腾讯地图调用次数比较多,请<a href="http://lbs.qq.com/console/mykey.html" target="_blank">点击单独申请</a>。</p>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:HiddenField ID="txtAutoRedirectClient" runat="server" Value="true" />
                    <asp:Button ID="btnOK" runat="server" Text="提交" CssClass="btn btn-primary" OnClientClick="return getUploadImages();" />
                    <asp:Button ID="btnClear" runat="server" Text="清空缓存" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                </div>
            </div>
        </div>
    </div>


    <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        // 初始化图片上传控件
        function initImageUpload() {
            var imgSrc = '<%=hidOldImages.Value%>';
            $('#imageContainer span[name="defaultImage"]').hishopUpload(
                           {
                               title: '商品默认图片',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: imgSrc,
                               imgFieldName: "defaultImage",
                               defaultImg: '',
                               pictureSize: '600*600',
                               imagesCount: 1,
                               dataWidth: 9
                           });


            var logoSrc = '<%=hidOldLogo.Value%>';
            $('#logoContainer span[name="siteLogo"]').hishopUpload(
                           {
                               title: '商城logo',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: logoSrc,
                               imgFieldName: "siteLogo",
                               defaultImg: '',
                               pictureSize: '200 * 40',
                               imagesCount: 1,
                               dataWidth: 9
                           });
        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!PageIsValid()) return false;
            var srcImg = $('#imageContainer span[name="defaultImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);

            var srcLogo = $('#logoContainer span[name="siteLogo"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadLogo.ClientID%>").val(srcLogo);
            return true;
        }

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtSiteName', 1, 60, false, null, '商城名称为必填项，长度限制在60字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtDomainName', 1, 128, false, '', '商城域名必须控制在128个字符以内'))

            initValid(new InputValidator('ctl00_contentHolder_txtTQCode', 0, 4000, true, null, '请在这里填入您获取的网页客服代码'))
            initValid(new SelectValidator('ctl00_contentHolder_dropBitNumber', false, '设置商品的价格经过运算后数值精确到小数点后几位，超出将四舍五入。'))
            initValid(new InputValidator('ctl00_contentHolder_txtNamePrice', 1, 10, true, null, '您的价长度不能超过10个字符'))
            initValid(new InputValidator('ctl00_contentHolder_txtSiteDescription', 1, 100, false, null, '简单介绍，长度限制在100字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtSearchMetaDescription', 1, 260, false, '', '商城描述,长度必须控制在260个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtSearchMetaKeywords', 1, 160, true, null, '搜索关键字，长度必须控制在160个字符以内'))
        }
        $(document).ready(function () { InitValidators(); initImageUpload(); });


    </script>
</asp:Content>


