<%@ Page Title="" Language="C#" MasterPageFile="~/Applet/Applet.Master" AutoEventWireup="true" CodeBehind="SplittinRule.aspx.cs" Inherits="Hidistro.UI.Web.Applet.SplittinRule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="hdAppId" runat="server" clientidmode="Static" />
    <input type="hidden" id="hdTimestamp" runat="server" clientidmode="Static" />
    <input type="hidden" id="hdNonceStr" runat="server" clientidmode="Static" />
    <input type="hidden" id="hdSignature" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidExtendShareTitle" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidExtendShareDetail" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidExtendSharePic" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidExtendShareUrl" runat="server" clientidmode="Static" />
<div id="posterPanel" runat="server">
    <asp:Image ID="userReferralQRCode" runat="server" ImageUrl="" width="100%" ClientIDMode="Static" />
    <div class="zhezhao">
        <img src="../images/new/Splittin_pic.png" id="imghtmlfxvshop" runat="server">
        <img src="../images/new/SplittinWap_pic.png" id="imghtmlfxwapshop" runat="server">
    </div>
    <script type="text/javascript">
        String.prototype.replaceAll = function (str, tostr) {
            oStr = this;
            while (oStr.indexOf(str) > -1) {
                oStr = oStr.replace(str, tostr);
            }
            return oStr;
        }
        $(document).ready(function () {

            $("#btnshare").click(function (e) {
                var title = $("#hidExtendShareTitle").val();
                var content = $("#hidExtendShareDetail").val();
                var imgUrl = $("#hidExtendSharePic").val();
                var shareUrl = $("#hidExtendShareUrl").val();
                if (content == "") { content = title; }
                shareJson = ("{\"title\":\"" + title + "\",\"content\":\"" + content + "\",\"image\":\"" + imgUrl + "\",\"url\":\"" + shareUrl + "\"}").replaceAll("\\", "").replaceAll("\n\r", "").replaceAll("\n", "").replaceAll("\r", "").replaceAll(" ", "");
                mui.fire(plus.webview.getWebviewById("appauto"), "ShareRefress", { 'shareparem': shareJson });
            });
        })
        ///跳转到分销员协议页面
        function goReferralAgreement() {
            wx.miniProgram.navigateTo("/pages/applicationpromotion/applicationpromotion");//没有登录则跳转登录页面
        }
        ///跳转到首页
        function goHomePage() {
            wx.miniProgram.navigateTo("/pages/home/home");//没有登录则跳转登录页面
        }

    </script>
    <div class="share_bottom"><div class="share_btn2"><a href="javascript:void(0)" id="btnshare">分享我的专属链接<i class="icon_yongjin_11 icon-icon_share2"></i></a></div></div>
</div>
<div id="noreferralPanel" runat="server" visible="false">
    <div class="notreferral">
        <div class="img"><img src="/Utility/pics/group-1.png" alt="" /></div>
        <div class="txt">您还不是本商城分销员噢~<br />申请成为分销员，分销有礼！</div>
        <div class="btns">
            <a href="javascript:void(0)" onclick="goReferralAgreement()" class="btn_login">了解分销员</a>
            <a onclick="goHomePage()" class="btn-default">返回商城首页</a>
        </div>
    </div>
</div>
<div id="repeledPanel" runat="server" visible="false" clientidmode="static">
    <div style="float:left;width:100%;padding:2.5rem;text-align:center;">
        <img src="/images/clear_mob.png" style="width:4.2rem; margin:0 auto;" />
    </div>
    <div class="t_1">
        您于 <asp:Literal id="repeledTime" runat="server"></asp:Literal>  被商城取消分销员资格。
    </div>
    <div id="divRepeledReason" runat="server" visible="false">
        <font>商家解释：</font>
        <span>
            <asp:Literal id="repeledReason" runat="server"></asp:Literal>
        </span>
    </div>
</div>
</asp:Content>
