
//定义应用与注册路由
(function () {
    //定义应用
    var app = angular.module("FreightTemplate", []);

    initProvinceData();

    var regionJson = null;
    if ($("#hidRegionJson").val() != "") {
        regionJson = JSON.parse($("#hidRegionJson").val());
        initRegionData(regionJson);
    }

    //包邮选择框事件
    $('#regionFree').on('change', '.setfreeshipping', function () {
        $(this).next().empty();
        switch ($(this).val()) {
            case '1':
                $(this).next().append('满<input type="text" value="" class="form-control mlr">件包邮');
                break;
            case '2':
                $(this).next().append('满<input type="text" value="" class="form-control mlr">元包邮');
                break;
            case '3':
                $(this).next().append('满<input type="text" value="" class="form-control mlr">件，<input type="text" value="" class="form-control"> 元包邮');
                break;
            default:
                return;
        }
    });

    if ($("#hidFreeJson").val() != "") {
        $(".hasFree").show();
        var freeJson = JSON.parse($("#hidFreeJson").val());
        initFreeData(freeJson);
    }

}());

function initProvinceData() {
    $.ajax({
        url: "/Handler/RegionHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "GetRegionsOfProvinceCounty" },
        async: false,
        success: function (resultData) {
            province = resultData.province;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus);

        },
        complete: function (XMLHttpRequest, textStatus) {
            //  alert(XMLHttpRequest + "-" + textStatus);
        }
    });
}
var province;



function initRegionData(regionJson) {
    $(regionJson).each(function (index) {

        var str = '<tr><td><a class="exit-area">编辑</a><div class="area-group" ><p data-area="">' + regionJson[index].RegionNames + '</p></div></td><td><input type="text" class="form-control input-xs" value="' + regionJson[index].DefaultNumber + '"/></td><td><input type="text" class="form-control input-xs" value="' + regionJson[index].DefaultPrice + '" /></td><td><input type="text" class="form-control input-xs" value="' + regionJson[index].AddNumber + '"/></td><td><input type="text" class="form-control input-xs" value="' + regionJson[index].AddPrice + '" /></td><td><span class=\"btn-a\"><a name=\"delContent\">删除</a></span></td></tr>';
        $("#regionFreight").find('tbody').append(str);
        $("#regionFreight tbody tr").last().find("td p").attr("data-storage", regionJson[index].RegionIds);
        bindProvinceItem(regionJson[index].RegionIds);
        $('a[name="delContent"]').click(function () {
            $(this).parent().parent().parent().remove();
        });
    });
}

function initFreeData(regionJson) {
    $(regionJson).each(function (index) {

        var str = '<tr><td><a class="exit-area">编辑</a><div class="area-group" ><p data-area="">' + regionJson[index].RegionNames + '</p></div></td><td><select class="setfreeshipping form-control fl" style="width:100px;"><option value="1">件数</option><option value="2">金额</option><option value="3">件数+金额</option></select><span class="free-contion">满<input type="text" value="" class="form-control mlr">件包邮</span></td><td><span class=\"btn-a\"><a name=\"delFree\">删除</a></span></td></tr>';
        $("#regionFree").find('tbody').append(str);
        $("#regionFree tbody tr").last().find(".setfreeshipping").val(regionJson[index].ConditionType);
        $("#regionFree tbody tr").last().find(".setfreeshipping").trigger("change");
        var $Conditions = $("#regionFree tbody tr").last().find("input");
        if ($Conditions.length > 1) {
            var tmps = regionJson[index].ConditionNumber.split("$");
            $Conditions.eq(0).val(tmps[0]);
            $Conditions.eq(1).val(tmps[1]);
        } else {
            $Conditions.val(regionJson[index].ConditionNumber);
        }
        $("#regionFree tbody tr").last().find("td p").attr("data-storage", regionJson[index].RegionIds);
        bindProvinceItem(regionJson[index].RegionIds);
        $('a[name="delFree"]').click(function () {
            $(this).parent().parent().parent().remove();
            if ($("#regionFree tr").length == 1)
                $('input[name="ctl00$contentHolder$ctl00"]').bootstrapSwitch("state", false);
        });
    });
}


function bindProvinceItem(regionIds) {
    var arr = [];
    var searchIds = [];
    for (var i = 0; i < regionIds.split(",").length; i++) {
        searchIds.push(parseInt(regionIds.split(",")[i]));
    }
    var chinaArea = '';
    searchIds.forEach(function (val) {
        arr.push({ RegionId: val });
    });

    var tempArr = [];
    $.each(arr, function (index, value) {
        tempArr.push(value.RegionId);
    })

    var htmlpaname = '';
    var nameList = [];
    $.each(province, function (index, prov) {

        var provName = prov.name;
        var tempProv = { id: prov.id, name: prov.name, city: [] };

        if (prov.city && prov.city.length > 0) {
            $.each(prov.city, function (index1, city) {

                var tempCity = { id: city.id, name: city.name, county: [] };
                var findIndex = tempArr.indexOf(city.id);
                if (findIndex > -1) {
                    tempProv.city.push(tempCity);
                    tempArr.splice(findIndex, 1);
                }
                else {
                    if (city.county && city.county.length > 0) {
                        $.each(city.county, function (index2, county) {


                            var findIndex = tempArr.indexOf(county.id);
                            if (findIndex > -1) {
                                tempCity.county.push({ id: county.id, name: county.name });

                                tempArr.splice(findIndex, 1);
                            }

                        });
                    }


                    if (tempCity.county.length > 0) {
                        tempProv.city.push(tempCity);
                    }
                    if (city.county && tempCity.county.length == city.county.length) {
                        tempCity.county = [];
                    }

                }
            });
        }
        if (tempProv.city.length > 0) {
            nameList.push(tempProv);
        }
        if (tempProv.city && prov.city.length == tempProv.city.length) {

            var hasChild = false;
            $.each(tempProv.city, function (index, tvalue) {

                if (tvalue.county && tvalue.county.length > 0) {
                    hasChild = true;
                    return false;
                }
            });

            if (!hasChild) {
                tempProv.city = [];
            }
        }
    });
    if (nameList.length > 0) {
        $.each(nameList, function (index3, value3) {
            htmlpaname += value3.name;
            if (value3.city && value3.city.length > 0) {
                htmlpaname += "(";
            }
            $.each(value3.city, function (index4, value4) {
                htmlpaname += value4.name;
                if (value4.county && value4.county.length > 0) {
                    htmlpaname += ":";
                }
                $.each(value4.county, function (index5, value5) {
                    htmlpaname += value5.name;
                    if (index5 < value4.county.length - 1) {
                        htmlpaname += "、";
                    }
                });
                htmlpaname += "；";
            });
            if (value3.city && value3.city.length > 0) {
                htmlpaname += ")";
            }
            if (index3 < nameList.length - 1) {
                htmlpaname += "；";
            }
        });
    }
    chinaArea = htmlpaname;

    if (chinaArea.length > 0) {
        $('.area-group p[data-area]').attr({
            'data-storage': searchIds.join(','),
            'title': chinaArea
        }).text("");
        if (chinaArea.indexOf("；") == -1) {
            var p = '<span class="province">' + chinaArea + '；'
            $('.area-group p[data-area]').html(p);

        } else {
            var str1 = chinaArea;
            var reg = /([\u4e00-\u9fa5]+\；)|([\u4e00-\u9fa5]+\(([\u4e00-\u9fa5]+\；)+\))|([\u4e00-\u9fa5]+\(([^\x00-\xff]+\:[^\x00-\xff]+\；)+\))|([\u4e00-\u9fa5]+\；)/g;
            var arr = str1.split("；")

            var matchs = str1.match(reg)

            var newArr = matchs.slice(0);

            if (arr[arr.length - 1].indexOf("省") > -1 || arr[arr.length - 1].indexOf("自治区") > -1 || arr[arr.length - 1].indexOf("海") > -1 || arr[arr.length - 1].indexOf("京") > -1) {
                newArr.push(arr[arr.length - 1])
            }
            var reg2 = /[\u4e00-\u9fa5]+\(/

            var reg3 = /\([\u4e00-\u9fa5]{2,}\；|[\u4e00-\u9fa5]{2,}\；|[\u4e00-\u9fa5]{2,}\:[^\x00-\xff]{2,}\；/g;

            var reg4 = /\:[\u4e00-\u9fa5]{2,}|\、[^\x00-\xff]{2,}/g;



            if (matchs) {

                for (var p = 0; p < newArr.length; p++) {

                    if (newArr[p].match(reg2)) {

                        var provinces = $('<span class="province"></span>').html(newArr[p].substring(0, newArr[p].indexOf("(")) + '<span class="contain">(</span>')

                        $('.area-group p[data-area]').append(provinces);

                        if (newArr[p].match(reg3)) {
                            for (c = 0; c < newArr[p].match(reg3).length; c++) {
                                var end = newArr[p].match(reg3)[c].indexOf(":") > 0 ? newArr[p].match(reg3)[c].indexOf(":") : (newArr[p].match(reg3)[c].length);
                                var city = $('<span class="city"></span>').html(newArr[p].match(reg3)[c].substring(0, end))
                                //$(".province").eq(p).append(city)
                                $('.area-group p[data-area]').find(".province").eq(p).append(city)



                                if (newArr[p].match(reg3)[c].match(reg4)) {
                                    for (var s = 0; s < newArr[p].match(reg4).length; s++) {
                                        var county = $('<span class="county"></span>').html(newArr[p].match(reg3)[c].match(reg4)[s])
                                        //$(".province").eq(p).find(".city").eq(c).append(county)
                                        $('.area-group p[data-area]').find(".province").eq(p).find(".city").eq(c).append(county)
                                    }


                                }


                            }


                        }

                    } else {
                        var end = newArr[p].indexOf("；") > -1 ? newArr[p].indexOf("；") : newArr[p].length;
                        var provinces = $('<span class="province"></span>').html(newArr[p].substring(0, end))
                        $('.area-group p[data-area]').append(provinces);
                    }

                }

                for (var i = 0; i < $(".province").length; i++) {
                    if ($(".province").eq(i).find(".city").eq(0).text()) {

                        if ($(".province").eq(i).find(".city").eq(0).text().indexOf("(") > -1) {
                            var text = $(".province").eq(i).find(".city").eq(0).text().substring(1);
                            $(".province").eq(i).find(".city").eq(0).text(text)

                        }

                    }
                }


                //$(".province").append('<span class="contain">)；</span>');
                $('.area-group p[data-area]').find(".province").append('<span class="contain">)；</span>');

                for (var i = 0; i < $(".province").length; i++) {
                    if ($(".province").eq(i).find(".contain").eq(0).text() == ")；") {
                        $(".province").eq(i).find(".contain").eq(0).text("；");

                    }
                }
            }
        }
    }

    $('.area-group p').removeAttr('data-area');
}

//定义控制器
(function (app) {
    app.controller('areaCtrl', ['$scope', function ($scope) {
        $scope.optional = angular.copy(province);
        $scope.optional.forEach(function (value) {
            value.isShow = true;
            value.isToggle = true;
            value.isenable = false;
            value.city.forEach(function (value1) {
                value1.isenable = false;
                if (value1.county) {
                    value1.county.forEach(function (value2) {
                        value2.isenable = false;
                    });
                }
            });
        })
        $scope.cancelAreaModal = function () {
            $('.area-modal-wrap').hide();
            $scope.optional = angular.copy(province);
            $scope.optional.forEach(function (value) {
                value.isShow = true;
                value.isToggle = true;
            })
        }
        $scope.showChild = function (item, event) {//显示隐藏子级
            if (event) {
                event.stopPropagation();
                var self = $(event.target);
                if (self.text() == '+') {
                    self.text('-');
                } else {
                    self.text('+');
                }
            }
            if (item.city) {
                item.city.forEach(function (value) {
                    if (!value.isShow) {
                        value.isShow = !value.isShow;
                    }
                    value.isToggle = !value.isToggle;
                })
            }
            if (item.county) {
                item.county.forEach(function (value) {
                    if (!value.isShow) {
                        value.isShow = !value.isShow;
                    }
                    value.isToggle = !value.isToggle;
                })
            }
            if (item.street) {
                item.street.forEach(function (value) {
                    if (!value.isShow) {
                        value.isShow = !value.isShow;
                    }
                    value.isToggle = !value.isToggle;
                })
            }
        }
        $scope.showChildR = function (item, event) {//显示隐藏子级
            event.stopPropagation();
            var self = $(event.target);
            if (self.text() == '+') {
                self.text('-');
            } else {
                self.text('+');
            }
            if (item.city) {
                item.city.forEach(function (value) {
                    value.isShow = !value.isShow;
                    value.isToggle = !Boolean(value.isToggle);
                })
            }
            if (item.county) {
                item.county.forEach(function (value) {
                    value.isShow = !value.isShow;
                    value.isToggle = !Boolean(value.isToggle);
                })
            }
            if (item.street) {
                item.street.forEach(function (value) {
                    value.isShow = !value.isShow;
                    value.isToggle = !Boolean(value.isToggle);
                })
            }
        }
        $scope.selectToggle = function (item, provinceItem) {
            var isSelected = !Boolean(item.isSelected);
            item.isSelected = isSelected;
            if (item.city) {
                item.city.forEach(function (city) {
                    city.isSelected = isSelected;
                    if (city.county && city.county.length > 0) {
                        city.county.forEach(function (county) {
                            county.isSelected = isSelected;
                            if (county.street) {
                                county.street.forEach(function (county) {
                                    county.isSelected = isSelected;
                                })
                            }
                        })
                    }
                })
            } else if (item.county && item.county.length > 0) {
                item.county.forEach(function (county) {
                    county.isSelected = isSelected;
                    if (county.street && county.street.length > 0) {
                        county.street.forEach(function (county) {
                            county.isSelected = isSelected;
                        })
                    }
                })
            } else if (item.street) {
                item.street.forEach(function (county) {
                    county.isSelected = isSelected;
                })
            }
            selectRecursion(provinceItem.city, provinceItem);//重置选择项
        }
        $scope.removeArea = function (item, provinceItem) {
            item.isShowr = false;
            item.isShow = true;
            if (item.id == provinceItem.id) {
                setChildIsShowr(provinceItem.city);
            }
            removeRecursion(provinceItem.city, provinceItem, item);

            selectRecursion(provinceItem.city, provinceItem);//重置选择项
        }
        $scope.addArea = function () {
            addRecursion($scope.optional, null);
        }
        $scope.areaId = [];
        $scope.saved = function () {
            var areaId = '';
            searchIds = [];
            chinaArea = '';
            //searchIdRecursion($scope.optional);
            chinaShowFullName($scope.optional, null)
            var arr = [];
            searchIds.forEach(function (val) {
                arr.push({ RegionId: val });
            });
            chinaArea = findProviceNameAndCount(arr);
            $('.area-modal-wrap').hide();
            if (chinaArea.length > 0) {
                $('.area-group p[data-area]').attr({
                    'data-storage': searchIds.join(','),
                    'title': chinaArea
                }).text("");
                if (chinaArea.indexOf("；") == -1) {
                    var p = '<span class="province">' + chinaArea + '；'
                    $('.area-group p[data-area]').html(p);

                } else {
                    var str1 = chinaArea;
                    var reg = /([\u4e00-\u9fa5]+\；)|([\u4e00-\u9fa5]+\(([\u4e00-\u9fa5]+\；)+\))|([\u4e00-\u9fa5]+\(([^\x00-\xff]+\:[^\x00-\xff]+\；)+\))|([\u4e00-\u9fa5]+\；)/g;
                    var arr = str1.split("；")

                    var matchs = str1.match(reg)

                    var newArr = matchs.slice(0);

                    if (arr[arr.length - 1].indexOf("省") > -1 || arr[arr.length - 1].indexOf("自治区") > -1 || arr[arr.length - 1].indexOf("海") > -1 || arr[arr.length - 1].indexOf("京") > -1) {
                        newArr.push(arr[arr.length - 1])
                    }
                    var reg2 = /[\u4e00-\u9fa5]+\(/

                    var reg3 = /\([\u4e00-\u9fa5]{2,}\；|[\u4e00-\u9fa5]{2,}\；|[\u4e00-\u9fa5]{2,}\:[^\x00-\xff]{2,}\；/g;

                    var reg4 = /\:[\u4e00-\u9fa5]{2,}|\、[^\x00-\xff]{2,}/g;



                    if (matchs) {

                        for (var p = 0; p < newArr.length; p++) {

                            if (newArr[p].match(reg2)) {

                                var provinces = $('<span class="province"></span>').html(newArr[p].substring(0, newArr[p].indexOf("(")) + '<span class="contain">(</span>')

                                $('.area-group p[data-area]').append(provinces);

                                if (newArr[p].match(reg3)) {
                                    for (c = 0; c < newArr[p].match(reg3).length; c++) {
                                        var end = newArr[p].match(reg3)[c].indexOf(":") > 0 ? newArr[p].match(reg3)[c].indexOf(":") : (newArr[p].match(reg3)[c].length);
                                        var city = $('<span class="city"></span>').html(newArr[p].match(reg3)[c].substring(0, end))
                                        //$(".province").eq(p).append(city)
                                        $(target).next().find(".province").eq(p).append(city)



                                        if (newArr[p].match(reg3)[c].match(reg4)) {
                                            for (var s = 0; s < newArr[p].match(reg4).length; s++) {
                                                var county = $('<span class="county"></span>').html(newArr[p].match(reg3)[c].match(reg4)[s])
                                                //$(".province").eq(p).find(".city").eq(c).append(county)
                                                $(target).next().find(".province").eq(p).find(".city").eq(c).append(county)
                                            }


                                        }


                                    }


                                }

                            } else {
                                var end = newArr[p].indexOf("；") > -1 ? newArr[p].indexOf("；") : newArr[p].length;
                                var provinces = $('<span class="province"></span>').html(newArr[p].substring(0, end))
                                $('.area-group p[data-area]').append(provinces);
                            }

                        }

                        for (var i = 0; i < $(".province").length; i++) {
                            if ($(".province").eq(i).find(".city").eq(0).text()) {

                                if ($(".province").eq(i).find(".city").eq(0).text().indexOf("(") > -1) {
                                    var text = $(".province").eq(i).find(".city").eq(0).text().substring(1);
                                    $(".province").eq(i).find(".city").eq(0).text(text)

                                }

                            }
                        }


                        //$(".province").append('<span class="contain">)；</span>');
                        $(target).next().find(".province").append('<span class="contain">)；</span>');

                        for (var i = 0; i < $(".province").length; i++) {
                            if ($(".province").eq(i).find(".contain").eq(0).text() == ")；") {
                                $(".province").eq(i).find(".contain").eq(0).text("；");

                            }
                        }
                    }
                }

            }
            $('.area-group p').removeAttr('data-area');
            $scope.optional = angular.copy(province);
            $scope.optional.forEach(function (value) {
                value.isShow = true;
                value.isToggle = true;
            })
        }
        var target;
        $('body').on('click', '.exit-area', function () {
            target = this;
            var areaListId = $(this).next().find('p').attr('data-storage');
            if (areaListId) {
                areaListId = areaListId.split(',');
                editArea($scope.optional, $scope.optional, areaListId);
            }
            //同一个地址，不能出现重复添加
            var siblings = $(this).parent().parent().siblings()
            var areaListEable = "";
            for (i = 0; i < siblings.length; i++) {
                areaListEable += siblings.eq(i).find('p').attr('data-storage') + ",";
            }
            areaListEable = areaListEable.substring(0, areaListEable.length - 1);
            if (areaListEable) {
                areaListEable = areaListEable.split(',');
                var tempArr = [];
                $.each(areaListEable, function (index, value) {
                    tempArr.push(parseInt(value));
                })
                DeleteOnSelect($scope.optional, tempArr)
            }
            $scope.$apply();
            $('.area-modal-wrap').show();

            $(this).next().find('p').attr('data-area', true);

        })
    }])

    //运费模板列表
    var ListController = function ($scope, $http, $routeParams) {
        $scope.Usertype = $routeParams.Usertype;
        //与服务器通信
        $http.post("/FreightTemplate/GetFreightTemplateList", { "Usertype": $scope.Usertype })
             .success(function (data) {
                 $scope.list = data;
             });
        //获取运送方式名称
        $scope.GetModelName = function (id) {
            switch (id) {
                case 1:
                    return '快递';
                case 2:
                    return 'EMS';
                case 3:
                    return '顺丰';
                case 4:
                    return '平邮';

            }
            return "快递";
        }

        //获取可送至地区信息

        $scope.InitAreaOn = function (arr) {
            if (arr == null || arr.length <= 0) {
                return '全国';
            } else {
                return findProviceNameAndCount(arr);
            }
        }

        $scope.ShowPopover = function (objid, arr) {
            var obj = $(".Regions[tabindex='" + objid + "']");
            obj.attr("data-content", findProviceNameAndCount(arr));
            obj.popover('show')
        }

        //删除
        $scope.Delete = function (id) {
            popnews.confirm("确认删除选择的模板，删除后不可恢复？", null, function () {
                $http.post("/FreightTemplate/DeleteFreightTemplate", { "id": id })
                     .success(function (data) {
                         if (data.Status) {
                             HiTipsShow("删除成功！", 'success');
                             for (var i = 0; i < $scope.list.length; i++) {
                                 if ($scope.list[i].Id == id) {
                                     $scope.list.splice(i, 1);
                                     break;
                                 }
                             }
                         } else {
                             HiTipsShow(data.Msg, 'fail');
                         }
                     });
            });
        }
        //全选
        $scope.SelectAll = function () {
            var checked = $("#selectAllInput").prop('checked');
            var inputs = $("input[name='CheckBoxGroup']");
            inputs.each(function () {
                $(this).prop('checked', checked);
            });

        }
        //批量删除
        $scope.DeleteSelect = function () {
            var inputs = $("input[name='CheckBoxGroup']:checked");
            if (inputs.length > 0) {
                var ids = '';
                inputs.each(function () {
                    ids += $(this).val() + '_';
                });
                ids = ids.substr(0, ids.length - 1);
                popnews.confirm("确认删除选择的模板，删除后不可恢复？", null, function () {
                    //删除
                    $http.post("/FreightTemplate/DeleteFreightTemplateSelect", { "ids": ids })
                         .success(function (data) {
                             if (data.Status) {
                                 if ($.trim(data.Msg).length > 0) {
                                     HiTipsShow("部分模版删除成功，系统不能删除的模版如下：</br>" + data.Msg, 'warning', function () {
                                         location.reload();
                                     });
                                 } else {
                                     HiTipsShow("全部删除成功！", 'success');
                                     location.reload();
                                     //var tempids = '_' + ids + '_';
                                     //for (var i = 0; i < $scope.list.length; i++) {
                                     //    if (tempids.indexOf('_' + $scope.list[i].Id + '_') > -1) {
                                     //        $scope.list.splice(i, 1);
                                     //    }
                                     //}
                                 }
                             } else {
                                 HiTipsShow(data.Msg, 'fail');
                             }
                         });
                });
            } else {
                HiTipsShow('请选择您要删除的模版!', 'fail');
            }
        }
    }

    app.controller("ListController", ListController);

    //添加运费模板
    var AddController = function ($scope, $http, $routeParams) {
        $scope.Usertype = $routeParams.Usertype;
        VaildateInput();
        Init();
    };
    app.controller("AddController", AddController);

    //编辑运费模板
    var EditController = function ($scope, $http, $routeParams) {
        var id = $routeParams.id;
        $scope.Usertype = $routeParams.Usertype;
        $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
            BindData(id);
        });
    };
    app.controller("EditController", EditController);

}(angular.module("FreightTemplate")));

//function queryId(listId, region) {//根据id编辑已选
//    region.forEach(function (value) {
//        var tempList = [];
//        if (value.city) {
//            tempList = value.city;
//        }
//        else if (value.county) {
//            tempList = value.county;
//        }
//        $.each(listId, function (index, item) {
//            if (item == value.id) {
//                value.isShowr = true;
//                listId.splice(index, 1);
//                return false;
//            }
//        })
//        queryId(listId, tempList);
//    })
//    addRecursion($scope.optional, null);
//}

var searchIds = [];
var chinaArea = '';
function searchIdRecursion(list) {//查询选择id
    list.forEach(function (value) {
        var tempList = [];
        if (value.city) {
            tempList = value.city;
        }
        else if (value.county) {
            tempList = value.county;
        }

        if (value.isShowr && tempList != null && tempList.length < 1) {
            searchIds.push(value.id);
            chinaArea += value.name + '、';
        }
        searchIdRecursion(tempList);
    })
}

function chinaShowFullName(list, topItem, toplist) {//查询选择id
    var childLen = 0;
    list.forEach(function (value) {
        var tempList = [];
        var isCountryAll = false;
        if (value.city) {
            tempList = value.city;
        }
        else if (value.county) {
            tempList = value.county;
            isCountryAll = true;
            tempList.forEach(function (cvalue) {
                if (!cvalue.isShowr) {
                    isCountryAll = false;
                }
            });
        }
        //如果市下面所有的区都已包含则要将市级ID保存
        if (value.isShowr && tempList != null && (tempList.length < 1 || isCountryAll)) {
            searchIds.push(value.id);
        }
        else {

        }

        if (value.isShowr) {
            childLen++;
        }


        chinaShowFullName(tempList, value, list);
    })

}

function setChildIsShowr(list) {//设置子级是否显示
    $.each(list, function (index, value) {

        value.isShowr = false;
        var tempList = [];
        if (value.city) {
            tempList = value.city;
        }
        else if (value.county) {
            tempList = value.county;
        } else if (value.street) {
            tempList = value.street;
        }
        setChildIsShowr(tempList);
    }
    );
}
function removeRecursion(list, topItem, findItem) {//删除已选择
    var childLen = 0;
    var flag = list.length;

    $.each(list, function (index, value) {

        var tempList = [];
        if (value.city) {
            tempList = value.city;
        }
        else if (value.county) {
            tempList = value.county;
        } else if (value.street) {
            tempList = value.street;
        }

        if (value.id == findItem.id) {
            setChildIsShowr(tempList)
        }
        else {

            if (tempList != null && tempList.length > 0) {
                removeRecursion(tempList, value, findItem);
            }
        }

        if (value.isShowr) {
            childLen++;
            flag--;
        }
    })

    if (topItem != null) {
        if (childLen > 0) {
            topItem.isShowr = true;
        } else {
            topItem.isShowr = false;
            topItem.isShow = true;
        }

        if (flag <= 0) {


            // topItem.isShow = false;

            var checkObj = { hasChild: false };

            findBottom(list, checkObj);

            if (checkObj.hasChild) {
                if (topItem.city) {
                    topItem.isShow = true;
                }

            }
            else {
                topItem.isShow = false;
            }

        }

    }
}


function findBottom(list, checkObj) {//设置子级是否显示
    $.each(list, function (index, value) {
        var tempList = [];
        if (value.city) {
            tempList = value.city;
        }
        else if (value.county) {
            tempList = value.county;
        } else if (value.street) {
            tempList = value.street;
        }
        findBottom(tempList, checkObj);

        if (!value.isShowr) {
            checkObj.hasChild = true;
        }
    }
    );
}
function editArea(list, topItem, areaListId) {//编辑
    var childLen = 0;
    var flag = list.length;
    list.forEach(function (value) {
        if (value.city) {
            editArea(value.city, value, areaListId)
        }
        else if (value.county) {
            editArea(value.county, value, areaListId)
        } else if (value.street) {
            editArea(value.street, value, areaListId)
        }
        $.each(areaListId, function (index, val) {
            if (value.id == val) {
                value.isShowr = true;
                value.isToggle = true;
                value.isShow = false;
                areaListId.splice(index, 1);
                return false;
            }
        })
        if (value.isShowr) {
            childLen++;
            flag--;
        }
    });


    if (topItem != null) {
        if (childLen > 0) {
            topItem.isShowr = true;
        } else {
            topItem.isShowr = false;
        }
        if (flag <= 0) {
            //topItem.isShow = false;
            //topItem.isShow = true;

            var checkObj = { hasChild: false };

            findBottom(list, checkObj);

            if (checkObj.hasChild) {
                if (topItem.city) {
                    topItem.isShow = true;
                }

            }
            else {
                topItem.isShow = false;
            }

        }
    }
}


function DeleteOnSelect(list, tempArr) {
    $.each(list, function (index, prov) {
        var provName = prov.name;
        var tempProv = { id: prov.id, name: prov.name, city: [] };

        if (prov.city && prov.city.length > 0) {
            $.each(prov.city, function (index1, city) {

                var tempCity = { id: city.id, name: city.name, county: [] };
                var findIndex = tempArr.indexOf(city.id);
                if (findIndex > -1) {
                    city.isenable = true;
                    tempProv.city.push(tempCity);
                    tempArr.remove(findIndex);
                }
                else {
                    city.isenable = false;
                    if (city.county && city.county.length > 0) {
                        $.each(city.county, function (index2, county) {
                            var findIndex = tempArr.indexOf(county.id);
                            if (findIndex > -1) {
                                county.isenable = true;
                                tempCity.county.push({ id: county.id, name: county.name })
                                tempArr.remove(findIndex);
                            }
                            else {
                                county.isenable = false;
                            }

                        });
                    }
                    if (tempCity.county.length > 0) {
                        tempProv.city.push(tempCity);
                    }
                    if (city.county && tempCity.county.length == city.county.length && tempCity.county.length > 0) {
                        //tempCity.county = [];
                        city.isenable = true;
                    }
                    else {
                        city.isenable = false;
                    }

                }
            });
        }
        if (tempProv.city && prov.city.length == tempProv.city.length && tempProv.city.length > 0) {

            var isenable = true;
            $.each(prov.city, function (index, tvalue) {
                if (!tvalue.isenable) {
                    isenable = false;
                    return false;
                }
            });
            prov.isenable = isenable;
        }
        else {
            prov.isenable = false;
        }
    });
    //var childLen = 0;
    //var flag = list.length;
    //list.forEach(function (value) {
    //    if (value.city) {
    //        DeleteOnSelect(value.city, value, areaListId)
    //    }
    //    else if (value.county) {
    //        DeleteOnSelect(value.county, value, areaListId)
    //    } 
    //    $.each(areaListId, function (index, val) {
    //        if (value.id == val) {
    //            value.isenable = true;
    //            areaListId.splice(index, 1);
    //            return false;
    //        }
    //    })
    //    if (value.isenable) {
    //        childLen++;
    //        flag--;
    //    }
    //});
    //if (topItem != null) {
    //    var templenght=0;
    //    if(topItem.city)
    //    {
    //        templenght=topItem.city.length;
    //    }
    //    if(topItem.county)
    //    {
    //        templenght=topItem.county.length;
    //    }
    //    if (childLen == templenght-1) {
    //        topItem.isenable = true ;
    //    } else {
    //        topItem.isenable = false;
    //    }

    //}
}

function addRecursion(list, topItem) {//添加到右边
    var childLen = 0;
    var flag = list.length;
    list.forEach(function (value) {
        if (value.city) {
            addRecursion(value.city, value)
        }
        else if (value.county) {
            addRecursion(value.county, value)
        }
        if (value.isSelected && !value.isenable) {
            value.isShowr = true;
            value.isShow = false;
        }
        if (value.isShowr) {
            childLen++;
            flag--;
        }
    });

    if (topItem != null) {
        if (childLen > 0) {
            topItem.isShowr = true;
        } else {
            topItem.isShowr = false;
        }
        if (flag <= 0) {
            //topItem.isShow = false;
            //topItem.isShow = true;

            var checkObj = { hasChild: false };

            findBottom(list, checkObj);

            if (checkObj.hasChild) {
                if (topItem.city) {
                    topItem.isShow = true;
                }

            }
            else {
                topItem.isShow = false;
            }

        }
    }
}



function selectRecursion(list, topItem) {//重置选择方法
    var childLen = 0;
    childLen = list.length;
    if (childLen > 0) {
        list.forEach(function (value) {
            if (value.county) {
                selectRecursion(value.county, value)
            } else if (value.street) {
                selectRecursion(value.street, value)
            }

            if (value.isSelected) {
                childLen--;
            }
        });
    }
    if (childLen == 0) {
        if (list.length > 0) {
            topItem.isSelected = true;
        }
    } else {
        topItem.isSelected = false;
    }
}

function BindData(id) {
    editId = id;
    Init();
    $.ajax({
        url: '/FreightTemplate/GetFreightTemplate/' + id,
        type: 'post',
        dataType: 'json',
        success: function (data) {
            initEdit(data);
        }
    });
}

//验证数据
function VaildateInput() {
    $("#aspnetForm").bootstrapValidator({
        fields: {
            templateName: {
                validators: {
                    notEmpty: {
                        message: '模板名称不能为空！'
                    },
                    stringLength: {
                        min: 2,
                        max: 20,
                        message: '模板名称2-20个字符！'
                    }
                }
            }
        }
    });
}


function initEdit(tempData) {

    if (tempData.Id == null || tempData.Id == "") {
        HiTipsShow("读取模板数据失败，页面两秒后跳转", 'success', function () {
            window.location = "ManageShippingTemplates.aspx";
        });
        return;
    }

    $("#templateId").val(tempData.Id);
    $("#templateId").attr("disabled", true);
    $("#templateName").val(tempData.Name);
    $("input[name=pric][value=" + tempData.MUnit + "]").prop("checked", true);

    switch (tempData.MUnit * 1) {
        case 1:
            str = '件';
            str1 = '件';
            break;
        case 2:
            str = 'kg';
            str1 = "重"
            break;
        case 3:
            str = 'm<sup>3</sup>';
            str1 = "体积"
            break;
    }

    $("input[name=free][value=" + tempData.FreeShip + "]").prop("checked", true);
    if (tempData.FreeShip) {
        $("#shippertypeid").hide();
        $('#freetypeid').hide();
        return;
    }

    //写入相关运送方式数据

    var $thisNext = null;
    var $Region = null;
    var CurrentId = 0;
    $.each(tempData.NotFreeShippings, function (i) {
        var inputShippingType = $("input[name='ShippingType'][value=" + this.ModeId + "]");
        inputShippingType.prop("checked", true);
        inputShippingType.trigger("change"); //.prop("checked", true);
        $thisNext = inputShippingType.parent().next(".freight-editor");

        if (this.IsDefault) {
            var $default = $thisNext.find(".default");
            var $default_input = $default.find("input"); //默认运价input
            $default_input.eq(0).val(this.FristNumber);
            $default_input.eq(1).val(this.FristPrice);
            $default_input.eq(2).val(this.AddNumber);
            $default_input.eq(3).val(this.AddPrice);

            //var $RegionItem_input = $(this).find("input");
        } else {
            var $editor = inputShippingType.parent().next();
            $editor.find(".designated-areas").trigger("click");
            if (CurrentId != this.ModeId) {
                $Region = $thisNext.find(".tbl-except");
            }

            var $RegionRow = $Region.find(".RegionItem:last");

            var $RegionItems = $RegionRow.find("input");
            $RegionItems.eq(0).val(this.FristNumber);
            $RegionItems.eq(1).val(this.FristPrice);
            $RegionItems.eq(2).val(this.AddNumber);
            $RegionItems.eq(3).val(this.AddPrice);


            var row = $RegionRow.index();

            SelData["Group" + this.ModeId].Regions[row].RegionIds = arrToStr(this.FreeShippingRegions);

            $RegionRow.find("p:first").attr("data-storage", arrToStr(this.FreeShippingRegions));
            $RegionRow.find("p:first").html(findProviceNameAndCount(this.FreeShippingRegions));

            CurrentId = this.ModeId;
        }
    })




    //勾选指定包邮
    if (!tempData.HasFree) {
        return;
    }
    $("#HasFree").prop("checked", true);
    $("#HasFree").trigger("change");
    var $add = $("#HasFree").parent().next().find(".add");

    //写入相关运送方式数据
    $.each(tempData.FreeShippings, function (i) {
        if (i > 0) {
            $add.trigger("click");
        }
        var $freerow = $(".FreeRegion").eq(i);

        if ($freerow.length > 0) {

            $freerow.find("p:first").attr("data-storage", arrToStr(this.FreeShippingRegions));
            $freerow.find("p:first").html(findProviceNameAndCount(this.FreeShippingRegions));

            $freerow.find(".select-express").val(this.ModeId);
            $freerow.find(".setfreeshipping").val(this.ConditionType);
            $freerow.find(".setfreeshipping").trigger("change");

            SelData.FreeShippings[$freerow.index()].RegionIds = arrToStr(this.FreeShippingRegions);


            var $Conditions = $freerow.find("input");
            if ($Conditions.length > 1) {
                var tmps = this.ConditionNumber.split("$");
                $Conditions.eq(0).val(tmps[0]);
                $Conditions.eq(1).val(tmps[1]);
            } else {
                $Conditions.val(this.ConditionNumber);
            }
        }
    });


}

//获取城市ID对应的省名，重复的分组
function getProvinces(rids) {
    var showHtml = "";
    var provHash = {};
    var tcity = "";
    if (rids.length > 0) {
        $.each(rids, function (i) {
            if (this != "") {
                tcity = findprovince(this);
                if (!provHash[tcity]) {
                    provHash[tcity] = 1
                } else {
                    provHash[tcity]++;
                }
            }
        });
    }
    for (key in provHash) {
        showHtml += key + "、";
    }
    if (showHtml != "") { showHtml = showHtml.substring(0, showHtml.length - 1) }
    return showHtml;
}

function findProviceName(arr) {
    var panmes = '';
    var data = arrToStr(arr).split(',');
    var i = 0;
    for (i; i < province.length; i++) {
        var j = 0;
        var param = "";
        var count = 0;
        for (j; j < province[i]["city"].length; j++) {
            var d = 0;
            for (d; d < data.length; d++) {
                if (province[i]["city"][j]["id"] == data[d]) {
                    param += province[i]["city"][j]["name"] + ",";
                    count++;
                }
            }
        }
        if (count == province[i]["city"].length)
            param = province[i].name + ",";
        panmes += param;
    }
    return panmes.substr(0, panmes.length - 1);
}

function findProviceNameAndCount(arr) {
    var tempArr = [];

    $.each(arr, function (index, value) {
        tempArr.push(value.RegionId);
    })
    var htmlpaname = '';
    var nameList = [];


    $.each(province, function (index, prov) {

        var provName = prov.name;
        var tempProv = { id: prov.id, name: prov.name, city: [] };
        if (prov.city && prov.city.length > 0) {
            $.each(prov.city, function (index1, city) {

                var tempCity = { id: city.id, name: city.name, county: [] };
                var findIndex = tempArr.indexOf(city.id);
                if (findIndex > -1) {
                    tempProv.city.push(tempCity);
                    tempArr.remove(findIndex);
                }
                else {
                    if (city.county && city.county.length > 0) {
                        $.each(city.county, function (index2, county) {


                            var findIndex = tempArr.indexOf(county.id);
                            if (findIndex > -1) {
                                tempCity.county.push({ id: county.id, name: county.name })
                                tempArr.remove(findIndex);
                            }

                        });
                    }


                    if (tempCity.county.length > 0) {
                        tempProv.city.push(tempCity);
                    }
                    if (city.county && tempCity.county.length == city.county.length) {
                        tempCity.county = [];
                    }

                }
            });
        }

        if (tempProv.city.length > 0) {
            nameList.push(tempProv);
        }
        if (tempProv.city && prov.city.length == tempProv.city.length) {

            var hasChild = false;
            $.each(tempProv.city, function (index, tvalue) {

                if (tvalue.county && tvalue.county.length > 0) {
                    hasChild = true;
                    return false;
                }
            });

            if (!hasChild) {
                tempProv.city = [];
            }
        }
    });
    var htmlpaname = '';
    if (nameList.length > 0) {
        $.each(nameList, function (index3, value3) {
            htmlpaname += value3.name;
            if (value3.city && value3.city.length > 0) {
                htmlpaname += "(";
            }
            $.each(value3.city, function (index4, value4) {
                htmlpaname += value4.name;
                if (value4.county && value4.county.length > 0) {
                    htmlpaname += ":";
                }
                $.each(value4.county, function (index5, value5) {
                    htmlpaname += value5.name;
                    if (index5 < value4.county.length - 1) {
                        htmlpaname += "、";
                    }
                });
                htmlpaname += "；";
            });
            if (value3.city && value3.city.length > 0) {
                htmlpaname += ")";
            }
            if (index3 < nameList.length - 1) {
                htmlpaname += "；";
            }
        });
    }
    return htmlpaname;

}



Array.prototype.remove = function (index) {
    if (index > -1) {
        this.splice(index, 1);
    }
};

//获取省信息列城市数
function findProviceNameAndCount_old(arr) {
    var panme = '';
    var proviceId = '';
    var lastProviceId = 0;
    var htmlpaname = '';
    $.each(arr, function () {
        var arrId = Number(this.RegionId);
        var provinceLenht = province.length;
        for (var i = 0; i < provinceLenht; i++) {
            if (i < provinceLenht - 1) {
                if (province[i].id * 1 < arrId && arrId < province[i + 1].id * 1 && arrId > lastProviceId) {
                    $.each(province[i].city, function () {
                        if (this.id * 1 == arrId) {
                            lastProviceId = province[i + 1].id * 1;
                            panme = panme + province[i].name + ',';
                            proviceId = proviceId + province[i].id + ',';
                            return;
                        }
                    });
                }
            } else {//最后一级
                $.each(province[i].city, function () {
                    if (this.id * 1 == arrId) {
                        panme = panme + province[i].name + ',';
                        proviceId = proviceId + province[i].id + ',';
                        lastProviceId = province[i].id * 1;
                        return;
                    }
                });
            }
        }
    });
    var arrProviceId = proviceId.substr(0, proviceId.length - 1).split(',');
    var arrProviceName = panme.substr(0, panme.length - 1).split(',');
    var arrProviceIdCount = arrProviceId.length
    for (var i = 0; i < arrProviceIdCount; i++) {
        var count = 0;
        $.each(arr, function () {
            var arrId = Number(this.RegionId);
            if (i < arrProviceIdCount - 1) {
                if (arrProviceId[i] * 1 < arrId && arrProviceId[i + 1] * 1 > arrId) {
                    count++;
                }
            } else {
                if (arrProviceId[i] * 1 < arrId) {
                    count++;
                }
            }
        });
        htmlpaname += arrProviceName[i] + '(' + count + ')，';
    }
    return htmlpaname.substr(0, htmlpaname.length - 1);
}

//获取省市名称
function findProviceAndCityName(arr) {
    var panme = '';
    var proviceId = '';
    var htmlpaname = '';
    var proviceList = [];
    var value = { proviceName: '', cityName: [] };
    proviceList.push(value);
    $.each(arr, function () {
        var arrId = Number(this.RegionId);
        var provinceLenht = province.length;
        var listLenght = proviceList.length - 1;
        for (var i = 0; i < provinceLenht; i++) {
            if (i < provinceLenht - 1) {
                if (province[i].id * 1 < arrId && arrId < province[i + 1].id * 1) {
                    $.each(province[i].city, function () {
                        if (this.id * 1 == arrId) {
                            if (proviceList[listLenght].proviceName == province[i].name) {
                                proviceList[listLenght].cityName.push(this.name);
                            } else {
                                proviceList.push({ proviceName: province[i].name, cityName: [this.name] });
                            }
                        }
                    });
                }
            } else {//最后一级
                $.each(province[i].city, function () {
                    if (this.id * 1 == arrId) {
                        if (proviceList[listLenght].proviceName == province[i].name) {
                            proviceList[listLenght].cityName.push(this.name);
                        } else {
                            proviceList.push({ proviceName: province[i].name, cityName: [this.name] });
                        }
                    }
                });
            }
        }
    });
    var htmlValue = '';
    $.each(proviceList, function () {
        if (this.proviceName != '') {

            htmlValue += this.proviceName + '【';
            var cityName = '';
            $.each(this.cityName, function () {
                cityName += this + '，';
            });
            htmlValue += cityName.substr(0, cityName.length - 1) + '】';
        }
    });
    return htmlValue;

}


//获取到省名
function findprovince(rid) {
    var pname = "";
    $.each(province, function () {
        if (rid * 1 < this.id * 1) {
            return false;
        } else {
            pname = this.name;
        }
    });
    return pname;

}
///把JSON对象变为str
function arrToStr(arr) {
    var str = '';
    $.each(arr, function () {
        str = str + this.RegionId + ',';
    });
    str = str.substr(0, str.length - 1);
    return str;
}

