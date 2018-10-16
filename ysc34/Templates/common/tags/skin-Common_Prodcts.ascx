<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li class="col-xs-6">
    <a href="<%# Eval("ProductType").ToInt()==1?("ServiceProductDetails.aspx?StoreId="+Eval("StoreId")+"&ProductId=" + Eval("ProductId")):Eval("StoreId").ToString()==""?("ProductDetails.aspx?ProductId=" + Eval("ProductId")):("StoreProductDetails.aspx?StoreId="+Eval("StoreId")+"&ProductId=" + Eval("ProductId")) %>">
        <div class="goods-list-pic col-xs-6 clearfix">
            <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl410" />
        </div>
    </a>
    <div class="info">
        <a href="<%# Eval("ProductType").ToInt()==1?("ServiceProductDetails.aspx?StoreId="+Eval("StoreId")+"&ProductId=" + Eval("ProductId")):Eval("StoreId").ToString()==""?("ProductDetails.aspx?ProductId=" + Eval("ProductId")):("StoreProductDetails.aspx?StoreId="+Eval("StoreId")+"&ProductId=" + Eval("ProductId")) %>">
            <div class="name bcolor"><%# Eval("ProductName") %></div>
        </a>
        <div class="price font-s text-danger">
            ￥<%#Eval("SalePrice","{0:F2}") %>
        </div>
        <div class="sales text-muted font-xs">已售<%# Eval("SaleCounts")%>件<i productid="<%# Eval("ProductId")%>" onclick="<%# Eval("ProductType").ToInt()==1?"serviceProductHref(this);":"AddToCart(this);" %>" StoreId="<%# Eval("StoreId")%>" class="btnAddToCart-1"></i></div>
    </div>
</li>

