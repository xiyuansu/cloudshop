function BFD(bfdinfo, bfdsrc) {
    window["_BFD"] = window["_BFD"] || {};
    _BFD.BFD_INFO = bfdinfo;
    _BFD.script = document.createElement("script");
    _BFD.script.type = 'text/javascript';
    _BFD.script.async = true;
    _BFD.script.charset = 'utf-8';
    switch (bfdsrc) {
        case 'default':
            _BFD.script.src = (('https:' == document.location.protocol ? 'https://ssl-static1' : 'http://static1') + '.baifendian.com/service/hishop/hishop_home.js');
            break;
        case 'usercenter':
            _BFD.script.src = (('https:' == document.location.protocol ? 'https://ssl-static1' : 'http://static1') + '.baifendian.com/service/hishop/hishop_member.js');
            break;
        case 'cart':
            _BFD.script.src = (('https:' == document.location.protocol ? 'https://ssl-static1' : 'http://static1') + '.baifendian.com/service/hishop/hishop_cart.js');
            break;
        case 'categroy':
            _BFD.script.src = (('https:' == document.location.protocol ? 'https://ssl-static1' : 'http://static1') + '.baifendian.com/service/hishop/hishop_list.js');
            break;
        case 'productdetail':
            _BFD.script.src = (('https:' == document.location.protocol ? 'https://ssl-static1' : 'http://static1') + '.baifendian.com/service/hishop/hishop_goods.js');
            break;
        case 'search':
            _BFD.script.src = (('https:' == document.location.protocol ? 'https://ssl-static1' : 'http://static1') + '.baifendian.com/service/hishop/hishop_search.js');
            break;
        case 'order':
            _BFD.script.src = (('https:' == document.location.protocol ? 'https://ssl-static1' : 'http://static1') + '.baifendian.com/service/hishop/hishop_order.js');
            break;
        case 'buy':
            _BFD.script.src = (('https:' == document.location.protocol ? 'https://ssl-static1' : 'http://static1') + '.baifendian.com/service/hishop/hishop_buy.js');
            break;
        default:
            _BFD.script.src = (('https:' == document.location.protocol ? 'https://ssl-static1' : 'http://static1') + '.baifendian.com/service/hishop/hishop_home.js');
            break;
    }
    document.getElementsByTagName("head")[0].appendChild(_BFD.script);
}

//取得虚拟目录地址
function getRootPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return (prePath + postPath);
}

//价格格式化

function changeTwoDecimal(x) {
    var f_x = parseFloat(x);
    if (isNaN(f_x)) {
        return 0.00;
        return false;
    }
    f_x = Math.round(f_x * 100) / 100;

    return f_x;
}


// [["卫衣","http://www.mrzero.cn/list-11.html"],["单鞋","http://www.baidu.com/html"]]

///获取类目信息
function GetcategroyDetail(isnav) {

    var cates;
    if (isnav) {
        cates = $(".category_nav_tt a:gt(0)");
    }
    else {
        cates = $(".details_nav a:gt(0)");
    }
    var s = "[";
    cates.each(function (index, item) {
        s += "[\"" + $(item).text() + "\",\"" + location.origin + $(item).attr("href") + "\"],"
    });
    if (s != "[")
        return eval(s.substr(0, s.length - 1) + "]");
}


function GetBFDCartitems() {
    var cartinfo = $(".bfdcartinfo");
    var s = "[";
    cartinfo.each(function (index, item) {
        s += "[" + $(item).text() + "],";
    });
    if (s != "[")
        return eval(s.substr(0, s.length - 1) + "]");
}