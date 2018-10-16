
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
    }
    return paramValue;
}

$(document).ready(function () {
    var pageurl = document.location.href.toLowerCase().split("?")[0];//去除页面参数，以免后面的参数判断错误
    var sUserAgent = navigator.userAgent.toLowerCase();
    //app
    var IsApp = pageurl.indexOf("/appshop/") > -1;
    //vshop
    var IsWX = sUserAgent.match(/micromessenger/i) == "micromessenger";
    //pc 
    var isPc = (pageurl.indexOf("/vshop/") == -1 && pageurl.indexOf("/wapshop/") == -1 && pageurl.indexOf("/alioh/") == -1 && pageurl.indexOf("/appshop/") == -1);
    var storeId = parseInt(getParam("StoreId"));
    if (isNaN(storeId) || storeId < 0) {
        storeId = 0;
    }
    var pagetype = 0;
    var sourceId = 0;
    var productId = 0;
    var activityType = 0;

    //端口
    if (IsApp) {
        sourceId = 3;
    } else if (IsWX) {
        sourceId = 2;
    } else if (isPc) {
        sourceId = 1;
    } else {
        sourceId = 99;
    }
    //
    var laststr = pageurl.substring(pageurl.length - 1, pageurl.length);
    if (laststr == "/" || pageurl.indexOf("/default") > -1 | pageurl.indexOf("/storehome") > -1) {
        pagetype = 1;

    }
    else if (pageurl.indexOf("/storelist") > -1 && getParam("productId") == "" && StoreDefaultPage == "StoreList") {//多门店首页
        pagetype = 0;
    }
    else {
        if (pageurl.indexOf("/product_detail") > -1 || pageurl.indexOf("/productdetails") > -1 || pageurl.indexOf("/storeproductdetails")) {
            //普通商品详情页
            pagetype = 3;
            if (isPc) {
                productId = $("#hidden_productId").val();
            } else {
                productId = $("#hidden_SKUSubmitOrderProductId").val();
            }
            productId = productId == 0 ? parseInt(getParam("productId")) : productId;
            activityType = 1;
        }
        else if (pageurl.indexOf("/countdownproductsdetails") > -1 || pageurl.indexOf("/countdownstoreproductsdetails") > -1) {
            //限时购
            pagetype = 3;
            if (isPc)
                productId = $("#hidden_productId").val();
            else
                productId = $("#hidden_SKUSubmitOrderProductId").val();

            activityType = 2;
        }
        else if (pageurl.indexOf("/groupbuyproductdetails") > -1) {
            //团购
            pagetype = 3;
            if (isPc)
                productId = $("#hidden_productId").val();
            else
                productId = $("#hidden_SKUSubmitOrderProductId").val();
            //productId = $("#txtProductId").val();

            activityType = 3;
        }
        else if (IsWX && (pageurl.indexOf("/fightgroupactivitydetails") > -1 || pageurl.indexOf("/fightgroupactivitydetailssoon") > -1)) {
            //火拼团
            pagetype = 3;
            productId = $("#hidden_SKUSubmitOrderProductId").val();
            activityType = 4;
        }
        else if (pageurl.indexOf("/presaleproductdetails") > -1) {
            //预售
            pagetype = 3;
            if (isPc)
                productId = $("#hidden_productId").val();
            else
                productId = $("#hidden_SKUSubmitOrderProductId").val();

            activityType = 5;

        }
        else if (pageurl.indexOf("/productlist") > -1 || pageurl.indexOf("/subcategory") > 1 || pageurl.indexOf("/category") > -1) {
            //商品列表页
            pagetype = 2;
        } else {
            pagetype = 99;
        }
    }

    $.ajax({
        url: "/API/TrafficStatistics.ashx",
        type: "post",
        dataType: "json",
        timeout: 10000,
        data: {
            pagetype: pagetype,
            sourceId: sourceId,
            productId: productId,
            activityType: activityType,
            storeId: storeId
        },
        async: true,
        success: function (data) {
        }
    });
});