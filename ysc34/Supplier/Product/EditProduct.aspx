<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Supplier/Admin.Master" CodeBehind="EditProduct.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.Product.EditProduct" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register Src="~/Admin/Ascx/ImageList.ascx" TagName="ImageList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../../Admin/css/style.css" rel="stylesheet" />
   <script type="text/javascript" src="/utility/jquery_hashtable.js"></script>
    <script type="text/javascript" src="/utility/jquery-powerFloat-min.js"></script>
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link href="../../Admin/css/bootstrap-switch.css" rel="stylesheet" />
   <script type="text/javascript" src="../../Admin/js/bootstrap-switch.js"></script>
    <script type="text/javascript" src="../../admin/product/attributes.helper.js"></script>
    <script type="text/javascript" src="../../admin/product/grade.price.helper.js"></script>
    <script type="text/javascript" src="../../admin/product/producttag.helper.js"></script>

    <style>
        #mainhtml {
            padding: 0 !important;
            background: #f3f3f3;
        }

            #mainhtml .formitemtitle {
                width: 250px;
                color: #999;
            }

            #mainhtml .databody .datafrom .formitem p {
                margin: 0 0 0 250px;
                width: 600px;
            }

        .iselect {
            width: 230px;
        }

        .databody .datafrom .formitem li {
            clear: both;
            float: left;
            margin-bottom: 30px;
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#myTab li:eq(1) a').tab('show');

        })
        function fuenableDeduct(event, state) {
            enableDeduct(state);
        }

    </script>
    <ul class="step_p_e" id="myTab">
        <li href="#home">1.选择商品分类</li>
        <li class="active" data-toggle="tab" id="information">2.编辑商品信息</li>
        <li data-toggle="tab" id="info">3.编辑商品详情</li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <asp:HiddenField ID="hidProductId" runat="server" />
        <asp:HiddenField ID="hidUploadImages" runat="server" />
        <asp:HiddenField ID="hidOldImages" runat="server" />
        <asp:HiddenField ID="txt_SalesCount" Value="0" runat="server" />
        <asp:HiddenField ID="txt_ShowSalesCount" Value="0" runat="server" />
        <asp:HiddenField ID="txt_VistiCounts" Value="0" runat="server" />
        <asp:HiddenField ID="hidSKUUploadImages" runat="server" />
        <asp:HiddenField ID="hidSKUOldImages" runat="server" />
        <asp:HiddenField ID="hidHasSku" runat="server" />
        <asp:HiddenField ID="hidOpenMultReferral" runat="server" Value="1" />
        <asp:HiddenField ID="hidHasActivity" runat="server" Value="0" />
        <asp:HiddenField ID="HidFromUrl" runat="server" Value="" />
        <asp:HiddenField ID="hidAuditStatus" runat="server" Value="2" />
        <div class="datafrom">
            <div class="formitem validator1">
                <div id="myTabContent" class="tab-content">
                    <div class="tab-pane fade  " id="home">
                    </div>
                    <div class="tab-pane fade active in" id="information_c">
                        <div class="info_group">
                            <h2 class="colorE">基本信息</h2>
                            <a class="img_toggle" href="javascript:;">
                                <img src="/admin/images/icon_fold.png" class="icon_fold" />
                                <img src="/admin/images/icon_down.png" class="icon_down" style="display: none" />
                            </a>
                            <ul class="hidetoggle">
                                <li>
                                    <span class="formitemtitle">所属商品分类：</span>
                                    <span style="line-height: 32px;" class="mr10">
                                        <asp:Literal runat="server" ID="litCategoryName"></asp:Literal></span>
                                    <asp:HyperLink runat="server" ID="lnkEditCategory" CssClass="colorBlue" Text="编辑" Style="line-height: 32px;"></asp:HyperLink>
                                </li>
                                <li>
                                    <span class="formitemtitle">商品类型：</span>
                                    <abbr class="formselect">
                                        <Hi:ProductTypeDownList runat="server" CssClass="productType iselect" ID="dropProductTypes" NullToDisplay="请选择" /></abbr>
                                </li>
                                <li>
                                    <span class="formitemtitle">品牌：</span>
                                    <abbr class="formselect">
                                        <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandCategories" NullToDisplay="请选择" CssClass="iselect" /></abbr>
                                </li>
                                <li style="margin-bottom: 0;">
                                    <span class="formitemtitle"><em>*</em>商品名称：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_l form-control" ID="txtProductName" placeholder=" 限定在60个字符" />
                                    <p id="ctl00_contentHolder_txtProductNameTip">&nbsp;</p>
                                </li>
                                <li class="clearfix"><span class="formitemtitle">商品简介：</span>
                                    <asp:TextBox runat="server" Rows="6" MaxLength="300" Height="100px" Columns="76" ID="txtShortDescription" TextMode="MultiLine" CssClass="form-control form_input_l" placeholder="限定在300个字符以内"></asp:TextBox>
                                </li>
                                <li class="clearfix" id="l_tags" runat="server">
                                    <span class="formitemtitle">商品标签定义：</span>
                                    <div id="div_tags">
                                        <div id="f_div" class="icheck-label-5-10 pull-left">
                                            <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
                                        </div>
                                    </div>
                                    <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                                </li>
                                <li style="margin-bottom: 0;" id="liDisplaySequenc"><span class="formitemtitle">排序：</span>
                                    <asp:Label runat="server" ID="txtDisplaySequence"></asp:Label>
                                    <p id="ctl00_contentHolder_txtDisplaySequenceTip"></p>
                                </li>
                                <li><span class="formitemtitle">商家编码：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtProductCode" onblur="isExistProductCode(this)" placeholder="长度不能超过20个字符" />
                                </li>
                                <li><span class="formitemtitle">计量单位：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtUnit" placeholder="20个字符英文和中文例:g/元" />
                                </li>
                            </ul>
                        </div>

                        <ul id="attributeRow" style="display: none;">
                            <li>
                                <h2 class="colorE">扩展属性</h2>
                            </li>

                            <li>
                                <div class="attributeContent icheck-label-5-10" id="attributeContent">
                                </div>
                                <Hi:TrimTextBox runat="server" ID="txtAttributes" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                            </li>
                        </ul>

                        <div class="info_group">
                            <%--<h2>物流及其他</h2>--%>
                            <a class="img_toggle" href="javascript:;" id="aSalestatus">
                                <img src="/admin/images/icon_fold.png" class="icon_fold" />
                                <img src="/admin/images/icon_down.png" class="icon_down" style="display: none" />
                            </a>
                            <ul>
                                <li id="liSalestatus" style="display: none;"><span class="formitemtitle">销售状态：</span>
                                    <div class="icheck_group">
                                        <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus" Text="出售中" CssClass="icheck"></asp:RadioButton>
                                        <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus" Text="下架区" CssClass="icheck"></asp:RadioButton>
                                        <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus" Text="仓库中" CssClass="icheck"></asp:RadioButton>
                                    </div>
                                </li>
                                <li><span class="formitemtitle"><em>*</em>运费模板：</span>
                                    <abbr class="formselect">
                                        <a name="aShippingTemplate" id="aShippingTemplate"></a>
                                        <Hi:ShippingTemplatesDropDownList runat="server" CssClass="shippingTemplates iselect" ID="ShippingTemplatesDropDownList" NullToDisplay="请选择运费模板" /></abbr>
                                </li>
                                <li id="weightRow" runat="server" clientidmode="Static"><span class="formitemtitle"><em>*</em>商品重量：</span>
                                    <div class="input-group">
                                        <Hi:TrimTextBox runat="server" CssClass="form_input_s form-control" ID="txtWeight" Text="0" placeholder="请输入数字,限制为0-100000" ClientIDMode="Static" />
                                        <span class="input-group-addon">KG</span>
                                    </div>
                                </li>
                                <li id="volumeRow" runat="server" clientidmode="Static"><span class="formitemtitle"><em>*</em>商品体积：</span>
                                    <div class="input-group">
                                        <Hi:TrimTextBox runat="server" CssClass="form_input_s form-control" ID="txtVolume" placeholder="请输入数字,限制为0-100000" Text="0" ClientIDMode="Static" />
                                        <span class="input-group-addon">M<sup>3</sup></span>
                                    </div>
                                </li>
                                <li style="display: none;"><span class="formitemtitle">商品包邮： </span>
                                    <Hi:OnOff runat="server" ID="ChkisfreeShipping"></Hi:OnOff>
                                </li>
                            </ul>
                        </div>

                        <div id="skuTitle" style="display: block;" class="info_group">
                            <h2 class="colorE" style="float: left">库存规格</h2>
                            <a class="img_toggle" href="javascript:;">
                                <img src="/admin/images/icon_fold.png" class="icon_fold" />
                                <img src="/admin/images/icon_down.png" class="icon_down" style="display: none" />
                            </a>
                            <ul>
                                <li><span class="formitemtitle">市场价：</span>
                                    <div class="input-group">
                                        <span class="input-group-addon">￥</span>
                                        <Hi:TrimTextBox runat="server" CssClass="form_input_s form-control" ID="txtMarketPrice" />
                                    </div>
                                </li>
                                <li id="salePriceRow" class="mb_0" style="display: none;"><span class="formitemtitle"><em>*</em>一口价：</span>
                                    <div class="input-group">
                                        <span class="input-group-addon">￥</span>
                                        <Hi:TrimTextBox runat="server" CssClass="form-control form_input_s" ID="txtSalePrice" />
                                    </div>
                                    <p id="ctl00_contentHolder_txtSalePriceTip">&nbsp;</p>
                                    <Hi:TrimTextBox runat="server" ID="txtMemberPrices" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>

                                </li>
                                <li id="costPriceRow"><span class="formitemtitle"><em>*</em>供货价：</span>
                                    <div class="input-group">
                                        <span class="input-group-addon">￥</span>
                                        <Hi:TrimTextBox runat="server" CssClass="form-control form_input_s" ID="txtCostPrice" />
                                    </div>
                                </li>
                                <li id="skuCodeRow"><span class="formitemtitle">货号：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtSku" onblur="isExistSkuCode(this)" paceholder="限定在20个字符" />
                                </li>
                                <li id="qtyRow"><span class="formitemtitle"><em>*</em>商品库存：</span><Hi:TrimTextBox
                                    runat="server" CssClass="form_input_m form-control" ID="txtStock" />
                                </li>
                                <li id="warningqtyRow" class="mb_0"><span class="formitemtitle"><em>*</em>警戒库存：</span><Hi:TrimTextBox
                                    runat="server" CssClass="form_input_m form-control" ID="txtWarningStock" Text="0" />
                                    <p id="ctl00_contentHolder_txtWarningStockTip"></p>
                                </li>
                                <li id="enableSkuRow" style="display: none;"><span class="formitemtitle">规格：</span><input
                                    id="btnEnableSku" type="button" class="btn btn-primary" value="开启规格" />
                                </li>
                                <li id="skuRow" style="display: none;">
                                    <div id="skuContent">
                                        <input type="button" class="btn btn-default" id="btnshowSkuValue" value="编辑规格" />
                                        <%--<input type="button" class="btn btn-primary" id="btnCloseSku" value="关闭规格" />
                                        <input type="button" class="btn btn-primary" id="btnGenerateAll" value="生成所有规格" />--%>
                                        <input type="button" class="btn btn-default" id="btnUploadSKUImg" value="上传规格图片" attrid="0" style="display: none;" />
                                    </div>
                                    <div class="batch">
                                        <dl>
                                            <dd><span style="color: #666; font-size: 14px;">批量填充：</span></dd>
                                            <dd id="batchMarketPrice">
                                                <input type="text" id="txtBatchMarketPrice" class="form-control" placeholder="一口价" /></dd>
                                            <dd id="batchCostPrice">
                                                <input type="text" id="txtBatchCostPrice" class="form-control" placeholder="供货价" /></dd>
                                            <dd id="batchWeight">
                                                <input type="text" id="txtBatchWeight" class="form-control" placeholder="重量/体积" /></dd>
                                            <dd id="batchStore">
                                                <input type="text" id="txtBatchStore" class="form-control" placeholder="库存" /></dd>
                                            <dd id="batchLimitStore">
                                                <input type="text" id="txtBatchLimitStore" class="form-control" placeholder="警戒库存" /></dd>
                                            <dd id="batchOk">
                                                <input type="button" class="btn btn-primary" id="btnBatchOk" value="确定" /></dd>

                                        </dl>
                                    </div>
                                    <p id="skuFieldHolder" style="margin: 0px auto; padding: 0; display: none;">
                                    </p>
                                    <div id="skuTableHolder" style="position: relative;">
                                        <input type="button" id="btnAddItem" value="" />
                                    </div>
                                    <Hi:TrimTextBox runat="server" ID="txtSkus" TextMode="MultiLine" Style="display: none"></Hi:TrimTextBox>
                                    <asp:CheckBox runat="server" ID="chkSkuEnabled" Style="display: none;" />
                                </li>
                            </ul>
                        </div>




                        <div class="info_group" style="display: none;">
                            <h2 class="colorE">搜索优化</h2>
                            <a class="img_toggle" href="javascript:;">
                                <img src="/admin/images/icon_fold.png" class="icon_fold" />
                                <img src="/admin/images/icon_down.png" class="icon_down" style="display: none" />
                            </a>

                            <ul>
                                <li><span class="formitemtitle">详细页标题：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_l form-control" ID="txtTitle" placeholder="详细页标题限制在100字符以内" />

                                </li>
                                <li><span class="formitemtitle">详细页描述：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_l form-control" ID="txtMetaDescription" placeholder="详细页描述限制在260字符以内" />

                                </li>
                                <li><span class="formitemtitle">详细页搜索关键字：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_l form-control" ID="txtMetaKeywords" placeholder="详细页搜索关键字限制在160字符以内" />
                                </li>
                            </ul>
                        </div>


                        <div class="info_group" id="ReferralConfig">
                            <h2 class="colorE clear">佣金设置</h2>
                            <a class="img_toggle" href="javascript:;">
                                <img src="/admin/images/icon_fold.png" class="icon_fold" />
                                <img src="/admin/images/icon_down.png" class="icon_down" style="display: none" />
                            </a>
                            <ul>
                                <li id="enableDeduct" runat="server" clientidmode="Static">
                                    <span class="formitemtitle">开启佣金</span>
                                    <Hi:OnOff runat="server" ID="ofEnableDeduct"></Hi:OnOff>
                                </li>

                                <li id="liSubMemberDeduct" runat="server" style="display: none;" clientidmode="Static" class="mb_0">
                                    <div class="input-group">
                                        <span class="formitemtitle">会员直接上级抽佣比例：</span>
                                        <asp:TextBox ID="txtSubMemberDeduct" CssClass="form_input_m form-control radius-left" MaxLength="10" runat="server" placeholder="A邀请了B，B后续消费A所能获得的佣金" />
                                        <span class="input-group-addon">%</span>
                                    </div>

                                    <p id="ctl00_contentHolder_txtSubMemberDeductTip"></p>
                                    <asp:Literal ID="litSubMemberDeduct" runat="server" />

                                </li>
                                <li id="liReferralDeduct" runat="server" style="display: none;" clientidmode="Static" class="mb_0">
                                    <span class="formitemtitle">会员上二级抽佣比例：</span>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" CssClass="form_input_m form-control" ID="txtSecondLevelDeduct" MaxLength="10" placeholder="A邀请的会员B后续发展了其它会员C，C消费时，A所能获得的佣金" />
                                        <span class="input-group-addon">%</span>
                                    </div>
                                    <p id="ctl00_contentHolder_txtSecondLevelDeductTip"></p>
                                    <asp:Literal ID="litReferralDeduct" runat="server" />
                                </li>
                                <li id="liSubReferralDeduct" runat="server" style="display: none;" clientidmode="Static" class="mb_0">
                                    <div class="input-group">
                                        <span class="formitemtitle">会员上三级抽佣比例：</span>
                                        <asp:TextBox ID="txtThreeLevelDeduct" CssClass="form_input_m form-control radius-left" MaxLength="10" runat="server" placeholder="A邀请的会员B后续发展了会员C，C又发展了会员D，D消费时，A所能获得的佣金" />
                                        <span class="input-group-addon">%</span>
                                    </div>
                                    <p id="ctl00_contentHolder_txtThreeLevelDeductTip"></p>
                                    <asp:Literal ID="litSubReferralDeduct" runat="server" />

                                </li>
                            </ul>
                        </div>
                    </div>

                    <div class="tab-pane fade" id="info_c">
                        <div class="info_group">
                            <h2 class="colorE">图片和描述</h2>
                            <a class="img_toggle" href="javascript:;">
                                <img src="/admin/images/icon_fold.png" class="icon_fold" />
                                <img src="/admin/images/icon_down.png" class="icon_down" style="display: none" />
                            </a>

                            <ul class="page2">
                                <li class="clearfix"><span class="formitemtitle">商品主图：</span>
                                    <div id="imageContainer">
                                        <span name="productImages" class="imgbox"></span>
                                    </div>
                                    <p>
                                        限定上传5张图片，建议尺寸：800*800像素
                                    </p>
                                </li>

                                <li class="clearfix m_none"><span class="formitemtitle">商品描述：</span> <span
                                    class="tab">
                                    <div class="status">
                                        <ul>
                                            <li style="clear: none;"><a onclick="ShowNotes(1)" class="off" id="tip1" style="cursor: pointer">PC端</a></li>
                                            <li style="clear: none;"><a onclick="ShowNotes(2)" id="tip2" style="cursor: pointer">移动端</a></li>
                                        </ul>
                                    </div>
                                </span><span>
                                    <div id="notes1" style="float: left; margin-left: 250px;">
                                        <Hi:Ueditor ID="fckDescription" runat="server" Width="660" />
                                    </div>
                                    <div id="notes2" style="float: left; margin-left: 250px; display: none;">
                                        <Hi:Ueditor ID="fckmobbileDescription" runat="server" Width="660" ImportLib="1" />
                                    </div>
                                </span>
                                    <p style="margin-bottom: 40px;">
                                        <asp:CheckBox runat="server" CssClass="icheck icheck-label-5-10" ID="ckbIsDownPic" Text="是否下载商品描述外站图片" />
                                        <i class="icon_query" style="margin: 7px 0 0 0;" data-toggle="tooltip" data-placement="top" title="如果勾选此选项时，商品里面有外站的图片则会下载到本地，相反则不会，由于要下载图片，如果图片过多或图片很大，需要下载的时间就多，请慎重选择。"></i>
                                    </p>
                                </li>
                            </ul>
                        </div>
                    </div>

                </div>
            </div>

            <div class="footer_btn">
                <a href="#info_c" id="next" class="btn btn-primary" data-toggle="tab">下一步</a>
                <a class="btn btn-default" id="pre" href="#information_c" data-toggle="tab" style="display: none">上一步</a>
                <asp:Button runat="server" ID="btnSave" Style="display: none" Text="完成" OnClientClick="return getUploadImages();" CssClass="btn  btn-primary" />
            </div>


        </div>
        <div class="Pop_up " id="priceBox" style="display: none;">
            <h1>
                <span id="popTitle"></span>
            </h1>
            <div class="img_datala">
                <i class="iconfont">&#xe601;</i>
            </div>
            <div class="mianform ">
                <div id="priceContent">
                </div>
                <div style="margin-top: 15px; text-align: right; padding: 10px 15px 0 0; border-top: 1px solid #ddd">
                    <input type="button" class="btn btn-default  close_pop" value="取消" />
                    <input type="button" class="btn btn-primary" value="确定" onclick="doneEditPrice();" />
                </div>
            </div>
        </div>
        <div class="Pop_up" id="skuValueBox" style="display: none;">
            <h1>
                <span>选择要生成的规格</span>
            </h1>
            <div class="img_datala">
                <i class="iconfont">&#xe601;</i>
            </div>
            <div class="mianform ">
                <div id="skuItems">
                </div>
                <div class="Pop_up_footer">
                    <input type="button" class="btn btn-primary" value="确定" id="btnGenerate" />
                    <a class="btn btn-default" id="btnClearSku">清空</a>
                </div>
            </div>
        </div>
        <div class="Pop_up" id="divUploadSKUImage" style="display: none;">
            <h1>
                <span id="skuImgTitle"></span>
            </h1>
            <div class="img_datala">
                <i class="iconfont">&#xe601;</i>
            </div>
            <div style="width: 530px; height: 350px; overflow: auto; margin-left: 20px; margin-top: 10px">
                <div id="skuImageContainer">
                </div>
            </div>
            <div class="Pop_footer">
                <input type="button" value="确定" class="btn btn-primary" id="btnUploadSkuImageOK" onclick="CloseDiv('divUploadSKUImage');" />
            </div>
        </div>

        <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" language="javascript">
        function ShowNotes(index) {

            document.getElementById("notes1").style.display = "none";
            document.getElementById("notes2").style.display = "none";
            var notesId = "notes" + index;
            document.getElementById(notesId).style.display = "block";

            document.getElementById("tip1").className = "";
            document.getElementById("tip2").className = "";
            var tipId = "tip" + index;
            document.getElementById(tipId).className = "off"
        }

        // 初始化图片上传控件
        function initImageUpload() {
            var imgSrcs = '<%=hidOldImages.Value%>';
            var arySrcs = imgSrcs.split(',');
            $('#imageContainer span[name="productImages"]').hishopUpload(
                           {
                               title: '商品图片',
                               url: "../../admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: arySrcs,
                               imgFieldName: "productImages",
                               defaultImg: '/Images/default_100x100.png',
                               pictureSize: '',
                               imagesCount: 5,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!doSubmit()) return false;
            var aryImgs = $('#imageContainer span[name="productImages"]').hishopUpload("getImgSrc");
            var imgSrcs = "";
            $(aryImgs).each(function () {
                imgSrcs += this + ",";
            });
            $("#<%=hidUploadImages.ClientID%>").val(imgSrcs);

            //规格图片
            var imgSrcs = "";
            $('#skuImageContainer span[name^="skuImages"]').each(function (i, obj) {
                var attrid = $(obj).attr("attrId");
                var oid = $(obj).attr("name");
                var skuId = oid.substring(10);
                var imgSrc = $(obj).hishopUpload("getImgSrc");
                if (imgSrc.length > 0) {
                    imgSrcs += attrid + "=" + skuId + "=" + imgSrc + ",";
                }
            });
            if (imgSrcs.length > 0) {
                imgSrcs = imgSrcs.substring(0, imgSrcs.length - 1);
            }
            $("#<%= hidSKUUploadImages.ClientID %>").val(imgSrcs);
            return true;
        }
        //初始化规格上传图片控件
        function initSKUImageUpload() {
            bindSkuImages();
            $("#skuImgTitle").html(uploadSKUImg.value);
            DivWindowOpen(550, 500, 'divUploadSKUImage');
        }
        function bindSkuImages() {
          

            var attributeId = uploadSKUImg.getAttribute("attrId");

            var oldSkuIDs = ",";
            $('#skuImageContainer span[name^="skuImages"]').each(function (i, obj) {
                var oid = $(obj).attr("name");
                var attrid = $(obj).attr("attrId");
                var skuId = oid.substring(10);
                oldSkuIDs += skuId + ",";
            });
            var obj = new Object();
            if (oldSkuIDs == ",") {//如果没有上传规格图，则取之前添加的
                var oldSkuSrcs = '<%=hidSKUOldImages.Value%>';
                var oldSrcs = oldSkuSrcs.split(',');
                
                for (var i = 0; i < oldSrcs.length; i++) {
                    var valueId = oldSrcs[i].split('=')[0];
                    var imgSrc = oldSrcs[i].split('=')[1];
                    obj[valueId] = imgSrc;
                }

                var selectedSku = ",";
                $('#skuTableHolder div[id^="skuDisplay_"][id$="' + attributeId + '"]').each(function (i, obj) {
                    var valueid = $(obj).attr("valueid");
                    if (selectedSku.indexOf("," + valueid + ",") < 0) {
                        selectedSku += valueid + ",";
                    }
                });
            }
            else {
                var skuImgObj = $("#skuImageContainer");
                var attributeId = uploadSKUImg.getAttribute("attrId");
                var skuValues = htSkuFields.get(attributeId).SKUValues;
                var selectedSku = ",";

                $('#skuTableHolder div[id^="skuDisplay_"][id$="' + attributeId + '"]').each(function (i, obj) {
                    var valueid = $(obj).attr("valueid");
                    if ((oldSkuIDs == "," && selectedSku.indexOf("," + valueid + ",") < 0) || (selectedSku.indexOf("," + valueid + ",") < 0 && oldSkuIDs.indexOf("," + valueid + ",") < 0 && oldSkuIDs != ",")) {
                        selectedSku += valueid + ",";
                    }
                    obj[valueId] = '';
                });
            }
            var skuImgObj = $("#skuImageContainer");
            if (attributeId != 0) {
                var skuValues = htSkuFields.get(attributeId).SKUValues;
                var oldValueIds = "";
                $.each(skuValues, function (i, skuValue) {                 
                    if (selectedSku.indexOf("," + skuValue.ValueId + ",") >= 0) {
                        var valueItem = String.format("<span style='display:inline-block;vertical-align:top;'>{0}：</span><span name='skuImages_{1}' attrId='{2}' class='imgbox' style='display:inline-block'></span>&nbsp;&nbsp;&nbsp;&nbsp;{3}", decodeURI(skuValue.ValueStr), skuValue.ValueId, attributeId, (i + 1) % 3 == 0 ? "<br /><br />" : "");
                        $("#skuImageContainer").append(valueItem);
                        var str = oldValueIds.replace("'" + skuValue.ValueId + "'", "");
                        if (str.length == oldValueIds.length) {
                            oldValueIds += "'" + skuValue.ValueId + "'";
                            $('#skuImageContainer span[name="skuImages_' + skuValue.ValueId + '"]').hishopUpload(
                            {
                                title: '规格图片',
                                url: "../../admin/UploadHandler.ashx?action=newupload",
                                imageDescript: '',
                                displayImgSrc: obj[skuValue.ValueId],
                                imgFieldName: "skuImages_" + skuValue.ValueId,
                                defaultImg: '',
                                pictureSize: '',
                                imagesCount: 1,
                                dataWidth: 9
                            });
                        }
                    }
                });
            }
        }
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtProductName', 1, 60, false, null, '商品名称字符长度在1-60之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 0, 300, true, null, '商品简介字符长度在0-300之间'));

            //initValid(new InputValidator('ctl00_contentHolder_txtDisplaySequence', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'))
            //appendValid(new NumberRangeValidator('ctl00_contentHolder_txtDisplaySequence', 1, 9999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtProductCode', 0, 20, true, null, '商家编码的长度不能超过20个字符'));

            initValid(new InputValidator('ctl00_contentHolder_txtCostPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCostPrice', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtMarketPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMarketPrice', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtSku', 0, 20, true, null, '货号的长度不能超过20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtStock', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));
            initValid(new InputValidator('ctl00_contentHolder_txtWarningStock', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtStock', 1, 9999999, '输入的数值超出了系统表示范围'));
            var isbool = true;
            if ($("#ctl00_contentHolder_hidOpenMultReferral").val() == "1")
                var isbool = $("#ctl00_contentHolder_txtSubMemberDeduct").val().length > 0 ? false : true;
            initValid(new InputValidator('ctl00_contentHolder_txtSecondLevelDeduct', 1, 10, isbool, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '输入的数值超出了系统表示范围'))
            initValid(new InputValidator('ctl00_contentHolder_txtThreeLevelDeduct', 1, 10, isbool, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '输入的数值超出了系统表示范围'))

            $("#ctl00_contentHolder_txtSecondLevelDeduct").change(function () { setDeductAllowEmpty(); });
            $("#ctl00_contentHolder_txtSubMemberDeduct").change(function () { setDeductAllowEmpty(); });
            $("#ctl00_contentHolder_txtThreeLevelDeduct").change(function () { setDeductAllowEmpty(); });

        }
        $(document).ready(function () { InitValidators(); initImageUpload(); setcontrol(); });
        function setDeductAllowEmpty() {
            if (($("#ctl00_contentHolder_txtSecondLevelDeduct").val().length + $("#ctl00_contentHolder_txtSubMemberDeduct").val().length + $("#ctl00_contentHolder_txtThreeLevelDeduct").val().length) > 0) {

                var b = $("#ctl00_contentHolder_hidOpenMultReferral").val() == "1" ? true : false;
                initValid(new InputValidator('ctl00_contentHolder_txtSubMemberDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '佣金比例必须同时填写或同时为空，且输入的数值不可超出系统表示范围'));
                if (b) {
                    initValid(new InputValidator('ctl00_contentHolder_txtSecondLevelDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '佣金比例必须同时填写或同时为空，且输入的数值不可超出系统表示范围'));
                    initValid(new InputValidator('ctl00_contentHolder_txtThreeLevelDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '佣金比例必须同时填写或同时为空，且输入的数值不可超出系统表示范围'));
                }
            } else {
                $("#ctl00_contentHolder_txtSecondLevelDeductTip").html("");
                $("#ctl00_contentHolder_txtSubMemberDeductTip").html("");
                $("#ctl00_contentHolder_txtThreeLevelDeductTip").html("");
                $("#ctl00_contentHolder_txtSecondLevelDeduct").removeClass("errorFocus");
                $("#ctl00_contentHolder_txtSubMemberDeduct").removeClass("errorFocus");
                $("#ctl00_contentHolder_txtThreeLevelDeduct").removeClass("errorFocus");

                initValid(new InputValidator('ctl00_contentHolder_txtSecondLevelDeduct', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '输入的数值超出了系统表示范围'))
                initValid(new InputValidator('ctl00_contentHolder_txtSubMemberDeduct', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '输入的数值超出了系统表示范围'))
                initValid(new InputValidator('ctl00_contentHolder_txtThreeLevelDeduct', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '输入的数值超出了系统表示范围'))

            }
        }

        function setcontrol() {
            if ($("#ctl00_contentHolder_hidHasActivity").val() == "1") {
                //开启规格

                $("#btnEnableSku").attr("disabled", true);
                //if ($("#ctl00_contentHolder_hidHasSku").val() == "0") {
                //    firstOpenSku();
                //}
            }
            else if ($("#ctl00_contentHolder_hidHasActivity").val() == "2") {
                $("#skuFieldHolder").hide();
                $("#skuContent").children("input").attr("disabled", true);
                $('#skuTableHolder input[id^="salePrice_"]').attr("disabled", true);
                $('#skuTableHolder input[id^="qty_"]').attr("disabled", true);
                $('#skuTableHolder div[id^="skuDisplay_"]').unbind("click");
                $('#skuTableHolder a[id^="deleSku_"]').hide();
                $('#skuTableHolder sup.glyphicon-remove').hide();
                $('#btnAddItem').hide();
                $('#batchMarketPrice').hide();
                $('#batchStore').hide();
                $("#btnUploadSKUImg").css("background-color", "gray");
            }
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $("#next").click(function () {
                if (!doSubmit()) return false;
                var costPrice = true;
                $(".skuItem_CostPrice.form-control").each(function (i, item) {
                    if ($(this).val() == "" || parseFloat($(this).val()) == 0) {
                        costPrice = false;
                        return false;
                    }
                });
                if ($("#skuRow").attr("style") != null && $("#skuRow").attr("style").indexOf("none") > -1) {
                    if ($("#ctl00_contentHolder_txtCostPrice").val() == "")
                        costPrice = false;// ShowMsg("请填写供货价！", false);
                }
                if (!costPrice) {
                    ShowMsg("请填写供货价！", false);
                    return false;
                }

                if (!checkShippingTemplates()) return false;
                $(this).hide();
                $("#info").css({
                    background: '#ff551f',
                    color: '#fff'
                });
                $("#information").css("background", "#ff9979 ");
                $('body,html').animate({ scrollTop: 0 });
                $("#pre").show();
                $("#ctl00_contentHolder_btnSave").show();
                $("#ctl00_contentHolder_btnSaveAudit").show();
            })

            $("#pre").click(function () {
                $(this).hide();
                $("#info").css({
                    background: '#ff9979',
                    color: '#fff'
                });
                $("#information").css("background", "#ff551f ");
                $('body,html').animate({ scrollTop: 0 }, 300);
                $("#next").show();
                $("#ctl00_contentHolder_btnSave").hide();
                $("#ctl00_contentHolder_btnSaveAudit").hide();
            });

            $(".img_toggle").click(function () {
                if ($(this).children(".icon_down").is(":hidden")) {
                    $(this).children(".icon_fold").hide();
                    $(this).children(".icon_down").show();
                } else {
                    $(this).children(".icon_fold").show();
                    $(this).children(".icon_down").hide();
                }
                $(this).next().slideToggle();
            });
        })

        function enableSku() {
            setCtrlDisplay("none");
            skuRow.style.display = "";
            cancelValid();
            prepareSkus();
            skuEnabled = true;
            $("#ctl00_contentHolder_chkSkuEnabled").iCheck('check');
        }
        function firstOpenSku() {
            $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2");
            showSkuValue();
        }
    </script>
    <script src="../js/SupplierProduct.js"></script>
</asp:Content>
