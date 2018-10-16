var skuOrderBusiness = 0;//  0普通商品 1拼团商品
var storeId = 0;//
$(document).ready(function () {
    skuOrderBusiness = parseInt($("#hidden_SKUSubmitOrderBusiness").val());
    if (skuOrderBusiness == 4) {
        $("#prediv").show();
    } else {
        $("#prediv").hide();
    }
    $("#buyButton").bind("click", function () {
        if (skuOrderBusiness == 4) {
            BuyProduct();
        } else {
            if ($(this).attr("signbuy") != "false") {//如果是立即购物
                BuyProduct();
            }
            else {
                BuyProductToCart();
            }
        }
    }); //购买商品

    $.each($(".SKUValueClass"), function () {
        $(this).bind("click", function () { SelectSkus(this); });
    });


    clearSkuCountname();

    $('.att-popup').on('click', function (event) {
        if ($(event.target).is('.att-popup-close,#addToCart') || $(event.target).is('.att-popup')) {
            event.preventDefault();
            $(this).removeClass('is-visible');
            $("#divMaxCount").hide();
        }

    });

    $("#spAdd").bind("click", function () {
        var num = parseInt($("#buyNum").val());
        if (isNaN(num) || num < 0) {
            num = 0;
        }
        $("#buyNum").val(num + 1)
        GetShippingFreight();
    });
    $("#spSub").bind("click", function () {
        var num = parseInt($("#buyNum").val()) - 1;
        if (num > 0) $("#buyNum").val(parseInt($("#buyNum").val()) - 1);
        GetShippingFreight();
    });
    checkToHideDivSkuShows();
    if ($("#hidden_StoreId").val() != "" && parseInt($("#hidden_StoreId").val()) >= 0)
        storeId = parseInt($("#hidStoreId").val());
});


function BuyProductToCart() {
    if (!ValidateBuyAmount()) {
        return false;
    }
    if (!IsallSelected()) {
        ShowMsg("请选择规格", false);
        return false;
    }
    if (IsEmptySku()) {
        $("#hidden_SKUSubmitOrderSelectedSkuId").val($("#hidden_SKUSubmitOrderProductId").val() + "_0");
    }
    var quantity = parseInt($("#buyNum").val());
    var stock = parseInt($("[id$=lblSKUSubmitOrderStockNow]").text());
    if (isNaN(stock) || stock == 0) {
        ShowMsg("库存不足", false);
        return false;
    }
    if (quantity > stock) {
        ShowMsg("商品库存不足 " + quantity + " 件，请修改购买数量!", false);
        return false;
    }
    var count_sku = GetSpCount($("#hidden_SKUSubmitOrderSelectedSkuId").val());
    if (quantity + count_sku > stock) {
        ShowMsg("商品库存不足，您购物车中已存在该规格的商品数量为" + count_sku + "!", false);
        return false;
    }
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: parseInt($("#buyNum").val()), productSkuId: $("#hidden_SKUSubmitOrderSelectedSkuId").val(), storeId: storeId },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                var xtarget = $("#addcartButton").offset().left;
                var ytarget = $("#addcartButton").offset().top;
                UpdateSpCount($("#hidden_SKUSubmitOrderSelectedSkuId").val(), resultData.SkuQuantity);
                $("#divshow").css("top", "200px");
                $("#divshow").css("left", parseInt(xtarget) + "px");

                ShowMsg("商品已经添加至购物车", true);
                $(".att-popup").removeClass('is-visible');
                //myConfirmBox('添加成功', '商品已经添加至购物车', '现在去购物车', '再逛逛', function () { location.replace('ShoppingCart.aspx'); }, function () { location.replace("default.aspx"); });
                //显示添加购物成功
            }
            else {
                // 商品已经下架
                ShowMsg("此商品已经不存在(可能被删除或被下架)", false);
                return false;
            }
        }
    });
}

function checkToHideDivSkuShows() {
    if ($("div.spec_pro").length == 0)
        $("#divSkuShows").html("");
}

function getDefaultCountDownPrice() {
    $(".SKUValueClass").removeClass("active");
    $("#hidden_SKUSubmitOrderSelectedSkuId").val("");
    $("input[type='hidden'][name='skuCountname']").val("");
    var stock = $("[id$=hidden_SKUSubmitOrderCountDownStock]").val();
    var salePrice = $("[id$=hidden_SKUSubmitOrderCountDownMinPrice]").val();
    $("[id$=lblSKUSubmitOrderStockNow]").html(stock)
    $("[id$=lblSKUSubmitOrderPrice]").html(salePrice);
    $("#divMaxCount").show();
}

function getDefaultFightGroupActivityPrice() {
    $(".SKUValueClass").removeClass("active");
    $("#hidden_SKUSubmitOrderSelectedSkuId").val("");
    $("input[type='hidden'][name='skuCountname']").val("");
    var stock = $("[id$=hidden_SKUSubmitOrderFightGroupActivityStock]").val();
    var salePrice = $("[id$=hidden_SKUSubmitOrderFightGroupActivityMinPrice]").val();
    $("[id$=lblSKUSubmitOrderStockNow]").html(stock)
    $("[id$=lblSKUSubmitOrderPrice]").html(salePrice);
    $("#divMaxCount").show();
}

function getDefaultGroupBuyPrice() {
    $(".SKUValueClass").removeClass("active");
    $("#hidden_SKUSubmitOrderSelectedSkuId").val("");
    $("input[type='hidden'][name='skuCountname']").val("");
    var groupbuyStock = parseInt($("[id$=hidden_SKUSubmitOrderGroupBuyStock]").val());
    if (isNaN(groupbuyStock)) {
        groupbuyStock = 0;
    }
    var productStock = parseInt($("#hidden_SKUSubmitOrderProductStock").val());
    if (isNaN(productStock)) {
        productStock;
    }
    var salePrice = $("[id$=hidden_SKUSubmitOrderGroupBuyMinPrice]").val();
    $("[id$=lblSKUSubmitOrderStockNow]").html(productStock < groupbuyStock ? productStock : groupbuyStock);
    $("[id$=lblSKUSubmitOrderPrice]").html(salePrice);
}

function getProductPrice() {
    $(".SKUValueClass").removeClass("active");
    $("#hidden_SKUSubmitOrderSelectedSkuId").val("");
    $("input[type='hidden'][name='skuCountname']").val("");
    var stock = $("[id$=hidden_SKUSubmitOrderProductStock]").val();
    var salePrice = $("[id$=hidden_SKUSubmitOrderProductMinPrice]").val();
    $("[id$=lblSKUSubmitOrderStockNow]").html(stock)
    $("[id$=lblSKUSubmitOrderPrice]").html(salePrice);
}

function clearSkuCountname() {
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        $(this).val("");
    });
}


function GetShippingFreight() {
    //重新计算运费
    var productId = 0;
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
    if ($("#hidden_SKUSubmitOrderSelectedSkuId").length > 0) {
        skuId = $("#hidden_SKUSubmitOrderSelectedSkuId").val();
    }
    var regionId = 0;
    var quantity = 1;
    var quantity = parseInt($("#buyNum").val());
    if (isNaN(quantity) || quantity < 1) {
        quantity = 1;
    }
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "GetProductFreight", regionId: regionId, productId: productId, SkuId: skuId, quantity: quantity },
        success: function (resultData) {
            if (resultData.Status == "OK" && !isNaN(parseFloat(resultData.Freight)) && parseFloat(resultData.Freight) > 0) {
                $("#labProductFreight").html(resultData.Freight);
            }
            else {
                $("#labProductFreight").html("免运费");
            }
        }
    });
}


function SelectSkus(clt) {
    //if ($(clt).hasClass("disabled")) {
    //    return;
    //}
    // 保存当前选择的规格
    var AttributeId = $(clt).attr("AttributeId");
    var ValueId = $(clt).attr("ValueId");

    $("#skuContent_" + AttributeId).val(AttributeId + ":" + ValueId);
    if ($(clt).hasClass("active")) {
        return false;
    }
    // 重置样式
    ResetSkuRowClass("skuRow_" + AttributeId, "skuValueId_" + AttributeId + "_" + ValueId);

    var imgUrl = $(clt).attr("imgurl");
    if (imgUrl != undefined && imgUrl != null && imgUrl != "") {
        //var targetImg = $(".slidesjs-control img[src='" + imgUrl + "']");
        //if (targetImg.length > 0 && targetImg != undefined) {
        //    $(".slidesjs-control a").css("display", "none").css("z-index", "0");
        //    var tagerta = targetImg.parent();
        //    var index = tagerta.attr("slidesjs-index");
        //    $(".slidesjs-pagination a[data-slidesjs-item='" + index + "']").click();
        //}
        targetImg = $("#imgSKUSubmitOrderProduct");
        if (targetImg.length > 0) {
            $(targetImg).attr("src", imgUrl);
        }
    }
}

function IsEmptySku() {
    return $("input[type='hidden'][name='skuCountname']").length == 0;
}
///禁用未设置的规格

// 是否所有规格都已选
function IsallSelected() {
    var allSelected = true;
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        if ($(this).val().length == 0) {
            allSelected = false;
        }
    });
    return allSelected;
}

// 重置规格值的样式
function ResetSkuRowClass(skuRowId, skuSelectId) {

    skuOrderBusiness = parseInt($("#hidden_SKUSubmitOrderBusiness").val());

    var pvid = skuSelectId.split("_");
    var data = {};
    data.action = "ProductSkus";
    data.productId = $("#hiddenProductId").val();
    data.AttributeId = pvid[1];
    data.ValueId = pvid[2];
    data.StoreId = 0;//门店ID
    if ($("#hidden_StoreId").val() != "" && parseInt($("#hidden_StoreId").val()) >= 0)
        data.StoreId = parseInt($("#hidden_StoreId").val());
    switch (skuOrderBusiness) {
        case 1:
            data.sourceId = $("#hidden_SKUSubmitOrderFightGroupActivityId").val();
            break;
        case 0:
            data.sourceId = $("#hidden_SKUSubmitOrderProductId").val();
            break;
        case 2:
            data.sourceId = $("#hidden_SKUSubmitOrderCountDownId").val();
            break;
        case 3:
            data.sourceId = $("#hidden_SKUSubmitOrderGroupBuyId").val();
            break;
        case 4:
            data.sourceId = $("#hidden_SKUSubmitOrderPreSaleId").val();
            break;
    }

    $.ajax({
        url: "/Handler/ShoppingHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: data,
        success: function (resultData) {
            //console.log(resultData);
            if (resultData.Status == "OK") {
                $.each($("div.SKUValueClass,div.SKUUNSelectValueClass"), function (index, item) {
                    var currentPid = $(this).attr("AttributeId");
                    var currentVid = $(this).attr("ValueId");
                    // 不同属性选择绑定事件
                    //var isBind = ValuesInSkuId(resultData.SkuItems, GetSelectValues(currentPid, currentVid), currentPid, data);
                    var isBind = false;
                    //selectValues = GetSelectValues();
                    $.each($(resultData.SkuItems), function (index, item) {
                        if (currentPid == item.AttributeId && currentVid == item.ValueId && item.Stock > 0) {
                            isBind = true;
                        }
                    });
                    if (isBind) {
                        if ($(item).hasClass("SKUUNSelectValueClass")) {
                            $(item).attr("class", "SKUValueClass");
                        }
                        $(item).unbind();
                        $(item).bind("click", function () { SelectSkus(this); });
                        $(item).removeClass("disabled");
                    }
                    else {
                        $(item).attr("class", "SKUUNSelectValueClass");
                        $(item).unbind();
                        $(item).addClass("disabled");
                    }
                });
                $.each($("div[id^='skuRow_']"), function (index, obj) {
                    var s = $(obj).attr("id");
                    var attrId = s.substring(7);
                    var noChoose = true;
                    $.each($("div[id^='skuValueId_" + attrId + "']"), function (index, item) {
                        if ($(item).hasClass("active")) {
                            noChoose = false;
                        }
                    });
                    if (noChoose) {
                        $("#skuContent_" + attrId).val("");
                    }
                });
                var skuValues = [];
                var skuShows = [];
                skuValues.push($("#hidden_SKUSubmitOrderProductId").val());
                $.each($("input[type='hidden'][name='skuCountname']"), function () {
                    var value = $(this).attr("value").split(':')[1];
                    skuValues.push(value);
                    if (value != "" && value != undefined)
                        skuShows.push($("div[valueid=" + value + "]").text());
                });
                $("#divSkuShows").text("已选择：" + skuShows.join(","));
                $("#divspecification").html("已选择：" + "<class=\"selected\">" + skuShows.join(",") + "</i><i class=\"viewdetial\"></i>");

                // 如果全选，则重置SKU
                var allSelected = IsallSelected();
                if (allSelected) {
                    var selectedSku;
                    var skuId = skuValues.join("_");
                    for (var i = 0; i < resultData.SkuItems.length; i++) {
                        var item = resultData.SkuItems[i];

                        if (skuValues.length == item.SkuId.split("_").length) {
                            var tempItem = item.SkuId + "_";
                            var isFound = true;
                            for (var j = 1; j < skuValues.length; j++) {
                                if (tempItem.indexOf("_" + skuValues[j] + "_") == -1) {
                                    isFound = false;
                                }
                            }
                            if (isFound) {
                                selectedSku = item;
                                break;
                            }
                        }
                    }
                    if (selectedSku) {
                        ResetCurrentSku(selectedSku.SkuId, selectedSku.SalePrice, selectedSku.Stock, skuShows);
                    }
                    else {
                        ResetCurrentSku("", 0, 0, skuShows);
                    }
                }
                else {

                }
            }
        }

    });
    $.each($("#" + skuRowId + " div"), function () {
        $(this).removeClass('active');
    });

    $("#" + skuSelectId).addClass('active');
}
///skuItems 规格列表    selectValues  已选中的值    attributeId  当前遍历的行属性ID，   data  当前选中的数据
function ValuesInSkuId(skuItems, selectValues, attributeId, data) {

    var selectedSku;
    var isFound = true;
    var selectAttributes = GetSelectAttributes();
    if ($.inArray(attributeId, selectAttributes) > -1) {
        return true;
    }
    for (var i = 0; i < skuItems.length; i++) {
        var item = skuItems[i];

        var tempItem = item.SkuId + "_";
        isFound = true;
        for (var j = 0; j < selectValues.length; j++) {//如果选中的规格组合不存在规格列表中
            if (tempItem.indexOf("_" + selectValues[j] + "_") == -1) {
                console.log(tempItem + "    " + selectValues + "     _" + selectValues[j] + "_");
                isFound = false;
            }
        }
    }
    console.log(isFound)
    return isFound;
}
function GetSelectAttributes() {
    var isInValues = false;
    var selectAttributes = [];
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        var value = $(this).attr("value").split(':');
        var attributeId = value[0];
        if (value.length >= 2) {
            selectAttributes.push(value[0]);
        }
    });
    return selectAttributes;
}

///获取选中的规格ID
function GetSelectValues(attrId, valueId) {
    var isInValues = false;
    var selectValues = [];
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        var value = $(this).attr("value").split(':');
        var attributeId = value[0];
        console.log($(this).attr("value"));
        if (value.length >= 2) {
            console.log($.inArray(value[1], selectValues) + "---" + value[1]);
            if ($("div[id^='skuValueId_" + attributeId + "_" + valueId + "']").length > 0) {
                isInValues = true;
                //if ($.inArray(valueId, selectValues) == -1) {
                selectValues.push(valueId);
                // }
            }
            else {
                // if ($.inArray(value[1], selectValues) == -1) {
                selectValues.push(value[1]);
                //  }
            }
        }
    });
    if ($.inArray(valueId, selectValues) == -1)
        selectValues.push(valueId);

    return selectValues;
}

// 重置SKU
function ResetCurrentSku(skuId, salePrice, stock, skuShows) {
    if (skuId != "") {
        $("#hidden_SKUSubmitOrderSelectedSkuId").val(skuId);
        GetShippingFreight();
    }
    else {
        $("#hidden_SKUSubmitOrderSelectedSkuId").val("");
    }
    if (stock == "" || stock == undefined) stock = 0;
    salePrice = parseFloat(salePrice);
    if (stock <= 0 || isNaN(salePrice))
        ShowMsg("库存不足", false);
    if (isNaN(salePrice) || salePrice == undefined) {
        salePrice = 0;
    }
    $("[id$=lblSKUSubmitOrderStockNow]").html(stock)
    $("[id$=lblSKUSubmitOrderPrice]").html(salePrice.toFixed(2));
    if (skuOrderBusiness == 4) {
        prePrice = parseFloat($("#hidden_SKUSubmitOrderDepositPercent").val());
        if (prePrice > 0) {
            var showSalePrice = (salePrice * prePrice / 100).toFixed(2);
            if (isNaN(showSalePrice)) {
                showSalePrice = 0;
            }
            $("[id$=lblSKUSubmitOrderPrePrice]").html(showSalePrice);
        }
    }
}




// 购买按钮单击事件
function BuyProduct() {

    console.log("begin");
    skuOrderBusiness = parseInt($("#hidden_SKUSubmitOrderBusiness").val());
    if (!ValidateBuyAmount()) {
        return false;
    }
    if (!IsallSelected()) {
        ShowMsg("请选择规格", false);
        return false;
    }
    if (IsEmptySku()) {
        $("#hidden_SKUSubmitOrderSelectedSkuId").val($("#hidden_SKUSubmitOrderProductId").val() + "_0");
    }
    var quantity = parseInt($("#buyNum").val());
    var stock = parseInt($("[id$=lblSKUSubmitOrderStockNow]").text());
    if (isNaN(stock) || stock == 0) {
        ShowMsg("库存不足", false);
        return false;
    }
    if (quantity > stock) {
        ShowMsg("商品库存不足 " + quantity + " 件，请修改购买数量!", false);
        return false;
    }

    var spMaxCount = parseInt($("#spMaxCount").text());
    if ($("#divMaxCount:visible").length == 1 && quantity > spMaxCount) {
        ShowMsg("超过每单限购数量，请正确填写数量!", false);
        return false;
    }
    if (skuOrderBusiness == 3) {
        var groupMaxCount = parseInt($("#groupBuyMaxCount").val());
        if (quantity > groupMaxCount) {
            ShowMsg("购买数量不能超过" + groupMaxCount + "件!", false);
            return false;
        }
    }


    //服务类商品订单提交跳转
    var service_submit = $("#hidService_submit").val();
    if (service_submit != undefined && service_submit == 1) {
        storeId = parseInt($("#hidStoreId").val());
        location.href = "ServiceProductSubmitOrder?buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hidden_SKUSubmitOrderSelectedSkuId").val() + "&from=signBuy&storeId=" + storeId;
    }
    else {
        switch (skuOrderBusiness) {
            case 0:
                location.href = "SubmmitOrder?buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hidden_SKUSubmitOrderSelectedSkuId").val() + "&from=signBuy&storeId=" + storeId;
                break;
            case 1:
                location.href = "SubmmitOrder?buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hidden_SKUSubmitOrderSelectedSkuId").val() + "&from=fightGroup" + "&fightGroupActivityId=" + $("#hidden_SKUSubmitOrderFightGroupActivityId").val() + "&fightGroupId=" + $("#hidden_SKUSubmitOrderFightGroupId").val();
                break;
            case 2:
                location.href = "SubmmitOrder?buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hidden_SKUSubmitOrderSelectedSkuId").val() + "&from=countDown&storeId=" + storeId;
                break;
            case 3:
                location.href = "SubmmitOrder?buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hidden_SKUSubmitOrderSelectedSkuId").val() + "&from=groupBuy"
                break;
            case 4:
                location.href = "SubmmitOrder?buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hidden_SKUSubmitOrderSelectedSkuId").val() + "&from=preSale&preSaleId=" + $("#hidden_SKUSubmitOrderPreSaleId").val();
                break;
        }
    }

}

// 验证数量输入
function ValidateBuyAmount() {
    var buyNum = $("#buyNum");
    var ibuyNum = parseInt($("#buyNum").val());
    if ($(buyNum).val().length == 0 || isNaN(ibuyNum) || ibuyNum <= 0) {
        ShowMsg("请先填写购买数量,购买数量必须大于0!", false);
        return false;
    }
    if ($(buyNum).val() == "0" || $(buyNum).val().length > 5 || ibuyNum <= 0 || ibuyNum > 99999) {
        ShowMsg("填写的购买数量必须大于0小于99999!", false);
        var str = $(buyNum).val();
        $(buyNum).val(str.substring(0, 5));
        return false;
    }
    var amountReg = /^[0-9]*[1-9][0-9]*$/;
    if (!amountReg.test($(buyNum).val())) {
        ShowMsg("请填写正确的购买数量!", false);
        return false;
    }
    $(buyNum).val(ibuyNum);

    return true;
}


//更新当前购物车指定规格已购买的数量
function UpdateSpCount(skuid, quantity) {
    quantity = parseInt(quantity);
    if (isNaN(quantity)) quantity = 0;
    spCountVal = $("#txCartQuantity").val();
    //alert(spCountVal + "---" + skuid + "---" + quantity);
    var newspCountVal = "";
    if (spCountVal == "") { newspCountVal = skuid + "|" + quantity; }
    else
    {
        var findSkuId = false;
        var spCountArr = spCountVal.split(",");
        for (var i = 0; i < spCountArr.length; i++) {
            if (spCountArr == "") continue;
            var itemArr = spCountArr[i].split('|');
            if (itemArr.length >= 2) {

                if (itemArr[0] == skuid) {
                    var temp_quantity = parseInt(itemArr[1]);
                    if (isNaN(temp_quantity)) temp_quantity = 0;
                    spCountArr[i] = skuid + "|" + (quantity);
                    findSkuId = true;
                }
            }
            newspCountVal += (newspCountVal == "" ? "" : ",") + (spCountArr[i]);
        }
        if (!findSkuId) newspCountVal += (newspCountVal == "" ? "" : ",") + (skuid + "|" + quantity);
    }
    $("#txCartQuantity").val(newspCountVal);
}
///获取当前购物车指定规格已购买的数量
function GetSpCount(skuid) {
    spCountVal = $("#txCartQuantity").val();
    //alert(spCountVal + "-" + skuid);
    if (spCountVal == "") { return 0; }
    else
    {
        var spCountArr = spCountVal.split(",");
        for (var i = 0; i < spCountArr.length; i++) {
            if (spCountArr == "") continue;
            var itemArr = spCountArr[i].split('|');
            if (itemArr.length >= 2) {
                if (itemArr[0] == skuid) {
                    var temp_quantity = parseInt(itemArr[1]);
                    return temp_quantity;
                }
            }
        }
    }
    return 0;
}




