<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core.Urls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
    <div class="p-img"><a href='<%#"ProductDetails.aspx?productId="+Eval("ProductId")%>'><img src="<%# Eval("ThumbnailUrl220") %>" tempSrc="<%# Eval("ThumbnailUrl220") %>" id="mainPic_<%# Eval("ProductId") %>" /></a></div>
    <div class="p-scroll">
        <%--<a class="prev"> </a>
        <a class="next"></a>--%>
        <div class="items">
            <ul>
                <li class="cur"><img id='detailPic_<%# Eval("ProductId") %>_1' src="<%# Eval("ThumbnailUrl40") %>" onclick="changeMainPic(this)"></li>
                <%# string.IsNullOrEmpty(Eval("ImageUrl2").ToString())?"":"<li><img id='detailPic_"+Eval("ProductId")+"_2' onclick='changeMainPic(this)' src='"+Eval("ImageUrl2")+"'></li>" %>
                <%# string.IsNullOrEmpty(Eval("ImageUrl3").ToString())?"":"<li><img id='detailPic_"+Eval("ProductId")+"_3' onclick='changeMainPic(this)' src='"+Eval("ImageUrl3")+"'></li>" %>
                <%# string.IsNullOrEmpty(Eval("ImageUrl4").ToString())?"":"<li><img id='detailPic_"+Eval("ProductId")+"_4' onclick='changeMainPic(this)' src='"+Eval("ImageUrl4")+"'></li>" %>
                <%# string.IsNullOrEmpty(Eval("ImageUrl5").ToString())?"":"<li><img id='detailPic_"+Eval("ProductId")+"_5' onclick='changeMainPic(this)' src='"+Eval("ImageUrl5")+"'></li>" %>
            </ul>
        </div>
    </div>
    <div class="p_name"><a href='<%#"ProductDetails.aspx?productId="+Eval("ProductId")%>'><%# Eval("ProductName") %></a></div>
    <div class="p_price"><span><i>￥</i><b><%# ((decimal)Eval("SalePrice")).F2ToString("f2") %></b></span> <%--<em><a href="#">加入购物车</a></em>--%></div>
</li>