if (qqMapAPIKey == undefined || qqMapAPIKey == null || qqMapAPIKey.length == 0 || qqMapAPIKey == undefined || qqMapAPIKey == "") {
    qqMapAPIKey = "OB4BZ-D4W3U-B7VVO-4PJWW-6TKDJ-WPB77";
}
$(document).ready(function (e) {
    var regionValue = parseInt($("#hidRegionId").val());
    if ((isNaN(regionValue) || regionValue == 0) && navigator.geolocation) {
        var geolocation = new qq.maps.Geolocation(qqMapAPIKey, "myapp1");
        if (geolocation) {
            geolocation.getLocation(ProcessGPSAddress, ShowError)
        }
        //        navigator.geolocation.getCurrentPosition(ProcessGPSAddress, ShowError);
    }
    else {
        GetProductFreight(regionValue);
    }

});

function GetProductFreight(regionId) {
    var quantity = 1;
    if ($("#buyAmount").length != 0) {
        quantity = parseInt($("#buyAmount").val());
        if (isNaN(quantity) || quantity <= 0) {
            quantity = 1;
        }
    }
    //重新计算运费
    var producttId = 0;
    if ($("#hidden_productId").length > 0) {
        productId = parseInt($("#hidden_productId").val());
    }
    if (isNaN(productId)) {
        productId = parseInt($("#hidden_SKUSubmitOrderProductId").val());
    }
    if (isNaN(productId) || productId <= 0) {
        productId = parseInt(getParam("productId"));
    }
    if (isNaN(productId)) {
        productId = 0;
    }
    var skuId = "";
    if ($("#hidden_SKUSubmitOrderProductId").length > 0) {
        skuId = $("#hidden_SKUSubmitOrderProductId").val();
    }
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "GetProductFreight", regionId: regionId, productId: productId, SkuId: skuId, quantity: quantity },
        success: function (resultData) {
            if (resultData.Status == "OK" && !isNaN(parseFloat(resultData.Freight)) && parseFloat(resultData.Freight) > 0) {
                $("#labProductFreight").html(resultData.Freight);
            }
            else {
                $("#labProductFreight").parent().html("免运费");
            }
        }
    });
}

function SaveDeliveryScopRegion(regionId) {
    var quantity = 1;
    if ($("#buyAmount").length != 0) {
        quantity = parseInt($("#buyAmount").val());
        if (isNaN(quantity) || quantity <= 0) {
            quantity = 1;
        }
    }
    $.ajax({
        url: "/Handler/GeneralHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "SaveDeliveryScop", regionId: regionId },
        success: function (resultData) {
            if (resultData.status != "true")
                alert("保存配送地区错误：" + resultData.msg);
            else {
                GetProductFreight(regionId);
            }
        }
    });
}

//请求定位
function RequestLocation() {
    navigator.geolocation.getCurrentPosition(ProcessGPSAddress, ShowError);
}

//处理GPS地址信息
function ProcessGPSAddress(position) {
    var quantity = 1;
    if ($("#buyAmount").length != 0) {
        quantity = parseInt($("#buyAmount").val());
        if (isNaN(quantity) || quantity <= 0) {
            quantity = 1;
        }
    }
    //获得GPS坐标
    var location = {};
    //location.lng = "114.21892734521";
    //location.lat = "29.575429778924";
    location.lat = position.lat;
    location.lng = position.lng;
    // location.lng = position.coords.longitude;//经度
    // location.lat = position.coords.latitude;//纬度

    //GPS坐标转换成百度坐标
    var locations = location.lat + "," + location.lng;

    //重新计算运费
    var producttId = 0;
    if ($("#hidden_productId").length > 0) {
        productId = parseInt($("#hidden_productId").val());
    }
    if (isNaN(productId)) {
        productId = parseInt($("#hidden_SKUSubmitOrderProductId").val());
    }
    if (isNaN(productId) || productId <= 0) {
        productId = parseInt(getParam("productId"));
    }
    if (isNaN(productId)) {
        productId = 0;
    }
    var skuId = "";
    if ($("#hidden_SKUSubmitOrderProductId").length > 0) {
        skuId = $("#hidden_SKUSubmitOrderProductId").val();
    }
    $.ajax({
        type: "get",
        async: false,
        url: '/api/VshopProcess.ashx',
        data: { action: 'GetProductFreightOfLatLng', locations: locations, productId: productId, SkuId: skuId, quantity: quantity },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (resultData) {
            if (resultData.Status == "OK" && !isNaN(parseFloat(resultData.Freight)) && parseFloat(resultData.Freight) > 0) {
                $("#labProductFreight").html(resultData.Freight);
            }
            else {
                $("#labProductFreight").parent().html("免运费");
            }
        }
    });
    //  RequestTransCoord(locations);
}

//GPS坐标转换
function RequestTransCoord(locations) {


    var transformurl = "https://apis.map.qq.com/ws/geocoder/v1/?key=" + qqMapAPIKey + "&get_poi=0&location=" + locations

    $.ajax({
        type: "GET",
        dataType: "jsonp",
        url: transformurl,
        success: function (json) {
            if (json.status == 0) {
                //获得转换坐标
                var locationObj = json.result;
                QQMapRequestAddess(locationObj);

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

           // alert_h("地址转换失败！" + XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus);
        }
    });
}

function QQMapRequestAddess(locationObj) {
    var address = {};
    address.formatted_address = locationObj.result.address;//结构化地址信息(北京市海淀区中关村大街27号1101-08室)
    address.country = locationObj.result.address_component.nation;//(中国)
    address.province = locationObj.address_component.province;//省(北京市)
    address.city = locationObj.address_component.city;//城市(北京市)
    address.district = locationObj.address_component.district;//区县名(海淀区)
    address.street = locationObj.address_component.street;//街道名(中关村大街)
    address.street_number = locationObj.address_component.street_number;//街道门牌号(27号1101-08室)
    address.country_code = locationObj.ad_info.adcode;//国家code(0)

    //尝试匹配
    MatcheAddress(address);
}

//获得当前详细地址信息
function RequestAddress(locationed) {
    var latlon = locationed.y + ',' + locationed.x;
    var url = "http://api.map.baidu.com/geocoder/v2/?ak=7K4enq9kvQqGSdYz6lB2vefq&location=" + latlon + "&output=json&pois=0";

    $.ajax({
        type: "GET",
        dataType: "jsonp",
        url: url,
        success: function (json) {
            if (json.status == 0) {
                //获得详细地址
                var address = {};
                address.formatted_address = json.result.formatted_address;//结构化地址信息(北京市海淀区中关村大街27号1101-08室)
                address.country = json.result.addressComponent.country;//(中国)
                address.province = json.result.addressComponent.province;//省(北京市)
                address.city = json.result.addressComponent.city;//城市(北京市)
                address.district = json.result.addressComponent.district;//区县名(海淀区)
                address.street = json.result.addressComponent.street;//街道名(中关村大街)
                address.street_number = json.result.addressComponent.street_number;//街道门牌号(27号1101-08室)
                address.country_code = json.result.addressComponent.country_code;//国家code(0)

                //尝试匹配
                MatcheAddress(address);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert_h("地址获取失败！");
        }
    });
}

//尝试匹配
function MatcheAddress(address) {
    var regionHandleUrl = '/Handler/RegionHandler.ashx';
    var formattedaddress = address.street + address.street_number;
    var county = address.district;//区县名
    var city = address.city;//城市
    var province = address.province;//省
    $.ajax({
        type: "get",
        async: false,
        url: regionHandleUrl,
        data: { action: 'getregionid', county: county, city: city, province: province },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.Status == '0') {
                var regionId = data.RegionId;
                SaveDeliveryScopRegion(regionId);
            }
            else {
                alert_h("匹配失败！");
            }
        }
    });
}

//定位错误
function ShowError(error) {

    //switch (error.code) {
    //    case error.PERMISSION_DENIED:
    //        alert_h("定位失败,用户拒绝请求地理定位");
    //        break;
    //    case error.POSITION_UNAVAILABLE:
    //        alert_h("定位失败,位置信息是不可用");
    //        break;
    //    case error.TIMEOUT:
    //        alert_h("定位失败,请求获取用户位置超时");
    //        break;
    //    case error.UNKNOWN_ERROR:
    //        alert_h("定位失败,定位系统失效");
    //        break;
    //}
}
