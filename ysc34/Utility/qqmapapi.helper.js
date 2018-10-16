if (qqMapAPIKey == null || qqMapAPIKey.length == 0 || qqMapAPIKey == undefined || qqMapAPIKey == "") {
    qqMapAPIKey = "SYJBZ-DSLR3-IWX3Q-3XNTM-ELURH-23FTP";
}

function AddressToLatLng(address) {
    var latLng = "";
    var transformurl = "https://apis.map.qq.com/ws/geocoder/v1/?address=" + address + "&key=" + qqMapAPIKey;
    var address = {};
    $.ajax({
        type: "GET",
        dataType: "json",
        url: transformurl,
        success: function (json) {
            if (json.status == 0) {
                //获得转换坐标
                var locationObj = json.result;
                latLng = locationObj.lng + "," + locationObj.lat;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
           // alert_h("地址转换失败！");
        }
    });
}

//将经纬度坐标转换成详细地址
function LatLngToAddress(latLng) {
    var transformurl = "https://apis.map.qq.com/ws/geocoder/v1/?key=" + qqMapAPIKey + "&get_poi=0&location=" + latLng
    var address = {};
    $.ajax({
        type: "GET",
        dataType: "json",
        url: transformurl,
        success: function (json) {
            if (json.status == 0) {
                //获得转换坐标
                var locationObj = json.result;
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
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
           // alert_h("地址转换失败！");
        }
    });
    return address;
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

//获取一个地点坐标至另外（一个或者多个)地坐标之间的距离，多个坐标之间使用;分隔,经度纬度之间使用,分隔
//返回一个距离数组
function GetDistance(fromLatLng, toLatLngs) {
    var distance = new Array();
    //to的格式： 39.996060,116.353455;39.949227,116.394310
    var transformurl = "https://apis.map.qq.com/ws/distance/v1/?mode=walking&from=" + fromLatLng + "&to=" + toLatLngs + "&key=" + qqMapAPIKey;

    $.ajax({
        type: "GET",
        dataType: "json",
        url: transformurl,
        success: function (json) {
            if (json.status == 0) {
                //获得转换坐标
                var distance = new Array(json.result.elements.length);
                $.each(function (index) {
                    distance[index] = this.distance;
                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
           // alert_h("地址转换失败！");
        }
    });
    return distance;
}