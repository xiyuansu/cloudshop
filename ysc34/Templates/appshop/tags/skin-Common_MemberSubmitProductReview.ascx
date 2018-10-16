<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div style="float: left; width: 100%;" class="mainreview">
    <span class="ProductId">
        <input type="hidden" runat="server" id="hdproductId" value='<%# Eval("ProductId")+"&"+Eval("SKU")+"&"+Eval("SKUid")%>' /></span>
    <span class="SKUContent">
        <input type="hidden" runat="server" id="hidSKUContent" value='<%# Eval("SKUContent")%>' /></span>
    <div class="mreview">
        <Hi:ListImage ID="Common_ProductThumbnail1"  runat="server" DataField="ThumbnailsUrl" />
        <div class="mreview_r">
            <em>评分：</em>
            <span class="like">
                <input type="hidden" id="HidScore" />
                <input id="input-2ba" type="number" class="rating" min="0" max="5" step="1" data-size="xs" data-stars="5" value="5" data-symbol="&#xe005;">
            </span>

        </div>
    </div>
    <div class="mreview" id="wordCount">
        <textarea placeholder="请写下您对该宝贝的使用心得" id="txtcontent" class="ReviewText" runat="server" maxlength="500"></textarea>
        <span class="wordwrap color_9">500</span>
    </div>
    <div class="mreview clear">
        <h3 >晒图</h3>
        <div class="imageContainer" style="display: inline-block;">
            <span name="articleImage" class="imgbox"></span>
            <span class="uploadImages">
                <input type="hidden" runat="server" id="hidUploadImages" />
            </span>
        </div>
    </div>
</div>

