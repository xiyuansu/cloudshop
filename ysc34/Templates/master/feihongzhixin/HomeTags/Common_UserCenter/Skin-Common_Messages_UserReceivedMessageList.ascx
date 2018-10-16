<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
        
  <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1 top_border">
      <tr id="spqingdan_title">
        <td width="7%" align="center">选择</td>
        <td width="11%" align="center">状态</td>
        <td width="37%" align="center">标题</td>
        <td width="14%" align="center">发件人</td>
        <td width="16%" align="center">时间</td>
        <td width="15%" align="center">操作</td>
      </tr>     
        <asp:Repeater ID="repeaterMessageList" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="center">
                       <input type="checkbox" name="CheckBoxGroup" value='<%# Eval("MessageId") %>'/>
                    </td>
                    <td align="center">
                       <%#Convert.ToInt32(Eval("IsRead")) == 0 ? "<image src=\'/Templates/master/default/images/users/shouyj_03.jpg\' style=\'width:16px;height:15px;\'  />" : "<image src=\'/Templates/master/default/images/users/shouyj_06.jpg\' style=\'width:16px;height:15px;\' />"%>
                        </td>
                    <td align="center" style=" line-height: 25px; padding: 0px 10px;">
                        <%# Eval("Title")%>
                    </td>
                    <td align="center">
                        管理员
                    </td>
                    <td align="center">
                        <Hi:FormatedTimeLabel ID="litDateTime" Time='<%# Eval("Date")%>' runat="server" />
                    </td>
                    <td align="center">
                        <a href="javascript:void(0);"onclick="javascript:ReplayMessages(<%# Eval("MessageId")%>)" class="SmallCommonTextButton">查看回复</a>         
                        <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("MessageId") %>' OnClientClick="return window.confirm('确认要删除吗！')" CommandName="Delete" Text="删除"></asp:LinkButton>
                    </td>
                </tr> 
            </ItemTemplate>
</asp:Repeater>
</table>
<script type="text/javascript">
    function ReplayMessages(mid) {
        DialogFrame("ReplyReceivedMessage.aspx?MessageId=" + mid, "查看站内消息", 508, 620);
    }
</script>