<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddProductPreSale.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.AddProductPreSale" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function ShowProductDiv() {
            DialogFrameClose("product/SearchPreSaleProduct.aspx", "选择预售商品", 1200, 600, function (e) { CloseFrameWindow(); });
        }
        function CloseFrameWindow() {
            var arr = $("#ctl00_contentHolder_hidSelectProducts").val();
            if (arr == "") return;
            var record = arr.split("|||");
            $("#spanProductPrice").html(record[2]);
            $("#spanProductPricetip").css("display","block");
            $("#spanProductName").html(record[1]);
            $("#ctl00_contentHolder_hidSalePrice").val(record[2]);
            $("#ctl00_contentHolder_hidProductId").val(record[0]);
        }
        function onPreSaleSave() {
            var productId = $("#ctl00_contentHolder_hidProductId").val();
            if (productId == "" || productId == null || productId == undefined) {
                alert("请选择预售商品！");
                return false;
            }
            if ($("#ctl00_contentHolder_radDepositPercent").is(':checked')) {
                var percent = $("#ctl00_contentHolder_txtDepositPercent").val();
                var regr = /^[1-9]\d*$/;
                if (!regr.test(percent) || percent > 100 || percent <= 0) {
                    alert("百分比只能输入整数，限制在1-100之间");
                    return false;
                }
            }
            else {
                var deposit = $("#ctl00_contentHolder_txtDeposit").val();
                var regr = /^\d+(\.\d{1,2})?$/;
                if (!regr.test(deposit)) {
                    alert("请输入正确的固定金额, 不超过两位小数");
                    return false;
                }
                var salePrice = $("#ctl00_contentHolder_hidSalePrice").val();
                if (parseFloat(deposit) > parseFloat(salePrice)) {
                    alert("定金不能大于商品售价");
                    return false;
                }
                if (parseFloat(deposit) > 100000000) {
                    alert("您输入的固定金额过大");
                    return false;
                }
            }
            if (!PageIsValid()) return false;
            var paymentStartDate = $("#ctl00_contentHolder_PaymentStartDate").val();
            var paymentEndDate = $("#ctl00_contentHolder_PaymentEndDate").val();
            var preSaleEndDate = $("#ctl00_contentHolder_PreSaleEndDate").val();
            var currentDate = getDateFormatter(new Date());
            if (getNowFormatDate() > preSaleEndDate) {
                alert("预售结束时间不能早于当前时间！");
                return false;
            }
            if (currentDate > paymentStartDate) {
                alert("尾款支付开始时间不能早于当前时间！");
                return false;
            }
            if (paymentStartDate > paymentEndDate) {
                alert("尾款支付结束时间不能早于尾款支付开始时间！");
                return false;
            }
            if (paymentEndDate < preSaleEndDate) {
                alert("尾款支付结束时间不能早于预售结束时间！");
                return false;
            }
            if ($("#ctl00_contentHolder_radDeliveryDays").is(':checked')) {
                var days = $("#ctl00_contentHolder_txtDeliveryDays").val();
                var regr = /^[1-9]\d*$/;
                if (!regr.test(days) || days <= 0 || days > 1000) {
                    alert("天数只能输入整数，限制在1-1000之间");
                    return false;
                }
            }
            else {
                var deliveryDate = $("#ctl00_contentHolder_DeliveryDate").val();
                var currentDate = getDateFormatter(new Date());
                if (deliveryDate == "" || deliveryDate == null || deliveryDate == undefined) {
                    alert("请选择发货日期！");
                    return false;
                }
                if (currentDate > deliveryDate) {
                    alert("发货时间不能早于当前时间！");
                    return false;
                }
            }
            return true;
        }
        $(document).ready(function () {
            InitValidators();
        });
        function InitValidators() {
            initValid(new InputValidator("ctl00_contentHolder_PreSaleEndDate", 1, 60, false, null, '请选择预售结束时间'));
            initValid(new InputValidator("ctl00_contentHolder_PaymentStartDate", 1, 60, false, null, '请选择尾款支付开始时间'));
            initValid(new InputValidator("ctl00_contentHolder_PaymentEndDate", 1, 60, false, null, '请选择尾款支付结束时间'));
        }
        //格式化日期
        function getDateFormatter(value) {
            if (value == undefined || value == "" || value == null) {
                return "";
            }
            var d = new Date(value);
            var Y = d.getFullYear();
            var mo = d.getMonth() + 1;
            M = mo < 10 ? "0" + mo : mo;
            D = d.getDate() < 10 ? "0" + d.getDate() : d.getDate();
            return Y + "-" + M + "-" + D;
        }
        function getNowFormatDate() {
            var date = new Date();
            var seperator1 = "-";
            var seperator2 = ":";
            var month = date.getMonth() + 1;
            var strDate = date.getDate();
            if (month >= 1 && month <= 9) {
                month = "0" + month;
            }
            if (strDate >= 0 && strDate <= 9) {
                strDate = "0" + strDate;
            }
            var hour = date.getHours();
            var minutes = date.getMinutes();
            if (hour < 10) {
                hour = "0" + hour;

            }
            if (minutes < 10) {
                minutes = "0" + minutes;
            }
            var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
                    + " " + hour + seperator2 + minutes;
            return currentdate;
        }
        function calcuDeposit() {
            if ($("#ctl00_contentHolder_radDepositPercent").is(':checked')) {
                var percent = $("#ctl00_contentHolder_txtDepositPercent").val();
                var regr = /^[1-9]\d*$/;
                if (!regr.test(percent) || percent > 100 || percent <= 0) {
                    alert("百分比只能输入整数，限制在1-100之间");
                    $("#lblDepositMoney").text(0);
                    return;
                }
                var price = $("#spanProductPrice").html();
                var depositMoney = price * percent / 100;
                $("#lblDepositMoney").text(depositMoney.toFixed(2));
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField runat="server" ID="hidProductId" />
    <asp:HiddenField runat="server" ID="hidSelectProducts" />
    <asp:HiddenField runat="server" ID="hidSalePrice" />
    <div class="areacolumn clearfix">
        <div class="dataarea mainwidth databody">
            <div class="title">
                <ul class="title-nav">
                    <li><a href="ProductPreSale.aspx">管理</a></li>
                    <li class="hover"><a href="javascript:void">添加</a></li>

                </ul>
            </div>
        </div>
        <div class="columnright">
            <div class="formitem validator2">
                <ul>
                    <li>
                        <span class="formitemtitle"><em>*</em>选择预售商品：</span>
                        <span id="spanProductName"></span>
                        <input type="button" value="选择" class="btn btn-default float" onclick="ShowProductDiv()" />
                    </li>
                    <li>
                        <span class="formitemtitle">商品价格：</span>
                        <span id="spanProductPrice"></span>
                        <span style="color: #999999; margin-left: 20px; display: none;" id="spanProductPricetip">商品有多个规格时显示最低价格</span>
                        <%--<asp:TextBox runat="server" ID="txtCombinationPrice" onblur='checkIsMoney(this)' CssClass="forminput form-control" Width="100px"></asp:TextBox>--%>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle"><em>*</em>定金：</span>
                        <asp:RadioButton runat="server" GroupName="Deposit" ID="radDepositPercent" CssClass="icheck mt_10" Checked="true" /><span>百分比&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        <asp:TextBox runat="server" ID="txtDepositPercent" CssClass="forminput form-control fl" onkeyup="value=value.replace(/[^\d.]/g,'')" onblur="calcuDeposit()" Width="50"></asp:TextBox>&nbsp;% &nbsp;&nbsp;定金金额<label id="lblDepositMoney">0</label>&nbsp;元                            
                    </li>
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:RadioButton runat="server" GroupName="Deposit" ID="radDeposit" CssClass="icheck mt_10" /><span>固定金额&nbsp;</span><asp:TextBox runat="server" ID="txtDeposit" CssClass="forminput form-control fl" Width="50"></asp:TextBox>&nbsp;元 </
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle"><em>*</em>预售结束时间：</span>
                        <Hi:CalendarPanel runat="server" ID="PreSaleEndDate" Width="160"></Hi:CalendarPanel>
                        <p id="ctl00_contentHolder_PreSaleEndDateTip"></p>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle"><em>*</em>尾款支付时间：</span>
                        <Hi:CalendarPanel runat="server" ID="PaymentStartDate" Width="160"></Hi:CalendarPanel><span>&nbsp;至&nbsp;</span><Hi:CalendarPanel runat="server" ID="PaymentEndDate" Width="160"></Hi:CalendarPanel>
                        <p id="ctl00_contentHolder_PaymentStartDateTip">
                    </li>
                    <li class="mb_0" style="margin-bottom: 5px !important;">
                        <span class="formitemtitle"><em>*</em>发货时间：</span>
                        <asp:RadioButton runat="server" GroupName="Delivery" ID="radDeliveryDays" CssClass="icheck mt_10" Checked="true" /><span>尾款支付后&nbsp;</span><asp:TextBox runat="server" ID="txtDeliveryDays" CssClass=" form-control fl" Width="50"></asp:TextBox><span>&nbsp;天发货</span>
                    </li>
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:RadioButton runat="server" GroupName="Delivery" ID="radDeliveryDate" CssClass="icheck mt_10" /><span>发货日期&nbsp;</span><Hi:CalendarPanel runat="server" ID="DeliveryDate" Width="160"></Hi:CalendarPanel>
                    </li>
                    <asp:Button ID="btnAddPreSale" runat="server" Text="添加" OnClientClick="return onPreSaleSave();" CssClass="btn btn-primary ml_198" />
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
