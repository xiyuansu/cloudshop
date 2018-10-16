
window.onload = function () {
    init();
    checkHasPrized();
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
               alert_h_wheel("活动不存在！", function () {
                   location.href = "/vshop/default.aspx";
               });
           }
           else if (no == -2) {
               alert_h_wheel("您已经中过奖了,请领奖！", function () {
                   gotoResult();
               });
           }
           else if (no == -3) {
               alert_h_wheel("您已经中过奖了！", function () {                  
               });
           }
           else if (no == -4) {
               alert_h_wheel("对不起，活动还未开始，或者已经结束！", function () {                   
               });
           }
        }
    });
}

  
  var startAngle = 0;
  var arc = Math.PI / 6;
  var spinTimeout = null;
  
  var spinArcStart = 10;
  var spinTime = 0;
  var spinTimeTotal = 0;
  
  var ctx;
  var isRunning = false;
  var u = navigator.userAgent,mobile = '';

  var submitStatus = false;

  var lastSubmitTime = new Date();
  var submitTimes = 0;
  function init() {
      var canvas = document.getElementById("wheelcanvas");
      if (u.indexOf('iPhone') > -1) mobile = 'iphone';
      if (u.indexOf('Android') > -1 || u.indexOf('Linux') > -1) mobile = 'Android';

      canvas.addEventListener("touchstart", function (e) { //touchstart  mousedown
         
          e.preventDefault();
          if (isRunning)
              return;
          var _x, _y;
          if (mobile == 'Android') {
              _x = e.changedTouches[0].pageX - this.offsetLeft;
              _y = e.changedTouches[0].pageY - this.offsetTop;
          } else {
              _x = e.pageX - this.offsetLeft;
              _y = e.pageY - this.offsetTop;
          }
          var num = Math.sqrt((_x - 130) * (_x - 130) + (_y - 130) * (_y - 130));
          if (num <= 40) {
              setTimeout(function () {
                  var isUsePoint = document.getElementById("vBigWheel_hidIsUsePoint").value;
                  if (isUsePoint == 1) {
                      bigwheelConfirm('操作提示', '您的抽奖次数已经用完，再次抽奖将消耗您的积分，确定要继续抽奖吗？', '确定抽奖', function () {
                          //两秒内重复点击直接返回false
                          var tempDate = new Date();
                          if ((tempDate.getTime() - lastSubmitTime.getTime()) < 2000 && submitTimes > 0) {
                              lastSubmitTime = new Date();
                              submitTimes += 1;
                              return false;
                          }
                          lastSubmitTime = new Date();
                          submitTimes += 1;

                          //if (submitStatus) {
                          //    return;
                          //}
                          //submitStatus = true;
                          spin();
                      });
                  }
                  else {
                      var tempDate = new Date();
                      if ((tempDate.getTime() - lastSubmitTime.getTime()) < 2000 && submitTimes > 0) {
                          lastSubmitTime = new Date();
                          submitTimes += 1;
                          return false;
                      }
                      lastSubmitTime = new Date();
                      submitTimes += 1;
                      //if (submitStatus) {
                      //    return;
                      //}
                      //submitStatus = true;
                      spin();
                  }
              }, 300);
          }
      }, false);
      canvas.addEventListener("click", function (e) { //touchstart  mousedown
          e.preventDefault();
          if (isRunning)
              return;
          var _x, _y;
          if (mobile == 'Android') {
              _x = e.changedTouches[0].pageX - this.offsetLeft;
              _y = e.changedTouches[0].pageY - this.offsetTop;
          } else {
              _x = e.pageX - this.offsetLeft;
              _y = e.pageY - this.offsetTop;
          }
          var num = Math.sqrt((_x - 130) * (_x - 130) + (_y - 130) * (_y - 130));
          if (num <= 40) {
                var isUsePoint = document.getElementById("vBigWheel_hidIsUsePoint").value;
                if (isUsePoint == 1) {
                    bigwheelConfirm('询问', '您的抽奖次数已经用完，再次抽奖将消耗您的积分，确定要继续抽奖吗？', '确定抽奖', function () {
                        //两秒内重复点击直接返回false
                        var tempDate = new Date();
                        if ((tempDate.getTime() - lastSubmitTime.getTime()) < 2000 && submitTimes > 0) {
                            lastSubmitTime = new Date();
                            submitTimes += 1;
                            return false;
                        }
                        lastSubmitTime = new Date();
                        submitTimes += 1;
                        //if (submitStatus) {
                        //    return;
                        //}
                        //submitStatus = true;
                        spin();
                    });
                }
                else {
                    //两秒内重复点击直接返回false
                    var tempDate = new Date();
                    if ((tempDate.getTime() - lastSubmitTime.getTime()) < 2000 && submitTimes > 0) {
                        lastSubmitTime = new Date();
                        submitTimes += 1;
                        return false;
                    }
                    lastSubmitTime = new Date();
                    submitTimes += 1;
                    //if (submitStatus) {
                    //    return;
                    //}
                    //submitStatus = true;
                    spin();
                }
          }
      }, false);
      drawRouletteWheel();
  }
  
  function drawRouletteWheel() {
    var canvas = document.getElementById("wheelcanvas");
    var bgImg = document.getElementById("vBigWheel_bgimg");
    var flagImg = document.getElementById("flagimg");
    ctx = canvas.getContext("2d");
    var textRadius = 110;

    if (canvas.getContext) {
		ctx.clearRect(0,0,canvas.width,canvas.height);
		ctx.save();
		ctx.translate(130,130);
		ctx.rotate(startAngle);
		ctx.drawImage(bgImg,-bgImg.width/2,-bgImg.height/2);

		ctx.restore();

        ctx.save();
		ctx.translate(130,130);
		ctx.drawImage(flagImg,-flagImg.width/2,-flagImg.height/2-14);
        ctx.restore();
	}
  }
  function spin() {
      var no = parseInt(getPrize());
      if (no == -1) {
          submitStatus = false;
          alert_h_wheel("您已经达到抽奖次数上限");
      }
      else if (no == -2) {
          submitStatus = false;
          alert_h_wheel("您未登录或者，登录超时，请重新从微信进入！");
          
      }
      else if (no == -3) {
          submitStatus = false;
          alert_h_wheel("对不起，活动还未开始，或者已经结束！", function () {
              location.href = "/vshop/default.aspx";
          });
      }
      else if (no == -4) {
          submitStatus = false;
          alert_h_wheel("您的积分不足，无法继续抽奖");
      }
      else if (no == -18) {
          submitStatus = false;
          alert_h_wheel("您已经中过奖了,请领奖！", function () {
              gotoResult();
          });
      }
      else if (no == -19) {
          submitStatus = false;
          alert_h_wheel("您已经中过奖了！", function () {
              location.href = "/vshop/default.aspx";
          });
      }
      else {
          var r = Math.random() * 100;
          if (r > 70)
              r = (r % 70) / 100;
          else
              r = r / 100;
          r = 0.35 - r;
          spinAngleStart = 8.744 * 1 + 8.744 / 12 * (11 - ((2 + no) % 12)) + r;
          spinTime = 0;
          startAngle = 0;
          spinTimeTotal = 5000;
          isRunning = true;
          rotateWheel();
          submitStatus = false;
      }
  }
  
  function rotateWheel() {
    spinTime += 30;
    if(spinTime >= spinTimeTotal) {
      stopRotateWheel();
      return;
    }
    var spinAngle = spinAngleStart - easeOut(spinTime, 0, spinAngleStart, spinTimeTotal);
    startAngle += (spinAngle * Math.PI / 180);
    drawRouletteWheel();
    spinTimeout = setTimeout('rotateWheel()', 30);
  }

  function stopRotateWheel() {
      clearTimeout(spinTimeout);
      var degrees = startAngle * 180 / Math.PI + 90 - 15;
      var arcd = arc * 180 / Math.PI;
      var index = Math.floor((360 - degrees % 360) / arcd);
      switch (index) {
          case 1:
              alert_h_wheel("三等奖", function () { gotoResult(); });
              break;
          case 3:
              alert_h_wheel("四等奖", function () { gotoResult(); });
              break;
          case 5:
              alert_h_wheel("五等奖", function () { gotoResult(); });
              break;
          case 7:
              alert_h_wheel("六等奖", function () { gotoResult(); });
              break;
          case 9:
              alert_h_wheel("一等奖", function () { gotoResult(); });
              break;
          case 11:
              alert_h_wheel("二等奖", function () { gotoResult(); });
              break;
          default:
              alert_h_wheel("继续努力", function () { location.reload(); });
              break;
      }
      isRunning = false;

  }

  function gotoResult() {
      window.location.href = "/Vshop/WinningResults.aspx?activityid=" + GetActivityid();
  
  }





  function easeOut(t, b, c, d) {
      var ts = (t /= d) * t;
      var tc = ts * t;
      return b + c * (tc + -3 * ts + 3 * t);
  }


function GetActivityid() {
    var activityid = window.location.search.substr(window.location.search.indexOf("=") + 1);
    if (activityid.indexOf("&") > 0)
        activityid = activityid.substr(0, activityid.indexOf("&"));
    return activityid;
}

function getPrize() {
    var no = 0;
    var activityid = GetActivityid();   
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "GetPrize", "activityid": activityid },
        async: false,
        success: function (resultData) {
            no = resultData.No;
        }
    });
    return no;
}


function bigwheelConfirm(title, content, ensureText, ensuredCallback) {
    var bigwheelConfirmCode = '<div class="modal fade" id="bigwheelConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
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
                        <button type="button" class="btn btn-danger">' + ensureText + '</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';
    if ($("#bigwheelConfirm").length != 0) {
        $('#bigwheelConfirm').remove();
    }
    $("body").append(bigwheelConfirmCode);
    $('#bigwheelConfirm').modal("show");
    $('#bigwheelConfirm button.btn-danger').click(function (event) {
        $('#bigwheelConfirm button.btn-danger').attr("disabled", "disabled");
        if (ensuredCallback) {
            $('#bigwheelConfirm').modal('hide');
            ensuredCallback();
        }
    });
}


function alert_h_wheel(content, ensuredCallback) {
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
                        <button type="button" class="btn btn-primary" data-dismiss="modal" aria-hidden="true"  style="width:100%;text-algin:center;border-radius: 0 0 5px 5px;" data-dismiss="modal">确认</button>\
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