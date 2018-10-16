function dload() {
    /**判断浏览器是否支持canvas**/
    try {
        document.createElement('canvas').getContext('2d');
    } catch (e) {
        var addDiv = document.createElement('div');
        alert_h_scratch('您的手机不支持刮刮卡效果哦~!');
    }
};

var IsPrized = false;

var submitStatus = false;
var IsTrue = true;

function Prized() {   
    var index = parseInt(getPrize());
    switch (index) {
        case 3:
            $(".textScratch").text("三等奖");
            break;
        case 4:
            $(".textScratch").text("四等奖");
            break;
        case 5:
            $(".textScratch").text("五等奖");
            break;
        case 6:
            $(".textScratch").text("六等奖");
            break;
        case 1:
            $(".textScratch").text("一等奖");
            break;
        case 2:
            $(".textScratch").text("二等奖");
            break;
        case 1002:
            $(".textScratch").text("未开始");
            break;
        case 1001:
            $(".textScratch").text("未登录");
            break;
        case 1003:
            $(".textScratch").text("结束");
            break;
        case 1006:
            $(".textScratch").text("不足");
            break;
        case 0:
            $(".textScratch").text("继续努力");
            break;
        case 1009:
            $(".textScratch").text("继续努力");
            break;
        default:
            $(".textScratch").text("继续努力");
            break;
    }
}

$(document).ready(function () {
    dload();
    //Prized();
	var container = document.getElementById('canvasScratch');
	init(container, 190, 40, '#c8c8c8', mobile);
    /* if ($(".textScratch").text() == "未登录") {
        alert_h_scratch("您未登录或者登录超时，请重新从微信进入！", function () {
            submitStatus = false;
            location.href = "/vshop/default.aspx";
        });
    }
    else if ($(".textScratch").text() == "结束") {
        alert_h_scratch("对不起，活动还未开始，或者已经结束！", function () {
            submitStatus = false;
            location.href = "/vshop/default.aspx";
        });
    }
    else if ($(".textScratch").text() == "已中未领") {
        alert_h_scratch("您已经中过奖了，请领奖！", function () {
            submitStatus = false;
            window.location.href = "/Vshop/WinningResults.aspx?activityid=" + GetActivityid();
        });
    }
    else if ($(".textScratch").text() == "已中") {
        alert_h_scratch("您已经中过奖了！", function () {
            submitStatus = false;
            location.href = "/vshop/default.aspx";
        });
    }
    else if ($(".textScratch").text() == "上限") {
        alert_h_scratch("您已经达到抽奖次数上限", function () {
            submitStatus = false;
            location.href = "/vshop/default.aspx";
        });
    }
    else { */
        //var isUsePoint = document.getElementById("vScratch_hidIsUsePoint").value;
        //if (isUsePoint == 1) {
          
        //    scratchConfirm('操作提示', '您的刮奖次数已经用完，再次刮奖将消耗您的积分，确定要继续刮奖吗？', '确定刮奖', function () {
               
        //    }, function () {
        //        location.href = "default.aspx";
        //    });
        //}
    // }
});


var u = navigator.userAgent, mobile = '';
if (u.indexOf('iPhone') > -1) mobile = 'iphone';
if (u.indexOf('Android') > -1 || u.indexOf('Linux') > -1) mobile = 'Android';

function createCanvas(parent, width, height) {
    var canvas = {};
    canvas.node = document.createElement('canvas');
    canvas.context = canvas.node.getContext('2d');
    canvas.node.width = width || 100;
    canvas.node.height = height || 100;
    parent.appendChild(canvas.node);
    return canvas;
}

var _x, _y;
var canvas;
var ctx;
function reset() {
    ctx.globalCompositeOperation = 'source-over';
    ctx.clearTo("#c8c8c8");
}

function init(container, width, height, fillColor, type) {
    canvas = createCanvas(container, width, height);
    ctx = canvas.context;
    // define a custom fillCircle method 
    ctx.fillCircle = function (x, y, radius, fillColor) {
        this.fillStyle = fillColor;
        this.beginPath();
        this.moveTo(x, y);
        this.arc(x, y, radius, 0, Math.PI * 2, false);
        this.fill();
    };
    ctx.clearTo = function (fillColor) {
        ctx.fillStyle = fillColor;
        ctx.fillRect(0, 0, width, height);
        ctx.fillStyle = "#a0a0a0";
        ctx.font = "14px Georgia";
        ctx.fillText("刮开中大奖!", width / 2-35, height / 2+5);
    };
    ctx.clearTo(fillColor || "#ddd");
    canvas.node.addEventListener("touchstart", function (e) {
        
        canvas.isDrawing = true;
        e.preventDefault();
        if (type == 'Android') {
            _x = e.changedTouches[0].pageX - this.offsetLeft;
            _y = e.changedTouches[0].pageY - this.offsetTop;
        } else {
            _x = e.pageX - this.offsetLeft;
            _y = e.pageY - this.offsetTop;
        }
    }, false);
    canvas.node.addEventListener("touchend", function (e) {
        e.preventDefault();
        canvas.isDrawing = false;
       

    }, false);
    canvas.node.addEventListener("touchmove", function (e) {
        //canvas.node.addEventListener("click", function (e) {
        e.preventDefault();
        if (!canvas.isDrawing) {
            return;
        }
        if (type == 'Android') {
            var x = e.changedTouches[0].pageX - this.offsetLeft;
            var y = e.changedTouches[0].pageY - this.offsetTop;
        } else {
            var x = e.pageX - this.offsetLeft;
            var y = e.pageY - this.offsetTop;
        }
        ctx.globalCompositeOperation = 'destination-out';
        ctx.lineWidth = 10;
        ctx.lineCap = "round";
        ctx.strokeStyle = "#c8c8c8";
        ctx.beginPath();
        ctx.moveTo(_x, _y);
        ctx.lineTo(x, y);
        ctx.stroke();
        _x = x;
        _y = y;
        var _w = parseInt(canvas.node.width / 20);
        var _h = parseInt(canvas.node.height / 20);
        var _c = _w * _h;
        var _c1 = 0;
        for (var i = 0; i < _w; i++) {
            for (var j = 0; j < _h; j++) {
                var imgData = ctx.getImageData(i * 20 + 1, j * 20 + 1, 1, 1);
                var alpha = imgData.data[3];
                if (alpha < 255) {
                    _c1 = _c1 + 1;
                }
            }
        }
        if (_c1 > 0)
        {
            if ($("#hidFreeTimes").val() == "0" && IsTrue) {

                if (confirm("刮奖将消耗您的积分，确定要继续刮奖吗？")) {
                    IsTrue = false;
                }
                else {
                    gotoDefult();
                }
            }           
        }
        if (_c1 > (_c / 4)) {            
            ctx.globalCompositeOperation = 'destination-out';
            ctx.fillStyle = fillColor;
            ctx.fillRect(0, 0, width, height);            
            if (!IsPrized) {
                if (submitStatus) {
                    return;
                }
                AwardDraw();
            }
            else {
                return;
            }
        }
    }, false);
}

function AwardDraw()
{
    Prized();
    submitStatus = true;
    if ($(".textScratch").text() == "无数据" || $(".textScratch").text() == "请重试") {
        alert_h_scratch("请重试", function () {
            submitStatus = false;
            //location.href = "default.aspx";
            reloadCurr();
        });
        return;
    }
    else if ($(".textScratch").text() == "未登录") {
        alert_h_scratch("您未登录或者登录超时，请重新登入！", function () {
            gotoDefult();           
        });
    }
    else if ($(".textScratch").text() == "结束") {
        alert_h_scratch("对不起，活动已经结束！", function () {            
            gotoDefult();
        });
    }
    else if($(".textScratch").text() == "未开始")
    {
        alert_h_scratch("对不起，活动还未开始！", function () {
            reloadCurr();
        });
    }
    else if ($(".textScratch").text() == "不足") {
        alert_h_scratch("您的积分不足，无法继续刮奖！", function () {            
            reloadCurr();
        });
    }
    else if ($(".textScratch").text() == "继续努力") {

        alert_h_scratch("亲，就差一点点就中奖了，继续努力吧！", function () {
            reloadCurr();
        });
    }
    else {

        alert_h_scratch("恭喜你获得" + $(".textScratch").text() + "!", function () {
            reloadCurr();
        });
    }
}

function GetActivityid() {
    var activityid = window.location.search.substr(window.location.search.indexOf("=") + 1);
    if (activityid.indexOf("&") > 0)
        activityid = activityid.substr(0, activityid.indexOf("&"));
    return activityid;
}

function getPrize() {
    var no = 0;
    var activityid = getParam("activityid");
   
    $.ajax({
        url: "/API/ActivitysHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "ActivityDraw", "ActivityId": activityid },
        async: false,
        success: function (resultData) {           
            if (resultData.Code == "1005" || resultData.Code == "1004") {
                no = resultData.AwardGrade;
                $("#hidFreeTimes").val(resultData.FreeTimes)
            }
            else {
                no = resultData.Code;
            }
        }        
    });
    return no;
}


function scratchConfirm(title, content, ensureText, ensuredCallback, notCallback) {
    var scratchConfirmCode = '<div class="modal fade" id="scratchConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content">\
                      <div class="modal-header">\
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>\
                        <h4 class="modal-title" id="myModalLabel">'+ title + '</h4>\
                      </div>\
                      <div class="modal-body">\
                        '+ content + '\
                      </div>\
                      <div class="modal-footer">\
                        <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>\
                        <button type="button" class="btn btn-danger">'+ ensureText + '</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';
    if ($("#scratchConfirm").length != 0) {
        $('#scratchConfirm').remove();
    }
    $("body").append(scratchConfirmCode);
    $('#scratchConfirm').modal("show");
    $('#scratchConfirm button.btn-default').unbind("click", "");
    $('#scratchConfirm button.btn-default').click(function (event) {
        $('#scratchConfirm').modal('hide');
        if (notCallback)
            notCallback();
    });
    $('#scratchConfirm button.btn-danger').unbind("click", "");
    $('#scratchConfirm button.btn-danger').click(function (event) {
        $('#scratchConfirm .btn-danger').attr("disabled", "disabled");
        $('#scratchConfirm').modal('hide');
        if (ensuredCallback)
            ensuredCallback();
    });
}

function alert_h_scratch(content, ensuredCallback) {
    var myConfirmCode = '<div class="modal fade" id="alert_h11" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content">\
                      <div class="modal-header">\
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>\
                        <h4 class="modal-title" id="myModalLabel">操作提示</h4>\
                      </div>\
                      <div class="modal-body">\
                        '+ content + '\
                      </div>\
                      <div class="modal-footer">\
                        <button type="button" style="width:100%;text-algin:center;border-radius: 0 0 5px 5px;"  class="btn btn-primary" data-dismiss="modal" aria-hidden="true">确认</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';

    //modal('hide')为异步方法，解决多对话框会导致页面不可用的问题
    if ($("#alert_h11").length != 0) {
        $('#alert_h11').off('hide.bs.modal').on('hide.bs.modal', function (e) {
            $('#alert_h11').remove();
            $("body").append(myConfirmCode);
            $('#alert_h11').modal();
            $('#alert_h11').off('hide.bs.modal').on('hide.bs.modal', function (e) {
                if (ensuredCallback)
                    ensuredCallback();
            });
        });
        $('#alert_h11').modal('hide');

    }
    else {
        $("body").append(myConfirmCode);
        $('#alert_h11').modal();
        $('#alert_h11').off('hide.bs.modal').on('hide.bs.modal', function (e) {
            if (ensuredCallback)
                ensuredCallback();
        });
	}
}

function gotoDefult()
{
    var pageurl = document.location.href.toLowerCase();
    if (pageurl.indexOf("/appshop/") == -1)
        location.href = "default.aspx";
    else
        goHomePage();
}

function isWeiXin() {
    var ua = window.navigator.userAgent.toLowerCase();
    if (ua.match(/micromessenger/i) == "micromessenger") {
        return true;
    } else {
        return false;
    }
}

function reloadCurr() {
    if (isWeiXin)
        window.location.href = window.location.href + "&id=" + 10000 * Math.random();
    else
        window.location.reload();
}