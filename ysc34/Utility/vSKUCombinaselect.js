$(document).ready(function () {
    $("#minusnum").bind("click", function () {
        var number = $("#buysetnum");
        var num = parseInt(number.val()) - 1;
        if (num > 0) {
            chageProductNum(num, number);
        }
    });

    $("#addnum").bind("click", function () {
        var number = $("#buysetnum");
        var num = parseInt(number.val()) + 1;
        if (num > 0) {
            chageProductNum(num, number);
        }
    });

    $.each($(".SKUValueClass"), function () {
        $(this).bind("click", function () { SelectSkus(this); });
    });

    $('.att-popup').on('click', function (event) {
        if ($(event.target).is('.att-popup-close') || $(event.target).is('.att-popup')) {
            event.preventDefault();
            $(this).removeClass('is-visible');
        }
    });

    $("#productbuy").bind("click", function () {
        submitOrder();
    });
    $("#buysetnum").val(1);
    clearSkuCountname();
    changeBuyProduct();
});

function keyPress() {
    var keyCode = event.keyCode;
    if ((keyCode >= 48 && keyCode <= 57)) {
        event.returnValue = true;
    } else {
        event.returnValue = false;
    }
}

function valuechange() {
    var number = $("#buysetnum");
    var num = parseInt(number.val());
    if (num > 0 && checkStock(num)) {
        $("span.cart_num").text("x" + num);
        //计算总价格和优惠金额
        changeBuyProduct();
    }
}

//清除
function clearSkuCountname() {
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        $(this).val("");
    });
}

//选择规格
function SelectSkus(clt) {
    // 保存当前选择的规格
    var AttributeId = $(clt).attr("AttributeId");
    var ValueId = $(clt).attr("ValueId");
    var ProductId = $(clt).attr("ProductId");
    $("#skuContent_" + AttributeId + "_" + ProductId).val(AttributeId + ":" + ValueId);
    if ($(clt).hasClass("active")) {
        return false;
    }
    // 重置样式
    ResetSkuRowClass("skuRow_" + AttributeId + "_" + ProductId, "skuValueId_" + AttributeId + "_" + ValueId + "_" + ProductId, ProductId);

    var imgUrl = $(clt).attr("imgurl");
    if (imgUrl != undefined && imgUrl != null && imgUrl != "") {
        targetImg = $("#imgSKUSubmitOrderProduct_" + ProductId);
        if (targetImg != undefined) {
            $(targetImg).attr("src", imgUrl);
        }
    }
}


// 重置规格值的样式
function ResetSkuRowClass(skuRowId, skuSelectId, productid) {
    var skuValues = [];
    var skuShows = [];
    skuValues.push(productid);
    $.each($("input[type='hidden'][name='skuCountname'][productid=" + productid + "]"), function () {
        var value = $(this).attr("value").split(':')[1];
        skuValues.push(value);
        skuShows.push($("div[valueid=" + value + "][productid =" + productid + "]").text());
    });
    $("#divSkuShows_" + productid).text("已选择：" + skuShows.join(","));
    var pvid = skuSelectId.split("_");
    var data = {};
    data.action = "GetCombinationSku";
    data.ProductId = productid;
    data.AttributeId = pvid[1];
    data.ValueId = pvid[2];
    data.CombinationId = $("#vCombinationBuyDetail_hidcombinaid").val();
    $.ajax({
        url: "../API/CombinationBuyHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: data,
        success: function (resultData) {

            //if (resultData.Status == "OK") {

            $.each($("div.SKUValueClass,div.SKUUNSelectValueClass"), function (index, item) {
                var currentPid = $(this).attr("AttributeId");
                var currentVid = $(this).attr("ValueId");
                var currentproductid = $(this).attr("productid");
                if (parseInt(currentproductid) == parseInt(productid)) {
                    // 不同属性选择绑定事件
                    var isBind = false;
                    $.each($(resultData), function () {
                        if (currentPid == this.AttributeId && currentVid == this.ValueId) {
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
                }
            });

            // 如果全选，则重置SKU
            var allSelected = IsallSelected(productid);
            if (allSelected) {
                var selectedSku;
                var skuId = skuValues.join("_");
                for (var i = 0; i < resultData.length; i++) {
                    var item = resultData[i];
                    if (item.SkuId == skuId) {
                        selectedSku = item;
                        break;
                    }
                }
                if (selectedSku)
                    ResetCurrentSku(selectedSku.SkuId, selectedSku.SalePrice, selectedSku.CombinationPrice, selectedSku.Stock, skuShows, productid);
            }
            //}
        }

    });
    $.each($("#" + skuRowId + " div"), function () {
        $(this).removeClass('active');
    });

    $("#" + skuSelectId).addClass('active');
}

// 重置SKU
function ResetCurrentSku(skuId, salePrice, combinaPrice, stock, skuShows, productid) {
    if (stock == "" || stock == undefined) stock = 0;
    if (stock <= 0) {
        ShowMsg("库存不足", false);
    }

    $("#hiddenSkuId_" + productid).val(skuId);
    salePrice = parseFloat(salePrice);
    if (salePrice == undefined) {
        salePrice = 0;
    }

    combinaPrice = parseFloat(combinaPrice);
    if (combinaPrice == undefined) {
        combinaPrice = 0;
    }

    //赋值价格和库存
    $("[id=stock_" + productid + "]").html(stock)
    $("[id=combianskuprice_" + productid + "]").html(combinaPrice.toFixed(2));
    $("[id=combianskuSaleprice_" + productid + "]").html(salePrice.toFixed(2));


}

//确认选择规格
function btnyesSku(clt) {
    var productid = $(clt).attr("productid");
    //判断是否全选
    var allSelected = IsallSelected(productid);
    if (!allSelected) {
        ShowMsg("请选择规格！", false);
        return;
    }
    var currstock = $("#stock_" + productid).html();
    if (parseInt(currstock) <= 0) {
        ShowMsg("当前规格库存不足请选择其他规格！", false);
        return;
    }
    //赋值选择skuid  
    var skuId = $("#hiddenSkuId_" + productid).val();
    if (skuId == productid + "_0") {
        $('#att-popup_' + productid).removeClass('is-visible');
        return;
    }
    //alert(skuId);
    var comprice = $("#combianskuprice_" + productid + "").html()//组合价
    var saleprice = $("#combianskuSaleprice_" + productid + "").html()//一口价
    var masterproductid = $("#vCombinationBuyDetail_hidproductid").val();
    var currselect = $("#divSkuShows_" + productid).html().replace("已选择：", "");
    if (masterproductid != undefined && masterproductid != null && parseInt(productid) == parseInt(masterproductid)) {
        //主商品
        $("#vCombinationBuyDetail_selectmainsku").html("<em>已选:</em><b>" + currselect + "</b><i></i>").addClass("select");
        $("#masterprice").html(comprice);
        $("#mastersaleprice").html(saleprice);
        $("#vCombinationBuyDetail_hidmasterselectsku").val(skuId);
        $("#vCombinationBuyDetail_hidmasterstock").val(currstock);
    }
    else {
        $("#divspecification_" + productid).html("<em>已选:</em><b>" + currselect + "</b><i></i>").addClass("select");
        $("#ck_" + productid).val(skuId);
        $("#skustock_" + productid).val(currstock);
        //alert($("#skustock_" + productid).val());
        $("#combinaprice_" + productid + "").html(comprice);
        $("#saleprice_" + productid + "").html(saleprice);
    }
    $('#att-popup_' + productid).removeClass('is-visible');

    changeBuyProduct();

}

// 是否所有规格都已选
function IsallSelected(productid) {
    var allSelected = true;
    $.each($("input[type='hidden'][name='skuCountname'][productid=" + productid + "]"), function () {
        if ($(this).val().length == 0) {
            allSelected = false;
        }
    });
    return allSelected;
}

function showsku(productid) {
    var skuid = "0";
    if (productid == 0) {
        productid = $("#vCombinationBuyDetail_hidproductid").val();
        if ($("#vCombinationBuyDetail_hidmasterproducthassku").val() == "1") {
            $('#att-popup_' + productid).addClass('is-visible');
            skuid = $("#vCombinationBuyDetail_hidmasterselectsku").val();
        }
    }
    else {
        $('#att-popup_' + productid).addClass('is-visible');
        skuid = $("#ck_" + productid).val();
    }

    //恢复当前选中规格
    if (skuid != "0" && skuid != productid + "_0") {
        var skuarry = skuid.split("_");
        $.each(skuarry, function (n, value) {
            if (n == 0) {
                // 0 是productid
            }
            else {
                var ctl = $("div[valueid=" + value + "][productid =" + productid + "]").first();
                SelectSkus(ctl);
            }
        });
    }


}
//获取选中商品最少库存
function selectProductMinStock() {
    var masterstock = $("#vCombinationBuyDetail_hidmasterstock").val();
    var minstock = parseInt(masterstock);;
    var cpids = getSelectProductids();
    if (cpids.length > 0) {
        var pidarray = cpids.split(",");
        $.each(pidarray, function (n, value) {
            var stock = parseFloat($("#skustock_" + value).val());
            if (parseInt(stock) < minstock) {
                minstock = parseInt(stock);
            }
        });
    }

    return minstock;

}
//校验库存 简单校验
function checkStock(buynum) {

    //然后判断组合商品
    var cpids = getSelectProductids();
    var b = true;
    if (cpids.length > 0) {
        var pidarray = cpids.split(",");
        $.each(pidarray, function (n, value) {
            var stock = parseFloat($("#skustock_" + value).val());
            if (parseInt(stock) < buynum) {
                ShowMsg("组合商品库存不足！", false);
                b = false;
                return false;
            }
        });
    }
    else {
        ShowMsg("至少得选择一件组合商品", false);
        b = false;
    }

    if (b) {
        //先判断主商品
        var mastercurrstock = $("#vCombinationBuyDetail_hidmasterstock").val();
        if (parseInt(mastercurrstock) < buynum) {
            ShowMsg("商品库存不足！", false);
            b = false;
        }
    }

    if (!b) {
        var minstock = selectProductMinStock();
        if (parseInt($("#buysetnum").val()) > minstock) {
            $("#buysetnum").val(minstock);
            $("span.cart_num").text("x" + minstock);
        }
    }
    return b;
}
//提交订单
function submitOrder() {

    //主规格检测    
    var masterskuid = $("#vCombinationBuyDetail_hidmasterselectsku").val();
    if (masterskuid == "0") {
        ShowMsg("主商品请选择规格！", false);
        return;
    }
    //组合商品
    var chk = $("input[name=ck_productId]:checked");
    var ck_skuId = "";
    var b = false;
    $(chk).each(function () {
        var skuid = $(this).val();
        if (skuid == "0") {
            ShowMsg("勾选的组合商品请选择规格！", false);
            b = true;
            return false;
        }
        ck_skuId += skuid + ",";
    });

    if (!b) {
        var num = $("#buysetnum").val();//购买数量
        var combinaid = $("#vCombinationBuyDetail_hidcombinaid").val();
        //库存检测
        if (!checkStock(num)) {
            return;
        }
        ck_skuId += masterskuid;
        location.href = "SubmmitOrder?buyAmount=" + num + "&productSku=" + ck_skuId + "&from=combinationbuy&combinaid=" + combinaid;
    }

}

//修改购买套数
function chageProductNum(num, number) {
    //验证库存
    if (!checkStock(num)) {
        return;
    }
    //赋值
    number.val(num);
    $("span.cart_num").text("x" + num);

    //计算总价格和优惠金额
    changeBuyProduct();
}

//修改了勾选的商品
function changeBuyProduct() {
    var cpids = getSelectProductids();//选中的商品
    getSelectProductAmount(cpids);//计算组合价和实际售价      
}

// 获取选中购买的产品
function getSelectProductids() {
    var chk = $("input[name=ck_productId]:checked");
    var ck_productId = "";
    $(chk).each(function () {
        ck_productId += $(this).attr("productid") + ",";
    });
    ck_productId = ck_productId.substring(0, ck_productId.length - 1);
    return ck_productId;
}

// 获取选中购买的产品数量
function getSelectProductAmount(cpids) {

    var combinaprice = parseFloat($("#masterprice").text());
    var saleprice = parseFloat($("#mastersaleprice").text());

    if (cpids.length > 0) {
        var pidarray = cpids.split(",");
        $.each(pidarray, function (n, value) {
            combinaprice += parseFloat($("#combinaprice_" + value).text());
            saleprice += parseFloat($("#saleprice_" + value).text());
        });
    }
    //计算折扣
    var discountprice = saleprice - combinaprice;
    if (discountprice < 0) {
        discountprice = 0;
    }
    var setnum = $("#buysetnum").val();

    //乘以购买套数
    combinaprice = combinaprice * parseFloat(setnum);
    discountprice = discountprice * parseFloat(setnum);

    $("#totalPrice").text(combinaprice.toFixed(2));
    $("#discount_price").text(discountprice.toFixed(2));

}