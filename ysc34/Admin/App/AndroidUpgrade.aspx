<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AndroidUpgrade.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AndroidUpgrade" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <style type="text/css">
        p.downloadurl span { float: left; font-size: 14px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //if ($("#ctl00_contentHolder_ooOpen input").is(':checked')) {
            //    $(".content").show();
            //}
            //else {
            //    $(".content").hide();
            //    //隐藏时不要加载复制功能，会导致无法获取控件位置而导致复制失败
            //}
        });

        function fuCheckEnableWXPay(event, state) {
            if (state) {
                $(".content").show();
            }
            else {
                $(".content").hide();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <%--         <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">安卓升级</a></li>
                <li><a href="IosUpgrade.aspx">ios升级</a></li>
            </ul>
        </div>--%>
        <div class="datafrom">
            <div class="formitem validator1">

                <ul class="content">
                    <li><span class="formitemtitle">版本号：</span>
                        <asp:Literal ID="litVersion" runat="server" Text="1.00" />
                        <input type="hidden" id="hidIsForcibleUpgrade" runat="server" />
                    </li>
                    <li><span class="formitemtitle">版本描述：</span>
                        <asp:Literal ID="litDescription" runat="server" Text="初始版本" />
                    </li>
                    <li><span class="formitemtitle">安卓APP：</span>
                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="input-file" />
                        <p runat="server" class="pl0">上传了升级包之后将会覆盖原来的版本，上传前请确认上传的更新包版本比当前版本新</p>
                        <p class="downloadurl" id="txtAndroidDownloadUrl" runat="server"></p>
                    </li>

                    <li><span class="formitemtitle"><em></em>苹果APP：</span>
                        <asp:TextBox ID="txtIosDownloadUrl" runat="server" CssClass="forminput form-control" />
                        <p runat="server">苹果版本下载地址</p>
                    </li>

                    <li><span class="formitemtitle"><em></em>上架审核使用API地址：</span>
                        <asp:TextBox ID="txtAppAuditAPIUrl" runat="server" CssClass="forminput form-control" />
                        <p runat="server">站点升级前上架新版本请填写</p>
                    </li>
                </ul>
                <ul>
                    <li>
                        <span class="formitemtitle">PC端是否显示下载二维码：</span>
                        <div class="input-group">
                            <Hi:OnOff runat="server" ID="ooOpen"></Hi:OnOff>
                        </div>
                        <div style="position: absolute; right: 169px; top: 54px;">
                            <a href="http://download.92hi1.com/bangzhuzhongxin/changjianwenti/APP上架到华为应用市场申请流程.pdf" target="_blank">华为应用市场上架指南</a>
                        </div>
                    </li>
                </ul>
                <ul>
                    <li><span class="formitemtitle">&nbsp;</span>
                        <asp:Button ID="btnUpoad" runat="server" Text="保   存" CssClass="btn btn-primary" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
