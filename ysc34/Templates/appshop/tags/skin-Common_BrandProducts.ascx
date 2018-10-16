<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a href="javascript:showProductDetail('<%#Eval("ProductId") %>')">
    <div class="well goods-box">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl100" />
        <div class="info">
            <div class="name font-xl">
                <%# Eval("ProductName") %></div>
            <div class="price text-danger">
                ¥<%# Eval("SalePrice", "{0:F2}") %><span class="sales font-s text-muted">已售<%#Eval("ShowSaleCounts")%>件</span></div>
        </div>
    </div>
</a>
<script type="text/javascript">
    function showProductDetail(id) {
        var type = GetAgentType();
        // 设置标题
        if (type == 2)
            window.HiCmd.webShowProduct(id);
        else if (type == 1)
            loadIframeURL("hishop://webShowProduct/null/" + id);
    }
</script>