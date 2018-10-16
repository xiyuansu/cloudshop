<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddGroupBuy.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddGroupBuy" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="groupbuys.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">添加</a></li>

            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">


            <div class="formitem" style="padding-left: 15px;">
                <ul>

                    <li><span class="formitemtitle">团购商品：</span>
                        <span class="text-ellipsis mr10" style="max-width: 500px;">
                            <asp:Literal ID="ltProductName" runat="server"></asp:Literal>
                        </span>
                        <a href="javascript:;" class=" btn btn-default" onclick="selectProduct()">选择商品</a>
                    </li>
                    <li id="li_price" runat="server" style="display: none;"><span class="formitemtitle">一口价：</span>
                        <abbr class="formselect" style="color: red; font-family: Arial, Helvetica, sans-serif; font-size: 18px;">
                            <asp:Label ID="lblPrice" runat="server"></asp:Label>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>开始日期：</span>
                        <Hi:CalendarPanel runat="server" ID="calendarStartDate" CssClass="form_input_m form-control"></Hi:CalendarPanel>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>结束日期：</span>
                        <Hi:CalendarPanel runat="server" ID="calendarEndDate" CssClass="form_input_m form-control"></Hi:CalendarPanel>

                    </li>
                    <li>
                        <span class="formitemtitle">违约金：</span>
                        <div class="input-group">
                            <span class="input-group-addon">￥</span>
                            <asp:TextBox ID="txtNeedPrice" runat="server" CssClass="form_input_s form-control"></asp:TextBox>
                        </div>



                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>限购总数量：</span>
                        <asp:TextBox ID="txtMaxCount" runat="server" CssClass="form_input_m form-control" placeholder="此次活动可购买的商品总数量"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtMaxCountTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>团购满足数量：</span>
                        <asp:TextBox ID="txtCount" runat="server" CssClass="form_input_m form-control" placeholder="此次活动的最低商品订购数量"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtCountTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>团购价格：</span>
                        <div class="input-group">
                            <span class="input-group-addon">￥</span>
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form_input_s form-control" placeholder="满足数量后享受的团购价格,不能为空"></asp:TextBox>
                        </div>

                        <p id="ctl00_contentHolder_txtPriceTip"></p>
                    </li>
                    <li>
                        <span class="formitemtitle">活动说明：</span>
                        <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Columns="50" Rows="5" CssClass="form_input_l form-control"></asp:TextBox>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnAddGroupBuy" runat="server" Text="添加" OnClientClick="return PageIsValid();" CssClass=" btn btn-primary" />
                </div>
            </div>


        </div>
    </div>


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
            var url = "/Admin/ChoicePage/CPProducts.aspx?returnUrl=/Admin/promotion/AddGroupBuy.aspx&isFilterFightGroupProduct=true&isFilterCountDownProduct=true&IsFilterGroupBuyProduct=true&isFilterCombinationBuyProduct=true&isFilterPreSaleProduct=true&isFilterCombinationBuyOtherProduct=true";
            DialogFrame(url, "选择商品", 1200, 600);
        }
        function InitValidators() {
            initValid(new InputValidator("ctl00_contentHolder_calendarStartDate", 1, 60, false, null, '数据类型错误，请选择开始时间'));
            initValid(new InputValidator("ctl00_contentHolder_calendarEndDate", 1, 60, false, null, '数据类型错误，请选择结束时间'));

            initValid(new InputValidator('ctl00_contentHolder_txtNeedPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，违约金只能输入数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtNeedPrice', 1, 9999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtMaxCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，限购总数量只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxCount', 1, 9999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，团购满足数量只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCount', 1, 9999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator('ctl00_contentHolder_txtPrice', 1, 10, false, '([0-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，团购价格只能输入数值'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPrice', 0.01, 9999999, '输入的数值超出了系统表示范围'));


        }
        $(document).ready(function () {
            InitValidators();
        });
    </script>
</asp:Content>
