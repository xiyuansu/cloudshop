<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Panel ID="pnlShopProductCart" runat="server">


    <div class="title">
        <div class="select">
            <input type="checkbox" checked="checked" id="checkall" class="icheck" onclick="jiesuan()" /><em>全选</em>
        </div>
        <div class="name"><em>商品</em></div>
        <div class="price">单价（元）</div>
        <div class="num">数量</div>
        <div class="total">操作</div>
    </div>
    <div class="list">
        <asp:Repeater ID="dataListShoppingCart" runat="server">
            <ItemTemplate>
                <div class="floor">
                    <div class="con">
                        <div class="select">
                            <input type="checkbox" checked="checked" class="icheck" id="ck_<%# Eval("SkuId") %>" onclick="jiesuan();" name="ck_productId" value="<%# Eval("SkuId") %>" />
                        </div>
                        <div class="name">
                            <div class="cart_commodit_span">
                                <Hi:ProductDetailsLink ID="ProductDetailsLink2" ProductId='<%# Eval("ProductId")%>' ProductName='<%# Eval("Name")%>' runat="server" ImageLink="true">
  <Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl100" runat="server" />
                                </Hi:ProductDetailsLink>
                            </div>
                            <div class="info">
                                <div class="cart_commodit_name">
                                    <Hi:ProductDetailsLink ID="ProductDetailsLink1" ProductId='<%# Eval("ProductId")%>' ProductName='<%# Eval("Name")%>' runat="server" />

                                </div>
                                <div class="cart_commodit_para">
                                    <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal><asp:Literal ID="litStock" runat="server"></asp:Literal>
                                </div>
                                <div class="step3">
                                    <%#Eval("HasStore").ToBool() ? "<div class=\"ziti\">支持自提</div>" : "" %>
                                    <asp:Repeater ID="rptPromotionGifts" runat="server">
                                        <ItemTemplate>
                                            <div class="gift">
                                                <em>
                                                    <img src='<%#Eval("ThumbnailUrl40") %>' title='<%#Eval("Name") %>'></em> <b>× <span gid='giftNum_<%# DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "SkuId") %>'><%# DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "Quantity") %></span></b>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>

                        <div class="price"><span class="cart_commodit_price">￥<span id="divPrice<%#Eval("SkuId") %>"><%#Eval("AdjustedPrice").ToDecimal().ToString("F2") %></span></span></div>

                        <div class="num">
                            <div class="info1">
                                <asp:HiddenField runat="server" ID="hidSkuId" Value='<%# Eval("SkuId")%>' ClientIDMode="Static"></asp:HiddenField>
                                <span t="cp" m="pminus" class="num_minus">-</span>
                                <asp:TextBox runat="server" ID="txtBuyNum" Text='<%# Eval("Quantity")%>' oldNumb='<%# Eval("Quantity")%>' CssClass="cart_txtbuynum" inputTagID='<%# Eval("SKU")%>' stock='<%#Hidistro.SaleSystem.Shopping.ShoppingCartProcessor.GetSkuStock(Eval("SkuId").ToNullString())%>' onblur="autoUpdateProductNum(this)" />
                                <span t="cp" m="padd" class="num_add">+</span>
                            </div>
                            <div class="info1" id="divStockInfo_<%#Eval("SkuId") %>">
                                <span style="color: red;" id="spanStock" runat="server" visible="false">【库存不足】</span>
                            </div>
                        </div>

                        <span class="cart_commodit_price2" style="display: none">￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" runat="server" Money='<%# Eval("SubTotal") %>' /></span>

                        <div class="total">
                            <button type="button" id="delCartItem" class="cart_commodit_del" value="<%# Eval("SkuId")%>" onclick="removeCartItem('<%# Eval("SkuId")%>')">删除</button>
                        </div>

                    </div>
                    <div class="line"></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <script type="text/javascript">
        function jiesuan() {

            var chk = $(":checkbox[name=ck_productId]:checked");

            var productNum = 0;
            var totalprice = 0;

            $(chk).each(function () {
                var price = $(this).parents('.con').find('.price span span').html();
                var buynum = $(this).parents('.con').find('.num .cart_txtbuynum').val();
                totalprice = totalprice + parseFloat(parseFloat(price) * parseInt(buynum));
                productNum = productNum + parseInt(buynum);
            });
            $('.select_num em').html(productNum);
            if (productNum == 0)
                $("#ShoppingCart_lblTotalPrice").text("0.00");
            else {
                $.ajax({
                    url: "API/ShoppingCartHandler.ashx",
                    type: "post",
                    dataType: "json",
                    timeout: 10000,
                    data: { action: 'reducedpromotion', Amount: totalprice, Quantity: productNum },
                    async: true,
                    success: function (data) {
                        totalprice = totalprice - parseFloat(data.ReducedPromotionAmount);
                        $("#ShoppingCart_lblTotalPrice").text(totalprice.toFixed(2));
                        if ($(".con").length == $(".ziti").length || $(".ziti").length == 0) {
                            $(".notes").hide();
                        }
                    }
                });
            }
            var chk = $(":checkbox[name=ck_productId]:checked");
            if (chk.length == 0 && ($("#ShoppingCart_hidPointGifts").val() == "" || $("#ShoppingCart_hidPointGifts").val() == "0")) {
                $("#ShoppingCart_btnCheckout").hide();
                $("#ShoppingCart_btnUnCheckout").show();
            } else {
                $("#ShoppingCart_btnCheckout").show();
                $("#ShoppingCart_btnUnCheckout").hide();
            }
        }
        $(function () {
            $('.icheck').iCheck({
                checkboxClass: 'icheckbox_flat-red',
                radioClass: 'iradio_flat-red'
            });

            $('#checkall').on('ifChecked', function (event) {
                $("input[name='ck_productId']").iCheck('check');
                $("input[name='ck_productId']").iCheck('check');
            });
            $('#checkall').on('ifUnchecked', function (event) {
                $("input[name='ck_productId']").iCheck('uncheck');
                $("input[name='ck_productId']").iCheck('uncheck');
            });
        })


        $(document).ready(function () {

            //点击添加购买数量
            $("span[m=padd]").click(function () {
                var txtBuyNum = $($(this).parent().find(":text[id*=txtBuyNum]").get(0));
                var currentNum = parseInt(txtBuyNum.val());
                var stock = txtBuyNum.attr('stock');
                var skuId = txtBuyNum.parent().find("#hidSkuId").val();
                if (currentNum >= stock) {
                    ShowMsg("库存不足", false);
                    return;
                } else {
                    $("#divStockInfo_" + skuId).hide();
                    $("#ShoppingCart_btnCheckout").show();
                    $("#ShoppingCart_btnUnCheckout").hide();
                }
                txtBuyNum.val(currentNum + 1);
                ChangeNum(txtBuyNum);
            });

            //点击减少购买数量
            $("span[m=pminus]").click(function () {
                var txtBuyNum = $($(this).parent().find(":text[id*=txtBuyNum]").get(0));
                var stock = txtBuyNum.attr('stock');
                var currentNum = parseInt(txtBuyNum.val());
                var skuId = txtBuyNum.parent().find("#hidSkuId").val();
                if (currentNum == 1) {
                    return;
                }

                if (currentNum - 1 > stock) {
                    $("#divStockInfo_" + skuId).show();
                    $("#ShoppingCart_btnCheckout").hide();
                    $("#ShoppingCart_btnUnCheckout").show();
                    return;
                } else {
                    $("#divStockInfo_" + skuId).hide();
                    $("#ShoppingCart_btnCheckout").show();
                    $("#ShoppingCart_btnUnCheckout").hide();
                }

                txtBuyNum.val(currentNum - 1);
                ChangeNum(txtBuyNum);
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

        function removeCartItem(sskuid) {
            if (confirm('确定删除？')) {
                $.ajax({
                    url: "API/ShoppingCartHandler.ashx",
                    type: "post",
                    dataType: "json",
                    timeout: 10000,
                    data: { action: 'delete', SkuId: sskuid },
                    async: true,
                    success: function (data) {
                        if (data.status == 'true') {
                            $("#ck_" + sskuid).parents('.floor').remove();
                            jiesuan();
                            clearCart();
                        }
                    }
                });
            }
        }

        //数量输入框失去焦点后自动更新
        function autoUpdateProductNum(el) {
            var txtBuyNum = $(el);
            if (isNaN(txtBuyNum.val())) {
                ShowMsg("请输入数字", false);
                txtBuyNum.val(txtBuyNum.attr('oldNumb')); return;
            }
            if (txtBuyNum.val().indexOf('.') > -1 || txtBuyNum.val().indexOf('-') > -1) {
                ShowMsg("购买数量必须为正整数", false);
                txtBuyNum.val(txtBuyNum.attr('oldNumb')); return;
            }
            if (parseInt(txtBuyNum.val()) > parseInt(txtBuyNum.attr('stock'))) {
                ShowMsg("购买数量超出库存", false);
                txtBuyNum.val(txtBuyNum.attr('oldNumb')); return;
            }
            ChangeNum(txtBuyNum);
        }

        $("#ShoppingCart_Common_ShoppingCart_ProductList___dataListShoppingCrat input[type='checkbox']").first().bind("click", function () {
            $("#ShoppingCart_Common_ShoppingCart_ProductList___dataListShoppingCrat input[type='checkbox']").not(this).attr("checked", this.checked);
            jiesuan();
        });

        function ChangeNum(txtBuyNum) {
            var sskuid, quantity;
            sskuid = txtBuyNum.parent().find("#hidSkuId").val();
            quantity = txtBuyNum.val();
            $.ajax({
                url: "API/ShoppingCartHandler.ashx",
                type: "post",
                dataType: "json",
                timeout: 10000,
                data: { action: 'updateBuyNum', SkuId: sskuid, BuyNum: quantity },
                async: true,
                success: function (data) {
                    if (data.status == 'true') {
                        txtBuyNum.attr('oldNumb', txtBuyNum.val());
                        $("span[gid=giftNum_" + sskuid + "]").text(txtBuyNum.val());
                        $("#divPrice" + sskuid).html(data.adjustedPrice);
                        jiesuan();
                    } else if (data.status == 'numError') {
                        ShowMsg("购买数量必须为大于0的整数", false);
                        txtBuyNum.val(data.oldNumb);
                    } else if (data.status == 'StockError') {
                        ShowMsg("该商品库存不足", false);
                        txtBuyNum.val(data.oldNumb);
                    }
                }
            });
        }

    </script>
</asp:Panel>
