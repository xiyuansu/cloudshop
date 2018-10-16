<%@ Page Title="门店设置" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.Setting.Config" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="style.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript">
        // 初始化图片上传控件
        function initImageUpload() {
            var imgSrc = '<%=hidOldImages.Value%>';
            var imgHref = "<%=hidUrls.Value%>";
            $('#imageContainer span[name="HomeImage"]').hishopUpload(
                           {
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               displayImgSrc: imgSrc.split(','),
                               displayImgHref: imgHref.split(','),
                               imgFieldName: "HomeImage",
                               defaultImg: '',
                               //pictureSize: '750*280',
                               imagesCount: 5,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            var srcImg = $('#imageContainer span[name="HomeImage"]').hishopUpload("getImgSrc");
            var ImgHref = $('#imageContainer').hishopUpload("getImgHref");
            $("#<%=hidUrls.ClientID%>").val(ImgHref);
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);
            if ($("#imgStatus").val() != "1") {
                //
                return true;
            }
            if (srcImg == "") {
                ShowMsg("请至少上传一张广告图");
                return false;
            }
            return true;
        }
        $(document).ready(function () {
            initImageUpload();
            showHomeBanner($("input[name='ctl00$contentHolder$rblPositionRouteTo'][checked]").val());
            $("label", "#rdForClick").click(function () {
                showHomeBanner($($(this)[0]).attr("for"));
            });
            $("input[type=radio][name='ctl00$contentHolder$rblPositionRouteTo']").click(function () {
                showHomeBanner($(this).attr("value"));

            });
        });
        //展示多图
        function showHomeBanner(name) {
            console.log(name);
            if (name == "StoreList" || name == "ctl00_contentHolder_rblPositionRouteTo_1") {
                $("#liHomeImg").show(); $("#imgStatus").val("1");
            }
            else {
                $("#liHomeImg").hide(); $("#imgStatus").val("0");
            }
            if (name == "Platform" || name == "ctl00_contentHolder_rblPositionRouteTo_2") {//
                $("#liNoMatchStore").hide();
            } else {
                $("#liNoMatchStore").show();
            }
            //门店会员访问限制
            if (name == "NearestStore" || name == "ctl00_contentHolder_rblPositionRouteTo_0") {
                $("#liVisit").show();
            } else {
                $("#liVisit").hide();
            }

        }
    </script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:return false;">门店设置</a></li>
                <li><a href="MarketingImageList.aspx">营销图库</a></li>
                <li><a href="MarktingList.aspx">营销图标设置</a></li>
                <li><a href="TagList.aspx">门店标签设置</a></li>
                <li><a href="../StoreSetting.aspx">门店APP推送设置</a></li>
                <li><a href="StoreAppDonwload.aspx">门店APP下载设置</a></li>
                <li><a href="../../store/DadaLogistics.aspx">达达物流设置</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li class="mb_0" id="rdForClick"><span class="formitemtitle">定位后：</span>

                        <asp:RadioButtonList ID="rblPositionRouteTo" runat="server" RepeatDirection="Horizontal" CssClass="icheck"></asp:RadioButtonList>
                        <p>（多门店首页系统将根据用户当前位置,推荐同城且满足条件的门店,同城或区域电商且平台不发货时,推荐使用）</p>

                    </li>
                    <li class="mb_0" id="liNoMatchStore">
                        <span class="formitemtitle">无匹配门店时：</span>
                        <asp:RadioButtonList ID="rblPositionNoMatchTo" runat="server" RepeatDirection="Horizontal" CssClass="icheck"></asp:RadioButtonList>
                    </li>
                    <li class="mb_0" id="liHomeImg" style="display: none;">
                        <input type="hidden" id="imgStatus" value="0" />
                        <span class="formitemtitle">首页广告图：<br />
                            (750*360)</span>
                        <div id="imageContainer">
                            <span name="HomeImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                            <asp:HiddenField ID="hidUrls" runat="server" />
                        </div>
                    </li>
                </ul>
                <ul>
                    <li>
                        <h2 class="colorE">订单设置</h2>
                    </li>
                    <li><span class="formitemtitle">订单自动分配到门店：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radAutoAllotOrder"></Hi:OnOff>
                        </abbr>
                        <p>开启时，订单根据配送范围自动匹配到门店。<br />
                            若关闭，则订单由平台手动匹配。</p>
                    </li>
                    <li><span class="formitemtitle">上门自提需要验证提货码：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radStoreNeedTakeCode"></Hi:OnOff>
                        </abbr>
                        <p>开启时，自提订单必须要提供核销码才能核销订单<br />
                            关闭时，无核销码也能核销订单</p>
                    </li>
                </ul>
                <ul>
                    <li>
                        <h2 class="colorE">其它设置</h2>
                    </li>
                    <li class="mb_0"><span class="formitemtitle">是否开启门店推荐：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radIsRecommend"></Hi:OnOff>
                        </abbr>
                        <p>当用户访问某个商品详情页时，若所在门店已售罄，或该门店不在服务范围内，是否向用户推荐其他门店</p>
                    </li>

                    <li class="mb_0"><span class="formitemtitle">非营业时间访问门店下单：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radOrderInClosingTime"></Hi:OnOff>

                        </abbr>
                        <p>&nbsp;</p>
                    </li>

                    <li class="mb_0" id="liVisit"><span class="formitemtitle">门店发展的会员只能访问对应的门店：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radMemberVisitOtherStore"></Hi:OnOff>
                        </abbr>
                        <p>当用户在访问商城首页时，会将用户匹配至建立绑定关系的门店，但是用户可以通过点击某一家门店的链接进入到其它门店中</p>
                    </li>
                </ul>
                <ul>
                    <li class="mb_0">&nbsp;</li>
                </ul>
                <asp:Button runat="server" ID="btnSave" Text="保 存" OnClick="btnSave_Click" OnClientClick="return getUploadImages();" CssClass="btn btn-primary ml_198" />
            </div>
        </div>
    </div>
</asp:Content>
