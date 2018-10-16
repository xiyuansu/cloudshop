function initProvinceData() {
    $.ajax({
        url: "/Handler/RegionHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "GetRegionsOfProvinceCounty" },
        async: false,
        success: function (resultData) {
            province = resultData.province;
            $(province).each(function (i) {
                if (this.city != null) {
                    cityCounts += this.city.length;
                }
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
           //alert(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus);

        },
        complete: function (XMLHttpRequest, textStatus) {
            //  alert(XMLHttpRequest + "-" + textStatus);
        }
    });
}
var city;
var province;
var cityCounts = 0;


$(function () {
    //初始化页面
    //$('#inputDefFirstUnit').val(initDefFirst);
    //$('#inputDefFirstUnitMonry').val(initDefFirstMoney);
    //$('#inputDefAccumulationUnit').val(initDefAccumulationUnit);
    //$('#inputDefAccumulationUnitMoney').val(initDefAccumulationUnitMoney);
    //初始化商家地址
    initProvinceData();
    //var regionJson = null;
    //if ($("#hidRegionJson").val() != "") {
    //    regionJson = JSON.parse($("#hidRegionJson").val());
    //    initRegionData(regionJson);
    //}
    $('input[name="ctl00$contentHolder$radIsFreeShipping"]').on('ifChecked', function (event) {
        SetFreeStatus();
    });
    $('input[name="ctl00$contentHolder$radValuationMethods"]').on('ifChecked', function (event) {
        setUnit();
    });
    //根据计价方式设置单位
    setUnit();
    //根据是否包邮隐藏地区运费
    SetFreeStatus();
    //修改模板名称则需要重新测试名称
    $("#ctl00_contentHolder_txtModeName").change(function (e) {
        checkNameSuccess = false;
    })
    //新增行
    $('#addCityFreight').click(function () {
        var msg = "";
        $('#regionFreight tr').siblings().find('.chooseArea').each(function () {
            if ($(this).data('id') == null || $(this).data('id') == undefined || $(this).data('id') == "") {
                msg = "请先将未添加地区的行设置好地区，再添加新的行";
                alert(msg);
                return false;
            }
        });
        if (msg != "") {
            return false;
        }
        var valuationMethod = 1;
        if ($('input:radio[name="ctl00$contentHolder$radValuationMethods"]:checked').length > 0) {
            valuationMethod = parseInt($('input:radio[name="ctl00$contentHolder$radValuationMethods"]:checked').val());
        }
        var len = $("#regionFreight tr").length;
        var defaultNumber = parseFloat($("#txtDefaultNumber").val());
        var defaultPrice = parseFloat($("#txtDefaultPrice").val());
        var addNumber = parseFloat($("#txtAddNumber").val());
        var addPrice = parseFloat($("#txtAddPrice").val());
        if (isNaN(defaultPrice) || defaultPrice < 0 || defaultPrice > 100000) {
            defaultPrice = 0;
        }
        if (isNaN(addPrice) || addPrice < 0 || addPrice > 100000 || (defaultPrice == 0 && addPrice == 0)) {
            addPrice = 0;
        }
        if (isNaN(defaultNumber) || defaultNumber < 0 || defaultNumber > 10000000) {
            defaultNumber = 0;
        }

        if (isNaN(addNumber) || addNumber < 0 || addNumber > 10000000 || (defaultNumber == 0 && addNumber == 0)) {
            addNumber = 0;
        }
        if (valuationMethod == 1) {
            defaultNumber = parseInt(defaultNumber);
            addNumber = parseInt(addNumber);
        }
        var str = '<tr><td><a class="exit-area">编辑</a><div class="area-group"><p>未添加地区</p></div></td><td><input type="text" class="form-control input-xs" value="' + defaultNumber + '"/></td><td><input type="text" class="form-control input-xs" value="' + defaultPrice + '" /></td><td><input type="text" class="form-control input-xs" value="' + addNumber + '"/></td><td><input type="text" class="form-control input-xs" value="' + addPrice + '" /></td><td><span class=\"btn-a\"><a name=\"delContent\">删除</a></span></td></tr>';
        $("#regionFreight").find('tbody').append(str);
        $('a[name="delContent"]').click(function () {
            $(this).parent().parent().parent().remove();
        });
    });

    $('a[name="delContent"]').click(function () {
        $(this).parent().parent().parent().remove();
    });

    //弹框显示市级
    $(document).on('click', '.operate', function () {
        var cityDiv = $(this).siblings('div');
        if (cityDiv.is(':hidden')) {
            $('.city-box').hide();
            cityDiv.show();
        } else {
            cityDiv.hide();
        }

    });
    //if (IsUsed == 1)
    //{
    //    $('input[name="valuationMethod"]').attr('disabled', 'disabled');
    //    $('#valuationMethodTip').text('已使用，不能修改');
    //}

    /*

    //弹框关闭市级
    $(document).on('click', '.city .colse', function () {
        $(this).parents('.city-box').hide();
    });

    //指定行编辑操作
    $('.table-area-freight').on('click', '.exit-area', function () {
        $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2");
        var areaContent = null;
        if ($("#ulArea").size() == 0) {
            //初始化弹窗内容
            $('.table-area-freight').after("<ul id='ulArea' class='province clearfix'></ul>");
            areaContent = $("#ulArea");
            if (province) {
                var html = "";
                var city = "";
                $(province).each(function (i) {
                    var provinceid = this.id;
                    var provincename = this.name;
                    var operate = this.city != null ? "<b class='operate glyphicon glyphicon-menu-down'></b>" : "";
                    city = "";
                    if (this.city != null) {
                        //加载城市
                        city += "<div class='city-box' id='dvCity_" + i + "'><ul class='city clearfix'><i class='colse'>×</i>";
                        $(this.city).each(function (j) {
                            city += "<li><label><input type='checkbox' id='city_" + this.id + "' value='" + this.id + "' />" + this.name + "</label></li>";
                        });
                        city += "</ul></div>";
                    }

                    html += "<li><label><input type='checkbox' class='pro-check' id='province_" + provinceid + "' />" + provincename + "</label><span class='spCount'></span>" + operate + city + "</li>";

                });

                areaContent.html(html);

                $("#ulArea input[type='checkbox']").change(function () {
                    var isprovince = $(this).attr("id").indexOf("province_") > -1;
                    var count = $(this).parent().siblings(".spCount");
                    var cityCheck = $(this).parents('li').find("input:checkbox[id^='city_']");
                    var thisCity = $(this).parents('.city-box');
                    if (isprovince) {
                        //省份
                        if (this.checked)
                            cityCheck.not('.hide').attr("checked", "checked");
                        else
                            cityCheck.removeAttr("checked");
                        count.text("(" + cityCheck.filter('input:checked').length + ")");
                    } else {
                        thisCity.siblings(".spCount").text("(" + cityCheck.filter('input:checked').length + ")");
                    }
                });
            }
        }

        var _this = $(this);

        //弹窗
        $.dialog({
            title: '选择区域',
            lock: true,
            width: 310,
            id: 'logoArea',
            content: $("#ulArea")[0],
            init: function () {
                clearData(_this);
                bindData(_this);
            },
            padding: '20px 10px',
            okVal: '保存',
            ok: function () {
                var data = "", text = "";
                $("#ulArea :checkbox[id^='city_']:checked").each(function () {
                    data += $(this).attr("id").replace("city_", "") + ',';
                    text += $(this).parent().text() + ',';
                });
                _this.siblings('span').html(text.substring(0, text.length - 1)).data('id', data.substring(0, data.length - 1));
                if (IsAllSelected()) {
                    $("#addCityFreight").hide();
                }
                else {
                    $("#addCityFreight").show();
                }
                $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
            }
        });

        $(".aui_close").click(function () {
            $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
        });
    });
    */

    //新增行
    $('#addFree').click(function () {
        addFree();
    });

    
    $('input[name="ctl00$contentHolder$ctl00"]').on({
        'switchChange.bootstrapSwitch': function (event, state) {
            $("#regionFree tr:gt(0)").remove();
            if (state == true) {
                addFree();
                $(".hasFree").show();
            }
            else {
                $(".hasFree").hide();
            }
        }
    });
});

function addFree() {
    var str = '<tr><td><a class="exit-area">编辑</a><div class="area-group"><p>未添加地区</p></div></td><td><select class="setfreeshipping form-control fl" style="width:100px;"><option value="1">件数</option><option value="2">金额</option><option value="3">件数+金额</option></select><span class="free-contion">满<input type="text" value="" class="form-control mlr">件包邮</span></td><td><span class=\"btn-a\"><a name=\"delFree\">删除</a></span></td></tr>';
    $("#regionFree").find('tbody').append(str);
    $('a[name="delFree"]').click(function () {
        $(this).parent().parent().parent().remove();
        if ($("#regionFree tr").length == 1)
            $('input[name="ctl00$contentHolder$ctl00"]').bootstrapSwitch("state", false);
    });
}


function IsAllSelected() {
    var chooseId = "";
    $('#regionFreight tr').siblings().find('.chooseArea').each(function () {
        if ($(this).data('id') != null && $(this).data('id') != undefined) {
            chooseId += (chooseId == "" ? "" : ",") + $(this).data('id').toString();
        }
    });
    ///如果已选择的城市数==总城市数则全部已选择完
    if (chooseId != "" && chooseId.split(",").length == cityCounts) {
        return true;
    }
    else {
        return false;
    }
}

//清空弹框内容并绑定数据
function clearData(cur) {
    var area = $("#ulArea");
    area.find('.spCount').text('');
    area.find('.city-box').hide();
    area.find('input:checked').removeAttr("checked");
    area.find('input.hide').removeClass();
    area.find('li').removeAttr('style');

    var strs = new Array();
    var chooseId;
    cur.parents('tr').siblings().find('.chooseArea').each(function () {
        if ($(this).data('id') != null && $(this).data('id') != undefined) {
            chooseId = $(this).data('id').toString();
            if (chooseId.indexOf(',') > 0) {
                strs = chooseId.split(",");
                for (i = 0; i < strs.length; i++) {
                    $('#city_' + strs[i]).addClass('hide').parent().parent().hide();
                }
            } else {
                $('#city_' + chooseId).addClass('hide').parent().parent().hide();
            }
        }
    });
    area.find('.city-box').each(function () {
        if ($(this).find('.hide').length == $(this).find('input').length) {
            $(this).parent().hide();
        }
    });

}
function bindData(cur) {
    var area = $("#ulArea");
    var dataId = cur.siblings('span').data('id');
    if (dataId != null) {
        if (dataId.toString().indexOf(',') > 0) {
            var strs = new Array();
            strs = dataId.split(",");
            for (i = 0; i < strs.length; i++) {
                $('#city_' + strs[i]).attr('checked', true);
            }
        } else {
            $('#city_' + dataId).attr('checked', true);
        }

    }
    area.find('li').each(function () {
        var len = $(this).find('input:checked').length;
        var AllLen = $(this).find('.city').find('input').length;
        if (len > 0)
            $(this).find('.spCount').text('(' + len + ')');
        if (len == AllLen)
            $(this).find('.pro-check').attr('checked', true);
    });
}
function setUnit() {
    var valuationMethod = 1;
    if ($('input:radio[name="ctl00$contentHolder$radValuationMethods"]:checked').length > 0) {
        valuationMethod = parseInt($('input:radio[name="ctl00$contentHolder$radValuationMethods"]:checked').val());
    }
    if (valuationMethod == 1) {
        $('span[name="ValuationUnitDesc"]').text('件');
        $('span[name="ValuationUnit"]').html('件');
    }
    if (valuationMethod == 2) {
        $('span[name="ValuationUnitDesc"]').text('重');
        $('span[name="ValuationUnit"]').html('kg');
    }
    if (valuationMethod == 3) {
        $('span[name="ValuationUnitDesc"]').text('体积');
        $('span[name="ValuationUnit"]').html('m<sup>3</sup>');
    }
}
function SetFreeStatus() {
    var isFreeShipping = $('input:radio[name="ctl00$contentHolder$radIsFreeShipping"]:checked').val();
    if (isFreeShipping == "0" || isFreeShipping == "") {
        $('.moreContent').show();
    }
    else {
        $('.moreContent').hide();
    }
}
var freightTempContent = '';

$("#txtDefaultNumber,#txtDefaultPrice,#txtAddNumber,#txtAddPrice").blur(function (e) {
    checkDefaultValuationData();
});

function checkDefaultValuationData() {
    var valuationMethod = 1;
    if ($('input:radio[name="ctl00$contentHolder$radValuationMethods"]:checked').length > 0) {
        valuationMethod = parseInt($('input:radio[name="ctl00$contentHolder$radValuationMethods"]:checked').val());
    }
    var defaultNumber = parseFloat($("#txtDefaultNumber").val());
    var defaultPrice = parseFloat($("#txtDefaultPrice").val());
    var addNumber = parseFloat($("#txtAddNumber").val());
    var addPrice = parseFloat($("#txtAddPrice").val());
    var valuationMethodErrMsg = "";
    if (isNaN(defaultNumber) || defaultNumber <= 0 || defaultNumber > 10000000) {
        valuationMethodErrMsg += (valuationMethodErrMsg == "" ? "" : ",") + "默认数量不能为空,数值限制在0-10000000之间,并且大于0";
        $("#txtValuationMethodTip").text(valuationMethodErrMsg);
        return false;
    }
    if (isNaN(defaultPrice) || defaultPrice < 0 || defaultPrice > 100000) {
        valuationMethodErrMsg += (valuationMethodErrMsg == "" ? "" : ",") + "默认运费不能为空,并且限制的0-100000之间";
        $("#txtValuationMethodTip").text(valuationMethodErrMsg);
        return false;
    }
    if (isNaN(addNumber) || addNumber < 0 || addNumber > 10000000 || (defaultNumber == 0 && addNumber == 0)) {
        valuationMethodErrMsg += (valuationMethodErrMsg == "" ? "" : ",") + "增量数量不能为空,并且限制在0-10000000之间,如果默认数量为0,则增加数量必须大于0";
        $("#txtValuationMethodTip").text(valuationMethodErrMsg);
        return false;
    }
 //   if (isNaN(addPrice) || addPrice < 0 || addPrice > 100000 || (defaultPrice == 0 && addPrice == 0)) {
    if (isNaN(addPrice) || addPrice < 0 || addPrice > 100000 ) {
        valuationMethodErrMsg += (valuationMethodErrMsg == "" ? "" : ",") + "增加运费不能为空,并且限制在0-100000之间,默认运费为0,则增加运费必须大于0";
        $("#txtValuationMethodTip").text(valuationMethodErrMsg);
        return false;
    }
    if (valuationMethod == 1 && ($("#txtDefaultNumber").val().indexOf(".") > -1 || $("#txtAddNumber").val().indexOf(".") > -1)) {
        valuationMethodErrMsg += (valuationMethodErrMsg == "" ? "" : ",") + "计价方式为按件数时,默认数量和增量数量只能为整数不能为小数";
    }
    if (valuationMethodErrMsg != "") {
        $("#txtValuationMethodTip").text(valuationMethodErrMsg);
        return false;
    }
    if (valuationMethod == 1) {
        $("#txtDefaultNumber").val(parseInt(defaultNumber));
        $("#txtAddNumber").val(parseInt(addNumber));
    }
    $("#txtValuationMethodTip").text("");
    return true;
}
var checkNameSuccess = false;
var lastSubmitTime = new Date();
var submitTimes = 0;
function checkData() {

    valuationMethod = 0;
    if ($.trim($("#ctl00_contentHolder_txtModeName").val()) == "" || $.trim($("#ctl00_contentHolder_txtModeName").val()).length > 20) {
        $("#ctl00_contentHolder_txtModeNameTip").html("模板名称不能为空，长度限制在20字符以内").css("color", "red").show();
        return false;
    }
    else {
        if (!checkNameSuccess) {
            checkTemplateName($("#ctl00_contentHolder_txtModeName").val());
            return false;
        }
        $("#ctl00_contentHolder_txtModeNameTip").html("&nbsp;");
    }
    //两秒内重复点击直接返回false
    var tempDate = new Date();
    if ((tempDate.getTime() - lastSubmitTime.getTime()) < 2000 && submitTimes > 0) {
        lastSubmitTime = new Date();
        submitTimes += 1;
        return false;
    }
    lastSubmitTime = new Date();
    submitTimes += 1;
    //如果不包邮，则检测计价方式的数据
    if ($('input:radio[name="ctl00$contentHolder$radIsFreeShipping"]:checked').val() == "0" && !checkDefaultValuationData()) {
        return false;
    }
    if (!PageIsValid()) {
        return false;
    }

    if ($('input:radio[name="ctl00$contentHolder$radIsFreeShipping"]:checked').length == 0) {
        alert("请选择是否包邮！");
        return false;
    }
    if ($('input:radio[name="ctl00$contentHolder$radValuationMethods"]:checked').length == 0) {
        alert("请选择计价方式");
        return false;
    }
    else {
        valuationMethod = parseInt($('input:radio[name="ctl00$contentHolder$radValuationMethods"]:checked').val());
    }
    var valuationJson = "";
    if ($('input:radio[name="ctl00$contentHolder$radIsFreeShipping"]:checked').val() == "0") {
        //地区模板运费检查
        var areaContent = '';
        var defaultNumber = 0, defaultPrice = 0, addNumber = 0, addPrice = 0;
        var valuationMethodErrMsg = '';
        $('#regionFreight tr').each(function (idx, el) {
            if (idx > 0 && valuationMethodErrMsg == '') {
                areaContent = $('td', el).eq(0).find('p').attr('data-storage') || '';
                defaultNumber = parseFloat($('td', el).eq(1).find('input').val() || 0);
                defaultPrice = parseFloat($('td', el).eq(2).find('input').val() || 0);
                addNumber = parseFloat($('td', el).eq(3).find('input').val() || 0);
                addPrice = parseFloat($('td', el).eq(4).find('input').val() || 0);
                if (valuationMethod == 1) {
                    defaultNumber = parseInt($('td', el).eq(1).find('input').val() || 0);
                    addNumber = parseInt($('td', el).eq(3).find('input').val() || 0);
                }
                if (areaContent == "") {
                    valuationMethodErrMsg += "运送地区不能为空，请检查";
                    alert(valuationMethodErrMsg);
                    return false;
                }
                if (isNaN(defaultPrice) || defaultPrice < 0 || defaultPrice > 100000) {
                    valuationMethodErrMsg += (valuationMethodErrMsg == "" ? "" : ",") + "地区默认运费不能为空,并且限制在0-100000之间";
                    alert(valuationMethodErrMsg);
                    return false;
                }
               // if (isNaN(addPrice) || addPrice < 0 || addPrice > 100000 || (defaultPrice == 0 && addPrice == 0)) {
                if (isNaN(addPrice) || addPrice < 0 || addPrice > 100000 ) {
                    valuationMethodErrMsg += (valuationMethodErrMsg == "" ? "" : ",") + "地区增加运费不能为空,并且限制在0-100000之间,默认运费为0,则增加运费必须大于0";
                    alert(valuationMethodErrMsg);
                    return false;
                }
                if (isNaN(defaultNumber) || defaultNumber <= 0 || defaultNumber > 10000000) {
                    valuationMethodErrMsg += (valuationMethodErrMsg == "" ? "" : ",") + "地区默认数量不能为空,数值限制在0-10000000之间,并且大于0";
                    alert(valuationMethodErrMsg);
                    return false;
                }
                if (isNaN(addNumber) || addNumber < 0 || addNumber > 10000000 || (defaultNumber == 0 && addNumber == 0)) {
                    valuationMethodErrMsg += (valuationMethodErrMsg == "" ? "" : ",") + "地区增量数量不能为空,并且限制在0-10000000之间,如果默认数量为0,则增加数量必须大于0";
                    alert(valuationMethodErrMsg);
                    return false;
                }
                if (valuationMethod == 1 && ($('td', el).eq(1).find('input').val().indexOf(".") > -1 || $('td', el).eq(3).find('input').val().indexOf(".") > -1)) {
                    valuationMethodErrMsg += (valuationMethodErrMsg == "" ? "" : ",") + "计价方式为按件数时,地区默认数量和增量数量只能为整数不能为小数";
                    alert(valuationMethodErrMsg);
                    return false;
                }
                if (valuationMethodErrMsg != "") {
                    alert(valuationMethodErrMsg);
                    return false;
                }
                freightTempContent = [freightTempContent, '{\"RegionIds\":"', areaContent, '",\"DefaultNumber\":"', defaultNumber, '",\"DefaultPrice\":"', defaultPrice, '",\"AddNumber\":"', addNumber, '",\"AddPrice\":"', addPrice, '",\"RegionNames\":\"\"},'].join('');
            }
        });
        if (valuationMethodErrMsg != "") {
            return false;
        }
       

        //包邮地区模板运费检查
        var freeContent = '';
        var freeTempContent = '';
        var conditionType = 0, conditionNumber = 0;
        $('#regionFree tr').each(function (idx, el) {
            if (idx > 0 && valuationMethodErrMsg == '') {
                freeContent = $('td', el).eq(0).find('p').attr('data-storage') || '';
                conditionType = parseFloat($('td', el).find('.setfreeshipping').val() || 0);
                var conditions = $('td', el).find("input");
                conditionNumber = parseFloat(conditions.eq(0).val() || 0);
                if (conditions.length > 1)
                    conditionNumber += "$" + parseFloat(conditions.eq(1).val() || 0);
                if (freeContent == "") {
                    valuationMethodErrMsg += "运送地区不能为空，请检查";
                    alert(valuationMethodErrMsg);
                    return false;
                }
                
                if (conditionNumber == "") {
                    valuationMethodErrMsg += "指定包邮第" + idx + "项未填数值！";
                    alert(valuationMethodErrMsg);
                    return false;
                } else {
                    if (! /(^\d+(\$\d+)?$)/.test(conditionNumber)) {
                        valuationMethodErrMsg += "指定包邮第" + idx + "项数值请输入整数！";
                        alert(valuationMethodErrMsg);
                        return false;
                    }
                }
                if (valuationMethodErrMsg != "") {
                    alert(valuationMethodErrMsg);
                    return false;
                }
                freeTempContent = [freeTempContent, '{\"RegionIds\":"', freeContent, '",\"ConditionType\":"', conditionType, '",\"ConditionNumber\":"', conditionNumber, '"},'].join('');
            }
        });
        if (valuationMethodErrMsg != "") {
            return false;
        }
        if (freightTempContent.length > 0) {
            freightTempContent = freightTempContent.substr(0, freightTempContent.length - 1);
        }
        freightTempContent = '[' + freightTempContent + ']';
        $("#hidRegionJson").val(freightTempContent);

        if (freeTempContent.length > 0) {
            freeTempContent = freeTempContent.substr(0, freeTempContent.length - 1);
        }
        freeTempContent = '[' + freeTempContent + ']';
        $("#hidFreeJson").val(freeTempContent);
    }

    return true;
}


function checkTemplateName(templateName) {
    var templateId = getParam("templateId");
    $.ajax({
        url: "AddShippingTemplate?action=ValidTemplateName",
        type: 'post', dataType: 'json', timeout: 10000,
        data: {
            IsCallback: "true",
            TemplateName: templateName,
            TemplateId: templateId
        },
        async: true,
        success: function (resultData) {
            if (resultData.Status != "OK") {
                checkNameSuccess = false;
                $("#ctl00_contentHolder_txtModeNameTip").html(resultData.Message).css("color", "red").show();
            }
            else {
                checkNameSuccess = true;

                $("#ctl00_contentHolder_txtModeNameTip").html("").css("color", "red").show();
                if (templateId > 0) {
                    $("#btnUpdate").trigger("click");
                }
                else {
                    $("#btnCreate").trigger("click");
                }
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus);
        },

    });
}

