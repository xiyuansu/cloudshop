<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddProduct" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register Src="../Ascx/ImageList.ascx" TagName="ImageList" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <script type="text/javascript" src="/utility/jquery_hashtable.js"></script>
    <script type="text/javascript" src="/utility/jquery-powerFloat-min.js"></script>
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
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

        .inputitem {
            margin-bottom: 15px;
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

        <div class="datafrom">
            <span style="display: none;">
                <asp:HiddenField ID="hidUploadImages" runat="server" />
                <asp:HiddenField ID="hidOldImages" runat="server" />
                <asp:HiddenField ID="hidSKUUploadImages" runat="server" />
                <asp:HiddenField ID="hidSKUOldImages" runat="server" />
                <input type="hidden" id="hidValuationMethod" value="0" />
                <asp:HiddenField ID="hidOpenMultReferral" runat="server" Value="1" />
                <asp:HiddenField ID="hidOpenSecondReferral" runat="server" ClientIDMode="Static" Value="0" />
                <asp:HiddenField ID="hidOpenThirdReferral" runat="server" Value="0" ClientIDMode="Static"/>
            </span>
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
                                    <span class="formitemtitle">所属分类：</span>
                                    <span style="line-height: 32px;">
                                        <asp:Literal runat="server" ID="litCategoryName"></asp:Literal></span>&nbsp;
                                 <asp:HyperLink runat="server" ID="lnkEditCategory" CssClass="colorBlue" Text="编辑" Style="line-height: 32px;"></asp:HyperLink>
                                </li>
                                <li>
                                    <span class="formitemtitle">商品种类：</span>
                                    <div class="icheck_group">
                                        <asp:RadioButton runat="server" ID="radPhysicalProduct" GroupName="productType" Checked="true" Text="实物商品" CssClass="icheck"></asp:RadioButton>
                                        <asp:RadioButton runat="server" ID="radServiceProduct" GroupName="productType" Text="服务类商品" CssClass="icheck"></asp:RadioButton>
                                    </div>
                                    <p style="height: 16px; line-height: 16px; color: #999;">(发布后不能修改)</p>
                                </li>
                                <li><span class="formitemtitle">商品类型：</span>
                                    <abbr class="formselect">
                                        <Hi:ProductTypeDownList runat="server" CssClass="productType iselect" ID="dropProductTypes" NullToDisplay="请选择商品类型" /></abbr>
                                </li>
                                <li>
                                    <span class="formitemtitle">品牌：</span>
                                    <abbr class="formselect">
                                        <Hi:BrandCategoriesDropDownList runat="server" CssClass="iselect" ID="dropBrandCategories" NullToDisplay="请选择品牌" /></abbr>
                                </li>
                                <li style="margin-bottom: 0;"><span class="formitemtitle"><em>*</em>商品名称：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form-control form_input_l" ID="txtProductName" placeholder=" 限定在60个字符" />
                                    <p id="ctl00_contentHolder_txtProductNameTip">&nbsp;</p>
                                </li>
                                <li class="clearfix"><span class="formitemtitle">商品简介：</span>
                                    <Hi:TrimTextBox runat="server" Rows="6" Height="100px" Columns="76" ID="txtShortDescription" TextMode="MultiLine" CssClass="form-control form_input_l" placeholder="限定在300个字符以内" />
                                </li>
                                <li class="clearfix" id="l_tags" runat="server">
                                    <span class="formitemtitle">商品标签定义：</span>
                                    <div id="div_tags">
                                        <div id="f_div" class="icheck-label-5-10 pull-left">
                                            <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
                                        </div>
                                        <div style="width: 100%; float: left;">
                                            <a id="a_addtag" href="javascript:void(0)" onclick="javascript:AddTags()" class="input-group-a">添加</a>
                                            <div id="div_addtag" style="display: none;">
                                                <input type="text" id="txtaddtag" class="form-control" style="margin: 0; float: left" />
                                                <input type="button" style="float: left; margin-left: 10px;" class="btn btn-primary" value="保存" onclick="return AddAjaxTags()" />
                                            </div>
                                        </div>
                                    </div>
                                    <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                                </li>
                                <li><span class="formitemtitle">商家编码：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtProductCode" onblur="isExistProductCode(this)" placeholder="限定在50个字符" />
                                </li>
                                <li id="unit"><span class="formitemtitle">计量单位：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtUnit" placeholder="输入中文或英文，例:g/元" />
                                </li>
                                <a name="aShippingTemplate" id="aShippingTemplate"></a>
                                <li id="liIsCrossborder" runat="server"><span class="formitemtitle">是否跨境商品：</span>
                                    <div class="icheck_group">
                                        <asp:CheckBox ID="chkIsCrossborder" runat="server" CssClass="icheck" Text="&nbsp;" />
                                    </div>
                                </li>
                                <li><span class="formitemtitle">销售状态：</span>
                                    <div class="icheck_group">
                                        <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus" Checked="true" Text="出售中" CssClass="icheck"></asp:RadioButton>
                                        <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus" Text="下架区" CssClass="icheck"></asp:RadioButton>
                                        <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus" Text="仓库中" CssClass="icheck"></asp:RadioButton>
                                    </div>
                                </li>

                                <li id="shippingTemplates"><span class="formitemtitle"><em>*</em>运费模板：</span>
                                    <abbr class="formselect">
                                        <Hi:ShippingTemplatesDropDownList runat="server" CssClass="shippingTemplates iselect" ID="ShippingTemplatesDropDownList" NullToDisplay="请选择运费模板" /></abbr>
                                    <a href="../Sales/AddShippingTemplate.aspx" class="input-group-a">&nbsp;&nbsp;新增运费模板</a>
                                </li>
                                <li id="weightRow" style="display: none;"><span class="formitemtitle"><em>*</em>商品重量：</span>
                                    <div class="input-group">
                                        <Hi:TrimTextBox runat="server" CssClass="form_input_s form-control" ID="txtWeight" Text="0" placeholder="请输入数字,限制为0-100000" ClientIDMode="Static" />
                                        <span class="input-group-addon">KG</span>
                                    </div>
                                </li>
                                <li id="volumeRow" style="display: none;"><span class="formitemtitle"><em>*</em>商品体积：</span>
                                    <div class="input-group">
                                        <Hi:TrimTextBox runat="server" CssClass="form_input_s form-control" ID="txtVolume" Text="0" placeholder="请输入数字,限制为0-100000" ClientIDMode="Static" />
                                        <span class="input-group-addon">M<sup>3</sup></span>
                                    </div>
                                </li>
                                <li style="display: none;"><span class="formitemtitle">商品包邮： </span>
                                    <Hi:OnOff runat="server" ID="ChkisfreeShipping"></Hi:OnOff>
                                </li>

                                <li style="display: none;"><span class="formitemtitle">会员打折：</span>
                                    <Hi:YesNoRadioButtonList runat="server" RepeatLayout="Flow" ID="radlEnableMemberDiscount"
                                        YesText="参与会员打折" NoText="不参与会员打折" />
                                </li>
                            </ul>
                        </div>


                        <div id="serviceProduct" style="display: none;" class="info_group">
                            <h2 class="colorE">服务类商品设置</h2>
                            <a class="img_toggle" href="javascript:;">
                                <img src="/admin/images/icon_fold.png" class="icon_fold" />
                                <img src="/admin/images/icon_down.png" class="icon_down" style="display: none" />
                            </a>
                            <ul>
                                <li><span class="formitemtitle">有效使用时间 ：</span>
                                    <div class="icheck_group">
                                        <asp:RadioButton runat="server" ID="valid" GroupName="isValid" Checked="true" Text="长期有效" CssClass="icheck"></asp:RadioButton>
                                        <asp:RadioButton runat="server" ID="customValid" GroupName="isValid" Text="自定义日期" CssClass="icheck" Style="margin-right: 0px;"></asp:RadioButton>
                                        <div id="validDate" style="display: none;">
                                            <Hi:CalendarPanel ClientIDMode="Static" runat="server" ID="validStartDate" Width="125"></Hi:CalendarPanel>
                                            <span class="Pg_1010">至</span>
                                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="validEndDate" IsEndDate="true" Width="125"></Hi:CalendarPanel>
                                        </div>
                                    </div>
                                </li>
                                <li><span class="formitemtitle">有效期内支持退款 ：</span>
                                    <div class="icheck_group">
                                        <asp:RadioButton runat="server" ID="IsRefund" GroupName="IsRefund" Checked="true" Text="是" CssClass="icheck"></asp:RadioButton>
                                        <asp:RadioButton runat="server" ID="IsRefund2" GroupName="IsRefund" Text="否" CssClass="icheck"></asp:RadioButton>
                                    </div>
                                </li>
                                <li id="overRefund"><span class="formitemtitle">支持过期退款 ：</span>
                                    <div class="icheck_group">
                                        <asp:RadioButton runat="server" ID="IsOverRefund" GroupName="IsOverRefund" Checked="true" Text="是" CssClass="icheck"></asp:RadioButton>
                                        <asp:RadioButton runat="server" ID="IsOverRefund2" GroupName="IsOverRefund" Text="否" CssClass="icheck"></asp:RadioButton>
                                    </div>
                                </li>
                                <li><span class="formitemtitle">信息填写项 ：</span>
                                    <div style="float: left; width: 540px; line-height: 28px;">
                                        <div id="itemcontent">
                                            <input type="hidden" id="hidJson" runat="server" clientidmode="static" />
                                            <div id="index1" class="inputitem">
                                                <input type="text" id="inputtitle_1" placeholder="字段标题" value="" class="form-control" style="width: 100px; float: left;" />
                                                <select class="form-control" id="inputtype_1" style="width: 100px; float: left; margin: 0 10px;">
                                                    <option value="1">文本格式</option>
                                                    <option value="2">日期</option>
                                                    <option value="3">身份证</option>
                                                    <option value="4">手机</option>
                                                    <option value="5">数字格式</option>
                                                    <option value="6">图片</option>
                                                </select>
                                                <input type="checkbox" id="isrequired_1" class="icheck icheck-label-5-10 pull-left" />&nbsp;&nbsp;必填
                                                <a href="javascript:;" onclick="DeleteItems(1);" class="input-group-a">&nbsp;&nbsp;删除</a>
                                            </div>
                                        </div>
                                        <div style="width: 100%; float: left;">
                                            <a href="javascript:;" onclick="AddItems();" class="input-group-a">+ 添加字段</a>
                                        </div>
                                    </div>
                                </li>
                                <li id="generateMore"><span class="formitemtitle">填写项是否生成多份 ：</span>
                                    <div class="icheck_group">
                                        <asp:RadioButton runat="server" ID="IsGenerateMore" GroupName="IsGenerateMore" Checked="true" Text="是" CssClass="icheck"></asp:RadioButton>
                                        <asp:RadioButton runat="server" ID="IsGenerateMore2" GroupName="IsGenerateMore" Text="否" CssClass="icheck"></asp:RadioButton>
                                    </div>
                                    <p style="line-height: 20px; color: #999; padding-left: 0px;">
                                        若设置是，则会根据买家购买份数，系统自动生成多条填写记录，供买家填写；<br>
                                        若设为否，则无论买家购买多少份，都只生成一条填写记录，供买家填写
                                    </p>
                                </li>
                            </ul>
                        </div>

                        <ul id="attributeRow" style="display: none;">
                            <li>
                                <h2 class="colorE" style="float: left">扩展属性</h2>
                                <a class="img_toggle" href="javascript:;">
                                    <img src="/admin/images/icon_fold.png" class="icon_fold" />
                                    <img src="/admin/images/icon_down.png" class="icon_down" style="display: none" />
                                </a>
                            </li>

                            <li class="icheck-label-5-10">
                                <div class="attributeContent" id="attributeContent">
                                </div>
                                <Hi:TrimTextBox runat="server" ID="txtAttributes" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                            </li>

                        </ul>

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
                                <li id="salePriceRow" class="mb_0"><span class="formitemtitle"><em>*</em>一口价：</span>
                                    <div class="input-group">
                                        <span class="input-group-addon">￥</span>
                                        <Hi:TrimTextBox runat="server" CssClass="form-control form_input_s" ID="txtSalePrice" />
                                        <a href="javascript:;" onclick="editProductMemberPrice();" class="input-group-a">&nbsp;&nbsp;编辑会员价</a>
                                    </div>
                                    <p id="ctl00_contentHolder_txtSalePriceTip">&nbsp;</p>
                                    <Hi:TrimTextBox runat="server" ID="txtMemberPrices" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>

                                </li>
                                <li id="costPriceRow"><span class="formitemtitle">成本价：</span>
                                    <div class="input-group">
                                        <span class="input-group-addon">￥</span>
                                        <Hi:TrimTextBox runat="server" CssClass="form-control form_input_s" ID="txtCostPrice" />
                                    </div>
                                </li>
                                <li id="skuCodeRow"><span class="formitemtitle">货号：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtSku" onblur="isExistSkuCode(this)" paceholder="限定在20个字符" />
                                </li>
                                <li id="qtyRow" class="mb_0"><span class="formitemtitle"><em>*</em>商品库存：</span><Hi:TrimTextBox
                                    runat="server" CssClass="form_input_m form-control" ID="txtStock" />
                                    <p id="ctl00_contentHolder_txtStockTip"></p>
                                </li>
                                <li id="warningqtyRow" class="mb_0"><span class="formitemtitle"><em>*</em>警戒库存：</span><Hi:TrimTextBox
                                    runat="server" CssClass="form_input_m form-control" ID="txtWarningStock" Text="0" />
                                    <p id="ctl00_contentHolder_txtWarningStockTip"></p>
                                </li>

                                <li id="enableSkuRow" style="display: none;"><span class="formitemtitle">规格：</span>
                                    <input id="btnEnableSku" class="btn btn-primary" type="button" value="开启规格" />
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
                                                <input type="text" id="txtBatchCostPrice" class="form-control" placeholder="成本价" /></dd>
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
                                    <p id="skuFieldHolder" style="padding: 0; margin: 0; padding: 0; display: none;">
                                    </p>
                                    <div style="position: relative; float: left; width: 100%;">
                                        <div id="skuTableHolder">
                                        </div>
                                        <input type="button" id="btnAddItem" value="" />

                                    </div>
                                    <Hi:TrimTextBox runat="server" ID="txtSkus" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                                    <asp:CheckBox runat="server" ID="chkSkuEnabled" Style="display: none;" />

                                </li>
                            </ul>
                        </div>

                        <div class="info_group">
                            <h2 class="colorE">搜索优化</h2>
                            <a class="img_toggle" href="javascript:;">
                                <img src="/admin/images/icon_fold.png" class="icon_fold" />
                                <img src="/admin/images/icon_down.png" class="icon_down" style="display: none" />
                            </a>
                            <ul>
                                <li><span class="formitemtitle">详细页标题：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_l form-control" ID="txtTitle" placeholder="限制在100字符以内" />

                                </li>
                                <li><span class="formitemtitle">详细页描述：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_l form-control" ID="txtMetaDescription" placeholder="限制在260字符以内" />

                                </li>
                                <li><span class="formitemtitle">详细页搜索关键字：</span>
                                    <Hi:TrimTextBox runat="server" CssClass="form_input_l form-control" ID="txtMetaKeywords" placeholder="限制在160字符以内" />
                                </li>
                            </ul>
                        </div>


                        <div class="info_group" id="ReferralConfig" runat="server">
                            <h2 class="colorE clear">佣金设置</h2>
                            <a class="img_toggle" href="javascript:;">
                                <img src="/admin/images/icon_fold.png" class="icon_fold" />
                                <img src="/admin/images/icon_down.png" class="icon_down" style="display: none" />
                            </a>
                            <ul>
                                <li id="enableDeduct" runat="server" clientidmode="Static">
                                    <span class="formitemtitle">开启佣金</span>
                                    <Hi:OnOff runat="server" ID="ofEnableDeduct"></Hi:OnOff>
                                    <span style="color: #999">&nbsp; &nbsp;关闭后该商品将不产生佣金</span>
                                </li>

                                <li id="liSubMemberDeduct" runat="server" style="display: none;" clientidmode="Static" class="mb_0">
                                    <div class="input-group">
                                        <span class="formitemtitle">会员直接上级抽佣比例：</span>
                                        <asp:TextBox ID="txtSubMemberDeduct" CssClass="form_input_m form-control" runat="server" MaxLength="10" placeholder="A邀请了B，B后续消费A所能获得的佣金" />
                                        <span class="input-group-addon">%</span>
                                    </div>
                                    <p id="ctl00_contentHolder_txtSubMemberDeductTip"></p>
                                </li>
                                <li id="liReferralDeduct" runat="server" style="display: none;" clientidmode="Static" class="mb_0">
                                    <div class="input-group">
                                        <span class="formitemtitle">会员上二级抽佣比例：</span>
                                        <asp:TextBox runat="server" CssClass="form_input_m form-control" ID="txtSecondLevelDeduct" MaxLength="10" placeholder="A邀请的会员B后续发展了其它会员C，C消费时，A所能获得的佣金" />
                                        <span class="input-group-addon">%</span>
                                    </div>
                                    <p id="ctl00_contentHolder_txtSecondLevelDeductTip"></p>
                                </li>
                                <li id="liSubReferralDeduct" runat="server" style="display: none;" clientidmode="Static" class="mb_0">
                                    <div class="input-group">
                                        <span class="formitemtitle">会员上三级抽佣比例：</span>
                                        <asp:TextBox ID="txtThreeLevelDeduct" CssClass="form_input_m form-control" runat="server" MaxLength="10" placeholder="A邀请的会员B后续发展了会员C，C又发展了会员D，D消费时，A所能获得的佣金" />
                                        <span class="input-group-addon">%</span>
                                    </div>
                                    <p id="ctl00_contentHolder_txtThreeLevelDeductTip"></p>
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
                                        <Hi:Ueditor ID="editDescription" runat="server" Width="660" />
                                    </div>
                                    <div id="notes2" style="float: left; margin-left: 250px; display: none;">
                                        <Hi:Ueditor ID="editmobbileDescription" runat="server" Width="660" ImportLib="1" />
                                    </div>
                                </span>
                                    <p style="margin-bottom: 40px;">
                                        <asp:CheckBox runat="server" CssClass="icheck" ID="ckbIsDownPic" Text="是否下载商品描述外站图片" />
                                        <i class="icon_query" style="margin: 16px 0 0 10px;" data-toggle="tooltip" data-placement="top" title="如果勾选此选项时，商品里面有外站的图片则会下载到本地，相反则不会，由于要下载图片，如果图片过多或图片很大，需要下载的时间就多，请慎重选择。"></i>
                                    </p>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>

                <div class="footer_btn">
                    <a href="#info_c" id="next" class="btn btn-primary" data-toggle="tab">下一步</a>
                    <a class="btn btn-default" id="pre" href="#information_c" data-toggle="tab" style="display: none">上一步</a>
                    <asp:Button runat="server" ID="btnAdd" Style="display: none" Text="完成" OnClientClick="return getUploadImages();"
                        CssClass="btn  btn-primary" />
                </div>


            </div>
        </div>
        <div class="Pop_up " id="priceBox" style="display: none;">
            <h1>
                <span id="popTitle"></span>
            </h1>
            <div class="img_datala">
                <span class="glyphicon glyphicon-remove"></span>
            </div>
            <div class="mianform ">
                <div id="priceContent">
                </div>
                <div style="margin-top: 10px; text-align: right; padding: 10px 15px 0 0; border-top: 1px solid #ddd">
                    <input type="button" class="btn btn-primary" value="确定" onclick="doneEditPrice();" />
                    <input type="button" class="btn btn-default  close_pop ml_20" value="取消" />
                </div>
            </div>
        </div>
        <div class="Pop_up" id="skuValueBox" style="display: none; padding-bottom: 52px;">
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
            <div style="width: 530px; height: 350px; overflow: auto; margin-left: 20px; margin-top: 20px">
                <div id="skuImageContainer">
                </div>
            </div>
            <div class="Pop_footer">
                <input type="button" class="btn btn-primary" value="确定" id="btnUploadSkuImageOK" onclick="CloseDiv('divUploadSKUImage');" />
            </div>
        </div>
    </div>
    <!-- start ImgPicker -->
    <uc1:ImageList ID="ImageList" runat="server" />

    <div style="display: none;" id="Demo">
        <div id="indexDemo" class="inputitem">
            <input type="text" id="inputtitle" value="" placeholder="字段标题" class="form-control" style="width: 100px; float: left;" />
            <select class="form-control" id="inputtype" style="width: 100px; float: left; margin: 0 10px;">
                <option value="1">文本格式</option>
                <option value="2">日期</option>
                <option value="3">身份证</option>
                <option value="4">手机</option>
                <option value="5">数字格式</option>
                <option value="6">图片</option>
            </select>
            <input type="checkbox" id="isrequired" class="icheck-label-5-10 pull-left" />&nbsp;&nbsp;必填
            <a href="javascript:;" onclick="DeleteItems(datas);" class="input-group-a">&nbsp;&nbsp;删除</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" src="attributes.helper.js?v=3.0"></script>
    <script type="text/javascript" src="grade.price.helper.js?v=3.0"></script>
    <script type="text/javascript" src="producttag.helper.js?v=3.0"></script>
    <script type="text/javascript" language="javascript">
        // 初始化图片上传控件
        function initImageUpload() {
            $('#imageContainer span[name="productImages"]').hishopUpload(
                           {
                               title: '商品图片',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: '',
                               imgFieldName: "productImages",
                               defaultImg: '',
                               pictureSize: '',
                               imagesCount: 5,
                               dataWidth: 9
                           });

        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!doSubmit()) return false;
            //商品图片
            var aryImgs = $('#imageContainer span[name="productImages"]').hishopUpload("getImgSrc");
            var imgSrcs = "";
            $(aryImgs).each(function () {
                imgSrcs += this + ",";
            });
            $("#<%=hidUploadImages.ClientID%>").val(imgSrcs);
            //规格图片
            var imgSrcs = "";
            $('#skuImageContainer span[name^="skuImages"]').each(function (i, obj) {
                var oid = $(obj).attr("name");
                var attrid = $(obj).attr("attrId");
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
        //初始化规格上传图片控件
        function initSKUImageUpload() {
            bindSkuImages();
            $("#skuImgTitle").html(uploadSKUImg.value);
            DivWindowOpen(550, 500, 'divUploadSKUImage');
        }
        function bindSkuImages() {
            var oldSkuIDs = ",";
            $('#skuImageContainer span[name^="skuImages"]').each(function (i, obj) {
                var oid = $(obj).attr("name");
                var attrid = $(obj).attr("attrId");
                var skuId = oid.substring(10);
                oldSkuIDs += skuId + ",";
            });
            var skuImgObj = $("#skuImageContainer");
            var attributeId = uploadSKUImg.getAttribute("attrId");
            var skuValues = htSkuFields.get(attributeId).SKUValues;
            var selectedSku = ",";
            $('#skuTableHolder div[id^="skuDisplay_"][id$="' + attributeId + '"]').each(function (i, obj) {
                var valueid = $(obj).attr("valueid");
                if ((oldSkuIDs == "," && selectedSku.indexOf("," + valueid + ",") < 0) || (selectedSku.indexOf("," + valueid + ",") < 0 && oldSkuIDs.indexOf("," + valueid + ",") < 0 && oldSkuIDs != ",")) {
                    selectedSku += valueid + ",";
                }
            });
            $.each(skuValues, function (i, skuValue) {
                if (selectedSku.indexOf("," + skuValue.ValueId + ",") >= 0) {
                    var valueItem = String.format("<span style='display:inline-block;vertical-align:top;'>{0}：</span><span name='skuImages_{1}' attrId='{2}' class='imgbox' style='display:inline-block'></span>&nbsp;&nbsp;&nbsp;&nbsp;{3}", decodeURI(skuValue.ValueStr), skuValue.ValueId, attributeId, (i + 1) % 3 == 0 ? "<br /><br />" : "");
                    skuImgObj.append(valueItem);
                    $('#skuImageContainer span[name="skuImages_' + skuValue.ValueId + '"]').hishopUpload(
                    {
                        title: '规格图片',
                        url: "/admin/UploadHandler.ashx?action=newupload",
                        imageDescript: '',
                        displayImgSrc: '',
                        imgFieldName: "skuImages_" + skuValue.ValueId,
                        defaultImg: '',
                        pictureSize: '500*500',
                        imagesCount: 1,
                        dataWidth: 9
                    });
                }
            });
        }

        function InitValidators() {
            //initValid(new SelectValidator('ctl00_contentHolder_dropProductLines', false, '请选择商品所属的产品线'));
            initValid(new InputValidator('ctl00_contentHolder_txtProductName', 1, 60, false, null, '商品名称字符长度在1-60之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtProductCode', 0, 50, true, null, '商家编码的长度不能超过50个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtSalePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '必须大于0.01元'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSalePrice', 0.01, 10000000, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtCostPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '必须大于0.01元'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCostPrice', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtMarketPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '必须大于0.01元'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMarketPrice', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtSku', 0, 20, true, null, '货号的长度不能超过20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtStock', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));
            initValid(new InputValidator('ctl00_contentHolder_txtWarningStock', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtStock', 1, 9999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtUnit', 1, 20, true, '[a-zA-Z\/\u4e00-\u9fa5]*$', '必须限制在20个字符以内且只能是英文和中文，例:g/元'));
            initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 0, 300, true, null, '商品简介必须限制在300个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 0, 100, true, null, '详细页标题长度限制在100个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 0, 260, true, null, '详细页描述长度限制在260个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtMetaKeywords', 0, 160, true, null, '详细页搜索关键字长度限制在160个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtSubMemberDeduct', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '输入的数值超出了系统表示范围'))
            var isbool = true;
            var isbool = $("#ctl00_contentHolder_txtSubMemberDeduct").length > 0 && $("#ctl00_contentHolder_txtSubMemberDeduct").val().length > 0 ? false : true;
            if ($("#hidOpenSecondReferral").val() == "1") {
                initValid(new InputValidator('ctl00_contentHolder_txtSecondLevelDeduct', 1, 10, isbool, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '输入的数值超出了系统表示范围'))
            }
            if ($("#hidOpenThirdReferral").val() == "1") {
                initValid(new InputValidator('ctl00_contentHolder_txtThreeLevelDeduct', 1, 10, isbool, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '输入的数值超出了系统表示范围'))
            }
            $("#ctl00_contentHolder_txtSecondLevelDeduct").change(function () { setDeductAllowEmpty(); });
            $("#ctl00_contentHolder_txtSubMemberDeduct").change(function () { setDeductAllowEmpty(); });
            $("#ctl00_contentHolder_txtThreeLevelDeduct").change(function () { setDeductAllowEmpty(); });
        }
        $(document).ready(function () { InitValidators(); initImageUpload(); });
        function setDeductAllowEmpty() {
            if (($("#ctl00_contentHolder_txtSecondLevelDeduct").val().length + $("#ctl00_contentHolder_txtSubMemberDeduct").val().length + $("#ctl00_contentHolder_txtThreeLevelDeduct").val().length) > 0) {
                initValid(new InputValidator('ctl00_contentHolder_txtSubMemberDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '佣金比例必须同时填写或同时为空，且输入的数值不可超出系统表示范围'));
                if ($("#hidOpenSecondReferral").val() == "1") {
                    initValid(new InputValidator('ctl00_contentHolder_txtSecondLevelDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '佣金比例必须同时填写或同时为空，且输入的数值不可超出系统表示范围'));
                }
                if ($("#hidOpenThirdReferral").val() == "1") {
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
    </script>
    <script type="text/javascript">
        $(function () {
            $("#next").click(function () {
                if (!doSubmit()) return false;
                if (!checkShippingTemplates()) return false;
                if ($("#ctl00_contentHolder_radServiceProduct").is(":checked") && $("#ctl00_contentHolder_customValid").is(":checked")) {
                    if ($("#validStartDate").val() == "" || $("#validEndDate").val() == "") {
                        alert("请选择自定义日期");
                        return false;
                    }
                    var startdate = new Date($("#validStartDate").val());
                    var enddate = new Date($("#validEndDate").val());
                    if (startdate > enddate) {
                        alert("开始时间必须小于结束时间");
                        return false;
                    }
                }
                $(this).hide();
                $("#info").css({
                    background: '#ff551f',
                    color: '#fff'
                });
                $("#information").css("background", "#ff9979 ");
                $('body,html').animate({ scrollTop: 0 });
                $("#pre").show();
                $("#ctl00_contentHolder_btnAdd").show();
                GetJsonByAward();
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
                $("#ctl00_contentHolder_btnAdd").hide();
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

            $("#ctl00_contentHolder_radPhysicalProduct").on('ifChecked', function (event) {
                $("#serviceProduct").hide();
                $("#unit").show();
                $("#shippingTemplates").show();
                GetInvild();
            });

            $("#ctl00_contentHolder_radServiceProduct").on('ifChecked', function (event) {
                $("#serviceProduct").show();
                $("#unit").hide();
                $("#shippingTemplates").hide();
                GetInvild();
            });

            $("#ctl00_contentHolder_valid").on('ifChecked', function (event) {
                $("#validDate").hide();
            });

            $("#ctl00_contentHolder_customValid").on('ifChecked', function (event) {
                $("#validDate").show();
            });

            $("#ctl00_contentHolder_IsRefund").on('ifChecked', function (event) {
                $("#overRefund").show();
                $("#ctl00_contentHolder_IsOverRefund").iCheck('check');
            });

            $("#ctl00_contentHolder_IsRefund2").on('ifChecked', function (event) {
                $("#overRefund").hide();
                $("#ctl00_contentHolder_IsOverRefund2").iCheck('check');
            });
        })

        // 开启规格
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

        function AddItems() {

            if ($("#itemcontent .inputitem").length == 0) {
                $("#ctl00_contentHolder_IsGenerateMore").iCheck('check');
                $("#generateMore").show();
            }
            var length = $("#itemcontent .inputitem").length + 1;
            var html = $("#Demo").html().replace("indexDemo", "index" + length).replace("inputtitle", "inputtitle_" + length).replace("inputtype", "inputtype_" + length).replace("isrequired", "isrequired_" + length).replace("datas", length).replace("icheck-label-5-10", "icheck icheck-label-5-10");
            $("#itemcontent").append(html);
            $('#itemcontent .icheck').iCheck({
                checkboxClass: 'icheckbox_square-red',
                radioClass: 'iradio_square-red',
                increaseArea: '20%',
            });
            GetInvild();
        }

        function GetInvild() {
            $("#itemcontent .inputitem").each(function (i) {
                var num = 1 + i;
                if ($("#ctl00_contentHolder_radServiceProduct").is(":checked")) {
                    initValid(new InputValidator('inputtitle_' + num, 1, 8, false, null, '字段标题字符长度在1-8之间'));
                } else {
                    initValid(new InputValidator('inputtitle_' + num, 1, 8, true, null, '字段标题字符长度在1-8之间'));
                }
            });
        }

        function DeleteItems(n) {
            $("#index" + n).remove();
            if ($("#itemcontent .inputitem").length == 0) {
                $("#ctl00_contentHolder_IsGenerateMore2").iCheck('check');
                $("#generateMore").hide();
            }
        }

        function GetJsonByAward() {

            var slength = $("#itemcontent .inputitem").length;
            var str = "";
            if (slength > 0) {
                str += "[";
                for (var i = 0; i < slength; i++) {
                    if (i >= 1) {
                        str += ',';
                    }
                    var num = i + 1;
                    inputtitle, inputtype, isrequired
                    str += '{';
                    var inputtitle = $("#inputtitle_" + num);
                    var inputtype = $("#inputtype_" + num);
                    var isrequired = $("#isrequired_" + num);
                    str += "\"InputFieldTitle\":\"" + inputtitle.val() + '\",';
                    str += "\"InputFieldType\":\"" + inputtype.val() + '\",';
                    str += "\"IsRequired\":" + isrequired.is(":checked");
                    str += '}';
                }

                str += ']';
            }
            $("#hidJson").val(str);

        }

    </script>
</asp:Content>
