/// <reference path="jquery-1.6.4.min.js" />



function vShop_RegionSelector(containerId, onSelected, defaultRegionText) {
    /// <param name="onSelected" type="function">选择地址后回调,包括两个参数，依次为址址和地址编码</param>

    var regionHandleUrl = '/Handler/RegionHandler.ashx';
    init();
    var address = '';
    var code = 0;


    function init() {
        if (!defaultRegionText)
            defaultRegionText = '请选择省市区';
        var text = '<div class="btn-group bmargin">\
        <button id="address-check-btn" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">' + defaultRegionText + '</button>\
        <ul name="province" class="dropdown-menu" role="menu"></ul>\
        <ul name="city" class="dropdown-menu hide" role="menu"></ul>\
        <ul name="district" class="dropdown-menu hide" role="menu"></ul>\
        </div>';

        $('#' + containerId).html(text);

        getRegin("province", 0, function (noSub) { bind(noSub); });
    }

    this.getRegin = function (regionType, parentRegionId, callback) {
        /// <param name="regionType" type="String">"province-省,city-市,district-区"</param>  
        var text = '';

        if (!parentRegionId) {
            parentRegionId = 0;
            address = '';
        }

        jQuery.ajax({
            type: "get",
            async: false,
            url: regionHandleUrl,
            data: { action: 'getregions', parentId: parentRegionId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {

                var noSub = false;

                if (data.Status == 'OK') {
                    $.each(data.Regions, function (i, province) {
                        text += '<li><a href="#" name="' + province.RegionId + '">' + province.RegionName + '</a></li>';
                    });
                    $('#' + containerId + ' ul[name="' + regionType + '"]').html(text);

                }
                else if (data.Status == 0) {
                    noSub = true;
                }

                callback(noSub);
            }
        });
    };

    function bind(noSub) {
        $('#' + containerId + ' ul li a').unbind('click');
        $('#' + containerId + ' ul li a').click(function () {
            var currentUl = $(this).parent().parent();
            var regionId = $(this).attr('name');
            var nextRegionUl = currentUl.next();
            var prevRegionUl = currentUl.prev();
            var nextRegionType = nextRegionUl ? $(nextRegionUl).attr('name') : '';

            address += $(this).html() + " ";
            if (!noSub && nextRegionType) {
                code = $(this).attr('name');
                getRegin(nextRegionType, regionId, function (noSub) {
                    currentUl.addClass('hide');
                    if (noSub) {
                        var first = currentUl.parent().find('ul').first();
                        $(first).removeClass('hide');
                        onSelected(address, code);
                        address = '';

                    }
                    else {
                        nextRegionUl && !noSub && $(nextRegionUl).removeClass('hide');
                        setTimeout(function () {
                            $(".btn-group").addClass('open');
                        }, 1);
                    }
                    bind();
                });
            }
            else {
                var first = currentUl.parent().find('ul').first();
                $(first).removeClass('hide');
                currentUl.addClass('hide');
                code = $(this).attr('name');
                onSelected(address, code);
                address = '';
            }
        });
    }
}


//自动定位脚本
$(document).ready(function () {
    //自动定位
    ProcessGPSAddress();

    if (!navigator.geolocation) {
        $("#btnlocation").css("display", "none");
    }
});

//请求定位
function RequestLocation() {
    navigator.geolocation.getCurrentPosition(ProcessGPSAddress, ShowError);
}

//处理GPS地址信息
function ProcessGPSAddress(position) {
    //获得GPRS坐标
    var location = {};
    location.lng = "114.21892734521";// position.coords.longitude;//经度
    location.lat = "29.575429778924";// position.coords.latitude;//纬度

    //GPRS坐标转换成百度坐标
    var locations = location.lng + "," + location.lat;
    RequestTransCoord(locations);
}

//GPRS坐标转换
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
           // alert_h("地址转换失败！");
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
            alert_h("地址获取失败！");
        }
    });
}

//尝试匹配
function MatcheAddress(address) {
    var regionHandleUrl = '/Handler/RegionHandler.ashx';
    var formattedaddress = address.formatted_address;
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

                alert_h("匹配完成！");
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
