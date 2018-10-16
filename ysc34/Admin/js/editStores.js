var geocoder, geolocation, map, marker, searchRenderer, infoWin = null;

$(function () {
    var copenVstore = parseInt($("#ctl00_contentHolder_hfOpenVstore").val());
    if (copenVstore == 0) {

    }
    else {
        initMap();
    }


    hidWXStore();
})


function hidWXStore() {
    if ($("#ctl00_contentHolder_radIbeacon_1:checked").length > 0) {
        $("#ulWXStore li:gt(0)").hide();
    }

    $("#ctl00_contentHolder_radIbeacon_0").click(function () {
        if (doSubmit()) {
            $("#ulWXStore li:gt(0)").show();
            $("#ctl00_contentHolder_txtWXBranchName").val($("#ctl00_contentHolder_txtStoresName").val());
            $("#ctl00_contentHolder_txtWXBusinessName").val($("#ctl00_contentHolder_hfSiteName").val());
            var provincename = $("#provincename").text();
            var cityname = $("#cityname").text();
            var areaname = $("#areaname").text();
            var address = $("#ctl00_contentHolder_txtAddress").val();
            $("#ctl00_contentHolder_txtWxAddress").val(provincename + cityname + areaname + address);
            var tel = $("#ctl00_contentHolder_txtTel").val();
            $("#ctl00_contentHolder_txtWXTelephone").val(tel);
            $("#ctl00_contentHolder_js_search_pos").trigger("click");

            //微信验证
            $("#ctl00_contentHolder_txtWxAddress,#ctl00_contentHolder_txtWXBusinessName,#ctl00_contentHolder_txtWXTelephone").unbind();
            initValid(new InputValidator('ctl00_contentHolder_txtWxAddress', 2, 100, false, null, '微信门店地址不能为空长度必须为2-100个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtWXBusinessName', 2, 50, false, null, '微信门店名长度必须为2-50个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtWXTelephone', 7, 20, false, '1\\d{10}$|^\\d{2,4}-\\d{7,8}(-\\d{2,4})?', '微信门店电话不能为空，请输入合法的电话或者手机号码'));
        } else {
            $("#ctl00_contentHolder_radIbeacon_1").trigger("click");
        }
    });
    $("#ctl00_contentHolder_radIbeacon_1").click(function () {
        $("#ulWXStore li:gt(0)").hide();
    });
}



var initMap = function () {
    var longitude = $("#ctl00_contentHolder_hfLongitude").val();
    var latitude = $("#ctl00_contentHolder_hfLatitude").val();
    var center = new soso.maps.LatLng(longitude, latitude);
    map = new soso.maps.Map(document.getElementById('container'), {
        center: center,
        zoomLevel: 13
    });
    geocoder = new soso.maps.Geocoder();
    //缩放控件
    var navControl = new soso.maps.NavigationControl({
        align: soso.maps.ALIGN.TOP_LEFT,
        margin: new soso.maps.Size(5, 15),
        map: map
    });

    //根据客户端IP定位地图中心位置
    geolocation = new soso.maps.Geolocation();
    geolocation.position({}, function (results, status) {
        if (status == soso.maps.GeolocationStatus.OK) {
            map.setCenter(results.latLng);
            if (marker != null) {
                marker.setMap(null);
            }
            center = new soso.maps.LatLng(results.latLng.lat, results.latLng.lng);
            marker = new soso.maps.Marker({
                position: center,
                draggable: true,
                map: map
            });
            //拖放事件，获取坐标
            soso.maps.Event.addListener(marker, 'dragend', function () {
                if (marker.getPosition()) {
                    $("#ctl00_contentHolder_hfLongitude").val(marker.getPosition().getLng());
                    $("#ctl00_contentHolder_hfLatitude").val(marker.getPosition().getLat());
                    //codeLatLng(marker.getPosition().getLat() + "," + marker.getPosition().getLng());
                }
            });

        } else {
            alert("检索没有结果，原因: " + status);
        }
    });
    //}
}
function getResult() {
    //清除当前所有marker
    marker != null ? marker.setMap(null) : marker;
    this.clearMarkers();
    if (infoWin) {
        infoWin.close();
    }
    //设置searchRequest
    var provincename = $("#provincename").text();
    var cityname = $("#cityname").text();
    var areaname = $("#areaname").text();

    var poiText = document.getElementById("ctl00_contentHolder_txtWxAddress").value;
    var regionText = provincename + "," + cityname + "," + areaname;
    if (poiText.indexOf(areaname) < 0) {
        poiText = areaname + poiText;
    }
    if (poiText.indexOf(cityname) < 0) {
        poiText = cityname + poiText;
    }
    if (poiText.indexOf(provincename) < 0) {
        poiText = provincename + poiText;
    }
    var searchRequest = {
        keyword: poiText,
        region: regionText
    };
    var searchService = new soso.maps.SearchService();
    var markerOptions = {
        clickable: true,
        draggable: true
    };

    searchRenderer = new soso.maps.SearchRenderer({
        panel: document.getElementById('infoDiv'),
        map: map,
        markerOptions: markerOptions
    });
    var icon = new soso.maps.MarkerImage(
            "/Images/marker_red.png",
            new soso.maps.Size(22, 34),
            new soso.maps.Point(11, 34),
            new soso.maps.Point(0, 0)
    );

    infoWin = new soso.maps.InfoWindow({ map: map });
    searchService.setRenderer(searchRenderer);
    soso.maps.Event.addListener(searchRenderer, 'item_clicked', function (event) {
        infoWin.open('<div style = "width:200px;padding:10px 0;">' + event[0].address + '<div class="map-import-btn"><input type="button" class="btn btn-xs btn-primary" value="导入门店地址" onclick="chooseShopLoc(this);" address=' + event[0].address + ' /></div></div>', event[1]);
        $("#Address").val(event[0].address);
        codeAddress(event[0].address);
    });
    soso.maps.Event.bind(searchRenderer, 'item_mouseover', this, function (event) {
        //console.log(event)
        var panel = searchRenderer.getPanel();
        event[1].setIcon(icon);
        var index = event[2];
        panel.getElementsByTagName('li')[index].style.color = 'BLUE';
    });
    soso.maps.Event.bind(searchRenderer, 'item_mouseout', this, function (event) {
        event[1].setIcon(null);
        var panel = searchRenderer.getPanel();
        var index = event[2];
        panel.getElementsByTagName('li')[index].style.color = 'BLACK';
    });
    soso.maps.Event.addListener(searchRenderer, 'pageIndex_changed', function (event) {
        if (event > -1) {
            infoWin.close();
        }
    });
    searchService.search(searchRequest);
    $("#infoDiv").show();
    $("#map_des").show();
}
function clearMarkers(noMarker) {
    if (searchRenderer) {
        for (var i = 0; i < searchRenderer.getMarkers().length; i++) {
            searchRenderer.getMarkers()[i].setMap(null);
        }
    }
}

//根据点击选择的位置查询 省市区，经纬度
function codeAddress(address) {
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == soso.maps.GeocoderStatus.OK) {
            if (results.location) {
                $("#ctl00_contentHolder_hfLongitude").val(results.location.getLng());
                $("#ctl00_contentHolder_hfLatitude").val(results.location.getLat());
            }
            //console.log(results.addressComponents);
            if (results.addressComponents) {
                $("#ctl00_contentHolder_hfProvince").val(results.addressComponents.province);
                $("#ctl00_contentHolder_hfCity").val(results.addressComponents.city);
                $("#ctl00_contentHolder_hfDistrict").val(results.addressComponents.district);
            }
        } else {
            alert("检索没有结果，原因: " + status);
        }
    });
}
//根据坐标获取省市县事件
function codeLatLng(input) {
    marker != null ? marker.setMap(null) : marker;
    var latlngStr = input.split(",", 2);
    var lat = parseFloat(latlngStr[0]);
    var lng = parseFloat(latlngStr[1]);
    var latLng = new soso.maps.LatLng(lat, lng);
    var info = new soso.maps.InfoWindow({ map: map });
    geocoder.geocode({ 'location': latLng }, function (results, status) {
        if (status == soso.maps.GeocoderStatus.OK) {
            map.setCenter(results.location);
            marker = new soso.maps.Marker({
                map: map,
                //draggable: true,
                position: results.location
            });
            infoWin != null ? infoWin.close() : infoWin;
            //拖放事件，获取坐标
            /*soso.maps.Event.addListener(marker, 'dragend', function () {
                if (marker.getPosition()) {
                    codeLatLng(marker.getPosition().getLat() + "," + marker.getPosition().getLng());
                }
            });
            infoWin = new soso.maps.InfoWindow({
                map: map
            });
            infoWin.open(
                '<div style="width:200px;padding-top:10px;">' + results.address + '</div>',
                map.getCenter()
            );*/
        } else {
            $.dialog.alert("检索没有结果，原因: " + status);
        }
    });
}
//导入门店信息
function chooseShopLoc(t) {
    var address = $(t).attr("address");
    this.clearMarkers();
    codeAddress(address);
    codeLatLng($("#ctl00_contentHolder_hfLatitude").val() + "," + $("#ctl00_contentHolder_hfLongitude").val());
    $("#ctl00_contentHolder_txtWxAddress").val(address);
    $("#infoDiv").hide();
    $("#map_des").hide();
}

function newdoSubmit() {
    if (doSubmit()) {
        if ($("#ctl00_contentHolder_radIbeacon_0:checked").length > 0) {
            var wxAddress = $("#ctl00_contentHolder_txtWxAddress").val();
            if (wxAddress == "" || wxAddress.length > 50) {
                alert("微信门店地址不能为空长度必须为2-50个字符");
                return false;
            }

            var canPassWXAddress = wxAddress.replace($("#ctl00_contentHolder_hfProvince").val(), "").replace($("#ctl00_contentHolder_hfCity").val(), "").replace($("#ctl00_contentHolder_hfDistrict").val(), "");
            if (canPassWXAddress == "") {
                alert("微信门店地址不能纯粹省份、城市和地区");
                return false;
            }

            if ($("#ctl00_contentHolder_hfLongitude").val() == "" || $("#ctl00_contentHolder_hfLatitude").val() == "") {
                alert("请搜索标注，定位地图");
                return false;
            }
            if ($("#ctl00_contentHolder_txtWXBusinessName").val() == "" || $("#ctl00_contentHolder_txtWXBusinessName").val().length > 50) {
                alert("微信门店名长度必须为2-50个字符");
                return false;
            }
            var region = new RegExp("^1\\d{10}$|^\\d{2,4}-\\d{7,8}(-\\d{2,4})?$");
            if ($("#ctl00_contentHolder_txtWXTelephone").val() == "" || !region.test($("#ctl00_contentHolder_txtWXTelephone").val())) {
                alert("微信门店电话不能为空，请输入合法的电话或者手机号码");
                return false;
            }
            region = new RegExp("^\\d{1,8}$");
            if ($("#ctl00_contentHolder_txtWXAvgPrice").val() != "" && !region.test($("#ctl00_contentHolder_txtWXAvgPrice").val())) {
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
                region = new RegExp("^2015-1-1 \\d{1,2}:\\d{1,2}:01$");
                if (!region.test(firstTime) || !region.test(secondTime)) {
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

            if ($("#ctl00_contentHolder_txtStoresName").val() != $("#ctl00_contentHolder_txtWXBranchName").val()) {
                alert("门店名称必须和微信分店名相同，请正确输入");
                return false;
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


        }
        getUploadImages();
        return true;
    }
    return false;
}


