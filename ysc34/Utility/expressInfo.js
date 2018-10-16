(function ($) {

    function getExpressData(orderId) {
        var url = '/API/VshopProcess.ashx';
        var expressData;
        $.ajax({
            type: "get",
            url: url,
            data: { action: 'Logistic', orderId: orderId },
            dataType: "json",
            async: false,
            success: function (data) {
                expressData = data;
            }
        });
        return expressData;
    }
    ///获取退货或者换货的物流信息
    function getReturnOrReplaceExpressData(objId, types) {
        var url = '/API/VshopProcess.ashx';
        var expressData;
        var actions = "ReturnLogistic";
        var varName = "ReturnId";
        if (types == "replace") {
            actions = "ReplaceLogistic";
            varName = "ReplaceId";
        }
        $.ajax({
            type: "get",
            url: url,
            data: { action: actions, OperId: objId },
            dataType: "json",
            async: false,
            success: function (data) {
                expressData = data;
            }
        });
        return expressData;
    }

    $.fn.ReturnOrReplaceExpressData = function (objId, types, headhtml) {
        var expressData = getReturnOrReplaceExpressData(objId, types);
        var html = '<table>';
        if (expressData != undefined && expressData.success == true && expressData.state != "此单无物流信息" && expressData.state != "无轨迹") {
            var data = expressData.traces;
            for (var i = 0; i < data.length; i++) {
                html += '<tr><td class="dTime">' + data[i].acceptTime + '</td>\
                             <td>' + data[i].acceptStation + '</td>';
                html += '</tr>';
            }
        }
        else {
            html += '<tr><td>暂无物流信息，请稍后再试！<br>如有信息推送不及时，请到物流公司官网查询！</td></tr>';
            if (headhtml == undefined || headhtml == "") {
                if (expressData.shipperCode.toUpperCase() == "SF") {
                    html += '<tr><td style=\"text-align:right;\"><a href=\"http://www.sf-express.com/cn/sc/dynamic_function/waybill/#search/bill-number/' + expressData.logisticsCode + '\" target=\"_blank\">点击顺丰官网查询</a></td></tr>';
                }
                else {
                    html += '<tr><td style=\"text-align:right;\"><a href=\"https://m.kuaidi100.com/result.jsp?nu=' + expressData.logisticsCode + '\" target=\"_blank\">点击使用快递100查询</a></td></tr>';
                }
            }
        }
        html += '</table>';
        if (headhtml) {
            $(this).html(headhtml + html);
        } else {
            $(this).html(html);
        }
        return $;
    }

    function getReplaceExpressDate(infoType, replaceId) {
        var url = '/API/VshopProcess.ashx';
        var expressData;
        var actions = "ReplaceLogistic";
        $.ajax({
            type: "get",
            url: url,
            data: { action: actions, OperId: replaceId, InfoType: infoType },
            dataType: "json",
            async: false,
            success: function (data) {
                expressData = data;
            }
        });
        return expressData;
    }
    ///获取换货用户发货信息
    $.fn.ReplaceUserExpressData = function (replaceId, headhtml) {
        var expressData = getReplaceExpressDate("User", replaceId);
        var html = '<table>';
        if (expressData != undefined && expressData.success == true && expressData.state != "此单无物流信息" && expressData.state != "无轨迹") {
            var data = expressData.traces;
            for (var i = 0; i < data.length; i++) {
                html += '<tr><td class="dTime">' + data[i].acceptTime + '</td>\
                             <td>' + data[i].acceptStation + '</td>';
                html += '</tr>';
            }
        }
        else {
            html += '<tr><td>暂无物流信息，请稍后再试！<br>如有信息推送不及时，请到物流公司官网查询！</td></tr>';
            if (headhtml == undefined || headhtml == "") {
                if (expressData.shipperCode.toUpperCase() == "SF") {
                    html += '<tr><td style=\"text-align:right;\"><a href=\"http://www.sf-express.com/cn/sc/dynamic_function/waybill/#search/bill-number/' + expressData.logisticsCode + '\" target=\"_blank\">点击顺丰官网查询</a></td></tr>';
                }
                else {
                    html += '<tr><td style=\"text-align:right;\"><a href=\"https://m.kuaidi100.com/result.jsp?nu=' + expressData.logisticsCode + '\" target=\"_blank\">点击使用快递100查询</a></td></tr>';
                }
            }
        }
        html += '</table>';
        if (headhtml) {
            $(this).html(headhtml + html);
        } else {
            $(this).html(html);
        }
        return $;
    }

    ///获取换货商户发货信息
    $.fn.ReplaceMallExpressData = function (replaceId, headhtml) {
        var expressData = getReplaceExpressDate("Mall", replaceId);
        var html = '<table>';
        if (expressData != undefined && expressData.success == true && expressData.state != "此单无物流信息" && expressData.state != "无轨迹") {
            var data = expressData.traces;
            for (var i = 0; i < data.length; i++) {
                html += '<tr><td class="dTime">' + data[i].acceptTime + '</td>\
                             <td>' + data[i].acceptStation + '</td>';
                html += '</tr>';
            }
        }
        else {
            html += '<tr><td>暂无物流信息，请稍后再试！<br>如有信息推送不及时，请到物流公司官网查询！</td></tr>';
            if (headhtml == undefined || headhtml == "") {
                if (expressData.shipperCode.toUpperCase() == "SF") {
                    html += '<tr><td style=\"text-align:right;\"><a href=\"http://www.sf-express.com/cn/sc/dynamic_function/waybill/#search/bill-number/' + expressData.logisticsCode + '\" target=\"_blank\">点击顺丰官网查询</a></td></tr>';
                }
                else {
                    html += '<tr><td style=\"text-align:right;\"><a href=\"https://m.kuaidi100.com/result.jsp?nu=' + expressData.logisticsCode + '\" target=\"_blank\">点击使用快递100查询</a></td></tr>';
                }
            }
        }
        html += '</table>';
        $(this).html(html);
        if (headhtml) {
            $(this).html(headhtml + html);
        } else {
            $(this).html(html);
        }
        return $;
    }

    $.fn.expressInfo = function (orderId, _orderId, headhtml) {
        /// <param name="orderId" type="String">订单编号</param>  
        var expressData = getExpressData(orderId);
        var html = '<table>';

        if (expressData != undefined && expressData.success == true && expressData.state != "此单无物流信息" && expressData.state != "无轨迹") {
            var data = expressData.traces;
            for (var i = 0; i < data.length; i++) {
                html += '<tr><td class="dTime">' + data[i].acceptTime + '</td>\
                             <td>' + data[i].acceptStation + '</td>';
                html += '</tr>';
            }
        }
        else {
            html += '<tr><td>暂无物流信息，请稍后再试！<br>如有信息推送不及时，请到物流公司官网查询！</td></tr>';
            if (headhtml == undefined || headhtml == "") {
                if (expressData.shipperCode.toUpperCase() == "SF") {
                    html += '<tr><td style=\"text-align:right;\"><a href=\"http://www.sf-express.com/cn/sc/dynamic_function/waybill/#search/bill-number/' + expressData.logisticsCode + '\" target=\"_blank\">点击顺丰官网查询</a></td></tr>';
                }
                else {
                    html += '<tr><td style=\"text-align:right;\"><a href=\"https://m.kuaidi100.com/result.jsp?nu=' + expressData.logisticsCode + '\" target=\"_blank\">点击使用快递100查询</a></td></tr>';
                }
            }
        }
        html += '</table>';
        if (headhtml) {
            $(this).html(headhtml + html);
        } else {
            $(this).html(html);
        }
        return $;

    }

})(jQuery);
