<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 
<asp:Repeater ID="regroupbuy" runat="server">
    <ItemTemplate>
   <li>

              <p class="tuangou_time" style="display:none"><span id='<%# "htmlspan"+Eval("ProductId") %>'></span><Hi:LeaveListTime runat="server" ID="LeaveListTime" /> </p>  
            <div class="pic"><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'><Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl220" IsDelayedLoading="true" 
                Width="214px" Height="198px"/></Hi:ProductDetailsLink></div>
            <div class="info">
            <div class="name"  style="display:none;">  <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductId='<%# Eval("ProductId") %>' ProductName='<%# Eval("ProductName") %>' href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'></Hi:ProductDetailsLink></div>
            <div class="price font-yh float-l"><b><em>￥</em><Hi:FormatedMoneyLabel runat="server" ID="FormatedMoneyLabel1" Money='<%# Eval("Price") %>' /></b><span style="display:none;"><em>￥</em><Hi:FormatedMoneyLabel runat="server" ID="lblNeedPrice" Money='<%# Eval("OldPrice") %>' style="color:#B5B5B5;font-size: 12px;" /></span></div>
            <div class="btn float-r"> <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'>抢购</Hi:ProductDetailsLink></div>
            </div>

            </li> 

       
    </ItemTemplate>
</asp:Repeater>
