<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<div id="divGiftList" runat="server" visible="false">
    <h3>礼品清单</h3>
    <div class="cart_Order_info2">
        <div class="list">
            <div class="title">
                <div class="name"><em>礼品</em></div>
                <div class="price">积分</div>
                <div class="num">数量</div>
                <div class="total">总计</div>
            </div>
            <asp:Repeater ID="repPointGifts" runat="server">
                <ItemTemplate>
                    <div class="con">
                        <div class="name">
                            <div class="pic">
                                <a href='<%# GetRouteUrl("GiftDetails", new {giftId =Eval("GiftId")})%>' target="_blank" title='<%#Eval("Name") %>'>
                                    <Hi:ListImage DataField="ThumbnailUrl180" runat="server" Width="80" />
                                </a>
                            </div>
                            <div class="item-msg">
                                <a href='<%# GetRouteUrl("GiftDetails", new {giftId =Eval("GiftId")})%>' target="_blank" title='<%#Eval("Name") %>'><%# Eval("Name") %></a>
                            </div>
                        </div>
                        <div class="price">
                            <span><%# Eval("NeedPoint")%></span>
                        </div>
                        <div class="num">
                            <%# Eval("Quantity")%>
                        </div>
                        <div class="total">
                            <span><%# Eval("SubPointTotal") %></span>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
