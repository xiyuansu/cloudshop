<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.UI.SaleSystem.CodeBehind" %>
<%@ Import Namespace="Hidistro.Entities.Promotions" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box goods-box-shopcart <%#Eval("IsValid").ToBool() ? "Invalidpro" : Eval("HasEnoughStock").ToBool()?"noStock":"" %>" style="margin-bottom: 0; padding-bottom: 0;">
    <div class="cartlist_left">
        <label class="label-checkbox item-content" id="ck_productId" runat="server">
            <input type="checkbox" onclick="changeBuyProduct()" id="ck_<%# Eval("SkuId") %>" name="ck_productId" value="<%# Eval("SkuId") %>" />
            <div class="item-media"><i class="icon icon-form-checkbox"></i></div>
        </label>
        <label class="label-checkbox item-content" id="lblNoStock" runat="server">
            <div class="item-media"><b>库存不足</b></div>
        </label>
        <label class="label-checkbox item-content" id="lblInValid" runat="server">
            <div class="item-media"><b>失效</b></div>
        </label>
    </div>
    <div class="info_border">
        <div class="info">
            <a href="<%# "ProductDetails.aspx?productId=" + Eval("ProductId")%>" class="cart_1">
                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl180" />
                <span class="info_b_pu" skuid="<%# Eval("SkuId")%>" <%#Eval("HasStore").ToBool() ? "":"style='display: none'"%>'>支持自提</span>
            </a>
            <div class="name font-xl bcolor">
                <a href="<%# "ProductDetails.aspx?productId=" + Eval("ProductId")%>">
                    <%# Eval("Name")%></a>
            </div>
            <div class="pro_num" id="divChangeAmount" runat="server">
                <div name="spSub" class="shopcart-add" skuid='<%# Eval("SkuId")%>'>
                    -
                </div>
                <input type="tel" class="ui_textinput" name="buyNum" value='<%# Eval("Quantity")%>'
                    skuid='<%# Eval("SkuId")%>' />
                <div name="spAdd" class="shopcart-minus" skuid='<%# Eval("SkuId")%>'>
                    +
                </div>
            </div>
            <div class="pro_num" id="divQuantity" runat="server">
                <input type="tel" class="ui_textinput" name="buyNum" value='<%# Eval("Quantity")%>' disabled="disabled"
                    skuid='<%# Eval("SkuId")%>' />
            </div>
            <div class="cart_sku">
                <asp:Literal ID="ltlSKUContent" runat="server"></asp:Literal>
            </div>
            <input name="skucontent" type="hidden" value="<%# Eval("SkuContent")%>" />
            <input name="promotionName" type="hidden" value="<%#Convert.ToString(Eval("PromotionName"))%>" />
            <input name="promotionShortName" type="hidden" value="<%# PromotionHelper.GetShortName((PromoteType)Eval("PromoteType")) %>" />
            <div class="price text-danger">
                ¥<span id="spanPrice<%# Eval("SkuId")%>"><%# Eval("AdjustedPrice", "{0:F2}")%></span>
            </div>
            <span class="cart_num">x<%# Eval("Quantity")%></span>
            <span class="icon_trash icon-icon_trash_48" onclick="deleteProducts('<%#Eval("SkuId") %>')"></span>
        </div>
        <div  class="divPhonePrice pd_0" <%# Eval("IsMobileExclusive").ToBool()? "style=\"display:block\"":"style=\"display:none\"" %>>
        <em></em>
        </div>
            <div class="info_b">
        <asp:Repeater ID="repProductGifts" runat="server">
            <ItemTemplate>
                <span class="info_b_gift"><em>赠</em>&nbsp;&nbsp;<%#Eval("Name").ToNullString().Length>18?Eval("Name").ToNullString().Substring(0,18)+"...": Eval("Name")%>&nbsp;<i gid='giftNum_<%# DataBinder.Eval((Container.Parent.NamingContainer.NamingContainer as RepeaterItem).DataItem, "SkuId") %>'>x<%#Eval("Quantity") %></i></span>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </div>    

</div>

