

//查看退货物流信息
function ViewReturnLogistics(obj) {
    var returnsid = $(obj).attr("returnsid");
    if (returnsid) {
        var ExpressCompanyName = $(obj).attr("expresscompanyname");
        var ShipOrderNumber = $(obj).attr("shipordernumber")
        var hidHeadHtml = "";
        if (ExpressCompanyName != "同城物流配送" && ExpressCompanyName != "") {
            hidHeadHtml = "<ul id='hidheard' class='buyer_information'><li><span>物流公司：</span>" + ExpressCompanyName + "</li>"
                       + "<li><span>物流单号：</span>" + ShipOrderNumber + "</li>"
            if (ExpressCompanyName.indexOf("顺丰") > -1) {
                hidHeadHtml += "<li style='text-align:right'><a href=\"http://www.sf-express.com/cn/sc/dynamic_function/waybill/#search/bill-number/" + ShipOrderNumber + "\"  target='_blank'>顺丰官网查询></a></li></ul>";
            }
            else {
                hidHeadHtml += "<li style='text-align:right'><a href=\"https://www.kuaidi100.com/chaxun?nu=" + ShipOrderNumber + "\"  target='_blank'>快递100查询></a></li></ul>";
            }
        }
        $('#expressInfo').ReturnOrReplaceExpressData(returnsid, "return", hidHeadHtml);
        ShowMessageDialog("查看退货用户物流", "viewlogistics", "ViewLogistics");
    }

}
//查看换货物流信息
function ViewReplaceLogistics(obj) {
    var ReplaceId = $(obj).attr("replaceid");
    if (ReplaceId)
        if (ReplaceId) {
            var hidHeadHtml = "";
            var ExpressCompanyName = $(obj).attr("expresscompanyname");
            var ShipOrderNumber = $(obj).attr("shipordernumber")
            if (ExpressCompanyName != "同城物流配送" && ExpressCompanyName != "") {
                hidHeadHtml = "<ul id='hidheard' class='buyer_information'><li><span>物流公司：</span>" + ExpressCompanyName + "</li>"
                           + "<li><span>物流单号：</span>" + ShipOrderNumber + "</li>"
                if (ExpressCompanyName.indexOf("顺丰") > -1) {
                    hidHeadHtml += "<li style='text-align:right'><a href=\"http://www.sf-express.com/cn/sc/dynamic_function/waybill/#search/bill-number/" + ShipOrderNumber + "\"  target='_blank'>顺丰官网查询></a></li></ul>";
                }
                else {
                    hidHeadHtml += "<li style='text-align:right'><a href=\"https://www.kuaidi100.com/chaxun?nu=" + ShipOrderNumber + "\"  target='_blank'>快递100查询></a></li></ul>";
                }
            }
            $('#expressInfo').ReturnOrReplaceExpressData(ReplaceId, "replace", hidHeadHtml);
            ShowMessageDialog("查看换货用户物流", "viewlogistics", "ViewLogistics");
        }
}


//查看换货用户发货物流信息
function ViewReplaceUserLogistics(obj) {
    var ReplaceId = $(obj).attr("replaceid");
    if (ReplaceId)
        if (ReplaceId) {
            var hidHeadHtml = "";
            var ExpressCompanyName = $(obj).attr("expresscompanyname");
            var ShipOrderNumber = $(obj).attr("shipordernumber")
            if (ExpressCompanyName != "同城物流配送" && ExpressCompanyName != "") {
                hidHeadHtml = "<ul id='hidheard' class='buyer_information'><li><span>物流公司：</span>" + ExpressCompanyName + "</li>"
                           + "<li><span>物流单号：</span>" + ShipOrderNumber + "</li>"
                if (ExpressCompanyName.indexOf("顺丰") > -1) {
                    hidHeadHtml += "<li style='text-align:right'><a href=\"http://www.sf-express.com/cn/sc/dynamic_function/waybill/#search/bill-number/" + ShipOrderNumber + "\"  target='_blank'>顺丰官网查询></a></li></ul>";
                }
                else {
                    hidHeadHtml += "<li style='text-align:right'><a href=\"https://www.kuaidi100.com/chaxun?nu=" + ShipOrderNumber + "\"  target='_blank'>快递100查询></a></li></ul>";
                }
            }
            $('#expressInfo').ReplaceUserExpressData(ReplaceId, hidHeadHtml);
            ShowMessageDialog("查看换货用户物流", "viewlogistics", "ViewLogistics");
        }
}


//查看换货商户发货货物流信息
function ViewReplaceMallLogistics(obj) {
    var ReplaceId = $(obj).attr("replaceid");
    if (ReplaceId) {
        var hidHeadHtml = "";
        var ExpressCompanyName = $(obj).attr("expresscompanyname");
        var ShipOrderNumber = $(obj).attr("shipordernumber")
        if (ExpressCompanyName != "同城物流配送" && ExpressCompanyName != "") {
            hidHeadHtml = "<ul id='hidheard' class='buyer_information'><li><span>物流公司：</span>" + ExpressCompanyName + "</li>"
                       + "<li><span>物流单号：</span>" + ShipOrderNumber + "</li>"
            if (ExpressCompanyName.indexOf("顺丰") > -1) {
                hidHeadHtml += "<li style='text-align:right'><a href=\"http://www.sf-express.com/cn/sc/dynamic_function/waybill/#search/bill-number/" + ShipOrderNumber + "\"  target='_blank'>顺丰官网查询></a></li></ul>";
            }
            else {
                hidHeadHtml += "<li style='text-align:right'><a href=\"https://www.kuaidi100.com/chaxun?nu=" + ShipOrderNumber + "\"  target='_blank'>快递100查询></a></li></ul>";
            }
        }
        $('#expressInfo').ReplaceMallExpressData(ReplaceId, hidHeadHtml);
        ShowMessageDialog("查看换货用户物流", "viewlogistics", "ViewLogistics");
    }
}