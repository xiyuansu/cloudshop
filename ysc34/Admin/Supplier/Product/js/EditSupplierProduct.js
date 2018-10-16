$(function () {
    if ($("#ctl00_contentHolder_hidAuditStatus").val() == "1") {//申请审核
        $("#liSalestatus").hide();
    } else {
        $("#divAudit").html("");
    }
    $("#enableSkuRow,#btnEnableSku,#btnshowSkuValue,#btnAddItem,#aSalestatus,#txtBatchWeight,#btnUploadSKUImg").hide();//开启规格 编辑规格	添加规格 隐藏   
    $(".skuItem_CostPrice.form-control").hide();//.attr("readonly", "readonly").attr("css", "specdiv").attr("title", "不可编辑");//.attr("placeholder", "不可编辑");
    $(".skuItem_Weight.form-control").hide();
    $(".skuItem_WarningQty.form-control").hide();
    $(".skuItem_Qty.form-control").hide();
    
    $(".skuSpan4Show").show();

    $("#txtBatchCostPrice").hide();//.attr("readonly", "readonly").attr("title", "不可编辑");//供货价（成本价）
    $(".specdiv").unbind("click");
    $(".SpecificationTh td:last-child").remove();//操作列
    $(".SpecificationTr td:last-child").remove();//      $(".glyphicon.glyphicon-trash").hide();//删除规格
    $(".skuItem_Qty.form-control").attr("readonly", "readonly").attr("title", "不可编辑");//规格表格里面库存 只读
    $(".skuItem_WarningQty.form-control").attr("readonly", "readonly").attr("title", "不可编辑");//规格表格里面警戒库存 只读 

    
})
function ShowSecondMenuLeft(firstnode, secondurl, threeurl, itemName) {
    window.parent.ShowMenuLeft(firstnode, secondurl, threeurl, itemName);
}