<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MeiQiaOnlineService.aspx.cs" Inherits="Hidistro.UI.Web.Admin.MeiQiaOnlineService" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="OnlineService.aspx">QQ/阿里旺旺</a></li>
                <li class="hover"><a href="javascript:void">美洽客服</a></li>
            </ul>

        </div>

        <div class="datalist clearfix">
            <div class="datafrom" style="padding: 0 !important;">
                <div class="formitem validator1" style="padding: 0 !important;">
                    <div style="width: 100%; float: left;">
                        <ul>

                            <li style="float: left; width: 100%; "><span class="formitemtitle">开启美洽客服：</span>
                                <div class="input-group">
                                    <Hi:OnOff runat="server" ID="ooMeiQiaActivated" ClientIDMode="Static"></Hi:OnOff>
                                </div>
                            </li>

                            <li><span class="formitemtitle" id="meiQiaUnitSpan"><big style="color: red;">*</big>请填写您的企业 ID：
                            </span>
                                <div class="">
                                    <asp:TextBox ID="txtmeiQiaUnitId" CssClass="form-control forminput " ClientIDMode="Static" runat="server" Width="304"></asp:TextBox>
                                </div>

                                <p id="txtmeiQiaUnitIdTip">
                                    <a href="https://app.meiqia.com/signup" target="_blank" class="colorBlue">注册美洽客服账号</a>
                                    后，
                                <a href="https://app.meiqia.com/login"  class="colorBlue" target="_blank">登录账号</a>，在【设置】—
                                <a href="https://app.meiqia.com/setting/id-query"  class="colorBlue" target="_blank">【ID查询】</a>中，获取到企业ID，填写至此处。
                                </p>
                            </li>
                            <li style="float: left; width: 100%; "><span class="formitemtitle" id="meiQiaDownloadSpan">客户端下载：</span>

                                <div class="btn-group" >
                                    <button type="button" class="btn btn-default" onclick="window.open('https://meiqia-cdn.b0.upaiyun.com/meiqia_for_windows.zip')">Windows版</button>
                                    <button type="button" class="btn btn-default" onclick="window.open('https://itunes.apple.com/cn/app/mei-qia-yi-dong-zai-xian-ke-fu/id1050591118')">IPhone版</button>
                                    <button type="button" class="btn btn-default" onclick="window.open('https://meiqia-cdn.b0.upaiyun.com/meiqia_for_android.apk')">Android版</button>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div class="ml_198">
                        <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="btn btn-primary inbnt" OnClientClick="return checkSubmit()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        $(document).ready(function (e) {
            initValid(new InputValidator('txtmeiQiaUnitId', 1, 12, false, '(^[0-9]*$)', '美洽企业ID不能为空，且必须为不超过12位的数字'));
            $('input[name="ctl00$contentHolder$ctl00"]').on('switchChange.bootstrapSwitch', function (event, state) {

                if (state) {
                    initValid(new InputValidator('txtmeiQiaUnitId', 1, 12, false, '(^[0-9]*$)', '美洽企业ID不能为空，且必须为不超过12位的数字'));
                } else {
                    initValid(new InputValidator('txtmeiQiaUnitId', 1, 12, true, '(^[0-9]*$)', '美洽企业ID必须为不超过12位的数字'));
                }
            });
            if ($('input[name="ctl00$contentHolder$ctl00"]').bootstrapSwitch('state') == false) {
                initValid(new InputValidator('txtmeiQiaUnitId', 1, 50, true, '(^[0-9]*$)', '美洽企业ID必须为数字'));
            }

        });
        function checkSubmit() {
            if (!PageIsValid())
                return false;

            return true;
        }
    </script>
</asp:Content>
