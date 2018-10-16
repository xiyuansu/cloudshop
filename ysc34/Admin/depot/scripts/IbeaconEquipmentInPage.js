var curpagesize = 10, curpageindex = 1, total = 0;
var showbox, dataurl, showpager,datanullbox,datanulldom;
const NO_DATA_MSG = "<div class='dataNull' id='dataNull'><img src='../images/data_null.png'><p>没有找到符合条件的数据!</p></div>";

//页面初始
$(function () {
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
datanullbox = showbox;
    
   
    //初始数据显示
    ShowListData(showbox, curpageindex, curpagesize, true);
    //搜索
    $("#btnSearch").on("click", function () {
//curpagesize = parseInt($("#pagesize_dropdown").val());
        ShowListData(showbox, 1, curpagesize, true);
    });
});

 

//搜索参数
function initQuery(obj) {
 
    var page_id = getParam("page_id");
    if (page_id.length>0) {
        obj.page_id = page_id;
    }
  
    return obj;
}
//参数检测
function checkQuery(obj) {
    var result = true;

    return result;
}

function DataRedraw(databox) {
    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red' });

}


function ShowListData(target, page, pagesize, initpagination) {
    page = page || 1;
    pagesize = pagesize || 10;
    if (typeof (initpagination) == "undefined") { initpagination = true; }
    var urldata = {
        page: page, rows: pagesize, action: "getlist"
    }
    initQuery(urldata);
    if (!checkQuery(urldata)) {
        return;
    }

    var loadingobj;
    try {
        target.empty();
        DataNullHelper.hide(datanullbox);
        loadingobj = showLoading(target);
    } catch (e) { }

    $.ajax({
        type: "GET",
        url: dataurl,
        data: urldata,
        dataType: "json",
        success: function (data) {
            var isnodata = true;
            var isshowpage = true;
            var databox = $(target);
            try {
                loadingobj.close();
            } catch (e) { }
            databox.empty();DataNullHelper.hide(datanullbox);
            if (data) {
                total = data.total;
                if (total == 0) {
                    isshowpage = false;
                }
                if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));
                    DataRedraw(databox);
                } else {
                    initpagination = true;  //强制更新分页组件
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
            try {
                loadingobj.close();
            } catch (e) { }
            ShowMsg("系统内部异常", false);
        }
    });
}




function ReloadPageData(pageindex) {
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
    ShowListData(showbox, curpageindex, curpagesize, true);
    setCheckbox();
}
 
//全选按钮关闭
function setCheckbox() {

    if ($("#checkall").attr('checked') == 'checked') {
        $("#checkall").iCheck('uncheck');
    }

}
//获取选中的ID
function GetSelectId() {
    var v_str = "";

    $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
        v_str += $(rowItem).attr("value") + ",";
    });
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

function Select()
{
    var page_id = getParam("page_id");
    DialogFrame('/admin/depot/SearchIbeaconEquipment.aspx?callback=CloseDialogAndReloadData&page_id=' + page_id, '将设备添加到页面', 800, 600, function () { ReloadPageData();});
}

function bat_Delete() {
    var ids = GetSelectId();
    if (ids.length == 0) {
        alert("参数错误");
        return;
    }
    Post_Deletes(ids);
}
function  Delete(ids) {
    Post_Deletes(ids);
}

function Post_Deletes(ids) {
    if (ids.length < 1) {
        ShowMsg("错误的编号", false);
        return;
    }
    var candel = false;
    candel = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    var page_id = getParam("page_id");
    if (candel) {
        var pdata = {
            ids: ids, action: "delete", page_id: page_id
        }
        var loading;
        try {
            loading = showCommonLoading();
        } catch (e) { }
        $.ajax({
            type: "post",
            url: dataurl,
            data: pdata,
            dataType: "json",
            success: function (data) {
                try {
                    loading.close();
                } catch (e) { }
                if (data.success) {
                    ShowMsg(data.message, true);
                    ReloadPageData();
                } else {
                    ShowMsg(data.message, false);
                }
            },
            error: function () {
                try {
                    loading.close();
                } catch (e) { }
            }
        });
    }
}



 