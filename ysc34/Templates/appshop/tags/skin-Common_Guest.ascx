<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li class="mingoods">
    <div class="b_mingoods_wrapper">
        <a href="javascript:showProductDetail(<%# Eval("ProductId") %>)">
            <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl180" CustomToolTip="ProductName" />
        </a>
        <div class="text-ellipsis fs_12 pt_10">春装新款韩版中长款卫衣女学生开衫纯色显瘦连</div>        
        <span class="replace" style="text-align:center;">
            ￥<Hi:FormatedMoneyLabel Money='<%# Eval("SalePrice") %>' runat="server" CssClass="fs_14" />
        </span>
    </div>
</li>
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
