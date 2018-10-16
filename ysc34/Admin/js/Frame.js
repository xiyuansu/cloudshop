// JavaScript Document
function CheckAuthorization(authstr) {
    //如果设置了权限，并且不是公共权限则判断当前权限是否需要显示
    if (authstr != undefined && authstr != null && authstr != "0") {
        var needshow = false;
        //只要需要的授权有一个授权了就需要显示
        //pc端
        if (authstr.indexOf("1") > -1 && OpenPcShop == 1) {
            needshow = true;
        }
        //微信端
        if (authstr.indexOf("2") > -1 && OpenVstore == 1) {
            needshow = true;
        }
        //触屏端
        if (authstr.indexOf("3") > -1 && OpenWap == 1) {
            needshow = true;
        }
        //生活号（原支付宝服务窗）
        if (authstr.indexOf("4") > -1 && OpenAliho == 1) {
            needshow = true;
        }
        //APP
        if (authstr.indexOf("5") > -1 && OpenMobbile == 1) {
            needshow = true;
        }
        //门店
        if (authstr.indexOf("6") > -1 && OpenMultiStore == 1) {
            needshow = true;
        }
        //供应商
        if (authstr.indexOf("7") > -1 && OpenSupplier == 1) {
            needshow = true;
        }
        //小程序
        if (authstr.indexOf("8") > -1 && OpenApplet == 1) {
            needshow = true;
        }
        //分销员
        if (authstr.indexOf("9") > -1 && OpenReferral == 1) {
            needshow = true;
        }
        //O2O小程序
        if (authstr.indexOf("a") > -1 && OpenWXO2OApplet == 1) {
            needshow = true;
        }
        return needshow;
    }
    return true;
}
//选择点击一级菜单显示
//菜单权限设置   1、PC端 2、微信端  3、触屏端  4、生活号（原支付宝服务窗）  5、APP  6、门店  7、供应商 8、小程序 9、分销员  0、共用页面 a、O2O小程序
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
                var authorization = $(this).attr("Authorization")
                if (CheckAuthorization(authorization)) {

                    var item_icon = $(this).attr("icon");
                    $menutitle = $('<div class="hishop_menutitle" style="position: relative;"></div>');
                    $menuicon = $('<img />');
                    $menutoogle = $('<div class="hishop_menutoogle"></div>');
                    if (Ispromote == "0") {
                       
                        if ($(this).attr("Title") == "商品配置") {
                            $menuaspan = $("<span>" + $(this).attr("Title") + "</span><a title=\"帮助文档\" href='http://download.92hi1.com/bangzhuzhongxin/changjianwenti/商品类型与商品分类的区别.pdf' target=\'_blank\' style='display: block;position: absolute;margin:0;top:0;left: 79px;border-left: none;'><img src='images/ic_query.png' /></a>"); //获取二级分类名称"
                        }
                        else {
                            $menuaspan = $("<span>" + $(this).attr("Title") + "</span>"); //获取二级分类名称"
                        }
                    }
                    else
                        $menuaspan = $("<span onclick=\"ShowMenuLeft('" + firstnode.replace(/\s/g, "") + "',null,null)\">" + $(this).attr("Title") + "</span>"); //获取二级分类名称"
                    $menutitle.append($menuicon);
                    //侧栏图标开始               
                    $menuicon.attr("src", "images/menuicon/" + item_icon + ".png");
                    $menutitle.append($menuaspan);
                    $menutitle.append($menutoogle);
                    if ((Ispromote == "1" && secondnode != null) || Ispromote == "0") {
                        $(this).find("PageLink").each(function (k) {
                            var authorization = $(this).attr("Authorization")
                            if (CheckAuthorization(authorization)) {
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
                            }
                        });
                    }
                    $("#menu_left").append($menutitle);
                }
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

    if ($.cookie("guide") == null || $.cookie("guide") == "undefined" || $.cookie("guide") != 1) {
        DialogFrame('help/index.html', '<font style="font-size:14px">新手向导</font>', 750, null);
    }
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


