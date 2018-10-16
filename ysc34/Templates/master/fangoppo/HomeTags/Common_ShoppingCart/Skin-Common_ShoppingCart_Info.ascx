<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<Hi:SiteUrl ID="SiteUrl1" UrlName="shoppingCart" Target="_blank" runat="server">

<div style=" display:none;">购物车<b  id="spanCartNum"><asp:Literal ID="cartNum" runat="server" Text="0" /></b> 件<b style=" display:none;">
<Hi:FormatedMoneyLabel ID="cartMoney" NullToDisplay="0.00" runat="server" /></b>元 </div></Hi:SiteUrl>
 