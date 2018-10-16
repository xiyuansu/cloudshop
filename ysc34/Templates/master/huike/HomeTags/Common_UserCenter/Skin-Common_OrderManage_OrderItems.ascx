<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
    <tr id="spqingdan_title">
        <td width="9%" align="center">商品信息</td>
        <td width="8%" align="center">货号</td>
        <td width="33%" align="center">商品名称</td>
        <td width="10%" align="center">购买数量</td>
        <td width="10%" align="center">商品单价</td>
        <td width="10%" align="center">发货数量</td>
        <td width="10%" align="center">状态</td>
        <td width="13%" align="center">操作</td>
    </tr>
    <asp:Repeater ID="dataListOrderItems" runat="server">
        <ItemTemplate>
            <tr>
                <td align="center">
                    <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server" ProductName='<%# Eval("ItemDescription") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                        <Hi:ListImage ID="Common_ProductThumbnail1" Width="60px" Height="60px" runat="server" DataField="ThumbnailsUrl"/>
                    </Hi:ProductDetailsLink>
                </td>
                <td align="center">
                    <asp:Literal ID="litSKU" runat="server" Text='<%# Eval("SKU") %>'></asp:Literal>&nbsp;</td>
                <td align="center">
                    <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ItemDescription") %>' ProductId='<%# Eval("ProductId") %>' runat="server" />
                    <br />
                    <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                </td>
                <td align="center">
                    <asp:Literal ID="lblProductQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal></td>
                <td align="center">
                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel" runat="server" Money='<%# Eval("ItemListPrice") %>' />
                </td>
                <td align="center">
                    <asp:Literal ID="lblShipQuantity" runat="server" Text='<%# Eval("ShipmentQuantity") %>'></asp:Literal></td>
                <td align="center" nowrap="nowrap">
                    <asp:Literal ID="litStatusText" runat="server"></asp:Literal><br />
                    <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(this)">物流跟踪</a></asp:Label>
                </td>
                <td align="center" nowrap="nowrap">
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
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
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


</script>

