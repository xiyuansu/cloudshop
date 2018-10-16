<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddManager.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddManager" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="managers.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">添加</a></li>

            </ul>

        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">

            <div class="formitem ">
                <ul>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>用户名：</span>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput form-control" placeholder="用户名不能为空，必须以汉字或是字母开头,且在2-20个字符之间" />
                        <p id="ctl00_contentHolder_txtUserNameTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>密码：</span>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="forminput form-control" placeholder="密码长度在6-20个字符之间" />
                        <p id="ctl00_contentHolder_txtPasswordTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>确认密码：</span>
                        <asp:TextBox ID="txtPasswordagain" runat="server" TextMode="Password" CssClass="forminput form-control" placeholder="请重复一次上面输入的登录密码" />
                        <p id="ctl00_contentHolder_txtPasswordagainTip"></p>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>所属部门：</span><abbr class="formselect">
                        <Hi:RoleDropDownList ID="dropRole" runat="server" AllowNull="false" CssClass="iselect" />
                    </abbr>
                    </li>
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button ID="btnCreate" runat="server" OnClientClick="return PageIsValid();" Text="添 加" CssClass="btn btn-primary" />
                    </li>
                </ul>
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
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtUserName', 2, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '用户名不能为空，必须以汉字或是字母开头,且在2-20个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtPassword', 6, 20, false, null, '密码长度在6-20个字符之间'))
            initValid(new InputValidator('ctl00_contentHolder_txtPasswordagain', 6, 20, false, null, '请重复一次上面输入的登录密码'))
            appendValid(new CompareValidator('ctl00_contentHolder_txtPasswordagain', 'ctl00_contentHolder_txtPassword', '重复密码错误'));
            initValid(new InputValidator('ctl00_contentHolder_txtEmail', 1, 256, false, '[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\.[\\w-]+)+', '请输入有效的电子邮件地址，电子邮件地址的长度在256个字符以内'))
            initValid(new SelectValidator('ctl00_contentHolder_dropRole', false, '选择管理员要加入的部门'))
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>

