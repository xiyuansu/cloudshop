
<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1 top_border">
    <tr id="spqingdan_title">
        <td width="7%" align="center">选择</td>
        <td width="37%" align="center">标题</td>
        <td width="14%" align="center">发件人</td>
        <td width="16%" align="center">时间</td>
        <td width="15%" align="center">操作</td>
    </tr>
    <asp:Repeater ID="messagesList" runat="server">
        <ItemTemplate>
            <tr>
                <td align="center">
                    <input type="checkbox" name="CheckBoxGroup" value='<%# Eval("MessageId") %>' />
                </td>

                <td align="center">
                    <%# Eval("Title")%>
                </td>
                <td align="center">管理员
                </td>
                <td align="center">
                    <Hi:FormatedTimeLabel ID="FormatedTimeLabel1" Time='<%# Eval("Date")%>' runat="server" />
                </td>
                <td align="center">
                    <input id="hidcontent<%# Eval("MessageId") %>" value='<%# Eval("Content") %>' type="hidden" />
                    <a id="btnEdit" onclick="javascript:SendMessage('<%# Eval("Title") %>',<%# Eval("MessageId") %>,'<%# Eval("Date","{0:d}") %>')">查看</a>
                    <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("MessageId") %>' CommandName="Delete" OnClientClick="return window.confirm('确认要删除吗！')"  Text="删除"></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<div id="message" style="display: none;">
    <div class="frame-content">
        <table width="477px">
            <tr>
                <td class="td_right">时间：</td>
                <td><span id="lbltime" /></td>
            </tr>
            <tr>
                <td class="td_right">标题：</td>
                <td><span id="lbltitle" /></td>
                <td></td>
            </tr>
            <tr>
                <td class="td_right">内容：</td>
                <td>
                    <textarea id="txtarea" readonly="readonly" style="color: #A0A0A0" name="txtarea" rows="10" cols="50">{content}</textarea></td>
            </tr>
        </table>
    </div>
</div>
<div id="tempmessage" style="display:none"></div>
<script type="text/javascript">
    function SendMessage(title, mid, timeformat) {
        arrytext = null;
        $("#lbltime").html(timeformat);
        $("#lbltitle").html(title);
        setArryText("txtarea", $("#hidcontent" + mid).val());
        $("#tempmessage").html($("#message").html().replace("{content}", $("#hidcontent" + mid).val()));
        ShowMessageDialog("查看发件箱", "messageContent", "tempmessage")
    }
</script>
