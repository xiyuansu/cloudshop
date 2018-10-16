<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>

<div class="footer" id="footer">
	<div class="footer_topad"><Hi:Common_ImageAd runat="server" AdId="35" /></div>
    <div class="footer_mid"><div class="ftmid_help"><ul><Hi:Common_Help runat="server" TemplateFile="/HomeTags/Common_Comment/Skin-Common_Help.ascx" /></ul></div><div class="ftmid_ad"><Hi:Common_ImageAd runat="server" AdId="36" /></div></div>
    <div class="footer_bot">
<div class="footer_bot_c">
        	<div class="ft_top"><Hi:Common_CustomAd runat="server" AdId="37" />
            </div>
            <div class="ft_mid">合作伙伴：
            <Hi:Common_FriendLinks runat="server" TemplateFile="/HomeTags/Common_Comment/Skin-Common_FriendLinks.ascx" />
            </div>
            <div class="ft_bot"><span><Hi:PageFooter ID="PageFooter1" runat="server" /></span>  <Hi:CnzzShow ID="CnzzShow1" runat="server" /></div>
            </div>

    </div>
</div>

<script src="/Utility/china.js"></script>
<script src="/Utility/jquery.cookie.js"></script>
<script type="text/javascript">
    var uid = 0;
    $(document).ready(function () {
        uid = parseInt($.cookie("uid"));
        if (isNaN(uid)) uid = 0;
        if (uid == 1) {
            uid ? (run()) : (run());
            $("#id_c").html("切换简体");
        }
    });
    $(document).bind('click', function (e) {
        var a = $(e.target).attr('id');
        if (a == 'id_c') {
            uid ? (run()) : (run());
            (uid == 1) ? (uid = 0, $('#id_c').html('切换繁体')) : (uid += 1, $('#id_c').html('切换简体'));
            $.cookie("uid", uid);
        }
    });
    $(function () {
        $(".dialog_title_r").click(function () {
            $("#loginForBuy").hide();
            $(".modal_qt").remove();
        });
    });
</script>
</body>
</html> 