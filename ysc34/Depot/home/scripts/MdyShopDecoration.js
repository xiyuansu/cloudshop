
$(function () {
    BindProductHtml('Load');
    getProductPager(1);
    currentPage = 1;
});

function ShowAddMarketingImageDiv() {
    DialogFrameClose("/Depot/home/SearchMarketingImage", "选择营销图片", null, null, function (e) { });
}

function ShowAddProductDiv() {
    DialogFrameClose("/Depot/home/SearchProduct?ProductType=-1&ProductIds=" + getSelectedProductIds(), "选择商品", null, null, function (e) { CloseFrameWindow(); });
}
function CloseFrameWindow() {
    var arr = $("#ctl00_contentHolder_hidSelectProducts").val();
    if (arr == "") return;
    var allProducts = $("#ctl00_contentHolder_hidAllSelectedProducts");
    if (allProducts.val() != "") {
        allProducts.val(allProducts.val() + ",,," + arr);
    }
    else {
        allProducts.val(arr);
    }
    BindProductHtml(arr);
    getProductPager(1);
    currentPage = 1;
    $("#ctl00_contentHolder_hidSelectProducts").val('');
}
function BindProductHtml(arr) {
    if (arr == "Load")
    {
       arr = $("#ctl00_contentHolder_hidSelectProducts").val();
    }
    if (arr == "") return;
    var items = arr.split(",,,");
    var content = "";
    for (var i = 0; i < items.length; i++) {
        var item = items[i];
        var record = item.split("|||");
        content += String.format("<tr name='appendlist'><td>{0}</td><td><input type='hidden' value='{1}' id='hidProduct_{2}' /><span class='icon_close' onclick='onDelPrduct(this,\"{0}\");'></span></td></tr>", record[1], record[0], record[0]);
    }
    $("#addlist tr:eq(0)").after(content);
    var listCount = $("[name='appendlist']").length;  //总记录数
    $("#ctl00_contentHolder_lblSelectCount").html(listCount);
}

function onDelPrduct(obj, name) {
    if (confirm("确定要删除商品【" + name + "】吗？")) {
        $(obj).parent().parent().remove();
        $("#ctl00_contentHolder_lblSelectCount").html($("[name='appendlist']").length);
        getProductPager(1);
        currentPage = 1;
    }
}
function getSelectedProductIds() {
    var productIds = "";
    $("#addlist input[id^='hidProduct_']").each(function (i, obj) {
        if (productIds != "")
            productIds += "," + $(obj).val();
        else
            productIds += $(obj).val();
    });
    return productIds;
}
function setChooseProducts() {
    $("#ctl00_contentHolder_hidProductIds").val(getSelectedProductIds());
}
var pageSize = 5;
var currentPage = 1;
var pageCount = 1;
var listCount;
function getProductPager(pageIndex) {
    var listCount = $("[name='appendlist']").length;  //总记录数
    if (listCount <= 5) {
        $("[name='appendlist']").show();
        $("#divPage").hide();
        return;
    }
    $("#divPage").show();
    pageCount = Math.ceil(listCount / pageSize);  //总页数
    $("#spanPageCount").html(pageIndex + "/" + pageCount);
    var startIndex = pageSize * (pageIndex - 1) + 1;
    var endIndex = startIndex + pageSize;
    $("[name='appendlist']").hide();
    $("[name='appendlist']").slice(startIndex - 1, endIndex - 1).show();
}

function goToPrePage() {
    if (currentPage == 1) return;
    getProductPager(currentPage - 1);
    currentPage = currentPage - 1;
}
function goToNextPage() {
    if (currentPage == pageCount) return;
    getProductPager(currentPage + 1);
    currentPage = currentPage + 1;
}

function doSubmit() {
    var FloorId = getQueryString("FloorId");;
    var FloorName = $("#ctl00_contentHolder_txtFloorName").val();
    var ProductIds = getSelectedProductIds();
    var ImageId = $("#ctl00_contentHolder_spGLYXTPID").html();
    var displaySequence = parseInt($("#ctl00_contentHolder_txtDisplaySequence").val());
    if (isNaN(displaySequence)) {
        ShowMsg("请输入正确的楼层排序", false);
        return;
    }
    if (FloorName == "") {
        ShowMsg("楼层名称不能为空!", false);
        return;
    }
    if (ProductIds == "") {
        ShowMsg("请选择关联商品!", false);
        return;
    }
    if (ImageId == "") {
        ShowMsg("请选择关联营销图片!", false);
        return;
    }

    $.post('/Depot/home/ashx/ShopDecoration.ashx', { flag: "Mdy", FloorId: FloorId, FloorName: FloorName, ImageId: ImageId, ProductIds: ProductIds, DisplaySequence: displaySequence }, function (json) {
        var obj = eval('(' + json + ')');
        if (obj.Result != undefined) {
            if (obj.Result.Success.Status == true) {
                ShowMsg("修改成功", true);
                window.location.href = "/Depot/home/ShopDecoration.aspx";
            }
            else {
                ShowMsg(obj.Result.Success.Msg, false);
                $("#divLoading").hide();
            }
        }
        else {
            ShowMsg(obj.ErrorResponse.ErrorMsg,false);
        }
    }
        );
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
