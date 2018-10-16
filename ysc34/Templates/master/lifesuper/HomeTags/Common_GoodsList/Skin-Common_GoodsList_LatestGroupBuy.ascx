﻿<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 
<asp:Repeater ID="regroupbuy" runat="server">
<ItemTemplate>
<li class="pl10 pr10">

             
            <div class="pic m0 mt10"><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'><Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl180" /></Hi:ProductDetailsLink></div>
            <div class="info">
            <div class="price center yahei font20"><em>￥</em><Hi:FormatedMoneyLabel runat="server" ID="FormatedMoneyLabel1" Money='<%# Eval("Price") %>' /><span style="display:none"><em>￥</em><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("OldPrice") %>'/></span></div>
            <div class="name"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductId='<%# Eval("ProductId") %>' ProductName='<%# Eval("ProductName") %>' href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'></Hi:ProductDetailsLink></div>
            <div class="btn"  style="display:none"><Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'>抢购</Hi:ProductDetailsLink></div>
            </div>
             <p class="tuangou_time mt10" ><span id='<%# "htmlspan"+Eval("GroupBuyId") %>'></span>
                <Hi:LeaveListTime runat="server" ID="LeaveListTime" />
 </p>  

            </li>
            </ItemTemplate>
</asp:Repeater>
