<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<div class="well goods-box goods-box-shopcart" style="margin-bottom:0;padding-bottom:0;">
    <div class="cartlist_left">
        <label class="label-checkbox item-content" id="ck_productId">
            <input type="checkbox" onclick="changeBuyProduct()"  id="ck_<%# Eval("ProductId") %>" name="ck_productId" productid="<%# Eval("ProductId") %>" <%#Convert.ToBoolean(Eval("HasSKU"))?"value=\"0\"":"value=\""+Eval("ProductId")+"_0\"" %>>
            <div class="item-media"><i class="icon icon-form-checkbox"></i></div>
            <input id="skustock_<%# Eval("ProductId") %>" value="<%# Eval("totalstock") %>" style="display:none"/>
        </label>
    </div>
    <div class="info_border">
        <div class="info">
            <a href="ProductDetails.aspx?ProductId=<%# Eval("ProductId") %>" class="cart_1">
                <img src="<%#string.IsNullOrEmpty(Eval("ThumbnailUrl180").ToNullString())?Globals.GetImageServerUrl("http://", Hidistro.Context.HiContext.Current.SiteSettings.DefaultProductImage):Globals.GetImageServerUrl("http://",Eval("ThumbnailUrl180").ToNullString()) %>">
            </a>
            <div class="name font-xl bcolor">
                <a href="ProductDetails.aspx?ProductId=<%# Eval("ProductId") %>"><%# Eval("ProductName") %></a>
            </div>
           
            <div class="price text-danger">
                ¥<span id="combinaprice_<%# Eval("ProductId") %>"><%# Eval("minCombinationPrice") %></span>
                <span id="saleprice_<%# Eval("ProductId") %>" style="display: none"><%# Eval("minSalePrice") %></span>
            </div>
            <span class="cart_num">x1</span>
        </div>
        
         <div class="cart_sku" onclick="showsku(<%# Eval("ProductId") %>)" <%#Convert.ToBoolean(Eval("HasSKU"))?"style=\"display:block\"":"style=\"display:none\"" %> id="divspecification_<%# Eval("ProductId") %>"><em>选择规格</em><i></i></div>
         
    </div>
</div>
<asp:Panel ID="Panelsku" runat="server"></asp:Panel>

