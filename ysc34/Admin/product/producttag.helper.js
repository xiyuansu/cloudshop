function CheckTagId() {
    var tagIds = "";
    $("#div_tags").find(":checkbox:checked").each(function () {
        tagIds += $(this).val() + ",";
    });

    if (tagIds.length > 0) {
        tagIds = tagIds.substr(0, tagIds.length - 1);
    }

    $("#ctl00_contentHolder_txtProductTag").val(tagIds);
}
$(document).ready(function (e) {
    $('input[name="productTags"]').on('ifChecked', function (event) {
        CheckTagId($(this).get(0));
    });
});


function AddTags(aobj) {
    if ($("#a_addtag").text() == "添加") {
        $("#div_addtag").show();
        $("#a_addtag").text('取消');
        $("#a_addtag").attr('class', 'del');
    } else {
        $("#div_addtag").hide();
        $("#a_addtag").text('添加');
        $("#a_addtag").attr('class', 'add');
    }

}

function AddAjaxTags() {
    var tagvalu = $("#txtaddtag").val().replace(/\s/g, "");
    if (tagvalu == "") {
        alert('请输入标签名称！');
        return false;
    }
    $("#div_addtag").hide();
    $("#a_addtag").text('添加');
    $("#a_addtag").attr('class', 'add');
    $.ajax({
        url: "/admin/product/ProductTags",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { TagValue: tagvalu, Mode: "Add", isAjax: "true" },
        async: false,
        success: function (data) {
            if (data.Status == "true") {
                var newtagId = data.msg;
                if (newtagId != "" && parseInt(newtagId) > 0) {
                    $("#f_div ").append($("<span style=\"margin-right:10px;\"><input type=\"checkbox\" value=\"" + newtagId + "\" class=\"icheck\"  onclick=\"CheckTagId(this)\" checked=\"checked\" /><labe style=\"margin-left:5px\">" + tagvalu + "</labe></span>"));
                    //$("#f_div ").append(tagvalu);
                    var oldtagId = $("#ctl00_contentHolder_txtProductTag").val().replace(/\s/g, "");
                    if (oldtagId != "") {
                        oldtagId += "," + newtagId;
                    } else {
                        oldtagId = newtagId;
                    }

                    $("#ctl00_contentHolder_txtProductTag").val(oldtagId);
                    ShowMsg("添加标签名成功", true);
                    $('#div_tags .icheck').iCheck({
                        checkboxClass: 'icheckbox_square-red',
                        radioClass: 'iradio_square-red',
                        increaseArea: '20%' // optional
                    });
                }
            }
            else {
                ShowMsg(data.msg, false);
            }
        }
    });
}