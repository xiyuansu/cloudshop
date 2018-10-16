
window.onload = function () {
    init();    
}

function checkHasPrized() {
    var activityid = GetActivityid();
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "CheckHasPrized", "activityid": activityid },
        async: false,
        success: function (resultData) {
            var no = parseInt(resultData.No);
            if (no == -1) {
                alert_h_egg("活动不存在！", function () {
                    location.href = "/vshop/default.aspx";
                });
            }
            else if (no == -2) {
                alert_h_egg("您已经中过奖了,请领奖！", function () {
                    gotoResult();
                });
            }
            else if (no == -3) {
                alert_h_egg("您已经中过奖了！", function () {
                    location.href = "/vshop/default.aspx";
                });
            }
            else if (no == -4) {
                alert_h_egg("对不起，活动还未开始，或者已经结束！", function () {
                    location.href = "/vshop/default.aspx";
                });
            }
        }
    });
}

var isprized = false;
// JavaScript Document
var ctx;
var canvas;
var eggImg;
var egg1Img;
var hammerImg;
var angles;
var speed = 0.009;//速度
var radiusX = 50;//
var radiusY = 15;//
var centerX = 150;//
var centerY = 220;//
var u = navigator.userAgent, mobile = '';
var isRunning = false;
var xArray, yArray;
var selectIndex = -1;
var spinTimeout = null;
var prizeNO = -1;
var submitStatus = false;



var lastSubmitTime = new Date();
var submitTimes = 0;
function init() {

    angles = new Array(3);
    angles[0] = Math.PI / 18 * 3;
    angles[1] = Math.PI / 18 * 15;
    angles[2] = Math.PI / 18 * 27;

    xArray = new Array(3);
    yArray = new Array(3);

    canvas = document.getElementById("wheelcanvas");
    eggImg = document.getElementById("eggImg");
    egg1Img = document.getElementById("egg1Img");
    hammerImg = document.getElementById("hammerImg");

    if (canvas.getContext) {
        ctx = canvas.getContext("2d");
        draw();
        go();
    } else {
        alert_h('亲，你的手机不支持此活动!');
        return;
    }

    if (u.indexOf('iPhone') > -1) mobile = 'iphone';
    if (u.indexOf('Android') > -1 || u.indexOf('Linux') > -1) mobile = 'Android';

    canvas.addEventListener("touchstart", function (e) {
        //canvas.addEventListener("click", function (e) {
        //两秒内重复点击直接返回false
        //var tempDate = new Date();
        //if ((tempDate.getTime() - lastSubmitTime.getTime()) < 0 && submitTimes > 0) {
        //    lastSubmitTime = new Date();
        //    submitTimes += 1;
        //    return false;
        //}
        //lastSubmitTime = new Date();
        //submitTimes += 1;
        var log;
        e.preventDefault();
        var _x, _y;
        if (mobile == 'Android') {
            log = "Page =>" + e.changedTouches[0].pageX + ":" + e.changedTouches[0].pageY;
            _x = e.changedTouches[0].pageX - this.offsetLeft - this.parentNode.offsetLeft;
            _y = e.changedTouches[0].pageY - this.offsetTop - this.parentNode.offsetTop;
        } else {
            log = "Page =>" + e.pageX + ":" + e.pageY;
            _x = e.pageX - this.offsetLeft - this.parentNode.offsetLeft;
            _y = e.pageY - this.offsetTop - this.parentNode.offsetTop;
        }

        log += "\r\nOffset =>" + this.offsetLeft + ":" + this.offsetTop;
        log += "\r\nPos =>" + _x + ":" + _y;

        //   document.getElementById("logs").innerText = log;
       // prizeNO = getPrize();
        if (!isRunning)
            return;
        var min = -1, minDistant = 300;
        var __x, __y;
        for (var i = 0; i < 3; i++) {
            var num = Math.sqrt((_x - xArray[i]) * (_x - xArray[i]) + (_y - yArray[i]) * (_y - yArray[i]));
            if ((num < (20 / scales[i])) && (num < minDistant)) {
                minDistant = num;
                min = i;
                __x = xArray[i];
                __y = yArray[i];
            }
        }
        if (min >= 0) {
            var isUsePoint = document.getElementById("hidIsUsePoint").value;
            if (isUsePoint == 1) {
                eggConfirm('操作提示', '您的砸蛋次数已经用完，再次砸蛋将消耗您的积分，确定要继续砸蛋吗？', '确定砸蛋', function () {
                    //spin();
                    if (submitStatus) {
                        return;
                    }
                    submitStatus = true;
                    prizeNO = getPrize();
                    isRunning = false;
                    selectIndex = min;
                    clearTimeout(spinTimeout);
                    createBoom(__x, __y);
                    submitStatus = false;
                });
            }
            else {
                if (submitStatus) {
                    return;
                }
                submitStatus = true;
                //spin();
                prizeNO = getPrize();
                isRunning = false;
                selectIndex = min;
                clearTimeout(spinTimeout);
                createBoom(__x, __y);
                submitStatus = false;
            }
        }
    }, false);
}

var scales = new Array(3);
var sorts = new Array(3);

function draw() {
    ctx.clearRect(0, 0, 300, 300);
    ctx.save();

    var _x, _y;
    var w = Number(eggImg.width);
    var h = Number(eggImg.height);

    for (var i = 0; i < 3; i++) {
        scales[i] = 2 - Math.cos(angles[i] - Math.PI / 2);
    }

    var max = 0; var min = 5;
    for (var j = 0; j < 3; j++) {
        if (scales[j] > max) {
            max = scales[j];
            sorts[0] = j;
        }
        if (scales[j] < min) {
            min = scales[j];
            sorts[2] = j;
        }
    }
    for (var j = 0; j < 3; j++) {
        if ((sorts[0] != j) && (sorts[2] != j)) {
            sorts[1] = j;
        }
    }

    for (var i = 0; i < 3; i++) {
        var pos = sorts[i];
        var scale = scales[pos];
        _x = Math.cos(angles[pos]) * radiusX + centerX;
        _y = Math.sin(angles[pos]) * radiusY + centerY;
        _w = w / scale / 1.3;
        _h = h / scale / 1.3;
        _y = _y - _h / 2;
        xArray[pos] = _x;
        yArray[pos] = _y;
        ctx.drawImage(eggImg, _x - _w / 2, _y - _h / 2, _w, _h);
        angles[pos] += speed;
    }
    ctx.restore();
}

function rnd(n) {
    return (n || 1) * Math.random();
}

function createPT(x, y, r, g, b) {
    return {
        r: r, g: g, b: b,
        x: x, y: y,
        //
        pl: [],
        dx: rnd(20) - 10,
        dy: rnd(10) - 7,
        life: 30 + rnd(30),
        //
        move: function () {
            this.dx *= .98;
            this.dy *= .98;
            this.dy += .22;
            this.x += this.dx;
            this.y += this.dy;
            this.pl.push([this.x, this.y]);
            //
            if (this.pl.length > 10) this.pl.shift();
            this.life--;
        }
    };
}

var q = [];
var fireTimer = null;
function createBoom(x, y) {
    q = [];
    for (var i = 0; i < rnd(16) + 15; i++) q.push(createPT(x, y, rnd(255) | 0, rnd(255) | 0, rnd(255) | 0));
    fireTimer = setInterval('drawFire()', 20);
}

function dq(ar, z, r, g, b) {
    ctx.save();
    //
    for (var i = 0; i < ar.length; i++) {
        ctx.strokeStyle = 'rgba(' + r + ',' + g + ',' + b + ',' + Math.abs(.2 * z) + ')';
        ctx.lineWidth = Math.min(i + 1, 4) * 2;
        ctx.beginPath();
        ctx.moveTo(ar[i][0], ar[i][1]);
        for (var j = i + 1; j < ar.length; j++) ctx.lineTo(ar[j][0], ar[j][1]);
        ctx.stroke();
    }
    ctx.restore();
}

function drawFire() {
    ctx.fillStyle = "#EEEEFF";
    ctx.clearRect(0, 0, 300, 300);
    ctx.save();

    for (var i = 0; i < 3; i++) {
        var pos = sorts[i];
        var scale = scales[pos];
        _x = Math.cos(angles[pos]) * radiusX + centerX;
        _y = Math.sin(angles[pos]) * radiusY + centerY;
        _w = eggImg.width / scale / 2;
        _h = eggImg.height / scale / 2;
        xArray[pos] = _x;
        yArray[pos] = _y;
        if (pos == selectIndex) {
            ctx.drawImage(egg1Img, _x - _w / 2, _y - _h / 2, _w, _h);
        } else {
            ctx.drawImage(eggImg, _x - _w / 2, _y - _h / 2, _w, _h);
        }
    }

    ctx.restore();
    for (var i = 0; i < q.length; i++) {
        var pt = q[i];
        pt.move();
        dq(pt.pl, pt.life / 30, pt.r, pt.g, pt.b);
        //
        if (pt.life <= 0) q.splice(i, 1);
    }
    if (q.length <= 0)//
    {
        clearTimeout(fireTimer);

        if (!isprized) {
            Prized();
        }
        else {
            return;
        }
    }
}

function go() {
    if (isRunning) { return; }
    angles[0] = Math.PI / 18 * 3;
    angles[1] = Math.PI / 18 * 15;
    angles[2] = Math.PI / 18 * 27;
    xArray[0] = 0;
    xArray[1] = 0;
    xArray[2] = 0;
    yArray[0] = 0;
    yArray[1] = 0;
    yArray[2] = 0;
    isRunning = true;
    spinTimeout = setInterval('draw()', 5);
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

function Prized() {
    isprized = true;
    var index = Number(prizeNO);
    switch (index) {
        case 3:
            alert_h_egg("恭喜获得：三等奖！", function () {
                reloadCurr();
            });
            break;
        case 4:
            alert_h_egg("恭喜获得：四等奖！", function () { reloadCurr(); });
            break;
        case 5:
            alert_h_egg("恭喜获得：五等奖！", function () { reloadCurr(); });
            break;
        case 6:
            alert_h_egg("恭喜获得：六等奖！", function () { reloadCurr(); });
            break;
        case 1:
            alert_h_egg("恭喜获得：一等奖！", function () { reloadCurr(); });
            break;
        case 2:
            alert_h_egg("恭喜获得：二等奖！", function () { reloadCurr(); });
            break;
        case 1002:          
            alert_h_egg("亲，活动未开始呦！！", function () {
                reloadCurr();
            });
            break;
        case 1001:            
            alert_h_egg("亲，请先登入再来！", function () {
                gotoDefult();
            });
            break;
        case 1003:          
            alert_h_egg("亲，活动已结束呦！", function () {
                gotoDefult();
            });
            break;
        case 1006:         
            alert_h_egg("亲，积分不足无法继续抽奖！", function () {
                reloadCurr();
            });
            break;       
        default:           
            alert_h_egg("亲，就差一点点啦，继续努力哟！", function () {
                reloadCurr();
            });
            break;
    }

}

function gotoResult() {
    window.location.href = "/Vshop/WinningResults.aspx?activityid=" + GetActivityid();

}



function eggConfirm(title, content, ensureText, ensuredCallback) {
    var eggConfirmCode = '<div class="modal fade" id="eggConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
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
    if ($("#eggConfirm").length != 0) {
        $('#eggConfirm').remove();
    }
    $("body").append(eggConfirmCode);
    $('#eggConfirm').modal("show");
    $('#eggConfirm button.btn-danger').unbind("click", "");
    $('#eggConfirm button.btn-danger').click(function (event) {
        $('#eggConfirm button.btn-danger').attr("disabled", "disabled");
        $('#eggConfirm').modal('hide');
        if (ensuredCallback)
            ensuredCallback();
    });
}



function alert_h_egg(content, ensuredCallback) {
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
                        <button type="button" class="btn btn-primary" data-dismiss="modal" aria-hidden="true" style=\"width:100%\">确认</button>\
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

function gotoDefult() {
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