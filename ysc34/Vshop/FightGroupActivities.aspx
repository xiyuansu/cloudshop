<%@ Page Language="C#" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.CodeBehind" Assembly="Hidistro.UI.SaleSystem.CodeBehind" %>

<Hi:FightGroupActivities ClientType="VShop" ID="vFightGroupActivities" runat="server" />

<script type="text/javascript">
    var pageIndex = 1;


    $(window).scroll(function () {
        var scrollTop = $(this).scrollTop();
        var scrollHeight = $(document).height() - 10;
        var windowHeight = $(this).height();

        if (scrollTop + windowHeight >= scrollHeight) {
            pageIndex++;
            loadFightGroupActivities();
        }
    });

    function loadFightGroupActivities() {
        $.ajax({
            type: "post",
            url: "/API/CouponHandler.ashx",
            data: { action: "FightGroupActivities", PageIndex: pageIndex },
            async: false,
            success: function (result) {
                $("div.fg_list ul").append(result);
            }
        });
    }



</script>
