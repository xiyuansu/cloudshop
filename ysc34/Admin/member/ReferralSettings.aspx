<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReferralSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ReferralSettings" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="DeductSettings.aspx">分佣规则设置</a></li>
                <li class="hover"><a href="javascript:void">分销员申请设置</a></li>
                <li><a href="ReferralPosterSet.aspx">分销海报设置</a></li>
                <li><a href="ExtendShareSettings.aspx">分销分享设置</a></li>
                <li><a href="RecruitPlanSettings.aspx">招募计划设置</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li><span class="formitemtitle">注册会员即为分销员：</span>
                        <Hi:OnOff runat="server" ID="chkRegisterBecomePromoter"></Hi:OnOff>
                    </li>
                </ul>
                <ul id="ulSubmitInfo">
                    <li><span class="formitemtitle">申请需提交信息：</span>
                        <asp:CheckBox ID="chkName" runat="server" Text="真实姓名&nbsp;&nbsp;" CssClass="icheck" />
                        <asp:CheckBox ID="chkPhone" runat="server" Text="手机号&nbsp;&nbsp;" CssClass="icheck" onclick="onPhoneCheck($(this))" />
                        <asp:CheckBox ID="chkEmail" runat="server" Text="邮箱&nbsp;&nbsp;" CssClass="icheck" />
                        <asp:CheckBox ID="chkAddress" runat="server" Text="住址&nbsp;&nbsp;" CssClass="icheck" />
                    </li>
                </ul>
                <ul id="ulValidatePhone">
                    <li><span class="formitemtitle">是否验证手机：</span>
                        <Hi:OnOff runat="server" ID="radIsPromoterValidatePhone"></Hi:OnOff>
                        <span id="spanPhoneMsg">&nbsp;&nbsp;请确保已配置好短信服务</span>
                    </li>
                </ul>
                <ul id="ulBeComeCondition">
                    <li><span class="formitemtitle">分销员申请条件：</span>
                        <div class="input-group">
                            <asp:RadioButtonList runat="server" ID="radApplyCondition" ForeColor="Black" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="0" Selected="True">无条件</asp:ListItem>
                                <asp:ListItem Value="1">累计金额达到</asp:ListItem>
                            </asp:RadioButtonList>
                            &nbsp;<asp:TextBox ID="txtApplyReferralNeedAmount" CssClass="form-control" Width="100px" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <span class="input-group-addon">元</span>
                        </div>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="btn btn-primary" OnClientClick="return PageIsValid();" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidSMSEnabled" />
    <script type="text/javascript">
        $(function () {
            $("#txtApplyReferralNeedAmount").click(function (e) {
                $("input[name='ctl00$contentHolder$radApplyCondition']").eq(1).click();
            });
            $("#txtApplyReferralNeedAmount").focus(function (e) {
                $("input[name='ctl00$contentHolder$radApplyCondition']").eq(1).click();
            });
            if ($("#ctl00_contentHolder_chkRegisterBecomePromoter input").is(':checked')) {
                $("#ulSubmitInfo").hide();
                $("#ulValidatePhone").hide();
                $("#ulBeComeCondition").hide();
            }
            else {
                $("#ulSubmitInfo").show();
                $("#ulBeComeCondition").show();
                onPhoneCheck($("#ctl00_contentHolder_chkPhone"));
            }

            if ($("#ctl00_contentHolder_hidSMSEnabled").val() == "false") {
                $("#ctl00_contentHolder_ctl01").bootstrapSwitch('disabled', true);
                $("#spanPhoneMsg").show();
            }
            else {
                $("#spanPhoneMsg").hide();
            }
        });
        function onPhoneCheck(obj) {
            if (obj.checked || obj.is(':checked')) {
                $("#ulValidatePhone").show();
            }
            else {
                $("#ulValidatePhone").hide();
            }
        }
        function fuCheckEnableBecomePromoter(event, state) {
            if (state) {
                $("#ulSubmitInfo").hide();
                $("#ulValidatePhone").hide();
                $("#ulBeComeCondition").hide();
            }
            else {
                $("#ulSubmitInfo").show();
                $("#ulBeComeCondition").show();
                onPhoneCheck($("#ctl00_contentHolder_chkPhone"));
            }
        }

    </script>
</asp:Content>
