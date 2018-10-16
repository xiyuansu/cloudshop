<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li class="mingoods">
    <div class="b_mingoods_wrapper">
    <a href='<%#"ProductDetails.aspx?productId="+Eval("ProductId")%>'>
            <hi:hiimage runat="server" DataField="ThumbnailUrl220"></hi:hiimage>
         </a>
        <div class="text-ellipsis"><%# Eval("ProductName") %></div>
        <div class="replace"><span class="pirce">¥ <%# ((decimal)Eval("SalePrice")).F2ToString("f2") %></span></div>
   
        </div>
</li>
