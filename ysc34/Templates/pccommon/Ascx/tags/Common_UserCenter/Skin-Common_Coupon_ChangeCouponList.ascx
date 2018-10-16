<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
  <tr id="spqingdan_title">
    <td width="25%" align="center">优惠券名称</td>
    <td width="15%" align="center">满足金额</td>
    <td width="15%" align="center">面值</td>
    <td width="15%" align="center">兑换所需积分</td>
    <td width="15%" align="center">有效期（止）</td>
    <td width="15%" align="center">操作</td>
  </tr>
<asp:Repeater ID="repeaterCoupon" runat="server" >
        <ItemTemplate>
            <tr>
                <td align="center">
                    <Hi:SubStringLabel ID="lblName" Field="Name" StrLength="12" StrReplace="..  " runat="server" /></td>
                <td align="center">
                    <Hi:FormatedMoneyLabel ID="lblAmount" Money='<%#Eval("Amount") %>' runat="server"></Hi:FormatedMoneyLabel></td>
                <td align="center">
                    <Hi:FormatedMoneyLabel ID="lblValue" Money='<%#Eval("DiscountValue") %>' runat="server"></Hi:FormatedMoneyLabel></td>
                <td align="center">
                    <asp:Literal ID="litNeedPoint" Text='<%#Eval("NeedPoint") %>' runat="server" /></td>
                <td align="center">
                    <Hi:FormatedTimeLabel ID="lblClosingTime" Time='<%#Eval("ClosingTime") %>' runat="server"></Hi:FormatedTimeLabel></td>
                <td align="center"><Hi:ImageLinkButton runat="server" ID="lbtnChage" IsShow="true" DeleteMsg="确定要兑换吗，兑换后扣除您的积分" Text="兑换"  CommandName="Change" CommandArgument='<%#Eval("CouponId") %>' /></td>
                
            </tr>
        </ItemTemplate>
</asp:Repeater>
</table>