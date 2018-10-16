<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditGroupBuy.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditGroupBuy" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="groupbuys.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">编辑</a></li>

            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">

            <div class="formitem">
                <ul>
                    <li></li>
                    <li><span class="formitemtitle ">团购商品：</span>
                        <asp:Literal ID="ltProductName" runat="server"></asp:Literal>
                        <a onclick="selectProduct()" id="selectProductA" runat="server" class="btn btn-default">选择商品</a>
                    </li>
                    <li id="li_price"><span class="formitemtitle ">一口价：</span>
                        <abbr class="formselect" style="color: red; font-family: Arial, Helvetica, sans-serif; font-size: 18px;">
                            <asp:Label ID="lblPrice" runat="server"></asp:Label>
                        </abbr>
                        <span id="P4"></span>
                    </li>
                    <li><span class="formitemtitle "><em>*</em>开始日期：</span>
                        <Hi:CalendarPanel runat="server" ID="calendarStartDate" Style="margin-right: 5px;"></Hi:CalendarPanel>

                    </li>
                    <li><span class="formitemtitle "><em>*</em>结束日期：</span>
                        <Hi:CalendarPanel runat="server" ID="calendarEndDate" Style="margin-right: 5px;"></Hi:CalendarPanel>

                    </li>
                    <li>
                        <span class="formitemtitle ">违约金：</span>
                        <div class="input-group">
                            <span class="input-group-addon">￥</span>
                            <asp:TextBox ID="txtNeedPrice" runat="server" CssClass="form_input_s form-control"></asp:TextBox>
                        </div>

                    </li>
                    <li class="mb_0"><span class="formitemtitle "><em>*</em>限购总数量：</span>
                        <asp:TextBox ID="txtMaxCount" runat="server" CssClass="form_input_m form-control" placeholder="商品总数量,不能为空"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtMaxCountTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle "><em>*</em>团购满足数量：</span>
                        <asp:TextBox ID="txtCount" runat="server" CssClass="form_input_m form-control" placeholder="活动的最低商品订购数量,不能为空"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtCountTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle "><em>*</em>团购价格：</span>
                        <div class="input-group">
                            <span class="input-group-addon">￥</span>
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form_input_s form-control" placeholder="团购价格,不能为空"></asp:TextBox>
                        </div>

                        <p id="ctl00_contentHolder_txtPriceTip"></p>
                    </li>
                    <li>
                        <span class="formitemtitle ">活动说明：</span>
                        <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Columns="50" Rows="5" CssClass="form_input_l form-control"></asp:TextBox>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnUpdateGroupBuy" runat="server" Text="保存" OnClientClick="return PageIsValid();" CssClass="btn btn-primary float" />
                    <asp:Button ID="btnFinish" runat="server" Text="结束活动" Visible="false" CssClass="btn btn-primary float" />
                    <asp:Button ID="btnSuccess" runat="server" Text="活动成功" Visible="false" CssClass="btn btn-primary float" />
                    <asp:Button ID="btnFail" runat="server" Text="活动失败" Visible="false" CssClass="btn btn-danger float" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfGroupId" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="groupbuy.helper.js"></script>
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
            var url = "/Admin/ChoicePage/CPProducts.aspx?returnUrl=/Admin/promotion/EditGroupBuy.aspx&groupBuyId=" + $("#<%=this.hfGroupId.ClientID%>").val() + "&isFilterFightGroupProduct=true&isFilterCountDownProduct=true&IsFilterGroupBuyProduct=true&isFilterCombinationBuyProduct=true&isFilterPreSaleProduct=true&isFilterCombinationBuyOtherProduct=true";
            DialogFrame(url, "选择商品", 1200, 600);
        }
        function InitValidators() {

            initValid(new InputValidator("ctl00_contentHolder_calendarStartDate", 1, 60, false, null, '数据类型错误，请选择开始时间'));
            initValid(new InputValidator("ctl00_contentHolder_calendarEndDate", 1, 60, false, null, '数据类型错误，请选择结束时间'));

            initValid(new InputValidator('ctl00_contentHolder_txtNeedPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，违约金只能输入数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtNeedPrice', 1, 9999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtMaxCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，限购数量只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxCount', 1, 9999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，限购数量只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCount', 1, 9999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtPrice', 1, 10, false, '([0-9]\\d*(\\.\\d{1,2})?)', '团购价格只能输入数值,且不能超过2位小数'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPrice', 0.01, 9999999, '输入的数值超出了系统表示范围'));


        }
        $(document).ready(function () {
            InitValidators();
            $.ajax({
                url: "EditGroupBuy.aspx",
                data:
                        {
                            isCallback: "true",
                            productId: $("#ctl00_contentHolder_dropGroupBuyProduct").val()
                        },
                type: 'GET', dataType: 'json', timeout: 10000,
                async: false,
                success: function (resultData) {
                    if (resultData.Status == "OK") {
                        var price = resultData.Price;
                        $("#ctl00_contentHolder_lblPrice").html(price);
                        $("#li_price").show();
                    }
                }
            });
            $("#ctl00_contentHolder_dropGroupBuyProduct").change(function () {
                var pId = $(this).val();
                if (pId == "") {
                    $("#li_price").hide();
                }
                else {
                    $.ajax({
                        url: "EditGroupBuy.aspx",
                        data:
                        {
                            isCallback: "true",
                            productId: pId
                        },
                        type: 'GET', dataType: 'json', timeout: 10000,
                        async: false,
                        success: function (resultData) {
                            if (resultData.Status == "OK") {
                                var price = resultData.Price;
                                $("#ctl00_contentHolder_lblPrice").html(price);
                                $("#li_price").show();
                            }
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>

