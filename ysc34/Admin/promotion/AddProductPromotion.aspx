<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddProductPromotion.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddProductPromotion" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<%@ Register TagPrefix="cc1" TagName="PromotionView" Src="~/Admin/promotion/Ascx/PromotionView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        var IsAddGift = false;
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
            if (promoteType == 1)
                $("#radPromoteType_Discount").attr("checked", true);
            else if (promoteType == 2)
                $("#radPromoteType_Amount").attr("checked", true);
            else if (promoteType == 3)
                $("#radPromoteType_Reduced").attr("checked", true);
            else if (promoteType == 4)
                $("#radPromoteType_QuantityDiscount").attr("checked", true);
            else if (promoteType == 5)
                $("#radPromoteType_SentGift").attr("checked", true);
            else if (promoteType == 6)
                $("#radPromoteType_SentProduct").attr("checked", true);
            else if (promoteType == 7) {
                $("#memberGrade").hide();
                $("#radPromoteType_MobileExclusive").attr("checked", true);
                $("#radPromoteType_MobileExclusive").iCheck("check");
            }
        }

        function ShowPromotion(isClick) {
            $("#liPromoteTypeDiscount").hide();
            $("#ctl00_contentHolder_txtCondition").addClass("discount_radius_left");
            if (isClick) {
                $("#ctl00_contentHolder_txtCondition").val("");
                $("#ctl00_contentHolder_txtDiscountValue").val("");
            }
            if ($("#radPromoteType_Discount").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#liPromoteTypeCondition").hide();
                $("#spCondition").hide();
                $("#spDiscountValue").show();
                $("#ctl00_contentHolder_txtDiscountValue").addClass("forminput").removeClass("form_input_s").addClass("discount_allradius").removeClass("discount_radius_left");
                $("#lblDiscountValueTip").html("折扣值(如果打9折，请输入0.9)：");
                $("#spanDiscountValue").hide();
            }
            else if ($("#radPromoteType_Amount").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#liPromoteTypeCondition").hide();
                $("#spCondition").hide();
                $("#spDiscountValue").show();
                $("#ctl00_contentHolder_txtDiscountValue").removeClass("forminput").addClass("form_input_s").addClass("discount_radius_left").removeClass("discount_allradius");
                $("#spanDiscountValue").show();
                $("#lblDiscountValueTip").html("出售金额：");
                $("#spanDiscountValue").html("元");
            }
            else if ($("#radPromoteType_Reduced").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#liPromoteTypeCondition").hide();
                $("#spCondition").hide();
                $("#spDiscountValue").show();
                $("#ctl00_contentHolder_txtDiscountValue").removeClass("forminput").addClass("form_input_s").addClass("discount_radius_left").removeClass("discount_allradius");
                $("#lblDiscountValueTip").html("立减金额：");
                $("#spanDiscountValue").html("元");
                $("#spanDiscountValue").show();
            }
            else if ($("#radPromoteType_QuantityDiscount").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#liPromoteTypeCondition").show();
                $("#spCondition").show();
                $("#spDiscountValue").show();
                $("#ctl00_contentHolder_txtDiscountValue").addClass("forminput").removeClass("form_input_s").addClass("discount_allradius").removeClass("discount_radius_left");

                $("#lblConditionTip").html("购买：");
                $("#spanCondition").html("件");
                $("#lblDiscountValueTip").html("折扣值(如果打9折，请输入0.9)：");
                $("#spanDiscountValue").hide();
            }
            else if ($("#radPromoteType_SentProduct").is(':checked')) {
                $("#liPromoteTypeDiscount").show();
                $("#liPromoteTypeCondition").show();
                $("#spCondition").show();
                $("#spDiscountValue").show();
                $("#ctl00_contentHolder_txtDiscountValue").removeClass("forminput").addClass("form_input_s").addClass("discount_radius_left").removeClass("discount_allradius");

                $("#lblConditionTip").html("购买：");
                $("#spanCondition").html("件");
                $("#lblDiscountValueTip").html("赠送：");
                $("#spanDiscountValue").html("件");
                $("#spanDiscountValue").show();
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
            else if ($("#radPromoteType_SentGift").is(':checked')) {
                $("#liSendGift").show();
                $("#liPromoteTypeDiscount").hide();
                $("#liPromoteTypeCondition").hide();
            }
            else if ($("#radPromoteType_MobileExclusive").is(':checked')) {
                $("#memberGrade").hide();
                $("#liMoblieTip").show();
                $("#liPromoteTypeDiscount").show();
                $("#liPromoteTypeCondition").hide();
                $("#spCondition").hide();
                $("#spDiscountValue").show();
                $("#ctl00_contentHolder_txtDiscountValue").removeClass("forminput").addClass("form_input_s").addClass("discount_radius_left").removeClass("discount_allradius");
                $("#lblDiscountValueTip").html("<em>*</em>立减金额：");
                $("#spanDiscountValue").html("元");
                $("#spanDiscountValue").show();

            }
        }

        function selectGift() {
            $("#hidSelectGifts").val("");
            if ($("#addlist input[id^='hidGift_']").length > 4) {
                alert("一个活动最多只能添加5个礼品");
                return;
            }
            IsAddGift = true;
            DialogFrameClose("/Admin/ChoicePage/CPGifts.aspx?giftIds=" + getSelectedGiftIds(), "选择礼品", 800, 600, function (e) { CloseFrameWindow(); });
        }

        function CloseFrameWindow() {
            var arr = $("#hidSelectGifts").val();
            if (arr == "") return;
            var allGifts = $("#hidAllSelectedGifts");
            if (allGifts.val() != "") {
                if ($("#addlist input[id^='hidGift_']").length + arr.split(",,,").length > 5) {
                    alert("一个活动最多只能添加5个礼品");
                    $("#hidSelectGifts").val("");
                    return;
                }

                allGifts.val(allGifts.val() + ",,," + arr);
            }
            else {
                var items = arr.split(",,,");
                if (items.length > 5) {
                    $("#hidSelectGifts").val("");
                    alert("一个活动最多只能添加5个礼品");
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
        var lastSubmitTime = new Date();;
        var submitTimes = 0;
        function Valid() {
            var promoteType = $("input[type='radio'][name='radPromoteType']:checked").val();
            var condition = $("#ctl00_contentHolder_txtCondition").val();
            var discountValue = $("#ctl00_contentHolder_txtDiscountValue").val();
            var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");
            var numexp = new RegExp("^[0-9]\\d*$", "i");

            if (promoteType == undefined) {
                alert("请选择促销活动类型！")
                return false;
            }

            $("#ctl00_contentHolder_txtPromoteType").val(promoteType);

            if (promoteType == 1) {
                if (discountValue.length == 0) {
                    alert("请输入折扣值(一般在0.01-1之间)！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    alert("输入折扣值有误(必须是数值)，请重新输入正确的折扣值！");
                    return false;
                }
                var num = parseFloat(discountValue);
                if (num < 0.01 || num > 1) {
                    alert("折扣值要在0.01-1之间，请重新输入正确的折扣值！");
                    return false;
                }
            }
            else if (promoteType == 2) {
                if (discountValue.length == 0) {
                    alert("请输入出售金额(元)！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    alert("输入出售金额(元)有误(必须是数值)，请重新输入正确的出售金额！");
                    return false;
                }
            }
            else if (promoteType == 3) {
                if (discountValue.length == 0) {
                    alert("请输入立减金额(元)！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    alert("输入立减金额(元)有误(必须是数值)，请重新输入正确的立减金额！");
                    return false;
                }
            }
            else if (promoteType == 5) {
                if ($("#hidSelectGiftId").val().length == 0) {
                    alert("请选择要赠送的礼品!");
                    return false;
                }
            }
            else if (promoteType == 4) {
                if (condition.length == 0) {
                    alert("请输入购买数量！");
                    return false;
                }

                if (!numexp.test(condition) || parseInt(condition) <= 1) {
                    alert("输入购买数量有误(必须是正数，并且大于1)，请重新输入正确的购买数量！");
                    return false;
                }

                if (discountValue.length == 0) {
                    alert("请输入折扣值(一般在0.01-1之间)！");
                    return false;
                }
                if (!exp.test(discountValue)) {
                    alert("折扣值有误(必须是数值)，请重新输入正确的折扣值！");
                    return false;
                }
                var num = parseFloat(discountValue);
                if (num < 0.01 || num > 1) {
                    alert("折扣值要在0.01-1之间，请重新输入正确的折扣值！");
                    return false;
                }
            }
            else if (promoteType == 6) {
                if (condition.length == 0) {
                    alert("请输入购买数量！");
                    return false;
                }

                if (!numexp.test(condition)) {
                    alert("输入购买数量有误(必须是正数)，请重新输入正确的购买数量！");
                    return false;
                }

                if (discountValue.length == 0) {
                    alert("请输入赠送数量(件)！");
                    return false;
                }
                if (!numexp.test(discountValue)) {
                    alert("输入赠送数量(件)有误(必须是正值)，请重新输入正确的赠送数量！");
                    return false;
                }
            }

            if (!PageIsValid())
                return false;
            if ($("#ctl00_contentHolder_promotionView_calendarStartDate").val() == "") {
                alert("请选择促销活动开始时间！")
                return false;
            }
            if ($("#ctl00_contentHolder_promotionView_calendarEndDate").val() == "") {
                alert("请选择促销活动结束时间！")
                return false;
            }

            //3秒内重复点击直接返回false
            var tempDate = new Date();
            if ((tempDate.getTime() - lastSubmitTime.getTime()) < 3000 && submitTimes > 0) {
                lastSubmitTime = new Date();
                submitTimes += 1;
                return false;
            }
            lastSubmitTime = new Date();
            submitTimes += 1;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
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
    <div class="areacolumn clearfix ">
        <div class="columnright">

            <div class="datafrom">
                <div class="formitem ">
                    <ul>
                        <li class="icheck-label-5-10" id="promoteli" runat="server"><span class="formitemtitle "><em>*</em>促销活动类型：</span>
                            <Hi:PromoteTypeRadioButtonList IsProductPromote="true" runat="server" ID="radPromoteType" CssClass="icheck" />
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
                                <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="form_input_s form-control"></asp:TextBox>
                                <span class="input-group-addon" id="spanDiscountValue"></span>
                            </div>
                        </li>
                        <li id="liMoblieTip" style="display: none;">
                            <div class="input-group">
                                <span class="formitemtitle"><em>*</em></span>
                                <span>优惠金额将应用到您在该活动中的所添加的所有商品中，请谨慎设置，以免部分商品优惠金额过大！</span>
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
                                    <td><div id="spCondition" class="input-group"><span id="lblConditionTip" class="formitemtitle " style="margin-bottom:5px;"></span>
                                        <asp:TextBox ID="txtCondition" runat="server" CssClass="form_input_s form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="spanCondition"></span>
                                        </div>　　
                                    <div id="spDiscountValue" class="input-group"><span id="lblDiscountValueTip" class="formitemtitle "></span>
                                        <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="form_input_s form-control"></asp:TextBox>
                                        <span class="input-group-addon"  id="spanDiscountValue"></span>
                                    </div></td>
                                </tr>
                            </table>

                        </li>--%>
                        <cc1:PromotionView runat="server" ID="promotionView" />
                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnNext" runat="server" Text="下一步,添加促销商品" OnClientClick="setChooseGifts();return Valid();" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidSelectGiftId" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidSelectGifts" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidAllSelectedGifts" ClientIDMode="Static" />
</asp:Content>
