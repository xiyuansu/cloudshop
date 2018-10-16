$(document).ready(function () {
    $.each($(".SKUValueClass"), function () {
        $(this).bind("click", function () { SelectSkus(this); });
    });

    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        $(this).val("");
    });

    $("#buyButton").bind("click", function () { BuyProduct(); }); //立即购买
    $("#spAdd").bind("click", function () { $("#buyNum").val(parseInt($("#buyNum").val()) + 1) });
    $("#spSub").bind("click", function () { var num = parseInt($("#buyNum").val()) - 1; if (num > 0) $("#buyNum").val(parseInt($("#buyNum").val()) - 1) });
    $("#spcloces").bind("click", function () { $("#divshow").hide() });
});

function disableShoppingBtn(disabled) {//禁用(启用)购买和加入购物车按钮
    var btns = $('button[type=shoppingBtn]');
    if (disabled)
        btns.addClass('disabled');
    else
        btns.removeClass('disabled');
}

function SelectSkus(clt) {
    //禁用购买和加入购物车按钮
    //  disableShoppingBtn(true);

    // 保存当前选择的规格
    var AttributeId = $(clt).attr("AttributeId");
    var ValueId = $(clt).attr("ValueId");
    //disableSelected(AttributeId, ValueId);
    $("#skuContent_" + AttributeId).val(AttributeId + ":" + ValueId);
    // 重置样式
    ResetSkuRowClass("skuRow_" + AttributeId, "skuValueId_" + AttributeId + "_" + ValueId);
    // 如果全选，则重置SKU
    var allSelected = IsallSelected();
    var skuValues = [];
    if (allSelected) {
        $.each($("input[type='hidden'][name='skuCountname']"), function () {
            skuValues.push($(this).attr("value").split(':')[1]);
        });
        var skuItems = eval($('#hidden_skus').val());
        var selectedSku;
        if (skuItems != undefined && skuItems != null) {
            for (var j = 0; j < skuItems.length; j++) {
                var item = skuItems[j];
                var found = true;
                for (var i = 0; i < skuValues.length; i++) {
                    if (item.SkuId.indexOf('_' + skuValues[i]) == -1)
                        found = false;
                }
                if (found) {
                    selectedSku = item;
                    break;
                }
            }
        }
        if (selectedSku)
            ResetCurrentSku(selectedSku.SkuId, selectedSku.SKU, selectedSku.Weight, selectedSku.Stock, selectedSku.SalePrice);
        else
            ResetCurrentSku("", "", "", "", "0"); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
        // disableShoppingBtn(false);
    }
    var imgUrl = $(clt).attr("imgurl");
    if (imgUrl != undefined && imgUrl != null && imgUrl != "") {
        var targetImg = $(".slidesjs-control img[src='" + imgUrl + "']");
        if (targetImg != undefined) {
            $(".slidesjs-control a").css("display", "none").css("z-index", "0");
            var tagerta = targetImg.parent();
            var index = tagerta.attr("slidesjs-index");
            //$('#slides').goto(index);
            //tagerta.css("display", "block");
            //tagerta.css("z-index", "10");
            $(".slidesjs-pagination a[data-slidesjs-item='" + index + "']").click();
            //$(".slidesjs-pagination a").attr("class", "");
            //$(".slidesjs-pagination a[data-slidesjs-item='" + index + "']").attr("class", "active");
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
    var pvid = skuSelectId.split("_");
    $.ajax({
        url: "/Handler/ShoppingHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "UnUpsellingSku", productId: $("#hiddenProductId").val(), AttributeId: pvid[1], ValueId: pvid[2] },
        success: function (resultData) {

            if (resultData.Status == "OK") {
                $.each($(".specification div.SKUValueClass,.specification div.SKUUNSelectValueClass"), function (index, item) {
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
                //如果是限时购改变库存金额
                var SKURowClassCount = $(".SKURowClass").length;
                var SKUSelectValueClassCount = $(".SKUSelectValueClass").length;
                if (SKURowClassCount == SKUSelectValueClassCount) {
                    var skuId = $("#productDetails_sku_v").val();
                    for (var i = 0; i < resultData.SkuItems.length; i++) {
                        var item = resultData.SkuItems[i];
                        if (item.SkuId == skuId) {
                            $("#surplusNumber").text(item.Stock);
                            $("#productDetails_Total").text(item.SalePrice);
                            $("#productDetails_Total_v").val(item.SalePrice);
                            $("#CountDownProductsDetails_lblCurrentSalePrice").text(item.SalePrice);
                            $("#CountDownProductsDetails_lblSalePrice").text(item.OldSalePrice);                         
                            break;
                        }
                    }

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
function ResetCurrentSku(skuId, sku, weight, stock, salePrice) {
    $("#hiddenSkuId").val(skuId);
    salePrice = parseFloat(salePrice);
    if (salePrice == undefined) {
        salePrice = 0;
    }
    $("#spSalaPrice").html(salePrice.toFixed(2));
    if (stock == "" || stock == undefined) stock = 0;
    //if (IsOpenStores) {
    //    $("#productDetails_Stock").html(stock > 0 ? "有货" : "无货");
        $("#productDetails_Stock").attr("stock", stock);
    //}
    //else {
        $("#productDetails_Stock").html(stock);
    //}
    if (stock <= 0)
        alert_h("库存不足");
    if ($("#referSpan").length > 0) {
        GetReferDedut($("#hiddenProductId").val(), skuId);

    }
    //if (!isNaN(parseInt(stock))) {
    //    $("#spStock").html(stock);
    //}
    //else {
    //    $("#spStock").html("0");
    //    alert_h("库存不足");
    //}
}

//获取指定规格的分销分佣
function GetReferDedut(productId, skuId) {
    var data = {};
    data.ProductId = productId;
    data.SkuId = skuId;
    $.ajax({
        url: '/API/VshopProcess.ashx?action=GetSkuReferralDeduct',
        type: 'POST',
        dataType: 'json',
        data: data,
        timeout: 5000,
        error: function () {
            alert('操作错误,请与系统管理员联系!');
        },
        success: function (json) {
            if (json.success == "true") {
                $("#referSpan").html("￥" + json.ReferralDeduct);
            }
            else {
                $("#referSpan").html("￥0");
            }
        }

    });

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
    var count_sku = GetSpCount($("#hiddenSkuId").val());
    if (quantity + count_sku > stock) {
        alert_h("商品库存不足，您购物车中已存在该规格的商品数量为" + count_sku + "!");
        return false;
    }
    location.href = "SubmmitOrder?buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hiddenSkuId").val() + "&from=signBuy";
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
    var amountReg = /^[0-9]*[1-9][0-9]*$/;
    if (!amountReg.test($(buyNum).val())) {
        alert_h("请填写正确的购买数量!");
        return false;
    }

    //return true;
}

function AddProductToCart() {
    if (!ValidateBuyAmount()) {
        return false;
    }
    if (!IsallSelected()) {
        alert_h("请选择规格");
        document.location.href = "#specification";
    }
    else {
        var quantity = parseInt($("#buyNum").val());
        var stock = parseInt(document.getElementById("productDetails_Stock").innerHTML);
        if (isNaN(stock))
            stock = parseInt($("#productDetails_Stock").attr("stock"));
        if (quantity > stock) {
            $("#buyNum").focus();
            document.location.href = "#buyQuantity";
            alert_h("商品库存不足 " + quantity + " 件，请修改购买数量!");
            return false;
        }
        var count_sku = GetSpCount($("#hiddenSkuId").val());
        if (quantity + count_sku > stock) {
            alert_h("商品库存不足，您购物车中已存在该规格的商品数量为" + count_sku + "!");
            return false;
        }

        BuyProductToCart(); //添加到购物车
    }
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
