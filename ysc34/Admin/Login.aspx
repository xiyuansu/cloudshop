<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Login" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script type="text/javascript" src="../Utility/jquery-1.8.3.min.js"></script>
    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    <Hi:PageTitle ID="PageTitle1" runat="server" />
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="css/login.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <div class="login_bg"></div>
    <form id="form1" runat="server">
        <div class="Con_r" id="mt">
            <div class="title"></div>
            <div class="logo">
                <img src="images/logo.jpg" alt="logo" />
            </div>
            <ul>

                <li>
                    <div class="input">
                        <span>
                            <img src="images/icon_username.png" /></span>
                        <asp:TextBox ID="txtAdminName" CssClass="input1" runat="server" TabIndex="1"></asp:TextBox>
                    </div>
                </li>
                <li>
                    <div class="input">
                        <span>
                            <img src="images/icon_keywords.png" style="margin-top: 7px;" /></span>
                        <asp:TextBox ID="txtAdminPassWord" CssClass="input2" runat="server" TextMode="Password" TabIndex="2" />
                    </div>
                </li>
                <li runat="server" id="imgCode" visible="false">
                    <div class="input">
                        <div class="Verifi">
                            <a href="javascript:refreshCode();" tabindex="-1">
                                <i class="glyphicon glyphicon-ok"></i>
                                <i class="glyphicon glyphicon-trash"></i>
                                <img id="imgVerifyCode" src="../VerifyCodeImage.aspx" alt="" style="border-style: none" /></a>
                        </div>
                        <asp:TextBox ID="txtCode" runat="server" class="input3" MaxLength="4" placeholder="请输入验证码" TabIndex="3"></asp:TextBox>
                    </div>

                </li>
            </ul>

            <div class="input_b">
                <asp:Button ID="btnAdminLogin" runat="server" Text="登   录" CssClass="submit" TabIndex="4" />
                <asp:HiddenField ID="ErrorTimes" runat="server" Value="0" />
            </div>
            <div class="input_c">
                <asp:Literal ID="lblStatus" runat="server" Visible="false"></asp:Literal>
            </div>

        </div>
    </form>
    <script language="javascript" type="text/javascript">
        function refreshCode() {
            var img = document.getElementById("imgVerifyCode");
            if (img != null) {
                var currentDate = new Date();
                img.src = '<%=  "/VerifyCodeImage.aspx?t=" %>' + currentDate.getTime();
            }
        }
        $(document).ready(function () {
            $("#txtCode").keyup(function () {
                var value = $(this).val();
                if (value.length < 4) {
                    $(".input i").hide();
                    temp = "";
                }
                else if (value.length == 4) {
                    if (temp != value) {
                        $.ajax({
                            url: "Login.aspx",
                            type: 'post', dataType: 'json', timeout: 10000,
                            data: {
                                isCallback: "true",
                                code: $("#txtCode").val()
                            },
                            async: false,
                            success: function (resultData) {
                                var flag = resultData.flag;
                                if (flag == "1") {
                                    $(".glyphicon-ok").show();
                                    $(".input_c").html("&nbsp;");
                                }
                                else if (flag == "0") {
                                    $(".glyphicon-remove").show();
                                    $(".input_c").html("验证码不正确");
                                }
                            }
                        });
                    }
                    temp = value;
                }
            });

            if ($(".input3").val() == "") {
                $(".input_c").html("请输入验证码");
            }

        });

        $(function () {
            var w = $(window).width();
            var h = $(window).height();
            var ml = w / 2 - 180;
            var mt = h * 0.25 - 100;
            $('#mt').css("margin-left", ml);
            $('#mt').css("margin-top", mt);
        });
    </script>
</body>
</html>
