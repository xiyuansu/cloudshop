<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Panel ID="pnlShopGiftCart" runat="server">
    <div class="title">
        <div class="select" style="width: 60px;">
            &nbsp;
        </div>
        <div class="name"><em>礼品</em></div>
        <div class="price">所需积分</div>
        <div class="num">数量</div>
        <div class="total">操作</div>
    </div>
    <div class="list">
        <asp:Repeater ID="dataListGiftShoppingCrat" runat="server">
            <ItemTemplate>
                <div class="floor" id='ck_<%#Eval("GiftId") %>'>
                    <div class="con">
                        <div class="select" style="width: 30px;">
                            &nbsp;
                        </div>
                        <div class="name">
                            <div class="cart_commodit_span">
                                <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" title='<%#Eval("Name") %>'>
                                    <Hi:ListImage DataField="ThumbnailUrl100" runat="server" />
                                </a>
                            </div>
                            <div class="info">
                                <div class="cart_commodit_name">
                                    <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" title='<%#Eval("Name") %>'><%# Eval("Name") %></a>
                                </div>
                            </div>
                        </div>
                        <div class="price"><span class="cart_commodit_price"><%# Eval("NeedPoint")%></span></div>

                        <div class="num">
                            <div class="info1">
                                <asp:Literal runat="server" ID="litGiftId" Text='<%# Eval("GiftId")%>' Visible="false"></asp:Literal>
                                <span t="cg" m="gminus" class="num_minus">-</span>

                                <asp:TextBox runat="server" ID="txtGiftBuyNum" Text='<%# Eval("Quantity")%>' oldNumb='<%# Eval("Quantity")%>' inputTagID='<%# Eval("GiftId")%>' NeedPoint='<%# Eval("NeedPoint")%>'
                                    CssClass="cart_txtbuynum" onblur="autoUpdateGiftNum(this)" />

                                <span t="cg" m="gadd" class="num_add">+</span>
                            </div>
                        </div>
                        <div class="total">
                            <button type="button" id="delCartGiftItem" class="cart_commodit_del" value="<%# Eval("GiftId")%>" onclick='removeCartGiftItem(<%# Eval("GiftId")%>)'>删除</button>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Panel>
<script type="text/javascript">
    $(document).ready(function () {

        //点击添加购买数量
        $("span[m=gadd]").click(function () {
            var txtBuyNum = $($(this).parent().find(":text[id*=txtGiftBuyNum]").get(0));
            var currentNum = parseInt(txtBuyNum.val());
            txtBuyNum.val(currentNum + 1);
            ChangeGiftNum(txtBuyNum);
        });

        //点击减少购买数量
        $("span[m=gminus]").click(function () {
            var txtBuyNum = $($(this).parent().find(":text[id*=txtGiftBuyNum]").get(0));
            var currentNum = parseInt(txtBuyNum.val());
            if (currentNum == 1) {
                return;
            }
            txtBuyNum.val(currentNum - 1);
            ChangeGiftNum(txtBuyNum);
        });

        $("input").each(function (i, obj) {
            var inputTagObj = $(this).attr("inputTagID");
            if (inputTagObj) {
                //按下回车键
                $(this).keydown(function (obj) {
                    var key = window.event ? obj.keyCode : obj.which;
                    if (key == 13) {
                        $("input").each(function (i, submitObj) {
                            var submitTagObj = $(submitObj).attr("SubmitTagID");
                            if (submitTagObj == inputTagObj) { $(submitObj).focus(); }
                        })
                    }
                })
                //失去焦点
                $(this).blur(function (obj) {
                    $("input").each(function (i, submitObj) {
                        var submitTagObj = $(submitObj).attr("SubmitTagID");
                        if (submitTagObj == inputTagObj) { $(submitObj).focus(); }
                    })
                })
            }
        })
    });

    function removeCartGiftItem(giftId) {
        if (confirm('确定删除？')) {
            $.ajax({
                url: "API/ShoppingCartHandler.ashx",
                type: "post",
                dataType: "json",
                timeout: 10000,
                data: { action: 'deleteGift', giftId: giftId },
                async: false,
                success: function (data) {
                    if (data.status == 'true') {
                        $("#ck_" + giftId).remove();
                        var GiftsNum = $("#ShoppingCart_hidPointGifts").val();
                        if (parseInt(GiftsNum) && parseInt(GiftsNum) > 0)
                            $("#ShoppingCart_hidPointGifts").val(GiftsNum - 1);
                        clearCart();
                    }
                }
            });
        }
    }

    //数量输入框失去焦点后自动更新
    function autoUpdateGiftNum(el) {
        var txtBuyNum = $(el);
        if (isNaN(txtBuyNum.val())) {
        	ShowMsg("请输入数字",false);
            txtBuyNum.val(txtBuyNum.attr('oldNumb')); return;
        }
        if (txtBuyNum.val().indexOf('.') > -1 || txtBuyNum.val().indexOf('-') > -1) {
        	ShowMsg("购买数量必须为正整数",false);
            txtBuyNum.val(txtBuyNum.attr('oldNumb')); return;
        }

        ChangeGiftNum(txtBuyNum);
    }

    function ChangeGiftNum(txtBuyNum) {
        var giftId, quantity;
        giftId = txtBuyNum.attr("inputTagID");
        quantity = txtBuyNum.val();
        $.ajax({
            url: "API/ShoppingCartHandler.ashx",
            type: "post",
            dataType: "json",
            timeout: 10000,
            data: { action: 'updateGiftBuyNum', giftId: giftId, BuyNum: quantity },
            async: false,
            success: function (data) {
                if (data.status == 'true') {
                    txtBuyNum.attr('oldNumb', txtBuyNum.val());
                } else if (data.status == 'numError') {
                	ShowMsg("购买数量必须为大于0的整数",false);
                    txtBuyNum.val(data.oldNumb);
                }
            }
        });
    }
</script>
