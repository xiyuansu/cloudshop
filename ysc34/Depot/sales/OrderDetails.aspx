<%@ Page Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="OrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Depot.OrderDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Admin/Ascx/Order_ItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ChargesList" Src="~/Admin/Ascx/Order_ChargesList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Admin/Ascx/Order_ShippingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script src="/Depot/Sales/order.helper.js?v=3.4" type="text/javascript"></script>
    <script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>
    <script type="text/javascript">
        function GetLogisticsInformation() {
            var orderId = $("#hidOrderId").val();
            DialogFrame('/AppDepot/OrderLogistics.aspx?OrderId=' + orderId, '查看物流', null, null, function () {
                //ReloadPageData();
            });
        }
    </script>
    <style type="text/css">
        .orderinfomation ul { margin: 0px; padding: 0px; }
            .orderinfomation ul li { width: 50%; float: left; display: inline-block; font-size: 14px; }
                .orderinfomation ul li span { width: 100px; font-size: 16px; }
        .orderinfomation h3 { font-size: 18px; color: #333; line-height: 50px; }
        li.invoice { width: 50%; float: left; display: block; font-size: 14px; }
            li.invoice span { width: 100px; font-size: 16px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:void(0);" onclick="ToList()">返回列表</a></li>
                <li><a href="javascript:void(0);" class="hover">订单详情</a></li>
            </ul>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        function ToList() {
            var returnUrl = getParam("returnurl");
            if (returnUrl != "") {
                location.href = decodeURIComponent(returnUrl);;
            } else {
                window.history.back();
            }
        }
    </script>
    <div style="display: none;">
        <input type="hidden" id="hidOrderId" clientidmode="Static" runat="server" />
        <input type="hidden" id="hidDadaStatus" runat="server" clientidmode="Static" />
    </div>
    <div class="dataarea mainwidth databody">
        <div class="Purchase">
            <div class="Settlement">
                <table>
                    <tr>
                        <td><strong class="fonts colorE">当前订单（<asp:Literal runat="server" ID="litOrderId" />）状态：<Hi:OrderStatusLabel ID="lblOrderStatus" runat="server" /></strong></td>
                        <td>
                            <asp:Label runat="server" ID="lbCloseReason" Text="关闭原因：">
                                <asp:Label runat="server" ID="lbReason"></asp:Label>
                            </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="orderinfomation">
                            <h3>买家信息</h3>
                            <ul>
                                <li><span>会员名：</span><asp:Literal runat="server" ID="litUserName" />
                                </li>
                                <li><span>真实姓名：</span><asp:Literal runat="server" ID="litRealName" />
                                </li>
                                <li><span>联系电话：</span><asp:Literal runat="server" ID="litUserTel" />
                                </li>
                                <li><span>电子邮件：</span><asp:Literal runat="server" ID="litUserEmail" />
                                </li>
                                <li>
                                    <asp:Literal ID="litPayTime" runat="server" />
                                    <asp:Literal ID="litSendGoodTime" runat="server" />
                                    <asp:Literal ID="litFinishTime" runat="server" />
                                </li>
                                <li style="width: 100%;"><span>收货信息：</span><asp:Literal ID="lblShipAddress" runat="server" /></li>
                                <li style="width: 100%;"><span>送货时间：</span><asp:Literal ID="litShipToDate" runat="server" /></li>
                                <li style="width: 100%;"><span>买家留言：</span><asp:Label ID="litRemark" runat="server" Style="word-wrap: break-word; word-break: break-all;" /></li>
                                <li>
                                    <a href="javascript:ShowRemarkOrder();" class="btn btn-primary">备  注</a>
                                    <a id="lbtnModifyShippingOrder" runat="server" href="javascript:ShowPurchaseOrder();" visible="false" class="btn btn-default">修改发货单号</a>
                                    <asp:HyperLink runat="server" ID="lkbtnSendGoods" CssClass="btn btn-primary" Text="发  货" NavigateUrl="javascript:ShowSend()"></asp:HyperLink>
                                    <asp:HyperLink runat="server" ID="lkbtnViewLogistics" CssClass="btn btn-warning" Text="物流跟踪" NavigateUrl="javascript:GetLogisticsInformation()"></asp:HyperLink>
                                </li>
                            </ul>
                            <div role="tabpanel" class="tab-pane active" id="oderinformation">
                                <asp:Panel ID="InvoicePanel" runat="server">

                                    <script type="text/javascript" language="javascript">
                                        $(document).ready(function (e) {
                                            $("li.invoice font").each(function () {
                                                if ($(this).html() == "") {
                                                    $(this).parent().hide();
                                                }
                                            });
                                        })
                                    </script>
                                    <h3>发票信息</h3>
                                    <ul>
                                        <li class="invoice">
                                            <span>发票类型：</span><font><asp:Literal runat="server" ID="litInvoiceType" /></font>
                                        </li>
                                        <li class="invoice">
                                            <span>发票抬头：</span><font><asp:Literal runat="server" ID="litInvoiceTitle" /></font>
                                        </li>
                                        <li class="invoice">
                                            <span>纳税人识别号：</span><font><asp:Literal runat="server" ID="litInvoiceTaxpayerNumber" /></font>
                                        </li>
                                        <li class="invoice">
                                            <span>注册地址：</span><font><asp:Literal runat="server" ID="litRegisterAddress" /></font>
                                        </li>
                                        <li class="invoice">
                                            <span>注册电话：</span><font><asp:Literal runat="server" ID="litRegisterTel" /></font>
                                        </li>
                                        <li class="invoice">
                                            <span>开户银行：</span><font><asp:Literal runat="server" ID="litOpenBank" /></font>
                                        </li>
                                        <li class="invoice">
                                            <span>银行帐户：</span><font><asp:Literal runat="server" ID="litBankName" /></font>
                                        </li>
                                        <li class="invoice">
                                            <span>收票人姓名：</span><font><asp:Literal runat="server" ID="litReceiveName" /></font>
                                        </li>
                                        <li class="invoice">
                                            <span>收票人手机：</span><font><asp:Literal runat="server" ID="litReceiveMobbile" /></font>
                                        </li>
                                        <li class="invoice">
                                            <span>收票人邮箱：</span><font><asp:Literal runat="server" ID="litReceiveEmail" /></font>
                                        </li>
                                        <li class="invoice">
                                            <span>收票人地区：</span><font><asp:Literal runat="server" ID="litReceiveRegionName" /></font>
                                        </li>
                                        <li class="invoice">
                                            <span>收票人地址：</span><font><asp:Literal runat="server" ID="litReceiveAddress" /></font>
                                        </li>
                                    </ul>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="blank12 clearfix"></div>
        <div class="list">
            <a name="list" id="list"></a>
            <cc1:Order_ItemsList runat="server" ID="itemsList" />
            <cc1:Order_ChargesList ID="chargesList" runat="server" IsStoreAdminView="true" />
            <div class="clear"></div>
            <cc1:Order_ShippingAddress runat="server" ID="shippingAddress" IsStoreAdminView="true" />
        </div>
    </div>

    <!--编辑备注信息-->
    <div id="RemarkOrder" style="display: none;">
        <div class="frame-content">
            <p><span class="frame-span frame-input110">订单编号：</span><asp:Literal ID="spanOrderId" runat="server" /></p>

            <p><span class="frame-span frame-input110">成交时间：</span><Hi:FormatedTimeLabel runat="server" ID="lblorderDateForRemark" /></p>
            <p>
                <span class="frame-span frame-input110">订单实收款(元)：</span><em><Hi:FormatedMoneyLabel
                    ID="lblorderTotalForRemark" runat="server" /></em>
            </p>
            <span class="frame-span frame-input110">标志：</span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="10" />

            <p><span class="frame-span frame-input110">备忘录：</span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" /></p>
        </div>
    </div>

    <div style="display: none">
        <asp:Button runat="server" ID="btnRemark" Text="编辑备注" CssClass="submit_DAqueding" />
    </div>

    <!--查看物流-->
    <div id="ViewLogistics" style="display: none">
        <div class="frame-content" style="margin-top: -20px;">
            <h1>快递单物流信息</h1>

            <div id="expressInfo">正在加载中....</div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        var formtype = "";
        //备注弹出框
        function ShowRemarkOrder() {
            arrytext = null;
            formtype = "remark";

            DialogShow("订单备注", 'orderrmark', 'RemarkOrder', 'ctl00_contentHolder_btnRemark');
        }

        //发货
        function ShowSend() {
            var orderId = '<%=Page.Request.QueryString["OrderId"] %>';
            DialogFrame("sales/SendOrderGoods.aspx?OrderId=" + orderId, '发货', null, null, function (e) { location.reload(); })
        }

        function validatorForm() {
            switch (formtype) {
                case "remark":
                    $radioId = $("input[type='radio'][name='ctl00$contentHolder$orderRemarkImageForRemark']:checked")[0];
                    if ($radioId == null || $radioId == "undefined") {
                        alert('请先标记备注');
                        return false;
                    }
                    setArryText($radioId.id, "true");
                    setArryText("ctl00_contentHolder_txtRemark", $("#ctl00_contentHolder_txtRemark").val());
                    break;
                case "shipptype":
                    return ValidationShippingMode();
                    break;
                case "paytype":
                    return ValidationPayment();
                    break;
                case "changeorder":
                    var postCompanyName = $("#expressRadioButtonList").val();
                    $("#txt_expressCompanyName").val(postCompanyName);
                    if (postCompanyName == "") {
                        alert("请选择物流公司！");
                        $("#expressRadioButtonList").focus();
                        return false;
                    }
                    if ($("#txtpost").val().replace(/\s/g, "") == "") {
                        alert("发货单号不允许为空！");
                        return false;
                    }
                    setArryText("txt_expressCompanyName", postCompanyName);
                    setArryText("txtpost", $("#txtpost").val());
                    break;
            }
            return true;
        }
        function CloseFrameWindow() {
            var win = art.dialog.open.origin;
            win.location.reload();
        }
    </script>
</asp:Content>
