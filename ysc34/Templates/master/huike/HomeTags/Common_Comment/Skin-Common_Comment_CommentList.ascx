<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Context" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<!-- 第一层第一行[商品名称] -->
<div class="column">
    <div class="column1">
        <div class="pic">
            <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server" ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                        <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl100"/>
            </Hi:ProductDetailsLink>
        </div>
        <div class="name">
            <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                ProductId='<%# Eval("ProductId") %>' runat="server" />
        </div>
        <div class="size"><%# Eval("SKUContent") %></div>
    </div>

    <div class="column2">
        <div class="grade-star">
            <em>评分：</em>
            <input id="input-2ba" type="number" class="rating" min="0" max="5" step="1" data-size="xs" data-stars="5" value="<%# Eval("Score") %>" data-symbol="&#xe005;" readonly="readonly">
        </div>
        <div class="text">
            <em>评论：</em>
            <span>
                <asp:Label ID="Label7" runat="server" Text='<%# Eval("ReviewText") %>'></asp:Label></span>
        </div>
        <div class="pic">
        <div class="small_pic">
            <em>晒图：</em>
            <span class="after-service-img">
                <%# Eval("ImageUrl1").ToNullString()==""?"":"<img width=\"50\" height=\"50\" src=\"" + Globals.ApplicationPath + "/Admin/PicRar.aspx?P=" + Eval("ImageUrl1") + "&W=50&H=50\" bigsrc='" + Globals.ApplicationPath + Eval("ImageUrl1") + "' />" %>
                <%# Eval("ImageUrl2").ToNullString()==""?"":"<img width=\"50\" height=\"50\" src=\"" + Globals.ApplicationPath + "/Admin/PicRar.aspx?P=" + Eval("ImageUrl2") + "&W=50&H=50\" bigsrc='" + Globals.ApplicationPath + Eval("ImageUrl2") + "' />" %>
                <%# Eval("ImageUrl3").ToNullString()==""?"":"<img width=\"50\" height=\"50\" src=\"" + Globals.ApplicationPath + "/Admin/PicRar.aspx?P=" + Eval("ImageUrl3") + "&W=50&H=50\" bigsrc='" + Globals.ApplicationPath + Eval("ImageUrl3") + "' />" %>
                <%# Eval("ImageUrl4").ToNullString()==""?"":"<img width=\"50\" height=\"50\" src=\"" + Globals.ApplicationPath + "/Admin/PicRar.aspx?P=" + Eval("ImageUrl4") + "&W=50&H=50\" bigsrc='" + Globals.ApplicationPath + Eval("ImageUrl4") + "' />" %>
                <%# Eval("ImageUrl5").ToNullString()==""?"":"<img width=\"50\" height=\"50\" src=\"" + Globals.ApplicationPath + "/Admin/PicRar.aspx?P=" + Eval("ImageUrl5") + "&W=50&H=50\" bigsrc='" + Globals.ApplicationPath + Eval("ImageUrl5") + "' />" %>
            </span>
            </div>
            <div class="preview-img" style=" display: none;"><img src=""></div>
        </div>
        <%# Eval("ReplyText").ToNullString()==""?"":"<div class=\"reply\">商家回复："+Eval("ReplyText").ToNullString()+"</div>" %>
    </div>
    <div class="column3">
        <Hi:FormatedTimeLabel ID="FormatedTimeLabel" runat="server" Time='<%# Eval("ReviewDate") %>' />
    </div>
</div>



<!-- 评论内容 -->



