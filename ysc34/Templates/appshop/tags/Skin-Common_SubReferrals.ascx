<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Context" %>
<a href="SubReferral_Detail.aspx?userid=<%# Eval("UserId") %>">
    <div class="referral clearfix">
        <asp:Image ImageUrl='<%#(Eval("Picture")==DBNull.Value || string.IsNullOrEmpty(Eval("Picture").ToString()))?"../images/headerimg.png":Eval("Picture").ToString() %>' runat="server" CssClass="img-circle" />
        <div class="left">

            <div><%# Eval("UserName") %></div>
            <span class="describe">贡献奖励 <b>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("SubReferralSplittin") %>' NullToDisplay="0.00"  runat="server" /></b>
                直接分销订单量 <b><%#Eval("ReferralOrderNumber")%></b>
            </span>
        </div>
        <span class="glyphicon glyphicon-chevron-right right"></span>
    </div>
</a>
