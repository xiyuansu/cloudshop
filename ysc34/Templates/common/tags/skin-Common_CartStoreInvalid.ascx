<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.UI.SaleSystem.CodeBehind" %>
<%@ Import Namespace="Hidistro.Entities.Promotions" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box goods-box-shopcart <%#Eval("IsValid").ToBool() ? "noStock" : "Invalidpro" %>" style="margin-bottom: 0; padding-bottom: 0;">
    <div class="cartlist_left">
        <label class="label-checkbox item-content">
            <div class="item-media"><b><%#Eval("IsValid").ToBool() ? "库存不足" : "失效" %></b></div>
        </label>
    </div>
    <div class="info_border">
        <div class="info">
            <a href="<%# (Eval("StoreId").ToInt()>0?"StoreProductDetails.aspx":"ProductDetails.aspx")+"?productId=" + Eval("ProductId")+"&StoreId="+Eval("StoreId")%>" class="cart_1">
                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl180" />
                 <%#Eval("HasStore").ToBool() ? "<span class=\"info_b_pu\">支持自提</span>" : "" %>
            </a>
            <div class="name font-xl bcolor">
                <a href="<%# (Eval("StoreId").ToInt()>0?"StoreProductDetails.aspx":"ProductDetails.aspx")+"?productId=" + Eval("ProductId")+"&StoreId="+Eval("StoreId")%>">
                    <%# Eval("Name")%></a>
            </div>
            <div class="pro_num" id="divQuantity" runat="server">
                <input type="tel" class="ui_textinput" name="buyNum" value='<%# Eval("Quantity")%>' disabled="disabled"
                    skuid='<%# Eval("SkuId")%><%# Eval("StoreId")%>' />
            </div>
            <div class="cart_sku">
                <asp:Literal ID="ltlSKUContent" runat="server" Text='<%# Eval("SkuContent")%>'></asp:Literal>
            </div>
            <div class="price text-danger">
                ¥<span id="spanPrice<%# Eval("SkuId")%><%# Eval("StoreId")%>"><%# Eval("AdjustedPrice", "{0:F2}")%></span>
            </div>
            <span class="cart_num">x<%# Eval("Quantity")%></span>
            <span class="icon_trash icon-icon_trash_48" onclick="deleteProducts('<%#Eval("SkuId") %>|<%# Eval("StoreId")%>')"></span>
        </div>
        <div  class="divPhonePrice pd_0" <%# Eval("IsMobileExclusive").ToBool()? "style=\"display:block\"":"style=\"display:none\"" %>>
        <em></em>
        </div>
    </div>    

</div>

