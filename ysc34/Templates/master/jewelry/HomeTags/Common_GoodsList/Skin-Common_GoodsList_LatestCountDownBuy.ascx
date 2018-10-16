﻿<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 
<asp:Repeater ID="repcountdown" runat="server">
<ItemTemplate>

 <li>

          <div class="time ml10 both"><span id='<%# "htmlspan"+Eval("CountDownId") %>'></span>
 <Hi:LeaveListTime runat="server" ID="LeaveListTime" /></div>  
           <div class="info m0 o-hidden">
            <div class="pic"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/CountDownProductsDetails.aspx?countDownId="+ Eval("CountDownId") %>'> 
        <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl180" /></Hi:ProductDetailsLink></div>
            <div class="name"><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>' href='<%#"/CountDownProductsDetails.aspx?countDownId="+ Eval("CountDownId") %>'
        ProductId='<%# Eval("ProductId") %>' ImageLink="false"/></div>
            <div class="price">抢购价:<span>￥<Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("CountDownPrice") %>'/></span></div>
            </div>
            </li>
                        </ItemTemplate>
</asp:Repeater>
<p style="display: none;">
    <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true" CssClass="btnbuy" href='<%#"/CountDownProductsDetails.aspx?countDownId="+ Eval("CountDownId") %>'>立即抢购</Hi:ProductDetailsLink>
</p>
