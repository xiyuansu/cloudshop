$(function () {
    $("#liSalestatus,#aSalestatus").hide(); //销售状态 收缩按钮	不可见    
    
    $("#ctl00_contentHolder_l_tags,#salePriceRow,#txtBatchMarketPrice,#ReferralConfig").hide();// 商品标签	隐藏。 一口价 佣金   
    $("#skuItems a").hide();
    //如果是发布商品，则隐藏某些字段
    if (parseInt($("#ctl00_contentHolder_hidProductId").val()) == 0) {
        $("#liDisplaySequenc").hide();
    }
   
})

        function ShowSecondMenuLeft(firstnode, secondurl, threeurl,itemName) {
            window.parent.ShowMenuLeft(firstnode, secondurl, threeurl, itemName);
        }