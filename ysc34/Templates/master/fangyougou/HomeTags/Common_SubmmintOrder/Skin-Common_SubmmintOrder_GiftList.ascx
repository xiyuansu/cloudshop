<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Panel ID="pnlAllGift" runat="server" Visible="false">
    <div class="list">
        <em>礼品：</em>
        <ul>
            <asp:Repeater ID="dataShopGiftCart" runat="server">
                <ItemTemplate>
                    <li>
                        <b><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" title='<%#Eval("Name") %>'>
                            <Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl180" runat="server" Width="30" Height="30" />
                        </a>
                        </b>
                        <span><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" title='<%#Eval("Name") %>'></a>×<%# Eval("Quantity")%> </span>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</asp:Panel>