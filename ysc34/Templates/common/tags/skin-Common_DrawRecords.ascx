<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<a href="SplittinDraw_Detail.aspx?id=<%# Eval("JournalNumber") %>">
    <div class="referral_1">
        <div>
            <Hi:SplitDrawStatusLable Status = '<%#Eval("AuditStatus")%>' RequestState='<%#Eval("RequestState")%>' runat="server"></Hi:SplitDrawStatusLable></div>
           <div> ￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("Amount") %>' runat="server" /></div>       
            <div><Hi:FormatedTimeLabel ID="lblTradeDate" runat="server" Time='<%# Eval("RequestDate") %>' FormatDateTime="yyyy-MM-dd"></Hi:FormatedTimeLabel></div>    

    </div>
</a>