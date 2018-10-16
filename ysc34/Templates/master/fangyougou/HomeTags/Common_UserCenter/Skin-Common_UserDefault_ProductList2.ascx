<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Repeater ID="rp_hot" runat="server">
    <ItemTemplate>
        <li>
            <div class="hyzxpic">
                <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                                                    <Hi:ListImage runat="server" DataField="ThumbnailUrl160" CustomToolTip="ProductName" />
                </Hi:ProductDetailsLink>
            </div>
            <div class="hyzxname">
                <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true"><%# Eval("ProductName") %></Hi:ProductDetailsLink>
            </div>
            <div class="hyzxprice">
                <strong>￥<Hi:FormatedMoneyLabel Money='<%# Eval("SalePrice") %>' runat="server" /></strong>
                <em>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice") %>' runat="server" /></em>
            </div>
        </li>
    </ItemTemplate>

</asp:Repeater>

