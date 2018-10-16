<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditMemberLoginPassword.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditMemberLoginPassword" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">
    <script type="text/javascript">
        //$(function () {
        //    var menu_left = window.parent.document.getElementById("menu_left");
        //    var aReturnTitle = $(".curent", menu_left);
        //    if (aReturnTitle) {
        //        $("#aReturnTitle").text($(aReturnTitle).text());
        //        var href = "/admin/" + $(aReturnTitle).attr("href");
        //        $("#aReturnTitle").attr("href", href);
        //    }
        //})

        function ToList() {
            var returnUrl = getParam("returnurl");
            if (returnUrl != "") {
                location.href = decodeURIComponent(returnUrl);
            } else {
                window.history.back();
            }
        }
    </script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="javascript:ToList()" id="aReturnTitle">会员列表</a></li>
                <li><a href='<%="EditMember.aspx?userId="+Page.Request.QueryString["userId"]+"&returnUrl="+Page.Request.QueryString["returnUrl"].ToNullString() %>'>基本信息</a></li>
                <li class="hover"><a >登录密码</a></li>
                <li><a href='<%="EditMemberTransactionPassword.aspx?userId="+Page.Request.QueryString["userId"]+"&returnUrl="+Page.Request.QueryString["returnUrl"].ToNullString() %>'>交易密码</a></li>

            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">

            <div class="formitem">
                <ul>
                    <li>
                        <span class="formitemtitle">用户名：</span>
                        <asp:Literal runat="server" ID="litlUserName" />
                    </li>
                    <li><span class="formitemtitle"><em>*</em>新登录密码：</span>
                        <asp:TextBox runat="server" ID="txtNewPassWord" TextMode="Password" CssClass="forminput form-control" placeholder="长度在6-20个字符之间" />
                    </li>
                    <li><span class="formitemtitle"><em>*</em>重复输入一次：</span>
                        <asp:TextBox runat="server" ID="txtPassWordCompare" TextMode="Password" CssClass="forminput form-control" />
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnEditUser" runat="server" Text="确 定" OnClientClick="return PageIsValid();" CssClass="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtNewPassWord', 6, 20, false, null, '密码不能为空，长度在6-20个字符之间'));
            initValid(new CompareValidator('ctl00_contentHolder_txtPassWordCompare', 'ctl00_contentHolder_txtNewPassWord', '两次输入的密码不一致请重新输入'));
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>
