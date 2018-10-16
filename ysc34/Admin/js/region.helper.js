$(document).ready(function () {
    if ($("#area_select li,city_select li").size() <= 0) {
        $("#city_top").addClass("disabled");
        $("#area_top").addClass("disabled");
    }
    $(".dropdown_box").live("click", function (e) {
        return ShowRegion($(this).attr("id"), e);
    });
    $(".dropdown_button").live("click", function (e) {
        return ShowRegion($(this).parent().attr("id"), e);
    });
    $(".ap_content a").live("click", function (e) {
        ChooiceRegion($(this).attr("id"), e);
    });
    //$("a[t=clear]").live("click", function (e) {
    //    ChooiceRegion($(this).attr("id"), e);
    //});
});

//点击非操作项时关闭选项卡
$(document).click(function (e) {
    var srcElement = e.srcElement || e.target;
    var target = $(srcElement).attr("id");
    if (target == undefined)
        target = $(srcElement).attr("class");
    var targetStr = "dropdown_button|provincename|cityname|areaname|streetname|province_top|area_top|city_top|area_floor|province_floor|city_floor|city_info|province_info|area_info|province_select|city_select|area_select|street_select|street_floor|street_info";
    var targetArr = targetStr.split('|');
    var parentTarget = $(srcElement).parent().parent().parent().attr("id");
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
        $(".dp_address_list").hide();
        $(".dropdown_box").removeClass("dropdownhover").removeClass("nobotborder");
        $(".dp_border").hide();
    }
    if (!$(".dp_address_list").is(':visible')) {
        $(".dp_border").hide();
        $(".dropdown_box").removeClass("dropdownhover").removeClass("nobotborder");
    }

})

String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

function stopPropagation(event) {
    var event = event || window.event;
    if (event) {
        event.cancelBubble = true;
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
    var IsShift = $("#regionIsShift").val() == "true" ? true : false;
    var lengwidth = 2;
    switch (regionType) {
        case "province_top":
            $("#area_floor").hide();
            $("#city_floor").hide();
            $("#street_floor").hide();
            $("#province_floor").css({ left: lengwidth }).show();
            break;
        case "city_top":
            $("#province_floor").hide();
            $("#area_floor").hide();
            $("#street_floor").hide();
            if (IsShift)
                lengwidth = $("#province_top").width() + 10;
            $("#city_floor").css({ left: lengwidth }).show();
            break;
        case "area_top":
            $("#province_floor").hide();
            $("#city_floor").hide();
            $("#street_floor").hide();
            if (IsShift)
                lengwidth = $("#city_top").width() + $("#province_top").width() + 18;
            $("#area_floor").css({ left: lengwidth }).show();
            break;
        case "street_top":
            $("#province_floor").hide();
            $("#city_floor").hide();
            $("#area_floor").hide();
            if (IsShift)
                lengwidth = $("#area_top").width() + $("#city_top").width() + $("#province_top").width() + 18;
            $("#street_floor").css({ left: lengwidth }).show();
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
}

// 获取名称
function GetRegionName() {
    return $("#regionSelectorName").val();
}

function ChooiceRegion(currentControlId, e) {
    var depth = GetDepthBySelectId(currentControlId);
    var regionSpan = GetRegionTypeSelectdepth(depth);
    var selectedRegionId = currentControlId.replace(/[^0-9]/ig, "");
    var hasvalue = (selectedRegionId != null && parseInt(selectedRegionId) > 0);
    var IsClear = ($("#" + currentControlId).attr("t") == "clear");
    if (IsClear) {
        //如果清空，则清空
        var regionSpan1;

        regionSpan1 = GetRegionTypeSelectdepth(4);
        $("#" + regionSpan1).text("请选择街道");
        if (depth == 3) {
            regionSpan1 = GetRegionTypeSelectdepth(depth);
            $("#" + regionSpan1).text("请选择区/县");
            $("#street_select").empty();
            $("#street_top").addClass("disabled");
        }
        if (depth == 2) {
            regionSpan1 = GetRegionTypeSelectdepth(depth);
            $("#" + regionSpan1).text("请选择市");
            $("#" + GetRegionTypeSelectdepth(depth + 1)).text("请选择区/县");
            $("#area_top").addClass("disabled");
            $("#area_select").empty();
            $("#street_select").empty();
            $("#street_top").addClass("disabled");
        }
        if (depth == 1) {
            regionSpan1 = GetRegionTypeSelectdepth(depth);
            $("#" + regionSpan1).text("请选择省");
            $("#" + GetRegionTypeSelectdepth(depth + 1)).text("请选择市");
            $("#" + GetRegionTypeSelectdepth(depth + 2)).text("请选择区/县");
            $("#area_top").addClass("disabled");
            $("#area_select").empty();
            $("#street_select").empty();
            $("#street_top").addClass("disabled");
            $("#city_select").empty();
            $("#city_top").addClass("disabled");
        }
        SetRegionName(depth - 1);
        if (depth == 1) {
            $("#regionSelectorValue").val("");
            $("#regionSelectorValue").attr("depth", "0");
        }
        else {
            $("#regionSelectorValue").val(selectedRegionId);
            $("#regionSelectorValue").attr("depth", depth - 1);
        }
    }
    else {
        // 更新当前选择的地区
        if (hasvalue) {
            $("#regionSelectorValue").val(selectedRegionId);
            $("#regionSelectorValue").attr("depth", depth);
            $("#" + regionSpan).text($("#" + currentControlId).text());
            $("#" + regionSpan).attr("value", selectedRegionId);
            SetRegionName(depth);
        }
        else {
            if (depth == 1) {
                $("#regionSelectorValue").val("");
                $("#regionSelectorValue").attr("depth", "0");
                SetRegionName(-1);
                $("#provincename").text('请选择省');
            }
            else {
                var prevRegion = GetRegionTypeSelectdepth(depth - 1);
                var selectorId = $("#" + prevRegion).attr("value");
                $("#" + regionSpan).text(depth == 2 ? "请选择市" : (depth == 3 ? "请选择区/县" : "请选择街道"));
                $("#regionSelectorValue").val(selectorId);
                $("#regionSelectorValue").attr("depth", depth);
                SetRegionName(depth);
            }
        }

        if (!hasvalue) return;

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
        if (depth == 4) {
            ShowRegion("", e);
        } else {
            ShowRegion(ul_type.replace("select", "top"), e);
        }
    }
    stopPropagation(e)
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
            $("#city_top").addClass("disabled");
            $("#area_top").addClass("disabled");
            $("#street_top").addClass("disabled");
            break;
        case 2:
            $("#" + selector).text('请选择市');
            $("#area_top").addClass("disabled");
            $("#street_top").addClass("disabled");
            break;
        case 3:
            $("#" + selector).text('请选择县/区');
            $("#street_top").addClass("disabled");
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

                $("#" + selector).append("<li><a href=\"javascript:;\" t=\"clear\" id=\"select_new_" + selector.replace("select", "") + "_" + parentId + "\">[清空]</a></li>");
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
        }
    });
}