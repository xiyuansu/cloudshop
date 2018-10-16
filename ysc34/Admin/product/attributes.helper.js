var productTypeSelector;
var brandCategorysSelector;
var skuFields, cellFields, bindCellFields;
var currentTypeId;
var skuCounts = 0;
var skuTableHolder, skuFieldHolder;
var skuTable, tableBody, tableHeader;
var htSkuFields = new jQuery.Hashtable();
var htSkuItems = new jQuery.Hashtable();

// 以下为需要根据当前商品类型是否具有扩展属性控制显示隐藏的项目
var attributeRow; // 扩展属性操作区域（默认不显示）
var attributeContentHolder; //扩展属性内容显示和操作容器

// 以下为需要根据当前商品类型是否有规格控制显示隐藏的项目
var skuTitle; // “商品规格”标题显示（默认不显示）
var enableSkuRow; // 开启规格提示及操作按钮的显示（默认不显示）
var uploadSKUImg; //上传规格图片按钮的显示（默认隐藏）
// 以下为点击开起规格以后需要显示的项目
var skuRow; // 规格操作区域（默认不显示）

// 以下为开启规格需要隐藏，关闭规格后显示的项目
var skuCodeRow; // 货号
var salePriceRow; // 一口价
var costPriceRow; // 成本价
var qtyRow; // 库存
var warningQtyRow; //警戒库存
var weightRow; // 重量
var volumeRow; // 体积

var skuEnabled = false;
var rootPath = "";
$(document).ready(function () {
    if (location.href.toLowerCase().indexOf("admin/") == -1)
        rootPath = getRealPath();
    skuTitle = document.getElementById("skuTitle");
    attributeRow = document.getElementById("attributeRow");
    enableSkuRow = document.getElementById("enableSkuRow");
    skuRow = document.getElementById("skuRow");
    skuCodeRow = document.getElementById("skuCodeRow");
    salePriceRow = document.getElementById("salePriceRow");
    costPriceRow = document.getElementById("costPriceRow");
    qtyRow = document.getElementById("qtyRow");
    warningQtyRow = document.getElementById("warningqtyRow");
    weightRow = document.getElementById("weightRow");
    volumeRow = document.getElementById("volumeRow");
    uploadSKUImg = document.getElementById("btnUploadSKUImg");

    productTypeSelector = $(".productType"); //商品类型
    brandCategorysSelector = $("#ctl00_contentHolder_dropBrandCategories"); //商品品牌
    attributeContentHolder = $("#attributeContent"); //扩展属性区域容器
    skuTableHolder = $("#skuTableHolder");
    skuFieldHolder = $("#skuFieldHolder");

    productTypeSelector.bind("change", function () { reset(); });
    $("#btnEnableSku").bind("click", function () {
        if ($("#ctl00_contentHolder_ShippingTemplatesDropDownList").val() == "" && !$("#ctl00_contentHolder_radServiceProduct").is(":checked")) {
            ShowMsg("请先选择运费模板",false);
            location.href = "#aShippingTemplate";
            return false;
        }
        //enableSku();
        firstOpenSku();
        $("html,body").animate({ scrollTop: $(skuTitle).offset().top }, 500);
    });
    $("#btnAddItem").bind("click", function () { addItem(); });
    $("#btnCloseSku").bind("click", function () { closeSku(); });
    $("#btnGenerateAll").bind("click", function () { generateAll(); });
    $("#btnshowSkuValue").bind("click", function () { $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2"); showSkuValue(); });
    $("#btnUploadSKUImg").bind("click", function () { $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2"); });
    $(".img_datala").bind("click", function () { $('.hishop_menu_scroll', window.parent.document).css("opacity", "1"); });
    $("#btnGenerate").bind("click", function () { generateSku(); });
    $("#btnClearSku").bind("click", function () { clearSku(); });
    $("#btnBatchOk").bind("click", function () { BatchFillValue(); });
    $("#btnEnableDeduct").bind("click", function () { enableDeduct(); });
    $("#btnUploadSKUImg").bind("click", function () { initSKUImageUpload(); });
    $(".shippingTemplates").bind("change", function (e) {
        var valuationMethod = getValuationMethod();
        if (valuationMethod != "") {
            if (valuationMethod == "2") {
                if (skuEnabled)
                    $("#weightRow").hide();
                else
                    $("#weightRow").show();
                $("#volumeRow").hide();
            }
            else if (valuationMethod == "3") {
                if (skuEnabled)
                    $("#volumeRow").hide();
                else
                    $("#volumeRow").show();
                $("#weightRow").hide();
            }
            else {
                $("#volumeRow").hide();
                $("#weightRow").hide();
            }
            removeORAddWeightField(valuationMethod);
        }
        else {
            $("#volumeRow").hide();
            $("#weightRow").hide();
            removeORAddWeightField("1");
        }
    });
    init();
    $('.shippingTemplates').trigger("change");
});

//检测运费模板的选择
function checkShippingTemplates() {
    if ($("#ctl00_contentHolder_ShippingTemplatesDropDownList").val() == ""&&!$("#ctl00_contentHolder_radServiceProduct").is(":checked")) {
        ShowMsg("请选择运费模板", false);
        location.href = "#aShippingTemplate";
        return false;
    }
    else {
        var valuationMethod = getValuationMethod();
        var volume = parseFloat($("#txtVolume").val());
        var weight = parseFloat($("#txtWeight").val());
        if (!$("#weightRow").is(":hidden")) {
            if (valuationMethod == "2" && isNaN(weight) || weight < 0 || weight > 1000000) {
                ShowMsg("请输入正确的商品重量，数值必须在0-1000000之间","false");
                 
                return false;
            }
        }
        if (!$("#volumeRow").is(":hidden")) {
            if (valuationMethod == "3" && isNaN(volume) || volume < 0 || volume > 1000000) {
                ShowMsg("请输入正确的商品体积，数值必须在0-1000000之间");
                
                return false;
            }
        }
    }
    return true;
}
//获取计价方式
function getValuationMethod() {
    var valuationMethod = "";
    var selectedIndex = $('option:selected', '.shippingTemplates').index();
    var valuationMethod = $(".shippingTemplates").attr("valuationmethodlist");
    if (selectedIndex != 0) {
        valuationMethod = valuationMethod.split(",")[selectedIndex - 1];
    }
    return valuationMethod;
}

function init() {
    currentTypeId = productTypeSelector.val();
    if (currentTypeId.length == 0 || currentTypeId == "0")
        return;

    prepareControls(false);
    restoreState();
}

// 重新选择商品类型以后重置页面所有相关内容
function reset() {
    if (currentTypeId != "") {
        if (!confirm('切换商品类型将会导致已经编辑的品牌，属性和规格数据丢失，确定要切换吗？')) {
            productTypeSelector.val(currentTypeId);
            return false;
        }
    }
    skuCounts = 0;
    skuEnabled = false;
    currentTypeId = productTypeSelector.val();
    attributeContentHolder.empty();
    skuTableHolder.empty();
    setCtrlDisplay("");
    skuRow.style.display = "none";
    uploadSKUImg.style.display = "none";
    htSkuFields.clear();
    htSkuItems.clear();
    reBindValid();

    $("#ctl00_contentHolder_chkSkuEnabled").iCheck('uncheck');

    prepareControls(true);
    if ($("#ctl00_contentHolder_ShippingTemplatesDropDownList").val() == "") {
        $("#volumeRow").hide();
        $("#weightRow").hide();
    }
}

function prepareControls(isReset) {
   
    if (currentTypeId.length == 0) currentTypeId = "0";
    var postUrl = rootPath+"/admin/product/addproduct?isCallback=true&action=getPrepareData&timestamp=";
    postUrl += new Date().getTime() + "&typeId=" + currentTypeId;

    $.ajax({
        url: postUrl,
        type: 'GET', dataType: 'json', timeout: 10000,
        async: false,
        success: function (resultData) {
            var hasAttribute = (resultData.HasAttribute == "True");
            var hasSku = (resultData.HasSKU == "True")
            var hasBrandCategory = (resultData.HasBrandCategory == "True");
            var hasAttImg = (resultData.HasAttributeImage == "True");
            setCtrlDisplay("");
            updateDisplayStatus(hasAttribute, hasSku, hasAttImg, resultData.ImageAttributeText, resultData.ImageAttributeId);
            if (hasAttribute)
                prepareAttributes(resultData.Attributes);
            if (isReset && hasBrandCategory)
                prepareBrandCategories(resultData.BrandCategories);
            if (hasSku && resultData.SKUs.length > 0) {
                cellFields = new Array(resultData.SKUs.length);
                bindCellFields = new Array(resultData.SKUs.length);
                skuCounts = resultData.SKUs.length;

                $.each(resultData.SKUs, function (i, skuItem) {
                    cellFields[i] = skuItem.AttributeId;
                    bindCellFields[i] = skuItem.AttributeId;
                    htSkuFields.add(skuItem.AttributeId, skuItem);
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowMsg(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus + "-" + postUrl);
        },

    });
}

function updateDisplayStatus(hasAttribute, hasSku, hasAttImg, imageAttributeText, imageAttributeId) {
    attributeRow.style.display = hasAttribute ? "" : "none";
    //skuTitle.style.display = hasSku ? "" : "none";
    skuTitle.style.display = "";
    enableSkuRow.style.display = hasSku ? "" : "none";
    skuRow.style.display = "none";
    if (hasAttImg) {
        uploadSKUImg.style.display = "inlie";
        $(uploadSKUImg).val("上传\"" + imageAttributeText + "\"规格图片");
        $(uploadSKUImg).attr("attrId", imageAttributeId);
    }
    else {
        uploadSKUImg.style.display = "none";
    }
}

function prepareAttributes(attributes) {
    if (attributes == null || attributes == undefined || attributes.length == 0)
        return;

    var ulTag = $("<ul><\/ul>");
    attributeContentHolder.append(ulTag);   
    $.each(attributes, function (i, attribute) {
        var liTag = $(String.format("<li class=\"attributeItem\" attributeId=\"{0}\" usageMode=\"{1}\"><\/li>", attribute.AttributeId, attribute.UsageMode));
        var liLDiv = $("<div style=\"float:left\"></div>");
        var liRDiv = $("<div style=\"float:left;width:690px\" ></div>");
        var titleSpan = $(String.format("<span class=\"formitemtitle\" style=\"float:left\">{0}：<\/span>", attribute.Name));
        liLDiv.append(titleSpan);
        liLDiv.append(liRDiv);
        if (attribute.UsageMode == "1") {
            var selectTag = $(String.format("<select id=\"attribute{0}\" class=\"iselect_one\" style=\"float:left; margin-right:10px;width:150px\"><\/select>", attribute.AttributeId));
            selectTag.append($("<option value=\"\">\u8BF7\u9009\u62E9<\/option>"));
            if (attribute.AttributeValues.length > 0) {
                $.each(attribute.AttributeValues, function (vIndex, attributeValue) {
                    selectTag.append($(String.format("<option value=\"{0}\">{1}<\/option>", attributeValue.ValueId, decode_Hishop(attributeValue.ValueStr))));
                });
            }
            liRDiv.append(selectTag);
        }
        else if (attribute.UsageMode == "0") {
            if (attribute.AttributeValues.length > 0) {
                var checkGroupName = "vallist" + attribute.AttributeId;
                    $.each(attribute.AttributeValues, function (vIndex, attributeValue) {
                        var cid = String.format("att_{0}_{1}", attribute.AttributeId, attributeValue.ValueId);
                        var valItem = $("<span id=\"span_" + attribute.AttributeId + "\" class=\"valspan\" style=\"float:left;margin-right:5px;\"><\/span>");
                        valItem.append($(String.format("<input type=\"checkbox\" class=\"icheck\"  name=\"{0}\" id=\"{1}\" valueId=\"{2}\" \/>", checkGroupName, cid, attributeValue.ValueId)));
                        valItem.append($(String.format("<label for=\"{0}\">{1}<\/label>", cid, decode_Hishop(attributeValue.ValueStr))));
                        liRDiv.append(valItem);
                    });
            }
        }
        //liLDiv.append($("<a href=\"javascript:void(0)\" onclick=\"ShowAttributeValue(" + attribute.AttributeId + ",this)\" class=\"input-group-a\" style=\"margin-top:10px\">添加</a>"));
        liTag.append(liLDiv);
        if (rootPath == "") {
            liRDiv.append($("<div style=\"float:left\"><a href=\"javascript:void(0)\" onclick=\"ShowAttributeValue(" + attribute.AttributeId + ",this)\" class=\"input-group-a\" style=\"float:left\">添加</a><div id=\"div_" + attribute.AttributeId + "\" class=\"input-group\" style=\"display:none;height:33px;float:left;margin:0 0 0 10px;\"><input class=\"form-control\" style=\"width:120px;margin-right:10px;\" type=\"text\" id=\"txtvalue" + attribute.AttributeId + "\"/><input type=\"button\" class=\"btn btn-primary\"  style=\"float:left\" value=\"保存\" onclick=\"AddValue('" + attribute.AttributeId + "','" + attribute.UsageMode + "',this)\"/></div></div>"));
        }
           
        ulTag.append(liTag);
    });
    attributeRow.style.display = "";
    $('.icheck').iCheck({
        checkboxClass: 'icheckbox_square-red',
        radioClass: 'iradio_square-red',
        increaseArea: '20%' // optional
    });
}



//显示添加属性值的文本框
function ShowAttributeValue(attributeId, obja) {
    if ($(obja).text() == "添加") {
        $("#div_" + attributeId).show();
        $(obja).text('取消');
        $(obja).attr('class', 'input-group-a');
    } else {
        $("#div_" + attributeId).hide();
        $(obja).text('添加');
        $(obja).attr('class', 'input-group-a');
    }
}

//保存属性
function AddValue(attributeId, userMode, btnobj) {
    var valuename = $("#txtvalue" + attributeId).val().replace(/\s/g, "");
    if (valuename == "") {
        ShowMsg("请输入要添加的属性值");
        return false;
    }
    if (valuename.length > 15 || valuename.indexOf(',') >= 0) {
        ShowMsg("输入的属性值字符必须控制在15个字符并且不包含英文逗号");
        return false;
    }
    $("#div_" + attributeId).hide('hide');
    $("#txtvalue" + attributeId).val('');
    $(btnobj).parent().parent().find("a").text('添加');
    $(btnobj).parent().parent().find("a").attr("class", "input-group-a");
    AddAttributeValue(attributeId, valuename, userMode);
}


function AddAttributeValue(attributeId, valuename, userMode) {
    $.post(rootPath+"/Admin/product/AddSpecification",
        { AttributeId: attributeId, ValueName: valuename, Mode: "Add", isAjax: "true" },
        function (data) {
            if (data.Status == "true") {
                ShowMsg("添加属性值成功", true);
                if (userMode == "1") {
                    $("#attribute" + attributeId).append($(String.format("<option value=\"{0}\" selected=\"selected\">{1}<\/option>", data.msg, valuename)));
                    $("#attribute" + attributeId).val(data.msg);
                } else {
                    var checkGroupName = "vallist" + attributeId;
                    var cid = String.format("att_{0}_{1}", attributeId, data.msg);

                    var valItem = $("<span id=\"span_" + attributeId + "\" class=\"valspan\" style=\"float:left;margin-right:5px;\"><\/span>");
                    valItem.append($(String.format("<input type=\"checkbox\" class=\"icheck\"  name=\"{0}\" id=\"{1}\" valueId=\"{2}\"  checked=\"checked\" \/>", checkGroupName, cid, data.msg)));
                    valItem.append($(String.format("<label for=\"{0}\">{1}<\/label>", cid, valuename)));
                    $("#span_" + attributeId).parent().children(":last-child").before(valItem);
                    $('.icheck').iCheck({
                        checkboxClass: 'icheckbox_square-red',
                        radioClass: 'iradio_square-red',
                        increaseArea: '20%' // optional
                    });
                }

            }
            else {
                ShowMsg(data.msg, false);
            }
        },
        "json"
    );
}

function prepareBrandCategories(brandCategories) {
    document.getElementById("ctl00_contentHolder_dropBrandCategories").options.length = 0;

    var scroll = $("#ctl00_contentHolder_dropBrandCategories").parent().parent().find("#scroll");
    var scrolls = $("#ctl00_contentHolder_dropBrandCategories").parent().parent().find("#scroll li");
    var selectobj = $("#ctl00_contentHolder_dropBrandCategories").parent().parent().find(".selected");
    $(selectobj).html("请选择");
    $(scroll).find("li").remove();
    brandCategorysSelector.append("<option selected=\"selected\" value=\"0\">--\u8BF7\u9009\u62E9--<\/option>");
    $(scroll).append("<li class>--\u8BF7\u9009\u62E9--</li>")

    $.each(brandCategories, function (i, brand) {
        brandCategorysSelector.append(String.format("<option value=\"{0}\">{1}<\/option>", brand.BrandId, brand.BrandName));
        $(scroll).append("<li>" + brand.BrandName + "</li>")
    });
    $(scroll).children("li").click(function (e) {
        var seltext = $(this).text();
        $(selectobj).html(seltext);
        $(brandCategorysSelector).get(0).selectedIndex = $("#ctl00_contentHolder_dropBrandCategories").parent().parent().find("#scroll li").index($(this));
        brandCategorysSelector.options[brandCategorysSelector.selectedIndex].selected = true;

    });
}

function restoreState() {
    if ($("#ctl00_contentHolder_txtAttributes").val().length > 0) {

        var xml;
        if ($.browser.msie) {
            xml = new ActiveXObject("Microsoft.XMLDOM");
            xml.async = false;
            xml.loadXML($("#ctl00_contentHolder_txtAttributes").val());
        }
        else {
            xml = new DOMParser().parseFromString($("#ctl00_contentHolder_txtAttributes").val(), "text/xml");
        }

        // 属性值回发状态维护
        var selectedAttributes = $(xml).find("item");

        $.each(selectedAttributes, function (itemIndex, itemNode) {
            var attributeId = $(itemNode).attr("attributeId");
            var valueList = $(itemNode).children();

            if ($(itemNode).attr("usageMode") == "0") {
                $.each(valueList, function () {
                    var ctl = $("input[name='vallist" + attributeId + "'][valueId='" + $(this).attr("valueId") + "']");
                    if ($(ctl).length > 0) $(ctl).iCheck('check');
                });
            }
            else {
                var attributeControl = $("#attribute" + attributeId);
                if ($(attributeControl).length > 0) $(attributeControl).val($(itemNode).children().eq(0).attr("valueId"));
            }
        });
    }
    if ($("#ctl00_contentHolder_chkSkuEnabled").attr("checked") == "checked") {
        // 规格值回发状态维护
        enableSku();
        var xml;
        if ($.browser.msie) {
            xml = new ActiveXObject("Microsoft.XMLDOM");
            xml.async = false;
            xml.loadXML($("#ctl00_contentHolder_txtSkus").val());
        }
        else {
            xml = new DOMParser().parseFromString($("#ctl00_contentHolder_txtSkus").val(), "text/xml");
        }
        var selectedSkus = $(xml).find("item");
        $.each($(".fieldCell"), function () {
            var skuId = $(this).attr("skuId");
            if ($(selectedSkus[0]).find("sku[attributeId='" + skuId + "']").length == 0) {
                removeSkuField(skuId, $(this).children().eq(0).text(), false);
            }
        });

        $.each(selectedSkus, function () {
            var rowIndex = addItem();
            $("#skuCode_" + rowIndex).val($(this).attr("skuCode"));
            $("#salePrice_" + rowIndex).val($(this).attr("salePrice"));
            $("#costPrice_" + rowIndex).val($(this).attr("costPrice"));
            $("#costPricespan_" + rowIndex).html($(this).attr("costPrice"));
            $("#qty_" + rowIndex).val($(this).attr("qty"));
            $("#qtyspan_" + rowIndex).html($(this).attr("qty"));
            $("#warningqty_" + rowIndex).val($(this).attr("warningQty"));
            $("#warningqtyspan_" + rowIndex).html($(this).attr("warningQty"));
            var valuationMethod = getValuationMethod();
            if (valuationMethod == 2 || valuationMethod == 3) {
                $("#weight_" + rowIndex).val($(this).attr("weight"));
                $("#weightspan_" + rowIndex).html($(this).attr("weight"));
            }

            $.each($(this).find("sku"), function () {
                var attributeId = $(this).attr("attributeId");
                var valueId = $(this).attr("valueId");
                selectSkuValue(rowIndex, attributeId, valueId, $("span[class='sku" + attributeId + "values'][valueId='" + valueId + "']").text());
            });

            var s = "";
            if ($(this).find("memberGrande").length > 0) {
                s = "<xml><gradePrices>";
                $.each($(this).find("memberGrande"), function () {
                    s += String.format("<grande id=\"{0}\" price=\"{1}\" \/>", $(this).attr("id"), $(this).attr("price"));
                });
                s += "<\/gradePrices><\/xml>";
            }
            $("#gradeSalePrice_" + rowIndex).val(s);
        });
        bindSkuImages();
        HasSelectedImgAttribute();
    }
}

function setCtrlDisplay(displayCssStatus) {
    skuCodeRow.style.display = displayCssStatus;
    if (rootPath == "")
        salePriceRow.style.display = displayCssStatus;
    costPriceRow.style.display = displayCssStatus;
    qtyRow.style.display = displayCssStatus;
    warningQtyRow.style.display = displayCssStatus;
    var valuationMethod = getValuationMethod();
    if (valuationMethod == "2") {
        $("#weightRow").show();
        $("#volumeRow").hide();
    }
    else if (valuationMethod == "3") {
        $("#weightRow").hide();
        $("#volumeRow").show();
    }
    else {
        $("#weightRow").hide();
        $("#volumeRow").hide();
    }
    //weightRow.style.display = displayCssStatus;
    //volumeRow.style.display = displayCssStatus;
    enableSkuRow.style.display = displayCssStatus;
}



// 开启佣金
function enableDeduct(state) {

    var OpenSecondReferral = $("#hidOpenSecondReferral").val();//是否开启二级分佣
    var OpenThirdReferral = $("#hidOpenThirdReferral").val();//是否开启二级分佣
    if (state) {
        $("#liSubMemberDeduct").show();
        if (OpenSecondReferral == "1") {
            $("#liReferralDeduct").show();
        }
        else {
            $("#liReferralDeduct").hide();
            $("#liSubReferralDeduct").hide();
        }
        if (OpenThirdReferral == "1") {
            $("#liSubReferralDeduct").show();
        }
        else {
            $("#liSubReferralDeduct").hide();
        }
        var subMemberDeduct = parseFloat($("#ctl00_contentHolder_txtSubMemberDeduct").val());
        var secondLevelDeduct = parseFloat($("#ctl00_contentHolder_txtSecondLevelDeduct").val());
        var threeLevelDeduct = parseFloat($("#ctl00_contentHolder_txtThreeLevelDeduct").val());
        //佣金都设置为0时，则关闭了佣金，此时开启分销佣金，则为系统默认值，否则保留原值
        if ((isNaN(subMemberDeduct) || subMemberDeduct == 0) && (isNaN(secondLevelDeduct) || secondLevelDeduct == 0) && (isNaN(threeLevelDeduct) || threeLevelDeduct == 0)) {
            $("#ctl00_contentHolder_txtSubMemberDeduct").val("");
            $("#ctl00_contentHolder_txtSecondLevelDeduct").val("");
            $("#ctl00_contentHolder_txtThreeLevelDeduct").val("");
        }
        else {
            if (isNaN(subMemberDeduct)) {
                $("#ctl00_contentHolder_txtSubMemberDeduct").val("");
            }
            if (isNaN(secondLevelDeduct)) {
                $("#ctl00_contentHolder_txtSecondLevelDeduct").val("");
            }
            if (isNaN(threeLevelDeduct)) {
                $("#ctl00_contentHolder_txtThreeLevelDeduct").val("");
            }
        }
    } else {
        $("#liReferralDeduct").hide();
        $("#liSubReferralDeduct").hide();
        $("#liSubMemberDeduct").hide();
        if (!state) {
            $("#ctl00_contentHolder_txtSubMemberDeduct").val("0");
            $("#ctl00_contentHolder_txtSecondLevelDeduct").val("0");
            $("#ctl00_contentHolder_txtThreeLevelDeduct").val("0");
        }
    }
    //   $("#enableDeduct").hide();
}

// 关闭规格
function closeSku() {
    if ($(".SpecificationTr").length > 0 && !confirm("关闭规格后现已添加的所有规格数据都会丢失，确定要关闭吗？"))
        return;

    setCtrlDisplay("");
    skuRow.style.display = "none";
    uploadSKUImg.style.display = "none";
    skuTableHolder.empty();
    htSkuItems.clear();
    reBindValid();
    skuEnabled = false;
    HasSelectedImgAttribute();
    $("#ctl00_contentHolder_chkSkuEnabled").iCheck('uncheck');
}

function prepareSkus() {
    skuTable = $("<table width=\"100%\" border=\"0\" cellSpacing=\"0\" cellPadding=\"2\" class=\"SpecificationTable\"><\/table>");
    tableBody = $("<tbody><\/tbody>");
    tableHeader = $("<tr class=\"SpecificationTh\"><\/tr>");

    for (cellIndex = 0; cellIndex < cellFields.length; cellIndex++) {
        var skuId = cellFields[cellIndex];
        var skuItem = htSkuFields.get(skuId);
        var fieldCell = createFieldCell(skuId, skuItem.Name);
        tableHeader.append(fieldCell);
        var skuBox = $(String.format("<div style=\"display: none;\" id=\"skuBox_{0}\" class=\"target_box\"><\/div>", skuId));
        fillSkuBox(skuBox, skuId, skuItem.SKUValues);
        $("body").append(skuBox);
    }
    var valuationMethod = getValuationMethod();
    tableHeader.append($("<td align=\"center\">货号<\/td>"));
    if (rootPath == "") {
        tableHeader.append($("<td align=\"center\" width=\"200\" ><em >*<\/em>一口价<\/td>"));
        tableHeader.append($("<td align=\"center\">成本价<\/td>"));
    } else {
        tableHeader.append($("<td style='display:none;' >一口价<\/td>"));
        tableHeader.append($("<td align=\"center\" id='storeCostPrice' name='storeCostPrice'><em >*<\/em>供货价<\/td>"));
    }
    if (valuationMethod == 2) {
        tableHeader.append($("<td align=\"center\" id='weightOrVolumeField'><em >*<\/em>重量(KG)<\/td>"));
        $("#txtWeight").val(0);
        $("#txtVolume").val(0);
        $("#txtBatchWeight").attr("placeholder", "重量");
    }
    else if (valuationMethod == 3) {
        tableHeader.append($("<td align=\"center\" id='weightOrVolumeField'><em >*<\/em>体积<\/td>"));
        $("#txtVolume").val(0);
        $("#txtWeight").val(0);
        $("#txtBatchWeight").attr("placeholder", "体积");
    }
    else {
        $("#batchWeight").hide();
    }
    tableHeader.append($("<td align=\"center\" id='storeField'><em >*<\/em>库存<\/td>"));
    tableHeader.append($("<td align=\"center\" >警戒库存<\/td>"));
    tableHeader.append($("<td align=\"center\" >操作<\/td>"));
    tableBody.append(tableHeader);

    skuTable.append(tableBody);
    skuTableHolder.append(skuTable);
    //if(rootPath)
    skuTableHolder.append($("<input type=\"button\"  id=\"btnAddItem\" onclick=\"addItem()\" value=\"\" />"));
}

//function generateAll() {
//    if (cellFields.length == 0) {
//        ShowMsg("生成所有规格以前至少需要加入一个规格项！");
//        return;
//    }

//    var dataRows = $(".SpecificationTr");
//    if (dataRows.length > 0 && !confirm("生成所有规格前会先删除已编辑的所有规格，确定吗？"))
//        return;

//    var skuValues = htSkuFields.get(cellFields[0]).SKUValues;
//    var skuArray = new Array(skuValues.length);
//    $.each(skuValues, function (i, skuValue) {
//        skuArray[i] = new Array(1);
//        skuArray[i][0] = skuValue;
//    });
//    var allIndex = 0;
//    for (index = 1; index < cellFields.length; index++) {
//        skuValues = htSkuFields.get(cellFields[index]).SKUValues;
//        var tmpArray = new Array(skuArray.length * skuValues.length);
//        var rowCounter = 0;
//        for (sindex = 0; sindex < skuValues.length; sindex++) {
//            for (cindex = 0; cindex < skuArray.length; cindex++) {

//                tmpArray[rowCounter] = new Array(index + 1);
//                for (rindex = 0; rindex < (index + 1) ; rindex++) {

//                    if (rindex == index)
//                        tmpArray[rowCounter][rindex] = skuValues[sindex];
//                    else {
//                        tmpArray[rowCounter][rindex] = skuArray[cindex][rindex];
//                    }
//                    allIndex += 1;
//                }
//                rowCounter++;
//            }
//        }

//        skuArray = tmpArray;
//    }
//    $.each(dataRows, function () { $(this).remove(); });
//    for (i = 0; i < skuArray.length; i++) {
//        var rowIndex = addItem();
//        for (j = 0; j < cellFields.length; j++) {
//            var skuItem = skuArray[i][j];
//            selectSkuValue(rowIndex, cellFields[j], skuItem.ValueId, skuItem.ValueStr);
//        }
//    }
//    HasSelectedImgAttribute();
//}


// 生成部分规格
//function generateSku() {
//    var dataRows = $(".SpecificationTr");
//    var currentSkuFields = getSkuFields();
//    var skuValues = currentSkuFields.get(cellFields[0]).SKUValues;
//    var skuArray = new Array(skuValues.length);

//    $.each(skuValues, function (i, skuValue) {
//        skuArray[i] = new Array(1);
//        skuArray[i][0] = skuValue;
//    });

//    for (index = 1; index < cellFields.length; index++) {
//        skuValues = currentSkuFields.get(cellFields[index]).SKUValues;
//        var tmpArray = new Array(skuArray.length * skuValues.length);
//        var rowCounter = 0;

//        for (sindex = 0; sindex < skuValues.length; sindex++) {
//            for (cindex = 0; cindex < skuArray.length; cindex++) {
//                tmpArray[rowCounter] = new Array(index + 1);
//                for (rindex = 0; rindex < (index + 1) ; rindex++) {
//                    if (rindex == index)
//                        tmpArray[rowCounter][rindex] = skuValues[sindex];
//                    else {
//                        tmpArray[rowCounter][rindex] = skuArray[cindex][rindex];
//                    }
//                }
//                rowCounter++;
//            }
//        }

//        skuArray = tmpArray;
//    }


//    $.each(dataRows, function () { $(this).remove(); });
//    for (i = 0; i < skuArray.length; i++) {
//        var rowIndex = addItem();
//        for (j = 0; j < cellFields.length; j++) {
//            var skuItem = skuArray[i][j];
//            selectSkuValue(rowIndex, cellFields[j], skuItem.ValueId, skuItem.ValueStr);
//        }
//    }
//    HasSelectedImgAttribute();
//    CloseDiv('skuValueBox');
//}
//// 获取要生成哪些规格
//function getSkuFields() {
//    var currentSkuFields = new jQuery.Hashtable();
//    for (i = 0; i < cellFields.length; i++) {
//        var skuItems = $(String.format("input[type='checkbox'][name='cp_{0}']:checked", cellFields[i]));
//        var skuStr = "({";
//        skuStr += String.format("\"Name\":\"{0}\",", htSkuFields.get(cellFields[i]).Name);
//        skuStr += String.format("\"AttributeId\":\"{0}\",", cellFields[i]);

//        var skuValueStr = "";
//        $.each(skuItems, function (itemIndex, skuItem) {
//            skuValueStr += "{" + String.format("\"ValueId\":\"{0}\",\"ValueStr\":\"{1}\"", $(skuItem).attr("valueId"), $(skuItem).attr("valueStr")) + "},";
//        });
//        if (skuValueStr != "")
//            skuValueStr = skuValueStr.substring(0, skuValueStr.length - 1);
//        skuStr += String.format("\"SKUValues\":[{0}]", skuValueStr);
//        skuStr += "})"
//        currentSkuFields.add(cellFields[i], eval(skuStr));
//    }
//    return currentSkuFields;
//}

// 展示要生成的部分规格内容
function showSkuValue() {
    if (bindCellFields.length == 0) {
        ShowMsg("生成部分规格以前至少需要加入一个规格项！");
        return;
    }

    $("#skuItems").empty();
    var values;
    var arryContent = new Array();
    arryContent.push("<ul>");
    for (index = 0; index < bindCellFields.length; index++) {
        values = htSkuFields.get(bindCellFields[index]).SKUValues;
        arryContent.push(String.format("<li class=\"skuItem\" skuId=\"{0}\" style=\"margin-bottom:0\">", bindCellFields[index]));
        arryContent.push(String.format("<span class=\"formitemtitle4\" style=\"width:100%;float:left;\">{0}：<\/span>", htSkuFields.get(bindCellFields[index]).Name));
        arryContent.push("<span class=\"skuItemList\"><ul style=\"width:100%;float:left;\">");
        for (var i = 0; i < values.length; i++) {
            var skuValue = values[i];
            arryContent.push("<li style=\"padding:5px 0;margin-bottom:0;float:left;\" class=\"contentLi\">");
            arryContent.push(String.format("<input type=\"checkbox\" class=\"icheck\" style=\"float:left\"   name=\"cp_{0}\" id=\"prop_{0}_{1}\" value=\"{0}:{1}\" valueId=\"{1}\" valueStr=\"{2}\" \/>",
                bindCellFields[index], skuValue.ValueId, skuValue.ValueStr));
            arryContent.push(String.format("<span style=\"margin-top:6px;float:left;\" itemId=\"prop_{1}_{2}\">{0}<\/span>", decodeURIComponent(skuValue.ValueStr), bindCellFields[index], skuValue.ValueId));
            arryContent.push("<\/li>");
        }
        if (rootPath == "") {
            arryContent.push(String.format("<li style=\"float:left;overflow:hidden;width:auto;\" id='li_{0}'>", bindCellFields[index]));
            arryContent.push(String.format("<a  id='btnadd_{0}'  href=\"javascript:;\" style=\"margin-top:6px;line-height:1.8;float:left\">添加</a>", bindCellFields[index]));
            arryContent.push(String.format("<span style='display:none;margin-top:10px;' id='sp_{0}'><input type=text id='txt_{0}' class='form-control' style='width:160px' name='txt_{0}'><input type=button id='btnins_{0}' class='btn btn-primary' value='保存' onclick='addSkuValue({0})'  style='margin-left:10px'></span>", bindCellFields[index]));
            arryContent.push("<\/li>");
        }
        arryContent.push("<\/ul><\/span><\/li>");
    }
    arryContent.push("<\/ul>");
    $("#skuItems").append(arryContent.join(""));
    $('.contentLi .icheck').iCheck({
        checkboxClass: 'icheckbox_square-red',
        radioClass: 'iradio_square-red',
        increaseArea: '20%' // optional
    });
    if (rootPath == "") {
        $($("a[id^='btnadd_']")).click(function () {
            $(this).next().show();
            $(this).hide();
        });
    }
    // 已经生成的规格默认选中
    $.each($(".specdiv"), function (itemIndex, itemNode) {
        var skuItems = $("input[type='checkbox']");
        $.each(skuItems, function (attIndex, attNode) {
            if ($(itemNode).attr("valueId") == $(attNode).attr("valueId"))
                $(attNode).iCheck('check');
        });
    });

    DivWindowOpen(750, 500, 'skuValueBox');
}
function addSkuValue(attrId) {
    var valueStr = $("#txt_" + attrId).val();
    $.post(rootPath+"/Admin/product/AddSpecification",
        { AttributeId: attrId, ValueName: valueStr, Mode: "AddSkuItemValue", isAjax: "true" },
        function (data) {
            if (data.Status == "true") {
                var valueId = data.msg;
                var contentLi = $("<li style=\"padding:5px 0;margin-bottom:0;float:left;\" class=\"contentLi\"><\/li>");
                var chkItem = $(String.format("<input type=\"checkbox\" class=\"icheck\"  style=\"float:left\"  name=\"cp_{0}\" id=\"prop_{0}_{1}\" value=\"{0}:{1}\" valueId=\"{1}\" valueStr=\"{2}\"  \/>",
                attrId, valueId, valueStr));
                chkItem.iCheck('check');
                var valueSpan = $(String.format("<span style=\"margin-top:6px;float:left;\" itemId=\"prop_{1}_{2}\">{0}<\/span>", valueStr, attrId, valueId));
                contentLi.append(chkItem);
                contentLi.append(valueSpan);
                contentLi.insertBefore($("#li_" + attrId));
                $("#sp_" + attrId).hide();
                $("#txt_" + attrId).val();
                $("#btnadd_" + attrId).show();
                var skuItems = htSkuFields.get(attrId).SKUValues;
                var skuItem = { "ValueId": valueId, "ValueStr": valueStr };
                skuItems.push(skuItem);
                htSkuFields[attrId].SKUValues = skuItems;
            }
            else {
            }
        },
        "json"
    );
}
function removeSkuField(skuId, skuName, showConfirm) {

    if (skuCounts <= 1) { ShowMsg("请至少保留一个自定义规格项或者关闭规格。"); return false; }
    if (showConfirm && !confirm("确定要从商品规格中删除 \"" + skuName + "\" 吗？"))
        return;
    var fieldCell = $(".SpecificationTable td[skuId='" + skuId + "']");
    var cellIndex = fieldCell.parent("tr").children().index(fieldCell);

    $(".SpecificationTable tr").each(function () {
        $("td:eq(" + cellIndex + ")", $(this)).remove();
    });
    skuCounts -= 1;
    var tmpArr = new Array(cellFields.length - 1);
    var counter = 0;
    for (i = 0; i < cellFields.length; i++) {
        if (cellFields[i] != skuId) {
            tmpArr[counter] = cellFields[i];
            counter++;
        }
    }
    cellFields = tmpArr;
    //var skuField = $(String.format("<span class=\"skufield\" id=\"addSku_{1}\" onclick=\"addSkuField({1},'{2}');\" cellIndex=\"{0}\" skuId=\"{1}\" skuName=\"{2}\"><a href=\"javascript:;\">{2}<sup class=\"glyphicon glyphicon-remove\"><\/sup><\/a><\/span>", cellIndex, skuId, skuName));
    //skuFieldHolder.append(skuField);
    //skuFieldHolder.css("display", "");

    htSkuItems.clear();

    $.each($(".SpecificationTr"), function () {
        var rowId = $(this).attr("rowindex");
        var rowIdentity = getRowIdentity(rowId);
        if (htSkuItems.containsValue(rowIdentity))
            $(this).remove();
        else
            htSkuItems.add(rowId, rowIdentity);
    });
    HasSelectedImgAttribute();
}

//function addSkuField(skuField) {
function addSkuField(skuId, skuName) {
    //var skuId = $(skuField).attr("skuId");
    var fieldCell = createFieldCell(skuId, skuName);

    $(fieldCell).insertBefore($("td:eq(0)", $(tableHeader)));
    $.each($(".SpecificationTr"), function () {
        var skuCell = createSkuCell($(this).attr("rowindex"), skuId);
        $(skuCell).insertBefore($("td:eq(0)", $(this)));
    });
    skuCounts += 1;
    var tmpArr = new Array(cellFields.length + 1);
    tmpArr[0] = skuId;
    for (i = 1; i <= cellFields.length; i++) {
        tmpArr[i] = cellFields[i - 1];
    }
    cellFields = tmpArr;

    //$(skuField).remove();
    $("#addSku_" + skuId).remove();
    if ($(skuFieldHolder).children().length == 0)
        skuFieldHolder.css("display", "none");
}

function fillSkuBox(box, skuId, skuValues) {
    $.each(skuValues, function (valIndex, val) {
        box.append($(String.format("<span valueId=\"{0}\" class=\"sku{1}values\" style=\"padding:3px;\">{2}<\/span>", val.ValueId, skuId, decode_Hishop(val.ValueStr))));
    });
}

var newRowIndex = 0;
// 增加一个规格
function addItem() {
    if (cellFields.length == 0) {
        ShowMsg("增加一个规格前必须先选择一个规格名！");
        return false;
    }
    var valuationMethod = getValuationMethod();
    newRowIndex++;
    var dataRow = $(String.format("<tr id=\"sku_{0}\" rowindex=\"{0}\" class=\"SpecificationTr\"><\/tr>", newRowIndex));

    for (itemIndex = 0; itemIndex < cellFields.length; itemIndex++) {
        dataRow.append(createSkuCell(newRowIndex, cellFields[itemIndex]));
    }
    dataRow.append(createSkuCodeCell(newRowIndex));    
    dataRow.append(createSalePriceCell(newRowIndex));//一口价
    dataRow.append(createCostPriceCell(newRowIndex));
    if (valuationMethod == 2 || valuationMethod == 3) {
        dataRow.append(createWeightCell(newRowIndex));
    }
    dataRow.append(createQtyCell(newRowIndex));
    dataRow.append(createWarningQtyCell(newRowIndex));
    //dataRow.append(createWeightCell(newRowIndex));
    dataRow.append(createActionCell(newRowIndex));

    tableBody.append(dataRow);
    return newRowIndex;
}

function createFieldCell(skuId, skuName) {
    var fieldCell = $(String.format("<td align=\"center\" class=\"fieldCell\" style=\"width:50px !important\" skuId=\"{0}\"><span>{1}<\/span><\/td>", skuId, skuName));
    //var delCtl = $("<a href=\"javascript:;\" onclick=\"removeSkuField(" + skuId + ",'" + skuName + "', true);\" title=\"删除此规格项\" style=\"line-hiegth:30px;margin-top:-10px;\"><sup class=\"glyphicon glyphicon-remove\"><\/sup><\/a>");
    //fieldCell.append(delCtl);
    return fieldCell;
}

function createSkuCell(rowIndex, skuId) {
    var cell = createCell();
    var panel = $(String.format("<div id=\"skuDisplay_{0}_{1}\" rowId=\"{0}\" skuId=\"{1}\" valueId=\"\" class=\"specdefault\">请选择<\/div>", rowIndex, skuId));
    $(panel).powerFloat({
        eventType: "click",
        target: $("#skuBox_" + skuId),
        showCall: adjustSkuBox,

    });
    cell.append(panel);
    return cell;
}

function adjustSkuBox() {
    var rowId = $(this).attr("rowId");
    var skuId = $(this).attr("skuId");
    var skuBox = $("#skuBox_" + skuId);
    var valueList = $(String.format(".sku{0}values", skuId));
    if (valueList.length == 0) {
        ShowMsg("该规格没有规格值,请先添加规格值，或者删除该规格");
        return;
    }
    $.each(valueList, function (i, listItem) {
        $(listItem).unbind("click"); //因为每个规格都是用的同一个弹出层，所以必须先取消上一次事件绑定
        if (checkValue(rowId, skuId, $(this).attr("valueId"))) {
            $(listItem).addClass("specsna");
        }
        else {
            $(listItem).addClass("specspan").removeClass("specsna");
            $(listItem).bind("click", function () {
                selectSkuValue(rowId, skuId, $(this).attr("valueId"), $(this).html());
                HasSelectedImgAttribute();
            });
        }
    });
}

//在解码时 当字符串内容特殊字符解码 解码不成功报错，则这里特殊处理下，如报错说明不需解码
function decode_Hishop(valueStr) {
    try { return decodeURIComponent(valueStr); } catch (ex) { return valueStr; }
}

function selectSkuValue(rowId, skuId, valueId, valueStr) {
    var displayCtl = $(String.format("#skuDisplay_{0}_{1}", rowId, skuId));
    $(displayCtl).html(decode_Hishop(valueStr));
    $(displayCtl).attr("valueId", valueId);
    $(displayCtl).addClass("specdiv").removeClass("specdefault");

    var rowIdentity = getRowIdentity(rowId);
    if (htSkuItems.containsKey(rowId))
        htSkuItems.items[rowId] = rowIdentity;
    else
        htSkuItems.add(rowId, rowIdentity);

    $.powerFloat.hide();
}

function checkValue(rowId, skuId, valueId) {
    var rowIdentity = "";
    for (index = 0; index < cellFields.length; index++) {
        if (cellFields[index] == skuId)
            rowIdentity += valueId + "-";
        else
            rowIdentity += $(String.format("#skuDisplay_{0}_{1}", rowId, cellFields[index])).attr("valueId") + "-";
    }
    return htSkuItems.containsValue(rowIdentity);
}

function getRowIdentity(rowId) {
    var rowIdentity = "";
    for (index = 0; index < cellFields.length; index++) {
        rowIdentity += $(String.format("#skuDisplay_{0}_{1}", rowId, cellFields[index])).attr("valueId") + "-";
    }
    return rowIdentity;
}

function createSalePriceCell(rowIndex) {
    var cell = createCell(rootPath == "" ? false : true);
    var priceCell = $(String.format("<input type=\"text\" class=\"skuItem_SalePrice form-control\"  id=\"salePrice_{0}\"\ style=\"width:80px;float:left;margin:0 5px 0 15px;\" \/>", rowIndex));
    var gradePrice = $(String.format("<input type=\"text\" id=\"gradeSalePrice_{0}\"\ style=\"width:50px;display:none\" \/>", rowIndex));
    var btnSkuMemberPrice = $("<input type=\"button\" value=\"会员价\" style=\"float:left;\" class=\"btn btn-default\"  onclick=\"editSkuMemberPrice(" + rowIndex + ");\" \/>");
    $(priceCell).val($("#ctl00_contentHolder_txtSalePrice").val());
    $(gradePrice).val($("#ctl00_contentHolder_txtMemberPrices").val());
    cell.append(priceCell);
    cell.append(gradePrice);
    cell.append(btnSkuMemberPrice);
    return cell;
}

function createCostPriceCell(rowIndex) {
    var cell = createCell();
    var priceCell = $(String.format("<input type=\"text\" class=\"skuItem_CostPrice form-control\" id=\"costPrice_{0}\" style=\"width:80px;\" \/>", rowIndex)); 
    $(priceCell).val($("#ctl00_contentHolder_txtCostPrice").val());
    cell.append(priceCell);
    if (rootPath == "") {
        var priceCellspan = $(String.format("<span class=\"skuSpan4Show\"  style=\"width:100%;display:none;\" id=\"costPricespan_{0}\" />", rowIndex));
        cell.append(priceCellspan);
    }
    return cell;
}

function createQtyCell(rowIndex) {
    var cell = createCell();
    var quantityCell = $(String.format("<input type=\"text\" class=\"skuItem_Qty form-control\" id=\"qty_{0}\" style=\"width:50px;\" \/>", rowIndex));
    $(quantityCell).val($("#ctl00_contentHolder_txtStock").val());
    cell.append(quantityCell);
    if (rootPath == "") {
        var quantityCellSpan = $(String.format("<span span class=\"skuSpan4Show\"  style=\"width:100%;display:none;\"  id=\"qtyspan_{0}\" />", rowIndex));
        cell.append(quantityCellSpan);
    }
    return cell;
}

function createWarningQtyCell(rowIndex) {
    var cell = createCell();
    var quantityCell = $(String.format("<input type=\"text\" class=\"skuItem_WarningQty form-control\" id=\"warningqty_{0}\" style=\"width:50px;\" \/>", rowIndex));
    $(quantityCell).val($("#ctl00_contentHolder_txtWarningStock").val());
    cell.append(quantityCell);
    if (rootPath == "") {
        var quantityCellspan = $(String.format("<span class=\"skuSpan4Show\"  style=\"width:100%;display:none;\" id=\"warningqtyspan_{0}\"/>", rowIndex));
        cell.append(quantityCellspan);
    }
    return cell;
}

function createWeightCell(rowIndex) {
    var cell = createCell();
    var weightCell = $(String.format("<input type=\"text\" class=\"skuItem_Weight form-control\" id=\"weight_{0}\" style=\"width:50px;\" \/>", rowIndex));
    $(weightCell).val($("#ctl00_contentHolder_txtWeight").val());
    cell.append(weightCell);

    if (rootPath == "") {
        var weightCellspan = $(String.format("<span class=\"skuSpan4Show\"  style=\"width:100%;display:none;\" id=\"weightspan_{0}\"></span>", rowIndex));
        cell.append(weightCellspan);
    }
    return cell;
}

function createSkuCodeCell(rowIndex) {
    var cell = createCell();
    var skuCodeCell = $(String.format("<input type=\"text\" class=\"skuItem_SkuCode form-control\" id=\"skuCode_{0}\" style=\"width:100px;\"  onblur=\"isExistSkuCode(this)\" \/>", rowIndex));
    if ($("#ctl00_contentHolder_txtSku").val().length > 0) $(skuCodeCell).val($("#ctl00_contentHolder_txtSku").val() + "-" + rowIndex);
    cell.append(skuCodeCell);
    return cell;
}

function createActionCell(rowIndex) {
    var cell = createCell();
    var actionCell = $(String.format("<a style=\"float:left;width:100%;text-algin:center\" href=\"javascript:;\" onclick=\"removeSku({0});\" id=\"deleSku_{0}\"><span style=\"float:none;color:red\" class=\"glyphicon glyphicon-trash\"><\/span><\/a>", rowIndex));
    cell.append(actionCell);
    return cell;
}

function createCell(isHide) {
    if (isHide)
        return $("<td align=\"center\" style='display:none;'><\/td>");
    return $("<td align=\"center\"><\/td>");
}

function removeSku(rowIndex) {
    if (!confirm("规格数据删除以后不能恢复，确定要删除吗？"))
        return;

    $("#sku_" + rowIndex).remove();
    htSkuItems.remove(rowIndex);
    HasSelectedImgAttribute();
}

/* 
开启规格以后，需要把隐藏的有客户端验证的控件取消验证，关闭规格后，又要重新开启验证
原理：每个需要验证的控件在initValid中都会被追加一个ValidateGroup属性，如果开发人员没有手工指定属性值，
则属性值默认为：“default”，执行PageIsValid()开始验证时，脚本会使用属性筛选器通过ValidateGroup属性筛选出
需要执行验证的控件，然后分别对每个控件执行验证。所以，如果要取消某个控件不进行客户端验证，只要
把这个控件的ValidateGroup属性删掉，需要再次验证时，再添加上去即可。
*/

// 取消需隐藏控件的客户端验证
function cancelValid() {
    $("#ctl00_contentHolder_txtSalePrice").removeAttr("ValidateGroup");
    $("#ctl00_contentHolder_txtCostPrice").removeAttr("ValidateGroup");
    $("#ctl00_contentHolder_txtStock").removeAttr("ValidateGroup");
    $("#ctl00_contentHolder_txtWarningStock").removeAttr("ValidateGroup");
    $("#ctl00_contentHolder_txtWeight").removeAttr("ValidateGroup");
}

// 重新绑定需隐藏控件的客户端验证
function reBindValid() {
    $("#ctl00_contentHolder_txtSalePrice").attr("ValidateGroup", "default");
    $("#ctl00_contentHolder_txtCostPrice").attr("ValidateGroup", "default");
    $("#ctl00_contentHolder_txtStock").attr("ValidateGroup", "default");
    $("#ctl00_contentHolder_txtWarningStock").attr("ValidateGroup", "default");
    $("#ctl00_contentHolder_txtWeight").attr("ValidateGroup", "default");
}

function doSubmit() {
    // 1.先执行jquery客户端验证检查其他表单项
    if (!PageIsValid())
        return false;

    // 2.如果开启了规格，则做以下检查
    if (skuEnabled) {
        if (skuCounts <= 0) {
            ShowMsg("请至少保留一个自定义规格项或者关闭规格。");
            return false;
        }
        // 商品规格数量需大于等于2
        if ($(".SpecificationTr").length < 1) {
            ShowMsg("开启规格以后，您至少需要增加1个商品规格！");
            return false;
        }

        // 检查有无规格值为空的情况
        if ($(".specdefault").length > 0) {
            ShowMsg("您需要为每一个规格项选择一个值！");
            return false;
        }

        // 检查规格字段输入数据的有效性
        if (!checkSkuCode()) return false;
        if (!checkSalePrice()) return false;
        if (!checkCostPrice()) return false;
        if (!checkQty()) return false;
        if (!checkWarningQty()) return false;
        if (!checkWeight()) return false;
    }

    // 收集扩展属性和规格数据
    loadAttributes();
    loadSkus();

    return true;
}

function loadAttributes() {
    var attributesXml = "<xml><attributes>";

    $.each($(".attributeItem"), function (i, att) {
        var attributeId = $(att).attr("attributeId");
        var usageMode = $(att).attr("usageMode");
        var itemXml = String.format("<item attributeId=\"{0}\" usageMode=\"{1}\">", attributeId, usageMode);

        if (usageMode == "0") {
            //多选属性
            $.each($("input[name='vallist" + attributeId + "']:checked"), function () {
                itemXml += String.format("<attValue valueId=\"{0}\" \/>", $(this).attr("valueId"));
            });
        }
        else {
            itemXml += String.format("<attValue valueId=\"{0}\" \/>", $("#attribute" + attributeId).val());
        }

        itemXml += "<\/item>";
        attributesXml += itemXml;
    });

    attributesXml += "<\/attributes><\/xml>";
    $("#ctl00_contentHolder_txtAttributes").val(attributesXml);
}

function loadSkus() {
    var skusXml = "<xml><productSkus>";
    $.each($(".SpecificationTr"), function (i, valitem) {
        var rowIndex = $(valitem).attr("rowindex");
        var skuCode = $("#skuCode_" + rowIndex).val();
        var salePrice = $("#salePrice_" + rowIndex) ? $("#salePrice_" + rowIndex).val() : 0;
        var costPrice = $("#costPrice_" + rowIndex).val() ;
        var qty = $("#qty_" + rowIndex).val();
        var warningQty = $("#warningqty_" + rowIndex).val();
        var weight = $("#weight_" + rowIndex).val() != undefined ? $("#weight_" + rowIndex).val() : "0";
        var itemXml = String.format("<item skuCode=\"{0}\" salePrice=\"{1}\" costPrice=\"{2}\" qty=\"{3}\" warningQty = \"{4}\" weight=\"{5}\">", skuCode, salePrice, costPrice, qty, warningQty, weight);
        itemXml += "<skuFields>";
        for (i = 0; i < cellFields.length; i++) {
            itemXml += String.format("<sku attributeId=\"{0}\" valueId=\"{1}\" \/>", cellFields[i], $(String.format("#skuDisplay_{0}_{1}", rowIndex, cellFields[i])).attr("valueId"));
        }
        itemXml += "<\/skuFields>";

        // 获取每个规格的会员价
        if (rootPath==""&&$("#gradeSalePrice_" + rowIndex).val().length > 0) {
            itemXml += "<memberPrices>";
            var xml;
            if ($.browser.msie) {
                xml = new ActiveXObject("Microsoft.XMLDOM");
                xml.async = false;
                xml.loadXML($("#gradeSalePrice_" + rowIndex).val());
            }
            else {
                xml = new DOMParser().parseFromString($("#gradeSalePrice_" + rowIndex).val(), "text/xml");
            }

            $.each($(xml).find("grande"), function () {
                itemXml += String.format("<memberGrande id=\"{0}\" price=\"{1}\" />", $(this).attr("id"), $(this).attr("price"));
            });
            itemXml += "<\/memberPrices>";
        }
        itemXml += "<\/item>";
        skusXml += itemXml;
    });
    skusXml += "<\/productSkus><\/xml>";
    $("#ctl00_contentHolder_txtSkus").val(skusXml);
}

function checkSkuCode() {
    var validated = true;
    $.each($(".skuItem_SkuCode"), function () {
        if ($(this).val().length > 50) {
            ShowMsg("商品规格货号的长度不能超过50个字符！");
            $(this).focus();
            validated = false;
            return false;
        }
    });

    return validated;
}

function checkSalePrice() {
    if(rootPath=="")
        return checkPrice($(".skuItem_SalePrice"), true, "一口价");
    return true;
}

function checkCostPrice() {
    return checkPrice($(".skuItem_CostPrice"), false, "成本价");
}

function checkQty() {
    return checkNumber($(".skuItem_Qty"), true, "库存");
}

function checkWarningQty() {
    return checkNumber($(".skuItem_Qty"), true, "警戒库存");
}

function checkWeight() {
    return checkPrice($(".skuItem_Weight"), true, "重量/体积");
}

function checkPrice(inputItems, required, priceName) {
    var validated = true;
    var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");

    $.each(inputItems, function () {
        var val = $(this).val();

        // 检查必填项是否填了
        if (required && val.length == 0) {
            ShowMsg(String.format("商品规格的{0}为必填项！", priceName));
            $(this).focus();
            validated = false;
            return false;
        }

        if (val.length > 0) {
            // 检查输入的是否是有效的金额
            if (!exp.test(val)) {
                ShowMsg(String.format("商品规格的{0}输入有误！", priceName));
                $(this).focus();
                validated = false;
                return false;
            }

            // 检查金额是否超过了系统范围
            var num = parseFloat(val);
            if (num==0) {
                ShowMsg(String.format("商品规格的{0}不能为0，请重新设置！", priceName));
                $(this).focus();
                validated = false;
                return false;
            }
            if (!((num >= 0.01) && (num <= 10000000))) {
                ShowMsg(String.format("商品规格的{0}超出了系统表示范围！", priceName));
                $(this).focus();
                validated = false;
                return false;
            }
        }
    });

    return validated;
}

function checkNumber(inputItems, required, numberName) {
    var validated = true;
    var exp = new RegExp("^-?[0-9]\\d*$", "i");

    $.each(inputItems, function () {
        var val = $(this).val();

        // 检查必填项是否填了
        if (required && val.length == 0) {
            ShowMsg(String.format("商品规格的{0}为必填项！", numberName));
            $(this).focus();
            validated = false;
            return false;
        }

        if (val.length > 0) {
            // 检查输入的是否是有效的数字
            if (!exp.test(val)) {
                ShowMsg(String.format("商品规格的{0}输入有误！", numberName));
                $(this).focus();
                validated = false;
                return false;
            }

            // 检查数字是否超过了系统范围
            var num = parseFloat(val);
            if (!((num >= 0) && (num <= 9999999))) {
                ShowMsg(String.format("商品规格的{0}超出了系统表示范围！", numberName));
                $(this).focus();
                validated = false;
                return false;
            }
        }
    });
    return validated;
}


function HasSelectedImgAttribute() {
    var attributeId = uploadSKUImg.getAttribute("attrId");
    if (attributeId != "0") {
        var chooseCount = $('#skuTableHolder div[id^="skuDisplay_"][id$="' + attributeId + '"]').length;
        uploadSKUImg.style.display = chooseCount > 0 ? "" : "none";
    }
}


//判断货号重复
function isExistSkuCode(obj) {
    var productId = "";
    if ($("#ctl00_contentHolder_hidProductId")) {
        productId = $("#ctl00_contentHolder_hidProductId").val();
    }
    var skuCode = $(obj).val();
    if (skuCode == "") {
        return false;
    }
    var postUrl = rootPath+"/admin/product/addproduct.aspx?isCallback=true&action=checkSkuCode&timestamp=";
    postUrl += new Date().getTime() + "&skuCode=" + skuCode + "&productId=" + productId;
    $.ajax({
        url: postUrl,
        type: 'GET', dataType: 'text', timeout: 10000,
        async: false,
        success: function (resultData) {
            if (resultData == "True") {
                ShowMsg("货号重复，请重新输入！");
                $(obj).val('');
                $(obj).focus();
                return false;
            }
            else {
                return true;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowMsg(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus + "-" + postUrl);
            $(obj).val('');
            $(obj).focus();
            return false;
        },
    });
}

//判断商家编码重复
function isExistProductCode(obj) {
    var productId = "";
    if ($("#ctl00_contentHolder_hidProductId")) {
        productId = $("#ctl00_contentHolder_hidProductId").val();
    }
    var productCode = $(obj).val();
    if (productCode == "") {
        return false;
    }
    var postUrl = rootPath+"/admin/product/addproduct.aspx?isCallback=true&action=checkProductCode&timestamp=";
    postUrl += new Date().getTime() + "&productCode=" + productCode + "&productId=" + productId;
    $.ajax({
        url: postUrl,
        type: 'GET', dataType: 'text', timeout: 10000,
        async: false,
        success: function (resultData) {
            if (resultData == "True") {
                ShowMsg("商家编码重复，请重新输入！");
                $(obj).val('');
                $(obj).focus();
                return false;
            }
            else {
                return true;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowMsg(XMLHttpRequest.status + "-" + XMLHttpRequest.readyState + "-" + textStatus + "-" + postUrl);
            $(obj).val('');
            $(obj).focus();
            return false;
        },
    });
}

function clearSku() {
    //$("tr[id^='sku_']").remove();
    $("input[type='checkbox'][name^='cp_']").iCheck("uncheck");
    //htSkuItems.clear();
    //HasSelectedImgAttribute();
}

function BatchFillValue() {
    var batchMarketPrice = $("#txtBatchMarketPrice").val();
    var batchCostPrice = $("#txtBatchCostPrice").val();
    var batchWeight = $("#txtBatchWeight").val();
    var batchStore = $("#txtBatchStore").val();
    var batchLimitStore = $("#txtBatchLimitStore").val();

    var amountReg = /^[0-9]*[1-9][0-9]*$/;
    var moneyReg = /^\d+(\.\d{1,2})?$/;
    if (batchMarketPrice != "" && !moneyReg.test(batchMarketPrice)) {
        ShowMsg("请填写正确的一口价!");
        return;
    }
    if (batchCostPrice != "" && !moneyReg.test(batchCostPrice)) {
        if (rootPath == "")
            ShowMsg("请填写正确的成本价!");
        else
            ShowMsg("请填写正确的供货价!");
        return;
    }
    if (batchWeight != "" && !moneyReg.test(batchWeight)) {
        ShowMsg("请填写正确的重量/体积!");
        return;
    }
    if (batchStore != "" && !amountReg.test(batchStore)) {
        ShowMsg("请填写正确的库存!");
        return;
    }
    if (batchLimitStore != "" && !amountReg.test(batchLimitStore)) {
        ShowMsg("请填写正确的警戒库存!");
        return;
    }
    if (batchMarketPrice != "" && batchMarketPrice != null && batchMarketPrice != undefined) {
        $("input[id^='salePrice_']").val(batchMarketPrice);
    }
    if (batchCostPrice != "" && batchCostPrice != null && batchCostPrice != undefined) {
        $("input[id^='costPrice_']").val(batchCostPrice);

    }
    if (batchWeight != "" && batchWeight != null && batchWeight != undefined) {
        $("input[id^='weight_']").val(batchWeight);
    }
    if (batchStore != "" && batchStore != null && batchStore != undefined) {
        $("input[id^='qty_']").val(batchStore);
    }
    if (batchLimitStore != "" && batchLimitStore != null && batchLimitStore != undefined) {
        $("input[id^='warningqty_']").val(batchLimitStore);
    }
}



// 获取要生成哪些规格
function getSkuFields() {
    var currentSkuFields = new Array();
    var removeSkuIds = new Array();
    var removeIndex = 0;
    var currentSkuIndex = 0;
    var addSkuIdsIndex = 0;
    var addSkuIds = new Array();
    for (i = 0; i < bindCellFields.length; i++) {
        var skuItems = $(String.format("input[type='checkbox'][name='cp_{0}']:checked", bindCellFields[i]));
        if (skuItems.length == 0) {
            removeSkuIds[removeIndex] = bindCellFields[i];
            removeIndex++;
            continue;
        }
        if ($.inArray(bindCellFields[i], cellFields) < 0) {
            addSkuIds[addSkuIdsIndex] = bindCellFields[i];
            addSkuIdsIndex++;
        }
        var skuStr = "({";
        skuStr += String.format("\"Name\":\"{0}\",", htSkuFields.get(bindCellFields[i]).Name);
        skuStr += String.format("\"AttributeId\":\"{0}\",", bindCellFields[i]);

        var skuValueStr = "";
        $.each(skuItems, function (itemIndex, skuItem) {
            skuValueStr += "{" + String.format("\"ValueId\":\"{0}\",\"ValueStr\":\"{1}\"", $(skuItem).attr("valueId"), encodeURIComponent($(skuItem).attr("valueStr"))) + "},";
        });
        if (skuValueStr != "")
            skuValueStr = skuValueStr.substring(0, skuValueStr.length - 1);
        skuStr += String.format("\"SKUValues\":[{0}]", skuValueStr);
        skuStr += "})"
        currentSkuFields[currentSkuIndex] = eval(skuStr);
        currentSkuIndex++;
    }
    if (removeSkuIds.length == bindCellFields.length) {
        clearSku();
        closeSku();
        CloseDiv('skuValueBox');
        return;
    }
    else {
        for (var k = 0; k < addSkuIds.length; k++) {
            addSkuField(addSkuIds[k], htSkuFields.get(addSkuIds[k]).Name, false);
        }
        for (var j = 0; j < removeSkuIds.length; j++) {
            if ($.inArray(removeSkuIds[j], cellFields) > -1) {
                removeSkuField(removeSkuIds[j], htSkuFields.get(removeSkuIds[j]).Name, false);
            }
        }
    }
    return currentSkuFields;
}
// 生成部分规格
function generateSku() {
    $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
    if (!skuEnabled) {
        enableSku();
    }
    var valuationMethod = getValuationMethod();
    if (valuationMethod != "") {
        if (valuationMethod == "2") {
            if (skuEnabled)
                $("#weightRow").hide();
            else
                $("#weightRow").show();
            $("#volumeRow").hide();
        }
        else if (valuationMethod == "3") {
            if (skuEnabled)
                $("#volumeRow").hide();
            else
                $("#volumeRow").show();
            $("#weightRow").hide();
        }
        else {
            $("#volumeRow").hide();
            $("#weightRow").hide();
        }
    }
    else {
        $("#volumeRow").hide();
        $("#weightRow").hide();
    }

    var dataRows = $(".SpecificationTr");
    var currentSkuFields = getSkuFields();
    var skuValues = currentSkuFields[0].SKUValues;
    var skuArray = new Array(skuValues.length);

    $.each(skuValues, function (i, skuValue) {
        skuArray[i] = new Array(1);
        skuArray[i][0] = skuValue;
    });
    for (index = 1; index < currentSkuFields.length; index++) {
        skuValues = currentSkuFields[index].SKUValues;
        var tmpArray = new Array(skuArray.length * skuValues.length);
        var rowCounter = 0;

        for (sindex = 0; sindex < skuValues.length; sindex++) {
            for (cindex = 0; cindex < skuArray.length; cindex++) {
                tmpArray[rowCounter] = new Array(index + 1);
                for (rindex = 0; rindex < (index + 1) ; rindex++) {
                    if (rindex == index)
                        tmpArray[rowCounter][rindex] = skuValues[sindex];
                    else {
                        tmpArray[rowCounter][rindex] = skuArray[cindex][rindex];
                    }
                }
                rowCounter++;
            }
        }

        skuArray = tmpArray;
    }


    $.each(dataRows, function () { $(this).remove(); });
    for (i = 0; i < skuArray.length; i++) {
        var rowIndex = addItem();
        for (j = 0; j < currentSkuFields.length; j++) {
            var skuItem = skuArray[i][j];
            selectSkuValue(rowIndex, currentSkuFields[j].AttributeId, skuItem.ValueId, decode_Hishop(skuItem.ValueStr));
        }
    }
    HasSelectedImgAttribute();
    CloseDiv('skuValueBox');
}



function removeORAddWeightField(valuationMethod) {
    if (skuEnabled) {
        var noWeightCell = false;
        var ncell = $(".SpecificationTable td[id='weightOrVolumeField']").html();
        if (ncell == undefined || ncell == null) {
            noWeightCell = true;
        }
        if (noWeightCell) {
            if (valuationMethod != "2" && valuationMethod != "3") {
                return;
            }
            $("#batchWeight").show();
            var fieldCell;
            if (valuationMethod == "2") {
                fieldCell = $("<td align=\"center\" id='weightOrVolumeField'><em >*<\/em>重量(KG)<\/td>");
                $("#txtBatchWeight").attr("placeholder", "重量");
            }
            else if (valuationMethod == "3") {
                fieldCell = $("<td align=\"center\" id='weightOrVolumeField'><em >*<\/em>体积<\/td>");
                $("#txtBatchWeight").attr("placeholder", "体积");
            }
            var storefieldCell = $(".SpecificationTable td[id='storeField']");
            var cellIndex = storefieldCell.parent("tr").children().index(storefieldCell);
            $(fieldCell).insertBefore($("td:eq(" + cellIndex + ")", $(tableHeader)));
            $.each($(".SpecificationTr"), function () {
                var skuCell = createWeightCell($(this).attr("rowindex"));
                $(skuCell).insertBefore($("td:eq(" + cellIndex + ")", $(this)));
            });
        }
        else {
            if (valuationMethod == "2") {
                $("#weightOrVolumeField").html("重量(KG)");
                $("#txtBatchWeight").attr("placeholder", "重量");
            }
            else if (valuationMethod == "3") {
                $("#weightOrVolumeField").html("体积");
                $("#txtBatchWeight").attr("placeholder", "体积");
            }
            else {
                var fieldCell = $(".SpecificationTable td[id='weightOrVolumeField']");
                var cellIndex = fieldCell.parent("tr").children().index(fieldCell);
                $(".SpecificationTable tr").each(function () {
                    $("td:eq(" + cellIndex + ")", $(this)).remove();
                });
                $("#batchWeight").hide();
            }
        }
    }
}



