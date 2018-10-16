<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div style=" margin: 15px; float: left; width:930px;">
    <div class="zx_pic60">
        <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
    </div>
    <div class="zx_name">
        <span style=" height: 25px; overflow: hidden;">
            <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                ProductId='<%# Eval("ProductId") %>' runat="server" /></span> <span><b>我的咨询：</b><em><%# Eval("ConsultationText") %></em></span>
    </div>
    <div class="zx_time">
        咨询于
        <Hi:FormatedTimeLabel ID="FormatedTimeLabel2" Time='<%# Eval("ConsultationDate") %>'
            runat="server"></Hi:FormatedTimeLabel>
    </div>

</div>
<div style="clear: both">
</div>
<div class="uesr_reply">

    <div>
        <span><b>管理员回复</b><p><%# Eval("ReplyText") %></p>
        </span>
    </div>
    <div class="zx_time">
        回复于
                <Hi:FormatedTimeLabel ID="FormatedTimeLabel1" Time='<%# Eval("ReplyDate") %>' runat="server"></Hi:FormatedTimeLabel>
    </div>

</div>
<div style="clear: both">
</div>
