
$(document).ready(function (e) {
    if (!navigator.geolocation) {
        $("#btnlocation").css("display", "none");
        $("#regionPosition").width("100%");
        $("#btnlocation").click(function (e) {
            alert_h("浏览器不支持定位。");
        })
    }
    else {
        $("#btnlocation").click(function (e) {
            RequestLocation();
        })
        var regionWidth = $("#regionPosition").parent().width() - 24;
        $("#regionPosition").width(regionWidth);
    }
    
    var regionSelector = new vShop_RegionSelector('vshopRegion', function (address, code) {
        setRegion(address, code);
    }, $("#vshopRegion").html());

    var regionValue = $("#region").val();
    if (regionValue == undefined || regionValue == "" || regionValue == "0" && navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(ProcessGPSAddress, ShowError);
    }

});



function setRegion(address, code) {
    $("#address-check-btn").html(address);
    $('#region').val(code);
    if ($("#IsDeliveryScopeRegion").val() != undefined && $("#IsDeliveryScopeRegion").val() == "true") {
        SaveDeliveryScopRegion(code);
    }
}

function SaveDeliveryScopRegion(regionId) {
    $.ajax({
        url: "/Handler/GeneralHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "SaveDeliveryScop", regionId: regionId },
        success: function (resultData) {
            if (resultData.status != "true")
                alert(resultData.msg);
        }
    });
}

//请求定位
function RequestLocation() {
    navigator.geolocation.getCurrentPosition(ProcessGPSAddress, ShowError);
}

//处理GPS地址信息
function ProcessGPSAddress(position) {
    //获得GPS坐标
    var location = {};
    //location.lng = "114.21892734521";
    //location.lat = "29.575429778924";

    location.lng = position.coords.longitude;//经度
    location.lat = position.coords.latitude;//纬度

    //GPS坐标转换成百度坐标
    var locations = location.lng + "," + location.lat;
    RequestTransCoord(locations);
}

//GPS坐标转换
function RequestTransCoord(locations) {
    var transformurl = "http://api.map.baidu.com/geoconv/v1/?coords=" + locations + "&from=1&to=5&ak=7K4enq9kvQqGSdYz6lB2vefq";

    $.ajax({
        type: "GET",
        dataType: "jsonp",
        url: transformurl,
        success: function (json) {
            if (json.status == 0) {
                //获得转换坐标
                var locationed = json.result[0];
                RequestAddress(locationed);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert_h("地址转换失败！");
        }
    });
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
           // alert_h("地址获取失败！");
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

                $("#region").val(regionId);
                $("#address-check-btn").html(province + " " + city + " " + county);
                $("#address").val(formattedaddress);
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

    switch (error.code) {
        case error.PERMISSION_DENIED:         
            break;
        case error.POSITION_UNAVAILABLE:         
            break;
        case error.TIMEOUT:         
            break;
        case error.UNKNOWN_ERROR:          
            break;
    }
}
