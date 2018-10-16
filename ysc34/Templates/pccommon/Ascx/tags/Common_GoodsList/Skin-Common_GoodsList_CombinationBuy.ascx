<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<div class="plus"></div>
<div class="product">
    <div class="pic"><a href='<%#"/ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><img data-url="<%# Eval("ThumbnailUrl100") %>" /></a></div>
    <div class="name"><a href='<%#"/ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></div>
    <div class="price"><input type="checkbox" <%# int.Parse(Eval("INDEX").ToString())==1?"checked='checked'":"" %> onchange="getCombinationTotalPrice()" id="combinationChk_<%# Eval("ProductId") %>" value="<%# Eval("minSalePrice") %>_<%# Eval("minCombinationPrice") %>" /> <label><b>￥<%# Eval("minCombinationPrice") %></b></label></div>
</div>
