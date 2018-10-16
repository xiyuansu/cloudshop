<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditGift.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditGift" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register Src="../Ascx/ImageList.ascx" TagName="ImageList" TagPrefix="uc1" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content2" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="gifts.aspx">列表</a></li>
                <li class="hover"><a href="javascript:void">编辑</a></li>

            </ul>
        </div>
    </div>
    <div class="areacolumn">
        <div class="columnright">

            <div class="datafrom">
                <div class="formitem">
                    <ul>
                        <li runat="server" visible="false" id="liprize">
                            <span class="formitemtitle" style="color: red">友情提示：</span>
                            <asp:Label ID="lblprizeMsg" runat="server" Text="" Style="color: red"></asp:Label>
                        </li>
                        <li>
                            <span class="formitemtitle">是否参与促销赠送：</span>
                            <abbr class="formselect">
                                <Hi:OnOff runat="server" ID="chkPromotion"></Hi:OnOff>
                            </abbr>
                            <span id="spanPromotionMsg" style="color: red">&nbsp;&nbsp;参与促销赠送的礼品不计算运费;积分兑换和抽奖获得的礼品仍按对应的运费模板计算运费</span>
                        </li>
                        <li><span class="formitemtitle"><em>*</em>礼品名称：</span>
                            <asp:TextBox ID="txtGiftName" runat="server" CssClass="form_input_l form-control" placeholder="在1至60个字符之间"></asp:TextBox>
                        </li>

                        <li><span class="formitemtitle">礼品图片：</span>
                            <div id="imageContainer">
                                <span name="giftImage" class="imgbox"></span>
                                <asp:HiddenField ID="hidUploadImages" runat="server" />
                                <asp:HiddenField ID="hidOldImages" runat="server" />
                            </div>
                        </li>

                        <li class="clearfix"><span class="formitemtitle">计量单位：</span>
                            <asp:TextBox ID="txtUnit" runat="server" CssClass="form_input_m form-control" placeholder="在1至10个字符之间"></asp:TextBox>
                        </li>
                        <li><span class="formitemtitle">成本价：</span>
                            <div class="input-group">
                                <span class="input-group-addon">￥</span>
                                <asp:TextBox ID="txtCostPrice" runat="server" CssClass="form_input_s form-control" placeholder="不能超过2位小数"></asp:TextBox>
                            </div>
                        </li>
                        <li><span class="formitemtitle">市场参考价：</span>
                            <div class="input-group">
                                <span class="input-group-addon">￥</span>
                                <asp:TextBox ID="txtMarketPrice" runat="server" CssClass="form_input_s form-control" placeholder="不能超过2位小数"></asp:TextBox>
                            </div>
                        </li>

                        <li>
                            <span class="formitemtitle">是否可以积分兑换：</span>
                            <abbr class="formselect">
                                <Hi:OnOff runat="server" ID="onoffIsPointExchange"></Hi:OnOff>
                            </abbr>
                        </li>
                        <li id="lipoint">
                            <span class="formitemtitle"><em>*</em>兑换需积分：</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtNeedPoint" runat="server" Text="0" CssClass="form_input_s form-control" placeholder="必须大于等于0,0表示不能兑换"></asp:TextBox>
                                <span class="input-group-addon">分</span>
                            </div>
                        </li>
                        <li style="display: none;">
                            <span class="formitemtitle">是否包邮：</span>
                            <abbr class="formselect">
                                <Hi:OnOff runat="server" ID="chkIsExemptionPostage"></Hi:OnOff>

                            </abbr>
                        </li>
                        <li><span class="formitemtitle"><em>*</em>运费模板：</span>
                            <abbr class="formselect">
                                <Hi:ShippingTemplatesDropDownList runat="server" CssClass="shippingTemplates iselect" ID="ShippingTemplatesDropDownList" NullToDisplay="请选择运费模板" /></abbr>
                            <a href="../Sales/AddShippingTemplate.aspx" class="input-group-a">&nbsp;&nbsp;新增运费模板</a>
                        </li>
                        <li id="weightRow" runat="server" clientidmode="Static"><span class="formitemtitle"><em>*</em>商品重量：</span>
                            <div class="input-group">
                                <Hi:TrimTextBox runat="server" CssClass="form_input_s form-control" ID="txtWeight" Text="" placeholder="请输入数字,限制为0-100000" ClientIDMode="Static" />
                                <span class="input-group-addon">KG</span>
                            </div>
                        </li>
                        <li id="volumeRow" runat="server" clientidmode="Static"><span class="formitemtitle"><em>*</em>商品体积：</span>
                            <div class="input-group">
                                <Hi:TrimTextBox runat="server" CssClass="form_input_s form-control" ID="txtVolume" placeholder="请输入数字,限制为0-100000" Text="" ClientIDMode="Static" />
                                <span class="input-group-addon">M<sup>3</sup></span>
                            </div>
                        </li>
                        <li class="h2_100">
                            <h2>礼品描述</h2>
                        </li>
                        <li><span class="formitemtitle">简单介绍：</span>
                            <asp:TextBox ID="txtShortDescription" runat="server" TextMode="MultiLine" Height="70px" CssClass="form_input_l form-control" placeholder="限制在300个字符内"></asp:TextBox>
                            <!--<p id="ctl00_contentHolder_txtShortDescriptionTip"></p>-->
                        </li>
                        <li><span class="formitemtitle">详细信息：</span><Hi:Ueditor ID="fcDescription" runat="server" Width="660" CssClass="fl" />
                        </li>
                        <li class="h2_100">
                            <h2>SEO设置</h2>
                        </li>
                        <li><span class="formitemtitle">详细页标题：</span>
                            <asp:TextBox ID="txtGiftTitle" runat="server" CssClass="form_input_l form-control" MaxLength="100" placeholder="在1至100个字符之间"></asp:TextBox>
                        </li>
                        <li><span class="formitemtitle">详细页关键字：</span>
                            <asp:TextBox ID="txtTitleKeywords" runat="server" CssClass="form_input_l form-control" MaxLength="160" placeholder="在1至160个字符之间"></asp:TextBox>
                        </li>
                        <li><span class="formitemtitle">详细页描述：</span>
                            <asp:TextBox ID="txtTitleDescription" runat="server" CssClass="form_input_l form-control" MaxLength="260" placeholder="在1至260个字符之间"></asp:TextBox>
                        </li>
                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnUpdate" runat="server" Text="保 存" OnClientClick="return getUploadImages();" CssClass="btn btn-primary" />

                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" src="../js/GiftPublish.helper.js?v=3.4"></script>
    <script type="text/javascript" language="javascript">
        // 初始化图片上传控件
        function initImageUpload() {
            var imgSrc = '<%=hidOldImages.Value%>';
            $('#imageContainer span[name="giftImage"]').hishopUpload(
                           {
                               title: '礼品图片',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: imgSrc,
                               imgFieldName: "giftImage",
                               defaultImg: '',
                               pictureSize: '300*200',
                               imagesCount: 1,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!PageIsValid()) return false;
            if (!doSubmit()) {
                return false;
            }
            var srcImg = $('#imageContainer span[name="giftImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);
            return true;
        }
        function fuCheckEnableZFBPage(event, state) {
            if (state) {
                $("#spanPromotionMsg").show();
            }
            else {
                $("#spanPromotionMsg").hide();
            }
        }
        function fuCheckEnablePointExchange(event, state) {
            if (state) {
                $("#lipoint").show();
            }
            else {
                $("#lipoint").hide();
            }
        }
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtGiftName', 1, 60, false, null, '礼品的名称，在1至60个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtUnit', 1, 10, true, null, '计量单位，在1至10个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtCostPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '成本价只能是数值，且不能超过2位小数'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCostPrice', 0.01, 10000000, '成本价只能是数值，不能超过10000000，且不能超过2位小数'));
            initValid(new InputValidator('ctl00_contentHolder_txtMarketPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '市场参考价只能是数值，且不能超过2位小数'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMarketPrice', 0.01, 10000000, '市场参考价只能是数值，不能超过10000000，且不能超过2位小数'));
            initValid(new InputValidator('ctl00_contentHolder_txtNeedPoint', 1, 10, false, '-?[0-9]\\d*', '兑换所需积分只能是数字，必须大于1'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtNeedPoint', 1, 10000000, '兑换所需积分不能为空，大小1-10000000之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 1, 300, true, null, '限制在300个字符内'))
            initValid(new InputValidator('ctl00_contentHolder_txtGiftTitle', 1, 100, true, null, '详细页标题，在1至100个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtTitleKeywords', 1, 160, true, null, '详细页关键字，在1至160个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtTitleDescription', 1, 260, true, null, '详细页描述，在1至260个字符之间'))

        }
        $(document).ready(function () {
            if ($("#ctl00_contentHolder_chkPromotion input").is(':checked')) {
                $("#spanPromotionMsg").show();
            }
            else {
                $("#spanPromotionMsg").hide();
            }
            if ($("#ctl00_contentHolder_onoffIsPointExchange input").is(':checked')) {
                $("#lipoint").show();
            }
            else {
                $("#lipoint").hide();
            }
            InitValidators();
            initImageUpload();
        });


    </script>
</asp:Content>

