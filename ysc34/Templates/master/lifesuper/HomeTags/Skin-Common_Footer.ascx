<%@ OutputCache Duration="600" VaryByParam="None" %>
<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
<div class="footer pt20 both mt20" id="footer">
    <div class="w1200 o-hidden m0">
      <div class="link-help w1200 o-hidden pb20">
            <div class="footer-link fl o-hidden">
               <h2 class="yahei font14">友情链接</h2>
               <ul>
                  <Hi:Common_FriendLinks runat="server" TemplateFile="/HomeTags/Common_Comment/Skin-Common_FriendLinks.ascx" />
               </ul>
            </div>
            <div class="footer-help fr o-hidden">
                <h2 class="yahei font14">帮助中心</h2>
                <ul>
                   <hi:common_help runat="server" templatefile="/HomeTags/Common_Comment/Skin-Common_Help.ascx" />
                </ul>       
            </div>
       </div>  
        <div class="footer-custom pt20 both">
                <Hi:PageFooter ID="PageFooter1" runat="server" />
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