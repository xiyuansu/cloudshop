var url = 'ashx/ApplyCashManage.ashx';
$(function () {
    CheckPasswordInfo();
});
var IsOpenAlipayDraw = false;
function CheckPasswordInfo() {
    $.ajax({
        url: url, data: { flag: "CheckPasswordInfo" }, type: "post", success: function (json) {
            var obj = eval('(' + json + ')');
            if (obj.Result.Status == "FAIL") {
                ShowMsg("请先设置交易密码!", false);
                window.location.href = "SetTradePassword.aspx";
            }
            else {
                BindBankInfo();
            }

        }, error: function (r) {
        }
    });
}

function BindBankInfo() {
    $.ajax({
        url: url, data: { flag: "GetDrawCardInfo" }, type: "post", success: function (json) {
            var obj = eval('(' + json + ')');

            $("#txtBankName").val(obj.Result.BankName);
            $("#txtBankAccountName").val(obj.Result.BankAccountName);
            $("#txtBankCardNo").val(obj.Result.BankCardNo);
            $("#txtPassword").val("");
            $("#txtAlipayAccount").val(obj.Result.AlipayAccount);
            $("#txtAlipayRealName").val(obj.Result.AlipayRealName);
            IsOpenAlipayDraw = obj.Result.IsOpenAlipayDraw;

            if (obj.Result.IsOpenAlipayDraw) {
                $("#ulAlipay").show();
            }
            else {
                $("#ulAlipay").hide();
            }
        }, error: function (r) {
            try {
                loadingobj.close();
            } catch (e) { }
            ShowMsg("系统内部异常", false);

        }
    });
}

function doSubmit() {
    var Password = $("#txtPassword").val();
    var CardType = 2;//银行卡
    var BankName = $("#txtBankName").val();
    var BankAccountName = $("#txtBankAccountName").val();
    var BankCardNo = $("#txtBankCardNo").val();
    var alipayAccount = $("#txtAlipayAccount").val();
    var alipayRealName = $("#txtAlipayRealName").val();
    if (!IsOpenAlipayDraw) {
        alipayAccount = "";
        alipayRealName = "";
    }
    else {
        if ((alipayAccount != "" && alipayRealName == "") || (alipayAccount == "" && alipayRealName != "")) {
            ShowMsg("支付宝帐号信息请填写完整，帐号和姓名必须都要填写，如果不绑定请都不填写!", false);
            if (alipayAccount != "" && alipayRealName == "")
                $("#txtAlipayRealName").focus();
            else
                $("#txtAlipayAccount").focus();
            return;
        }
    }
    if (Password == "") {
        ShowMsg("交易密码不能为空,请重新输入!", false);
        $("#txtPassword").focus();
        return;
    }
    if (CheckPassword(Password) == false) {
        ShowMsg("交易密码验证不通过,请重新输入!", false);
        $("#txtPassword").focus();
        return;
    }
    if (!IsOpenAlipayDraw) {
        if (BankName == "") {
            ShowMsg("开户银行不能为空,请重新输入!", false);
            $("#txtBankName").focus();
            return;
        }
        if (BankAccountName == "") {
            ShowMsg("账户名不能为空,请重新输入!", false);
            $("#txtBankAccountName").focus();
            return;
        }
        if (BankCardNo == "") {
            ShowMsg("银行卡号不能为空,请重新输入!", false);
            $("#txtBankCardNo").focus();
            return;
        }
    }


    $.post(url, { flag: "BindDrawCardInfo", AlipayAccount: alipayAccount, AlipayRealName: alipayRealName, Password: Password, CardType: CardType, BankName: BankName, BankAccountName: BankAccountName, BankCardNo: BankCardNo }, function (json) {
        var obj = eval('(' + json + ')');
        if (obj.Result != undefined) {
            if (obj.Result.Status == "SUCCESS") {
                ShowMsg("绑定成功!", true);
                window.location.href = "/Depot/home/AccountSettings.aspx";
            }
            else {
                ShowMsg(obj.Result.Msg, false);
            }
        }
        else {
            ShowMsg(obj.ErrorResponse.ErrorMsg, false);
        }
    }
       );

}

function CheckPassword(Password) {
    $.ajax({
        url: url, data: { flag: "ValidTradePassword", Password: Password }, type: "post", success: function (json) {
            var obj = eval('(' + json + ')');
            if (obj.Result.Msg == "验证成功") {
                return true;
            }
            else {
                return false;
            }

        }, error: function (r) {
            return false;
        }
    });
}