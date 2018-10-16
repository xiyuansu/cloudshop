var url = 'ashx/ServiceVerification.ashx';
$(function () {
});

function doSubmit() {
    var VerificationItems = $("#txtVerificationItems").val();

    if (VerificationItems == "") {
        ShowMsg("核销码不能为空,请重新输入!", false);
        $("#txtVerificationItems").focus();
        return;
    }

    $.post(url, { flag: "CheckVerification", VerificationItems: VerificationItems }, function (json) {
        var obj = eval('(' + json + ')');
        if (obj.Result != undefined) {
            if (obj.Result.Success.Status == true) {
                DialogFrameClose('home/CheckVerification.aspx?Code=' + VerificationItems, "验证核销", 800, 500, function (e) { CloseFrameWindow(); });
            }
            else {
                ShowMsg(obj, false);
                $("#divLoading").hide();
            }
        }
        else {
            ShowMsg(obj.ErrorResponse.ErrorMsg, false);
        }
    }
       );
}

function CloseFrameWindow()
{ }