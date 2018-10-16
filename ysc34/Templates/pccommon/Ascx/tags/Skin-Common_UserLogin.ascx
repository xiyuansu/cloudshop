<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Context" %>
<%@ Import Namespace="Hidistro.Core" %>

<script type="text/javascript">
    $(document).ready(function () {
        $('#btn_Common_Login_Button').click(function () {
            var username = $("#txt_Common_Login_UserName").val();
            var password = $("#txt_Common_Login_Password").val();

            if (username.length == 0 || password.length == 0) {
                ShowMsg("请输入您的用户名和密码!", false);
                return;
            }

            $.ajax({
                url: "/User/Login",
                type: "post",
                dataType: "json",
                timeout: 10000,
                data: { username: username, password: password, action: "Common_UserLogin" },
                async: false,
                success: function (data) {
                    if (data.Status == "Succes") {
                        window.location.reload();
                    }
                    else {
                       ShowMsg(data.Msg,false);
                    }
                }
            });
        });

        $("#txt_Common_Login_Password").keydown(function (e) {
            if (e.keyCode == 13) {
                $('#btn_Common_Login_Button').focus();
                $('#btn_Common_Login_Button').click(function () { });
            }
        });

        $("#txt_Common_Login_UserName").keydown(function (e) {
            if (e.keyCode == 13) {
                $('#btn_Common_Login_Button').focus();
                $('#btn_Common_Login_Button').click(function () { });
            }
        });

    });
</script>
<asp:Panel runat="server" ID="pnlLogin">
    <span>
        <div class="form-group">
            <input id="txt_Common_Login_UserName" type="text" class="form-control" placeholder="用户名" />
        </div>
    </span>
    <span>
        <div class="form-group">
            <input name="Common_Login_Password" type="password" id="txt_Common_Login_Password" class="form-control" placeholder="密码" />
        </div>
    </span>

    <ul class="Default_Login_pad5">
        <li class="Default_Login_bg">
            <a href="#" class="cWhite" id="btn_Common_Login_Button" type="button">登录</a>

        </li>
        <li class="Default_Login_fwbg lCenter">
            <a href="#" class="cWhite" onclick='<%= "top.location.href=\"/Register\"" %>'>注册</a>
            <a href='/ForgotPassword' class="cWhite" target="_blank">忘记密码？</a>
        </li>
    </ul>
</asp:Panel>
<asp:Panel runat="server" ID="pnlLogout">
    <span>
        <asp:Image ID="userPicture" runat="server" ImageUrl="" ClientIDMode="Static" Width="100" Height="100" /> 
        </span>
    <span><%= HiContext.Current.User.UserName %></span>
    <span>
        <asp:Literal ID="litMemberGrade" runat="server" /></span>
    <span>可用积分：<em><asp:Literal ID="litPoint" runat="server" /></em> 分</span>
    <span class="btn"><a href="/user/UserPoints.aspx">查看积分明细</a></span>
</asp:Panel>

