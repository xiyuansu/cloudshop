var  total = 0;
var showbox, dataurl, showpager, datanullbox, datanulldom;


//页面初始
$(function () {
    showbox = $("#datashow");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;
    //初始数据显示
    ShowListData(showbox);
    $("#btnEditRoles").on("click", function () {
        AddFunction();
    });
     
});

 

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

    target.empty();
    DataNullHelper.hide(datanullbox);
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
            databox.empty();
            DataNullHelper.hide(datanullbox);
            try {
                loadingobj.close();
            } catch (e) { }
            if (data) {
                total = data.total;
              if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));          
                }
                if (isnodata) {
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
 

function ReloadPageData() {
    /*复位*/
    try {
        var chkall = $("#checkall");
        if (chkall) {
            if (chkall.iCheck) {
                chkall.iCheck('uncheck');
            }
            chkall.prop("checked", false);
        }
    } catch (e) { }
    
    ShowListData(showbox);   
}

 


 
function Delete(roleId) {
    var candel = false;
    candel = confirm("确定要执行该删除操作吗？删除后将不可以恢复！");
    if (candel) {
        var urldata = {
            action: "delete", roleId: roleId
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
 


var formtype = "";


//function AddNewRoles() {
//    arrytext = null;
//    formtype = "add";
//    setArryText('txtAddRoleName', "");
//    setArryText('txtRoleDesc', "");
//    DialogShow('添加部门', 'rolesetcmp', 'divaddroles', 'btnSubmitRoles');
//}


function ShowEditDiv(roleId, name, description) {
    arrytext = null;
    formtype = "edite";
    setArryText('txtRoleId', roleId);
    setArryText('txtRoleName', name);
    setArryText('txtEditRoleName', name);
    setArryText('txtEditRoleDesc', description);
    DialogShow('修改部门', 'rolesetcmp', 'EditRole', 'btnEditRoles');
}

function validatorForm() {
    //InitValidators();
    if (formtype == "add") {
        var rolename = $("#txtAddRoleName").val().replace(/\s/g, "");
        var roledes = $("#txtRoleDesc").val().replace(/\s/g, "");
        if (rolename == "" || rolename.length < 1 || rolename.length > 60) {
            alert("部门名称不能为空,长度限制在60个字符以内");
            return false;
        }
        if (roledes != "" && roledes.length > 100) {
            alert("职能说明的长度限制在100个字符以内");
            return false;
        }
    } else {
        var rolieId = $("#txtRoleIdtxtRoleId").val();
        var rolename = $("#txtEditRoleName").val();
        var roledes = $("#txtEditRoleDesc").val();
        if (rolename == "" || rolename.length < 1 || rolename.length > 60) {
            alert("部门名称不能为空,长度限制在60个字符以内");
            return false;
        }
        if (roledes != "" && roledes.length > 100) {
            alert("职能说明的长度限制在100个字符以内");
            return false;
        }
    }
    return true;
}





 
function AddFunction() {
    var RoleId = $("#txtRoleId").val();
    var RoleName = $("#txtEditRoleName").val();
    var RoleDesc = $("#txtEditRoleDesc").val();
    var urldata = {
        action: "addandupdate", RoleName: RoleName, RoleDesc: RoleDesc, RoleId: RoleId
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
                ShowMsg(data.message, true);
                ReloadPageData();
            } else {
                ShowMsg(data.message, false);
            }
            $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
        },
        error: function () {
            try {
                loading.close();
            } catch (e) { }
        }
    });
}

 