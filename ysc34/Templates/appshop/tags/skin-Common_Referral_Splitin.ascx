<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<a href="SplittinDetail_Detail.aspx?id=<%# Eval("JournalNumber") %>">
    <div class="referral clearfix">
        <Hi:HiImage ImageUrl="../images/purse.png" runat="server" CssClass="img-circle" />
        <div class="left">
            <div><%# (Eval("FromUserName")==DBNull.Value||Eval("FromUserName").ToString()=="")?"匿名用户":Eval("FromUserName") %></div>
            <span><Hi:SplittingTypeNameLabel ID="SplittingTypeNameLabel1" runat="server" SplittingType='<%# Eval("TradeType")%>' /> 
                <b>￥ <%# Eval("TradeType").ToInt() == (int)Hidistro.Entities.Members.SplittingTypes.DrawRequest ? "-" + Eval("Expenses", "{0:F2}") : Eval("Income", "{0:F2}")%></b></span>
        </div>
        <span class="glyphicon glyphicon-chevron-right right"></span>
    </div>
</a>