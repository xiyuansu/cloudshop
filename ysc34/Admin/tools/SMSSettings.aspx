<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SMSSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SMSSettings" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="blank12 clearfix">
    </div>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="sendmessagetemplets.aspx">消息提醒</a></li>
                <li class="hover"><a href="javascript:void">短信设置</a></li>


            </ul>

        </div>

        <div class="datafrom">
            <div class="formitem">
                <ul id="pluginContainer" class="attributeContent2">
                    <li>
                        <asp:Label ID="lbNum" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label>
                    </li>
                    <li><span class="formitemtitle ">发送方式：</span>
                        <span class="formselect">
                            <select id="ddlSms" name="ddlSms" class="iselect_one"></select>
                        </span>
                        <a href="https://dysms.console.aliyun.com/dysms.htm?spm=5176.2020520115.aliyun_topbar.133.5e5479d6w21Eii#/overview" target="_blank">点击此处</a>登录后（没有账号先注册）获取短信接口的AccessKeyID和AccessKeySecret
                    </li>
                    <li rowtype="attributeTemplate" style="display: none;"><span class="formitemtitle ">$Name$：</span>
                        $Input$
                    </li>
                </ul>
                <div class="ml_198 mt_0">
                    <asp:Button ID="btnSaveSMSSettings" runat="server" OnClientClick="return Save();" Text="保 存" CssClass="btn btn-primary float" />
                </div>
            </div>
            <div class="formitem" style="margin-top: 30px;">
                <ul>
                    <li><span class="formitemtitle ">接收手机号：</span>
                        <asp:TextBox ID="txtTestCellPhone" runat="server" CssClass="forminput form-control"></asp:TextBox>
                    </li>

                    <li><span class="formitemtitle ">短信模板Code：</span>
                        <asp:TextBox ID="txtTestSMSTemplateCode" runat="server" CssClass="forminput form-control" Text="SMS_139390116" ReadOnly="true"></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle ">发送内容：</span>
                        <asp:TextBox ID="txtTestSubject" runat="server" CssClass="forminput form-control" Text='{"code":"100"}' ReadOnly="true"></asp:TextBox>
                    </li>
                </ul>
                <div class="ml_198 mt_0">
                    <asp:Button ID="btnTestSend" runat="server" OnClientClick="return TestCheck();" Text="测试发送" CssClass="btn btn-primary inbnt"></asp:Button>
                </div>
            </div>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <asp:HiddenField runat="server" ID="txtSelectedName" />
    <asp:HiddenField runat="server" ID="txtConfigData" />
    <script type="text/javascript" src="/utility/plugin.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            pluginContainer = $("#pluginContainer");
            templateRow = $(pluginContainer).find("[rowType=attributeTemplate]");
            dropPlugins = $("#ddlSms");
            selectedNameCtl = $("#<%=txtSelectedName.ClientID %>");
            configDataCtl = $("#<%=txtConfigData.ClientID %>");

            // 绑定短信类型列表
            $(dropPlugins).append($("<option value=\"\">-请选择发送方式-</option>"));
            $.ajax({
                url: "/PluginHandler?type=SMSSender&action=getlist",
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
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus);
                }
            });

            $(dropPlugins).bind("change", function() { SelectPlugin("SMSSender"); });
            //$(dropPlugins).attr("disabled", "disabled");

            if ($(selectedNameCtl).val().length > 0) {
                SelectPlugin("SMSSender");
            }
        });

        function TestCheck() {
            if ($(dropPlugins).val() == "") {
                alert("请先选择发送方式并填写配置信息");
                return false;
            }
            if ($("#Appkey").val().length == 0) {
                alert("AccessKeyID必须填");
                return false;
            }
            if ($("#Appsecret").val().length == 0) {
                alert("AccessKeySecret必须填");
                return false;
            }
            if ($("#ctl00_contentHolder_txtTestCellPhone").val().length == 0) {
                alert("请输入接收手机号码");
                return false;
            }
            if ($("#ctl00_contentHolder_txtTestSubject").val().length == 0) {
                alert("请输入发送内容");
                return false;
            }
            $(dropPlugins).removeAttr("disabled");
            return true;
        }

        function Save() {
            //if ($("#Appkey").val().length == 0) {
            //    alert("Appkey必须填");
            //    return false;
            //}
            //if ($("#Appsecret").val().length == 0) {
            //    alert("Appsecret必须填");
            //    return false;
            //}
            $(dropPlugins).removeAttr("disabled");
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
