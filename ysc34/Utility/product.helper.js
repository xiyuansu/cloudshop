var isadd = false;
var pageUrl = document.URL.toLowerCase();
var isCountDown = false;
var isGroupBuy = false;
var isPreSale = false;
var groupBuyId = parseInt(getParam("groupBuyId"));
if (!isNaN(groupBuyId) && groupBuyId > 0) {
    isGroupBuy = true;
}

var countdownId = parseInt(getParam("countDownId"));
if (!isNaN(countdownId) && countdownId > 0) {
    isCountDown = true;
}

var preSaleId = parseInt(getParam("PreSaleId"));
if (!isNaN(preSaleId) && preSaleId > 0) {
    isPreSale = true;
}
var bg = "<div class='modal_qt'></div>";
var bodyw = $("body").width();
$(document).ready(function () {
    $.each($(".SKUValueClass"), function () {
        $(this).bind("click", function () { SelectSkus(this); });
    });
    BindFavotesTag(); //绑定收藏标签    
    $("#tagDiv i").live("click", function () { removeTag(this); }); //绑定删除操作

    $("#sptyperemark").bind("click", function () { ShowProductTypeRemark(); });//显示商品类型说明
    $("#buyAmount").bind("blur", function () { ChangeBuyAmount(); });
    $("#buyButton").bind("click", function () { AddCurrentProductToCart(); });//立即购买
    $("#addcartButton").bind("click", function () { AddProductToCart(); });  //加入购物车
    $("#imgCloseLogin").bind("click", function () { $("#btnAmoBuy").show(); $("#loginForBuy").hide(); $(".modal_qt").remove(); });
    $("#btnLoginAndBuy").bind("click", function () { LoginAndBuy(); });
    $("#btnAmoBuy").bind("click", function () { AnonymousBuy(); });
    $("#textfieldusername").keydown(function (e) {
        if (e.keyCode == 13) {
            LoginAndBuy();
        }
    });

    $("#textfieldpassword").keydown(function (e) {
        if (e.keyCode == 13) {
            LoginAndBuy();
        }
    });
    if ($("#hiddenProductType").val() == null || $("#hiddenProductType").val() == "undefined") {
        $("#sptyperemark").css("display", "none");
    }
});

function GetShippingFreight(quantity) {
    var data = {};
    var productId = $("#hidden_productId").val();
    data.ProductId = productId;
    data.Quantity = quantity;
    data.SkuId = $("#productDetails_sku_v").val();
    data.regionId = 0;
    $.ajax({
        url: '/API/VshopProcess.ashx?action=GetProductFreight',
        type: 'POST',
        dataType: 'json',
        data: data,
        timeout: 600000,
        error: function () {
            ShowMsg("操作错误,请与系统管理员联系!", false);


        },
        success: function (json) {
            if (json.Status == "OK" && !isNaN(parseFloat(json.Freight)) && parseFloat(json.Freight) > 0) {
                $("#labProductFreight").html("运费：<label>" + json.Freight + "</label>元");
            }
            else {
                $("#labProductFreight").html("免运费");
            }
        }

    });

}

function SelectSkus(clt) {
    // 保存当前选择的规格
    var AttributeId = $(clt).attr("AttributeId");
    var ValueId = $(clt).attr("ValueId");
    $("#skuContent_" + AttributeId).val(AttributeId + ":" + ValueId);
    $("#skuContent_" + AttributeId).attr("ValueStr", $(clt).attr("value"));
    // 重置样式
    ResetSkuRowClass("skuRow_" + AttributeId, "skuValueId_" + AttributeId + "_" + ValueId);
    // 如果全选，则重置SKU
    var allSelected = IsallSelected();
    var skuValues = [];
    var skuShows = "";
    if (allSelected) {
        $.each($("input[type='hidden'][name='skuCountname']"), function () {
            skuValues.push($(this).attr("value").split(':')[1]);
            skuShows += $(this).attr("ValueStr") + ",";
        });
        //        selectedOptions = selectedOptions.substring(0, selectedOptions.length - 1);
        skuShows = skuShows.substring(0, skuShows.length - 1);

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
        if (selectedSku) {
            if (isGroupBuy || isCountDown) {
                ResetCurrentSku(selectedSku.SkuId, selectedSku.SKU, selectedSku.Weight, selectedSku.Stock, selectedSku.AlertStock, selectedSku.SalePrice, "已选择：" + skuShows);
            }
            else {
                ResetCurrentSku(selectedSku.SkuId, selectedSku.SKU, selectedSku.Weight, selectedSku.Stock, selectedSku.AlertStock, selectedSku.SalePrice, "已选择：" + skuShows);
            }
        }
        else
            ResetCurrentSku("", "", "", "", "0"); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
        //disableShoppingBtn(false);
        //        $.ajax({
        //            url: "ShoppingHandler.aspx",
        //            type: 'post', dataType: 'json', timeout: 10000,
        //            data: { action: "GetSkuByOptions", productId: $("#hiddenProductId").val(), options: selectedOptions },
        //            success: function(resultData) {
        //                if (resultData.Status == "OK") {
        //                    ResetCurrentSku(resultData.SkuId, resultData.SKU, resultData.Weight, resultData.Stock, resultData.AlertStock, resultData.SalePrice, "已选择：" + skuShows);
        //                }
        //                else {
        //                    ResetCurrentSku("", "", "", "", "", "0", "请选择："); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
        //                }
        //            }
        //        });
    }
}



// 是否所有规格都已选
function IsallSelected() {
    var allSelected = true;
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        if ($(this).attr("value").length == 0) {
            allSelected = false;
        }
    });
    return allSelected;
}

// 重置规格值的样式
function ResetSkuRowClass(skuRowId, skuSelectId) {
    var href = this.location.href.toLocaleLowerCase();
    var sourceId = 0;
    if (href.indexOf("countdown") >= 0) {
        sourceId = $("#hidden_CountDownId").val();
    }

    //判断某规格是否已下架
    var pvid = skuSelectId.split("_");
    $.ajax({
        url: "/Handler/ShoppingHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "UnUpsellingSku", productId: $("#hidden_productId").val(), AttributeId: pvid[1], ValueId: pvid[2], sourceId: sourceId },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                $.each($("#productSkuSelector dd input,#productSkuSelector dd img"), function (index, item) {
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
                        $(item).attr("disabled", false);
                    }
                    else {
                        $(item).attr("class", "SKUUNSelectValueClass");
                        $(item).attr("disabled", true);
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

    $.each($("#" + skuRowId + " dd input,#" + skuRowId + " dd img"), function () {
        $(this).attr({ "class": "SKUValueClass" });
    });

    $("#" + skuSelectId).attr({ "class": "SKUSelectValueClass" });
}

// 重置SKU
function ResetCurrentSku(skuId, sku, weight, stock, alertstock, salePrice, options) {
    $("#productDetails_sku").html(sku);
    $("#productDetails_sku_v").val(skuId);
    if (stock == "" || stock == undefined) stock = 0;
    //if (IsOpenStores && !isGroupBuy && !isCountDown) {
    //    $("#productDetails_Stock").html(stock > 0 ? "有货" : "无货");
    $("#productDetails_Stock").attr("stock", stock);
    //}
    //else {
    $("#productDetails_Stock").html(stock);
    //}
    if (alertstock == undefined) alertstock = 0;
    if (alertstock != "")
        $("#productDetails_AlertStock").val(alertstock);
    if (weight != "")
        weight = weight + " g";
    $("#ProductDetails_litWeight").html(weight);
    $("#SubmitOrder_Weight").html(weight);
    //重新选择规格后，重置运费
    var tempQuantity = parseInt($("#buyAmount").val());
    if (isNaN(tempQuantity) || tempQuantity <= 0) { tempQuantity = 1;}
    GetShippingFreight(tempQuantity)
    $("#ProductDetails_lblBuyPrice").html(salePrice.toFixed(2));
    if ($("#referSpan").length > 0) {
        GetReferDedut($("#hidden_productId").val(), skuId);

    }
    if (ValidateBuyAmount() && !isCountDown && !isGroupBuy) {
        var quantity = parseInt($("#buyAmount").val());
        var totalPrice = eval(salePrice) * quantity;
        if (!isNaN(totalPrice)) {
            $("#productDetails_Total").html(totalPrice.toFixed(2));
            $("#productDetails_Total_v").val(totalPrice);
        }
        else {
            $("#productDetails_Total").html("0");
            $("#productDetails_Total_v").val("0");
        }
    }
    if (isPreSale) {
        var depositPercent = $("#hidDepositPercent").val();
        if (depositPercent > 0) {
            var deposit = totalPrice * depositPercent / 100;
            $("#ProductDetails_lblDeposit").html("￥" + deposit.toFixed(2));
            $("#ProductDetails_lblFinalPayment").html("￥" + (totalPrice - deposit).toFixed(2));
        }
        else {
            var deposit = $("#hidDeposit").val();
            $("#ProductDetails_lblFinalPayment").html("尾款:￥" + (totalPrice - deposit).toFixed(2));
        }
    }
    //重新显示奖励
    var deduct = $("#hidSubMemberDeduct").val();
    if (deduct != "" && deduct != undefined && deduct != null) {
        var deductF = parseFloat(deduct);
        $("#ProductDetails_lblReferralMoney").html((salePrice.toFixed(2) * deductF / 100));
    }

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
            // alert('操作错误,请与系统管理员联系!');
        },
        success: function (json) {
            if (json.success == "true") {
                $("#referSpan").html("￥" + json.ReferralDeduct);
            }
            else {
                $("#referSpan").html("￥0");
            }
            alet(json.msg);
        }

    });

}
//添加收藏
//选中标签
function BindFavotesTag() {
    $.ajax({
        url: '/Handler/MemberHandler.ashx?action=BindFavorite',
        type: 'POST',
        dataType: 'json',
        timeout: 5000,
        error: function () {
            //alert('操作错误,请与系统管理员联系!');
        },
        success: function (json) {
            if (json.success == "true") {
                $("#tag_num").empty();
                $(json.msg).each(function (index, item) {
                    $("#tag_num").append("<li><a href=\"#none\" onclick=\"selected(this)\">" + item.TagName + "</a><i></i></li>");
                });
            }
        }

    });

}

function selected(object_a) {
    if ($(object_a).hasClass("current")) {
        $(object_a).removeClass("current");
    } else {
        $(object_a).addClass("current").siblings("a").removeClass("current");
    }
}

function AppendTags() {
    var tagval = $("#txttags").val().replace("自定义", "").replace(/\s/g, ""); //获取新增的标签值
    if (!checkLength($("#txttags")))
        return false;
    $(".att-tag-new").before(" <li><a class=\"current\" href=\"#none\" onclick=\"selected(this)\">" + tagval + "</a><i></i></li>");
    var licount = $("#tag_num2 li").size(); //获取总共添加的标签数量
    if (licount == 4) {
        $(".att-tag-new").hide();
    }
    $("#txttags").val('自定义');
}

function checkLength(txtobj) {
    var regu = "^[0-9a-zA-Z\u4e00-\u9fa5]{1,10}$";
    var re = new RegExp(regu);
    var tagval = $("#txttags").val().replace("自定义", "").replace(/\s/g, "");
    if (tagval.length <= 0) {
        $("#tishi span:eq(3)").show();
        return false;
    }
    if (!re.test(tagval)) {
        $("#tishi span:eq(4)").show();
        return false;
    }
    return true;
}


function removeTag(tag) {
    var removetext = $(tag).prev().text();
    var ul_id = $(tag).parents("ul").attr("id");
    if (ul_id == "tag_num2") {
        if ($(".att-tag-new").is(":hidden")) {
            $(".att-tag-new").show();
        } else {
            $(tag).parents("li").remove();
        }
    } else {
        var data = {};
        data.tagname = removetext;
        $.ajax({
            url: '/Handler/MemberHandler.ashx?action=DelteFavoriteTags',
            type: 'POST',
            data: data,
            dataType: 'json',
            timeout: 5000,
            error: function () {
                //alert('操作错误,请与系统管理员联系!');
            },
            success: function (json) {
                if (json.success == "true") {
                    $("#tag_num").empty();
                    $(tag).parents("li").remove();
                    $(json.msg).each(function (index, item) {
                        $("#tag_num").append("<li><a href=\"#none\" onclick=\"selected(this)\">" + item.TagName + "</a><i></i></li>");
                    });
                } else {
                    if (json.msg == "1") {
                        $("#tishi span:eq(5)").show();
                    } else if (json.msg == "2") {
                        $("#tishi span:eq(6)").show();
                    }
                }
            }
        });
    }

}

function checkTagNum(txtobj) {
    $("#tishi span").hide();
    var isValid = true;
    var num = $(".att-tag-list li a[class='current']").length;
    if (num <= 0) {
        $("#tishi span:eq(1)").show();
        ShowMsg("没有添加任何标签", false);
        isValid = false;
    }
    if (num >= 4) {
        $("#tishi span:eq(0)").show();
        ShowMsg("最多只能添加3个标签", false);
        isValid = false;
    }
    return isValid;
}

function SaveTags() {
    $('.modal_qt').hide();
    if (!checkTagNum($("#txttags"))) {//判断收藏记录数
        return false;
    }
    var tags = "";
    var data = {};
    data.favoriteid = favoriteid;
    $(".att-tag-list li a[class='current']").each(function () { tags += $(this).text() + "," });
    data.tags = tags.substr(0, tags.length - 1);

    $.ajax({
        url: '/Handler/MemberHandler.ashx?action=UpdateFavorite',
        type: 'POST',
        data: data,
        dataType: 'json',
        timeout: 5000,
        error: function () {
            //alert('操作错误,请与系统管理员联系!');
        },
        success: function (json) {
            if (json.success == "true") {
                $("#tishi span:eq(2)").show();
            }
        }
    });
    parent.$("#divFavorite").hide();
    return true;
}

function CloseFavorite() {
    $("#divFavorite").hide();
    $(".modal_qt").hide();
    $("#tishi span").hide();
}
function AddToFavorite() {
    if ($("#hiddenIsLogin").val() == "nologin") {
        $("#loginForBuy").show();
        $("body").append(bg);

        return false;
    }
    var xtarget = $("#addFavorite").offset().left;
    var ytarget = $("#addFavorite").offset().top;
    if ($("#addcartButton").length > 0) {
        xtarget = $("#addcartButton").offset().left;
        ytarget = $("#addcartButton").offset().top;
    }

    $("#divFavorite").css("top", 200 + "px");

    $("#divFavorite").css("left", bodyw / 2 - 240 + "px");
    if ($(document).scrollTop() <= 145) {
        $("#divFavorite").css("top", parseInt(ytarget - 125) + "px");
    }
    $(".Favorite_title_r,.btn-cancel").bind("click", function () {
        $("#divFavorite").css('display', 'none');
        $(".modal_qt").remove();
    });
    $(".btn-ok").bind("click", function () { UpdateTags() });

    var data = {};
    data.ProductId = $("#hidden_productId").val();
    if ($("#hidden_productId").length == 0) {
        data.ProductId = $("#hidden_productId").val();
    }
    $.post("/Handler/MemberHandler.ashx?action=AddFavorite", data, function (result) {
        if (result.success == "true") {
            favoriteid = result.favoriteid;
            $("#divFavorite").show();
            $("body").append(bg);
            $("#favoriteCount").text(result.Count);
        }
        else {
            $("#divAlready").css("top", 200 + "px");
            $("#divAlready").css("left", bodyw / 2 - 180 + "px");
            if ($(document).scrollTop() <= 145) {
                $("#divAlready").css("top", parseInt(ytarget - 125) + "px");
            }
            $(".Favorite_title_r").bind("click", function () { $("#divAlready").css('display', 'none'); $(".modal_qt").hide(); });
            $("#divAlready").show();
            $("body").append(bg);
        }
    });
}
// 购买数量变化以后的处理
function ChangeBuyAmount() {
    if (ValidateBuyAmount()) {
        var quantity = parseInt($("#buyAmount").val());
        var oldQuantiy = parseInt($("#oldBuyNumHidden").val());
        var productTotal = eval($("#productDetails_Total").html());
        var totalPrice = productTotal / oldQuantiy * quantity;

        $("#productDetails_Total").html(totalPrice.toFixed(2));
        $("#oldBuyNumHidden").attr("value", quantity);
        GetShippingFreight(quantity);
    }
}

// 购买按钮单击事件
function AddCurrentProductToCart() {
    isadd = false;
    if (!ValidateBuyAmount()) {
        return false;
    }

    if (!IsallSelected()) {
        ShowMsg("请选择规格", false);
        return false;
    }

    var quantity = parseInt($("#buyAmount").val());
    var maxcount = parseInt($("#maxcount").html());
    //var stock = parseInt
    var stock = parseInt($("#productDetails_Stock").html());
    if (isNaN(stock))
        stock = parseInt($("#productDetails_Stock").attr("stock"));
    if (quantity > stock) {
        ShowMsg("商品库存不足 " + quantity + " 件，请修改购买数量!", false);
        return false;
    }
    if (maxcount != "" && maxcount != null) {
        if (quantity > maxcount) {
            ShowMsg("此为限购商品，每人限购" + maxcount + "件", false);
            return false;
        }
    }
    if ($('#txtMaxCount').length > 0) {
        maxcount = parseInt($('#txtMaxCount').val());
        var soldCount = parseInt($('#txtSoldCount').val());
        if (quantity > maxcount - soldCount) {
            if (maxcount - soldCount <= 0) {
                if (isCountDown || isGroupBuy || isPreSale) {
                    ShowMsg("商品已售数量已超过活动限制数量,不能购买。", false);
                }
                else {
                    ShowMsg("请输入正确的购买数量。", false);
                }
            }
            else {
                ShowMsg("购买不能超过" + (maxcount - soldCount) + "件", false);
            }
            return false;
        }
    }
    if ($("#hiddenIsLogin").val() == "nologin") {
        $("#loginForBuy").show();
        $("body").append(bg);
        return false;
    }

    if ($("#surplusNumber").length > 0) {
        var surplusNumber = parseInt($("#surplusNumber").text());
        if (quantity > surplusNumber) {
            ShowMsg("剩余数量不足", false);
            return false;
        }

        var bolFlag = false;
        $.ajax({
            url: "/API/VshopProcess.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "IsDuplicateBuyCountDown", countDownId: $("#hidden_CountDownId").val(), skuId: eval($("#hidden_skus").val())[0].SkuId }, async: false,
            success: function (resultData) {
                if (resultData.success == "false") {
                    alert(resultData.msg);
                } else {
                    bolFlag = true;
                }
            }
        });
        if (!bolFlag)
            return bolFlag;

    }

    BuyProduct();
}

// 登录后再购买
function LoginAndBuy() {

    var username = $("#textfieldusername").val();
    var password = $("#textfieldpassword").val();
    var thisURL = document.URL;

    if (username.length == 0 || password.length == 0) {
        ShowMsg("请输入您的用户名和密码!", false);
        return;
    }
    $.post("/User/Login", { username: username, password: password, action: "Common_UserLogin" },
          function (data) {
              if (data.Status == "Succes") {
                  if (isadd) {
                      $("#btnAmoBuy").show();
                      $("#loginForBuy").hide('hide');
                      $("#hiddenIsLogin").val('logined');
                      BuyProductToCart();//调用添加到购物车
                  } else {
                      BuyProduct();
                      window.location.reload();
                  }

              }
              else {
                  ShowMsg(data.Msg, false);
              }
          }, "json");

}

// 购买商品
function BuyProduct() {
    var thisURL = document.URL.toLowerCase();
    if ($("#productDetails_sku_v").val().replace(/\s/g, "") == "") {
        ShowMsg("此商品已经不存在(可能库存不足或被删除或被下架)，暂时不能购买", false);
        return false;
    }
    if (isGroupBuy) {
        location.href = "/SubmmitOrder?buyAmount=" + $("#buyAmount").val() + "&productSku=" + $("#productDetails_sku_v").val() + "&from=groupBuy";
    }
    else if (isCountDown) {
        //查看是否结束
        location.href = "/SubmmitOrder?buyAmount=" + $("#buyAmount").val() + "&productSku=" + $("#productDetails_sku_v").val() + "&from=countDown";
    }
    else if (isPreSale) {
        if ($("#buyAmount").val().replace(/\s/g, "") == "" || parseInt($("#buyAmount").val().replace(/\s/g, "")) <= 0) {
            ShowMsg("商品库存不足 " + parseInt($("#buyAmount").val()) + " 件，请修改购买数量!", false);
            return false;
        }
        location.href = "/SubmmitOrder?buyAmount=" + $("#buyAmount").val() + "&productSku=" + $("#productDetails_sku_v").val() + "&from=presale&PreSaleId=" + preSaleId;
    }
    else {

        if ($("#buyAmount").val().replace(/\s/g, "") == "" || parseInt($("#buyAmount").val().replace(/\s/g, "")) <= 0) {
            ShowMsg("商品库存不足 " + parseInt($("#buyAmount").val()) + " 件，请修改购买数量!", false);
            return false;
        }
        location.href = "/SubmmitOrder?buyAmount=" + $("#buyAmount").val() + "&productSku=" + $("#productDetails_sku_v").val() + "&from=signBuy";
    }
}

// 验证数量输入
function ValidateBuyAmount() {
    var buyAmount = $("#buyAmount");
    var pRegExp = new RegExp("[0-9]*");
    //var amount = pRegExp.exec($(buyAmount).val());
    var amount = parseInt($(buyAmount).val());
    if (isNaN(amount) || amount <= 0) {
        $(buyAmount).val("");
        ShowMsg("请正确输入购买数量", false);
        return false;
    }
    $(buyAmount).val(amount);
    var ibuyNum = parseInt($("#buyAmount").val());
    if ($(buyAmount).val().length == 0 || isNaN(ibuyNum) || ibuyNum <= 0) {
        ShowMsg("请先填写购买数量,购买数量必须大于0!!", false);
        $(buyAmount).val("");
        return false;
    }
    if ($(buyAmount).val() == "0" || $(buyAmount).val().length > 5 || ibuyNum <= 0 || ibuyNum > 99999) {
        ShowMsg("填写的购买数量必须大于0小于99999!", false);
        var str = $(buyAmount).val();
        $(buyAmount).val(str.substring(0, 5));
        return false;
    }
    var amountReg = /^[0-9]*[1-9][0-9]*$/;
    if (!amountReg.test($(buyAmount).val())) {
        ShowMsg("请填写正确的购买数量!", false);
        return false;
    }

    return true;
}
//*************匿名购买**********************************//
function AnonymousBuy() {
    if (isadd) {
        BuyProductToCart();
    }
    else {
        BuyProduct();
    }
    $("#loginForBuy").hide();
}

//*************2011-07-25  添加到购物车按钮单击事件****************//
function AddProductToCart() {
    if (!ValidateBuyAmount()) {
        return false;
    }

    if (!IsallSelected()) {
        ShowMsg("请选择规格", false);
        return false;
    }

    var quantity = parseInt($("#buyAmount").val());
    var stock = parseInt(document.getElementById("productDetails_Stock").innerHTML);
    if (isNaN(stock))
        stock = parseInt($("#productDetails_Stock").attr("stock"));
    if (quantity > stock) {
        ShowMsg("商品库存不足 " + quantity + " 件，请修改购买数量!", false);
        return false;
    }

    var count_sku = GetSpCount($("#productDetails_sku_v").val());
    // var AllStock = parseInt(document.getElementById("txtAllstock").value);
    // if (isNaN(AllStock) || AllStock <= 0) AllStock = stock;
    if (quantity + count_sku > stock) {
        ShowMsg("商品库存不足，您购物车中已存在该规格的商品数量为" + count_sku + "!", false);

        return false;
    }
    BuyProductToCart();//添加到购物车
}
//更新当前购物车指定规格已购买的数量
function UpdateSpCount(skuid, quantity) {
    quantity = parseInt(quantity);
    if (isNaN(quantity)) quantity = 0;
    spCountVal = $("#txCartQuantity").val();
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
                    spCountArr[i] = skuid + "|" + quantity;
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
    var xtarget = $("#addcartButton").offset().left;
    var ytarget = $("#addcartButton").offset().top;
    var w = $("body").width();
    $("#divshow,#divbefore").css("top", 450 + "px");
    $("#divshow,#divbefore").css("left", w / 2 - 180 + "px");
    if ($(document).scrollTop() <= 145) {
        $("#divshow,#divbefore").css("top", parseInt(ytarget - 125) + "px");
    }
    $(".dialog_title_r,.btn-continue").bind("click", function () { $("#divshow").css('display', 'none'); $(".modal_qt").remove(); });
    $(".btn-viewcart").attr("href", "/ShoppingCart");
    $.ajax({
        url: "/Handler/ShoppingHandler.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: parseInt($("#buyAmount").val()), productSkuId: $("#productDetails_sku_v").val() },
        async: false,
        beforeSend: function () {
            $("#divbefore").css('display', 'block');
        },
        complete: function () {
            // setTimeout("if($('#divshow').css('display')=='block'){$('#divshow').css('display','none')}",6000);
            //$("#divshow").blur(function(){alert('aaaa')});
        },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                $("#divbefore").css('display', 'none');
                $("#divshow").css('display', 'block');//显示添加购物成功
                $('body').append(bg);
                $("#spcounttype").text(resultData.Quantity);
                $("#spanCartNum").text(resultData.Quantity);
                UpdateSpCount($("#productDetails_sku_v").val(), resultData.SkuQuantity);
                $("#sptotal").text(parseFloat(resultData.TotalMoney));

                $("#spcartNum").text(resultData.Quantity);
                $("#ProductDetails_ctl03___cartMoney").text(resultData.TotalMoney);
            } else if (resultData.Status == "0") {
                // 商品已经下架
                $("#divbefore").css('display', 'none');
                ShowMsg("此商品已经不存在(可能被删除或被下架)，暂时不能购买", false);
            }
            else if (resultData.Status == "1") {
                // 商品库存不足
                $("#divbefore").css('display', 'none');
                ShowMsg("商品库存不足 " + parseInt($("#buyAmount").val()) + " 件，请修改购买数量!", false);
            }
            else if (resultData.Status == "2" || resultData.Status == "3") {
                // 规格不存在
                $("#divbefore").css('display', 'none');
                ShowMsg("商品规格获取失败，可能已被管理员删除！", false);
            }
            else {
                $("#divbefore").css('display', 'none');
                ShowMsg("系统错误", false);
            }
        }
    });
}



//点击弹出商品类型说明层
function ShowProductTypeRemark() {

    if ($("#hiddenProductType").val() != null && $("#hiddenProductType").val() != "undefined") {//是否存在商品类型
        var typeId = $("#hiddenProductType").val();
        var x = $("#sptyperemark").offset().left + 70;
        var y = $(document).scrollTop();
        var divobj = $("<div id=\"dv_typeremark\" class=\"blk\"></div>");
        var div_m = $("<div class=\"main\"></div>");
        var div_mclose = $("<div class=\"c_head\">查看适合我的尺寸<a class=\"closeBtn\" id=\"a_close\">关闭</a></div>");
        var div_mcontent = $("<div class=\"con\">暂无说明</div>");
        if (typeId != "" && typeId != "undefined") {
            div_mcontent.html(typeId);
        }
        if ($("#dv_typeremark").html() == null) {
            div_m.append(div_mclose);
            div_m.append(div_mcontent);
            divobj.append(div_m);
            $(divobj).appendTo("body");
            $("#dv_typeremark").css("margin-top", y - 50);
            $(divobj).show('slow');


        } else {
            if ($('#dv_typeremark').css("display") == "" && $('#dv_typeremark').css("display") != "undefined") {
                $("#dv_typeremark").show('slow');
            }
        }
        $("#a_close").bind("click", function () { $('#dv_typeremark').hide('hide'); $("#dv_typeremark").remove(); })
    }

}


