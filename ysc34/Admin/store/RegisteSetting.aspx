<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RegisteSetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.store.RegisteSetting" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#radOpenGeetest').on('ifChecked', function (event) {
                $('#liGeetestId').show();
                $('#liGeetestKey').show();
            });

            $('#radOpenImgCode').on('ifChecked', function (event) {
                $('#liGeetestId').hide();
                $('#liGeetestKey').hide();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <div class="areacolumn clearfix databody">
        <div class="areacolumn clearfix">
            <div class="columnright">
                <div class="formitem mt_10">
                    <ul>
                        <li>
                            <h2 class="clear">会员注册设置</h2>
                        </li>
                        <li><span class="formitemtitle"><em>*</em>会员注册账号需提交：</span>
                            <span style="margin-right: 10px;">
                                <asp:CheckBox ID="chkEmail" runat="server" Text="邮箱" class="icheck" /></span><span style="margin-right: 10px; color: #AEAEAE;">或</span>
                            <span style="margin-right: 20px;">
                                <asp:CheckBox ID="chkTel" runat="server" Text="手机号" class="icheck" /></span>
                            <span style="color: #AEAEAE;" id="spanphonesettingtip" runat="server" visible="false">手机注册需发送短信验证码,<a href="../tools/SMSSettings.aspx">去配置短信</a></span>
                        </li>

                        <li><span class="formitemtitle">是否验证邮箱：</span>
                            <span style="margin-right: 20px;">
                                <Hi:OnOff runat="server" ID="ooIsValidEmail"></Hi:OnOff>
                            </span>
                            <span style="color: #AEAEAE;" id="spanemailsettingtip" runat="server" visible="false">您还未配置邮箱,<a href="../tools/EmailSettings.aspx">去配置邮箱</a></span>
                        </li>
                        <li>
                            <span class="formitemtitle">验证码方式：</span>
                            <span style="margin-right: 30px;">
                                <asp:RadioButton runat="server" ID="radOpenImgCode" class="icheck" Checked="true" Text="图形验证" GroupName="checkcode" ClientIDMode="Static" />
                            </span>
                            <span>
                                <asp:RadioButton runat="server" ID="radOpenGeetest" class="icheck" Text="滑动验证" GroupName="checkcode" ClientIDMode="Static" /></span>
                            <span style="margin-left: 10px;"><a href="https://user.geetest.com/email/register" target="_blank">获取ID和Key</a></span>
                        </li>
                        <li id="liGeetestId" style="display: none;" runat="server" clientidmode="Static">
                            <span class="formitemtitle">ID：</span>
                            <span>
                                <asp:TextBox ID="txtGeetestId" runat="server" CssClass="form_input_m form-control"></asp:TextBox></span>
                        </li>
                        <li id="liGeetestKey" style="display: none;" runat="server" clientidmode="Static">
                            <span class="formitemtitle">Key：</span>
                            <span>
                                <asp:TextBox ID="txtGeetestKey" runat="server" CssClass="form_input_m form-control"></asp:TextBox></span>
                        </li>
                        <li>
                            <h2 class="clear">其他注册信息</h2>
                        </li>
                        <li>
                            <span class="formitemtitle">会员注册需提交：</span>
                            <span style="margin-right: 10px;">
                                <asp:CheckBox ID="chkRealName" runat="server" Text="真实姓名" class="icheck" /></span>
                            <span style="margin-right: 10px;">
                                <asp:CheckBox ID="chkBirthday" runat="server" Text="生日" class="icheck" /></span>
                            <span>
                                <asp:CheckBox ID="chkSex" runat="server" Text="性别" class="icheck" /></span>
                        </li>
                        <li>
                            <h2 class="clear">强制绑定手机号</h2>
                        </li>
                        <li><span class="formitemtitle">开启信任登录强制绑定：</span>
                            <span style="margin-right: 20px;">
                                <Hi:OnOff runat="server" ID="OnOffIsForceBindingMobbile"></Hi:OnOff>
                            </span>
                            <span style="color: #AEAEAE;" id="spancellphonesettingtip" runat="server">开启前请先进行短信相关配置,<a href="../tools/SMSSettings">去配置短信</a></span>
                        </li>
                        <li><span class="formitemtitle">&nbsp;</span>
                            <span style="margin-right: 20px;color: #AEAEAE;">
                                开启之后，信任登录帐号没有绑定手机号的会员在进行快捷登录、下单等操作时会强制要求绑定。
                            </span>
                        </li>
                        <li><span class="formitemtitle">开启用户密码登录强制绑定：</span>
                            <span style="margin-right: 20px;">
                                <Hi:OnOff runat="server" ID="OnOffUserLgoinIsForceBindingMobbile"></Hi:OnOff>
                            </span>
                            <span style="color: #AEAEAE;" id="spancellphonesettingtip1" runat="server">开启前请先进行短信相关配置,<a href="../tools/SMSSettings">去配置短信</a></span>
                        </li>
                        <li><span class="formitemtitle">&nbsp;</span>
                            <span style="margin-right: 20px;color: #AEAEAE;">
                                开启之后，用户密码帐号没有绑定手机号的会员在进行登录、下单等操作时会强制要求绑定。
                            </span>
                        </li>
                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnSave" runat="server" Text="保  存" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    </div>
                </div>

            </div>
        </div>


    </div>
</asp:Content>
