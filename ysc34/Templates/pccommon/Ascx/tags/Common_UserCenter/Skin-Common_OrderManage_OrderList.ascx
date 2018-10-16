<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Context" %>
<script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>
<table id="spqingdan_title">
    <tr>
        <td align="center" width="32%" class="img">订单信息</td>
        <td align="center" width="12%">单价</td>
        <td align="center" width="10%">数量</td>
        <td align="center" width="10%">操作商品</td>
        <td align="center" width="12%">订单金额</td>
        <td align="center" width="12%">
            <div class="txt1" id="StatusList">
                <em>订单状态 </em>
                <div class="txt2">
                    <span status="0">所有订单</span>
                    <span status="1">等待买家付款</span>
                    <span status="2">等待发货</span>
                    <span status="3">已发货</span>
                    <span status="5">成功订单</span>
                    <span status="21">待评价订单</span>
                    <span status="99">历史订单</span>
                </div>
            </div>
        </td>
        <td align="center" width="12%">操作</td>

    </tr>
</table>
<script type="text/javascript">
    $(document).ready(function (e) {
        //if ($(".tab_box1 tr").length <= 1) {
        //    $("#StatusList").addClass("null");
        //}
        $("#StatusList .txt2 span").click(function (e) {
            var status = $(this).attr("Status");
            window.location.href = "?orderStatus=" + status;
        })
    });
</script>


<!--订单查询增加生活号订单-->
<asp:Repeater ID="listOrders" runat="server">
    <ItemTemplate>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
            <tr class="ddgl">
                <td colspan="7">
                    <span class="ordertime"><%#Eval("OrderDate") %></span> <span>订单编号：<em><%# Eval("OrderId") %></em></span>
                    <asp:Image ID="imgRedEnvelope" runat="server" Visible="false" />
                    <asp:Label ID="lblsupplier" runat="server" Visible="false"></asp:Label>
                    <asp:Literal runat="server" ID="lblGiftTitle"></asp:Literal><span><asp:Literal ID="litPresale" runat="server" Visible="false"></asp:Literal></span>
                    <span><em><%#Eval("TakeCode").ToNullString().Trim().Length>0?Eval("TakeCode","提货码：{0}"):string.Empty %></em></span>
                    <%#Eval("IsError").ToBool()&&Eval("CloseReason").ToNullString() != "订单已退款完成"?"<span style='color:red'>支付异常,请联系商家</span>":"" %>                   
                </td>
            </tr>
            <tr class="ddgl_1">
                <td align="left" width="64%" class="rbnone bbnone">
                    <asp:Repeater ID="rpProduct" runat="server">
                        <HeaderTemplate>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="productlist">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td width="53%" class="img">
                                    <div class="pic">
                                        <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server" ProductName='<%# Eval("ItemDescription") %>'
                                            ProductId='<%# Eval("ProductId")%>' ImageLink="true">
                                            <Hi:HiImage runat="server" DataField="ThumbnailsUrl" ToolTip='<%# Eval("ItemDescription") %>' />                               
                                        </Hi:ProductDetailsLink>

                                    </div>
                                    <div class="info">
                                        <div class="name">
                                            <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ItemDescription") %>'
                                                ProductId='<%# Eval("ProductId")%>' ImageLink="false"></Hi:ProductDetailsLink>
                                        </div>
                                        <div class="sku"><%# Eval("SkuContent") %></div>
                                    </div>
                                </td>
                                <td width="15%">￥<%# Eval("ItemAdjustedPrice").ToDecimal().F2ToString("f2") %></td>
                                <td width="15%"><%# Eval("Quantity") %><span style='<%# (int)Eval("ShipmentQuantity")>(int)Eval("Quantity")?"display:inline": "display:none" %>'>赠<%# (int)Eval("ShipmentQuantity")-(int)Eval("Quantity") %></span></td>
                                <td width="17%" class="rbnone" style="border-right: none;">
                                    <asp:Literal ID="litStatusText" runat="server"></asp:Literal>
                                    <asp:Label ID="ItemLogistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(this)">物流跟踪</a></asp:Label>
                                    <a href="javascript:void(0)" onclick="ToAfterSales(this)" runat="server" id="lkbtnAfterSalesApply" visible="false">申请售后</a>
                                    <a href="javascript:void(0)" onclick="ViewMessage(this)" runat="server" id="lkbtnViewMessage" visible="false">查看信息</a>
                                    <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(this)">物流跟踪</a></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate></table></FooterTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="rpGift" runat="server">
                        <ItemTemplate>
                            <headertemplate>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="productlist">
                        </headertemplate>
                            <itemtemplate>
                            <tr>
                                <td width="53%" class="img">
                                    <div class="pic">
                                         <a style="float: left" href='<%#GetRouteUrl("GiftDetails", new {giftId =Eval("GiftId")})%>' title='<%#Eval("GiftName") %>'>
                                    <img title='<%# Eval("GiftName") %>' src="<%# string.IsNullOrEmpty(Eval("ThumbnailsUrl").ToNullString())?HttpContext.Current.Request.ApplicationPath+HiContext.Current.SiteSettings.DefaultProductThumbnail2:Eval("ThumbnailsUrl").ToString().Replace("thumbs40/40","thumbs60/60") %>" />
                                </a>

                                    </div>
                                    <div class="info">
                                        <div class="name">
                                              <a style="float: left" href='<%#GetRouteUrl("GiftDetails", new {giftId =Eval("GiftId")})%>' title='<%#Eval("GiftName") %>'>
                                              <%# Eval("GiftName") %>
                                              </a>
                                        </div>
                                        <div class="sku"></div>
                                    </div>
                                </td>                               
                            </tr>
                        </itemtemplate>
                            <footertemplate></table></footertemplate
                        </ItemTemplate>
                    </asp:Repeater>
                </td>

                <td align="center" nowrap="nowrap" class="top_1" width="12%">
                    <div>
                        <span <%# Eval("PreSaleId").ToInt() > 0 ? "": "style='display:none'" %>>总计:</span>
                        <span style="color: #ff5722; font-weight: bold;">￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("OrderTotal") %>' runat="server" /></span>
                    </div>
                    <div class="dj_1" <%# Eval("PreSaleId").ToInt() > 0 ? "": "style='display:none'" %>>
                        <span>定金:</span>
                        <span style="color: #ff5722; font-weight: bold;">
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("Deposit") %>' runat="server" /></span>
                    </div>
                    <div class="dj_1" <%# Eval("PreSaleId").ToInt() > 0 ? "": "style='display:none'" %>>
                        <span>尾款:</span>
                        <span style="color: #ff5722; font-weight: bold;">
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel3" Money='<%# Eval("FinalPayment") %>' runat="server" /></span>
                    </div>
                    <span><a href="<%# Eval("GateWay").ToString()=="hishop.plugins.payment.bankrequest"&&Eval("OrderStatus").ToString()=="1"?"bank.aspx?OrderId="+Eval("OrderId"):"javascript:void(0)"%>" target="_blank"><%# Eval("PaymentType") %></a></span>
                </td>
                <td align="center" nowrap="nowrap" class="top_order_state" width="12%">
                    <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' class="sdf" PreSaleId='<%# Eval("PreSaleId").ToInt() %>'
                        runat="server" ShipmentModelId='<%#Eval("ShippingModeId") %>' Gateway='<%# Eval("GateWay") %>' IsConfirm='<%#Eval("IsConfirm").ToBool() %>' OrderType='<%#(Hidistro.Entities.Orders.OrderType)Eval("OrderType").ToInt() %>' />
                    <span>
                        <asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" Text="查看" /></span>
                    <span style="color: red;">
                        <%#Eval("IsError").ToBool() && Eval("FightGroupId").ToInt() == 0? "支付异常，请联系商家":""%> 
                    </span>

                    <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(this)">物流跟踪</a></asp:Label>

                </td>
                <td align="center" nowrap="nowrap" class="top_1 last-btn" width="12%">
                    <asp:HyperLink ID="hplinkorderreview" runat="server" NavigateUrl='<%# GetRouteUrl("user_OrderReviews", new {OrderId =Eval("orderId")}) %>'>评论</asp:HyperLink>
                    <a href="javascript:void(0)" onclick="PayOrderFun(this)" runat="server" oid='<%# Eval("OrderId") %>' countdownid='<%#Eval("CountDownBuyId") %>' groupbuyid='<%#Eval("GroupBuyId") %>' presaleid='<%#Eval("PreSaleId") %>'
                        pid='<%# Eval("PaymentTypeId") %>' fightgroupid='<%# Eval("FightGroupId") %>' id="hlinkPay">付款</a>
                    <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="确认收货"
                        CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE" DeleteMsg="确认已经收到货并完成该订单吗？"
                        Visible="false" />
                    <Hi:ImageLinkButton ID="lkbtnCloseOrder" IsShow="true" runat="server" Text="取消" CommandArgument='<%# Eval("OrderId") %>'
                        CommandName="CLOSE_TRADE" DeleteMsg="确认取消该订单吗？" Visible="false" />
                    <a href="javascript:void(0)" onclick="return ApplyForRefund(this)" runat="server"
                        id="lkbtnApplyForRefund" visible="false">申请退款</a>
                    <a href="javascript:void(0)" onclick="return UserRealNameVerify(this)" runat="server"
                        id="lkbtnUserRealNameVerify" visible="false">实名验证</a>
                    <a href="javascript:void(0)" onclick="ViewMessage(this)" runat="server" id="lkbtnViewMessage" visible="false">查看信息</a>
                    <a runat="server" id="lkbtnRefundDetail" visible="false">退款详情</a>
                </td>
            </tr>
        </table>
    </ItemTemplate>
</asp:Repeater>
<asp:Panel ID="panl_nodata" runat="server" Visible="false">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
        <tr>
            <td colspan="5" align="center">暂无订单，这就去挑选商品：<a href="/Default.aspx">商城首页</a> <a href="/User/Favorites.aspx">收藏夹</a>
            </td>
        </tr>
    </table>
</asp:Panel>

<div id="myTab_Content1" class="none">
    <div id="spExpressData">
        正在加载中....
    </div>
</div>
<div id="showMessage" class="none">
    <div id="messageContent"></div>
</div>
<script type="text/javascript">
    function GetLogisticsInformation(obj) {
        var action = $(obj).parent().attr("action");
        var orderId = $(obj).parent().attr("OrderId");
        var replaceId = $(obj).parent().attr("ReplaceId");
        var returnId = $(obj).parent().attr("returnId");
        if (action == "order") {
            $('#spExpressData').expressInfo(orderId, 'OrderId');
        }
        else if (action == "replace") {
            $('#spExpressData').ReturnOrReplaceExpressData(replaceId, "replace");
        }
        else if (action == "return") {
            $('#spExpressData').ReturnOrReplaceExpressData(returnId, "return");
        }
        ShowMessageDialog("物流详情", "Exprass", "myTab_Content1")
    }
    function ViewMessage(obj) {
        var content = $(obj).attr("content");
        var title = $(obj).attr("title");
        $("#messageContent").html(content);
        DialogShow(title, "viewmessage", "viewmessage_div", "UserReturnsApply_btnViewMessage");
        return false;
    }

    function ViewMessage(obj) {
        var content = $(obj).attr("content");
        var title = $(obj).attr("title");
        $("#messageContent").html(content);
        ShowMessageDialog(title, "showmessage", "showMessage")
    }

    //实名验证
    function UserRealNameVerify(obj) {
        var orderId = $(obj).attr("OrderId");
        var vsendhref = "/user/SubmitIDInfo.aspx?OrderId=" + orderId;
        DialogFrame(vsendhref, '实名认证（根据海关要求，购买跨境电商需要实名认证）', 850, 600, function () { location.reload(); });
        return false;
    }

    function GetRedEnvelope(orderID) {
        url = "/user/GetRedEnvelopeCode?OrderId=" + orderID;
        DialogFrame(url, "扫二维码分享红包", 800, 600, function (e) { })
        return false;
    }
</script>
