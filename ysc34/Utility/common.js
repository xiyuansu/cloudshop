var Reg_int_non_negative = /^\d+$/;　　//非负整数（正整数 + 0） 
var Reg_int_positive = /^[0-9]*[1-9][0-9]*$/;　　//正整数 
var Reg_int_no_positive = /^((-\d+)|(0+))$/;　　//非正整数（负整数 + 0） 
var Reg_int_negativeg = /^-[0-9]*[1-9][0-9]*$/;　　//负整数 
var Reg_int = /^-?\d+$/;　　　　//整数 
var Reg_float_non_negative = /^\d+(\.\d+)?$/;　　//非负浮点数（正浮点数 + 0） 
var Reg_float_positive = /^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$/;　　//正浮点数
var Reg_float_no_positive = /^((-\d+(\.\d+)?)|(0+(\.0+)?))$/;　　//非正浮点数（负浮点数 + 0） 
var Reg_float_negative = /^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$/;　　//负浮点数
var Reg_float = /^(-?\d+)(\.\d+)?$/　　//浮点数 
var Reg_letter = /^[A-Za-z]+$/;　　//由26个英文字母组成的字符串 
var Reg_letter_upper = /^[A-Z]+$/;　　//由26个英文字母的大写组成的字符串 
var Reg_letter_lower = /^[a-z]+$/;　　//由26个英文字母的小写组成的字符串 
var Reg_letter_digital = /^[A-Za-z0-9]+$/;　　//由数字和26个英文字母组成的字符串
var Reg_letter_digital_underline = /^\w+$/;　　//由数字、26个英文字母或者下划线组成的字符串
var Reg_emal = /^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$/　　　　//email地址 
var Reg_url = /^[a-zA-z]+:\/\/(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$/;　　//url
var Reg_ipV4 = /^(\d+)\.(\d+)\.(\d+)\.(\d+)$/g
var Reg_mobbile = /^[1][3|4|5|6|7|8|9][0-9]{9}$/; //手机号码验证
var Reg_phoneWithArea = /^[0][1-9]{2,3}-[0-9]{5,10}$/;//是否是带区号的电话
var Reg_phoneRegNoArea = /^[1-9]{1}[0-9]{5,8}$/;//是否是不带区号的电话
var Reg_money = /^[0-9]+[\.][0-9]{0,2}$/;//是否是金额，两位小数
var Reg_letter_digital_zh = /^[0-9a-zA-Z\u4e00-\u9fa5]+$/;  //是否是由字符数字和中文组成
var Reg_chinese = /^[u4E00-u9FA5]+$/; //是否是中文
var Reg_contaisChinese = /.*[\u4e00-\u9fa5]+.*$/ //是否包含中文
var Reg_chinese_speicalchar = /[^\x00-\xff]/ig  //是否是中文和全角字符


String.prototype.IsDate = function () {
    var regDateTime = new RegExp("^(?:(?:([0-9]{4}(-|\/)(?:(?:0?[1,3-9]|1[0-2])(-|\/)(?:29|30)|((?:0?[13578]|1[02])(-|\/)31)))|([0-9]{4}(-|\/)(?:0?[1-9]|1[0-2])(-|\/)(?:0?[1-9]|1\\d|2[0-8]))|(((?:(\\d\\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|\/)0?2(-|\/)29))))$");
    return regDateTime.test(this);
}

String.prototype.IsCardNo = function () {
    var regIdCard = /^(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$)$/;
    return regIdCard.test(this);

}


function getParam(paramName) {
    paramValue = "";
    isFound = false;
    paramName = paramName.toLowerCase();
    var arrSource = this.location.search.substring(1, this.location.search.length).split("&");
    if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
        if (paramName == "returnurl") {
            var retIndex = this.location.search.toLowerCase().indexOf('returnurl=');
            if (retIndex > -1) {
                var returnUrl = decodeURIComponent(this.location.search.substring(retIndex + 10, this.location.search.length));
                if ((returnUrl.indexOf("http") != 0) && returnUrl != "" && returnUrl.indexOf(location.host.toLowerCase()) == 0) returnUrl = document.location.protocol + "//" + returnUrl;
                return returnUrl;
            }
        }
        i = 0;
        while (i < arrSource.length && !isFound) {
            if (arrSource[i].indexOf("=") > 0) {
                if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                    paramValue = arrSource[i].toLowerCase().split(paramName + "=")[1];
                    paramValue = arrSource[i].substr(paramName.length + 1, paramValue.length);
                    isFound = true;
                }
            }
            i++;
        }
    }
    return paramValue;
}

function getParamFromUrl(url, paramName) {
    paramValue = "";
    isFound = false;
    paramName = paramName.toLowerCase();
    var paramStartIndex = url.indexOf("?");
    if (paramStartIndex == -1 || paramStartIndex == url.length - 1)
        return "";
    var arrSource = url.substring(url.indexOf("?") + 1).split("&");
    if (paramName == "returnurl") {
        var retIndex = url.toLowerCase().indexOf('returnurl=');
        if (retIndex > -1) {
            var returnUrl = unescape(url.substring(retIndex + 10));
            if ((returnUrl.indexOf("http") != 0) && returnUrl != "" && returnUrl.indexOf(location.host.toLowerCase()) == 0) returnUrl = "http://" + returnUrl;
            return returnUrl;
        }
    }
    i = 0;
    while (i < arrSource.length && !isFound) {
        if (arrSource[i].indexOf("=") > 0) {
            if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                paramValue = arrSource[i].toLowerCase().split(paramName + "=")[1];
                paramValue = arrSource[i].substr(paramName.length + 1, paramValue.length);
                isFound = true;
            }
        }
        i++;
    }
    return paramValue;
}
///替换指定参数名的值为指定的参数值，如果不包含此参数返回URL+ 否则返回替换后的URL
function replaceParam(url, paramName, paramValue) {
    isFound = false;
    paramName = paramName.toLowerCase();
    var paramStartIndex = url.indexOf("?");
    if (paramStartIndex == -1 || paramStartIndex == url.length - 1) {
        if (paramStartIndex == url.length - 1) {
            return url + paramName + "=" + paramValue;
        }
        else {
            return url + "?" + paramName + "=" + paramValue;
        }
    }
    var arrSource = url.substring(url.indexOf("?") + 1).split("&");
    if (paramName == "returnurl") {
        var retIndex = url.toLowerCase().indexOf('returnurl=');
        if (retIndex > -1) {
            return url.substring(0, retIndex + 10) + paramValue;
        }
        else {
            return url + "&returnUrl=" + paramValue;
        }
    }
    i = 0;
    while (i < arrSource.length && !isFound) {
        if (arrSource[i].indexOf("=") > 0) {
            if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                arrSource[i] = arrSource[i].split("=")[0] + "=" + paramValue;
                isFound = true;
            }
        }
        i++;
    }
    if (isFound) {
        var newUrl = url.substring(0, url.indexOf("?"));
        for (var i = 0; i < arrSource.length; i++) {
            newUrl += (i == 0 ? "?" : "&") + arrSource[i];
        }
        return newUrl;
    }
    else {
        return url + "&" + paramName + "=" + paramValue;
    }
}

function getFullPath(path) {
    if (path == undefined || path == null) {
        return "";
    }
    var domain = document.location.protocol + "//" + window.location.host;
    domain = domain.toLowerCase();
    var host = window.location.host.toLowerCase();
    var temp = path;
    //如果不是http开头也不是https开关
    if (temp.toLowerCase().indexOf("http://") == -1 && temp.toLowerCase().indexOf("https://" == -1)) {
        if (temp.toLowerCase().indexOf(host) == -1) {
            if (temp.substr(0, 1) == "/") {
                temp = domain + temp;
            }
            else {
                temp = domain + "/" + temp;
            }
        }
        else {
            temp = document.location.protocol + "//" + temp;
        }
    }
    return temp;
}

String.prototype.Trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.LTrim = function () {
    return this.replace(/(^\s*)/g, "");
}
String.prototype.Rtrim = function () {
    return this.replace(/(\s*$)/g, "");
}

function alert_h(content, ensuredCallback) {
    var myConfirmCode = '<div class="modal" id="alert_h" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content">\
                      <div class="modal-header" >\
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>\
                        <h4 class="modal-title" id="myModalLabel" >操作提示</h4>\
                      </div>\
                      <div class="modal-body">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer" >\
                        <button type="button" class="btn btn-primary" data-dismiss="modal"  style="width:100%;text-algin:center;border-radius: 0 0 5px 5px; " data-dismiss="modal">确认</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';

    if ($("#alert_h").length != 0) {
        $('#alert_h').remove();
    }
    $("body").append(myConfirmCode);
    $('#alert_h').modal();



    $('#alert_h').off('hide.bs.modal').on('hide.bs.modal', function (e) {
        if (ensuredCallback)
            ensuredCallback();
    });
}



String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.ltrim = function () {
    return this.replace(/(^\s*)/g, "");
}
String.prototype.rtrim = function () {
    return this.replace(/(\s*$)/g, "");
}
//window.onerror=function(){return true;}

///获取指定重复次数的字符串
String.prototype.RepLetter = function (repTimes) {
    if (repTimes == undefined) {
        repTimes = 1;
    }
    var repStr = this;
    for (var i = 1; i <= repTimes; i++) {
        repStr += repStr;
    }
    return repStr;
}
///截取小数指定位数的小数位数
Number.prototype.SubDecimalDigits = function (digits) {
    if (digits == undefined) {
        digits = 2;
    }

    var numberStr = this.toString();//小数部分
    var intStr = "";//整数部分
    dotIndex = numberStr.lastIndexOf(".");
    //获取整数部分
    if (dotIndex > -1) {
        intStr = numberStr.substr(0, dotIndex);
    }
    else {
        intStr = numberStr;
    }
    //如果 位数 等于0则返回整数部分
    if (digits <= 0) {
        if (dotIndex > -1 && dotIndex > 0) {
            return parseFloat(numberStr.substring(dotIndex - 1));
        }
        else if (dotIndex == 0) {
            return 0;
        }
        else {
            return this;
        }
    }
    var decimalStr = "";
    //获取小数部分
    if (dotIndex > -1) {
        decimalStr = numberStr.substring(dotIndex + 1)
        if (decimalStr.length < digits) {
            decimalStr += "0".RepLetter(digits - decimalStr.length);
        }
        else {
            decimalStr = decimalStr.substr(0, digits);
        }
    }
    else {
        if (digits > 0) {
            decimalStr += "0".RepLetter(digits);
        }
    }
    return parseFloat(intStr + "." + decimalStr);
}

String.prototype.toFixed = function (d) {
    var newValue = parseFloat(this);
    if (isNaN(newValue)) {
        newValue = 0;
    }
    return newValue.toFixed(d);
};
//重新toFixed方法
Number.prototype.toFixed = function (d) {
    var s = this + "";
    if (!d) d = 0;
    if (s.indexOf(".") == -1) s += ".";
    s += new Array(d + 1).join("0");
    if (new RegExp("^(-|\\+)?(\\d+(\\.\\d{0," + (d + 1) + "})?)\\d*$").test(s)) {
        var s = "0" + RegExp.$2, pm = RegExp.$1, a = RegExp.$3.length, b = true;
        if (a == d + 2) {
            a = s.match(/\d/g);
            //不四舍五入了 bug  37380
            //if (parseInt(a[a.length - 1]) > 4) {
            //    for (var i = a.length - 2; i >= 0; i--) {
            //        a[i] = parseInt(a[i]) + 1;
            //        if (a[i] == 10) {
            //            a[i] = 0;
            //            b = i != 1;
            //        } else break;
            //    }
            //}
            s = a.join("").replace(new RegExp("(\\d+)(\\d{" + d + "})\\d$"), "$1.$2");
        }
        if (b) s = s.substr(1);
        return (pm + s).replace(/\.$/, "");
    }
    return this + "";
};

/*
由于js本身精度问题,加减乘除都有可能丢失精度,支持Number,String类型 
要注意的是乘法没有优先级,用括号解决 如 1+2*3 先算乘法  但 1.toAdd(2).toMul(3)会先算加法 变成(1.toAdd(2)).toMul(3)即可.
示例1   total+1+2+3+4+5  等价于 total.toAdd(1, 2, 3, 4, 5),(total * 100 - deposit * 100 - useBalance * 100) / 100 等价于  total.toSub(deposit, useBalance)
示例2   total = (total * 100 - pointPrice * 100) / 100  等价于   total.toSub(pointPrice);
示例3   total = (cartTotalPrice * 100 - couponPrice * 100) / 100 + tax 等价于 (cartTotalPrice.toSub(couponPrice)).toAdd(tax)
*/
function operation(a, array, op) {
    if (Number.isNaN(a)) {
        return 0;
    }
    if (array.length <= 0) {
        return parseFloat(a);
    }
    var start = 0;
    var end = array.length;
    var retVal = a;
    while (start < end) {
        var r2;
        r2 = array[start];
        if (!Number.isNaN(r2)) {
            switch (op) {
                case 'add':
                    retVal = accAdd(retVal, r2);
                    break;
                case 'subtract':
                    retVal = accSub(retVal, r2);
                    break;
                case 'multiply':
                    retVal = accMul(retVal, r2);
                    break;
                case 'divide':
                    retVal = accDiv(retVal, r2);
                    break;
            }

        }
        start++;
    }
    return retVal;
}
//加法
String.prototype.toAdd = function () {
    var newValue = parseFloat(this);
    if (isNaN(newValue)) {
        newValue = 0;
    }
    return operation(newValue, arguments, 'add');;
};
Number.prototype.toAdd = function () {
    var _this = this;
    return operation(_this, arguments, 'add');
}
function accAdd(arg1, arg2) {
    var r1, r2, m;
    try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
    try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
    m = Math.pow(10, Math.max(r1, r2))
    return (arg1.toMul(m) + arg2.toMul(m)).toDiv(m).toFixed(m);
}
//减法
String.prototype.toSub = function () {
    var newValue = parseFloat(this);
    if (isNaN(newValue)) {
        newValue = 0;
    }
    return operation(newValue, arguments, 'subtract');
};
Number.prototype.toSub = function () {
    var _this = this;
    return operation(_this, arguments, 'subtract');
}
function accSub(arg1, arg2) {
    var r1, r2, m, n;
    try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
    try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
    m = Math.pow(10, Math.max(r1, r2));
    //last modify by deeka 
    //动态控制精度长度 
    n = (r1 >= r2) ? r1 : r2;
    var v = ((arg1.toMul(m) - arg2.toMul(m)).toDiv(m));
    return v.toFixed(n);
}
//乘法
String.prototype.toMul = function () {
    var newValue = parseFloat(this);
    if (isNaN(newValue)) {
        newValue = 0;
    }
    return operation(newValue, arguments, 'multiply');
};
Number.prototype.toMul = function () {
    var _this = this;
    return operation(_this, arguments, 'multiply');
}
function accMul(arg1, arg2) {
    var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
    try { m += s1.split(".")[1].length } catch (e) { }
    try { m += s2.split(".")[1].length } catch (e) { }
    return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
}

//除法
String.prototype.toDiv = function () {
    var newValue = parseFloat(this);
    if (isNaN(newValue)) {
        newValue = 0;
    }
    return operation(newValue, arguments, 'divide');;
};
Number.prototype.toDiv = function () {
    var _this = this;
    return operation(_this, arguments, 'divide');
}
function accDiv(arg1, arg2) {
    var t1 = 0, t2 = 0, r1, r2;
    try { t1 = arg1.toString().split(".")[1].length } catch (e) { }
    try { t2 = arg2.toString().split(".")[1].length } catch (e) { }
    with (Math) {
        r1 = Number(arg1.toString().replace(".", ""))
        r2 = Number(arg2.toString().replace(".", ""))
        return (r1 / r2) * pow(10, t2 - t1);
    }
}

