var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;


var canModifyStock = false;

//页面初始
$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;

    curpagesize = parseInt($("#pagesize_dropdown").val());

    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
    //每页显示数量
    $("#pagesize_dropdown").on("change", function () {
        var _t = $(this);
        curpagesize = parseInt(_t.val());
        ShowListData(showbox, curpageindex, curpagesize, false);
        showpager.HiPaginator("option", { pageSize: curpagesize });
        showpager.HiPaginator("redraw");
    });
    //搜索
    $("#btnSearch").on("click", function () {
        curpagesize = parseInt($("#pagesize_dropdown").val());
        ShowListData(showbox, 1, curpagesize, true);
    });

    //下架操作
    showbox.on("click", ".downshelf", function () {
        var _t = $(this);
        var id = _t.data("id");
        DeleteProduct(id);
    });
    $(".j_storeName").attr("title", $(".j_storeName").html());
});

//批量下架
function batDelete() {
    var proids = GetProductId();
    if (proids.length < 1) {
        ShowMsg("请选择至少一个商品！", false);
        return;
    }
    DeleteProduct(proids);
}

//搜索参数
function initQuery(obj) {
    var ProductName = $("#txtSearchText").val();
    if (ProductName.length > 0) {
        obj.ProductName = ProductName;
    }
    var StoreId = $("#hidStoreId").val();
    if (StoreId.length > 0) {
        obj.StoreId = StoreId;
    }
    var categoryId = $("#dropCategories").val();
    if (categoryId.length > 0) {
        obj.categoryId = categoryId;
    }

    return obj;
}
//全选按钮关闭
function setCheckbox() {

    if ($("#checkall").attr('checked') == 'checked') {
        $("#checkall").iCheck('uncheck');
    }

}


function ShowListData(target, page, pagesize, initpagination) {
    page = page || 1;
    pagesize = pagesize || 10;
    if (typeof (initpagination) == "undefined") { initpagination = true; }
    var urldata = {
        page: page, rows: pagesize, action: "getlist"
    }
    initQuery(urldata);
    var loadingobj = showLoading(showbox);
    target.empty();
    DataNullHelper.hide(datanullbox);

    $.ajax({
        type: "GET",
        url: dataurl,
        data: urldata,
        dataType: "json",
        success: function (data) {
            var isnodata = true;
            var isshowpage = true;
            var databox = $(target);
            loadingobj.close();
            databox.empty(); DataNullHelper.hide(datanullbox);
            if (data) {
                total = data.total;
                if (total == 0) {
                    isshowpage = false;
                }
                if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));
                    DataRedraw(databox);


                }
            }
            if (isnodata) {
                //total = 0;
                DataNullHelper.show(datanullbox);
            }
            curpageindex = page;
            if (initpagination) {
                //初始分页组件
                if (isshowpage) {
                    showpager.HiPaginator({
                        totalCounts: total,
                        pageSize: curpagesize,
                        currentPage: curpageindex,
                        prev: '<a href="javascript:;" class="page-prev">上一页</a>',
                        next: '<a href="javascript:;" class="page-next">下一页</a>',
                        page: '<a href="javascript:;">{{page}}</a>',
                        first: '',
                        last: '',
                        visiblePages: 10,
                        disableClass: 'hide',
                        activeClass: 'page-cur',
                        onPageChange: function (pageindex, type) {
                            //分页回调
                            curpagesize = $("#showpager").HiPaginator("getPagesize");
                            if (type != "init") {
                                ShowListData(showbox, pageindex, curpagesize, false);
                            }
                        }
                    });
                } else {
                    if (showpager.HiPaginator("isExist")) {
                        showpager.HiPaginator("destroy");
                    }
                }
            }
        },
        error: function () {
            loadingobj.close();
            ShowMsg("系统内部异常", false);
        }
    });
}
function DataRedraw(databox) {
    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red' });

}

function ReloadPageData(pageindex) {
    /*复位*/
    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }
    var _page = showpager;
    var hasPaginator = showpager.HiPaginator("isExist");
    if (hasPaginator) {
        var opts = _page.HiPaginator("getOption");
        curpagesize = opts.pageSize;
        curpageindex = pageindex || opts.currentPage;
    } else {
        curpagesize = 10;
        curpageindex = 1;
    }
    setCheckbox();
    ShowListData(showbox, curpageindex, curpagesize, true);
}

//商品下架
function DeleteProduct(ids) {
    var StoreId = $("#hidStoreId").val();
    if (confirm('确定要把这些商品下架吗？')) {
        $.ajax({
            type: "post",
            url: dataurl,
            data: { action: "delete", productids: ids, StoreId: StoreId },
            dataType: "json",
            success: function (data) {
                if (data) {
                    if (data.success) {
                        ShowMsg("下架操作完成", true);
                    } else {
                        ShowMsg(data.message, false);
                    }
                }
                ReloadPageData();
            },
            error: function () {
                ShowMsg("系统内部异常", false);
            }
        });
    }
}

//批量修改库存
function batchAdjustStore() {
    var productIds = GetProductId();
    if (productIds.length == 0) {
        return;
    }
    openAdjustStockPage(productIds);
}
//调整库存页面
function openAdjustStockPage(productIds) {
    OpenDialogFrame(adjustStockurl + "?ProductIds=" + productIds + "&callback=CloseDialogAndReloadData", {
        title: "调整库存", width: 880, height: 300, button: [{
            name: '关闭'
        }, {
            name: '保存',
            callback: function () {
                var iframe = this.iframe.contentWindow;
                $("#btnSaveStock", iframe.document).trigger("click");  //触发保存
                return false;  //关闭动作由callback操作
            },
            focus: true
        }],
        init: function () {
            var iframe = this.iframe.contentWindow;
            //var top = art.dialog.top;
            $("#btnSaveStock", iframe.document).hide();
        }
    });
}



//获取选中的商品ID
function GetProductId() {
    var v_str = "";

    $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
        v_str += $(rowItem).attr("value") + ",";
    });

    if (v_str.length == 0) {
        alert("请选择商品");
        return "";
    }
    return v_str.substring(0, v_str.length - 1);
}
//关闭并刷新页面数据
function CloseDialogAndReloadData(id) {
    if (id) {
        art.dialog({ id: id }).close();
    } else {
        CloseDialogFrame("", false);
    }
    ReloadPageData();
}


function ShowSuccessAndCloseReload() {
    ShowMsg("编辑成功！", true);
    CloseDialogAndReloadData();
}
