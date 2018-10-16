var appId;
var timestamp;
var nonceStr;
var signature;
var title;
var desc;
var imgUrl;
var link;
var sUserAgent = navigator.userAgent.toLowerCase();
var bIsWX = sUserAgent.match(/micromessenger/i) == "micromessenger";

$(document).ready(function () {
    if (bIsWX) {
        title = $("#hdTitle").val();
        desc = $("#hdDesc").val();
        imgUrl = $("#hdImgUrl").val();
        link = $("#hdLink").val();
        $.ajax({
            url: "/API/VshopProcess.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "GetWXShareInfo" },
            success: function (resultData) {
                appId = $("#hdAppId").val();
                timestamp = resultData.timestamp;
                nonceStr = resultData.noncestr;
                signature = resultData.signature;
                //alert("appid," + appId);
                //alert("timestamp," + timestamp);
                //alert("nonceStr," + nonceStr);
                //alert("signature," + signature);       
                wx.config({
                    debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                    appId: appId, // 必填，公众号的唯一标识
                    timestamp: timestamp, // 必填，生成签名的时间戳
                    nonceStr: nonceStr, // 必填，生成签名的随机串
                    signature: signature,// 必填，签名，见附录1
                    jsApiList: ['onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
                });
            },
            error: function (xmlHttpRequest, error) {
                //alert(error);
            }
        });
    }
});

if (bIsWX) {
    wx.ready(function () {
        wx.checkJsApi({
            jsApiList: ['onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo'],
            success: function (res) {

                if (res.checkResult.onMenuShareTimeline)   //朋友圈
                {

                }
                if (!res.checkResult.onMenuShareAppMessage)  //朋友
                {
                }
                if (res.checkResult.onMenuShareQQ) {

                }
                if (res.checkResult.onMenuShareWeibo) {

                }
            }
        });

        wx.onMenuShareTimeline({
            title: $("#hdTitle").val(), // 分享标题
            link: link, // 分享链接
            imgUrl: $("#hdImgUrl").val(), // 分享图标
            success: function () {
                alert_h("分享成功!")
            },
            cancel: function () {
                // 用户取消分享后执行的回调函数
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus);
            }
        });

        wx.onMenuShareQQ({
            title: title, // 分享标题
            desc: $("#hdDesc").val(), // 分享描述
            link: link, // 分享链接
            imgUrl: $("#hdImgUrl").val(), // 分享图标
            success: function () {
                alert_h("分享成功!")
            },
            cancel: function () {
                // 用户取消分享后执行的回调函数
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus);
            }
        });

        wx.onMenuShareWeibo({
            title: title, // 分享标题
            desc: desc, // 分享描述
            link: link, // 分享链接
            imgUrl: $("#hdImgUrl").val(), // 分享图标
            success: function () {
                alert_h("分享成功!")
            },
            cancel: function () {
                // 用户取消分享后执行的回调函数
            }
        });

        wx.onMenuShareAppMessage({
            title: title, // 分享标题
            desc: desc, // 分享描述
            link: link, // 分享链接
            imgUrl: $("#hdImgUrl").val(), // 分享图标
            type: 'link', // 分享类型,music、video或link，不填默认为link
            dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
            success: function () {
                alert_h("分享成功!");
            },
            cancel: function () {
                // 用户取消分享后执行的回调函数
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus);
            }
        });
    });

    wx.error(function (res) {
        // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。
    });
}
