<%@ Page Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="OrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.sales.OrderDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Supplier/Ascx/Order_ItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ChargesList" Src="~/Supplier/Ascx/Order_ChargesList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Supplier/Ascx/Order_ShippingAddress.ascx" %>
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
                <a id="lbtnModifyShippingOrder" runat="server" href="javascript:ShowPurchaseOrder();" visible="false" class="btn btn-default">修改发货单号</a>
                <asp:HyperLink runat="server" ID="lkbtnSendGoods" CssClass="btn btn-default" Text="发  货" Visible="false" NavigateUrl="javascript:ShowSend()"></asp:HyperLink>
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
                    <li style="width: 100%;"><span>收货信息：</span><asp:Literal ID="lblShipAddress" runat="server" /></li>
                    <li style="width: 100%;"><span>送货时间：</span><asp:Literal ID="litShipToDate" runat="server" /></li>
                    <li style="width: 100%;"><span>买家留言：</span><asp:Label ID="litRemark" runat="server" Style="word-wrap: break-word; word-break: break-all;" /></li>
                </ul>

                <asp:Panel ID="InvoicePanel" runat="server">
                    <style type="text/css">
                        li.invoice {
                            width: 50%;
                        }

                            li.invoice span {
                                width: 100px;
                            }
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
                            <asp:Literal ID="LitDepositTime" runat="server" />
                        </li>
                        <li>
                            <asp:Literal ID="litPayTime" runat="server" />
                        </li>
                        <li>
                            <asp:Literal ID="litSendGoodTime" runat="server" />
                        </li>
                        <li>
                            <asp:Literal ID="litFinishTime" runat="server" />
                        </li>
                    </ul>
                    <cc1:Order_ChargesList ID="chargesList" runat="server" />
                </div>
            </div>
            <div class="clear"></div>
            <div>
                <cc1:Order_ShippingAddress runat="server" ID="shippingAddress" />
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

        <script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="order.helper.js?v=3.4" type="text/javascript"></script>
    <script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>
    <script type="text/javascript">
        //发货
        function ShowSend() {
            var orderId = '<%=Page.Request.QueryString["OrderId"] %>';
            DialogFrame("sales/SendOrderGoods.aspx?OrderId=" + orderId, '发货', null, null, function (e) {
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
            };
            return true;
        }

        function CloseFrameWindow() {
            var win = art.dialog.open.origin;
            win.location.reload();
        }
    </script>

</asp:Content>
