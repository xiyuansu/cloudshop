<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Repeater ID="listPrducts" runat="server">
    <HeaderTemplate>
        <div class="title">
            <ul>
                <li style="width: 580px; padding-left: 20px;">商品名称</li>
                <li style="width: 250px;">购买数量</li>
                <li style="width: 90px;">单价</li>
            </ul>
        </div>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="con">
            <div class="info">
                <div class="pic">
                    <Hi:ProductDetailsLink ID='ProductDetailsLink' runat='server'  ProductName='<%# Eval("ItemDescription") %>'
                        ProductId='<%# Eval("ProductId")%>' ImageLink="true">
                        <Hi:HiImage DataField="ThumbnailsUrl" Width="60px" runat="server" ToolTip="" />
                    </Hi:ProductDetailsLink>
                </div>
                <div class="name">
                    <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ItemDescription") %>'
                        ProductId='<%# Eval("ProductId")%>' ImageLink="false"></Hi:ProductDetailsLink>
                     <div class="sku"> <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></div>
                </div>
                
            </div>
            <div class="num">X  <b><%# Eval("Quantity") %></b></div>
            <div class="price">￥<%# Eval("ItemAdjustedPrice").ToDecimal().F2ToString("f2") %></div>
        </div>
    </ItemTemplate>
</asp:Repeater>
