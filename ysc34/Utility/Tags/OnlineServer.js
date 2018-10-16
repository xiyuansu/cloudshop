var ytop = 120;
var showPosition = 2;
$(document).ready(function () {
   showPosition = parseInt($("#hidPosition").val());
    ytop = parseInt($("#hidYPostion").val());
    if (isNaN(ytop)) {
        if (showPosition == 1 || showPosition == 3)
            ytop = 120;
        else
            ytop = 0;
    }
    if (showPosition == 2 || showPosition == 4) {
        $("#qq_right").removeClass("qqrighttop").addClass("qqrightbot");
        $("#qq_right").css("bottom", ytop);
    }
    else {
        $("#qq_right").removeClass("qqrightbot").addClass("qqrighttop");
        $("#qq_right").css("top", ytop);
    }
    if (isNaN(showPosition) || (showPosition != 1 && showPosition != 2 && showPosition != 3 && showPosition != 4)) {
        showPosition = 1;
    }
    if (showPosition == 1 || showPosition == 2) {
        $("#qq_right").css("left", "0px");
        $("#qq_right").css("right", "auto");
    }
    else {
        $("#qq_right").css("right","-70px");
        $("#qq_right").css("left", "auto");
    }
    if (showPosition == 3 || showPosition == 4) {
        $("#qq_right .b").addClass("b_r");
        $("#qq_right .c").addClass("c_r");
        $("#qq_right .d").addClass("d_r");
        $("#qq_right .e").addClass("e_r");
        //$("#e").remove();
        $(".close").css("left","-14px")
        //$("#e_r").attr("id", "e");
    }
    else {
        $(".close").css("left", "106px")
        //$("#e_r").remove();
    }
    $("#qq_right .a .d").hide();
    $("#e").show();
    $("#qq_right").show();
    $(window).scroll(function () {
        if (showPosition == 2 || showPosition == 4)
            scrollfrombot();
        else
            scrollfromtop();
    });
});
function showKefu(val) {
    val.style.display = 'none';
    if (showPosition == 1 || showPosition == 2)
        $("#qq_right").css("left", 0 + "px");
    else
        $("#qq_right").css("right", "0px");
    $("#mainonline").show();
    $("#qq_right .a .d").show();
}
function closekf() {
    $("#b").show();
    $("#qq_right .a .d").hide();
    if (showPosition == 1 || showPosition == 2) {
        $("#mainonline").hide();
        $("#qq_right").css("left", 0 + "px");
    }
    else {
        $("#mainonline").hide();
        $("#qq_right").css("right", "-70px");
    }
}
//滚动代码开始
function scrollfromtop() {
    if (document.body.offsetWidth > 900) {
        if ($("#qq_right") != null && ($("#qq_right") != undefined)) {
            var scrollTop = window.pageYOffset ? window.pageYOffset : document.documentElement.scrollTop;
            $("#qq_right").css("top", (scrollTop + ytop) + "px");
        }
        else { return false }
    }
    else {
        $("#qq_right").hide();
    }
}


function fullScreen() {
    this.moveTo(0, 0);
    this.outerWidth = screen.availWidth;
    this.outerHeight = screen.availHeight;
}

function scrollfrombot() {
    //var $bottomTools = $('#qq_right');
    var scrollHeight = $(document).height();
    var scrollTop = $(window).scrollTop();
    var $windowHeight = window.innerHeight
    $('#qq_right').css("bottom", ytop + "px");
    $("#qq_right").show();
    //alert(scrollHeight + "-" + scrollTop + "-" + ytop + "-" + $windowHeight);
}



//window.onresize = qqshow;
//window.onload = qqshow;
window.maximize = fullScreen;