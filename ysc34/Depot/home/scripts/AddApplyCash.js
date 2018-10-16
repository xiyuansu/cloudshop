var url = 'ashx/ApplyCashManage.ashx';
$(function () {
    CheckBankInfo();
});

function CheckBankInfo() {
    $.ajax({
        url: url, data: { flag: "GetDrawCardInfo" }, type: "post", success: function (json) {
            var obj = eval('(' + json + ')');
            if (obj.Result.BankCardNo == "" || obj.Result.BankCardNo == null) {
                ShowMsg("您还未绑定银行卡,请先绑定银行卡!", false);
                window.location.href = "AccountSettings.aspx";
            }
            else {
                $("#iBankAccountName").html(obj.Result.BankAccountName);
                $("#iBankCardNo").html(obj.Result.BankCardNo);
                $("#iBankName").html(obj.Result.BankName)
                $("#iBalance").html(obj.Result.Balance);
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
    var RequestAmount = $("#txtRequestAmount").val();
    var Remark = $("#txtRemark").val();
    var fix_amountTest = /^(([1-9]\d*)|\d)(\.\d{1,2})?$/;
    if (fix_amountTest.test(RequestAmount) == false) {
        ShowMsg("请输入有效金额!", false);
        $("#txtRequestAmount").focus();
        return;
    }
    if (parseFloat(RequestAmount) <= 0) {
        ShowMsg("提现金额必须大于0!", false);
        $("#txtRequestAmount").focus();
        return;
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


    $.post(url, { flag: "ApplyBalanceRequest", Password: Password, CardType: CardType, RequestAmount: RequestAmount, Remark: Remark }, function (json) {
        var obj = eval('(' + json + ')');
        if (obj.Result != undefined) {
            if (obj.Result.Status == "SUCCESS") {
                ShowMsg("申请成功!", true);
                window.location.href = "/Depot/home/ApplyCashManage.aspx";
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