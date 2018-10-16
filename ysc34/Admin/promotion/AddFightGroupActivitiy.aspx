<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddFightGroupActivitiy.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddFightGroupActivitiy" %>



<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function fuChangeStartDate(ev) {
            var clientId = ev.target.validator[0]._ValidateTargetId
            $("#" + clientId).trigger("blur");
        }

        function fuChangeEndDate(ev) {
            var clientId = ev.target.validator[0]._ValidateTargetId
            $("#" + clientId).trigger("blur");
        }
        function selectProduct() {
            var dataForm = {
                Price: $("#ctl00_contentHolder_txtPrice").val(),
                TotalCount: $("#ctl00_contentHolder_txtTotalCount").val(),
                StartTime: $("#ctl00_contentHolder_CPStartTime").val(),
                EndTime: $("#ctl00_contentHolder_CPEndDate").val(),
                Content: $("#ctl00_contentHolder_txtContent").val(),
                Icon: $(":hidden.hiddenImgSrc:eq(0)").val(),
                MaxCount: $("#ctl00_contentHolder_txtMaxCount").val(),
                JoinNumber: $("#ctl00_contentHolder_txtJoinNumber").val(),
                LimitedHour: $("#ctl00_contentHolder_txtLimitedHour").val()
            };
            var filterProductIds = '<%= this.filterProductIds%>';

            var url = "/Admin/ChoicePage/CPProducts.aspx?returnUrl=/Admin/promotion/AddFightGroupActivitiy.aspx&formData=" + JSON.stringify(dataForm) + "&filterProductIds=" + filterProductIds + "&isFilterFightGroupProduct=true&isFilterCountDownProduct=true&IsFilterGroupBuyProduct=true&isFilterCombinationBuyProduct=true&isFilterPreSaleProduct=true&isFilterCombinationBuyOtherProduct=true&isFilterPromotionProduct=true&isHasStock=true";
            DialogFrame(url, "选择商品", 1200, 600);
        }
        //获取上传成功后的图片路径
        function getUploadImages() {
            var productId = parseInt($("#ctl00_contentHolder_hfProductId").val());
            if (productId <= 0) {
                alert("请选择商品")
                return false;
            }

            if (!$("#txtFightGroupShareTitle").is(":hidden")) {
                if ($.trim($("#txtFightGroupShareTitle").val()) == "") {
                    alert("请输入分享标题");
                    return false;
                }
            }
            if ($.trim($("#txtFightGroupShareDetails").val()) == "") {
                alert("请输入分享详情");
                return false;
            }
            var srcLogo = $('#logoContainer span[name="siteLogo"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadLogo.ClientID%>").val(srcLogo);
            if (srcLogo == "") {
                alert("请上传活动图片")
                return false;
            }
            return PageIsValid();
        }
        $(document).ready(function () {
            $("#rbtlTitle_1").click(function () {
                $("#txtFightGroupShareTitle").show();
                $(this).parent().children().css("float", "left");
                $(this).parent().css("width", "400px");
                $(this).next().css("margin-right", "10px");
                $(this).parent().append($("#txtFightGroupShareTitle"));
            });
            $("#rbtlTitle_0").click(function () {
                $("#txtFightGroupShareTitle").hide();
            });
            $("#rbtlTitle_1:checked").trigger("click");

            var imgSrc = '<%=hidUploadLogo.Value%>';
            $('#logoContainer span[name="siteLogo"]').hishopUpload(
            {
                title: '缩略图',
                url: "/admin/UploadHandler.ashx?action=newupload",
                imageDescript: '',
                imgFieldName: "siteLogo",
                pictureSize: '',
                imagesCount: 1,
                displayImgSrc: imgSrc,
                fileMaxSize: 2,
            });
            InitValidators();
        });
        function InitValidators() {
            var hasSku = $("#ctl00_contentHolder_grdProducts tr:gt(0)").length > 0;
            if (hasSku) {
                $("#ctl00_contentHolder_grdProducts tr:gt(0)").find("[id$=txtActivitySalePrice]").each(function () {
                    var price = parseFloat($(this).parent().prev().text());
                    initValid(new InputValidator($(this).attr("id"), 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
                    appendValid(new MoneyRangeValidator($(this).attr("id"), 0.01, price, '输入的数值超出了系统表示范围'));
                });

                $("#ctl00_contentHolder_grdProducts tr:gt(0)").find("[id$=txtActivityStock]").each(function () {
                    var maxNumber = parseInt($(this).parent().prev().text());
                    initValid(new InputValidator($(this).attr("id"), 1, 10, false, '\\d*', '数据类型错误，只能输入实数型数值'));
                    appendValid(new MoneyRangeValidator($(this).attr("id"), 0, maxNumber, '输入的数值超出了系统表示范围'));
                });
            } else {
                var oldPrice = parseFloat($("#ctl00_contentHolder_lblPrice").text());
                initValid(new InputValidator('ctl00_contentHolder_txtPrice', 1, 10, false, '([0-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，火拼价格只能输入数值,且不能超过2位小数'))
                appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPrice', 0.01, oldPrice, '大于0，且不能超过商品正常售卖价'));
                var maxNumber = parseInt($("#ctl00_contentHolder_liDefaultStock").text().replace("商品当前库存：", ""));
                initValid(new InputValidator('ctl00_contentHolder_txtTotalCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，活动数量只能输入整数型数值'))
                appendValid(new NumberRangeValidator('ctl00_contentHolder_txtTotalCount', 1, maxNumber, '活动数量不能大于商品库存'));

            }
            initValid(new InputValidator('ctl00_contentHolder_txtMaxCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，每人限购数量只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxCount', 1, 9999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator("ctl00_contentHolder_CPStartTime", 1, 60, false, null, '数据类型错误，请选择开始时间'));
            initValid(new InputValidator("ctl00_contentHolder_CPEndDate", 1, 60, false, null, '数据类型错误，请选择结束时间'));

            initValid(new InputValidator('ctl00_contentHolder_txtJoinNumber', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，参团人数只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtJoinNumber', 2, 200, '参团人数取值范围：2-200'));

            initValid(new InputValidator('ctl00_contentHolder_txtLimitedHour', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，成团时限只能输入整数型数值'));
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a id="aReturn" href="FightGroupActivitiyList.aspx">管理</a></li>
                <li class="hover"><a>添加</a></li>
                <li><a href="FightGroupActivitiySeeting.aspx">拼团设置</a></li>
            </ul>
        </div>
        <div class="areacolumn clearfix">
            <div class="columnright">
                <div class="formitem">
                    <ul>
                        <li>
                            <div class="form-group" style="display: none">
                                <input type="hidden" id="dtp_input1" value="" />
                                <br />
                            </div>
                        </li>
                        <li><span class="formitemtitle "><em>*</em>活动商品：</span>
                            <span class="text-ellipsis mr10" style="max-width: 500px;">
                                <asp:Literal ID="ltProductName" runat="server"></asp:Literal>
                            </span>
                            <a href="javascript:;" onclick="selectProduct()" class="btn btn-default">选择商品</a>
                        </li>
                        <li id="liSkus" runat="server"><span class="formitemtitle "><em>*</em>商品规格：</span>
                            <div class="datalist clearfix " style="width: 700px; margin-left: 140px;">
                                <asp:Repeater ID="rptProductSkus" runat="server">
                                    <HeaderTemplate>
                                        <table cellspacing="0" border="0" id="ctl00_contentHolder_grdProducts" class="table table-striped">
                                            <tbody>
                                                <tr>
                                                    <th scope="col">规格</th>
                                                    <th>库存</th>
                                                    <th>活动数量</th>
                                                    <th>一口价</th>
                                                    <th>火拼价</th>
                                                </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("ValueStr")%>

                                                <asp:HiddenField ID="hfSkuId" Value='<%#Eval("SkuId")%>' runat="server" />
                                            </td>
                                            <td><%#Eval("Stock")%></td>
                                            <td>
                                                <asp:TextBox ID="txtActivityStock" runat="server" CssClass="forminput form-control" Width="100" onkeyup="value=value.replace(/[^\d]/g,'')"></asp:TextBox>
                                            </td>
                                            <td><%#Eval("SalePrice").ToDecimal().F2ToString("f2")%></td>
                                            <td>
                                                <asp:TextBox ID="txtActivitySalePrice" runat="server" CssClass="forminput form-control" Width="100" onkeyup="value=value.replace(/[^\d.]/g,'')"></asp:TextBox></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate></tbody></table></FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </li>
                        <li class="m_none"><span class="formitemtitle  m_none"><em>*</em>活动图片：</span>
                            <div id="logoContainer">
                                <span name="siteLogo" class="imgbox"></span>
                                <asp:HiddenField ID="hidUploadLogo" runat="server" />
                            </div>
                            <p>建议尺寸为640*400px</p>
                        </li>
                        <li><span class="formitemtitle"><em>*</em>分享标题：</span>
                            <asp:RadioButtonList ID="rbtlTitle" ClientIDMode="Static" runat="server">
                                <asp:ListItem Value="1" Selected="True">商品名称</asp:ListItem>
                                <asp:ListItem Value="2">自定义</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:TextBox ID="txtFightGroupShareTitle" ClientIDMode="Static" runat="server" CssClass="forminput form-control" placeholder="限60字" Style="display: none;" MaxLength="60"></asp:TextBox>
                        </li>
                        <li><span class="formitemtitle"><em>*</em>分享详情：</span>
                            <asp:TextBox ID="txtFightGroupShareDetails" ClientIDMode="Static" runat="server" CssClass="forminput form-control" placeholder="限60字" MaxLength="60"></asp:TextBox>
                        </li>
                        <li id="liSalePrice" runat="server" style="display: none;"><span class="formitemtitle ">一口价：</span>
                            <abbr class="formselect" style="color: red; font-family: Arial, Helvetica, sans-serif; font-size: 18px;">
                                <asp:Label ID="lblPrice" runat="server"></asp:Label>
                            </abbr>
                        </li>
                        <li id="liDefaultPrice" runat="server" class="mb_0"><span class="formitemtitle "><em>*</em>火拼价：</span>
                            <div class="input-group">
                                <span class="input-group-addon">￥</span>
                                <asp:TextBox ID="txtPrice" runat="server" CssClass="form_input_s form-control" onkeyup="value=value.replace(/[^\d.]/g,'')" placeholder=""></asp:TextBox>
                            </div>
                            <p id="ctl00_contentHolder_txtPriceTip"></p>
                        </li>

                        <li id="liDefaultStock" runat="server" style="display: none;"><span class="formitemtitle ">商品当前库存：</span>
                            &nbsp;&nbsp;
                            <asp:Literal ID="ltStock" runat="server"></asp:Literal>

                        </li>
                        <li id="liDefaultTotalCount" runat="server" class="mb_0"><span class="formitemtitle "><em>*</em>活动数量：</span>
                            <asp:TextBox ID="txtTotalCount" runat="server" MaxLength="10" CssClass="forminput form-control" onkeyup="value=value.replace(/[^\d]/g,'')" placeholder="必须小于商品当前库存数"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtTotalCountTip"></p>
                        </li>
                        <li><span class="formitemtitle "><em>*</em>开始时间：</span>
                            <Hi:CalendarPanel runat="server" ID="CPStartTime" Width="150"></Hi:CalendarPanel>
                        </li>
                        <li><span class="formitemtitle "><em>*</em>结束时间：</span>
                            <Hi:CalendarPanel runat="server" ID="CPEndDate" Width="150"></Hi:CalendarPanel>
                        </li>
                        <li><span class="formitemtitle "><em>*</em>参团人数：</span>
                            <asp:TextBox ID="txtJoinNumber" runat="server" CssClass="form_input_m form-control" onkeyup="value=value.replace(/[^\d]/g,'')" placeholder="参团人数" MaxLength="3"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtJoinNumberTip" class="colorD">
                                当未达到参团人数时，用户发起的此次拼团将失败，已支付的金额将自动退回给会员
                            </p>
                        </li>
                        <li><span class="formitemtitle "><em>*</em>成团时限：</span>
                            <div class="input-group">

                                <asp:TextBox ID="txtLimitedHour" runat="server" CssClass="form_input_m form-control" onkeyup="value=value.replace(/[^\d]/g,'')" placeholder="成团时限(小时)"></asp:TextBox>
                                <span class="input-group-addon">小时</span>
                            </div>

                            <p id="ctl00_contentHolder_txtLimitedHourTip" class="colorD">
                                开团成功后，团长发起的组团有效时间。
                            </p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle "><em>*</em>每人限购数量：</span>
                            <asp:TextBox ID="txtMaxCount" runat="server" CssClass="form_input_m form-control" onkeyup="value=value.replace(/[^\d]/g,'')" placeholder="每个人能购买的数量"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtMaxCountTip"></p>
                        </li>


                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnAddFightGroupActivitiy" runat="server" Text="添加" CssClass="btn btn-primary" OnClientClick="return getUploadImages();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfProductId" runat="server" />

</asp:Content>
