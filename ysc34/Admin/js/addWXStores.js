//var geocoder, geolocation, map, marker, searchRenderer, infoWin = null;

$(function () {
    initValidators();
    initMap();
})

function initValidators() {
    initValid(new InputValidator('ctl00_contentHolder_txtWxAddress', 2, 100, false, null, '微信门店地址不能为空长度必须为2-100个字符'));
    initValid(new InputValidator('ctl00_contentHolder_txtWXTelephone', 7, 20, false, '1\\d{10}$|^\\d{2,4}-\\d{7,8}(-\\d{2,4})?', '微信门店电话不能为空，请输入合法的电话或者手机号码'));
}

var map, searchService, marker, markers = [], infoWin = null;
var initMap = function () {
    var longitude = $("#ctl00_contentHolder_hfLongitude").val();
    var latitude = $("#ctl00_contentHolder_hfLatitude").val();
    var center = new qq.maps.LatLng(latitude, longitude);
    map = new qq.maps.Map(document.getElementById('container'), {
        center: center,
        zoom: 18
    });
    var scaleControl = new qq.maps.ScaleControl({
        align: qq.maps.ALIGN.BOTTOM_LEFT,
        margin: qq.maps.Size(85, 15),
        map: map
    });
    marker = new qq.maps.Marker({
        position: center,
        map: map,
        draggable: true
    });
    qq.maps.event.addListener(marker, 'dragend', function () {
        if (marker.getPosition()) {
            $("#ctl00_contentHolder_hfLongitude").val(marker.getPosition().getLng());
            $("#ctl00_contentHolder_hfLatitude").val(marker.getPosition().getLat());
        }
    });
    //调用Poi检索类
    searchService = new qq.maps.SearchService({
        //检索成功的回调函数
        complete: function (results) {
            //设置回调函数参数
            var pois = results.detail.pois;
            infoWin = new qq.maps.InfoWindow({
                map: map
            });
            var latlngBounds = new qq.maps.LatLngBounds();
            for (var i = 0, l = pois.length; i < l; i++) {
                var poi = pois[i];
                //扩展边界范围，用来包含搜索到的Poi点
                latlngBounds.extend(poi.latLng);
                (function (n) {
                    var marker = new qq.maps.Marker({
                        map: map
                    });
                    marker.setPosition(pois[n].latLng);
                    markers.push(marker);
                    qq.maps.event.addListener(marker, 'click', function () {
                        infoWin.open();
                        infoWin.setContent('<div style = "width:200px;padding:10px 0;">' + pois[n].address + '<div class="map-import-btn"><input type="button" class="btn btn-xs btn-primary" value="导入门店地址" onclick="chooseShopLoc(this);" address=' + pois[n].address + ' lat =' + pois[n].latLng.getLat() + '  lng =' + pois[n].latLng.getLng() + ' /></div></div>');
                        infoWin.setPosition(pois[n].latLng);
                    });
                })(i);
            }
            //调整地图视野
            map.fitBounds(latlngBounds);
        },
        //若服务请求失败，则运行以下函数
        error: function () {
            alert("出错了。");
        }
    });
}
//导入门店信息
function chooseShopLoc(t) {
    var address = $(t).attr("address");
    var lat = $(t).attr("lat");
    var lng = $(t).attr("lng");
    this.clearMarkers();
    var position = new qq.maps.LatLng(lat, lng);
    marker = new qq.maps.Marker({
        map: map,
        position: position,
        draggable: true
    });
    map.panTo(position);
    map.zoomTo(18);
    $("#ctl00_contentHolder_hfLongitude").val(lng);
    $("#ctl00_contentHolder_hfLatitude").val(lat);
    qq.maps.event.addListener(marker, 'dragend', function () {
        if (marker.getPosition()) {
            $("#ctl00_contentHolder_hfLongitude").val(marker.getPosition().getLng());
            $("#ctl00_contentHolder_hfLatitude").val(marker.getPosition().getLat());
        }
    });
    $("#ctl00_contentHolder_txtWxAddress").val(address);
    if (infoWin) {
        infoWin.close();
    }
    $("#map_des").hide();
}
////删除所有标记
function clearMarkers() {
    if (markers) {
        for (i in markers) {
            markers[i].setMap(null);
        }
        markers.length = 0;
    }
}
function getResult() {
    if (marker != null) marker.setMap(null);
    clearMarkers();
    if (infoWin) {
        infoWin.close();
    }
    var provinceCityArea = $("#regionSelectorName").val();
    var items = provinceCityArea.split(" ");
    provinceCityArea = items[0] + "," + items[1] + "," + items[2];
    var regions = items[0] + items[1] + items[2];
    var regionText = provinceCityArea;
    var poiText = regions + $("#ctl00_contentHolder_txtWxAddress").val();
    searchService.setLocation(regionText);
    searchService.search(poiText);
    $("#map_des").show();
}


//var initMap = function () {
//    if ($("#container").length == 0)
//        return;
//    var center = new soso.maps.LatLng(39.916527, 116.397128);
//    map = new soso.maps.Map(document.getElementById('container'), {
//        center: center,
//        zoomLevel: 13
//    });
//    geocoder = new soso.maps.Geocoder();
//    //缩放控件
//    var navControl = new soso.maps.NavigationControl({
//        align: soso.maps.ALIGN.TOP_LEFT,
//        margin: new soso.maps.Size(5, 15),
//        map: map
//    });

//    //根据客户端IP定位地图中心位置
//    geolocation = new soso.maps.Geolocation();
//    geolocation.position({}, function (results, status) {
//        if (status == soso.maps.GeolocationStatus.OK) {
//            map.setCenter(results.latLng);
//            if (marker != null) {
//                marker.setMap(null);
//            }
//            center = new soso.maps.LatLng(results.latLng.lat, results.latLng.lng);
//            marker = new soso.maps.Marker({
//                position: center,
//                draggable: true,
//                map: map
//            });
//            //拖放事件，获取坐标
//            soso.maps.Event.addListener(marker, 'dragend', function () {
//                if (marker.getPosition()) {
//                    $("#ctl00_contentHolder_hfLongitude").val(marker.getPosition().getLng());
//                    $("#ctl00_contentHolder_hfLatitude").val(marker.getPosition().getLat());
//                    //codeLatLng(marker.getPosition().getLat() + "," + marker.getPosition().getLng());
//                }
//            });
//        } else {
//            alert("检索没有结果，原因: " + status);
//        }
//    });
//    //}
//}
//function getResult() {
//    //清除当前所有marker
//    marker != null ? marker.setMap(null) : marker;
//    this.clearMarkers();
//    if (infoWin) {
//        infoWin.close();
//    }
//    //设置searchRequest   


//    var regionText = $("#ctl00_contentHolder_hfProvinceCityArea").val();
//    var poiText = $("#ctl00_contentHolder_txtWxAddress").val();

//    var searchRequest = {
//        keyword: poiText,
//        region: regionText
//    };
//    var searchService = new soso.maps.SearchService();
//    var markerOptions = {
//        clickable: true,
//        draggable: true
//    };

//    searchRenderer = new soso.maps.SearchRenderer({
//        panel: document.getElementById('infoDiv'),
//        map: map,
//        markerOptions: markerOptions
//    });
//    var icon = new soso.maps.MarkerImage(
//            "/Images/marker_red.png",
//            new soso.maps.Size(22, 34),
//            new soso.maps.Point(11, 34),
//            new soso.maps.Point(0, 0)
//    );

//    infoWin = new soso.maps.InfoWindow({ map: map });
//    searchService.setRenderer(searchRenderer);
//    soso.maps.Event.addListener(searchRenderer, 'item_clicked', function (event) {
//        infoWin.open('<div style = "width:200px;padding:10px 0;">' + event[0].address + '<div class="map-import-btn"><input type="button" class="btn btn-xs btn-primary" value="导入门店地址" onclick="chooseShopLoc(this);" address=' + event[0].address + ' /></div></div>', event[1]);
//        $("#Address").val(event[0].address);
//        codeAddress(event[0].address);
//    });
//    soso.maps.Event.bind(searchRenderer, 'item_mouseover', this, function (event) {
//        //console.log(event)
//        var panel = searchRenderer.getPanel();
//        event[1].setIcon(icon);
//        var index = event[2];
//        panel.getElementsByTagName('li')[index].style.color = 'BLUE';
//    });
//    soso.maps.Event.bind(searchRenderer, 'item_mouseout', this, function (event) {
//        event[1].setIcon(null);
//        var panel = searchRenderer.getPanel();
//        var index = event[2];
//        panel.getElementsByTagName('li')[index].style.color = 'BLACK';
//    });
//    soso.maps.Event.addListener(searchRenderer, 'pageIndex_changed', function (event) {
//        if (event > -1) {
//            infoWin.close();
//        }
//    });
//    searchService.search(searchRequest);
//    $("#infoDiv").show();
//    $("#map_des").show();
//}
//function clearMarkers(noMarker) {
//    if (searchRenderer) {
//        for (var i = 0; i < searchRenderer.getMarkers().length; i++) {
//            searchRenderer.getMarkers()[i].setMap(null);
//        }
//    }
//}

////根据点击选择的位置查询 省市区，经纬度
//function codeAddress(address) {
//    geocoder.geocode({ 'address': address }, function (results, status) {
//        if (status == soso.maps.GeocoderStatus.OK) {
//            if (results.location) {
//                $("#ctl00_contentHolder_hfLongitude").val(results.location.getLng());
//                $("#ctl00_contentHolder_hfLatitude").val(results.location.getLat());
//            }
//            console.log(results.addressComponents);
//            if (results.addressComponents) {
//                $("#ctl00_contentHolder_hfProvince").val(results.addressComponents.province);
//                $("#ctl00_contentHolder_hfCity").val(results.addressComponents.city);
//                $("#ctl00_contentHolder_hfDistrict").val(results.addressComponents.district);
//            }
//        } else {
//            alert("检索没有结果，原因: " + status);
//        }
//    });
//}
////根据坐标获取省市县事件
//function codeLatLng(input) {
//    marker != null ? marker.setMap(null) : marker;
//    var latlngStr = input.split(",", 2);
//    var lat = parseFloat(latlngStr[0]);
//    var lng = parseFloat(latlngStr[1]);
//    var latLng = new soso.maps.LatLng(lat, lng);
//    var info = new soso.maps.InfoWindow({ map: map });
//    geocoder.geocode({ 'location': latLng }, function (results, status) {
//        if (status == soso.maps.GeocoderStatus.OK) {
//            map.setCenter(results.location);
//            marker = new soso.maps.Marker({
//                map: map,
//                //draggable: true,
//                position: results.location
//            });
//            infoWin != null ? infoWin.close() : infoWin;
//            //拖放事件，获取坐标
//            /*soso.maps.Event.addListener(marker, 'dragend', function () {
//                if (marker.getPosition()) {
//                    codeLatLng(marker.getPosition().getLat() + "," + marker.getPosition().getLng());
//                }
//            });
//            infoWin = new soso.maps.InfoWindow({
//                map: map
//            });
//            infoWin.open(
//                '<div style="width:200px;padding-top:10px;">' + results.address + '</div>',
//                map.getCenter()
//            );*/
//        } else {
//            $.dialog.alert("检索没有结果，原因: " + status);
//        }
//    });
//}
////导入门店信息
//function chooseShopLoc(t) {
//    var address = $(t).attr("address");
//    this.clearMarkers();
//    codeAddress(address);
//    codeLatLng($("#ctl00_contentHolder_hfLatitude").val() + "," + $("#ctl00_contentHolder_hfLongitude").val());
//    $("#ctl00_contentHolder_txtWxAddress").val(address);
//    $("#infoDiv").hide();
//    $("#map_des").hide();
//}

function newdoSubmit() {
    if (!PageIsValid())
        return;


    if ($("#ctl00_contentHolder_hfLongitude").val() == "" || $("#ctl00_contentHolder_hfLatitude").val() == "") {
        alert("请搜索标注，定位地图");
        return false;
    }
    var regExp = new RegExp("^\\d{1,8}$");
    if ($("#ctl00_contentHolder_txtWXAvgPrice").val() != "" && !regExp.test($("#ctl00_contentHolder_txtWXAvgPrice").val())) {
        alert("人均价格(元)请输入大于零的整数，须如实填写，默认单位为人民币");
        return false;
    }
    var openTime = $("#ctl00_contentHolder_txtWXOpenTime").val();
    if (openTime == "") {

    } else if (openTime.indexOf('-') <= 0 && openTime != "") {
        alert("营业时间输入错误，请正确输入如，10:00-21:00");
        return false;
    } else {
        var firstTime = "2015-1-1 " + openTime.split('-')[0] + ":01";
        var secondTime = "2015-1-1 " + openTime.split('-')[1] + ":01";
        regExp = new RegExp("^2015-1-1 \\d{1,2}:\\d{1,2}:01$");
        if (!regExp.test(firstTime) || !regExp.test(secondTime)) {
            alert("营业时间输入错误，请正确输入如，10:00-21:00");
            return false;
        }
        var firstDate = Date.parse(firstTime);
        var secondDate = Date.parse(secondTime);
        if (isNaN(firstDate) || isNaN(secondDate)) {
            alert("营业时间输入错误，请正确输入如，10:00-21:00");
            return false;
        }
        if (firstDate >= secondDate) {
            alert("营业时间输入错误，请正确输入如，10:00-21:00");
            return false;
        }
    }


    var isUploadImage = false;
    $("div.upload-img-box input.hiddenImgSrc").each(function () {
        if ($(this).val().length > 0)
            isUploadImage = true;
    });
    if (!isUploadImage) {
        alert("请上传门店logo");
        return false;
    }

    getUploadImages();
    return true;


}

