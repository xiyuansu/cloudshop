﻿<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 
<asp:Repeater ID="regroupbuy" runat="server">
    <ItemTemplate>
        <div class="group_ip">
            <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'><Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl220" 
                Width="214px" Height="198px"/></Hi:ProductDetailsLink>
        </div>
        <div class="group_buy">
            <div class="pro-name" style="display:none;">
                <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductId='<%# Eval("ProductId") %>' ProductName='<%# Eval("ProductName") %>' href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'></Hi:ProductDetailsLink>
            </div>
           
                 <em style=" font-size:16px; font-weight:bold;">¥<Hi:FormatedMoneyLabel runat="server" ID="FormatedMoneyLabel1" Money='<%# Eval("Price") %>' /></em>
                 <em style=" text-decoration:line-through;">¥<Hi:FormatedMoneyLabel runat="server" ID="lblNeedPrice" Money='<%# Eval("OldPrice") %>' style="font-size: 12px;" />
            </em>
            <b> <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'> </Hi:ProductDetailsLink></b>
        </div>
       
    </ItemTemplate>
</asp:Repeater>