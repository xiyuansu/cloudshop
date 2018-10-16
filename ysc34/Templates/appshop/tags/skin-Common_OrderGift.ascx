<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>

<div class="step2_gift">
    <%#Eval("PromoType").ToInt() == (int)Hidistro.Entities.Promotions.PromoteType.FullAmountSentGift||Eval("PromoType").ToInt() == (int)Hidistro.Entities.Promotions.PromoteType.SentGift?" <i class=\"tag tag_blue\">送</i>": "<i class=\"tag tag_green\">减</i>" %>
    <span><%# Eval("Name").ToNullString().Length>20?Eval("Name").ToNullString().Substring(0,20)+"...": Eval("Name").ToNullString()%> × <%# Eval("Quantity") %></span>
</div>
