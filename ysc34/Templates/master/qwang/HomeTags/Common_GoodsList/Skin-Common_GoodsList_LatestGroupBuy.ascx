<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 

<asp:Repeater ID="regroupbuy" runat="server">
	<ItemTemplate>
		<li>

			<div class="pic">
				<Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server" ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'>
					<Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl220" Width="214px" Height="198px" /></Hi:ProductDetailsLink>
			</div>
			<div class="info">
				<div class="name">
					<Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductId='<%# Eval("ProductId") %>' ProductName='<%# Eval("ProductName") %>' href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'></Hi:ProductDetailsLink>
				</div>
				<p class="tuangou_time"><span id='<%# "htmlspan"+Eval("ProductId") %>'></span>
					<Hi:LeaveListTime runat="server" ID="LeaveListTime" /> </p>
				<div class="jiageprigh">
					<div class="price font-yh fl"><b><em>￥</em>
            <Hi:FormatedMoneyLabel runat="server" ID="FormatedMoneyLabel1" Money='<%# Eval("Price") %>' /></b>
						<span style=" display:none;"><em>￥</em><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("OldPrice") %>'/></span></div>
					<div class="btn fr">
						<Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" ProductId='<%# Eval("ProductId") %>' ImageLink="true" href='<%#"/GroupBuyProductDetails.aspx?groupBuyId="+ Eval("GroupBuyId") %>'>购买 </Hi:ProductDetailsLink>
					</div>
				</div>
			</div>
		</li>
	</ItemTemplate>
</asp:Repeater>