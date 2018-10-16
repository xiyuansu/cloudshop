<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<div class="well goods-box goods-box-shopcart" style="padding-left: 5px;">
    <a href="#">
        <Hi:HiImage runat="server" DataField="ThumbnailUrl180" />
    </a>
    <div class="info">
        <div class="name font-xl bcolor">
            <a href="#"><%# Eval("Name") %></a>
        </div>
        <div class="goods-num">
            <div name="spGiftSub" class="shopcart-add">
                -
            </div>
            <input type="tel" class="ui_textinput" name="buyGiftNum" giftid="<%# Eval("GiftId")%>" needpoints="<%# Eval("NeedPoint") %>" value='<%# Eval("Quantity")%>' />
            <div name="spGiftAdd" class="shopcart-minus">
                +
            </div>
           <span class="icon_trash icon-icon_trash_48" onclick="onGiftClick(this,<%# Eval("GiftId")%>)"></span>
        </div>
        <div class="price text-danger">
            <%# Eval("NeedPoint") %>积分
        </div>
    </div>
    <script type="text/javascript">
        
    </script>
