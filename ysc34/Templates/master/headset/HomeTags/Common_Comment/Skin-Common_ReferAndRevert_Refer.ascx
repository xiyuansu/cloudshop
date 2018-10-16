<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div>
    <div class="zx_pic60">
         <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40"/></div>
    <div class="zx_name">
        <span>
            <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                ProductId='<%# Eval("ProductId") %>' runat="server" /></span> <span><b>我的咨询：</b><%# Eval("ConsultationText") %></span>
    </div>
    <div class="zx_time">
        咨询于
        <Hi:FormatedTimeLabel ID="FormatedTimeLabel2" Time='<%# Eval("ConsultationDate") %>'
            runat="server"></Hi:FormatedTimeLabel></div>
    <div style="clear: both">
    </div>
</div>
