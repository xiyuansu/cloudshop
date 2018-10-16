<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.SendMessageTemplets" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <div class="search clearfix searcha mb_20">
                <ul>
                    <li class="mr_20">电子邮件
                        <a href="EmailSettings">配置</a></li>
                    <li class="mr_20">手机短信
                        <a href="SMSSettings">配置</a></li>
                    <li class="mr_20">微信
                        <a href="WeiXinTemplatesSetting">配置</a>
                    </li>
                </ul>
            </div>

            <asp:Repeater ID="grdEmailTempletsNew" runat="server">
                <HeaderTemplate>
                    <table cellspacing="0" border="0" id="grdEmailTempletsNew" class="table table-striped">
                        <tbody>
                            <tr>
                                <th>消息类型</th>
                                <th>电子邮件</th>
                                <th>站内消息</th>
                                <th>手机短信</th>
                                <th>微信</th>
                            </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hfMessageType" runat="server" Value='<%#Eval("MessageType") %>' />
                            <%#Eval("Name") %></td>
                        <td>

                            <asp:CheckBox runat="server" ID="chkSendEmail" class="icheck" Checked='<%# Eval("SendEmail")%>' MsgType='<%# Eval("Name")%>' /><span class="submit_bianji float" msgtype='<%# Eval("Name")%>'><a href='<%# "EditEmailTemplet.aspx?MessageType="+Eval("MessageType")%>'>编辑</a></span>
                        </td>
                        <td>

                            <asp:CheckBox runat="server" ID="chkInnerMessage" class="icheck" Checked='<%# Eval("SendInnerMessage")%>' MsgType='<%# Eval("Name")%>' /><span class="submit_bianji float" msgtype='<%# Eval("Name")%>'><a href='<%# "EditInnerMessageTemplet.aspx?MessageType="+Eval("MessageType")%>'>编辑</a></span>
                        </td>
                        <td>

                            <asp:CheckBox runat="server" ID="chkCellPhoneMessage" class="icheck" Checked='<%# Eval("SendSMS")%>' MsgType='<%# Eval("Name")%>' /><span class="submit_bianji" msgtype='<%# Eval("Name")%>'><a href='<%# "EditCellPhoneMessageTemplet.aspx?MessageType="+Eval("MessageType")%>'>编辑</a></span>
                        </td>
                        <td>
                            <div <%# Eval("WeiXinTemplateNo").ToNullString().Trim()==""?"style=\"display:none\"": ""%>>
                                <asp:CheckBox runat="server" ID="chkWeixinMessage" class="icheck" MessageType='<%# Eval("Name")%>' Checked='<%# Eval("SendWeixin")%>' />
                                <span class="submit_bianji"><a href='<%# "WeiXinTemplatesSetting#"+Eval("MessageType")%>'>编辑</a></span>
                            </div>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></tbody></table></FooterTemplate>
            </asp:Repeater>

            <div class="Pg_15 Pg_010" style="text-align: center;">
                <asp:Button ID="btnSaveSendSetting" runat="server" OnClientClick="return PageIsValid();" Text="保存设置" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <script>
        $(function () {

            $("#grdEmailTempletsNew tr:gt(0)").each(function () {
                var text = $.trim($("td:first", this).text());
                if ($("#ctl00_contentHolder_hfOpenMultStore").val() == "false" && text.indexOf("自提订单确认时") >= 0) {
                    $(this).remove();
                }
                if ($("#hidOpenReferral").val() == "false" && (text.indexOf("会员发展成功") >= 0 || text.indexOf("佣金获取提醒") >= 0)) {
                    $(this).remove();
                }
            });
        });

    </script>
    <asp:HiddenField ID="hfOpenMultStore" runat="server" />
   <asp:HiddenField ID="hidOpenReferral" runat="server" clientidmode="Static" />
</asp:Content>

