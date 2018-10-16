 /// <reference path="../Templates/vshop/default/script/jquery-1.11.0.min.js" />


function vShop_RegionSelector(containerId, onSelected, defaultRegionText) {
    /// <param name="onSelected" type="function">选择地址后回调,包括两个参数，依次为址址和地址编码</param>
    var regionHandleUrl = '/Handler/RegionHandler.ashx';
    init();
    var address = '';
    var code = 0;
    var country;
    function init() {
        if (defaultRegionText == undefined || defaultRegionText == "" || !defaultRegionText)
            defaultRegionText = '请选择省市区';
        var text = '<div class="btn-group bmargin">\
        <button id="address-check-btn" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">' + defaultRegionText + '</button>\
        <ul name="province" class="dropdown-menu" role="menu"></ul>\
        <ul name="city" class="dropdown-menu hide" role="menu"></ul>\
        <ul name="district" class="dropdown-menu hide" role="menu"></ul>\
        <ul name="street" class="dropdown-menu hide" role="menu"></ul>\
        </div>';
        $('#' + containerId).html(text);
        //初始化，获取省份列表
        getRegin("province", 0, function () { bind(); });
    }


    function getRegin(regionType, parentRegionId, callback) {
        /// <param name="regionType" type="String">"province-省,city-市,district-区"</param>

        if (!parentRegionId) {
            parentRegionId = 0;
            address = '';
        }
        var noSub = false;
        getSubRegionsFromServer(regionType, parentRegionId, callback);

    }

    function getSubRegionsFromServer(regionType, parentRegionId, callback) {
        var regions = [];

        if (!parentRegionId) {
            parentRegionId = 0;
            address = '';
        }
        $.ajax({
            url: "/Handler/RegionHandler.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "getregions", parentId: parentRegionId },
            async: false,
            success: function (resultData) {
                var noSub = false;
                var text = '';
                if (resultData.Status == "OK") {
                    regions = resultData.Regions;
                }
                if (regions && regions.length > 0) {
                    $.each(regions, function (i, region) {
                        text += '<li><a href="javascript:void(0)" name="' + region.RegionId + '">' + region.RegionName + '</a></li>';
                    });
                    $('#' + containerId + ' ul[name="' + regionType + '"]').html(text);
                }
                else noSub = true;
                callback(noSub);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus);

            },
            complete: function (XMLHttpRequest, textStatus) {
                //  alert(XMLHttpRequest + "-" + textStatus);
            }
        });
    }

    function bind(noSub) {
        $('#' + containerId + ' ul li a').unbind('click');
        $('#' + containerId + ' ul li a').click(function () {
            var currentUl = $(this).parent().parent();
            var regionId = $(this).attr('name');
            var nextRegionUl = currentUl.next();
            var prevRegionUl = currentUl.prev();
            var nextRegionType = nextRegionUl ? $(nextRegionUl).attr('name') : '';
            if (currentUl.attr("name") == "province") {
                address = $(this).html() + " ";
            }
            else {
                address += $(this).html() + " ";
            }
            if (nextRegionType) {

                code = $(this).attr('name');
                getRegin(nextRegionType, regionId, function (noSub) {
                    currentUl.addClass('hide');
                    if (noSub) {
                        var first = currentUl.parent().find('ul').first();
                        $(first).removeClass('hide');
                        onSelected(address, code);
                        //address = '';

                    }
                    else {
                        nextRegionUl && $(nextRegionUl).removeClass('hide');
                        setTimeout(function () {
                            $(".btn-group").addClass('open');
                        }, 1);
                    }
                    bind(noSub);
                });
                if ($(currentUl).attr("name") != "province") {//如果当前选中的级别不是省则设置选中区域
                    onSelected(address, code);
                }
                
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