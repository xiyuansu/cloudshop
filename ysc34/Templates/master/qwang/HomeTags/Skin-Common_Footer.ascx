<%@ OutputCache Duration="600" VaryByParam="None" %>
<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
<div class="footer" id="footer">
    <div class="footer-nav">
        <hi:common_help runat="server" templatefile="/HomeTags/Common_Comment/Skin-Common_Help.ascx" />
    </div>

    <div class="bottom">
       <div class="footer_link">
        <Hi:Common_FriendLinks runat="server" TemplateFile="/HomeTags/Common_Comment/Skin-Common_FriendLinks.ascx" />
    </div>
    <div class="Copyright">
        <Hi:PageFooter ID="PageFooter1" runat="server" />
         <Hi:CnzzShow ID="CnzzShow1" runat="server" />
         </div>
    </div><!--bottom结束-->
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