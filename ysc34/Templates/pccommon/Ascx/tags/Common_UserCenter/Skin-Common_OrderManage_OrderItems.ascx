<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
    <tr id="spqingdan_title">

        <td width="50%" align="center">商品名称</td>
        <td width="12.5%" align="center">单价</td>
        <td width="12.5%" align="center">数量</td>
        <td width="12.5%" align="center">状态</td>
        <td width="12.5%" align="center">操作</td>
    </tr>
    <asp:Repeater ID="dataSupplier" runat="server">
        <ItemTemplate>
            <tr>
                <td colspan="5">
                    <asp:Label ID="lblsupplier" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <asp:Repeater ID="dataListOrderItems" runat="server">
                <ItemTemplate>
                    <tr>
                        <td align="center" width="50%">
                            <div class="pic">
                                <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server" ProductName='<%# Eval("ItemDescription") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                <Hi:ListImage ID="Common_ProductThumbnail1" Width="60px" Height="60px" runat="server" DataField="ThumbnailsUrl"/>
                                </Hi:ProductDetailsLink>
                            </div>
                            <div class="info">
                                <div class="name">
                                    <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ItemDescription") %>' ProductId='<%# Eval("ProductId") %>' runat="server" />
                                </div>

                                <div class="sku">
                                    <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                                </div>
                            </div>
                        </td>
                        <td align="center">
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel" runat="server" Money='<%# Eval("ItemAdjustedPrice") %>' />
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblProductQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal>
                            <span style='<%# (int)Eval("ShipmentQuantity")>(int)Eval("Quantity")?"display:inline": "display:none" %>'>赠<%# (int)Eval("ShipmentQuantity")-(int)Eval("Quantity") %></span>
                        </td>


                        <td align="center" nowrap="nowrap">
                            <asp:Literal ID="litStatusText" runat="server"></asp:Literal><br />
                            <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(this)">物流跟踪</a></asp:Label>
                        </td>
                        <td align="center" nowrap="nowrap">
                            <a href="javascript:void(0)" onclick="ToAfterSales(this)" runat="server" id="lkbtnAfterSalesApply" visible="false">申请售后</a>
                            <a href="javascript:void(0)" onclick="ViewMessage(this)" runat="server" id="lkbtnViewMessage" visible="false">查看信息</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
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
    ///去申请售后
    function ToAfterSales(obj) {
        var orderId = $(obj).attr("OrderId");
        var skuId = $(obj).attr("SkuId");
        if (orderId != undefined && skuId != undefined && orderId != "" && skuId != "") {
            window.location.href = "/User/AfterSalesApply?OrderId=" + orderId + "&SkuId=" + skuId;
        }
    }

</script>

