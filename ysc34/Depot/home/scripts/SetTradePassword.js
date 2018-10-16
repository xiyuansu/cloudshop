var url = 'ashx/ApplyCashManage.ashx';

function doSubmit() {
    var Password = $("#txtPassword").val();
    var RePassword = $("#txtRePassword").val();

    if (Password == "") {
        ShowMsg("交易密码不能为空,请重新输入!", false);
        $("#txtPassword").focus();
        return;
    }
    if (RePassword == "") {
        ShowMsg("重复交易密码不能为空,请重新输入!", false);
        $("#txtRePassword").focus();
        return;
    }
    if (RePassword != Password) {
        ShowMsg("两次密码不一致,请重新输入!", false);
        $("#txtPassword").focus();
        return;
    }

    $.post(url, { flag: "SetTradePassword", Password: Password, RePassword: RePassword }, function (json) {
        var obj = eval('(' + json + ')');
        if (obj.Result != undefined) {
            if (obj.Result.Status == "SUCCESS") {
                ShowMsg("设置成功!", true);
                window.location.href = "/Depot/home/AccountSettings.aspx";
            }
            else {
                alert(obj.Result.Msg);
            }
        }
        else {
            alert(obj.ErrorResponse.ErrorMsg);
        }
    }
       );

}