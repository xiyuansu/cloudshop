<%@ Page Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="SendOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Depot.SendGoods" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Admin/Ascx/Order_ItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Admin/Ascx/Order_ShippingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title title_height m_none td_bottom">
            <em>
                <img src="../images/05.gif" width="32" height="32" /></em>
            <h1 class="title_line">订单发货</h1>
        </div>
        <div class="Purchase">
            <div class="State" style="width: auto; padding: 11px 12px 10px;">
                <h1>订单详情</h1>
                <table width="100%" border="0" cellspacing="0">
                    <tr style="background: #f0f0f0">
                        <td width="15%">订单编号：</td>
                        <td width="20%">
                            <asp:Label ID="lblOrderId" runat="server"></asp:Label></td>
                        <td width="15%">创建时间：</td>
                        <td width="28%">
                            <Hi:FormatedTimeLabel runat="server" ID="lblOrderTime"></Hi:FormatedTimeLabel></td>
                        <td width="10%">&nbsp;</td>
                        <td width="20%">&nbsp;</td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="list">
            <h1>发货</h1>
            <div class="Settlement">
                <style>
                    .databody .list .Settlement table {
                        border: 0px;
                    }
                </style>
                <table width="100%" border="0" cellspacing="0" class="br_none">
                    <tr>
                        <td style="width: 100px;">发货方式：</td>
                        <td>
                            <asp:HiddenField ID="txtSendGoodType" runat="server" ClientIDMode="Static" Value="1" />
                            <input type="radio" name="sendGoodType" id="radio_sendGoodType_1" value="1" checked="checked" /><label for="radio_sendGoodType_1" style="margin-right: 40px;">普通物流</label>
                            <label id="labSameCity" runat="server">
                                <input type="radio" name="sendGoodType" id="radio_sendGoodType_3" value="2" />
                                <label for="radio_sendGoodType_2" style="margin-right: 40px;">同城物流</label>
                            </label>
                            <input type="radio" name="sendGoodType" id="radio_sendGoodType_2" value="0" /><label for="radio_sendGoodType_2">无需物流</label>
                        </td>
                    </tr>
                    <tr id="tr_wuliu_1">
                        <td style="width: 100px;">物流公司：</td>
                        <td class="a_none">
                            <Hi:ExpressRadioButtonList runat="server" ClientIDMode="Static" RepeatColumns="6" RepeatDirection="Horizontal" ID="expressRadioButtonList" /></td>
                    </tr>
                    <tr id="tr_wuliu_2">
                        <td>运单号码：</td>
                        <td class="a_none">
                            <asp:TextBox runat="server" ClientIDMode="Static" ID="txtShipOrderNumber" class="forminput" />
                            <span id="txtShipOrderNumberTip" runat="server" style="line-height: 30px; color: red">&nbsp;运单号码不能为空，在1至20个字符之间</span></td>
                    </tr>
                    <tr id="tr_wuliu_3">
                        <td style="width: 100px;">预计费用：</td>
                        <td class="a_none">
                           <span id="txtFreight"></span>
                            <asp:HiddenField ID="txtDeliveryNo" ClientIDMode="Static" runat="server" Value="" />
                    </tr>
                </table>
            </div>
            <div class="bnt Pa_100 Pg_15 Pg_18" style="padding-left: 100px;">
                <asp:Button ID="btnSendGoods" runat="server" Text="确认发货" OnClientClick="return checkNeedWuliu();" class="btn btn-primary" />
            </div>
        </div>

        <div class="blank12 clearfix"></div>
        <div class="list">
            <cc1:Order_ItemsList runat="server" ID="itemsList" ShowAllItem="true" />
        </div>
        <div class="blank12 clearfix"></div>
        <div class="list">

            <h1>物流信息</h1>
            <div class="Settlement">
                <table width="200" border="0" cellspacing="0">
                    <tr>
                        <td width="15%" align="right">买家选择：</td>
                        <td colspan="2" class="a_none">
                            <asp:Literal runat="server" ID="litShippingModeName" /></td>
                    </tr>
                    <tr>
                        <td align="right">收货地址：</td>
                        <td width="65%" class="a_none">
                            <asp:Literal runat="server" ID="litReceivingInfo" /></td>
                        <td width="10%" class="a_none"><span class="Name"><a href="javascript:UpdateShippAddress('<%=Page.Request.QueryString["OrderId"] %>')">修改收货地址</a></span></td>
                    </tr>
                    <tr>
                        <td align="right" nowrap="nowrap">送货上门时间：</td>
                        <td colspan="2" class="a_none">
                            <asp:Label ID="litShipToDate" runat="server" Style="word-wrap: break-word; word-break: break-all;" /></td>
                    </tr>
                    <tr>
                        <td align="right" nowrap="nowrap">买家留言：</td>
                        <td colspan="2" class="a_none">&nbsp;
                            <asp:Label ID="litRemark" runat="server" Style="word-wrap: break-word; word-break: break-all;" /></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function checkNeedWuliu() {
            var need = $("#radio_sendGoodType_1").is(":checked");
            if (need) {
                var wuliuName = $("input[name='ctl00$contentHolder$expressRadioButtonList']:checked").val();
                var wuliuCode = $.trim($("#txtShipOrderNumber").val());
                if (wuliuName == undefined || wuliuName == "") {
                    alert("请选择物流公司");
                    return false;
                }
                if (wuliuCode == "") {
                    alert("运单号码不能为空，在1至20个字符之间");
                    return false;
                }
                return true;
            }
            return true;
        }

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtShipOrderNumber', 1, 20, false, null, '运单号码不能为空，在1至20个字符之间'));
        }
        $(document).ready(function () {
            InitValidators();
            $('input:radio[name="sendGoodType"]').change(function () {
                var need = $("input[name='sendGoodType']:checked").val();
                $("#txtSendGoodType").val(need);
                if (need == 1) {
                    $("#tr_wuliu_1").show();
                    $("#tr_wuliu_2").show();
                    $("#tr_wuliu_3").hide();
                }
                else {
                    if (need == 2) {
                        $("#tr_wuliu_3").show();
                        if ($("#txtFreight").html() == "") {
                            var orderId = $("#ctl00_contentHolder_lblOrderId").html();
                            var url = '/Depot/Sales/ashx/SendGoodOrders.ashx';
                            var expressData;
                            $.ajax({
                                type: "get",
                                url: url,
                                data: { action: 'QueryDeliverFee', orderId: orderId },
                                dataType: "json",
                                async: false,
                                success: function (data) {
                                    if (data.Result.Status == "SUCCESS") {
                                        $("#txtFreight").text(data.Result.fee.replace("预计运费：", ""));
                                        $("#txtDeliveryNo").val(data.Result.deliveryNo);
                                    }
                                    else {
                                        ShowMsg(data.Result.Message);
                                    }
                                }
                            });
                        }
                    }
                    else {
                        $("#tr_wuliu_3").hide();
                    }
                    $("#tr_wuliu_1").hide();
                    $("#tr_wuliu_2").hide();
                }
            });
        });


        function UpdateShippAddress(ordernumber) {
            var pathurl = "sales/ShippAddress.aspx?action=update&orderId=" + ordernumber;
            DialogFrame(pathurl, "修改收货地址", 640, 300);
        }
    </script>
</asp:Content>
