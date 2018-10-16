<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<div class="list">
    <div class="title">
        <div class="name"><em>商品</em></div>
        <div class="price">单价</div>
        <div class="num">数量</div>
        <div class="total">小计</div>
    </div>
    <asp:Repeater ID="dataListShoppingCrat" runat="server">
        <ItemTemplate>
            <div class="con skuid_<%# Eval("SKUid") %>">
                <div class="name">
                    <div class="pic">
                        <Hi:ProductDetailsLink ID="ProductDetailsLink2" ProductId='<%# Eval("ProductId")%>'
                            ProductName='<%# Eval("Name")%>' runat="server" ImageLink="true">
                                <Hi:ListImage ID="ListImage1"  DataField="ThumbnailUrl180" runat="server"  Width="80" />
                        </Hi:ProductDetailsLink>
                    </div>
                    <div class="item-msg">
                        <Hi:ProductDetailsLink ID="ProductDetailsLink1" ProductId='<%# Eval("ProductId")%>'
                            ProductName='<%# Eval("Name")%>' runat="server" ImageLink="false" />
                        <div class="p-extend">
                            <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                        </div>
                        <div class="gift_list">
                            <asp:Repeater ID="rptPromotionGifts" runat="server">
                                <ItemTemplate>
                                    <div class="gift">
                                        <em>
                                            <img src='<%#Eval("ThumbnailUrl40") %>' title='<%#Eval("Name") %>'></em> <b>×<%# DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "Quantity") %></b>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <div class="price">
                    <span>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" runat="server" Money='<%# Eval("MemberPrice")%>' /></span>
                </div>
                <div class="num">
                    <asp:Literal runat="server" ID="txtStock" Text='<%# Eval("Quantity")%>' />
                </div>
                <div class="total">
                    <span>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" runat="server" Money='<%#(Eval("AdjustedPrice").ToDecimal()*Eval("Quantity").ToInt())%>' /></span>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

