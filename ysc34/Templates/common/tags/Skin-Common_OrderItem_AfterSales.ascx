<ul>
    <asp:Repeater ID="listOrderItems" runat="server">
        <ItemTemplate>
            <li>
                <a id="lnkProductReview" runat="server" class="detailLink">
                    <hi:listimage id="ListImage1" runat="server" datafield="ThumbnailsUrl" />
                </a>
                <div class="step2_center">
                    <h3><a runat="server" class="detailLink text-ellipsis" id="hylinkProductName"></a></h3>
                    <span>
                        <asp:Literal ID="ltlSKUContent" runat="server"></asp:Literal></span>
                </div>
                <span class="step2_price"><i>￥<asp:Literal ID="litPrice" runat="server"></asp:Literal></i> </span>
                <span class="step2_num">×
                    <asp:Literal ID="ltlProductCount" runat="server"></asp:Literal>
                </span>
            </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>