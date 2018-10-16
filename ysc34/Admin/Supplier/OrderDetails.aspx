<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.OrderDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Admin/Ascx/Order_ItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ChargesList" Src="~/Admin/Ascx/Order_ChargesList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Admin/Ascx/Order_ShippingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="txtOrderId" runat="server" ClientIDMode="Static" />
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
    <div class="dataarea mainwidth databody">
        <div class="alert alert-order" role="alert">
            <h2 class="mb_10">
                <span class="text-right" style="display: inline-block; width: 115px;">当前订单状态：</span><Hi:OrderStatusLabel ID="lblOrderStatus" runat="server" />
            </h2>
            <h2>
                <span class="text-right" style="display: inline-block; width: 115px;">
                    <asp:Label runat="server" ID="lbCloseReason" Text="关闭原因："></asp:Label></span>
                <asp:Label runat="server" ID="lbReason"></asp:Label>
            </h2>
            <div class="btn-group-order">
                <%--<asp:HyperLink runat="server" ID="lkbtnSendGoods" CssClass="btn btn-default" Text="发  货" Visible="false" NavigateUrl="javascript:ShowSend()"></asp:HyperLink>--%>
                <a href="" class="btn btn-default" id="lkbtnEditPrice" visible="false" runat="server">修改价格</a>
                <a href="javascript:ShowEditAddress()" id="lkBtnEditShippingAddress" runat="server" class="btn btn-default" visible="false">修改收货地址</a>
               
                <a href="" class="btn btn-default" style="display: none;">确认付款</a>
                <asp:HyperLink runat="server" ID="hlkOrderGifts" Text="添加订单礼品" Visible="false" CssClass="btn btn-default" />
                <a id="lbtnClocsOrder" runat="server" href="javascript:ShowCloseOrder();" visible="false" class="btn btn-default">关闭订单</a>
                <a class="btn btn-default" href="javascript:ShowRemarkOrder();">备注</a>
            </div>
        </div>

        <ul class="nav nav-tabs" role="tablist">
        </ul>
        <!-- Tab panes -->
        <div class="tab-content  oder_tab">
            <div role="tabpanel" class="tab-pane active" id="oderinformation">
                <h3>买家信息</h3>
                <ul class="buyer_information">
                    <li>
                        <span>会员名：</span><asp:Literal runat="server" ID="litUserName" />

                    </li>
                    <li>
                        <span>联系电话：</span>
                        <asp:Literal runat="server" ID="litUserTel" />
                    </li>
                    <li>
                        <span>真实姓名：</span>
                        <asp:Literal runat="server" ID="litRealName" />
                    </li>
                    <li style="width:100%;"><span>收货信息：</span><asp:Literal ID="lblShipAddress" runat="server" /></li>
                    <li style="width:100%;"><span>送货时间：</span><asp:Literal ID="litShipToDate" runat="server" /></li>
                    <li style="width:100%;"> <span>买家留言：</span><asp:Label ID="litRemark"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></li>
                </ul>
                 <asp:Panel ID="InvoicePanel" runat="server">
                    <style type="text/css">
                        li.invoice{width:50%;}
                        li.invoice span{width:100px;}
                    </style>
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
                    <ul class="buyer_information">
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
                <cc1:Order_ItemsList runat="server" ID="itemsList" ShowAllItem="true" />
                <div class="order_footer">
                    <ul style="width: 50%; float: left;">
                        <li>订单编号：<asp:Literal runat="server" ID="litOrderId" />
                        </li>
                        <li>订单时间：<asp:Literal ID="litOrderTime" runat="server" />
                        </li>
                         <li>
                            <asp:Literal ID="LitDepositTime" runat="server" /></li>
                        <li>
                            <asp:Literal ID="litPayTime" runat="server" /></li>
                        <li>
                            <asp:Literal ID="litSendGoodTime" runat="server" /></li>
                        <li>
                            <asp:Literal ID="litFinishTime" runat="server" /></li>                        

                    </ul>
                    <cc1:Order_ChargesList ID="chargesList" runat="server" />
                </div>
            </div>

            <div>
                <cc1:Order_ShippingAddress runat="server" ID="shippingAddress" />
            </div>
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
            <span class="frame-span frame-input110">标志：</span><Hi:OrderRemarkImageRadioButtonList runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="10" CssClass="icheck" ID="orderRemarkImageForRemark" />

            <p><span class="frame-span frame-input110">备忘录：</span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="60" CssClass="forminput form-control" /></p>
        </div>
    </div>

    <!--关闭订单-->
    <div id="closeOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <em>关闭交易?请您确认已经通知买家,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</em>
            </p>
            <p>
                <span class="frame-span frame-input110"><em>*</em>关闭理由:</span>
                <Hi:CloseTranReasonDropDownList runat="server"
                    ID="ddlCloseReason" CssClass="forminput form-control" Width="200px" />
            </p>
        </div>
    </div>



    <!--修改支付方式-->
    <div id="setPaymentMode" style="display: none;">
        <div class="frame-content" style="width: 400px; height: 150px;">
            <span class="frame-span frame-input130"><em>*</em>选择支付方式：</span><Hi:PaymentDropDownList CssClass="form-control" Width="180" runat="server" ID="ddlpayment" />
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidOrderId" runat="server" />
    </div>
    <!--查看物流-->
    <div id="ViewLogistics" style="display: none">
        <div class="frame-content">
            <h1>快递单物流信息</h1>
            <div id="expressInfo">正在加载中....</div>
        </div>
    </div>
    <div style="display: none">
        <asp:Button runat="server" ID="btnRemark" Text="编辑备注" CssClass="btn btn-primary" />
        <asp:Button ID="btnCloseOrder" runat="server" CssClass="btn btn-primary" Text="关闭订单" />
        <asp:Button ID="btnMondifyShip" runat="server" CssClass="btn btn-primary" Text="修改配送方式" />
        <asp:Button ID="btnMondifyPay" runat="server" CssClass="btn btn-primary" Text="修改支付方式" />
    </div>
   
      <div style="display: none">
        <input type="hidden" id="hidExpressCompanyName" clientidmode="static" runat="server" />
        <input type="hidden" id="hidShipOrderNumber" clientidmode="Static" runat="server" />
        <asp:Button ID="Button1" runat="server" CssClass="btn btn-danger" Text="关闭订单" />
        <asp:Button runat="server" ID="Button2" Text="编辑备注信息" CssClass="btn btn-primary" />
        <asp:Button ID="btnOrderGoods" runat="server" CssClass="btn btn-primary" Text="订单配货表" />&nbsp;
        <asp:Button runat="server" ID="btnProductGoods" Text="商品配货表" CssClass="btn btn-primary" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="../sales/order.helper.js?v=3.4"></script>
    <script src="../../Utility/expressInfo.js"></script>
    <script type="text/javascript">
        var formtype = "";
        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
            if (reason == "请选择关闭的理由") {
                alert("请选择关闭的理由");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlCloseReason", reason);
            return true;
        }

        function ValidationPayment() {
            var payment = document.getElementById("ctl00_contentHolder_ddlpayment").value;
            if (payment == "") {
                alert("请选择支付方式");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlpayment", payment);
            return true;
        }

        function ValidationShippingMode() {
            var shipmode = document.getElementById("ctl00_contentHolder_ddlshippingMode").value;
            if (shipmode == "") {
                alert("请选择配送方式");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlshippingMode", shipmode);
            return true;
        }

        //备注弹出框
        function ShowRemarkOrder() {
            arrytext = null;
            formtype = "remark";
            DialogFrame('/Admin/sales/OrderRemark.aspx?OrderId=' + $("#txtOrderId").val(), '修改订单备注', 550, 350, function (e) {
                document.location.reload();
            });
            // DialogShow("订单备注", 'orderrmark', 'RemarkOrder', 'ctl00_contentHolder_btnRemark');
        }
        //关闭订单
        function ShowCloseOrder() {
            arrytext = null;
            formtype = "closeorder";
            DialogShow("关闭订单", 'closeorder', 'closeOrder', 'ctl00_contentHolder_btnCloseOrder');
        }

        //发货
        function ShowSend() {
            var orderId = '<%=Page.Request.QueryString["OrderId"] %>';
            DialogFrame("/Admin/sales/SendOrderGoods.aspx?OrderId=" + orderId, '发货', null, null, function (e) {
                document.location.reload();
            });
        }

        //修改收货地址
        function ShowEditAddress() {
            DialogFrame('/Admin/sales/ShippAddress.aspx?action=update&OrderId=' + $("#txtOrderId").val(), '修改收货地址', 600, 400, function (e) {
                document.location.reload();
            });
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
                case "closeorder":
                    return ValidationCloseReason();
                    break;
                case "paytype":
                    return ValidationPayment();
                    break;
                case "changeorder":
                    if ($("#txtpost").val().replace(/\s/g, "") == "") {
                        alert("发货单号不允许为空！");
                        return false;
                    }
                    setArryText("txtpost", $("#txtpost").val());
                    break;
            };
            return true;
        }
        function CloseFrameWindow() {
            var win = art.dialog.open.origin;
            win.location.reload();
        }
    </script>
    
</asp:Content>
