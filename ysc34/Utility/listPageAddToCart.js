$.format = function (source, params) {
    if (arguments.length == 1)
        return function () {
            var args = $.makeArray(arguments);
            args.unshift(source);
            return $.format.apply(this, args);
        };
    if (arguments.length > 2 && params.constructor != Array) {
        params = $.makeArray(arguments).slice(1);
    }
    if (params.constructor != Array) {
        params = [params];
    }
    $.each(params, function (i, n) {
        source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
    });
    return source;
};
var toCartStoreId = 0;
var skuOrderBusiness = 0;
$(document).ready(function (e) {
    var pophtml = "<div class=\"att-popup att-popup-cart\">";
    pophtml += "   <div class=\"att-popup-container\">";
    pophtml += "      <div class=\"att-popup-header\">";
    pophtml += "         <div class=\"thumb pull-left\"><img id=\"imgSKUSubmitOrderProduct\" /></div>";
    pophtml += "         <div class=\"info pull-left\">";
    pophtml += "            <div class=\"p_price_1\" ><i>￥<span id=\"lblSKUSubmitOrderPrice\"></span></i></div>";
    pophtml += "            <div class=\"stock-control\">库存：<span id=\"lblSKUSubmitOrderStockNow\"></span></div>";
    pophtml += "            <div class=\"selected\" id=\"divSkuShows\">已选择：</div>";
    pophtml += "         </div>";
    pophtml += "         <a href=\"#\" class=\"att-popup-close pop_close icon-icon_close_48\"></a>";
    pophtml += "      </div>";
    pophtml += "      <div class=\"att-popup-body\">";
    pophtml += "         <div class=\"item\">";
    pophtml += "            <a name=\"specification\" id=\"specification\"></a>";
    pophtml += "            <div id=\"skulist\"></div>";
    pophtml += "            <div class=\"goods-num\">";
    pophtml += "               <a name=\"buyQuantity\" id=\"buyQuantity\"></a>";
    pophtml += "               <div class=\"text-muted\">数量：</div>";
    pophtml += "               <div id=\"spSub\" class=\"shopcart-add\">-</div>";
    pophtml += "               <input type=\"text\" id=\"buyNum\" size=\"10\" value=\"1\" />";
    pophtml += "               <div id=\"spAdd\" class=\"shopcart-minus\">+</div>";
    pophtml += "               <div class=\"item\" id=\"divMaxCount\">(每人限购&nbsp;<span id=\"spMaxCount\">件)</span></div>";
    pophtml += "            </div>";
    pophtml += "         </div>";
    pophtml += "      </div>";
    pophtml += "      <div class=\"att-popup-footer operbtns\"  id=\"divOperNormal\">";
    pophtml += "         <button class=\"btn btn-warning btn-yes\" id=\"buyButton\" type=\"shoppingBtn\">确 定</button>";
    pophtml += "      </div>";
    pophtml += "      <div class=\"att-popup-footer operbtns\" id=\"divOperEx\" style=\"display:none;\"></div>";
    pophtml += "   </div>";
    pophtml += "</div>";
    pophtml += "<input type=\"hidden\" id=\"hidden_SelectedSkuId\" />";
    pophtml += "<input type=\"hidden\" id=\"hidden_SKUFightGroupActivityId\" />";
    pophtml += "<input type=\"hidden\" id=\"hidden_SKUProductId\" />";
    pophtml += "<input type=\"hidden\" id=\"hidden_SKUCountDownId\" />";
    pophtml += "<input type=\"hidden\" id=\"hidden_SKUGroupBuyId\" />";
    pophtml += "<input type=\"hidden\" id=\"hidden_SKUPreSaleId\" />";
    pophtml += "<input type=\"hidden\" id=\"txCartQuantity\" />";

    $(document.body).append(pophtml);

    $('.att-popup-cart').on('click', function (event) {
        if ($(event.target).is('.att-popup-close')) {
            event.preventDefault();
            $(this).removeClass('is-visible');
            $("#divMaxCount").hide();
        }

    });
    toCartStoreId = parseInt(getParam("StoreId"));
    if (isNaN(toCartStoreId) || toCartStoreId <= 0) {
        toCartStoreId = 0;
    }

    $("#spAdd").bind("click", function () {
        var num = parseInt($("#buyNum").val());
        if (isNaN(num) || num < 0) {
            num = 0;
        }
        $("#buyNum").val(num + 1)
    });
    $("#spSub").bind("click", function () {
        var num = parseInt($("#buyNum").val()) - 1;
        if (num > 0) $("#buyNum").val(parseInt($("#buyNum").val()) - 1);
    });

    $("#buyButton").bind("click", function () {
        BuyProductToCart();
    }); //购买商品

});

//加入购物车
function BuyProductToCart() {
    if (!ValidateBuyAmount()) {
        return false;
    }
    if (!IsallSelected()) {
        ShowMsg("请选择规格", false);
        return false;
    }
    if (IsEmptySku()) {
        $("#hidden_SelectedSkuId").val($("#hidden_SKUProductId").val() + "_0");
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
    var count_sku = GetSpCount($("#hidden_SelectedSkuId").val());
    if (quantity + count_sku > stock) {
        ShowMsg("商品库存不足，您购物车中已存在该规格的商品数量为" + count_sku + "!", false);
        return false;
    }
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: parseInt($("#buyNum").val()), productSkuId: $("#hidden_SelectedSkuId").val(), StoreId: toCartStoreId },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                UpdateSpCount($("#hidden_SelectedSkuId").val(), resultData.SkuQuantity);
                ShowMsg("商品已经添加至购物车", true);			
                $(".att-popup").removeClass('is-visible');              
                updateshortcart();
                //myConfirmBox('添加成功', '商品已经添加至购物车', '现在去购物车', '再逛逛', function () { location.replace('ShoppingCart.aspx'); }, function () { location.replace("default.aspx"); });
                //显示添加购物成功
            }
            else {
                // 商品已经下架
                ShowMsg("此商品已经不存在(可能被删除或被下架)", false);
                $(".att-popup").removeClass('is-visible');
                return false;
            }
        }
    });
}
//加入购物车按钮点击事件
$(".btnAddToCart").click(function (e) {
    //如果是APP端加入购物车，则判断是否已经登录，如果没有登录则跳转去登录页面
    if (document.location.href.toLowerCase().indexOf("/appshop/") > -1) {
        AppIsLogin();
    } else {
        AddToCart(this);
    }


});


function AppIsLogin() {
    var ms = plus.webview.currentWebview();
    var url = ms.url;
    // 登录
    var userid = getCookie("Shop-Member");
    if (userid == undefined || userid == "") {// 如果web端没登录
        var siteinfostr = plus.storage.getItem('wapsiteinfo') || "{}";
        var siteInfo = JSON.parse(siteinfostr);
        var sessionid = null;
        if (siteInfo.UserInfo) {
            sessionid = siteInfo.UserInfo.sessionid;
        }

        if (sessionid != undefined && sessionid != null && sessionid != "") {
            document.location.href = "/AppShop/AppLogin.aspx?sessionid=" + sessionid + "&returnUrl=" + encodeURIComponent(document.location.href);
        }
        else
            OpenUrl('login');
        return false;
    }
    return true;
}

function serviceProductHref(obj) {
    var storeId = parseInt($(obj).attr("storeId"));
    if (isNaN(storeId) || storeId <= 0) {
        storeId = 0;
    }
    
    var productId = $(obj).attr("ProductId");
    location.href = "ServiceProductDetails.aspx?StoreId=" + storeId + "&ProductId=" + productId;
}

function AddToCart(obj) {
	
    var productId = parseInt($(obj).attr("ProductId"));
    if (isNaN(productId) || productId <= 0) {
        return false;
    }
    toCartStoreId = parseInt($(obj).attr("StoreId"));
    if (isNaN(toCartStoreId) || toCartStoreId <= 0) {
        toCartStoreId = parseInt(getParam("StoreId"));
        if (isNaN(toCartStoreId) || toCartStoreId <= 0) {
            toCartStoreId = 0;
        }
    }
    $("#hidden_SKUProductId").val(productId);
    $(obj).addClass("btnAddToCart1");
    $(".operbtns").hide();
    $(".operbtns").eq(0).show();
    ShowSkuSelectorPanel(productId, "addcart",obj);
}
///显示规格选择面板
function ShowSkuSelectorPanel(productId, action,obj) {
    var proId = parseInt(productId);
    if (isNaN(proId) || proId <= 0) {
        return false;
    }
    if (action == undefined || action == "") {
        action = "addcart";
    }
    $.ajax({
        url: "/AppShop/AppShopHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "GetProductSkus", ProductId: productId, StoreId: toCartStoreId },
        async: false,
        success: function (resultData) {
            if (resultData.Result != undefined && resultData.Result != null) {
                //ActivityUrl = activityUrl,
                //    FightGroupActivityId = fightGroupActivityId,
                if (resultData.Result.ActivityUrl != "" || resultData.Result.FightGroupActivityId != 0) {
                    if (resultData.Result.ActivityUrl != "") {
                        location.href = resultData.Result.ActivityUrl;
                    }
                    else {

                        if (document.location.href.toLowerCase().indexOf("/appshop/") > -1) {
                            showProductDetail(productId);
                        }
                        else {
                            location.href = "FightGroupActivityDetails?fightGroupActivityId=" + resultData.Result.FightGroupActivityId;
                        }
                    }
                }
                else {
                    $('.att-popup-cart').addClass('is-visible');
                    ////如果是门店商品并且门店状态不
                    //if (resultData.Result.StoreStatus!=5&&toCartStoreId>0)
                    InitSkuSelectorPanel(resultData.Result)
                    event.preventDefault();

                    
                 	$(obj).removeClass("btnAddToCart1");
                }
            }
            else {
                
                $(".att-popup").removeClass('is-visible');
                $(obj).removeClass("btnAddToCart1");
                ShowMsg(resultData.ErrorResponse.ErrorMsg, false);
                return false;
            }
        }
    });
}
///初始化规格选择面板
function InitSkuSelectorPanel(data) {
    var attributeNames = new Array();
    var panelhtml = "<div class=\"spec_pro\">";
    if (data.SkuItems.length > 0) {
        for (var i = 0; i < data.SkuItems.length; i++) {
            var skuItem = data.SkuItems[i];
            if ($.inArray(skuItem.AttributeName) == -1) {
                attributeNames.push(skuItem.AttributeName);
                // 规格名
                panelhtml += $.format("<div class=\"text-muted\">{0}：</div><input type=\"hidden\" name=\"skuCountname\" AttributeName=\"{0}\" id=\"skuContent_{1}\" />", skuItem.AttributeName, skuItem.AttributeId);

                panelhtml += $.format("<div class=\"list clearfix\" id=\"skuRow_{0}\">", skuItem.AttributeId);
            }
            var attributeValues = new Array();
            for (var j = 0; j < data.SkuItems[i].AttributeValue.length; j++) {
                var skuValueItem = data.SkuItems[i].AttributeValue[j];

                // 规格值
                var skuValueId = "skuValueId_" + skuItem.AttributeId + "_" + skuValueItem.ValueId;
                attributeValues.push(skuValueItem.Value);

                // 显示图片
                if (skuItem.UseAttributeImage) {
                    panelhtml += $.format("<div class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" ImgUrl=\"{4}\">{3}</div>",
                        skuValueId, skuItem.AttributeId, skuValueItem.ValueId, skuValueItem.Value, skuValueItem.ImageUrl);
                }
                else {
                    panelhtml += $.format("<div class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\">{3}</div>",
                        skuValueId, skuItem.AttributeId, skuValueItem.ValueId, skuValueItem.Value);
                }
            }
            panelhtml += "</div>";
        }

    }
    panelhtml += "</div>";
    $("#imgSKUSubmitOrderProduct").attr("src", data.ImageUrl);
    $("#lblSKUSubmitOrderStockNow").html(data.Stock);
    $("#lblSKUSubmitOrderPrice").html(data.DefaultSku.SalePrice);
    $("#buyNum").val("1");
    $("#divSkuShows").text("");
    $("#skulist").html(panelhtml);
    $("#hidden_SelectedSkuId").val();
    $.each($(".SKUValueClass"), function () {
        $(this).bind("click", function () { SelectSkus(this); });
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

    skuOrderBusiness = 0;

    var pvid = skuSelectId.split("_");
    var data = {};
    data.action = "ProductSkus";
    data.productId = $("#hidden_SKUProductId").val();
    data.AttributeId = pvid[1];
    data.ValueId = pvid[2];
    data.StoreId = toCartStoreId;//门店ID

    switch (skuOrderBusiness) {
        case 1:
            data.sourceId = $("#hidden_SKUFightGroupActivityId").val();
            break;
        case 0:
            data.sourceId = $("#hidden_SKUProductId").val();
            break;
        case 2:
            data.sourceId = $("#hidden_SKUCountDownId").val();
            break;
        case 3:
            data.sourceId = $("#hidden_SKUGroupBuyId").val();
            break;
        case 4:
            data.sourceId = $("#hidden_SKUPreSaleId").val();
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
                skuValues.push($("#hidden_SKUProductId").val());
                $.each($("input[type='hidden'][name='skuCountname']"), function () {
                    var value = $(this).attr("value").split(':')[1];
                    skuValues.push(value);
                    if (value != "" && value != undefined)
                        skuShows.push($("div[valueid=" + value + "]").text());
                });
                $("#divSkuShows").text("已选择：" + skuShows.join(","));

                // 如果全选，则重置SKU
                var allSelected = IsallSelected();
                if (allSelected) {
                    var selectedSku;
                    var skuId = skuValues.join("_");
                    for (var i = 0; i < resultData.SkuItems.length; i++) {
                        var item = resultData.SkuItems[i];

                        if (skuValues.length == item.SkuId.split("_").length) {
                            var tempItem = "_" + item.SkuId + "_";
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

// 重置SKU
function ResetCurrentSku(skuId, salePrice, stock, skuShows) {
    $("#hidden_SelectedSkuId").val(skuId);
    if (stock == "" || stock == undefined) stock = 0;
    if (stock <= 0) {
        ShowMsg("库存不足", false);
    }
    salePrice = parseFloat(salePrice);
    if (salePrice == undefined) {
        salePrice = 0;
    }
    $("[id$=lblSKUSubmitOrderStockNow]").html(stock)
    $("[id$=lblSKUSubmitOrderPrice]").html(salePrice.toFixed(2));
    if (skuOrderBusiness == 4) {
        prePrice = parseFloat($("#hidden_SKUSubmitOrderDepositPercent").val());
        if (prePrice > 0)
            $("[id$=lblSKUSubmitOrderPrePrice]").html((salePrice * prePrice / 100).toFixed(2));
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
