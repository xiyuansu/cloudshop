<%@ Page Language="C#" EnableViewState="false" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.CodeBehind" Assembly="Hidistro.UI.SaleSystem.CodeBehind" %>
<%
Response.Buffer = true;
Response.ExpiresAbsolute = DateTime.Now - new TimeSpan(1, 0, 0);
Response.Expires = 0;
Response.CacheControl = "no-cache";
%>  

<Hi:Default id="Default" runat="server" />

<script src="/Utility/jquery.scrollLoading.min.js"></script>
<script>
    $(function () {
        setScrollLoading();
    })

    function setScrollLoading() {      
        $("img").scrollLoading();
    }
</script>