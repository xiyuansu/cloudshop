<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EmailSettings" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="blank12 clearfix"></div>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="sendmessagetemplets.aspx">消息提醒</a></li>
                <li class="hover"><a href="javascript:void">邮件设置</a></li>
            </ul>
        </div>

        <div class="datafrom">
            <div class="formitem">
                <blockquote class="blockquote-default blockquote-tip">
                    选择ASP.NET邮件组件方式进行正确设置，用以开通自动向用户发送如注册、订单付款等邮件<br />
                    如果您需要经常面向大量用户邮箱进行邮件群发，建议你开通更高品质的<font style="color: Red">EDM邮件营销服务。</font><a href="http://www.huz.com.cn/service/edm/" target="_blank">点此开通</a>
                </blockquote>
                 <ul id="pluginContainer" style="margin: 10px 0px;">
                    <li><span class="formitemtitle ">发送方式：</span>
                        <span class="formselect">
                            <select id="ddlEmails" name="ddlEmails" class="iselect_one"></select></span>
                    </li>
                    <li rowtype="attributeTemplate" style="display: none;"><span class="formitemtitle ">$Name$：</span>
                        $Input$
                    </li>
                </ul>
                <div class="ml_198 mt0">
                    <asp:Button ID="btnChangeEmailSettings" runat="server" Text="保 存" CssClass="btn btn-primary"></asp:Button>
                </div>
            </div>
            <div class="formitem">
                <ul>
                    <li  style="margin-top: 30px;"><span class="formitemtitle ">测试邮箱：</span>
                        <asp:TextBox runat="server" ID="txtTestEmail" CssClass="forminput form-control" />
                    </li>
                </ul>

                <div class="ml_198 mt0">
                    <asp:Button ID="btnTestEmailSettings" runat="server" OnClientClick="return TestCheck();" Text="发送测试邮件" CssClass="btn btn-primary inbnt"></asp:Button>
                </div>
            </div>
            </div>
       <%--     <div class="ml_198">
                <asp:Button ID="btnChangeEmailSettings" runat="server" Text="保存" CssClass="btn btn-primary"></asp:Button>
                <asp:Button ID="btnTestEmailSettings" runat="server" OnClientClick="return TestCheck();" Text="发送测试邮件" CssClass="btn btn-primary ml_20"></asp:Button>
            </div>--%>
        </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <asp:HiddenField runat="server" ID="txtSelectedName" />
    <asp:HiddenField runat="server" ID="txtConfigData" />
    <script type="text/javascript" src="/utility/plugin.js" ></script>
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            pluginContainer = $("#pluginContainer");
            templateRow = $(pluginContainer).find("[rowType=attributeTemplate]");
            dropPlugins = $("#ddlEmails");
            selectedNameCtl = $("#<%=txtSelectedName.ClientID %>");
            configDataCtl = $("#<%=txtConfigData.ClientID %>");

            // 绑定邮件类型列表
            $(dropPlugins).append($("<option value=\"\">请选择发送方式</option>"));
            $.ajax({
                url: "/PluginHandler?type=EmailSender&action=getlist",
                type: 'GET',
                async: false,
                dataType: 'json',
                timeout: 10000,
                success: function (resultData) {
                    if (resultData.qty == 0)
                        return;

                    $.each(resultData.items, function (i, item) {
                        if (item.FullName == $(selectedNameCtl).val())
                            $(dropPlugins).append($(String.format("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.FullName, item.DisplayName)));
                        else
                            $(dropPlugins).append($(String.format("<option value=\"{0}\">{1}</option>", item.FullName, item.DisplayName)));
                    });
                }
            });

            $(dropPlugins).bind("change", function () {
                SelectPlugin("EmailSender");
            });

            if ($(selectedNameCtl).val().length > 0) {
                SelectPlugin("EmailSender");
            }
        });

        function TestCheck() {
            if ($(dropPlugins).val() == "") {
                alert("请先选择发送方式并填写配置信息");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
