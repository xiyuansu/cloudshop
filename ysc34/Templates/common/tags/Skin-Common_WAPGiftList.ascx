<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<div class="well goods-box goods-box-shopcart" style="margin-bottom: 0; padding-bottom: 0;">
    <div class="cartlist_left">
        <label class="label-checkbox item-content" id="ck_giftId" runat="server">
            <input readonly="readonly" type="checkbox" checked="checked"  name="ck_giftId"/>
            <div class="item-media"><i class="icon icon-form-checkbox"></i></div>
        </label>
    </div>
    <div class="info_border">
        <div class="info">
            <a href="#" class="cart_1">
                <Hi:HiImage runat="server" ID="imgGift" DataField="ThumbnailUrl180" />
            </a>
            <div class="name font-xl bcolor">
                <a href="#"><%# Eval("Name") %></a>
            </div>

            <div class="pro_num">
                <div name="spGiftSub" class="shopcart-add">
                    -
                </div>
                <input type="tel" class="ui_textinput" name="buyGiftNum" giftid="<%# Eval("GiftId")%>" needpoints="<%# Eval("NeedPoint") %>" value='<%# Eval("Quantity")%>' />
                <div name="spGiftAdd" class="shopcart-minus">
                    +
                </div>
            </div>
            <span class="icon_trash icon-icon_trash_48" onclick="onGiftClick(this,<%# Eval("GiftId")%>)"></span>
            <div class="price text-danger">
                <%# Eval("NeedPoint") %>积分
            </div>
            <span class="cart_num">x<%# Eval("Quantity")%></span>
        </div>
    </div>
</div>
