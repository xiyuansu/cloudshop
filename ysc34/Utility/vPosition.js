function seachNearStore(fromLatLng, changeStore, ignoreConfig, routeTo, isDefaultPage) {
    if (fromLatLng == undefined || fromLatLng == "") {
        window.location.href = "NoPositionTip";
    }
    var storeSource = getParam("storeSource");
    var storeId = getParam("storeId");
    var referralUserId = getParam("ReferralUserId");
    $.post('../API/DepotHandler.ashx?action=SerachNearStore&fromLatLng=' + fromLatLng + "&changeStore=" + changeStore + "&storeSource=" + storeSource + "&storeId=" + storeId, function (result) {
        if (ignoreConfig) {//忽略配置(目前只有多门店需要处理这块)

            if (result.Status == "tipChange") {
                $('#noTip3').show();
                $("#iAdrCity").html(decodeURI(result.Addr));
                $('#aQuit').click(function () {
                    $('#noTip3').fadeOut();
                    seachNearStore(result.fromLatLng, 0, true, "", false);
                });
                $('#aTosee').click(function () {
                    $('#noTip3').fadeOut();
                    seachNearStore(result.fromLatLng, 1, true, "", false);
                });
            } else {
                $("#hidIsReloadPosition").val("0");
                if (typeof (ShowListData) == "undefined")
                    window.location.href = "StoreList?from";
                else {
                    ShowListData();
                    $("#spanAddr").html(decodeURI(result.Addr));
                }
            }
            return;
        }
        if (result.Status == "noLatLng") {
            if (routeTo == "Platform") {
                if (!isDefaultPage)
                    window.location.href = "Default?fromSource=1";
            }
            else
                window.location.href = "NoPositionTip";
        }
        else if (result.Status == "nothing") {
            if (routeTo == "Platform") {
                if (!isDefaultPage)
                    window.location.href = "Default?fromSource=1";
            }
            else
                window.location.href = "NoStoreTip?tipType=2";
        }
        else if (result.Status == "goToStore") {
            if (referralUserId != "" && referralUserId != null && referralUserId != undefined)
                window.location.href = "StoreHome?storeSource=1&StoreId=" + result.StoreId + "&ReferralUserId=" + referralUserId;
            else
                window.location.href = "StoreHome?storeSource=1&StoreId=" + result.StoreId;
        }
        else if (result.Status == "storeList") {
            $("#hidIsReloadPosition").val("0");
            if (typeof (ShowListData) == "undefined")
                window.location.href = "StoreList?from";
            else {
                $("#spanAddr").html(decodeURI(result.Addr));
                ShowListData();
            }
            //}
        }
        else if (result.Status == "error") {
            if (routeTo == "Platform") {
                if (!isDefaultPage)
                    window.location.href = "Default?fromSource=1";
            }
            else
                window.location.href = "ResourceNotFound";
        }
        else if (result.Status == "tipChange") {
            $('#noTip3').show();
            $("#iAdrCity").html(decodeURI(result.Addr));
            $('#aQuit').click(function () {
                $('#noTip3').fadeOut();
                seachNearStore(result.fromLatLng, 0, false, "", false);
            });
            $('#aTosee').click(function () {
                $('#noTip3').fadeOut();
                seachNearStore(result.fromLatLng, 1, false, "", false);
            });
        }
        else if (result.Status == "platform") {
            if (!isDefaultPage) {
                if (referralUserId != "" && referralUserId != null && referralUserId != undefined)
                    window.location.href = "Default?fromSource=1&ReferralUserId=" + referralUserId;
                else
                    window.location.href = "Default?fromSource=1";
            }
        }
        else if (result.Status == "noStoreToPlatform") {
            if (!isDefaultPage) {
                if (referralUserId != "" && referralUserId != null && referralUserId != undefined)
                    window.location.href = "Default?fromSource=2&ReferralUserId=" + referralUserId;
                else
                    window.location.href = "Default?fromSource=2";
            }
            else {
                $('#noTip2').show().delay(1000).fadeOut();
            }
        }
    });
}

function changePosition(fromLatLng, address) {
    if (fromLatLng == undefined || fromLatLng == "") {
        window.location.href = "NoPositionTip";
    }
    var data = {};
    data.fromLatLng = fromLatLng;
    data.address = address;
    $.post('../API/DepotHandler.ashx?action=ChangePosition', data, function (result) {
        if (result.Status == "noLatLng") {
            window.location.href = "NoPositionTip";
        }
        else if (result.Status == "nothing") {
            window.location.href = "NoStoreTip?tipType=2";
        }
        else if (result.Status == "goToStore") {
            window.location.href = "StoreHome?storeSource=1&StoreId=" + result.StoreId;
        }
        else if (result.Status == "storeList") {
            window.location.href = "StoreList?from";
        }
        else if (result.Status == "error") {
            window.location.href = "ResourceNotFound";
        }
        else if (result.Status == "platform") {
            window.location.href = "Default?fromSource=1";
        }
        else if (result.Status == "noStoreToPlatform") {
            window.location.href = "NoStoreTip?tipType=1";
            //window.location.href = "Default?fromSource=2";
        }
    });
}

function ShowError(error) {

    window.location.href = "NoPositionTip";
}