<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 
  <asp:Repeater ID="repcountdown" runat="server">
<ItemTemplate>
  
 <li>

            
            <div class="pic"> <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/CountDownProductsDetails.aspx?countDownId="+ Eval("CountDownId") %>'> 
        <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl160" IsDelayedLoading="true" /></Hi:ProductDetailsLink></div>
            <div class="name" style=" height:18px; overflow:hidden;"> <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>' href='<%#"/CountDownProductsDetails.aspx?countDownId="+ Eval("CountDownId") %>'
                    ProductId='<%# Eval("ProductId") %>' ImageLink="false" /></div>
            <div class="price">抢购价:<em>￥</em><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("CountDownPrice") %>' /></div>
            <div class="time"><span id='<%# "htmlspan"+Eval("CountDownId") %>'></span>
                <Hi:LeaveListTime runat="server" ID="LeaveListTime" /></div>
            </li>  
    
    </ItemTemplate>
</asp:Repeater>
<p style="display: none;">
    <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server"  ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true" CssClass="btnbuy" href='<%#"/CountDownProductsDetails.aspx?countDownId="+ Eval("CountDownId") %>'>立即抢购</Hi:ProductDetailsLink>
</p>
