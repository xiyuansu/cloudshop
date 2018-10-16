<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddOrderPromotion.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddOrderPromotion" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<%@ Register TagPrefix="cc1" TagName="PromotionView" Src="~/Admin/promotion/Ascx/PromotionView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_promotionView_txtPromoteSalesName', 1, 60, false, null, '不超过60个字符'));
        }

        $(document).ready(function () {
            InitValidators();
            SelectPromoteType();
            ShowPromotion(false);
            $("input[type='radio'][name='radPromoteType']").bind("click", function () { ShowPromotion(true); });
        });

        function SelectPromoteType() {
            var promoteType = $("#ctl00_contentHolder_txtPromoteType").val();
            if (promoteType == 11)
                $("#radPromoteType_FullAmountDiscount").attr("checked", true);
            else if (promoteType == 12)
                $("#radPromoteType_FullAmountReduced").attr("checked", true);
            else if (promoteType == 13)
                $("#radPromoteType_FullQuantityDiscount").attr("checked", true);
            else if (promoteType == 14)
                $("#radPromoteType_FullQuantityReduced").attr("checked", true);
            else if (promoteType == 15)
                $("#radPromoteType_FullAmountSentGift").attr("checked", true);
            else if (promoteType == 16)
                $("#radPromoteType_FullAmountSentTimesPoint").attr("checked", true);
            else if (promoteType == 17)
                $("#radPromoteType_FullAmountSentFreight").attr("checked", true);
        }

        function ShowPromotion(isClick) {
            $("#liPromoteTypeDiscount").hide();
            $("#ctl00_contentHolder_txtCondition").addClass("discount_radius_left");
            if (isClick) {
                $("#ctl00_contentHolder_txtCondition").val("");
                $("#ctl00_contentHolder_txtDiscountValue").val("");
            }

            if ($("#radPromoteType_FullAmountDiscount").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#liPromoteTypeCondition").show();
                $("#spanDiscountValue").hide();
                $("#ctl00_contentHolder_txtDiscountValue").addClass("forminput").removeClass("form_input_s").addClass("discount_allradius").removeClass("discount_radius_left");
                $("#spCondition").show();
                $("#spDiscountValue").show();
                $("#lblConditionTip").html("满足金额：");
                $("#spanCondition").html("￥");
                $("#lblDiscountValueTip").html("折扣值(如果打9折，请输入0.9)：");
                hideGiftPanel();
            }
            else if ($("#radPromoteType_FullAmountReduced").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#liPromoteTypeCondition").show();
                $("#spCondition").show();
                $("#spDiscountValue").show();
                $("#lblConditionTip").html("满足金额：");
                $("#spanCondition").html("￥");
                $("#lblDiscountValueTip").html("立减金额：");
                $("#spanDiscountValue").html("￥");
                $("#spanDiscountValue").show();
                $("#ctl00_contentHolder_txtDiscountValue").removeClass("forminput").addClass("form_input_s").addClass("discount_radius_left").removeClass("discount_allradius");
                hideGiftPanel();
            }
            else if ($("#radPromoteType_FullQuantityDiscount").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#liPromoteTypeCondition").show();
                $("#spCondition").show();
                $("#spanDiscountValue").hide();
                $("#spDiscountValue").show();
                $("#lblConditionTip").html("满足数量：")
                $("#spanCondition").html("件");
                $("#lblDiscountValueTip").html("折扣值(如果打9折，请输入0.9)：");
                $("#ctl00_contentHolder_txtDiscountValue").addClass("forminput").removeClass("form_input_s").addClass("discount_allradius").removeClass("discount_radius_left");
                hideGiftPanel();
            }
            else if ($("#radPromoteType_FullQuantityReduced").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#liPromoteTypeCondition").show();
                $("#spCondition").show();
                $("#spanDiscountValue").show();
                $("#spDiscountValue").show();
                $("#lblConditionTip").html("满足数量：");
                $("#spanCondition").html("件");
                $("#lblDiscountValueTip").html("立减金额：");
                $("#spanDiscountValue").html("￥");
                $("#ctl00_contentHolder_txtDiscountValue").removeClass("forminput").addClass("form_input_s").addClass("discount_radius_left").removeClass("discount_allradius");
                hideGiftPanel();
            }
            else if ($("#radPromoteType_FullAmountSentGift").is(':checked')) {
                $("#liPromoteTypeDiscount").hide();
                $("#liPromoteTypeCondition").show();
                $("#spCondition").show();
                $("#spanDiscountValue").show();
                $("#spDiscountValue").hide();
                $("#lblConditionTip").html("满足金额：");
                $("#spanCondition").html("￥");
                $("#ctl00_contentHolder_txtDiscountValue").removeClass("forminput").addClass("form_input_s").addClass("discount_radius_left").removeClass("discount_allradius");
                $("#liSendGift").show();
            }
            else if ($("#radPromoteType_FullAmountSentTimesPoint").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#liPromoteTypeCondition").show();
                $("#spCondition").show();
                $("#spDiscountValue").show();
                $("#lblConditionTip").html("满足金额：");
                $("#spanCondition").html("￥");
                $("#lblDiscountValueTip").html("倍数：");
                $("#ctl00_contentHolder_txtDiscountValue").addClass("forminput").removeClass("form_input_s").addClass("discount_allradius").removeClass("discount_radius_left");
                $("#spanDiscountValue").hide();
                hideGiftPanel();
            }
            else if ($("#radPromoteType_FullAmountSentFreight").is(':checked')) {
                $("#liPromoteTypeDiscount").hide();
                $("#liPromoteTypeCondition").show();
                $("#spCondition").show();
                $("#spanDiscountValue").show();
                $("#spDiscountValue").hide();
                $("#lblConditionTip").html("满足金额：");
                $("#spanCondition").html("￥");
                $("#ctl00_contentHolder_txtDiscountValue").removeClass("forminput").addClass("form_input_s").addClass("discount_radius_left").removeClass("discount_allradius");
                hideGiftPanel();
            }
        }

        function selectGift() {
            $("#hidSelectGifts").val("");
            if ($("#addlist input[id^='hidGift_']").length > 4) {
                ShowMsg("一个活动最多只能添加5个礼品");
                return;
            }

            DialogFrameClose("/Admin/ChoicePage/CPGifts.aspx?giftIds=" + getSelectedGiftIds(), "选择礼品", 800, 600, function (e) { CloseFrameWindow(); });
        }

        function CloseFrameWindow() {
            var arr = $("#hidSelectGifts").val();

            if (arr == "") return;
            var allGifts = $("#hidAllSelectedGifts");
            if (allGifts.val() != "") {
                if ($("#addlist input[id^='hidGift_']").length + arr.split(",,,").length > 5) {
                    ShowMsg("一个活动最多只能添加5个礼品");
                    $("#hidSelectGifts").val("");
                    return;
                }

                allGifts.val(allGifts.val() + ",,," + arr);
            }
            else {
                var items = arr.split(",,,");
                if (items.length > 5) {
                    $("#hidSelectGifts").val("");
                    ShowMsg("一个活动最多只能添加5个礼品");
                    return;
                }
                allGifts.val(arr);
            }
            $("#divSomeProducts").show();
            BindGiftHtml(arr);
        }

        function BindGiftHtml(arr) {
            if (arr == "") return;
            var items = arr.split(",,,");
            var content = "";

            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                var record = item.split("|||");
                content += String.format("<tr name='appendlist'><td>{0}</td><td>{1}</td><td><input type='hidden' value='{2}' id='hidGift_{3}' /><span style='cursor:pointer;color:blue' class='colorBlue' onclick='onDelGift(this,\"{0}\");'>删除</span></td></tr>", record[1], record[2], record[0], record[0]);
            }
            $("#addlist tr:eq(0)").after(content);
        }

        function onDelGift(obj, name) {
            if (confirm("确定要删除礼品【" + name + "】吗？")) {
                $(obj).parent().parent().remove();
            }
        }
        function getSelectedGiftIds() {
            var giftIds = "";
            $("#addlist input[id^='hidGift_']").each(function (i, obj) {
                if (giftIds != "")
                    giftIds += "," + $(obj).val();
                else
                    giftIds += $(obj).val();
            });
            return giftIds;
        }
        function setChooseGifts() {
            $("#hidSelectGiftId").val(getSelectedGiftIds());
        }

        function Valid() {
            var promoteType = $("input[type='radio'][name='radPromoteType']:checked").val();
            var condition = $("#ctl00_contentHolder_txtCondition").val();
            var discountValue = $("#ctl00_contentHolder_txtDiscountValue").val();
            var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");
            var numexp = new RegExp("^[0-9]\\d*$", "i");

            if (promoteType == undefined) {
                ShowMsg("请选择促销活动类型！")
                return false;
            }

            $("#ctl00_contentHolder_txtPromoteType").val(promoteType);

            if (promoteType == 11) {
                if (condition.length == 0) {
                    ShowMsg("请输入满足金额！");
                    return false;
                }
                if (!exp.test(condition)) {
                    ShowMsg("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                    return false;
                }
                if (discountValue.length == 0) {
                    ShowMsg("请输入折扣值(一般在0.01-1之间)！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    ShowMsg("输入折扣值有误(必须是数值)，请重新输入正确的折扣值！");
                    return false;
                }
                var num = parseFloat(discountValue);
                if (num < 0.01 || num > 1) {
                    ShowMsg("折扣值要在0.01-1之间，请重新输入正确的折扣值！");
                    return false;
                }
            }
            else if (promoteType == 12) {
                if (condition.length == 0) {
                    ShowMsg("请输入满足金额！");
                    return false;
                }
                if (!exp.test(condition)) {
                    ShowMsg("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                    return false;
                }
                if (discountValue.length == 0) {
                    ShowMsg("请输入立减金额！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    ShowMsg("输入立减金额有误(必须是数值)，请重新输入正确的立减金额！");
                    return false;
                }
                if (parseFloat(discountValue)>=10000) {
                    ShowMsg("立减金额必须小于10000！");
                    return false;
                }
            }
            else if (promoteType == 13) {
                if (condition.length == 0) {
                    ShowMsg("请输入满足数量！");
                    return false;
                }
                if (!numexp.test(condition)) {
                    ShowMsg("输入满足数量有误(必须是正数)，请重新输入正确的满足数量！");
                    return false;
                }
                if (discountValue.length == 0) {
                    ShowMsg("请输入折扣值(一般在0.01-1之间)！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    ShowMsg("输入折扣值有误(必须是数值)，请重新输入正确的折扣值！");
                    return false;
                }
                var num = parseFloat(discountValue);
                if (num < 0.01 || num > 1) {
                    ShowMsg("折扣值要在0.01-1之间，请重新输入正确的折扣值！");
                    return false;
                }
            }
            else if (promoteType == 14) {
                if (condition.length == 0) {
                    ShowMsg("请输入满足数量！");
                    return false;
                }
                if (!numexp.test(condition)) {
                    ShowMsg("输入满足数量有误(必须是正数)，请重新输入正确的满足数量！");
                    return false;
                }

                if (discountValue.length == 0) {
                    ShowMsg("请输入立减金额！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    ShowMsg("立减金额有误(必须是数值)，请重新输入正确的立减金额！");
                    return false;
                }
            }
            else if (promoteType == 15) {
                if (condition.length == 0) {
                    ShowMsg("请输入满足金额！");
                    return false;
                }
                if ($("#hidSelectGiftId").val().length == 0) {
                    ShowMsg("请选择要赠送的礼品!");
                    return false;
                }
                if (!exp.test(condition)) {
                    ShowMsg("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                    return false;
                }
            }
            else if (promoteType == 16) {
                if (condition.length == 0) {
                    ShowMsg("请输入满足金额！");
                    return false;
                }
                if (!exp.test(condition)) {
                    ShowMsg("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                    return false;
                }
                if (discountValue.length == 0 || isNaN(parseFloat(discountValue)) || parseFloat(discountValue) <= 1) {
                    ShowMsg("请输入倍数,倍数必须为数值且大于1！");
                    return false;
                }

            }
            else if (promoteType == 17) {
                if (condition.length == 0) {
                    ShowMsg("请输入满足金额！");
                    return false;
                }
                if (!exp.test(condition)) {
                    ShowMsg("输入满足金额有误(必须是数值)，请重新输入正确的满足金额！");
                    return false;
                }
            }
            if ($("div.icheckbox_square-red.checked").length == 0) {
                ShowMsg("必须选择一个适合的客户", false);
                return false;
            }
            if (!PageIsValid())
                return false;
            if ($("#ctl00_contentHolder_promotionView_calendarStartDate").val() == "") {
                ShowMsg("请选择促销活动开始时间！")
                return false;
            }
            if ($("#ctl00_contentHolder_promotionView_calendarEndDate").val() == "") {
                ShowMsg("请选择促销活动结束时间！")
                return false;
            }

            if (!checkEndTime($("#ctl00_contentHolder_promotionView_calendarStartDate").val(), $("#ctl00_contentHolder_promotionView_calendarEndDate").val())) {
                ShowMsg("活动结束时间要大于活动开始时间！")
                return false;
            }
            if (isOpenStore && storeIdArry.length == 0) {
                ShowMsg("请选择门店范围！")
                return false;
            }
            return true;
        }

        function hideGiftPanel() {
            $("#liSendGift").hide();
            $("#hidSelectGiftId").val("");
            $("#hidSelectGifts").val("");
            $("#hidAllSelectedGifts").val("");
            $("#divSomeProducts").hide();
            var trs = $("#addlist").find("tr");
            for (var i = 1; i < trs.length; i++) {
                $(trs[i]).remove();
            }
        }

        function checkEndTime(startTime, endTime) {
            //var startTime = $("#startTime").val();
            var start = new Date(startTime.replace("-", "/").replace("-", "/"));
            //var endTime = $("#endTime").val();
            var end = new Date(endTime.replace("-", "/").replace("-", "/"));
            if (end < start) {
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    
    <asp:HiddenField ID="hidStoreIds" runat="server" />
    <script type="text/javascript">
        $(function () {
            var menu_left = window.parent.document.getElementById("menu_left");
            var aReturnTitle = $(".curent", menu_left);
            if (aReturnTitle) {
                var href = "/admin/" + $(aReturnTitle).attr("href");
                $("#aReturnTitle").attr("href", href);
            }
        })
    </script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a id="aReturnTitle">管理</a></li>
                <li class="hover"><a href="javascript:void">添加</a></li>
            </ul>
        </div>
    </div>
    <div class="areacolumn">
        <div class="columnright">

            <div class="datafrom">
                <div class="formitem">
                    <ul>
                        <li><span class="formitemtitle"><em>*</em>促销活动类型：</span>
                            <Hi:PromoteTypeRadioButtonList IsProductPromote="false" runat="server" ID="radPromoteType" />
                            <Hi:TrimTextBox runat="server" ID="txtPromoteType" Style="display: none;"></Hi:TrimTextBox>
                        </li>
                        <li id="liPromoteTypeCondition" style="display: none;">
                            <div id="spCondition" class="input-group">
                                <span id="lblConditionTip" class="formitemtitle"></span>
                                <asp:TextBox ID="txtCondition" runat="server" CssClass="form_input_s form-control"></asp:TextBox>
                                <span class="input-group-addon" id="spanCondition"></span>
                            </div>
                        </li>
                        <li id="liPromoteTypeDiscount" style="display: none;">
                            <div id="spDiscountValue" class="input-group">
                                <span id="lblDiscountValueTip" class="formitemtitle"></span>
                                <span class="input-group-addon" id="spanDiscountValue" style="padding: 8px 5px;"></span>
                                <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="form_input_s form-control"></asp:TextBox>

                            </div>
                        </li>
                        <li id="liSendGift" style="display: none;">
                            <div class="input-group">
                                <span class="formitemtitle"><em>*</em>礼品：</span>
                                <a href="javascript:;" onclick="selectGift()" class="btn btn-default">选择礼品</a>
                                最多可选择5个礼品
                            </div>
                            <div style="width: 50%; text-align: left; margin-left: 248px; display: none;" id="divSomeProducts">
                                <table id="addlist" class="table table-striped bundling-table table-fixed">
                                    <tr>
                                        <th width="70%">礼品名称</th>
                                        <th width="15%">成本价</th>
                                        <th width="15%">操作</th>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <%--<li id="liPromoteTypeDiscount" style="display: none;">
                            <table>
                                <tr>
                                    <td><span id="spCondition"><span id="lblConditionTip" class="formitemtitle " style="margin-bottom:5px;"></span>
                                        <asp:TextBox ID="txtCondition" runat="server" CssClass="forminput form-control"></asp:TextBox></span>　　
                                    <span id="spDiscountValue"><span id="lblDiscountValueTip" class="formitemtitle "></span>
                                        <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="forminput form-control"></asp:TextBox></span></td>
                                </tr>
                            </table>

                        </li>--%>
                        <cc1:PromotionView runat="server" ID="promotionView" />
                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnAdd" runat="server" Text="添加" OnClientClick="setChooseGifts();return Valid();" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidSelectGiftId" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidSelectGifts" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidAllSelectedGifts" ClientIDMode="Static" />
</asp:Content>
