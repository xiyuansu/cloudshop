<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.UI.SaleSystem.CodeBehind" %>
<%@ Import Namespace="Hidistro.Entities.Promotions" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
    <a
        class="detailLink">
        <Hi:ListImage ID="ListImage2" runat="server" DataField="ThumbnailUrl40" /></a>
    <div class="step2_center">
        <h3><a
            class="detailLink text-ellipsis"><%# Eval("Name")%></a></h3>
        <span></span>
    </div>
    <span class="step2_price"><i><%# Eval("NeedPoint")%>积分</i> </span>
    <span class="step2_num">× <%# Eval("Quantity")%>
    </span>
</li>

