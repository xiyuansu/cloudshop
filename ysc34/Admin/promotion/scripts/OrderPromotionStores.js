var storeIdArry = new Array();
$(function () {
    if (artDialog.open.origin.restoreStoreId().length > 0) {
        storeIdArry = artDialog.open.origin.restoreStoreId();//当修改的时候，此处可获取当前选择的的门店ID数组
    }
    //搜索
    $("#btnSearch").on("click", function () {
        ShowListData(1, true);
    });
    ShowListData(1, true);
});
function checkIt(obj) {
    var jObj = $(obj);
    var storeId = parseInt(jObj.val());
    var index = jQuery.inArray(storeId, storeIdArry);
    if (jObj.is(':checked'))//选中
    {
        if (index < 0)//不存在
            storeIdArry.push(storeId);
    } else { //取消选择   
        if ($("#cbxAllEnable").is(':checked'))//选中
        {
            $("#cbxAllEnable").attr("checked", false);
            $("#CheckBoxGroup").attr("checked", false);
        }
        if (index > -1)//存在
            storeIdArry.splice(index, 1);
    }
    $("#spancheckNum").html(storeIdArry.length);
}

function getProductId() {
    var productId = GetQueryString("productId");
    if (productId <= 0) {
        productId = parseInt($("#ctl00_contentHolder_hidProductId").val());
    }
    return productId;
}
var curpagesize = 10, curpageindex = 1, total = 0, tagId;
function ShowListData(page, initpagination) {
    var productId = getProductId();
    page = page || 1;
    if (typeof (initpagination) == "undefined") { initpagination = true; }

    var urldata = {
        pageIndex: page, pageSize: curpagesize, action: "getstores", key: $("#txtStoresName").val(), productId: productId, regionId: $("#regionSelectorValue").val(), tagId: $("#ctl00_contentHolder_ddlTags").val(), activityId: GetQueryString("ActivityId")
    }
    $.ajax({
        type: "GET",
        url: "/Admin/promotion/ashx/OrderPromotions.ashx",
        data: urldata,
        dataType: "json",
        success: function (data) {
            var isnodata = true;
            var isshowpage = true;
            var databox = $("#datashow");
            databox.empty();
            if (data) {
                total = data.Total;
                if (total == 0) {
                    isshowpage = false;
                }
                if (data.Models.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));
                    if (storeIdArry.length <= 0) {//当新增的时候，已选了门店后重新打开选择门店弹框，初始化已选门店选中状态
                        var storeIdArryTemp = $(artDialog.open.origin.document.getElementById("ctl00_contentHolder_hidStoreIds")).val().split(',');
                        for (var i = 0; i < storeIdArryTemp.length; i++) {
                            var storeId = parseInt(storeIdArryTemp[i]);
                            if (storeIdArry.indexOf(storeId) == -1 && storeId >= 0) {
                                storeIdArry.push(storeId);
                            }
                        }
                    }
                    processPageCbx();
                } else {
                    initpagination = true;  //强制更新分页组件
                }
            }
            curpageindex = page;
            if (initpagination) {
                //初始分页组件
                if (isshowpage) {
                    $("#showpager").HiPaginator({
                        totalCounts: total,
                        pageSize: curpagesize,
                        currentPage: 1,
                        prev: '<a href="javascript:;" class="page-prev">上一页</a>',
                        next: '<a href="javascript:;" class="page-next">下一页</a>',
                        page: '<a href="javascript:;">{{page}}</a>',
                        first: '',
                        last: '',
                        visiblePages: 3,
                        disableClass: 'hide',
                        activeClass: 'page-cur',
                        onPageChange: function (pageindex, type) {
                            //分页回调
                            curpagesize = $("#showpager").HiPaginator("getPagesize");
                            if (type != "init") {
                                ShowListData(pageindex, false);
                            }
                        }
                    });
                } else {
                    if ($("#showpager").HiPaginator("isExist")) {
                        $("#showpager").HiPaginator("destroy");
                    }
                }
            } databox.addClass("cdstoreH");
            $('.cdstoreH').parent().parent('.datalist').css({ 'height': '300px', 'overflow-y': 'scroll', 'clear': 'both' });
            $('.cdstoreH').closest('.dataarea').prev('.searcharea').css({ 'margin-bottom': '0px', 'padding-top': '0px' })
        },
        error: function () {
            ShowMsg("系统内部异常", false);
        }
    });
}

template.helper("existsStorId", function (StoreId) {
    return storeIdArry.indexOf(StoreId) > -1 ? true : false;
});
function validatorForm() {
    return true;
}
//全部可选门店
function getAllStoreEnable() {
    storeIdArry = new Array();
    if ($("#cbxAllEnable").is(':checked'))//选中
    {
        var urldata = {
            action: "getenablestores", key: $("#txtStoresName").val(), productId: GetQueryString("productId"), regionId: $("#regionSelectorValue").val(), tagId: $("#ctl00_contentHolder_ddlTags").val(), activityId: GetQueryString("ActivityId")
        }
        $.ajax({
            type: "GET",
            url: "/admin/promotion/ashx/OrderPromotions.ashx",
            data: urldata,
            dataType: "json",
            success: function (data) {
                if (data) {
                    storeIdArry = data;
                    restoreStoreId();
                    //循环选择本页面
                    processPageCbx();

                }
            }
        });
    } else { //取消选择  
        processPageCbx();
    }
}
function processPageCbx() {
    var icheckd = 0;
    $("input[name='cbxStoreGroup']").each(function (i, item) {
        if (storeIdArry.indexOf(parseInt($(item).val())) > -1) {
            $(item).attr("checked", true);
            icheckd++;
        } else {
            $(item).attr("checked", false);
        }
    });
    if (icheckd > 0 && $("input[name='cbxStoreGroup']").length == icheckd)
        $("#CheckBoxGroup").attr("checked", true);
    else
        $("#CheckBoxGroup").attr("checked", false);

    $("#spancheckNum").html(storeIdArry.length);
}
//本页面全选
function checkThisPageAll() {
    if ($("#CheckBoxGroup").is(':checked'))//选中
    {
        $("input[name='cbxStoreGroup']:not(:disabled)").each(function (i, item) {
            $(item).attr("checked", true);
            checkIt(item);
        });

    } else {
        $("input[name='cbxStoreGroup']").each(function (i, item) {
            $(item).attr("checked", false);
            checkIt(item);
        })
    }
}

function restoreStoreId() {
    if (parseInt(GetQueryString("ActivityId")) > 0)//修改           
    {
        var storeIdArryTemp = $(artDialog.open.origin.document.getElementById("ctl00_contentHolder_hidStoreIds")).val().split(',');
        for (var i = 0; i < storeIdArryTemp.length; i++) {
            var storeId = parseInt(storeIdArryTemp[i]);
            if (storeIdArry.indexOf(storeId) == -1 && storeId >= 0) {
                storeIdArry.push(storeId);
            }
        }
        $(artDialog.open.origin.document.getElementById("divCount")).html(storeIdArry.length);
    }
}

//添加
function SelectStores() {
    var origin = artDialog.open.origin;

    $(origin.document.getElementById("ctl00_contentHolder_hidStoreIds")).val(storeIdArry.join());
    $(origin.document.getElementById("divCount")).html(storeIdArry.length);
    art.dialog.close();
}