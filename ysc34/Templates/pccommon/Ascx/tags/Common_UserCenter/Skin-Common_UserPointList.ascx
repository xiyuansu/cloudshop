<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
    <tr id="spqingdan_title">
        <td width="10%" align="center">流水号</td>
        <td width="17%" align="center">订单编号</td>
        <td width="20%" align="center">时间</td>
        <td width="14%" align="center">类型</td>
        <td width="12%" align="center">增加</td>
        <td width="11%" align="center">减少</td>
        <td width="16%" align="center">当前积分</td>
    </tr>
    <asp:Repeater ID="repeaterPointDetails" runat="server" >
         <ItemTemplate>
                  <tr>
                    <td align="center">
                        <asp:Label ID="lblJournalNumber" runat="server" Text='<%# Eval("JournalNumber") %>'></asp:Label>
                    </td>
                    <td align="center" >
                        <asp:Label ID="lblOrderId" runat="server" Text='<%# string.IsNullOrEmpty(Eval("OrderId").ToString())?"*":Eval("OrderId").ToString() %>'></asp:Label>
                    </td>
                    <td align="center" >
                        <Hi:FormatedTimeLabel ID="FormatedTimeLabel1" Time='<%#Eval("TradeDate") %>' runat="server" />
                    </td>
                    <td align="center" >
                        <asp:Label ID="lblPointType" runat="server" Text='<%#Eval("TradeType") %>'></asp:Label>
                    </td>
                    <td align="center" >
                        <asp:Label ID="increasedNumber" runat="server" Text='<%#Eval("Increased") %>'></asp:Label>
                    </td>
                    <td align="center" >
                        <asp:Label ID="reducedNumber" runat="server" Text='<%#Eval("Reduced") %>'></asp:Label>
                    </td>
                    <td align="center" class="rightborder">
                         <asp:Label ID="pointNumber" runat="server" Text='<%#Eval("Points") %>'></asp:Label>
                    </td>
                  </tr>
            </ItemTemplate>
    </asp:Repeater>
</table>