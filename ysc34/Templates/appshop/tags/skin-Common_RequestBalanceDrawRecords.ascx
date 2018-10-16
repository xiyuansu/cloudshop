<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>

<a href="RequestBalanceDraw_Detail.aspx?id=<%# Eval("ID") %>">
    <div class="referral_1">
        <div>
            <Hi:RequestBalanceDrawIsPassLable IsPass='<%#Eval("IsPass") %>' RequestState='<%#Eval("RequestState")%>' runat="server"></Hi:RequestBalanceDrawIsPassLable>
        </div>
        <div style="text-align: center;">
            ￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("Amount") %>' runat="server" />
        </div>
        <div class="color_9">
            <%# Eval("RequestTime","{0:yyyy-MM-dd}") %>
        </div>
    </div>
</a>