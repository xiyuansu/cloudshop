<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="CancelSendOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.CancelSendOrderGoods" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Admin/Ascx/Order_ItemsList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">
        function checkform() {

        }

        $(document).ready(function (e) {
            var orderId = $("#ctl00_contentHolder_lblOrderId").html();
            var url = '/API/LogisticsHandler.ashx?action=orderCancelReasons';
            var expressData;
            $.ajax({
                type: "post",
                url: url,
                dataType: "json",
                async: false,
                success: function (data) {
                    var expArr = data;
                    for (var i = 0; i < expArr.result.length; i++) {
                        $("#dropReasonlist").append("<option value='" + expArr.result[i].id + "'>" + expArr.result[i].reason + "</option>");
                    }
                }
            });

            $("#cancelsendgoods").click(function (e) {

                var reason = $("#dropReasonlist").find("option:selected").text();
                var reasonId = $("#dropReasonlist").val();
                var dadaStatus = $("#txtDadaStatus").val();
                if (reason == "") {
                    ShowMsg("请选择取消原因");
                    return false;
                }
                var isCancel = true;
                if (dadaStatus == "2") {
                    isCancel = window.confirm("配送员已经接单，取消订单将会扣除一定的违约金，是否继续？");

                }
                if (isCancel) {
                    var orderId = $("#ctl00_contentHolder_lblOrderId").html();
                    var url = '/Admin/Sales/ashx/ManageOrder.ashx?action=CancelSendGoods';
                    var expressData;
                    $.ajax({
                        type: "post",
                        url: url,
                        data: { cancel_reason_id: reasonId, cancel_reason: reason, order_id: orderId },
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            ShowMsg(data.message, data.success);
                        }
                    });
                }
            })
        })
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title title_height m_none td_bottom">
            <em>
                <img src="../images/05.gif" width="32" height="32" /></em>
            <h1 class="title_line">取消订单发货</h1>
        </div>
        <div class="Purchase">
            <div class="State" style="width: auto; padding: 11px 12px 10px;">
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
                <div class="blank12 clearfix"></div>
                <div class="list">
                    <cc1:Order_ItemsList runat="server" ID="itemsList" ShowAllItem="true" />
                </div>
            </div>
        </div>

        <div class="list">
            <h1>取消发货</h1>
            <div class="Settlement">
                <style>
                    .databody .list .Settlement table {
                        border: 0px;
                    }
                </style>
                <table width="100%" border="0" cellspacing="0" class="br_none">
                    <tr>
                        <td style="width: 100px;">取消原因：</td>
                        <td>
                            <asp:HiddenField ID="txtDadaStatus" runat="server" ClientIDMode="Static" Value="" />
                            <asp:HiddenField ID="txtReasonId" runat="server" ClientIDMode="Static" Value="1" />
                            <asp:HiddenField ID="txtReason" runat="server" ClientIDMode="Static" Value="" />
                            <select id="dropReasonlist"></select>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="bnt Pa_100 Pg_15 Pg_18" style="padding-left: 100px;">
                <input type="button" class="btn btn-primary" value="取消发货" id="cancelsendgoods" />
            </div>
        </div>


        <div class="blank12 clearfix"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>
