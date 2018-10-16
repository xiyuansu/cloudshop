//一共存在多少个图文
var twCount = 2;

var twCountb = 2;

//当前编辑的图文ID
var boxIdN = 1;

//是否为编辑状态
var edit = false;

//图文json对象
var tws = [];
var tw = function () { return { "BoxId": "", "Title": "", "Url": "", "Description": "", "Content": "", "PicUrl": "", "Status": "" } }
ntw = new tw();
ntw.BoxId = 1;
tws.push(ntw);
ntw = new tw();
ntw.BoxId = 2;
tws.push(ntw);

//select联动
//function changeRelevanceType(selectVal) {
//    if ('2' == selectVal) {
//        $("#w_url").show();
//    }
//    else {
//        $("#w_url").hide();
//    }
//}

//同步标题
function syncTitle(value) {
    $("#title" + boxIdN).text(value);
}

function syncSingleTitle(value) {
    $("#LbimgTitle").text(value);
}

function syncAbstract(value) {
    $("#Lbmsgdesc").text(value);
}

//添加图文
function addSBox() {
    if (twCount >= 10) {
        alert("最多可添加10个图文");
    } else {
        twCountb++;
        twCount++;
        var code = $('#modelSBox').html();
        code = code.replace(/rpcode1366/gm, twCountb);
        $('#addSBoxInfoHere').before(code);
        var ntw = new tw();
        ntw.BoxId = twCountb;
        tws.push(ntw);
    }
}

function editSBox() {
    if (twCount >= 10) {
        alert("最多可添加10个图文");
    } else {
        twCountb++;
        twCount++;
        var code = $('#modelSBox').html();
        code = code.replace(/rpcode1366/gm, twCountb);
        $('#addSBoxInfoHere').before(code);
    }
}


//删除图文
function sBoxDel(id) {
    saveDataToJson();
    if (twCount <= 2) {
        alert("请至少保留两个图文！");
    } else {
        $('#sbox' + id).remove();

        //json中设置status值为del
        tws[id - 1].Status = "del";
        twCount--;
        //默认回到第一个
        boxIdN = 1;
        loadData();
        //移动模型
        moveBox();
    }
}

//修改图文
function editTW(sBoxId) {
    
    //保存对象到json
    saveDataToJson();
    //初始化
    initializeInfoBox();

    boxIdN = sBoxId;

    //载入模型数据对象
    loadData();

    //移动模型
    moveBox();
}

function DelCount() {
    var count = 0;
    for (var a in tws) {
        if (a.Status == "del") {
            count += 1;
        }
    }
}
///递归运算直到下一次不是已删除的。
function GetNextIndex(CurrIndex) {
    if (CurrIndex < tws.length) {
        CurrIndex += 1;
        if (tws[CurrIndex - 1].Status != "del") {
            return CurrIndex;
        }
        else {
            return GetNextIndex(CurrIndex);
        }
    }
    else {
        CurrIndex = 1;
        if (tws[CurrIndex - 1].Status != "del") {
            return CurrIndex;
        }
        else {
            return GetNextIndex(CurrIndex);
        }
    }
}

function getIndex() {
    if (DelCount() == tws.length) boxIdN = 1;
    else {
        if (boxIdN > tws.length) boxIdN = 1;

        if (tws[boxIdN - 1].Status == "del")
            boxIdN = GetNextIndex(boxIdN);
    }

}
//移动模型
function moveBox() {
    getIndex();
    var h = $("#sbox" + boxIdN).offset().top - 65;
    $("#box_move").animate({ "margin-top": h + "px" });
}

//保存对象到json
function saveDataToJson() {
    debugger;
    for (var a in tws) {
        var ia = parseInt(a);
        if (ia + 1 == boxIdN) {
            tws[ia].Title = $("#title").val();
            tws[ia].Content = editor.html();
            tws[ia].Url = $("#urlData").val();
            tws[ia].PicUrl = $("#fmSrc").val();
        }
    }

}

//载入模型数据对象
function loadData() {
    getIndex();
    $("#title").val(tws[boxIdN - 1].Title);
    $("#fmSrc").val(tws[boxIdN - 1].PicUrl);
    $("#urlData").val(tws[boxIdN - 1].Url);
    editor.html(tws[boxIdN - 1].Content);
    if (tws[boxIdN - 1].PicUrl != "" && tws[boxIdN - 1].PicUrl != null) {
        var smallimg = $("<img>");
        var delimg = "<a id=\"delpic\" href='javascript:void(0)' onclick='RemovePicMulti()'>删除</a>";
        $("#smallpic").empty();
        smallimg.attr("src", tws[boxIdN - 1].PicUrl);
        $("#smallpic").append(smallimg);
        $("#smallpic").append(delimg);
        $("#smallpic").show();
    }

    //    if (!(isNull(tws[boxIdN - 1].UrlType) || 0 == tws[boxIdN - 1].UrlType)) {//如有编辑过
    //        $("#typeID").val(tws[boxIdN - 1].UrlType);
    //        switch (tws[boxIdN - 1].UrlType.toString()) {
    //            case "2": //URL
    //                $("#w_url").css("display", "block");
    //                $("#urlData").val(tws[boxIdN - 1].Url);
    //                break;

    //            default:
    //                //
    //                break;
    //        }
    //    }
}

//初始化数据录入
function initializeInfoBox() {
    $("#title").val("");
    $("#fmSrc").val("");
    editor.html('');
    $("#typeID").val("0");
    //$("#w_url").css("display", "none");
    $("#smallpic").empty().hide();
    $("#valueID option:first").attr("selected", "selected")
    $("#urlData").val("");
}

//显示遮罩
function sBoxzzShow(id) {
    $('#zz_sbox' + id).css('display', 'block')
}

//隐藏遮罩
function sBoxzzHide(id) {
    $('#zz_sbox' + id).css('display', 'none')
}

//是否为空
function isNull(obj) {
    if (null == obj) {
        return true;
    } else {
        if ("" == obj) {
            return true;
        }
        return false;
    }
}

//修改预览图
function changeSYLT(sboxID, src) {
    if (sboxID)
        $("#sylt" + sboxID).attr("src", src);
}

//载入封面图片
function loadFW(id, src) {
    $("#img" + id).attr("src", src).css("display", "block");
}

function IsLastEdit() {
    var IsLastEdit = true;
    for (var i = boxIdN - 1; i < tws.length; i++) {
        if (tws[i].Status != "del")
            IsLastEdit = false;
    }
    return IsLastEdit;
}

//（可删除）以String的方式查看Json
function viewJson() {
    $("#viewJson").val($.toJSON(tws));
}

//验证Json数据
function checkJson() {
    //{"BoxId":"","Title":"","UrlType":"","Url":"","Description":"","PicUrl":"","Status":""}
    editTW(boxIdN); //通过此方法保存最后一个编辑的数据
    var errorBoxId;
    var pass = true;

    //alert(tws.length+"-"+boxIdN);
    for (var a in tws) {
        if (tws[a].Status != "del" && a < boxIdN) {
            errorBoxId = a;
            if (isNull(tws[a].Title)) {
                $("#msg").text("标题不能为空！").show();
                //alert(a + "-" + tws[a].Title);
                pass = false;
                break;
            } else if (isNull(tws[a].PicUrl)) {
                $("#msg").text("请上传一张封面！").show();
                pass = false;
                break;
            } else if (isNull(tws[a].Content) && isNull(tws[a].Url)) {
                $("#msg").text("请输入内容或自定义链接！").show();
                pass = false;
                break;
            }
        }
    }
    if (pass && boxIdN < tws.length) {
        initializeInfoBox();

        boxIdN = boxIdN + 1;
        if (IsLastEdit()) {
            pass = true;
        }
        else {
            //载入模型数据对象
            loadData();

            //移动模型
            moveBox();
            return false;
        }

    }
    if (pass) {
        $("#Articlejson").val($.toJSON(tws));

        if (edit) {
            EditMultArticles();
        }
        else {
            AddMultArticles();
        }

    } else {
        //载入错误图文
        editTW(parseInt(errorBoxId) + 1);
        return false;
    }
}