var curpagesize = 10, curpageindex = 1, total = 0;
var DEFAULT_PAGE_INDEX = 1, DEFAULT_PAGESIZE = 10;
var showbox, dataurl, showpager, datanullbox, datanulldom;


//页面初始
$(function () {
    InitParameters();
    //初始数据显示
    ShowListData(showbox, true);
});

///参数处理
function InitParameters() {
    showbox = $("#datashow");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;
}


//搜索参数
function initQuery(obj) {
    return obj;
}
//参数检测
function checkQuery(obj) {
    var result = true;

    return result;
}



function ShowListData(target) {
    var urldata = {
        action: "getlist"
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
            var databox = $(target);
            try {
                loadingobj.close();
            } catch (e) { }
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
        },
        error: function () {
            try {
                loadingobj.close();
            } catch (e) { }
            ShowMsg("系统内部异常", false);
        }
    });
}
//后续处理
function DataRedraw(databox) {
    $('[data-toggle="tooltip"]').tooltip();
    $('.icheck', databox).iCheck({ handle: 'checkbox', checkboxClass: 'icheckbox_square-red', });
}


function ReloadPageData() {    /*复位*/    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }
    ShowListData(showbox);
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

//获取选中的ID
function GetSelectId() {
    var v_str = "";
    $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
        if (rowIndex > 0) {
            v_str += ",";
        }
        v_str += $(rowItem).attr("value");
    });
    return v_str;
}

function Post_ExportExcel() {
    var urldata = {
        action: "exportexcel"
    }
    initQuery(urldata);

    var url = dataurl + "?";
    for (var item in urldata) {
        url += item + "=" + urldata[item] + "&";
    }
    url = url.substr(0, url.length - 1);
    window.open(url);
}

function Post_Delete(id) {
    if (id.length < 1) {
        ShowMsg("请选择要操作的数据", false);
        return;
    }
    var canoper = false;
    canoper = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (canoper) {
        var pdata = {
            GradeId: id, action: "delete"
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