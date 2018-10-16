<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Install.aspx.cs" Inherits="Hidistro.UI.Web.Installer.Install" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>全网商城3.4安装向导</title>
     <script type="text/javascript" language="javascript" src="jquery-1.6.4.min.js"></script>
    <link href="Images/install.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#btnTest").bind("click", function () { RunTest(); });
        });

        var dbServer, dbName, dbUsername, dbPassword;
        var username, email, password, password2;
        var isAddDemo, testSuccessed = false;
        var siteName, siteDescription;

        function GetValues() {
            dbServer = $("#txtDbServer").val();
            dbName = $("#txtDbName").val();
            dbUsername = $("#txtDbUsername").val();
            dbPassword = $("#txtDbPassword").val();

            username = $("#txtUsername").val();
            email = $("#txtEmail").val();
            password = $("#txtPassword").val();
            password2 = $("#txtPassword2").val();

            isAddDemo = $("#chkIsAddDemo").attr("checked");

            siteName = $("#txtSiteName").val();
            siteDescription = $("#txtSiteDescription").val();
        }

        function Callback(action) {
            var resultData;

            $.ajax({
                url: "Install",
                type: 'post', dataType: 'json', timeout: 10000,
                data: {
                    isCallback: "true",
                    action: action,
                    DBServer: dbServer,
                    DBName: dbName,
                    DBUsername: dbUsername,
                    DBPassword: dbPassword,
                    Username: username,
                    Email: email,
                    Password: password,
                    Password2: password2,
                    IsAddDemo: isAddDemo,
                    SiteName: siteName,
                    SiteDescription: siteDescription,
                    TestSuccessed: testSuccessed
                },
                async: false,
                success: function (result) {
                    resultData = result;
                }
            });

            return resultData;
        }

        function RunTest() {
            if (testSuccessed && (confirm("上一次的安装环境测试已成功，您确定要再次测试吗？") == false)) {
                return;
            }

            DisableButtons();
            GetValues();
            var resultData = Callback("Test")

            if (resultData.Status == "OK") {
                testSuccessed = true;
                alert("测试成功，当前环境符合安装要求");
            }
            else {
                testSuccessed = false;
                ShowErrors(resultData);
            }

            EnableButtons();
        }

        function ShowErrors(resultData) {
            var msg = "";
            $.each(resultData.ErrorMsgs, function (i, item) {
                msg += item.Text + "\r\n";
            });
            alert(msg);
        }

        function EnableButtons() {
            $("#btnTest").removeAttr("disabled");
            $("#btnInstall").removeAttr("disabled");
        }

        function DisableButtons() {
            $("#btnTest").attr({ "disabled": "disabled" });
            $("#btnInstall").attr({ "disabled": "disabled" });
        }

        </script>
</head>
<body>
 <form id="form1" runat="server">
<div class="main">
    <div class="top"><img src="images/images_11.gif" /></div>
    <asp:Label ID="lblErrMessage" runat="server"></asp:Label>
    <div class="Install">
        <div style="Z-INDEX:99;POSITION:absolute;color:Red"><asp:Label ID="Label1" runat="server"></asp:Label></div>
        <ul>
        <li><div class="li1">数据库地址</div><div class="li8">：</div><div><asp:TextBox ID="txtDbServer" runat="server" /></div></li>
        <li><div>数据库名称</div><div class="li6">：</div><div><asp:TextBox ID="txtDbName" runat="server"/></div></li>

        <li><div>数据库登录名</div><div class="li7">：</div><div><asp:TextBox ID="txtDbUsername" runat="server"/></div></li>
        <li><div>数据库密码</div><div class="li6">：</div><div><asp:TextBox ID="txtDbPassword" TextMode="Password" runat="server"/></div></li>
        <li><div>管理员用户名</div><div class="li7">：</div><div><asp:TextBox ID="txtUsername" runat="server"/></div></li>
        <li><div class="li4">电子邮件</div><div class="li5">：</div><div><asp:TextBox ID="txtEmail" runat="server" /></div></li>
        <li><div class="li2">登录密码</div><div class="li3">：</div><div><asp:TextBox ID="txtPassword" TextMode="Password" runat="server"/></div></li>
        <li><div class="li4">确认密码</div><div class="li5">：</div><div><asp:TextBox ID="txtPassword2" TextMode="Password" runat="server"/></div></li>
        <li><div class="li2">网站名称</div><div class="li3">：</div><div><asp:TextBox ID="txtSiteName" runat="server"/></div></li>
        <li><div class="li4">简单介绍</div><div class="li5">：</div><div><asp:TextBox ID="txtSiteDescription" runat="server" /></div></li>
        </ul>
        <div class="Ca"><h4>如：202.103.87.3,12075（端口号）</h4></div>
        <div class="ch"><h2>添加演示数据：<asp:CheckBox ID="chkIsAddDemo" runat="server" />用于演示的数据，实际应用可删除</h2></div>
    </div>
    <div class="install_bottom">
        <div class="install_bo_1"><input id="btnTest" name="btnTest" type="button" /></div>
        <div class="install_bo_2"><asp:Button ID="btnInstall" runat="server"/></div>
    </div>
</div>
<div class="footer">Copyright 2009-2018 ShopEFX.com all Rights Reserved. 本产品资源均为 互站网络科技有限公司 版权所有</div>
</form>
</body>
</html>