<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
<tr id="spqingdan_title">
    <td width="11%" align="center" style="border-right:1px solid #ddd !important;">流水号</td>
    <td width="19%" align="center">时间</td>
    <td width="12%" align="center">类型</td>
    <td width="10%" align="center">收入</td>
    <td width="11%" align="center">支出</td>
    <td width="10%" align="center">账户余额</td>
     <td width="27%" align="center">备注</td>
  </tr>
<asp:Repeater ID="repeaterAccountDetails" runat="server" >
    <ItemTemplate>
    <tr class="border_r_b">
        <td width="11%" align="center">
            <asp:Literal ID="litJournalNumber" runat="server" Text='<%#Eval("JournalNumber") %>'></asp:Literal></td>
        <td width="19%" align="center">
            <Hi:FormatedTimeLabel ID="lblTradeDate" runat="server" Time='<%# Eval("TradeDate") %>'></Hi:FormatedTimeLabel></td>
        <td width="12%" align="center">
            <Hi:TradeTypeNameLabel ID="lblTradeType" runat="server" TradeType="TradeType"></Hi:TradeTypeNameLabel></td>
        <td width="10%" align="center">
            <Hi:FormatedMoneyLabel ID="lblIncome" runat="server" Money='<%# Eval("Income") %>'></Hi:FormatedMoneyLabel></td>
        <td width="11%" align="center">
            <Hi:FormatedMoneyLabel ID="lblExpenses" runat="server" Money='<%# Eval("Expenses") %>'></Hi:FormatedMoneyLabel></td>
        <td width="10%" align="center">
            <Hi:FormatedMoneyLabel ID="lblBalance" runat="server" Money='<%# Eval("Balance") %>'></Hi:FormatedMoneyLabel></td>
        <td width="27%" align="center"><asp:Literal ID="litRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Literal></td>
    </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
