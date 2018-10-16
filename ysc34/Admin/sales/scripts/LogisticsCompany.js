var  total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;


//页面初始
$(function () {
    showbox = $("#datashow");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;
    //初始数据显示
    ShowListData(showbox);
    //搜索
    $("#btnSearch").on("click", function () {
        ShowListData(showbox);
    });
});

 

//搜索参数
function initQuery(obj) {
    var companyname = $("#txtcompany").val();
    if (companyname.toString().length > 0) {
        obj.companyname = companyname;
    }
    var kuaidi100Code = $("#txtKuaidi100Code").val();
    if (kuaidi100Code.toString().length > 0) {
        obj.kuaidi100Code = kuaidi100Code;
    }
    var taobaoCode = $("#txtTaobaoCode").val();
    if (taobaoCode.toString().length > 0) {
        obj.taobaoCode = taobaoCode;
    }
    var jdCode = $("#txtJDCode").val();
    if (jdCode.toString().length > 0) {
        obj.jdCode = jdCode;
    }
    
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
        loadingobj = showLoading(showbox);
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
            databox.empty();
            DataNullHelper.hide(datanullbox);
            if (data) {
                total = data.total;
              if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));          
                }
                if (isnodata) {
                    //total = 0;
                    DataNullHelper.show(datanullbox);
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
    /*复位*/
    try { var chkall = $("#checkall"); if (chkall) { if (chkall.iCheck) { chkall.iCheck('uncheck'); } chkall.prop("checked", false); } } catch (e) { }
    
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


function initQuerys(obj) {
    var companyname = $("#txtAddCmpName").val();
    if (companyname.toString().length > 0) {
        obj.companyname = companyname;
    }
    var kuaidi100Code = $("#txtAddKuaidi100Code").val();
    if (kuaidi100Code.toString().length > 0) {
        obj.kuaidi100Code = kuaidi100Code;
    }
    var taobaoCode = $("#txtAddTaobaoCode").val();
    if (taobaoCode.toString().length > 0) {
        obj.taobaoCode = taobaoCode;
    }
    var jdCode = $("#txtAddJDCode").val();
    if (jdCode.toString().length > 0) {
        obj.jdCode = jdCode;
    }
    var hdcomputers = $("#hdcomputers").val();
    if (hdcomputers.toString().length > 0) {
        obj.hdcomputers = hdcomputers;
    }
    return obj;
}
function AddAndUpdate() {
    
    var urldata = {
        action: "addandupdate"
    }
    initQuerys(urldata);
    if (!checkQuery(urldata)) {
        return;
    }
        var loading;
        try {
            loading = showCommonLoading();
        } catch (e) { }
        $.ajax({
            type: "post",
            url: dataurl,
            data: urldata,
            dataType: "json",
            success: function (data) {
                try {
                    loading.close();
                } catch (e) { }
                if (data.success) {
                    setArryText('hdcomputers',"");
                    setArryText('txtAddCmpName', "");
                    setArryText('txtAddKuaidi100Code', "");
                    setArryText('txtAddTaobaoCode', "");
                    setArryText('txtAddJDCode', "");
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
function EditCloseStaute(CloseStaute, LogisticsName) {
    $.ajax({
        url: "../../API/LogisticsCompany.ashx",
        type: 'POST', dataType: 'json', timeout: 10000,
        data: {
            action: "UpdateCloseStaute",
            CloseStaute: CloseStaute,
            LogisticsName: LogisticsName
        },
        async: true,
        success: function (resultData) {
            if (resultData.IsSuccess == "1") {
                ShowMsg("操作成功",true);
                ReloadPageData();
            } else {
                ShowMsg("修改失败");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowMsg("修改失败");
        }

    });
}

function ShowAddSKUValueDiv(opers, strname, strKuaidi100Code, strTaobaoCode, strJDCode, CloseStatus) {

    arrytext = null;
    setArryText('hdcomputers', strname);
    setArryText('txtAddCmpName', strname);
    setArryText('txtAddKuaidi100Code', strKuaidi100Code);
    setArryText('txtAddTaobaoCode', strTaobaoCode);
    setArryText('txtAddJDCode', strJDCode);

    var Cmptitle = "添加物流公司";
    if (strname != "") {
        $("#spMsg").html("快递鸟Code是物流跟踪所需要的，淘宝Code是同步淘宝发货所需要的，请不要随意修改！");
        Cmptitle = "编辑" + strname + "物流公司";
    } else {
        $("#spMsg").html("快递鸟Code是物流跟踪所需要的，淘宝Code是同步淘宝发货所需要的,请填写正确！");
    }

    DialogShow(Cmptitle, 'expresscmp', 'divexpresscomputers', 'btnCreateValue');
}



function ShowEditSKUValueDiv(opers, link_obj, CloseStatus) {

    arrytext = null;
    var strname = $(link_obj).parents("tr").find("td").eq(0).text().trim();
    var strKuaidi100Code = $(link_obj).parents("tr").find("td").eq(1).text();
    var strTaobaoCode = $(link_obj).parents("tr").find("td").eq(2).text();
    var strJDCode = $(link_obj).parents("tr").find("td").eq(3).text();

    setArryText('hdcomputers', strname);
    setArryText('txtAddCmpName', strname);
    setArryText('txtAddKuaidi100Code', strKuaidi100Code);
    setArryText('txtAddTaobaoCode', strTaobaoCode);
    setArryText('txtAddJDCode', strJDCode);

    var Cmptitle = "添加物流公司";
    if (strname != "") {
        $("#spMsg").html("快递鸟Code是物流跟踪所需要的，淘宝Code是同步淘宝发货所需要的，京东Code是同步京东订单所需要的，请不要随意修改！");
        Cmptitle = "编辑物流公司";
    } else {
        $("#spMsg").html("快递鸟Code是物流跟踪所需要的，淘宝Code是同步淘宝发货所需要的,京东Code是同步京东订单所需要的，请填写正确！");
    }

    DialogShow(Cmptitle, 'expresscmp', 'divexpresscomputers', 'btnCreateValue');
}

function validatorForm() {
    var strname = $("#hdcomputers").val().replace(/\s/g, "");
    var cmpname = $("#txtAddCmpName").val().replace(/\s/g, "");
    var strKuaidi100Code = $("#txtAddKuaidi100Code").val().replace(/\s/g, "");
    var strTaobaoCode = $("#txtAddTaobaoCode").val().replace(/\s/g, "");
    var strJDCode = $("#txtAddJDCode").val().replace(/\s/g, "");

    if (cmpname == "") {
        alert("物流公司名称不允许为空!");
        return false;
    }
    if (strKuaidi100Code == "") {
        alert("快递鸟Code不允许为空！");
        return false;
    }
    if (strTaobaoCode == "") {
        alert("淘宝code不允许为空！");
        return false;
    }
    if (strJDCode == "") {
        alert("京东code不允许为空！");
        return false;
    }
    return true;
}