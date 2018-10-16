
//循环id
var vGroupBuyIntervalId = 0;

function loadSessionid(sessionid) {
    if (sessionid != "") {
        loadIframeURL("/AppShop/AppLogin.aspx?sessionid=" + sessionid);
    }
    else
        loadIframeURL("hishop://webLogin/openLogin/null");

}

function openLogin(ret, sessionId) {
    if (ret == 0) {
        loadIframeURL("/AppShop/AppLogin.aspx?sessionid=" + sessionId)
        document.location.reload();
    }
}

function setBuy(text) {
    $("#buyButtonNew").text(text);
    setTimeout(function () {
        $("#buyButton").unbind().text(text);
    }, 1000);

}

var pageLoadTime;
var passedSeconds = 0;

function GetRTime() {
    var d;
    var h;
    var m;
    var s;
    var url = document.location.href.toLowerCase();
    var type = "团购";
    if (url.indexOf("countdownproductsdetails.aspx") > -1 || url.indexOf("countdownproduct_detail") > -1) {
        type = "限时抢购";
    }

    var startVal = document.getElementById("startTime").value.replace("-", "/");
    var endVal = document.getElementById("endTime").value.replace("-", "/");
    /*兼容IOS浏览器*/
    var startValArr = startVal.split(/[- : \/]/);
    var endValArr = endVal.split(/[- : \/]/);
    var startTime = new Date(startValArr[0], startValArr[1] - 1, startValArr[2], startValArr[3], startValArr[4], startValArr[5]);
    var endTime = new Date(endValArr[0], endValArr[1] - 1, endValArr[2], endValArr[3], endValArr[4], endValArr[5]);
    var nowTimeVal = $('#nowTime').val().replace("-", "/");
    var nowTimeValArr = nowTimeVal.split(/[- : \/]/);
    var nowTime = new Date(nowTimeValArr[0], nowTimeValArr[1] - 1, nowTimeValArr[2], nowTimeValArr[3], nowTimeValArr[4], nowTimeValArr[5]);
    /*兼容IOS浏览器*/
    nowTime.setSeconds(nowTime.getSeconds() + passedSeconds);
    passedSeconds++;
    var now_startTime = nowTime.getTime() - startTime.getTime();    //当前时间 减去开始时间
    var s_nTime = startTime.getTime() - nowTime.getTime();          //开始时间减去当前时间
    var start_endTime = endTime.getTime() - startTime.getTime();    //结束时间减去开始时间
    var now_endTime = endTime.getTime() - nowTime.getTime();     //结束时间减去当前时间
    var now_pTime = nowTime.getTime() - pageLoadTime;               //当前时间减去页面刷新时间
    var p_sTime = startTime.getTime() - pageLoadTime;               //开始时间减去页面刷新时间
    var wid = now_startTime / start_endTime * 100;                    //开始后离结束的时间比
    var wid1 = now_pTime / p_sTime * 100;                             //未开始离开始的时间比
    //var tuan_button = document.getElementById("buyButton");  
    var tuan_time = document.getElementById("tuan_time");
    function docu() {
        document.getElementById("t_d").innerHTML = d + "天";
        document.getElementById("t_h").innerHTML = h + "时";
        document.getElementById("t_m").innerHTML = m + "分";
        document.getElementById("t_s").innerHTML = s + "秒";
    }
    if (pageLoadTime == null) {
        pageLoadTime = nowTime;
    }
    if (100 >= wid1 >= 0 && wid < 0) {
        d = Math.floor(Math.abs(now_startTime) / 1000 / 60 / 60 / 24);
        h = Math.floor(Math.abs(now_startTime) / 1000 / 60 / 60 % 24);
        m = Math.floor(Math.abs(now_startTime) / 1000 / 60 % 60);
        s = Math.floor(Math.abs(now_startTime) / 1000 % 60);
        docu();
        tuan_time.innerHTML = type + "开始时间：";  
        setBuy("即将开始");
        $("#buyButtonNew").text("即将开始").css("background-color", "#23b93a").css("border-color", "#23b93a");
        //$(".operbtns").hide();
    }
    if (wid1 > 100 || wid1 < 0) {
        if (wid >= 0 && wid < 70) {
            d = Math.floor(now_endTime / 1000 / 60 / 60 / 24);
            h = Math.floor(now_endTime / 1000 / 60 / 60 % 24);
            m = Math.floor(now_endTime / 1000 / 60 % 60);
            s = Math.floor(now_endTime / 1000 % 60);
            docu();
            tuan_time.innerHTML = type + "结束时间：";          
            //tuan_button.disabled = false;
        } else if (wid >= 70 && wid < 90) {
            d = Math.floor(now_endTime / 1000 / 60 / 60 / 24);
            h = Math.floor(now_endTime / 1000 / 60 / 60 % 24);
            m = Math.floor(now_endTime / 1000 / 60 % 60);
            s = Math.floor(now_endTime / 1000 % 60);
            docu();
            tuan_time.innerHTML = type + "结束时间：";     
            //tuan_button.disabled = false;
        } else if (wid >= 90 && wid <= 100) {
            d = Math.floor(now_endTime / 1000 / 60 / 60 / 24);
            h = Math.floor(now_endTime / 1000 / 60 / 60 % 24);
            m = Math.floor(now_endTime / 1000 / 60 % 60);
            s = Math.floor(now_endTime / 1000 % 60);
            docu();
            tuan_time.innerHTML = type + "结束时间：";         
            //tuan_button.disabled = false;
        }

        if (wid > 100) {            
            tuan_time.innerHTML = type + "已结束!";
            setBuy("已结束");
            //tuan_button.disabled = true;
            if (vGroupBuyIntervalId > 0) {
                clearInterval(vGroupBuyIntervalId);
                $(tuan_time).nextAll().hide();
            }
        }
    }

}
