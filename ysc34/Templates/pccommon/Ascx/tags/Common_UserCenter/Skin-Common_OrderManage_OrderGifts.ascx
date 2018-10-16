<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
    <tr id="spqingdan_title">
        <td width="9%" align="center">礼品图片</td>
        <td width="33%" align="center">商品名称</td>
        <td width="10%" align="center">数量</td>     
        <td width="10%" align="center">礼品类型</td>
    </tr>
    <asp:Repeater ID="dataListOrderItems" runat="server">
        <ItemTemplate>
            <tr>
                <td align="center">
                    <Hi:ListImage ID="Common_ProductThumbnail1" Width="60px" Height="60px" runat="server" DataField="ThumbnailsUrl" />
                </td>
                <td align="center">
                    <%# Eval("GiftName") %>
                </td>
                <td align="center">
                    <asp:Literal ID="lblProductQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal></td>              
                <td align="center">
                    <%# Eval("PromoteType").ToInt() == (int)Hidistro.Entities.Promotions.PromoteType.SentGift?"商品促销":(Eval("PromoteType").ToInt() == (int)Hidistro.Entities.Promotions.PromoteType.FullAmountSentGift?"订单促销":(Eval("PromoteType").ToInt() == -1 ?"中奖":"积分兑换")) %>    
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>


