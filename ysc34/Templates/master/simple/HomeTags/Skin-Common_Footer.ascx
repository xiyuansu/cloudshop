<%@ OutputCache Duration="600" VaryByParam="None" %>
<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
<div class="footer" id="footer">
	<div class="footer_top w950 m0"><hi:common_imagead runat="server" adid="18"></hi:common_imagead></div>
    <div class="footer_mid">
    	<div class="w1000 m0" style="position:relative;">
        	<div class="w950 m0" style=" overflow:hidden;">
                <ul>
                    <Hi:Common_Help runat="server" TemplateFile="HomeTags/Common_Comment/Skin-Common_Help.ascx" />
                </ul>
                <div class="erweima">
                    <hi:common_customad runat="server" adid="17"></hi:common_customad>
                </div>
            </div>
        </div>
    </div>
    <div class="footer_bot">
    	<div>
            <Hi:PageFooter ID="PageFooter1" runat="server" />
        </div>
        <div>
            <Hi:CnzzShow ID="CnzzShow1" runat="server" />
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
</script>
</body>
</html> 