<asp:Repeater ID="listOrders" runat="server">
    <ItemTemplate>
        <div class="panel panel-default order-list">
            <div class="panel-heading">
                <h3 class="panel-title stitle" id="OrderSupplierH3" runat="server">
                    <span id="OrderIdSpan" runat="server"></span>
                </h3>
                <asp:Literal runat="server" ID="litGiftTitle"></asp:Literal>
                <span class="order_shipping" id="divOrderStatus" runat="server">
                    <hi:orderstatuslabel visible="false" id="OrderStatusLabel1" runat="server" />
                    <asp:Label runat="server" ID="OrderStatusLabel2" Visible="false"></asp:Label>
                </span>
                <span class="order_shipping" style="color: red;" id="divOrderError" runat="server" visible="false">支付异常,请联系商家
                </span>
            </div>
            
                <div class="SendRedEnvelopeHeard" runat="server" id="divSendRedEnvelope" visible="false">
                    <a href="/vshop/SendRedEnvelope.aspx"></a> 
                </div>            
            <div class="panel-body">
                <div class="panel-body_1 step2 mt_0" id="divToDetail" runat="server">
                    <ul>
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a runat="server" class="detailLink" id="hyDetailLink">
                                        <hi:listimage runat="server" datafield="ThumbnailsUrl" />
                                    </a>
                                    <div class="step2_center">
                                        <a runat="server" id="hylinkProductName"
                                            class="detailLink text-ellipsis"></a>
                                        <span>
                                            <asp:Literal ID="ltlSKUContent" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                    <span class="step2_price"><i>￥<asp:Literal ID="ltlPrice" runat="server"></asp:Literal></i> </span>
                                    <span class="step2_num">×
                                        <asp:Literal ID="ltlProductCount" runat="server"></asp:Literal>
                                        <asp:Label ID="litSendCount" runat="server"></asp:Label>
                                    </span>
                                    <span class="step2_num" style="top: 2.0rem; color: red; font-weight: normal;">
                                        <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                    </span>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:Repeater ID="rptPointGifts" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a runat="server" class="detailLink" id="hyDetailLink">
                                        <hi:listimage runat="server" datafield="ThumbnailsUrl" />
                                    </a>
                                    <div class="step2_center">
                                        <h3>
                                            <a runat="server" id="hylinkGiftName"
                                                class="detailLink text-ellipsis"></a>
                                        </h3>
                                    </div>
                                    <span class="step2_price"><i>
                                        <asp:Literal ID="ltlPoints" runat="server"></asp:Literal></i> </span>
                                    <span class="step2_num">×
                                        <asp:Literal ID="ltlGiftCount" runat="server"></asp:Literal>
                                    </span>
                                </li>

                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <div class="panel-body_2" id="divOrderItems" runat="server">
                    <!--<span class="date">下单时间：<span id="OrderDateSpan" runat="server"></span></span>-->
                    <span class="float_r">共<asp:Literal ID="ltlOrderItems" runat="server"></asp:Literal>
                        件商品&nbsp;&nbsp;<span runat="server" id="sptotal">总价：￥</span><span id="PayMoneySpan" runat="server" class="float_r"></span></span>
                </div>
                <div class="panel-body_2" id="divOrderGifts" runat="server">
                    <span class="float_r">共<asp:Literal ID="ltlOrderGifts" runat="server"></asp:Literal>
                        件礼品&nbsp;&nbsp;</span>
                </div>
                <div class="panel-body_3" id="panelOperaters" runat="server">
                    <a id="lnkCertification" runat="server" class="btn_custom btn_orange_m paylink">实名认证</a>
                    <a runat="server" id="lkbtnApplyForRefund" visible="false" class='btn_custom btn_default_m'>申请退款</a>
                    <a href="javascript:void(0)" onclick="ViewMessage(this)" runat="server" id="lkbtnViewMessage" visible="false" class='btn_custom btn_default_m'>查看信息</a>
                    <a id="lnkViewTakeCodeQRCode" runat="server" visible="false" class='btn_custom btn_default_m'>提货二维码</a>
                    <a id="lnkViewLogistics" runat="server" class='btn_custom btn_default_m'>查看物流</a>
                    <a id="lnkClose" runat="server" class="btn_custom btn_default_m" visible="false">取消订单</a>
                    <a id="lnkToPay" runat="server" class="btn_custom btn_orange_m paylink">付款</a>
                    <a id="lnkHelpLink" runat="server" class='btn_custom btn_default_m'>线下支付帮助</a>
                    <a id="lnkFinishOrder" runat="server" href='javascript:void(0)' class='btn_custom btn_orange_m'>确认收货</a>
                    <a runat="server" class="btn_custom btn_default_m" id="lkbtnProductReview" visible="false">评价订单</a>
                    <a runat="server" class="btn_custom btn_default_m" id="lkbtnCouponCode" visible="false">查看劵码</a>
                </div>
            </div>
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
