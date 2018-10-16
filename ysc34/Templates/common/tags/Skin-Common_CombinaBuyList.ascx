<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
 <div class="plus"></div>

<div class="product">
     <div class="pic"><a href="CombinationBuyDetail.aspx?combinaId=<%# Eval("CombinationId") %>"><img data-url="<%#string.IsNullOrEmpty(Eval("ThumbnailUrl180").ToNullString())?Hidistro.Context.HiContext.Current.SiteSettings.DefaultProductImage:Eval("ThumbnailUrl180") %>"></a></div>
     <div class="price"> <a href="CombinationBuyDetail.aspx?combinaId=<%# Eval("CombinationId") %>"><label><b>￥<%# Eval("minCombinationPrice") %></b></label></a></div>
</div>
