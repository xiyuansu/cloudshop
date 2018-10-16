<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>


<li>

    <div class="buy_product">
        <div class="buy_time">
            <span id='<%# "htmlspan"+Eval("CountDownId") %>'></span>
                <Hi:LeaveListTime runat="server" ID="LeaveListTime" />
        </div>
        <div class="buy_pic" style="position: relative;">
            <a href='<%#"/CountDownProductsDetails.aspx?countDownId="+ Eval("CountDownId") %>'>
            <asp:Image ID="imageOver" runat="server" style="position:absolute;bottom:0;right:0;width:121px;height:121px;"/>
                </a>
            <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" 
                ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>'  href='<%#"/CountDownProductsDetails.aspx?countDownId="+ Eval("CountDownId") %>'
                ImageLink="true">
            <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl410" /></Hi:ProductDetailsLink>
        </div>
        
        <div class="buy_info">
        <div class="buy_name">
            <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server" 
                    ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>'  href='<%#"/CountDownProductsDetails.aspx?countDownId="+ Eval("CountDownId") %>'
                    ImageLink="false" />
        </div>
        <div class="buy_price">
            <em>￥<span><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("CountDownPrice") %>' /></span></em>
            <del>￥<Hi:FormatedMoneyLabel runat="server" ID="lblOldPrice" Money='<%# Eval("SalePrice") %>' /></del>
            <p id="countDownBackGroundImage" runat="server">                
               <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server"  ProductName='<%# Eval("ProductName") %>' href='<%#"/CountDownProductsDetails.aspx?countDownId="+ Eval("CountDownId") %>'
                    ProductId='<%# Eval("ProductId") %>' ImageLink="true" Text="去看看">
               </Hi:ProductDetailsLink>
                
            </p>
        </div>
        <div class="buy_andere">              
            <em style="display: none;">限购数量：<span><asp:Label runat="server" ID="lblCount" Text='<%# Eval("MaxCount") %>' /></span></em></div>
    </div>
    </div>
</li>
<div style="display: none;">
    结束时间：<Hi:FormatedTimeLabel runat="server" ID="lblEndTime" Time='<%# Eval("EndDate") %>' /></div>
