<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>

<!--右侧贴边导航quick_links.js控制-->

<div class="J-global-toolbar">
    <div class="toolbar-wrap J-wrap">
        <div class="toolbar">
            <div class="toolbar-panels J-panel">

                <div style="visibility: hidden;" class="J-content toolbar-panel tbar-panel-history toolbar-animate-in">
                    <h3 class="tbar-panel-header J-panel-header">
                        <em class="title"><a href="/ShoppingCart.aspx" target="_blank">可领用优惠券</a></em>

                    </h3>
                    <div class="tbar-panel-main">
                        <div class="tbar-panel-content J-panel-content">
                            <div class="jt-history-wrap">
                                <ul></ul>
                            </div>
                        </div>
                    </div>
                    <div class="tbar-panel-footer J-panel-footer"></div>
                </div>
            </div>

            <div class="toolbar-header"></div>

            <div class="toolbar-tabs J-tab">
                <div class="toolbar-tab tbar-tab-cart1" id="shopCart">
                    <a href="/user/UserDefault.aspx">
                        <i class="tab-ico">
                            <img src="/templates/pccommon/images/new/icon_03.png" /></i>
                        <em class="tab-text ">用户中心</em>
                    </a>
                </div>
                <div class=" toolbar-tab tbar-tab-follow1">
                    <a href="/ShoppingCart.aspx" target="_blank">
                        <i class="tab-ico">
                            <img src="/templates/pccommon/images/new/icon_06.png" /></i>
                        <em class="tab-text">购物车</em>
                    </a>
                </div>
                <div class=" toolbar-tab tbar-tab-history" onclick="loadCoupons();">
                    <i class="tab-ico">
                        <img src="/templates/pccommon/images/new/icon_08.png" /></i>
                    <em class="tab-text coupon2">优惠券</em>

                </div>
                <div class=" toolbar-tab tbar-tab-appcoupons" id="divappdownload" runat="server" clientidmodel="static">
                    <i class="tab-qrcode">
                        <%--<Hi:Common_AppDownloadCouponInfo ID="appDownloadCouponInfo" runat="server" />--%>
                        <img src="/templates/pccommon/images/new/Layer.png" />
                    </i>
                    <em class="tab-text coupon3"><Hi:Common_AppDownloadCouponInfo ID="Common_AppDownloadCouponInfo2" ShowText="true" Width="110" runat="server" /></em>

                </div>
            </div>

            <div class="toolbar-footer">
                <div class="toolbar-tab tbar-tab-top">
                    <a id="returnTop">
                        <i class="tab-ico  ">
                            <img src="/templates/pccommon/images/new/jiantou.png" /></i> <em class="footer-tab-text">顶部</em> </a>
                </div>
            </div>
            <div class="toolbar-mini"></div>
        </div>
        <div id="J-toolbar-load-hook"></div>
    </div>
</div>
<div class="login_tan" id="DivLogin" style="display: none;">

    <div class="dialog_title">
        <div class="dialog_title_l"><span>您尚未登录，请登录</span></div>
        <div class="dialog_title_r" id="imgCloseLogin1"></div>
    </div>
    <div class="dialog_cartitem">
        <input type="text" name="textfield" id="textfieldusername_right" placeholder="邮箱/手机号" class="login_tan_input1" />
        <input type="password" name="textfield2" id="textfieldpassword_right" placeholder="密码" class="login_tan_input2" />
        <div class="info">
            <a href="/register.aspx" class="register">我要注册</a>
            <a href="/ForgotPassword.aspx">忘记密码?</a>

        </div>
        <div class="btnbar">
            <input type="button" name="button" id="btnLogin" value="登  录 " class="login_tan_input3" />
        </div>
    </div>
</div>
<input type="hidden" id="lockSend" value="0" />

<div id="flyItem" class="fly_item">
    <img src="/templates/pccommon/images/new/youhuiquan.png" width="37" height="22">
</div>
<script type="text/javascript" src="/templates/pccommon/script/parabola.js"></script>
<script>
</script>
<script>
    var isloadedCoupons = false;
    var bg = "<div class='modal_qt'></div>";

    var wantCouponId = 0;
    // 元素以及其他一些变量
    var eleFlyElement = document.querySelector("#flyItem"), eleShopCart = document.querySelector("#shopCart");
    var numberItem = 0;
    // 抛物线运动
    var myParabola = funParabola(eleFlyElement, eleShopCart, {
        speed: 400, //抛物线速度
        curvature: 0.0008, //控制抛物线弧度
        complete: function () {
            eleFlyElement.style.visibility = "hidden";
            //eleShopCart.querySelector("span").innerHTML = ++numberItem;
        }
    });
    $(document).ready(function () {
        //如果没有APP下载二维码，则不显示小图标
        if ($(".coupon3").text().length == 0) {
            $(".tab-qrcode").parent().hide();
        }
        $("#lockSend").val("0");
        $('#returnTop').click(function () { $('body,html').animate({ scrollTop: 0 }, 1000) });
        $("#btnLogin").bind("click", function () {
            Login();
        });
        $("#imgCloseLogin1").bind("click", function () {
            $("#DivLogin").hide();
            $(".modal_qt").remove();
        });
    });

    // 加载优惠券
    function loadCoupons() {
        if (isloadedCoupons) return;
        isloadedCoupons = true;
        $('.jt-history-wrap ul').empty();
        var currenturl = window.location.href;
        currenturl = currenturl.toLowerCase();

        var productId = '0';
        var IsGroup = "false";
        var IsPanicBuying = "false";

        if (currenturl.indexOf('/product_detail') > -1) {
            productId = currenturl.substring(currenturl.indexOf('product_detail') + 15).replace('.aspx', '');
        }
        if (currenturl.indexOf('/productdetail') > -1) {
            productId = getParam("productId");
        }
        if (currenturl.indexOf('/groupbuyproductdetails') > -1) {
            IsGroup = "true";
            productId = getParam("groupBuyId");
        }
        if (currenturl.indexOf('/countdownproductsdetails') > -1) {
            IsPanicBuying = "true";
            productId = getParam("countDownId");
        }
        if (currenturl.indexOf('/groupbuyproduct') > -1) {
            IsGroup = "true";
        }
        if (currenturl.indexOf('/countdownproduct') > -1) {
            IsPanicBuying = "true";
        }

        $.ajax({
            url: "/Handler/RegionHandler.ashx",
            type: 'post', dataType: 'json', timeout: 1000,
            data: { action: "ShowCoupons", productId: productId, IsGroup: IsGroup, IsPanicBuying: IsPanicBuying },
            success: function (resultData) {
                $('.jt-history-wrap ul').empty();
                if (resultData.TotalRecords == 0) {
                    $('.jt-history-wrap ul').append("<span>暂无可领取的优惠券</span>");

                } else {
                    $('.coupon2').html("优惠券可领");
                    if (resultData.Data) {
                        for (var i = 0; i < resultData.Data.length; i++) {
                            $('.jt-history-wrap ul').append("<li id=\"" + resultData.Data[i].LiId + "\" class=\"" + resultData.Data[i].LiId + "\"><div class=\"top_line\"></div><div><a href=\"javascript:;\"><div class=\"price\"><em>¥</em> <b>" + resultData.Data[i].Price + "</b></div><span class=\"line\">" + resultData.Data[i].CanUseProducts + "</span><span class=\"color\">" + resultData.Data[i].OrderUseLimit + "</span><span class=\"color1\">" + resultData.Data[i].SCTime + "</span><span id=\"act-val_" + resultData.Data[i].CId + "\" class=\"act-val btnCart\" cid=\"" + resultData.Data[i].CId + "\"><em>立即领取</em></span></a></div><div class=\"bottom_line\"></div></li>");
                        }
                    }
                    if (eleFlyElement && eleShopCart) {
                        [].slice.call(document.getElementsByClassName("btnCart")).forEach(function (button) {
                            button.addEventListener("click", function (event) {
                                SendCoupon($(this).attr("cid"), event);
                            });
                        });
                    }
                    //获取优惠券的个数
                    var Length = $('.jt-history-wrap ul li').length;
                    if (Length >= 0) {
                        $('.tbar-tab-history').addClass('tbar-tab-click-selected');
                        $('.coupon2').addClass('tbar-tab-hover');
                    }

                    $('.top_line').first().addClass('top_line_f').removeClass('top_line');
                    var num = 0;
                    num = $('.bottom_line').size();
                    if (num == 1) {
                        $('.bottom_line').first().addClass('top_line_l').removeClass('bottom_line');
                    }
                    else {
                        $('.bottom_line').last().addClass('top_line_l').removeClass('bottom_line');
                    }
                }

            }
        });
    }

    function SendCoupon(CouponId, event) {
        if ($("#lockSend").val() == 0) {
            $("#lockSend").val("1");
            $.ajax({
                url: "/Handler/RegionHandler.ashx",
                type: 'post', dataType: 'json', timeout: 1000,
                data: { action: "SendCoupons", CouponId: CouponId },
                success: function (resultData) {
                    $("#lockSend").val("0");
                    if (resultData.Status == "0") {
                        wantCouponId = CouponId;
                        $("#DivLogin").show();
                        $('body').append(bg);
                    } else if (resultData.Status == "1" || resultData.Status == "2") {
                        // 滚动大小
                        var scrollLeft = document.documentElement.scrollLeft || document.body.scrollLeft || 0,
                            scrollTop = document.documentElement.scrollTop || document.body.scrollTop || 0;
                        eleFlyElement.style.left = event.clientX + scrollLeft + "px";
                        eleFlyElement.style.top = event.clientY + scrollTop + "px";
                        eleFlyElement.style.visibility = "visible";

                        // 需要重定位
                        myParabola.position().move();
                        if (resultData.Status == "1")
                            $("#act-val_" + CouponId).parents("li").remove();
                    } else {
                        alert(resultData.Error);
                        $("#act-val_" + CouponId).parents('li').remove();
                    }
                    if ($('.jt-history-wrap ul li').length == 0)
                        $('.jt-history-wrap ul').append("<span>暂无可领取的优惠券</span>");
                }
            });


        }
    }
    // 登录后再获取
    function Login() {

        var username = $("#textfieldusername_right").val();
        var password = $("#textfieldpassword_right").val();
        var thisURL = document.URL;

        if (username.length == 0 || password.length == 0) {
            ShowMsg("请输入您的用户名和密码!", false);
            return;
        }

        $.post("/User/Login", { username: username, password: password, action: "Common_UserLogin" },
          function (data) {
              if (data.Status == "Succes") {
                  window.location.reload();
              }
              else {
                  ShowMsg(data.Msg, false);
              }
          }, "json");

    }

    $(function () {
        //如果没有APP下载二维码则不弹出
        if ($(".coupon3").text() != "") {
            $(".coupon3").addClass("tbar-tab-hover");
            setTimeout(function () {
                $(".coupon3").removeClass("tbar-tab-hover")
            }, 10000);
        }
    })
</script>


