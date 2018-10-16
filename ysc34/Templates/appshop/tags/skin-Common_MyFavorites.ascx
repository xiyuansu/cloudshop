<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box goods-box-shopcart mb_0">
    <div class="cartlist_left">
        <label class="label-checkbox item-content" id="ck_productId">
            <input type="checkbox" id="ck_<%# Eval("FavoriteId") %>" name="ck_FavoriteId" value="<%# Eval("FavoriteId") %>" />
            <div class="item-media"><i class="icon icon-form-checkbox"></i></div>
        </label>
    </div>
    <div class="info_border">
        <div class="info">
            <a href="<%# "ProductDetails.aspx?productId=" + Eval("ProductId")%>" class="cart_1">
                <Hi:ListImage runat="server" DataField="ThumbnailUrl100" />
            </a>
            <div class="name font-xl bcolor">
                <a href="<%# "ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                    <%# Eval("ProductName")%></a>
            </div>
            <div class="price text-danger">
                ¥  ¥<%# Eval("SalePrice", "{0:F2}")%>
            </div>

        </div>

    </div>
</div>

