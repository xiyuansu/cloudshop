<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RecruitPlanSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.RecruitPlanSettings" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />

    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>

    <script type="text/javascript" src="../js/zclip/jquery.zclip.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#btnCopy').zclip({
                path: "../js/zclip/ZeroClipboard.swf",
                copy: function () {
                    return $('#copyContent').text();
                },
                afterCopy: function () {
                    alert("成功复制到剪切板：" + $('#copyContent').text());
                }
            });
            $("#imgsrc").attr("src", "/api/VshopProcess.ashx?action=GenerateTwoDimensionalImage&url=" + $("#copyContent").text());


            if ($("#ctl00_contentHolder_OnOffzmxy input").is(':checked')) {
                $("#zmxyUL").show();
            }
            else {
                $("#zmxyUL").hide();
            }
        });

        function fuOnOffzmxy(event, state) {
            if (state) {
                $("#zmxyUL").show();
            }
            else {
                $("#zmxyUL").hide();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="DeductSettings.aspx">分佣规则设置</a></li>
                <li><a href="ReferralSettings.aspx">分销员申请设置</a></li>
                <li><a href="ReferralPosterSet.aspx">分销海报设置</a></li>
                <li><a href="ExtendShareSettings.aspx">分销分享设置</a></li>
                <li class="hover"><a href="javascript:void">招募计划设置</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li ><span class="formitemtitle">分销员招募介绍：</span>
                        <span style="display: block; float: left; overflow: hidden;">
                            <Hi:Ueditor ID="fckReferralIntroduction" runat="server" Width="660" /></span>
                        <%--<p>用户在申请成为分销员时能看到这些介绍。</p>--%>
                    </li>
                </ul>

                <ul>
                    <li class="clearfix"><span class="formitemtitle">是否开启招募协议：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="OnOffzmxy"></Hi:OnOff>
                        </abbr>
                        <span>&nbsp;开启后分销员申请页面将显示下面设置的招募协议,并且需要用户同意才可申请!</span>
                    </li>
                </ul>
                <ul id="zmxyUL">
                    <li class="mb_0"><span class="formitemtitle">分销员招募协议：</span>
                        <span style="display: block; float: left; overflow: hidden;">
                            <Hi:Ueditor ID="fckRecruitmentAgreement" runat="server" Width="660" ImportLib="1" /></span>
                        <%--<p>用户在申请成为分销员时能看到这些介绍。</p>--%>
                    </li>
                </ul>
                <ul>
                    <li class="content">
                        <br />
                        <span class="formitemtitle">分销员招募链接：</span>
                        <div class="input-group">
                            <span id="copyContent">
                                <asp:Literal ID="ltRecruitPlanUrl" runat="server"></asp:Literal></span>
                            <input type="button" class="btn btn-default" value="复制" id="btnCopy" />
                        </div>
                    </li>
                </ul>
                <ul>
                    <li class="content">
                        <span class="formitemtitle">二维码：</span>
                        <div class="input-group">
                            <img id="imgsrc" width="100" height="100" style="margin-left: 20px;" />
                        </div>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="btn btn-primary" OnClientClick="return PageIsValid();" />
                </div>
            </div>
        </div>
    </div>

      <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>
