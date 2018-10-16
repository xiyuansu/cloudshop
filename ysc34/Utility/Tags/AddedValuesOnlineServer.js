var ytopAddedValues = 120;
var showPositiontopAddedValues = 2;
$(document).ready(function () {
    showPositiontopAddedValues = parseInt($("#hidPositionAddedValues").val());
    ytopAddedValues = parseInt($("#hidYPostionAddedValues").val());
    if (isNaN(ytopAddedValues)) {
        if (showPositiontopAddedValues == 1 || showPositiontopAddedValues == 3)
            ytopAddedValues = 120;
        else
            ytopAddedValues = 0;
    }
    if (showPositiontopAddedValues == 2 || showPositiontopAddedValues == 4) {
        $("#qq_right").removeClass("qqrighttop").addClass("qqrightbot");
        $("#qq_right").css("bottom", ytopAddedValues);
    }
    else {
        $("#qq_right").removeClass("qqrightbot").addClass("qqrighttop");
        $("#qq_right").css("top", ytopAddedValues);
    }
    if (isNaN(showPositiontopAddedValues) || (showPositiontopAddedValues != 1 && showPositiontopAddedValues != 2 && showPositiontopAddedValues != 3 && showPositiontopAddedValues != 4)) {
        showPositiontopAddedValues = 1;
    }
    if (showPositiontopAddedValues == 1 || showPositiontopAddedValues == 2) {
        $("#qq_right").css("left", "0px");
        $("#qq_right").css("right", "auto");
    }
    else {
        $("#qq_right").css("right", "-70px");
        $("#qq_right").css("left", "auto");
    }
    if (showPositiontopAddedValues == 3 || showPositiontopAddedValues == 4) {
        $("#qq_right .b").addClass("b_r");
        $("#qq_right .c").addClass("c_r");
        $("#qq_right .d").addClass("d_r");
        $("#qq_right .e").addClass("e_r");
        //$("#e").remove();
        $(".close").css("left", "-14px")
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
        if (showPositiontopAddedValues == 2 || showPositiontopAddedValues == 4)
            scrollfrombotAddedValues();
        else
            scrollfromtopAddedValues();
    });
});

//滚动代码开始
function scrollfromtopAddedValues() {   
    if (document.body.offsetWidth > 900) {
        if ($("#qq_right") != null && ($("#qq_right") != undefined)) {
            var scrollTop = window.pageYOffset ? window.pageYOffset : document.documentElement.scrollTop;
            $("#qq_right").css("top", (scrollTop + ytopAddedValues) + "px");
        }
        else { return false }
    }
    else {
        $("#qq_right").hide();
    }
}


function fullScreenAddedValues() {
    this.moveTo(0, 0);
    this.outerWidth = screen.availWidth;
    this.outerHeight = screen.availHeight;
}

function scrollfrombotAddedValues() {
    //var $bottomTools = $('#qq_right');
    var scrollHeight = $(document).height();
    var scrollTop = $(window).scrollTop();
    var $windowHeight = window.innerHeight
    $('#qq_right').css("bottom", ytopAddedValues + "px");
    $("#qq_right").show();
    //alert(scrollHeight + "-" + scrollTop + "-" + ytop + "-" + $windowHeight);
}



//window.onresize = qqshow;
//window.onload = qqshow;
window.maximize = fullScreenAddedValues;