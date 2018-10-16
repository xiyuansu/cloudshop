<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
        <tr id="spqingdan_title">
            <td align="center">
                用户名
            </td>
            <td align="center">
                姓名
            </td>
            <td align="center">
                联系电话
            </td>
            <td align="center">
                订单量
            </td>
            <td align="center">
                注册日期
            </td>
        </tr>
        <asp:Repeater ID="rptSubMembers" runat="server">
        <ItemTemplate>
            <tr>
            <td style="display:none">
                <asp:Literal runat="server" ID="litUserId" Text='<%# Eval("UserId") %>'></asp:Literal>
            </td>
                <td align="center">
                    <%# Eval("UserName")%>&nbsp;
                </td>
                <td align="center">
                    <%#Eval("RealName") %>&nbsp;
                </td>
                <td align="center">
                   <%#Eval("CellPhone")%>&nbsp;
                </td>
                <td align="center">
                    <%#Eval("OrderNumber")%>&nbsp;
                </td>
                <td align="center">
                    <%#Eval("CreateDate")%>&nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
