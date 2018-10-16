$(document).ready(function () {
    if ($("#area_select li,city_select li").size() <= 0) {
        $("#city_top").addClass("disabled");
        $("#area_top").addClass("disabled");
    }
    $(".dropdown_box").bind("click", function (e) {
        return ShowRegion($(this).attr("id"), e);
    });
    $(".ap_content a").live("click", function (e) {
        return ChooiceRegion($(this).attr("id"), e);
    });
});


$(document).click(function (e) {
    var target = $(e.srcElement).attr("id");
    if (target == undefined)
        target = $(e.srcElement).attr("class");
    var targetStr = "dropdown_button|provincename|cityname|areaname|streetname|province_top|area_top|city_top|area_floor|province_floor|city_floor|city_info|province_info|area_info|province_select|city_select|area_select|street_select|street_floor|street_info";
    var targetArr = targetStr.split('|');
    var parentTarget = $(e.srcElement).parent().parent().parent().attr("id");
    if (target == undefined)
        target = "";
    if (parentTarget == undefined)
        parentTarget = "";
    //console.log(target + "-" + parentTarget);
    var InTarget = false;
    for (var i = 0; i < targetArr.length; i++) {
        if (target == targetArr[i] || parentTarget == targetArr[i]) {
            InTarget = true;
            break;
        }

    }
    if (!InTarget) {
        //$(".dp_address_list").hide();
        $(".dropdown_box").removeClass("dropdownhover").removeClass("nobotborder");
        $(".dp_border").hide();
    }
    if (!$(".dp_address_list").is(':visible')) {
        $(".dp_border").hide();
        $(".dropdown_box").removeClass("dropdownhover").removeClass("nobotborder");
    }

    var r = /(showRegion)|(topborder)|(regions)|(.*select.*)/g;
    if (!r.test(e.target.id)) {

        $(".regions").hide();
        $(".topborder").hide();
        $(".showRegion").removeClass("showRegion_hover");
    }
})

String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

function ReSetAddress() {

}

function stopPropagation(event) {
    if (window.event) {
        window.event.cancelBubble = true;
    }
    else if (event.stopPropagation) {
        event.stopPropagation();
    }
}

function ShowRegion(regionType, e) {
    if ($("#" + regionType).hasClass("disabled")) {
        return false;
    }
    $(".dp_address_list").show();
    $(".dropdown_box").addClass("nobotborder");
    $(".dropdown_box").removeClass("dropdownhover");
    $(".dp_border").width($("#" + regionType).width()).css("left", "4px").show();
    var lengwidth = 0;
    switch (regionType) {
        case "province_top":
            $("#area_floor").hide();
            $("#city_floor").hide();
            $("#street_floor").hide();
            $("#province_floor").show();
            $(".dropdown_box").eq(0).addClass("dropdownhover");
            break;
        case "city_top":
            $("#province_floor").hide();
            $("#area_floor").hide();
            $("#street_floor").hide();
            lengwidth = $("#province_top").width() + 8;
            //$("#city_floor").css({ left: lengwidth }).show();
            $(".dp_border").css({ left: lengwidth });
            $("#city_floor").show();
            $(".dropdown_box").eq(1).addClass("dropdownhover");
            break;
        case "area_top":
            $("#province_floor").hide();
            $("#city_floor").hide();
            $("#street_floor").hide();
            lengwidth = $("#city_top").width() + $("#province_top").width() + 12;
            // $("#area_floor").css({ left: lengwidth }).show();
            $(".dp_border").css({ left: lengwidth });
            $("#area_floor").show();
            $(".dropdown_box").eq(2).addClass("dropdownhover");
            break;
        case "street_top":
            $("#province_floor").hide();
            $("#city_floor").hide();
            $("#area_floor").hide();
            lengwidth = $("#area_top").width() + $("#city_top").width() + $("#province_top").width() + 16;
            $(".dp_border").css({ left: lengwidth });
            $("#street_floor").show();
            $(".dropdown_box").eq(3).addClass("dropdownhover");
            break;
        default:
            $("#province_floor").hide();
            $("#city_floor").hide();
            $("#area_floor").hide();
            $("#street_floor").hide();
            break;
    };
    stopPropagation(e);
}

function SetRegionName(depth) {
    $("#regionSelectorName").val("");
    if (depth == 1)
        $("#regionSelectorName").val($("#provincename").text().trim());
    if (depth == 2)
        $("#regionSelectorName").val($("#provincename").text().trim() + " " + $("#cityname").text().trim());
    if (depth == 3)
        $("#regionSelectorName").val($("#provincename").text().trim() + " " + $("#cityname").text().trim() + " " + $("#areaname").text().trim());
    if (depth == 4 && $("#regionDisplayStreet").val() == "true")
        $("#regionSelectorName").val($("#provincename").text().trim() + " " + $("#cityname").text().trim() + " " + $("#areaname").text().trim() + " " + $("#streetname").text().trim());
    $(".showRegion").text($("#regionSelectorName").val());
    $(".showRegion").addClass("showRegion_hover");
    var w = $(".showRegion").width() + 81;
    $(".topborder").width(548 - w + 45);
    $(".topborder").css("left", (w) + "px").show();
}

function SaveDeliveryScopRegion(regionId, depth) {
    var quantity = 1;
    if ($("#buyAmount").length != 0) {
        quantity = parseInt($("#buyAmount").val());
        if (isNaN(quantity) || quantity <= 0) {
            quantity = 1;
        }
    } $.ajax({
        url: "/Handler/GeneralHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "SaveDeliveryScop", regionId: regionId },
        success: function (resultData) {
            if (resultData.status != "true")
                alert(resultData.msg);
            else {
                //如果是市级地区则重新计算运费
                if (depth == 3 || dpeth == 2) {
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
                    if ($("#productDetails_sku_v").length > 0) {
                        skuId = $("#productDetails_sku_v").val();
                    }
                    $.ajax({
                        url: "/API/VshopProcess.ashx",
                        type: 'post', dataType: 'json', timeout: 10000,
                        data: { action: "GetProductFreight", regionId: regionId, productId: productId, SkuId: skuId, quantity: quantity },
                        success: function (resultData) {
                            if (resultData.Status == "OK" && !isNaN(parseFloat(resultData.Freight)) && parseFloat(resultData.Freight) > 0) {
                                $("#labProductFreight").html("运费：<label>" + resultData.Freight + "</label>元");
                            }
                            else { $("#labProductFreight").html("免运费"); }
                        }
                    });
                }
            }
        }
    });
}

function ChooiceRegion(currentControlId, e) {
    stopPropagation(e);

    var depth = GetDepthBySelectId(currentControlId);
    var regionSpan = GetRegionTypeSelectdepth(depth);
    var selectedRegionId = currentControlId.replace(/[^0-9]/ig, "");
    var hasvalue = (selectedRegionId != null && parseInt(selectedRegionId) > 0);

    // 更新当前选择的地区
    if (hasvalue) {
        $("#regionSelectorValue").val(selectedRegionId);
        $("#" + regionSpan).text($("#" + currentControlId).text());
        SetRegionName(depth);

    }
    else {
        if (depth == 1) {
            $("#regionSelectorValue").val("");
            SetRegionName(-1);
            $("#provincename").text('请选择省');
        }
        else {
            var prevRegion = GetRegionTypeSelectdepth(depth - 1);
            var selectorId = $(".ap_content a:contains('" + prevRegion + "')").attr("id").replace(/[^0-9]/ig, "");
            $("#regionSelectorValue").val(selectorId);
            SetRegionName(depth);
        }
    }

    // 重置所有子区域的显示
    var subDepth = depth + 1;

    while (subDepth <= 4) {
        ResetSelector(subDepth);
        subDepth++;
    }

    var haschild = (subDepth > (depth + 1));

    var ul_type = "";
    var showtyp = "";
    if ((depth + 1) == 2) {
        $("#city_top").removeClass("disabled");
        ul_type = "city_select";
        showtyp = "请选择市";
    } else if ((depth + 1) == 3) {
        $("#area_top").removeClass("disabled");
        ul_type = "area_select";
        showtyp = "请选择区/县";
    }
    else if ((depth + 1) == 4 && $("#regionDisplayStreet").val() == "true") {
        $("#street_top").removeClass("disabled");
        ul_type = "street_select";
        showtyp = "请选择街道";
    }
    // 更新直接子区域的内容
    if (hasvalue && haschild) {
        FillSelector(selectedRegionId, ul_type, showtyp);
    }
    if (depth == 4 || depth == 3 || depth == 2)
        SaveDeliveryScopRegion(selectedRegionId, depth);
    if (depth == 4) {
        ShowRegion("", e);
    } else {
        ShowRegion(ul_type.replace("select", "top"), e);
    }
    $(".dropdown_box").removeClass("dropdownhover");
    if (depth != 4) {
        //  $(".dropdown_box").addClass("nobotborder");
        $(".dropdown_box").eq(depth).removeClass("nobotborder").addClass("dropdownhover");
    }
    else {
        $(".dropdown_box").removeClass("nobotborder");
        $(".dp_border").hide();
    }
    stopPropagation(e);
    if (depth > 1) btnziti();
}

function GetRegionTypeSelectdepth(depthId) {
    switch (depthId) {
        case 1:
            return "provincename";
            break;
        case 2:
            return "cityname";
            break;
        case 3:
            return "areaname";
            break;
        case 4:
            return "streetname";
            break;
    };
}

function GetDepthBySelectId(currentControlId) {
    if (currentControlId.indexOf('select_new_province_') >= 0) {
        return 1;
    }
    if (currentControlId.indexOf('select_new_city_') >= 0) {
        return 2;
    }
    if (currentControlId.indexOf('select_new_area_') >= 0) {
        return 3;
    }
    if (currentControlId.indexOf('select_new_street') >= 0) {
        return 4;
    }
}

// 重置指定的下拉选择框
function ResetSelector(dep) {
    var selector = GetRegionTypeSelectdepth(dep);
    switch (dep) {
        case 1:
            $("#" + selector).text('请选择省');
            $("#city_top,area_top").addClass("disabled");
            break;
        case 2:
            $("#" + selector).text('请选择市');
            $("#area_top").addClass("disabled");
            break;
        case 3:
            $("#" + selector).text('请选择县/区');
            break;
        case 4:
            $("#" + selector).text('请选择街道');
            break;
    };

}

// 根据指定的父地区编号填充地区下拉框的可选内容
function FillSelector(parentId, selector, selectedValue) {
    $.ajax({
        url: "/Handler/RegionHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "getregions", parentId: parentId },
        success: function (resultData) {
            $("#" + selector + " li").remove();
            if (resultData.Status == "OK") {
                $.each(resultData.Regions, function (i, region) {
                    $("#" + selector).append("<li><a href=\"javascript:;\" id=\"select_new_" + selector.replace("select", "") + region.RegionId + "\">" + region.RegionName + "</a></li>");
                });
            } else if (resultData.Status == "0") {
                if (selector == "street_select") {
                    $("#area_floor").hide();
                    $("#street_top").addClass("disabled");
                    $("#street_floor").hide();
                }
                else {
                    $("#area_floor").hide();
                    $("#street_top").addClass("disabled");
                    $("#street_floor").hide();
                    $("#area_top").addClass("disabled");
                }
            }
        }
    });
}

// 获取当前选择的地区编号
function GetSelectedRegionId() {
    return $("#regionSelectorValue").val();
}

// 手工设置当前要选中的地区
function ResetSelectedRegion(regionId) {
    $.ajax({
        url: "/Handler/RegionHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "getregioninfo", regionId: regionId },
        success: function (resultData) {
            if (resultData.Status != "OK")
                return;

            var depth = parseInt(resultData.Depth);
            if (depth == 1) {
                ChooiceRegion("select_new_province_" + resultData.RegionId);
                ResetSelector(2);
                ResetSelector(3);
                ResetSelector(4);
            }
            else if (depth == 2) {
                var pathArr = resultData.Path.split(",");
                var pathNameArr = resultData.RegionName.split(",");
                FillSelector(pathArr[0].replace(/\s/g, ""), 'city_select');
                $("#city_top").removeClass("disabled");
                $("#provincename").text(pathNameArr[0].replace(/\s/g, ""));
                $("#cityname").text(pathNameArr[1].replace(/\s/g, ""));
                ResetSelector(3);
                ResetSelector(4);
            }
            else if (depth == 3) {
                var pathArr = resultData.Path.split(",");
                var pathNameArr = resultData.RegionName.split(",");
                FillSelector(pathArr[0].replace(/\s/g, ""), 'city_select');
                FillSelector(pathArr[1].replace(/\s/g, ""), 'area_select');
                $("#city_top").removeClass("disabled");
                $("#area_top").removeClass("disabled");
                $("#provincename").text(pathNameArr[0].replace(/\s/g, ""));
                $("#cityname").text(pathNameArr[1].replace(/\s/g, ""));
                $("#areaname").text(pathNameArr[2].replace(/\s/g, ""));
                ResetSelector(4);
            }
            else {
                var pathArr = resultData.Path.split(",");
                var pathNameArr = resultData.RegionName.split(",");
                FillSelector(pathArr[0].replace(/\s/g, ""), 'city_select');
                FillSelector(pathArr[1].replace(/\s/g, ""), 'area_select');
                FillSelector(pathArr[2].replace(/\s/g, ""), 'street_select');
                $("#city_top").removeClass("disabled");
                $("#area_top").removeClass("disabled");
                $("#street_top").removeClass("disabled");
                $("#provincename").text(pathNameArr[0].replace(/\s/g, ""));
                $("#cityname").text(pathNameArr[1].replace(/\s/g, ""));
                $("#areaname").text(pathNameArr[2].replace(/\s/g, ""));
                $("#streetname").text(pathNameArr[3].replace(/\s/g, ""));

            }
            $("#regionSelectorValue").val(resultData.RegionId);
            if (depth > 1) btnziti();
        }
    });
}