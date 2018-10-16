<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.UI.SaleSystem.CodeBehind" %>
<%@ Import Namespace="Hidistro.Entities.Promotions" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Literal ID="ltlTop" runat="server" Text='<div class="suplist xuanzhongzt">'></asp:Literal>
    <div class="memtitle" id="divStoreTitle" runat="server" > 
         <div class="cartlist_left fleft">
             <label class="label-checkbox item-content"  runat="server">
                <input type="checkbox" class="store_listpro" />
                <div class="item-media"><i class="icon icon-form-checkbox"></i></div>
            </label>
        </div>
        <span class="title fleft"><%# Eval("StoreName")%></span>
    </div>

<div class="well goods-box goods-box-shopcart" style="margin-bottom: 0; padding-bottom: 0;">
    <div class="cartlist_left">
        <label class="label-checkbox item-content" id="ck_productId" runat="server">
            <input type="checkbox" data-protype="<%# Eval("StoreId") %>" id="ck_<%# Eval("SkuId") %>|<%# Eval("StoreId") %>" name="ck_productId" value="<%# Eval("SkuId") %>|<%# Eval("StoreId") %>" />
            <div class="item-media"><i class="icon icon-form-checkbox"></i></div>
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
            <div class="pro_num" id="divChangeAmount" runat="server">
                <div name="spSub" class="shopcart-add" skuid='<%# Eval("SkuId")%>|<%# Eval("StoreId") %>'>
                    -
                </div>
                <input type="tel" class="ui_textinput" name="buyNum" value='<%# Eval("Quantity")%>'
                    skuid='<%# Eval("SkuId")%>|<%# Eval("StoreId") %>' />
                <div name="spAdd" class="shopcart-minus" skuid='<%# Eval("SkuId")%>|<%# Eval("StoreId") %>'>
                    +
                </div>
            </div>
            <div class="cart_sku">
                <asp:Literal ID="ltlSKUContent" runat="server" Text='<%# Eval("SkuContent")%>'></asp:Literal>
            </div>
            <div class="price text-danger">
                ¥<span id="spanPrice<%# Eval("SkuId")%>|<%# Eval("StoreId") %>"><%# Eval("AdjustedPrice", "{0:F2}")%></span>
            </div>
            <span class="cart_num">x<%# Eval("Quantity")%></span>
            <span class="icon_trash icon-icon_trash_48" onclick="deleteProducts('<%#Eval("SkuId") %>|<%# Eval("StoreId")%>')"></span>
        </div>
        <div  class="divPhonePrice pd_0" <%# Eval("IsMobileExclusive").ToBool()? "style=\"display:block\"":"style=\"display:none\"" %>>
        <em></em>
        </div>
            <div class="info_b">
                <asp:Repeater ID="repProductGifts" runat="server">
            <ItemTemplate>
                <span class="info_b_gift"><em>赠</em>&nbsp;&nbsp;<%#Eval("Name").ToNullString().Length>18?Eval("Name").ToNullString().Substring(0,18)+"...": Eval("Name")%>&nbsp;<i gid='giftNum_<%# DataBinder.Eval((Container.Parent.NamingContainer.NamingContainer as RepeaterItem).DataItem, "SkuId") %>|<%# DataBinder.Eval((Container.Parent.NamingContainer.NamingContainer as RepeaterItem).DataItem, "StoreId") %>'>x<%#Eval("Quantity") %></i></span>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </div>   
</div>

<asp:Literal ID="ltlBottom" runat="server" Text='</div>'></asp:Literal>


