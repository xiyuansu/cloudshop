<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Context" %>
<script src="/Utility/expressInfo.js" type="text/javascript"></script>


<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
    <tr id="spqingdan_title">
        <td align="center">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="producttitle">
                <tr>
                    <td width="25%" class="img" style="border:none;border-right:1px solid #ddd;">商品信息
                    </td>
                    <td width="18%" style="border:none;border-right:1px solid #ddd;">单价</td>
                    <td width="20%" style="border:none;border-right:1px solid #ddd;">数量</td>
                    <td width="30%" style="border:none;">商品状态</td>
                </tr>
            </table> 
        </td>
        <td align="center" nowrap="nowrap">收货人
        </td>
        <td align="center">订单金额
        </td>
        <td align="center">订单状态
        </td>
        <td align="center">操作
        </td>
    </tr>
    <!--订单查询增加服务窗订单-->
    <asp:Repeater ID="listOrders" runat="server">
        <ItemTemplate>
            <tr class="ddgl">
                <td colspan="5">
                    <span>订单编号：<em style="color: #2060af;"><%# Eval("OrderId") %></em></span> <span>下单日期：<%#Eval("OrderDate") %></span>
                    <span><em style="color: #2060af;"><%#Eval("TakeCode").ToNullString().Trim().Length>0?Eval("TakeCode","提货码：{0}"):string.Empty %></em></span>
                </td>
            </tr>
            <tr class="ddgl_1">
                <td align="left" width="40%" class="rbnone bbnone">
                    <asp:Repeater ID="rpProduct" runat="server">
                        <HeaderTemplate>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="productlist">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td width="26%" class="img">
                                    <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server" ProductName='<%# Eval("ItemDescription") %>'
                                        ProductId='<%# Eval("ProductId")%>' ImageLink="true">
                                 <img title='<%# Eval("ItemDescription") %>' src="<%# Eval("ThumbnailsUrl") %>" />
                                    </Hi:ProductDetailsLink>
                                </td>
                                <td width="20%"><%# Eval("RealTotalPrice") %></td>
                                <td width="20%"><%# Eval("Quantity") %></td>
                                <td width="34%" class="rbnone" style="border-right: none;">
                                    <asp:Literal ID="litStatusText" runat="server"></asp:Literal><br />
                                    <asp:Label ID="ItemLogistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(this)">物流跟踪</a></asp:Label>
                                    <a href="javascript:void(0)" onclick="return ApplyForRefund(this)" runat="server"
                                        id="lkbtnApplyForRefund" visible="false">申请退款</a>
                                    <a href="javascript:void(0)" onclick="return ApplyForReturn(this)" runat="server"
                                        id="lkbtnApplyForReturn" visible="false">申请退货</a>
                                    <a href="javascript:void(0)" onclick="return ApplyForReplace(this)" runat="server"
                                        id="lkbtnApplyForReplace" visible="false">申请换货</a>
                                    <a href="javascript:void(0)" onclick="return SendGoodsForReturns(this)" runat="server" id="lkbtnSendGoodsForReturn" visible="false">退货发货</a>
                                    <a href="javascript:void(0)" onclick="return SendGoodsForReplace(this)" runat="server" id="lkbtnSendGoodsForReplace" visible="false">换货发货</a>
                                    <a href="javascript:void(0)" onclick="return FinishForReplace(this)" runat="server" id="lkbtnFinishReplace" visible="false">确认收货</a>
                                    <a href="javascript:void(0)" onclick="ViewMessage(this)" runat="server" id="lkbtnViewMessage" visible="false">查看信息</a>
                                    <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(this)">物流跟踪</a></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate></table></FooterTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="rpGift" runat="server">
                        <ItemTemplate>
                            <a href='<%#Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' title='<%#Eval("GiftName") %>'>
                                <img style="padding:10px 20px;" title='<%# Eval("GiftName") %>' src="<%# string.IsNullOrEmpty(Eval("ThumbnailsUrl").ToString())?Utils.ApplicationPath+HiContext.Current.SiteSettings.DefaultProductThumbnail2:Utils.ApplicationPath+Eval("ThumbnailsUrl").ToString().Replace("thumbs40/40","thumbs60/60") %>" />
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td align="center" nowrap="nowrap">
                    <%# Eval("ShipTo") %>
                </td>
                <td align="center" nowrap="nowrap">
                    <span>
                        <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("OrderTotal") %>'
                            runat="server" /></span> <span>
                                <%# Eval("PaymentType") %></span>
                    <%# Eval("GateWay").ToString()=="hishop.plugins.payment.bankrequest"&&Eval("OrderStatus").ToString()=="1"?"<span><a href=\"bank.aspx?OrderId="+Eval("OrderId")+"\" target=\"_blank\">线下付款</a></span>":"" %>
                </td>
                <td align="center" nowrap="nowrap">
                    <span class="fkzhuangtai">
                        <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>'
                            runat="server" ShipmentModelId='<%#Eval("ShippingModeId") %>' Gateway='<%# Eval("GateWay") %>' IsConfirm='<%#Eval("IsConfirm").ToBool() %>'/></span>
                </td>
                <td align="center" nowrap="nowrap">
                    <asp:HyperLink ID="hplinkorderreview" runat="server" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderReviews",Eval("orderId")) %>'>评论</asp:HyperLink>
                    <asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_OrderDetails",Eval("orderId"))%>'
                        Text="查看" />
                    <a href="javascript:void(0)" onclick="return paySelect(this)" runat="server" oid='<%# Eval("OrderId") %>' CountDownId='<%#Eval("CountDownBuyId") %>' GroupBuyId='<%#Eval("GroupBuyId") %>'
                        pid='<%# Eval("PaymentTypeId") %>' FightGroupId='<%# Eval("FightGroupId") %>' id="hlinkPay">付款</a>
                    <Hi:ImageLinkButton ID="lkbtnConfirmOrder" IsShow="true" runat="server" Text="确认订单"
                        CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE" DeleteMsg="确认已经收到货并完成该订单吗？"
                        Visible="false" ForeColor="Red" />
                    <Hi:ImageLinkButton ID="lkbtnCloseOrder" IsShow="true" runat="server" Text="取消" CommandArgument='<%# Eval("OrderId") %>'
                        CommandName="CLOSE_TRADE" DeleteMsg="确认取消该订单吗？" Visible="false" />
                    <a href="javascript:void(0)" onclick="return ApplyForRefund(this)" runat="server"
                        id="lkbtnApplyForRefund" visible="false">申请退款</a><br />
                    <a href="javascript:void(0)" onclick="return ApplyForReturn(this)" runat="server"
                        id="lkbtnApplyForReturn" visible="false">申请退货</a>
                    <a href="javascript:void(0)" onclick="return ApplyForReplace(this)" runat="server"
                        id="lkbtnApplyForReplace" visible="false">申请换货</a>
                    <a href="javascript:void(0)" onclick="return SendGoodsForReturns(this)" runat="server" id="lkbtnSendGoodsForReturn" visible="false">退货发货</a>
                    <a href="javascript:void(0)" onclick="return SendGoodsForReplace(this)" runat="server" id="lkbtnSendGoodsForReplace" visible="false">换货发货</a>
                    <a href="javascript:void(0)" onclick="return FinishForReplace(this)" runat="server" id="lkbtnFinishReplace" visible="false">确认收货</a>
                    <a href="javascript:void(0)" onclick="ViewMessage(this)" runat="server" id="lkbtnViewMessage" visible="false">查看信息</a>
                    <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(this)">物流跟踪</a></asp:Label>

                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Panel ID="panl_nodata" runat="server" Visible="false">
        <tr>
            <td colspan="5" align="center">暂无订单，这就去挑选商品：<a href="/Default.aspx">商城首页</a> <a href="/User/Favorites.aspx">收藏夹</a>
            </td>
        </tr>
    </asp:Panel>
</table>
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
</script>
