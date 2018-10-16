
 

$(document).ready(function () {
    InitValidators(); initImageUpload();
    $("#ctl00_contentHolder_btnAddActivity").bind("click", function () {
        return getUploadImages();
    });
   
});


function fuChangeStartDate(ev) {
    var clientId = ev.target.validator[0]._ValidateTargetId
    $("#" + clientId).trigger("blur");
}

function fuChangeEndDate(ev) {
    var clientId = ev.target.validator[0]._ValidateTargetId
    $("#" + clientId).trigger("blur");
}


function fuenableDeduct(event, state) {
    var tipspan = $("#spanCJText").parent()[0].children[1].children[2];
    if (state) {
        $("#spanCJText").html("<em>*<\/em>每天免费抽奖次数：");
        $(tipspan).hide();
    } else {
        $("#spanCJText").html("<em>*<\/em>免费抽奖总次数：");
        $(tipspan).show();
    }


}
$(document).ready(function () {
    var tipspan = $("#spanCJText").parent()[0].children[1].children[2];
    if ($("#ctl00_contentHolder_ooOpen input").is(':checked')) {
        $("#spanCJText").html("<em>*<\/em>每天免费抽奖次数：");
        $(tipspan).hide();
    }
    else {
        $("#spanCJText").html("<em>*<\/em>免费抽奖总次数：");
        $(tipspan).show();
    }


});
function AddItems() {

    var length = $(".mb_0.NewItem").length + 1;
    if (length <= 6) {
        var html = $("#Demo").html().replace("indexDemo", "index" + length).replace("selectTypes", "selectTypes_" + length).replace("Values", "Values_" + length).replace("CountsTip", "Counts_" + length + "Tip").replace("Counts", "Counts_" + length).replace("Values", "Values_" + length).replace("CostsTip", "Costs_" + length + "Tip").replace("Costs", "Costs_" + length).replace("yyy", ReturnWeekCN(length)).replace("xxx", length).replace("mb_0", "mb_0 NewItem").replace("datas", length).replace("hidAwardId", "hidAwardId_" + length);
        $("#ddcontent").append(html);
    }
    if (length == 6) {
        $("#additems").hide();
    }
    GetInvild();
}

function DeleteItems(n) {
    $("#index" + n).remove();
    if ($(".mb_0.NewItem").length < 6) {
        $("#additems").show();
    }
    var slength = $(".mb_0.NewItem").length + 2;
    n = n + 1;
    for (var i = n ; i < slength; i++) {
        $("#index" + i)[0].children[0].innerHTML = "<em>*<\/em>" + ReturnWeekCN(i - 1) + "等奖：";
        $("#index" + i)[0].children[1].value = (i - 1);
        $($("#index" + i)[0]).find(".spDel a").attr("href", "javascript:DeleteItems(" + (i - 1) + ")");
        $($("#index" + i)[0]).attr("id", "index" + (i - 1));
        $($("#selectTypes_" + i)[0]).attr("id", "selectTypes_" + (i - 1));
        $($("#Values_" + i)[0]).attr("id", "Values_" + (i - 1));
        $($("#Counts_" + i)[0]).attr("id", "Counts_" + (i - 1));
        $($("#Costs_" + i)[0]).attr("id", "Costs_" + (i - 1));

        $($("#Counts_" + i + "Tip")[0]).attr("id", "Counts_" + (i - 1) + "Tip");
        $($("#Costs_" + i + "Tip")[0]).attr("id", "Costs_" + (i - 1) + "Tip");
        $($("#hidAwardId_" + i)[0]).attr("id", "hidAwardId_" + (i - 1));
    }
    GetInvild();
}

function ReturnWeekCN(n) {
    switch (n) {
        case 2:
            return "二";
        case 3:
            return "三";
        case 4:
            return "四";
        case 5:
            return "五";
        case 6:
            return "六";
    }
}

function SelectType(obj) {

    var n = $(obj).parent()[0].children[1].value;
    var type = $(obj).val();
    var Values;
    var count = $(obj).attr("id").split('_');
    if ($(obj).attr("id") == 'selectTypes_1') {

        Values = "Values_1";
    } else {
        Values = "Values_" + count[1];
    }
    //选择项为积分
    if (type == "JF") {
        var html = "<div class=\"input-group\">";
        html += "<input type=\"text\" value=\"\" id=" + Values + " class=\"form_input_xs form-control\" \/>";
        html += "<span class=\"input-group-addon\">积分<\/span><\/div>";
        $("#index" + n)[0].children[3].innerHTML = html;
        //   initValid(new InputValidator(Values, 1, 4, false, '[0-9]\\d*', '请正确输入积分数值最大分数为9999'));
        //          $("#" + Values).parent().parent().parent().find(".spDel").removeClass("martop");

        initValid(new InputValidator(Values, 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '请正确输入中奖概率'));
        appendValid(new MoneyRangeValidator(Values, 1, 9999, '输入的数值超出了系统表示范围'));
    }
    //选择项为优惠券
    if (type == "YHQ") {
        var StartDate = $("#calendarStartDate").val();
        var EndDate = $("#calendarEndDate").val();
        //    if (StartDate != "" && StartDate != null && EndDate != "" && EndDate != null) {
        $.ajax({
            url: "NewActiveVLottrey.ashx?act=GetYHQ&p=" + Math.random(),
            dataType: "json",
            type: 'get',
            data: { 'StartDate': StartDate, 'EndDate': EndDate },
            success: function (data) {

                var html = "<select class=\"iselect_one_100\" id=" + Values + " onchange=\"SItems(this)\">";
                html += " <option value=\"\">请选择优惠券<\/option>";
                for (var p in data) {
                    html += " <option   countattr=\"" + data[p]["SendCount"] + "\"  value=\"" + data[p]["CouponId"] + "\">" + subStr(data[p]["CouponName"]) + "<\/option>";
                }
                html += " <\/select>";
                $("#index" + n)[0].children[3].innerHTML = html;
                var o = $("#" + Values);
                SItems(o);
            }
        });

    }

    if (type == "LP") {
        $.ajax({
            url: "NewActiveVLottrey.ashx?act=GetLP&p=" + Math.random(),
            dataType: "json",
            type: 'get',
            success: function (data) {
                var html = "<select class=\"iselect_one_100\" id=" + Values + " >";
                html += " <option value=\"\">请选择礼品<\/option>";
                for (var p in data) {
                    html += " <option value=\"" + data[p]["GiftId"] + "\">" + subStr(data[p]["Name"]) + "<\/option>";
                }
                html += " <\/select>";
                $("#index" + n)[0].children[3].innerHTML = html;
                //      $("#" + Values).parent().parent().find(".spDel").removeClass("martop");
            }
        });
    }


}


function subStr(str) {
    if (str.length > 10) {
        str = str.substr(0, 10);
        return str;

    } else {
        return str;
    }

}

function SelectTypes(obj, val) {

    var n = $(obj).parent()[0].children[1].value;
    var type = $(obj).val();
    var Values;
    var count = $(obj).attr("id").split('_');
    if ($(obj).attr("id") == 'selectTypes_1') {

        Values = "Values_1";
    } else {
        Values = "Values_" + count[1];
    }
    //选择项为积分
    if (type == "JF") {
        var html = "<div class=\"input-group\">";
        html += "<input type=\"text\" value=\"" + val + "\" id=" + Values + " class=\"form_input_xs form-control\" \/>";
        html += "<span class=\"input-group-addon\">积分<\/span><\/div>";
        $("#index" + n)[0].children[3].innerHTML = html;
        initValid(new InputValidator(Values, 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '请正确输入中奖概率'));
        appendValid(new MoneyRangeValidator(Values, 1, 9999, '输入的数值超出了系统表示范围'));
    }
    //选择项为优惠券
    if (type == "YHQ") {
        var StartDate = $("#calendarStartDate").val();
        var EndDate = $("#calendarEndDate").val();
        //          if (StartDate != "" && StartDate != null && EndDate != "" && EndDate != null) {
        $.ajax({
            url: "NewActiveVLottrey.ashx?act=GetYHQ&p=" + Math.random(),
            dataType: "json",
            type: 'get',
            data: { 'StartDate': StartDate, 'EndDate': EndDate },
            success: function (data) {

                var html = "<select class=\"iselect_one_100\" id=" + Values + " onchange=\"SItems(this)\">";
                html += " <option value=\"\"><\/option>";
                for (var p in data) {
                    if (data[p]["CouponId"] == val) {
                        html += " <option  selected=\"selected\"  countattr=\"" + data[p]["SendCount"] + "\"  value=\"" + data[p]["CouponId"] + "\">" + subStr(data[p]["CouponName"]) + "<\/option>";
                    }
                    else {
                        html += " <option   countattr=\"" + data[p]["SendCount"] + "\"  value=\"" + data[p]["CouponId"] + "\">" + subStr(data[p]["CouponName"]) + "<\/option>";
                    }
                }
                html += " <\/select>";
                $("#index" + n)[0].children[3].innerHTML = html;
                var o = $("#" + Values);
                SItems(o);
            }
        });

    }

    if (type == "LP") {
        $.ajax({
            url: "NewActiveVLottrey.ashx?act=GetLP&p=" + Math.random(),
            dataType: "json",
            type: 'get',
            success: function (data) {
                var html = "<select class=\"iselect_one_100\" id=" + Values + " >";
                html += " <option value=\"\"><\/option>";
                for (var p in data) {
                    if (data[p]["GiftId"] == val) {
                        html += " <option selected=\"selected\" value=\"" + data[p]["GiftId"] + "\">" + subStr(data[p]["Name"]) + "<\/option>";
                    } else {
                        html += " <option value=\"" + data[p]["GiftId"] + "\">" + subStr(data[p]["Name"]) + "<\/option>";
                    }
                }
                html += " <\/select>";
                $("#index" + n)[0].children[3].innerHTML = html;

            }
        });
    }


}

function SItems(n) {
    var str = $(n).find("option:selected").attr("countattr");
    if (str != null && str != "") {
        if ($(n).parent()[0].children.length == 2) {
            $($(n).parent()[0].children[1]).remove();
        }
        $(n).parent().append("<font color=\"red\">剩余数量：" + str + "<\/font>");
        ///       $(n).parent().parent().find(".spDel").addClass("martop");

    } else {
        $($(n).parent()[0].children[1]).remove();
    }
}

function GetJsonByAward() {

    var slength = $(".mb_0.NewItem").length;

    if (verification(slength)) {
        return true;
    } else {
        return false;
    }
}

function verification(slength) {

    var str = "";

    str += "[";

    var Costs = 0;
    for (var i = 0; i < slength; i++) {
        if (i >= 1) {
            str += ',';
        }
        var num = i + 1;

        str += '{';
        str += '"AwardGrade":' + num + ',';
        str += '"ActivityId":' + num + ',';
        var ddSelect = $("#selectTypes_" + num);
        var Values = $("#Values_" + num);
        var Counts = $("#Counts_" + num);
        var Cost = $("#Costs_" + num);
        var hidAwardId = $("#hidAwardId_" + num);
        if (ddSelect.val() == "" || ddSelect.val() == null) {
            $(ddSelect).addClass("errorFocus");
            return false;
        }
        else {

            str += "\"PrizeType\":" + GetPrizeType(ddSelect.val()) + ',';
        }

        if (Values.val() == "" || Values.val() == null) {
            $(Values).addClass("errorFocus");
            return false;
        }
        else {

            str += "\"PrizeValue\":" + Values.val() + ',';
        }

        if (Counts.val() == "" || Counts.val() == null) {
            $(Counts).addClass("errorFocus");
            return false;
        }
        else {
            str += "\"AwardNum\":" + Counts.val() + ',';
        }

        if (Cost.val() == "" || Cost.val() == null) {
            $(Cost).addClass("errorFocus");
            return false;
        }
        else {
            str += "\"HitRate\":" + Cost.val() + ',';
            Costs = Costs + parseFloat(Cost.val());
        }
        if (hidAwardId.val() == "") {
            str += "\"AwardId\":" + 0;
        } else {
            str += "\"AwardId\":" + hidAwardId.val();
        }
        str += '}';

    }

    str += ']';

    $("#hidJson").val(str);
    if (Costs > 100) {

        ShowMsg("概率总和不能超过100%！", false);
        return false;
    }
    return true;
}

function GetPrizeType(str) {
    if (str == "JF") {
        return 1;
    }
    else if (str == "YHQ") {
        return 2;
    } else if (str == "LP") {
        return 3;
    }

}

function GetPrizeTypes(str) {
    if (str == 1) {
        return "JF";
    }
    else if (str == 2) {
        return "YHQ";
    } else if (str == 3) {
        return "LP";
    }

}
 

function GetInvild() {

    $("#ddcontent li").each(function (i) {
        var num = 1 + i;

        initValid(new InputValidator("Costs_" + num, 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '请正确输入中奖概率'));
        appendValid(new MoneyRangeValidator("Costs_" + num, 0, 100, '输入的数值超出了系统表示范围'));
        initValid(new InputValidator("Counts_" + num, 1, 4, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '请正确输入奖品数量，最大分数为9999'));
    });
}
