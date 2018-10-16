// JavaScript Document
//选择点击一级菜单显示
function ShowMenuLeft(firstnode, secondnode, threenode) {
    var Showarguments = arguments;    
    $.ajax({
        url: "Menu.xml?date=" + new Date(),
        dataType: "xml",
        type: "GET",
        async: false,
        timeout: 10000,
        error: function (xm, msg) {
            document.location.href = "/admin/login.aspx?error=" + msg;
        },
        success: function (xml) {
            $("#menu_left").html('');
            var curenturl = null;

            curenturl = $(xml).find("Module[Title='" + firstnode.replace(/\s/g, "") + "']").attr("Link");
            //start  营销聚合页判断
            var Ispromote;
            Ispromote = $(xml).find("Module[Title='" + firstnode.replace(/\s/g, "") + "']").attr("IsPromote");
            if (Ispromote == undefined) Ispromote = "0";
            var itemName = "";
            if (Ispromote == "1");
            {
                itemName = Showarguments[3];
            }
            //end 营销聚合页判断

            if (secondnode != null) {
                curenturl = secondnode;
            }

            $(xml).find("Module[Title='" + firstnode.replace(/\s/g, "") + "'] Item").each(function (i) {
                var item_icon = $(this).attr("icon");
                $menutitle = $('<div class="hishop_menutitle"></div>');
                $menuicon = $('<img />');
                $menutoogle = $('<div class="hishop_menutoogle"></div>');
                if(Ispromote=="0")
                    $menuaspan = $("<span>" + $(this).attr("Title") + "</span>"); //获取二级分类名称"
                else
                    $menuaspan = $("<span onclick=\"ShowMenuLeft('" + firstnode.replace(/\s/g, "") + "',null,null)\">" + $(this).attr("Title") + "</span>"); //获取二级分类名称"
                $menutitle.append($menuicon);
                //侧栏图标开始               
                $menuicon.attr("src", "images/menuicon/" + item_icon + ".png");
                $menutitle.append($menuaspan);
                $menutitle.append($menutoogle);
                if ((Ispromote == "1" && secondnode != null) || Ispromote == "0") {
                    $(this).find("PageLink").each(function (k) {
                        if (Ispromote == "0") {                            
                            var link_href = $(this).attr("Link");
                            var link_title = $(this).attr("Title");
                        }
                        else {
                            var link_href = secondnode;
                            var link_title = itemName;
                        }

                        var link_target = $(this).attr("Target");
                        var target = link_target ? link_target : "frammain";
                        $alink = $("<a href='" + link_href + "' target=" + target + ">" + link_title + "</a>");
                        if (link_href == curenturl) {
                            $alink.addClass("curent");
                        }
                        $menutoogle.append($alink);
                    });
                }
                $("#menu_left").append($menutitle);
            });


            $(".hishop_menu_scroll").css("display", "block");
            $(".open_arrow").css("display", "block");
            if (threenode != null) {
                curenturl = threenode;
            }
            $("#frammain").attr("src", curenturl);
        }
    });
    $(".hishop_menu a:contains('" + firstnode + "')").addClass("hishop_curent").siblings().removeClass("hishop_curent");
}

//自适应高度
function AutoHeight() {
    var clientheight = $(this).height() - 57;
    var clientwidth = $(this).width();
    //$(".hishop_menu_scroll").height(clientheight -50);
    $(".hishop_content_r").height(clientheight);
}

//窗口变化
$(window).resize(function () {
    AutoHeight();
});

//窗口加载
$(function () {
    AutoHeight();
    $("#menu_left a").live("click", function () {
        $("#menu_left a").removeClass("curent");
        $(this).addClass("curent");
    });
    LoadTopLink();

});

function LoadTopLink() {
    $.ajax({
        url: 'LoginUser.ashx?action=login',
        async: true,
        cache: false,
        dataType: 'json',
        type: 'GET',
        timeout: 5000,
        error: function (xm, msg) {
            document.location.href = "/admin/login.aspx?error=" + msg;
        },
        success: function (siteinfo) {
            $(".hishop_banneritem .dropdown-toggle b").text(siteinfo.username);
            $("#taobao_link").attr("href", siteinfo.taobaourl);
            $(document).attr("title", siteinfo.sitename);
        }
    });
}


