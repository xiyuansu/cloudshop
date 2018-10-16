<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<Hi:Script ID="Script4" runat="server" Src="/utility/jquery.hishopUpload.user.js" />
<Hi:Script ID="Script3" runat="server" Src="/utility/jquery.form.js" />

<asp:Repeater ID="rp_orderItem" runat="server">
    <ItemTemplate>
        <div class="column">
            <div class="column1">
                <div class="pic">
                    <input type="hidden" runat="server" id="hdproductId" value='<%# Eval("ProductId")+"&"+Eval("SKU")+"&"+Eval("SKUid")%>' />
                    <Hi:ProductDetailsLink ID="ProductDetailsLink" runat="server" ProductName='<%# Eval("ItemDescription") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                        <Hi:ListImage ID="Common_ProductThumbnail1" Width="60px" Height="60px" runat="server" DataField="ThumbnailsUrl"/>
                    </Hi:ProductDetailsLink>
                </div>
                <div class="info">

                    <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ItemDescription") %>' ProductId='<%# Eval("ProductId") %>' runat="server" />

                    <em>
                        <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                    </em>

                </div>

                <div class="number" style="display: none;">
                    <asp:Literal ID="litSKU" runat="server" Text='<%# Eval("SKU") %>'></asp:Literal>
                </div>
            </div>
            <div class="column2" id="wordCount">
                <div class="grade-star">
                   <input type="hidden" runat="server" id="hidScore" />
                    <em>评分：</em>
                   <input id="input-2ba" type="number" class="rating" min="0" max="5" step="1" data-size="xs" data-stars=5 value="5" data-symbol="&#xe005;">
                </div>
                <div class="text">
                    <em>评论：</em>
                    <span>
                        <textarea id="txtcontent" rows="3" cols="33" runat="server"></textarea></span>
                    <b><var class="word">500</var>/500</b>
                </div>
                <div class="pic">
                    <span>
                        <div class="imageContainer">
                            <em>晒图：</em>
                            <span name="articleImage" class="imgbox"></span>
                            <span class="uploadImages">
                                <input type="hidden" runat="server" id="hidUploadImages" />
                            </span>
                        </div>
                    </span>
                </div>

            </div>
            <div class="column3"></div>
        </div>
    </ItemTemplate>
</asp:Repeater>




<script>
    $(function () {
        var max = 500;
        $('.column2').click(function () {
            var textArea = $(this).find("textarea");
            var word = $(this).find("var");
            var curLength;

            textArea[0].setAttribute("maxlength", max);
            curLength = textArea.val().length;
            word.text(parseInt(max - curLength));
            textArea.on('input propertychange', function () {
                if (max - $(this).val().length < 0) {
                    word.text(0);
                } else {
                    word.text(max - $(this).val().length);
                }
            });
        });
        initImageUpload();
        $(".rating-kv").rating();
    });
    // 初始化图片上传控件
    function initImageUpload() {
        $(".imageContainer").each(function () {
            $(this).find('span[name="articleImage"]').hishopUpload(
            {
                title: '',
                url: "/admin/UploadHandler.ashx?action=newupload",
                imageDescript: '',
                displayImgSrc: '',
                imgFieldName: "articleImage",
                defaultImg: '',
                pictureSize: '',
                imagesCount: 5,
                dataWidth: 4
            });
        });
    }

    //获取上传成功后的图片路径
    function getUploadImages() {
        $(".grade-star").each(function () {
            $(this).find("input").eq(0).val($(this).find(".rating").val());
        })
        $(".imageContainer").each(function () {
            var aryImgs = $(this).find('span[name="articleImage"]').hishopUpload("getImgSrc");
            var imgSrcs = "";
            $(aryImgs).each(function () {
                imgSrcs += this + ",";
            });
            $(this).find(".uploadImages input").val(imgSrcs); 

        });
        return true;
    }
</script>
<style>
    .upload-img-box {
        float: left;
        width: 50px;
        height: 50px;
        margin-left: 10px;
        margin-bottom: 5px; 
    }

    .upload-img-box img {
        width: 100%;
        height: 100%;
        border:none;
    }
.upload-img-box .glyphicon-plus::before{content:" ";}

    .upload-img-box .remove-img {
        position: absolute;
        top: 0px;
        right: 0px;
        display: none;
        width: 100%;
        height: 50px;
        line-height: 50px;
        text-align: center;
        cursor: pointer; background:url(/templates/master/default/images/users/new/upload-img2.png) no-repeat;
        z-index: 11;
    }

    .upload-img-box img:hover .remove-img {
        display: block;
    }

    .img-upload-btn {
        color: #fc9a18; background:url(/templates/master/default/images/users/new/upload-img.png) no-repeat;
        width: 50px;
        height: 50px;
        line-height:50px;
        text-align: center;
        font-size: 0px;
        border:none;
        cursor: pointer;
        top: 0; z-index: 9;
    }

    .img-upload-btn:hover,
    .img-upload-btn:focus {
        background-color: #fff;
    }
</style>
