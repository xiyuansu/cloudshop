<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box goods-box-shopcart mb_0">
    <div class="cartlist_left">
        <label class="label-checkbox item-content" id="ck_productId">
            <input type="checkbox" id="ck_<%# Eval("FavoriteId") %>" name="ck_FavoriteId" value="<%# Eval("FavoriteId") %>" onclick="getSelectALLChecked()" />
            <div class="item-media"><i class="icon icon-form-checkbox"></i></div>
        </label>
    </div>
    <div class="info_border">
        <div class="info">
            <a href="<%# Eval("ProductType").ToInt()==1?("ServiceProductDetails.aspx?StoreId="+Eval("StoreId")+"&ProductId=" + Eval("ProductId")):(Eval("StoreId").ToInt()==0?"ProductDetails.aspx":"StoreProductDetails.aspx")+"?productId=" + Eval("ProductId")+"&StoreId="+Eval("StoreId")%>" class="cart_1">
                <Hi:ListImage runat="server" DataField="ThumbnailUrl100" />
            </a>
            <div class="name font-xl bcolor">
                <a href="<%# Eval("ProductType").ToInt()==1?("ServiceProductDetails.aspx?StoreId="+Eval("StoreId")+"&ProductId=" + Eval("ProductId")):(Eval("StoreId").ToInt()==0?"ProductDetails.aspx":"StoreProductDetails.aspx")+"?productId=" + Eval("ProductId")+"&StoreId="+Eval("StoreId")%>">
                    <%# Eval("ProductName")%></a>
            </div>
            <div class="price text-danger">
                 ¥<%# Eval("SalePrice", "{0:F2}")%>
            </div>

        </div>

    </div>
</div>

