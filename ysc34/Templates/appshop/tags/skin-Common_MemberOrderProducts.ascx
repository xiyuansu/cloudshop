<ul>
    <asp:Repeater ID="rptSuppliers" runat="server">
        <ItemTemplate>
            <div class="step2">
                <div id="divSupplier" runat="server" class="stitle">
                    <asp:Label runat="server" ID="lblSupplierName"></asp:Label>
                </div>
                <div class="SendRedEnvelopeHeard_1" runat="server" id="divSendRedEnvelope" visible="false" onclick="showshare()">
                   <a href="/vshop/SendRedEnvelope.aspx"></a>
                </div>
            </div>
            <asp:Repeater ID="listOrderItems" runat="server">
                <ItemTemplate>
                    <li>
                        <a id="lnkProductReview" runat="server" class="detailLink AppProductDetail">
                            <hi:listimage id="ListImage1" runat="server" datafield="ThumbnailsUrl" />
                        </a>
                        <div class="step2_center">
                            <h3><a runat="server" class="detailLink text-ellipsis AppProductDetail" id="hylinkProductName"></a></h3>
                            <span>
                                <asp:Literal ID="ltlSKUContent" runat="server"></asp:Literal></span>
                        </div>
                        <span class="step2_price"><i>￥<asp:Literal ID="litPrice" runat="server"></asp:Literal></i> </span>
                        <span class="step2_num">×
                        <asp:Literal ID="ltlProductCount" runat="server"></asp:Literal>
                            <asp:Label ID="litSendCount" runat="server"></asp:Label>
                        </span>
                        <div class="refund">
                            <a runat="server" href="javascript:void(0)" visible="false" id="lkbtnItemStatus"></a>

                            <a runat="server" id="lkbtnApplyAfterSale" visible="false" class='btn_custom btn_default_m_1'>申请售后</a>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ItemTemplate>
    </asp:Repeater>
</ul>
<script language="javascript" type="text/javascript">

    function openSaleAfter(obj) {
        location.href = obj.href;
    }
    function GetLogisticsInformation(obj) {
        var replaceId = parseInt($(obj).parent().attr("replaceId"));
        var returnsId = parseInt($(obj).parent().attr("returnsId"));
        if (!isNaN(returnsId) && returnsId > 0) {
            document.location.href = "MyLogistics.aspx?returnsId=" + returnsId;
        }
        if (!isNaN(replaceId) && replaceId > 0) {
            document.location.href = "MyLogistics.aspx?replaceId=" + replaceId;
        }
    }
    function AfterSaleDetial(obj) {
        var replaceId = parseInt($(obj).attr("replaceId"));
        var returnsId = parseInt($(obj).attr("returnsId"));
        if (!isNaN(returnsId) && returnsId > 0) {
            document.location.href = "UserReturnDetail?returnId=" + returnsId;
        }
        //if (!isNaN(replaceId) && replaceId > 0) {
        //    document.location.href = "ReplaceDetail.aspx?replaceId=" + replaceId;
        //}
    }

    function ViewMessage(obj) {
        var content = decodeURIComponent($(obj).attr("content"));

        var title = decodeURIComponent($(obj).attr("title"));
        alert_h(content);
    }

</script>
