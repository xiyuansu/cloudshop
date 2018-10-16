<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditManagerPassword.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditManagerPassword" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="managers.aspx">管理</a></li>
                <li><a href='<%="EditManager.aspx?userId="+Page.Request.QueryString["userId"] %>'>基本信息</a></li>
                <li class="hover"><a >修改密码</a></li>
            </ul>
         
        </div>
    </div>

    <div class="areacolumn clearfix">
        <div class="columnleft clearfix">
        </div>
        <div class="columnright">


            <div class="formitem validator2">
                <ul>
                    <li>
                        <span class="formitemtitle">用户名：</span>
                        <strong class="colorG">
                            <asp:Literal ID="lblLoginNameValue" runat="server" /></strong>
                    </li>
                    <li id="panelOld" runat="server">

                        <span class="formitemtitle"><em>*</em>旧密码：</span>
                        <asp:TextBox ID="txtOldPassWord" runat="server" TextMode="Password" CssClass="forminput form-control"></asp:TextBox>

                    </li>
                    <li><span class="formitemtitle"><em>*</em>新密码：</span>
                        <asp:TextBox runat="server" ID="txtNewPassWord" TextMode="Password" CssClass="forminput form-control" placeholder="密码不能为空，长度在6-20个字符之间" />
                        <%--<p id="ctl00_contentHolder_txtNewPassWordTip">密码不能为空，长度在6-20个字符之间</p>--%>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>确认密码：</span>
                        <asp:TextBox runat="server" ID="txtPassWordCompare" TextMode="Password" CssClass="forminput form-control" placeholder="请再次输入密码" />
                        <%--<p id="ctl00_contentHolder_txtPassWordCompareTip">请再次输入密码</p>--%>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnEditPassWordOK" runat="server" OnClientClick="return PageIsValid();" CssClass="btn btn-primary" Text="保 存" />
                </div>
            </div>

        </div>
    </div>
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtNewPassWord', 6, 20, false, null, '密码不能为空，长度在6-20个字符之间'));
            initValid(new CompareValidator('ctl00_contentHolder_txtPassWordCompare', 'ctl00_contentHolder_txtNewPassWord', '两次输入的密码不一致请重新输入'));
        }

        function link() {
            window.location.href = 'Managers.aspx';
        }

        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>

