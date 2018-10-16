<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_ItemsList.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Order_ItemsList" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<style>
    table tbody tr th {
        line-height: 50px;
        height: 50px;
        font-size: 14px;
    }

    .table-striped tbody tr td {
        padding: 15px 0 !important;
    }

    table-striped tbody tr th:first-child,
    table-striped tbody tr td:first-child {
        padding-left: 20px !important;
    }

    .table-striped > tbody > tr:nth-of-type(2n+1) {
        background-color: #f8f8f8;
    }
</style>
<asp:Repeater ID="dlstOrderItems" runat="server">
    <HeaderTemplate>
        <table class="table table-striped" style="margin-top: 20px; float: left;">
            <tbody>
                <tr>
                    <th colspan="2" style="padding-left: 10px;">商品名称</th>
                    <th style="<%=(ShowCostPrice?"": "display:none")%>">成本价</th>
                    <th>商品单价(元) </th>
                    <th>购买数量 </th>
                    <th>发货数量 </th>
                    <th>小计(元) </th>
                    <th style="<%=(ShowAllItem?"": "display:none")%>">状态</th>
                    <th nowrap="nowrap" style="<%=(ShowAllItem?"": "display:none")%>">操作</th>
                </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td width="7%">
                <a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank" style="float: left; margin-left: 10px;">
                    <Hi:ListImage ID="HiImage2" runat="server" DataField="ThumbnailsUrl" Width="40px" /></a>
            </td>

            <td width="32%">
                <span class="Name"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank" class="text-ellipsis" style="padding-right: 10px; width: 300px;">
                    <%# Eval("ItemDescription")%></a></span>
                <span class="colorC">货号：
                    <asp:Literal runat="server" ID="litCode" Text='<%#Eval("sku") %>' />
                    <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></span>
            </td>
            <td style="<%=(ShowCostPrice?"": "display:none")%>"><Hi:FormatedMoneyLabel ID="lblItemCostPrice" runat="server" Money='<%# Eval("ItemCostPrice") %>' /></td>
            <td>
                <Hi:FormatedMoneyLabel ID="lblItemListPrice" runat="server" Money='<%# Eval("ItemListPrice") %>' /></td>
            <td>
                <asp:Literal runat="server" ID="litQuantity" Text='<%#Eval("Quantity") %>' /></td>
            <td>
                <asp:Literal runat="server" ID="litShipmentQuantity" Text='<%#Eval("ShipmentQuantity") %>' /></td>
            <td>
                <div class="color_36c">
                    <asp:HyperLink ID="hlinkPurchase" Visible="false" runat="server" NavigateUrl='<%# string.Format(GetRouteUrl("FavourableDetails", new { activityId = Eval("PromotionId") }))%>'
                        Text='<%# Eval("PromotionName")%>' Target="_blank"></asp:HyperLink>
                </div>
                <strong class="colorG">
                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" runat="server" Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>' /></strong>
            </td>
            <td style="<%=(ShowAllItem?"": "display:none")%>">
                <asp:Literal ID="litStatusText" runat="server"></asp:Literal><br />
                <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(this)">物流跟踪</a></asp:Label></td>
            <td style="<%=(ShowAllItem?"": "display:none")%>" nowrap="nowrap">
               
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
<asp:Repeater ID="grdOrderGift" runat="server">
    <HeaderTemplate>
        <table class="table table-striped" style="margin-top: 20px; float: left;">
            <tbody>
                <tr>
                    <th colspan="2" style="padding-left: 10px;">礼品名称</th>
                    <th>成本价(元) </th>
                    <th>数量 </th>
                    <th>礼品类型</th>
                </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td width="7%">
                <Hi:ListImage ID="Common_ProductThumbnail1" Width="40px" Height="40px" runat="server" DataField="ThumbnailsUrl" />
            </td>
            <td width="50%">
                <span class="Name">
                   <a href="../../GiftDetails.aspx?GiftId=<%#Eval("GiftId") %>" target="_blank" class="text-ellipsis" style="padding-right: 10px; width: 400px;"><%# Eval("GiftName") %></a>
                </span>
            </td>
            <td>
                <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel" runat="server" Money='<%# Eval("CostPrice") %>' />                
            <td>
                <asp:Literal ID="lblProductQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal></td>
            </td>
            <td>
                <%# Eval("PromoteType").ToInt() == (int)Hidistro.Entities.Promotions.PromoteType.SentGift?"商品促销":(Eval("PromoteType").ToInt() == (int)Hidistro.Entities.Promotions.PromoteType.FullAmountSentGift?"订单促销": (IsPrize ?"中奖":"积分兑换")) %>    
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>






