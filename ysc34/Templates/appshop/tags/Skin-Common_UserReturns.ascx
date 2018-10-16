<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a href='UserReturnDetail.aspx?ReturnsId=<%# Eval("ReturnsId") %>'>
    <div class="re">
        <div class="u_re">
            <font style="float: left; text-align: left">编号：<%# Eval("OrderId") %></font>
            <font style="float: right; text-align: right;"><%# Eval("ApplyForTime") %></font>
        </div>
        <ul class="u_ul">
            <li>
                <span style="color: #07b2f8"><%# ((Decimal)Eval("RefundAmount")).F2ToString("f2") %></span>
                <strong>退款金额</strong>
            </li>
            <li>
                <span style="color: #07b2f8"><%# Eval("RefundType") %></span>
                <strong>退款方式</strong>
            </li>
            <li>
                <span>
                    <Hi:ReturnStatusLable ID="lblHandleStatus" Status='<%# Eval("HandleStatus") %>' runat="server" /></span>
                <strong>状态</strong>
            </li>
        </ul>
    </div>
</a>


