<asp:Repeater ID="rptSuppliers" runat="server">
    <ItemTemplate>
        <div class="step2">
            <div id="divSupplier" runat="server" class="stitle">
                <asp:Label runat="server" ID="lblSupplierName"></asp:Label>
            </div>
            <ul>
                <asp:Repeater ID="rptOrderProducts" runat="server">
                    <ItemTemplate>
                        <li>
                            <a id="lnkProductReview" runat="server" class="detailLink">
                                <hi:listimage id="ListImage1" runat="server" datafield="ThumbnailUrl180" />
                            </a>
                            <div class="step2_center">
                                <h3><a id="lnkProductName" runat="server"
                                    class="detailLink text-ellipsis">
                                    <asp:Literal runat="server" ID="lblName"></asp:Literal></a></h3>
                                <span></span>
                                <samp class="specification">
                                    <input name="skucontent" type="hidden" id="hidSkucontent" runat="server" clientidmode="static" />
                                    <input name="promotionName" type="hidden" id="hidPromotionName" runat="server" />
                                    <input name="promotionShortName" type="hidden" id="hidPromotionShortName" runat="server" />
                                </samp>
                            </div>
                           <!-- <div class="SendRedEnvelope" id="show_Envelop">
                            	<a href="/vshop/SendRedEnvelope"></a>
                            </div>-->
                            <span class="step2_price"><i>￥<asp:Label runat="server" ID="lblAdjustedPrice"></asp:Label>
                            </i></span>
                            <span class="step2_num">×<asp:Label runat="server" ID="lblQuantity"></asp:Label><asp:Label runat="server" ID="lblSend"></asp:Label>
                            </span>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div class="tj_gys" runat="server" id="divFreightAndAmount">
                <i>运费：<asp:Label runat="server" ID="lblFreight"></asp:Label></i>
                <i>小计：<asp:Label runat="server" ID="lblSupplierAmount"></asp:Label></i>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>
