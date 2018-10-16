var curpagesize = 10, curpageindex = 1, total = 0;
var url = 'ashx/ShopDecoration.ashx';


$(function () {
    SeacherData();
});

function SeacherData() {
    $.ajax({
        url: url, data: { flag: "Select" }, type: "post", success: function (json) {
            var obj = eval('(' + json + ')');
            entity = obj;
            var table = $("#tbList");
            table.children().remove();
            if (obj.Result.length > 0) {
                for (var i = 0; i < obj.Result.length; i++) {
                    var tr = $("<tr>").appendTo(table);
                    $("<td>").html(obj.Result[i].FloorName).appendTo(tr);
                    $("<td>").html(obj.Result[i].Quantity).appendTo(tr);
                    $("<td>").html("<input type=\"text\" class=\"setorder\" floorId=\"" + obj.Result[i].FloorId + "\" oldvalue=\"" + obj.Result[i].DisplaySequence + "\" value=\"" + obj.Result[i].DisplaySequence + "\" />").appendTo(tr);
                    var strOption = "<a href=\"MdyShopDecoration.aspx?FloorId=" + obj.Result[i].FloorId + "\" >修改</a>&nbsp;&nbsp;<a href='javascript:void(0)' onclick='Delete(" + obj.Result[i].FloorId + ")' >删除</a>";
                    $("<td>").html(strOption).appendTo(tr);
                }
                $(".setorder").blur(function (e) {
                    oldvalue = parseInt($(this).attr("oldvalue"));
                    if (isNaN(oldvalue)) { oldvalue = 0; }
                    value = parseInt($(this).val());
                    if (isNaN(value)) { value = 0; }
                    floorId = parseInt($(this).attr("floorId"));
                    if (isNaN(floorId)) { floorId = 0; }
                    if (oldvalue != value && floorId > 0) {
                        $.post('/Depot/home/ashx/ShopDecoration.ashx', { flag: "SetDisplaySequence", FloorId: floorId, DisplaySequence: value }, function (json) {
                            var obj = eval('(' + json + ')');
                            if (obj.Result.Success.Status == true) {
                                SeacherData();
                                ShowMsg("排序修改成功", true);
                            }
                            else {
                                ShowMsg(obj.Result.Success.Msg, false);
                            }
                        }
);
                    }
                });
            }
            else {
                var tr = $("<tr>").appendTo(table);
                $("<td>").attr("colspan", "4").attr("align", "center").html("没有数据!").appendTo(tr);
            }
        }, error: function (r) {
            try {
                loadingobj.close();
            } catch (e) { }
            ShowMsg("系统内部异常", false);

        }
    })
}

function Delete(FloorId) {
    if (confirm("您确定要删除吗?")) {
        $.post('/Depot/home/ashx/ShopDecoration.ashx', { flag: "Delete", FloorId: FloorId }, function (json) {
            var obj = eval('(' + json + ')');
            if (obj.Result.Success.Status == true) {
                ShowMsg("删除成功", true);
                SeacherData();
            }
            else {
                ShowMsg(obj.ErrorResponse.ErrorMsg, false);
            }
        }
        );
    }
}