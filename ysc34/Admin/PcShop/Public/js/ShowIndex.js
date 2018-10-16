

$(function () {   
    jQuery.getJSON("/api/Hi_Ajax_NavMenu.ashx", function (settingjson) {
        //底部导航
        menu(settingjson);
    });
})

function menu(settingjson) { 
    GetUICss();
}

function GetUICss() {
    var oUl = $("#ul");
    var len = parseInt($("#ul  li").length);
    for (i = 0; i < $("#ul  li").length; i++) {
        var width = 100 / len;
        width += "%"
        $("#ul  li").css("width", width);
    }

    //$("#menuNav  div[data='1']").click(function () {  
    //    if ($(this).find(".childNav").css("display") == "none") {
    //        $(this).find(".childNav").css("display", "block");
    //    } else {
    //        $(this).find(".childNav").css("display", "none");
    //    }
    //});
}

