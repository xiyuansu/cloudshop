<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>

<%#(Eval("ItemIndex").ToInt() %2==0)?"<div class=\"coupon_list\">":(Eval("ItemIndex").ToInt() ==-1)?"<div class=\"coupon_list\">":""%>
<div class="coupon <%#Eval("isEnd").ToNullString() %>">
    <h1><%# Eval("OrderUseLimit").ToDecimal() == 0 ? "无限制" : "满" + Eval("OrderUseLimit").ToDecimal().F2ToString("f2") + "使用" %></h1>
    <span><i>￥</i><%#Eval("Price").ToDecimal().F2ToString("f2") %></span>
    <%#Eval("isEnd").ToNullString()=="couponed"?"<img src=\"/templates/common/images/coupons_over.png\" />":"" %>
    
</div>
<%#(Eval("ItemIndex").ToInt() %2==0)?"":"</div>"%>