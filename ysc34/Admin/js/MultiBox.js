//一共存在多少个图文
var twCount = 2;

var twCountb = 2;

//当前编辑的图文ID
var boxIdN = 0;

//是否为编辑状态
var edit = false;

//图文json对象
var tws = new Array();
pushTWS(1);
pushTWS(2);

function pushTWS(boxId) {
    tws.push({ BoxId: boxId, Title: '', Url: '', Description: "", Content: '', PicUrl: '', Status: "" });
}

function saveData() {
    getUploadImages();
    var title = $("#title").val();
    var hdpic = $("#hdpic").val();
    if (title == "") {
        alert("请填写标题!");
        return false;
    }
    if (hdpic == "") {
        alert("请上传!");
        return false;
    }
    var url = $("#urlData").val().length > 0 ? $("#urlData").val() : "";
    if (url.indexOf("http://") == -1 && url != "" && url.indexOf("https://") == -1) {
        url = "http://" + url;
    }
    var content = UEEditor.getContent();
    var bolExists = false;
    for (var i = 0; i < tws.length; i++) {
        if (parseInt(tws[i].BoxId) == (parseInt(boxIdN) + 1)) {
            bolExists = true;
            tws[i].Title = title;
            tws[i].Url = url;
            tws[i].Content = content;
            tws[i].PicUrl = hdpic;
        }
    }
    if (!bolExists) {
        if (!isNull(boxIdN)) {
            var tw = { BoxId: boxIdN, Title: title, Url: url, Description: "", Content: content, PicUrl: hdpic, Status: "" };
            tws.push(tw);
        }

    }


    editTW(boxIdN);
    if (boxIdN + 1 < tws.length)
        editTW(boxIdN + 1);

}

function syncSingleTitle(value) {
    $("#LbimgTitle").text(value);
}

function syncAbstract(value) {
    $("#Lbmsgdesc").text(value);
}

//添加图文
function addSBox() {
    if (tws.length >= 10) {
        alert("最多可添加10个图文");
    } else {
        var code = $('#modelSBox').html();
        code = code.replace(/rpcode1366/gm, tws.length);
        $('#addSBoxInfoHere').before(code);
        var addIndex = tws.length;
        $("#zz_sbox" + addIndex).attr("index", addIndex);
        $("#zz_sbox" + addIndex).mouseout(function (e) {
            sBoxzzHide($(this).attr("index"));
        })
        $("#sbox" + addIndex).attr("index", addIndex);
        $("#sbox" + addIndex).mousemove(function (e) {
            sBoxzzShow($(this).attr("index"))
        })
        $("#zz_edit" + addIndex).attr("index", addIndex);
        $("#zz_edit" + addIndex).click(function (e) {
            editTW($(this).attr("index"));
        });
        $("#zz_del" + addIndex).attr("index", addIndex);
        $("#zz_del" + addIndex).click(function (e) {
            sBoxDel($(this).attr("index"));
        });
        pushTWS(tws.length + 1);
        editTW(tws.length - 1);
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

function getObjIndex(obj) {

}

//删除图文
function sBoxDel(id) {
    if (tws.length <= 2) {
        alert("请至少保留两个图文！");
    } else {
        $('#sbox' + id).remove();
        tws.splice(id, 1);
        for (var i = 0; i < tws.length; i++) {
            tws[i].BoxId = (i + 1);
            if (i >= id) {
                $('#sbox' + (i + 1)).attr("id", "sbox" + i);
                $('#zz_sbox' + (i + 1)).attr("id", "zz_sbox" + i);
                $('#zz_edit' + (i + 1)).attr("id", "zz_edit" + i);
                $('#zz_del' + (i + 1)).attr("id", "zz_del" + i);
                $("#box" + i).unbind();

                $("#zz_box" + i).unbind();

                $("#zz_edit" + i).unbind();

                $("#zz_del" + i).unbind();

                $("#zz_sbox" + i).attr("index", i);
                $("#zz_sbox" + i).mouseout(function (e) {
                    sBoxzzHide($(this).attr("index"));
                })
                $("#sbox" + i).attr("index", i);
                $("#sbox" + i).mousemove(function (e) {
                    sBoxzzShow($(this).attr("index"))
                })
                $("#zz_edit" + i).attr("index", i);
                $("#zz_edit" + i).click(function (e) {
                    editTW($(this).attr("index"));
                });
                $("#zz_del" + i).attr("index", i);
                $("#zz_del" + i).click(function (e) {
                    sBoxDel($(this).attr("index"));
                });
            }
        }

        //默认回到第一个
        boxIdN = tws.length - 1;
        loadData();
        //移动模型
        moveBox();
    }
}

//修改图文
function editTW(sBoxId) {
    //初始化
    initializeInfoBox();

    boxIdN = parseInt(sBoxId);

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

function dataBind() {
    //创建大于2的框框
    for (var i = 2; i < tws.length; i++) {
        var code = $('#modelSBox').html();
        code = code.replace(/rpcode1366/gm, i);
        $('#addSBoxInfoHere').before(code);
        $("#zz_sbox" + i).attr("index", i);
        $("#zz_sbox" + i).mouseout(function (e) {
            sBoxzzHide($(this).attr("index"));
        })
        $("#sbox" + i).attr("index", i);
        $("#sbox" + i).mousemove(function (e) {
            sBoxzzShow($(this).attr("index"))
        })
        $("#zz_edit" + i).attr("index", i);
        $("#zz_edit" + i).click(function (e) {
            editTW($(this).attr("index"));
        });
        $("#zz_del" + i).attr("index", i);
        $("#zz_del" + i).click(function (e) {
            sBoxDel($(this).attr("index"));
        });
        editTW(i);
    }

    for (var i = 0; i < tws.length; i++) {
        $("#title" + i).text(tws[i].Title);
        $("#img" + i).attr("src", tws[i].PicUrl);
    }

    editTW(tws.length - 1);

}

//移动模型
function moveBox(moveIndex) {
    var nowH = 0;
    var h = 0;
    if (moveIndex == undefined) {
        moveIndex = parseInt(boxIdN);
    }
    if ($("#sbox" + (moveIndex)).length > 0) {
        nowH = $("#sbox" + (moveIndex)).offset().top;
        h = $("#sbox" + (moveIndex)).offset().top - 55;
    }
    else {
        moveIndex = 0;
        nowH = $("#sbox" + (moveIndex)).offset().top;
        h = $("#sbox" + (moveIndex)).offset().top - 55;

    }
    $("#box_move").animate({ "margin-top": h + "px" }, function () {
        var scrollTop = 100000;
        $(document).scrollTop(scrollTop);
    });
}





//载入模型数据对象
function loadData() {
    $('#logoContainer span[name="spanButtonPlaceholder"]').hishopUpload(
                        {
                            title: '缩略图',
                            url: "/admin/UploadHandler.ashx?action=newupload",
                            imageDescript: '',
                            imgFieldName: "siteLogo",
                            pictureSize: '360*200',
                            imagesCount: 1,
                            target: "#img" + boxIdN,
                            targetType: "src",
                            displayImgSrc: tws[boxIdN].PicUrl,
                        });
    $("#title").val(tws[boxIdN].Title);
    $("#urlData").val(tws[boxIdN].Url);

    $("#title" + boxIdN).text(tws[boxIdN].Title);
    $("#img" + boxIdN).attr("src", tws[boxIdN].PicUrl);


    //editor.html(tws[boxIdN].Content);
    UEEditor.setContent(tws[boxIdN].Content);
    if (tws[boxIdN].PicUrl != "" && tws[boxIdN].PicUrl != null) {
        var smallimg = $("<img>");
        var delimg = "<a id=\"delpic\" href='javascript:void(0)' onclick='RemovePicMulti()'>删除</a>";
        $("#smallpic").empty();
        smallimg.attr("src", tws[boxIdN].PicUrl);
        $("#smallpic").append(smallimg);
        $("#smallpic").append(delimg);
        $("#smallpic").show();
    }
}

//初始化数据录入
function initializeInfoBox() {
    $("#title").val("");
    $("#fmSrc").val("");
    //editor.html('');
    //  UEEditor.setContent("");
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
    saveData();
    var pass = true;
    for (var i = 0; i < tws.length; i++) {
        if (isNull(tws[i].Title)) {
            $("#msg").text("标题不能为空！").show();
            pass = false;
            editTW(i);
            break;
        } else if (isNull(tws[i].PicUrl)) {
            $("#msg").text("请上传一张封面！").show();
            pass = false;
            editTW(i);
            break;
        }
    }
    if (!pass)
        return false;
    if (pass) {
        $("#Articlejson").val($.toJSON(tws));

        if (edit) {
            EditMultArticles();
        }
        else {
            AddMultArticles();
        }
    }
}