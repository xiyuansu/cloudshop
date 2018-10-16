﻿<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 
<asp:Repeater ID="regroupbuy" runat="server">
    <ItemTemplate>
    <li>
        <div class="group-img">
            <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'><Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl180" IsDelayedLoading="true"/></Hi:ProductDetailsLink>
               
        </div>
        <div class="pro-info">
            <div class="pro-name">
                <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductId='<%# Eval("ProductId") %>' ProductName='<%# Eval("ProductName") %>' href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'></Hi:ProductDetailsLink>
            </div>
            <div class="pro-price">
                <span><i class="rmb">¥</i><Hi:FormatedMoneyLabel runat="server" ID="FormatedMoneyLabel1" Money='<%# Eval("Price") %>' /></span><i>¥<Hi:FormatedMoneyLabel runat="server" ID="lblNeedPrice" Money='<%# Eval("OldPrice") %>' style="color:#B5B5B5;font-size: 12px;" /></i>
            </div>
        </div>
        <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'><input type="button" value="马上参团" class="groupbybtn" > </Hi:ProductDetailsLink>
        </li>
    </ItemTemplate>
</asp:Repeater>
