<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<div class="goods-box" >
    <a href='<%# (Eval("StoreId").ToInt()>0?"CountDownStoreProductsDetails.aspx":"CountDownProductsDetails.aspx")+"?countDownId=" + Eval("CountDownId")+"&storeId="+Eval("StoreId") %>'  class="left_img">
        <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl180" /></a>
    <div class="info">
        <div class="name font-xl">
            <a style="overflow:visible;" href="<%#(Eval("StoreId").ToInt()>0?"CountDownStoreProductsDetails.aspx":"CountDownProductsDetails.aspx")+"?countDownId=" + Eval("CountDownId")+"&storeId="+Eval("StoreId") %>"><%# Eval("ProductName") %></a>
        </div>
      <%--  <div class="intro font-m text-muted">
            <a href="<%# "CountDownProductsDetails.aspx?countDownId=" + Eval("CountDownId") %>"><%# Eval("ShortDescription")%></a>
        </div>--%>
        <div class="price" >
            ¥<%# Eval("CountDownPrice", "{0:F2}") %>
            <a href='<%# (Eval("StoreId").ToInt()>0?"CountDownStoreProductsDetails.aspx":"CountDownProductsDetails.aspx")+"?countDownId=" + Eval("CountDownId")+"&storeId="+Eval("StoreId") %>'>

                <input id="btnNow" type="button" value="去抢购" class="btn_danger" style=" color: #fff;float:right;"  runat="server"/>
                <input id="btnWill" type="button" value="即将开始" class="btn_success" style=" color: #fff;float:right;" runat="server"/>
                <input id="btnOver" type="button" value="已抢完" class="btn_end" style=" color: #fff;float:right;" runat="server"/>
            </a>
            

        </div>
    </div>

</div>

