<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.VServerConfig" CodeBehind="VServerConfig.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<%@ Register Src="~/Admin/Ascx/AccountNumbersTextBox.ascx" TagPrefix="uc1" TagName="AccountNumbersTextBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li style="border-bottom: 1px #ededed solid;">
                        <h2 class="colorE" style="display: block; float: left;">基本通讯配置 </h2>
                        <a href="http://download.92hi1.com/bangzhuzhongxin/changjianwenti/微信公众号及菜单配置.pdf" target="_blank" style="display: block;float: right;margin-right: 100px;margin-top: 11px;">帮助文档</a>
                    </li>
                    <li class="clearfix"><span class="formitemtitle">URL：</span>
                        <abbr class="formselect">
                            <asp:Literal runat="server" ID="txtUrl"></asp:Literal>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle">Token：</span>
                        <asp:Literal runat="server" ID="txtToken"></asp:Literal>
                    </li>
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        请将URL和Token配置到<a href="http://mp.weixin.qq.com" target="_blank" class="colorBlue">公众平台</a>下。
                    </li>
                    <div id="ReferralConfig" runat="server">
                        <li>
                            <h2 class="colorE">分销员设置</h2>
                        </li>
                        <li class="clearfix"><span class="formitemtitle">是否显示分销员：</span>
                            <abbr class="formselect">
                                <Hi:OnOff runat="server" ID="ooShowReferral"></Hi:OnOff>
                            </abbr>
                        </li>

                    </div>
                    <li>
                        <h2 class="colorE">自定义菜单权限配置</h2>
                        <%--<span>如果您开通了自定义菜单，请将<a target="_blank" href="http://mp.weixin.qq.com">微信公众平台</a>下的AppId与AppSecret配置在下方。</span>--%>
                    </li>
                    <li class="clearfix"><span class="formitemtitle">AppId：</span>
                        <uc1:AccountNumbersTextBox runat="server" id="txtAppId" CssClass="form_input_l form-control " />
                    </li>
                    <li class="clearfix"><span class="formitemtitle">AppSecret：</span>
                        <uc1:AccountNumbersTextBox runat="server" id="txtAppSecret" CssClass="form_input_l form-control " />

                    </li>

                    <li>
                        <h2 class="colorE">登录接口配置</h2>
                    </li>
                    <li class="clearfix"><span class="formitemtitle">微信官方登录接口：</span>
                        <Hi:OnOff runat="server" ID="ooIsValidationService"></Hi:OnOff>
                        <%--                        <input type="checkbox" id="chkIsValidationService" runat="server" /><label for="ctl00_contentHolder_chkIsValidationService">确认使用</label>--%><span class="ml_10 c-666">(仅认证服务号可用）</span>
                    </li>
                    <li style="display: none;" class="clearfix"><span class="formitemtitle">微信二维码：</span>
                        <asp:FileUpload ID="fileUpload" CssClass="forminput form-control" runat="server" />
                        <asp:Button ID="btnUpoad" runat="server" Text="上传" CssClass="submit_queding" Style="margin-left: 5px;" />
                        <div class="Pa_128 Pg_8 clear">
                            <table width="300" border="0" cellspacing="0">
                                <tr>
                                    <td width="80">
                                        <Hi:HiImage runat="server" ID="imgPic" Width="100px" CssClass="Img100_60" /></td>
                                    <td width="80" align="left">
                                        <Hi:ImageLinkButton ID="btnPicDelete" runat="server" IsShow="true" Text="删除" /></td>
                                </tr>
                                <tr>
                                    <td width="160" colspan="2"></td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li style="display: none;" class="clearfix"><span class="formitemtitle">微信账号：</span>
                        <asp:TextBox ID="txtWeixinNumber" CssClass="forminput form-control formwidth" runat="server" />
                        <p>请在微信公众平台-设置-账号信息下获取“微信号”</p>
                    </li>
                    <li class="clearfix" style="display: none;"><span class="formitemtitle">微信通用登录接口：</span>
                        <asp:TextBox ID="txtWeixinLoginUrl" CssClass="forminput form-control formwidth" runat="server" />
                    </li>
                    <li>
                        <h2 class="colorE">多客服配置</h2>
                    </li>
                    <li class="clearfix"><span class="formitemtitle">是否开通多客服：</span>
                        <Hi:OnOff runat="server" ID="ooManyService"></Hi:OnOff>
                        <%--                        <input type="checkbox" id="chk_manyService" runat="server" /><label for="ctl00_contentHolder_chk_manyService">已开通</label>--%><span class="ml_10 c-666">(仅认证服务号可在公众平台开通多客服）</span>
                    </li>
                    <li>
                        <h2 class="colorE">引导关注公众账号</h2>
                    </li>
                     <li class="clearfix"><span class="formitemtitle">强制关注公众号：</span>
                        <Hi:OnOff runat="server" ID="ooIsForceAttention"></Hi:OnOff>
                        <span runat="server" id="hspanIsForceAttention" class="c-666"></span>
                        <span class="ml_10 c-666" style="margin-left: 250px;">开启后，必须关注公众号才能进入店铺.</span>
                    </li>
                    <li class="clearfix" id="liGuiderAttention"><span class="formitemtitle">引导关注公众账号：</span>
                        <Hi:OnOff runat="server" ID="ooWeixinGuideAttention"></Hi:OnOff>
                        <span runat="server" id="hspanWeixinGuideAttention" class="c-666"></span>
                        <span class="ml_10 c-666" style="margin-left: 250px;">开启后，会员首次进入商城可通过浮动的关注图标进入二维码引导关注页面，来关注您的微信公众账号.</span>
                    </li>
                </ul>
                <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClientClick="return PageIsValid();" OnClick="btnOK_Click" CssClass="btn btn-primary ml_198" />
            </div>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtSiteName', 1, 60, false, null, '商城名称为必填项，长度限制在60字符以内'));
        }
        $(document).ready(function () {
            InitValidators();
        });
        function fuCheckIsForceAttention(event, state) {
            if (state) {
                $("#liGuiderAttention").hide();
            }
            else {
                $("#liGuiderAttention").show();
            }
        }
    </script>
</asp:Content>

