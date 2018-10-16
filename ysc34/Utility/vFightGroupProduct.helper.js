var whichBuy = 0;//0等于普通购买 1等于拼团购买
var IsOpenStores;//是否对门店
$(document).ready(function () {
    IsOpenStores = $("#hidden_IsOpenStores").val() == "1";
    $("#buyFightGroupActivity").click(function () {
        whichBuy = 1;
        getFightGroupActivity();
    });
    $("#buyProduct").click(function () {
        whichBuy = 0;
        getProduct();
    });

    $("#buyButton").bind("click", function () { BuyProduct(); }); //购买商品

    $.each($(".SKUValueClass"), function () {
        $(this).bind("click", function () { SelectSkus(this); });
    });

    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        $(this).val("");
    });


    $("#spAdd").bind("click", function () { $("#buyNum").val(parseInt($("#buyNum").val()) + 1) });
    $("#spSub").bind("click", function () { var num = parseInt($("#buyNum").val()) - 1; if (num > 0) $("#buyNum").val(parseInt($("#buyNum").val()) - 1) });
    $("#spcloces").bind("click", function () { $("#divshow").hide() });
});
function getProduct() {
    var productId = $("#hidden_productId").val();
    var data = {};
    data.ProductId = productId;
    $.ajax({
        url: '/API/VshopProcess.ashx?action=GetProduct',
        type: 'GET',
        dataType: 'json',
        data: data,
        timeout: 5000,
        error: function () {
            alert('操作错误,请与系统管理员联系!');
        },
        success: function (json) {
            var stock = json.Stock;
            var salePrice = json.MaxSalePrice;
            $("#vFightGroupActivityDetails_lblPrice").text(salePrice);
            $("#vFightGroupActivityDetails_lblStockNow").text(stock);
        }

    });
}

function getFightGroupActivity() {
    var skuId = $("#hidden_MinSalePriceSkuId").val();
    var fightGroupActivityId = $("#hidden_fightGroupActivityId").val();
    var data = {};
    data.FightGroupActivityId = fightGroupActivityId;
    data.SkuId = skuId;
    $.ajax({
        url: '/API/VshopProcess.ashx?action=GetFightGroupActivity',
        type: 'GET',
        dataType: 'json',
        data: data,
        timeout: 5000,
        error: function () {
            alert('操作错误,请与系统管理员联系!');
        },
        success: function (json) {
            var stock = json.Stock;
            var salePrice = json.SalePrice;
            $("#vFightGroupActivityDetails_lblPrice").text(salePrice);
            $("#vFightGroupActivityDetails_lblStockNow").text(stock);
        }

    });

}

function disableShoppingBtn(disabled) {//禁用(启用)购买和加入购物车按钮
    var btns = $('button[type=shoppingBtn]');
    if (disabled)
        btns.addClass('disabled');
    else
        btns.removeClass('disabled');
}

function SelectSkus(clt) {
    // 保存当前选择的规格
    var AttributeId = $(clt).attr("AttributeId");
    var ValueId = $(clt).attr("ValueId");

    $("#skuContent_" + AttributeId).val(AttributeId + ":" + ValueId);
    // 重置样式
    ResetSkuRowClass("skuRow_" + AttributeId, "skuValueId_" + AttributeId + "_" + ValueId);

    var imgUrl = $(clt).attr("imgurl");
    if (imgUrl != undefined && imgUrl != null && imgUrl != "") {
        var targetImg = $(".slidesjs-control img[src='" + imgUrl + "']");
        if (targetImg != undefined) {
            $(".slidesjs-control a").css("display", "none").css("z-index", "0");
            var tagerta = targetImg.parent();
            var index = tagerta.attr("slidesjs-index");
            $(".slidesjs-pagination a[data-slidesjs-item='" + index + "']").click();
        }
    }
}
///禁用未设置的规格
function disableSelected(attributeId, valueId) {
    var skuItems = eval($('#hidden_skus').val());
    var index = 0;
    var hasSku = false;
    var rowObj = $("input[type='hidden'][id='skuContent_" + attributeId + "']");
    var rowIndex = $("input[type='hidden'][name='skuCountname']").index($(rowObj));
    var rowValue = valueId;
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        var attributeId = $(this).attr("id").replace("skuContent_", "");
        if (rowIndex != index) {
            hasSku = false;
            $.each($("div[attributeid='" + attributeId + "']"), function () {
                var itemObj = $(this);
                for (var j = 0; j < skuItems.length; j++) {
                    if ($(itemObj).attr("valueid") == "291") {
                        alert((skuItems[j].SkuId.split('_')[index + 1] + "-" + skuItems[j].SkuId.split('_')[rowIndex + 1] + "-" + rowValue));
                    }
                    if (skuItems[j].SkuId.split('_')[index + 1] == $(itemObj).attr("valueid") && skuItems[j].SkuId.split('_')[rowIndex + 1] == rowValue) {
                        hasSku = true;
                        break;
                    }
                }
                if (!hasSku) {
                    $(this).html($(this).html() + "-");
                }
            });
        }
        index += 1;
    });
}
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
    var skuValues = [];
    var skuShows = [];
    skuValues.push($("#hidden_productId").val());
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        var value = $(this).attr("value").split(':')[1];
        skuValues.push(value);
        skuShows.push($("div[valueid=" + value + "]").text());
    });
    $("#divSkuShows").text("已选择：" + skuShows.join(","));
    var pvid = skuSelectId.split("_");
    var data = {};
    data.action = "UnUpsellingSku";
    data.productId = $("#hiddenProductId").val();
    data.AttributeId = pvid[1];
    data.ValueId = pvid[2];
    switch (whichBuy) {
        case 1:
            data.sourceId = $("#hidden_fightGroupActivityId").val();
            break;
    }

    $.ajax({
        url: "/Handler/ShoppingHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: data,
        success: function (resultData) {
            
            if (resultData.Status == "OK") {
                $.each($("div.SKUValueClass,div.SKUUNSelectValueClass"), function (index, item) {
                    var currentPid = $(this).attr("AttributeId");
                    var currentVid = $(this).attr("ValueId");
                    // 不同属性选择绑定事件
                    var isBind = false;
                    $.each($(resultData.SkuItems), function () {
                        if (currentPid == this.AttributeId && currentVid == this.ValueId && this.Stock > 0) {
                            isBind = true;
                        }
                    });
                    if (isBind) {
                        if ($(item).attr("class") == "SKUUNSelectValueClass") {
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

                // 如果全选，则重置SKU
                var allSelected = IsallSelected();
                if (allSelected) {
                    var selectedSku;
                    var skuId = skuValues.join("_");
                    for (var i = 0; i < resultData.SkuItems.length; i++) {
                        var item = resultData.SkuItems[i];
                        if (item.SkuId == skuId) {
                            selectedSku = item;
                            break;
                        }
                    }
                    if (selectedSku)
                        ResetCurrentSku(selectedSku.SkuId, selectedSku.SalePrice, selectedSku.Stock, skuShows);
                }
            }
        }

    });
    $.each($("#" + skuRowId + " div"), function () {
        $(this).removeClass('active');
    });

    $("#" + skuSelectId).addClass('active');
}

// 重置SKU
function ResetCurrentSku(skuId, salePrice, stock, skuShows) {
    if (stock == "" || stock == undefined) stock = 0;
    $("#hiddenSkuId").val(skuId);;
    if (stock <= 0)
        alert_h("库存不足");
    salePrice = parseFloat(salePrice);
    if (salePrice == undefined) {
        salePrice = 0;
    }
    $("#vFightGroupActivityDetails_lblStockNow").html(stock)
    $("#vFightGroupActivityDetails_lblPrice").html(salePrice.toFixed(2));
}




// 购买按钮单击事件
function BuyProduct() {

    if (!ValidateBuyAmount()) {
        return false;
    }
    if (!IsallSelected()) {
        document.location.href = "#specification";
        alert_h("请选择规格");
        return false;
    }
    var quantity = parseInt($("#buyNum").val());
    var stock = parseInt($("#productDetails_Stock").html());
    if (isNaN(stock))
        stock = parseInt($("#productDetails_Stock").attr("stock"));
    if (isNaN(stock) || stock == 0) {
        alert_h("库存不足");
        return false;
    }
    if (quantity > stock) {
        alert_h("商品库存不足 " + quantity + " 件，请修改购买数量!");
        return false;
    }
    switch (whichBuy) {
        case 1:
            location.href = "SubmmitOrder?buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hiddenSkuId").val() + "&from=fightGroup" + "&fightGroupActivityId=" + $("#hidden_fightGroupActivityId").val();
            break;
        default:
            location.href = "SubmmitOrder?buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hiddenSkuId").val() + "&from=signBuy";
            break;
    }

}

// 验证数量输入
function ValidateBuyAmount() {
    var buyNum = $("#buyNum");
    var ibuyNum = parseInt($("#buyNum").val());
    if ($(buyNum).val().length == 0 || isNaN(ibuyNum) || ibuyNum <= 0) {
        alert_h("请先填写购买数量,购买数量必须大于0!");
        return false;
    }
    if ($(buyNum).val() == "0" || $(buyNum).val().length > 5 || ibuyNum <= 0 || ibuyNum > 99999) {
        alert_h("填写的购买数量必须大于0小于99999!", function () {
            var str = $(buyNum).val();
            $(buyNum).val(str.substring(0, 5));
            return false;
        });


    }
    var amountReg = /^[1-9]d*|0$/;
    if (!amountReg.test($(buyNum).val())) {
        alert_h("请填写正确的购买数量!");
        return false;
    }

    return true;
}







function BuyProductToCart() {

    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: parseInt($("#buyNum").val()), productSkuId: $("#hiddenSkuId").val() },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                var xtarget = $("#addcartButton").offset().left;
                var ytarget = $("#addcartButton").offset().top;
                UpdateSpCount($("#hiddenSkuId").val(), resultData.SkuQuantity);
                $("#divshow").css("top", "200px");
                $("#divshow").css("left", parseInt(xtarget) + "px");
                myConfirmBox('添加成功', '商品已经添加至购物车', '现在去购物车', '再逛逛', function () { location.replace('ShoppingCart'); }, function () { location.replace("default"); });
                //显示添加购物成功
            }
            else {
                // 商品已经下架
                alert_h("此商品已经不存在(可能被删除或被下架)，暂时不能购买" + resultData.Status);
            }
        }
    });
}
