<asp:Repeater ID="listOrders" runat="server">
    <ItemTemplate>
        <div class="panel panel-default order-list">
            <a id="lnkToDetail" runat="server">
                <div class="panel-heading">
                    <h3 class="panel-title">订单号：
                        <span id="AfterSaleIdSpan" runat="server"></span>
                    </h3>
                    <span class="order_shipping">
                        <span id="StatusLabel1" runat="server" />
                    </span>
                </div>
                <div class="panel-body">
                    <div class="panel-body_1 step2 mt_0" id="divToDetail" runat="server">
                        <ul>
                            <asp:Repeater ID="Repeater1" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <hi:listimage runat="server" datafield="ThumbnailsUrl" />
                                        <div class="step2_center">
                                            <h3>
                                                <span id="hylinkProductName" runat="server"
                                                    class="detailLink text-ellipsis"></span>
                                            </h3>
                                            <span>
                                                <asp:Literal ID="ltlSKUContent" runat="server"></asp:Literal></span>
                                        </div>
                                        <span class="step2_price"><i>￥<asp:Literal ID="ltlPrice" runat="server"></asp:Literal></i> </span>
                                        <span class="step2_num">×
                                        <asp:Literal ID="ltlProductCount" runat="server"></asp:Literal>
                                        </span>
                                    </li>

                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <div class="panel-body_2">
                        <span class="float_r">交易金额:￥<asp:Literal ID="PayMoneySpan" runat="server"></asp:Literal>
                            &nbsp;&nbsp;退款金额:￥<span id="litRefundMoney" runat="server"></span></span>
                    </div>
                    <div class="panel-body_3" id="panelOperaters" runat="server">
                        <a href="javascript:void(0)" onclick="ViewMessage(this)" runat="server" id="lkbtnViewMessage" visible="false" class='btn_custom btn_default_m'>查看信息</a>
                        <a id="lnkViewLogistics" runat="server"
                            class='btn_custom btn_orange_m' visible="false">查看物流</a>
                        <a runat="server" id="lkbtnSendGoodsForReturn" visible="false" class="btn_custom btn_orange_m">退货</a>
                    </div>
                </div>
            </a>
        </div>
    </ItemTemplate>
</asp:Repeater>
<script language="javascript" type="text/javascript">
    function ViewMessage(obj) {
        var content = decodeURIComponent($(obj).attr("content"));

        var title = decodeURIComponent($(obj).attr("title"));
        alert_h(content);
    }

</script>
