<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
<div class="search1">
<input type="text" id="txt_Search_Keywords" class="search" placeholder="" />
<input type="button" value="" onclick="searchs()" class="btn1" />
</div>
<div class="hot-key">
    <em class="fl block">热门关键字：</em>
    <Hi:Common_SubjectKeyword ID="Common_SubjectKeyword1" runat="server" CommentId="4" />
</div>
<script type="text/javascript">
    function searchs() {
        var item = $("#drop_Search_Class").val();
        var key = $("#txt_Search_Keywords").val();
        if (key == undefined)
            key = "";

        key = key.replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
        var url = "/SubCategory?keywords=" + encodeURIComponent(key);
        if (item != undefined)
            url += "&categoryId=" + item;
        window.location.href = url;
    }

    $(document).ready(function () {
        $('#txt_Search_Keywords').keydown(function (e) {
            if (e.keyCode == 13) {
                searchs();
                return false;
            }
        })
    });

</script>
