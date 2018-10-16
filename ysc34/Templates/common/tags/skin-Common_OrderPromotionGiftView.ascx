<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>

<div class="panel-body_2 step2_gift">
     <%#Eval("PromoteType").ToInt() == (int)Hidistro.Entities.Promotions.PromoteType.FullAmountSentGift||Eval("PromoteType").ToInt() == (int)Hidistro.Entities.Promotions.PromoteType.SentGift?" <i class=\"tag tag_blue\">送</i>": "<i class=\"tag tag_green\">减</i>" %>
    <span><%# Eval("GiftName").ToNullString().Length>20?Eval("GiftName").ToNullString().Substring(0,20)+"...": Eval("GiftName").ToNullString()%> × <%# Eval("Quantity") %></span>
</div>
