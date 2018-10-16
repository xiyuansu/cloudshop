function IsNum(num) {
    var reNum = /^\d*$/;
    if (reNum.test(num)) {
        return true;
    }
    else {
        return false;
    }
}

///验证是否是价格---
function isPriceNumber(_keyword) {
    if (_keyword == "0" || _keyword == "0." || _keyword == "0.0" || _keyword == "0.00") {
        _keyword = "0"; return true;
    } else {
        var index = _keyword.indexOf("0");
        var length = _keyword.length;
        if (index == 0 && length > 1) {/*0开头的数字串*/
            var reg = /^[0]{1}[.]{1}[0-9]{1,2}$/;
            if (!reg.test(_keyword)) {
                return false;
            } else {
                return true;
            }
        } else {/*非0开头的数字*/
            var reg = /^[1-9]{1}[0-9]{0,10}[.]{0,1}[0-9]{0,2}$/;
            if (!reg.test(_keyword)) {
                return false;
            } else {
                return true;
            }
        }
        return false;
    }
}
///验证是否是价格---


//////-----公用异步请求-------/////////////////
function ajaxjson(posturl) {
    var redata = null;
    //var buttonText = $(obj).text();//记住按钮文字

    //发送异步请求
    $.ajax({
        type: "POST",
        url: posturl,
        dataType: "json",
        //data : {"id":id,"Amount":amount},
        async: false,//false代表只有在等待ajax执行完毕后才ajax后面的语句  
        beforeSend: function (XMLHttpRequest) {
            //$(obj).prop("disabled",true).text("请稍候...");//发送前动作
        },
        success: function (data, textStatus) {
            //if (data.status == 1) {}//1查询成功
            redata = data;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("查询出错：" + textStatus + ",提示：" + errorThrown);
        },
        complete: function (XMLHttpRequest, textStatus) {
            //$(obj).prop("disabled",false).text(buttonText);////请求完后动作
        },
        timeout: 20000  //20秒
    });

    return redata;
}

function ajaxhtml(posturl) {
    var redata = null;
    //var buttonText = $(obj).text();//记住按钮文字

    //发送异步请求
    $.ajax({
        type: "POST",
        url: posturl,
        dataType: "html",
        //data : {"id":id,"Amount":amount},
        async: false,//false代表只有在等待ajax执行完毕后才ajax后面的语句  
        beforeSend: function (XMLHttpRequest) {
            //$(obj).prop("disabled",true).text("请稍候...");//发送前动作
        },
        success: function (data, textStatus) {
            //if (data.status == 1) {}//1查询成功
            redata = data;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("查询出错：" + textStatus + ",提示：" + errorThrown);
        },
        complete: function (XMLHttpRequest, textStatus) {
            //$(obj).prop("disabled",false).text(buttonText);////请求完后动作
        },
        timeout: 20000  //20秒
    });

    return redata;
}
///////-----end--------------------

///---回车文本时提交响应按钮事件----
function ClickEnter(en, button) {
    var e = en || event;
    var currKey = e.keyCode || e.which || e.charCode;
    if (currKey == 13) {
        document.getElementById(button).focus();
    }
}