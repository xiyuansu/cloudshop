
$(function () {

    initImageUpload();
    showbox = $("#datashow");
    showpager = $("#showpager");
    dataurl = $("#dataurl").val();
    datanullbox = showbox;
    ShowListData(showbox);
});

//搜索参数
function initQuery(obj) {
    return obj;
}

function ShowListData(target) {
    var urldata = {
        action: "GetAppletFloorList"
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
            var databox = $(target);
            loadingobj.close();
            databox.empty(); DataNullHelper.hide(datanullbox);
            if (data) {
                if (data.rows.length > 0) {
                    isnodata = false;
                    databox.html(template('datatmpl', data));
                }
                $(".delfloor").click(function (e) {
                    if (window.confirm("确认要删除楼层吗,删除后不能恢复！")) {
                        var floorId = parseInt($(this).attr("floorid"));
                        if (isNaN(floorId) || floorId <= 0) {
                            ShowMsg("错误的楼层ID", false);
                            return;
                        }
                        else {
                            var urldata = {
                                action: "DeleteAppletFloor", FloorId: floorId
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
                                        ShowListData(showbox);
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
                });
                $(".setorder").blur(function (e) {
                    oldvalue = parseInt($(this).attr("oldvalue"));
                    if (isNaN(oldvalue)) { oldvalue = 0; }
                    value = parseInt($(this).val());
                    if (isNaN(value)) { value = 0; }
                    floorId = parseInt($(this).attr("floorId"));
                    if (isNaN(floorId)) { floorId = 0; }
                    if (oldvalue != value && floorId > 0) {
                        var urldata = {
                            action: "SetDisplaySequence", FloorId: floorId, DisplaySequence: value
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
                                    ShowListData(showbox);
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
                });
            }
            if (isnodata) {
                //total = 0;
                DataNullHelper.show(datanullbox);
            }
        },
        error: function () {
            loadingobj.close();
            ShowMsg("系统内部异常", false);
        }
    });
}


// 初始化图片上传控件
function initImageUpload() {
    var imgSrcs = $("#ctl00_contentHolder_hidOldImages").val();
    var arySrcs = imgSrcs.split(',');
    $('#imageContainer span[name="productImages"]').hishopUpload(
                   {
                       title: '商品图片',
                       url: "/admin/UploadHandler.ashx?action=newupload",
                       imageDescript: '',
                       displayImgSrc: arySrcs,
                       imgFieldName: "AppletConfigImages",
                       defaultImg: '',
                       pictureSize: '',
                       imagesCount: 5,
                       dataWidth: 9
                   });

}

//获取上传成功后的图片路径
function getUploadImages() {
    var aryImgs = $('#imageContainer span[name="productImages"]').hishopUpload("getImgSrc");
    var imgSrcs = "";
    $(aryImgs).each(function () {
        imgSrcs += this + ",";
    });
    $("#ctl00_contentHolder_hidUploadImages").val(imgSrcs);

    //$("#ctl00_contentHolder_hidProductIds").val(getSelectedProductIds());
    return true;
}



function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
