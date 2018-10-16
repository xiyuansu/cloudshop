var arrytext = null; //定义全局变量
var arrytempstr = null;
var dialog = null;

//层对象弹出
function DialogShow(hishop_titile, hishop_id, hishop_div, btnId) {
    $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2");
    $('#divLock').show();
    dialog = art.dialog({
        id: hishop_id,
        title: hishop_titile,
        content: $("#" + hishop_div).html(),
        init: function () {
            if (arrytext != null) {
                getArryText(arrytext);
            }
        },
        resize: true,
        fixed: true,
        close: function () {
            arrytext = null;
        },
        button: [{
            name: '确 认',
            callback: function () {
                var istag = validatorForm();
                if (istag) {
                    var temparrytext = arrytext;
                    if (temparrytext != null) {
                        setShowText(temparrytext);
                        this.close();
                        getArryText(temparrytext);
                        $("#" + btnId).trigger("click");
                    } else {
                        this.close();
                        $("#" + btnId).trigger("click");
                    }
                } else {
                    return false;
                }
                $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
            },
            focus: true
        }, {
            name: '取消',
            callback: function () {
                $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
            }
        }]
    });
}

function DialogShowOfCalback(hishop_titile, hishop_id, hishop_div, btnId, okbutton_calback) {
    dialog = art.dialog({
        id: hishop_id,
        title: hishop_titile,
        content: $("#" + hishop_div).html(),
        resize: true,
        fixed: true,
        button: [{
            name: '确 认',
            callback: okbutton_calback,
            focus: true
        }, {
            name: '取 消',
        }]
    });
}

//关闭窗口并且刷新页面
function CloseDialogFrame(msg, refresh) {
    if (msg != undefined && msg != "")
        alert(msg);
    dialog.close();
    if (refresh == undefined || refresh == true)
        document.location.reload();
}
//弹窗参数开放
function OpenDialogFrame(url, params, cache) {
    dialog = art.dialog.open(url, params, cache);
}
//IFrame弹出
function DialogFrame(url, title_tip, w_width, h_height, callBack, button) {
    var currentwindow = null;
    var tmpwidth = 900;
    var tmpheight = 500;
    var params = {
        title: title_tip
    };
    if (w_width != null) {
        tmpwidth = w_width;
    }
    if (h_height != null) {
        tmpheight = h_height;
    }
    if (tmpwidth != 0) {
        params.width = tmpwidth;
        if (tmpheight != 0) {
            params.height = tmpheight;
        }
    } else {
        if (tmpheight != 0) {
            params.height = tmpheight;

        }
    }
    if (callBack) {
        params.close = function () {
            callBack();
        };
    }
    if (button) {
        params.button = button;
    }
    OpenDialogFrame(url, params, true);
}


function DialogFrameClose(url, title_tip, w_width, h_height) {
    var currentwindow = null;
    var tmpwidth = 900;
    var tmpheight = 500;
    if (w_width != null) {
        tmpwidth = w_width;
    }
    if (h_height != null) {
        tmpheight = h_height;
    }
    if (tmpwidth != 0) {
        if (tmpheight != 0) {
            dialog = art.dialog.open(url, {
                title: title_tip,
                width: tmpwidth,
                height: tmpheight,
                close: function () {
                    CloseFrameWindow();
                }
            }, true);
        } else {
            dialog = art.dialog.open(url, {
                title: title_tip,
                width: tmpwidth,
                close: function () {
                    CloseFrameWindow();
                }
            }, true);
        }
    } else {
        if (tmpheight != 0) {
            dialog = art.dialog.open(url, {
                title: title_tip,
                height: tmpheight,
                close: function () {
                    CloseFrameWindow();
                }
            }, true);
        } else {
            dialog = art.dialog.open(url, {
                title: title_tip,
                close: function () {
                    CloseFrameWindow();
                }
            }, true);
        }
    }
}

function ShowMessageDialog(hishop_titile, hishop_id, hishop_div) {
    dialog = art.dialog({
        id: hishop_id,
        title: hishop_titile,
        content: $("#" + hishop_div).html(),
        fixed: true,
        close: function () {
            $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
        }
    });
    $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2");
}



//取值
function getArryText(curent_arrytext) {
    if (curent_arrytext != null) {
        for (var keyname in curent_arrytext) {
            getTypeArrayText(keyname, curent_arrytext[keyname]);
        }
    } else {
        for (var keyname in arrytext) {
            getTypeArrayText(keyname, arrytext[keyname]);
        }
    }
}

function getTypeArrayText(keyname, vals) {
    switch ($("#" + keyname).attr("type")) {
        case "checkbox":
            $("#" + keyname).attr("checked", (vals == "true" || vals == "checked" || vals == true || vals == "1") ? true : false);
            break;
        case "radio":
            $("#" + keyname).attr("checked", vals);
            break;
        default:
            $("#" + keyname).val(unescape(vals));
            break;
    };
}


function setShowText(parmajson) {
    for (var keyname in parmajson) {
        parmajson[keyname] = $("#" + keyname).val();
    }
}






//设置值

function setArryText(keyname, keyvalue) {
    var temptex = "\"" + keyname + "\":\"" + escape(keyvalue) + "\"";
    if (arrytempstr != null) {
        arrytempstr = arrytempstr.substr(0, arrytempstr.length - 1);
        arrytempstr += "," + temptex + "}";

    } else {
        arrytempstr = "{" + temptex + "}"
    }
    arrytext = $.parseJSON(arrytempstr);
}

function getArrayString(keyname, keyvalue) {
    var temptex = "\"" + keyname + "\":\"" + keyvalue + "\"";
    return temptex;
};

$(function () {
    $(".aui_close").click(function () {
        $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
    })
})