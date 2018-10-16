<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 
<asp:Repeater ID="regroupbuy" runat="server">
    <ItemTemplate>
    <li>
        <div class="time" >
            <span id='<%# "htmlspan"+Eval("GroupBuyId") %>'></span>
            <Hi:LeaveListTime runat="server" ID="LeaveListTime" />
        </div>
        
        <div class="pic">
            <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'>
            <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl160" IsDelayedLoading="true"/></Hi:ProductDetailsLink>
        </div>
    
        <div class="name">
      	    <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductId='<%# Eval("ProductId") %>' ProductName='<%# Eval("ProductName") %>' href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'></Hi:ProductDetailsLink>
        </div>
        
        <div class="price">
            <em>￥</em>
            <Hi:FormatedMoneyLabel runat="server" ID="FormatedMoneyLabel1" Money='<%# Eval("Price") %>' />
            <span style="display:none"><em>￥</em><Hi:FormatedMoneyLabel runat="server" ID="lblNeedPrice" Money='<%# Eval("OldPrice") %>' /></span>
        </div>
        
        <div class="btn float-r" style="display:none;">
            <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'>抢购</Hi:ProductDetailsLink>
        </div>
      </li>
    </ItemTemplate>
</asp:Repeater>
