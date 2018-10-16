<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li style="padding-bottom: 35px;">
    <a id="lnkProductReview" runat="server" class="detailLink">
        <Hi:HiImage runat="server" DataField="ThumbnailsUrl" />
    </a>
    <div class="step2_center">
        <h3><a class="detailLink text-ellipsis" id="hylinkProductName"><%# Eval("GiftName") %></a></h3>
        <span>
            <asp:Literal ID="ltlSKUContent" runat="server"></asp:Literal></span>
    </div>
    <span class="step2_num">×
                   <%# Eval("Quantity") %>
    </span>
    <div class="refund">
        <a runat="server" id="lkbtnApplyForReturn" visible="false" class='btn_custom btn_default_m'>申请退货</a>
        <a runat="server" id="lkbtnSendGoodsForReturn" visible="false" class='btn_custom btn_default_m'>退货发货</a>
        <a href="#" onclick="ViewMessage(this)" runat="server" id="lkbtnViewMessage" class='btn_custom btn_default_m' visible="false">查看信息</a>
        <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" class='btn_custom btn_default_m' onclick="GetLogisticsInformation(this)">物流跟踪</a></asp:Label>
    </div>
</li>
