<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AliOHServerConfig.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AliOH.AliOHServerConfig" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                    <li>
                        <h2 class="colorE">基本配置</h2>
                    </li>
                    <li class="clearfix"><span class="formitemtitle">APPID：</span>
                        <asp:TextBox ID="txtAppId" CssClass="forminput form-control" runat="server" placeholder="请填写APPID" /><a class="colorBlue ml_10" href="https://fuwu.alipay.com/" target="_blank">如何获取？</a>
                    </li>
                    <%--<li class="clearfix"><span class="formitemtitle">欢迎语标题：</span>
                        <asp:TextBox ID="txtAppWelcomeTitle" CssClass="forminput form-control" runat="server" />
                    </li>--%>
                    <li class="clearfix"><span class="formitemtitle">欢迎语：</span>
                        <asp:TextBox ID="txtAppWelcome" CssClass="forminput form-control" placeholder="请填写欢迎语,欢迎语在关注服务号时出现" runat="server" />
                    </li>
                    <li>
                        <h2 class="colorE">请将下面信息配置到生活号（原支付宝服务窗）<a  class="colorBlue" target="_blank" href="http://help.alipay.com/support/help_detail.htm?help_id=430989&sh=Y&tab=null&info_type=9">如何配置？</a></h2>
                    </li>
                    <li class="clearfix"><span class="formitemtitle">开发者网关：</span>
                        <abbr class="formselect">
                            <asp:Literal runat="server" ID="txtUrl"></asp:Literal>
                        </abbr>
                    </li>
                    <li><span class="formitemtitle">开发者公钥：</span>
                        <asp:TextBox ID="txtPubKey" ClientIDMode="Static" placeholder="开发者公钥请在支付宝官网获取" CssClass="form-control"  runat="server" Enabled="false"  Width="450" Height="140" TextMode="MultiLine"></asp:TextBox>
                    </li>
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClientClick="return PageIsValid();" OnClick="btnOK_Click" CssClass="btn btn-primary inbnt" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <script src="../../Utility/ZeroClipboard.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        if (window.clipboardData) {
            $("#copyPubKey").click(function () {
                window.clipboardData.setData('text', $('#txtPubKey').val());
                $(this).val("复制成功").attr('disabled', true);
            })
        }
        else {
            var client = new ZeroClipboard($("#copyPubKey"));
            var text = $('#txtPubKey').val();
            client.setText(text);
            $("#copyPubKey").click(function () {
                $(this).val("复制成功").attr('disabled', true);;
            })
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtSiteName', 1, 60, false, null, '商城名称为必填项，长度限制在60字符以内'));
        }
        $(document).ready(function () { InitValidators(); });


    </script>
</asp:Content>
