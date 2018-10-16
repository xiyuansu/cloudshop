<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Topics.aspx.cs" Inherits="Hidistro.UI.Web.Topics" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <title><%=pageTitle %></title>
    <meta http-equiv="content-script-type" content="text/javascript">
    <meta name="format-detection" content="telephone=no" />
    <meta name="screen-orientation" content="portrait">
    <meta name="x5-orientation" content="portrait">    
    <link rel="stylesheet" href="/Admin/PcShop/PublicMob/css/dist/style.css">
    <%--<link rel="stylesheet" href="../Templates/common/style/index.css">--%>
    <style>
        .top .top-search{
            overflow:inherit !important;
        }
        .top1_r ul li .cw-icon i{
            margin-top:0 !important;
        }      
        .nav-wrap,.top .top-search,.top .top-main{
            width:1180px !important;
        }
        .nav {
      position: inherit;
}
      
       
    </style>
</head>
<body>   
    <asp:Literal ID="litImageServerUrl" runat="server"></asp:Literal>
    <asp:Literal ID="MeiQia_OnlineServer" runat="server"></asp:Literal>
    <div class="membersbox pad50">       
        <asp:Panel ID="PanelTheme" runat="server"></asp:Panel>
    </div>
    <Hi:common_footer runat="server" />
    <script src="/Admin/Shop/PublicMob/plugins/swipe/swipe.js"></script>
    <script src="/Utility/jquery.scrollLoading.min.js"></script>
    <script type="text/javascript">

        $(function () {
            $(".members_con").first().css("margin-top", "0px");
            $(".members_imgad").first().css("margin-top","0px");
            $('.j-swipe').each(function (index, el) {
                var me = $(this);
                me.attr('id', 'Swiper' + index); setScrollLoading();
                var id = me.attr('id');
                // alert(id)
                var elem = document.getElementById(id);
                window.mySwipe = Swipe(elem, {
                    startSlide: 0,
                    auto: 3000,
                    callback: function (m) {
                        $(elem).find('.members_flash_time').children('span').eq(m).addClass('cur').siblings().removeClass('cur')
                    },
                });
            });

            ////图片比例处理
            var imgw = $('body').width();
            $(".members_goodspic ul .b_mingoods a ").width(imgw / 2 - 20);
            $(".members_goodspic ul .b_mingoods a ").height(imgw / 2 - 20);
            setScrollLoading();

            var hidBackgroundColor = $("#hidBackgroundColor").val();
            var hidBgAlign = $("#hidBgAlign").val() + " top";
            var hidFillingMethod = $("#hidFillingMethod").val();
            var hidBackgroundImg = $("#hidBackgroundImg").val();

            $("body").css({
                "background-color": hidBackgroundColor,
                "background-position": hidBgAlign,
                "background-image": "url(" + hidBackgroundImg + ")",
                "background-repeat": hidFillingMethod,
            })          

        });

        function setScrollLoading() {
            $("img").scrollLoading();
        }

    </script>

    <script src="Utility/traffic.js"></script>
</body>
</html>
