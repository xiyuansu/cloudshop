<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Panel ID="pnlShopProductCart" runat="server">
    <style type="text/css">
        .pc_carttitle {
            padding: 10px 0 5px 10px;
            font-size: 18px;
        }

            .pc_carttitle span {
                padding: 0px 5px;
                margin-left: 5px;
            }

            .pc_carttitle .pcztitle {
                background-color: #ff5722;
                border-radius: 2px;
                color: #fff;
                font-size: 12px;
            }

            .pc_carttitle .pcztitle_new {
                background-image: url(/templates/pccommon/images/icon_platform_pc.png);
                background-repeat: no-repeat;
                background-position: left center;
                padding-left: 30px;
                margin-left: 10px;
                font-size: 12px;
            }

            .pc_carttitle .pcstitle {
                background-image: url(/templates/pccommon/images/icon_supplier_pc.png);
                background-repeat: no-repeat;
                background-position: left center;
                padding-left: 30px;
                margin-left: 10px;
                font-size: 12px;
            }

            .pc_carttitle .icheckbox_flat-red {
                margin-top: 10px;
            }

        .cart_commodit .topt {
            background-color: #f1f1f1;
            border-bottom: 0px;
        }

            .cart_commodit .topt .select {
                padding-left: 5px;
            }
    </style>

    <div class="title topt">
        <div class="select">
            <input type="checkbox" checked="checked" id="checkall" class="icheck" onclick="jiesuan()" /><em>全选</em>
        </div>
        <div class="name"><em>商品</em></div>
        <div class="price">单价（元）</div>
        <div class="num">数量</div>
        <div class="total">操作</div>
    </div>

    <asp:Repeater ID="rptSupplier" runat="server">
        <ItemTemplate>
            <asp:Literal ID="ltsupplierId" runat="server" Visible="false" Text='<%#Eval("Key.SupplierId") %>' />
            <div class="title pc_carttitle" <%# (Eval("Key.SupplierName").ToString()=="" || !Hidistro.Context.HiContext.Current.SiteSettings.OpenSupplier)?" style=\"display:none;\"":"" %>>
                <%--无效供应商是没名称的--%>
                <asp:Literal ID="ltsupplierName" runat="server" Text='<%#Eval("Key.SupplierName").ToString()=="平台"?"平台":Eval("Key.SupplierName").ToString() %>' />&nbsp;
            </div>

            <div class="list" supplierid='<%#Eval("Key.SupplierId") %>' <%#Eval("Key.SupplierName").ToString()==""?" name=\"listInvalid\" style=\"display:none;\"":"" %>>
                <div id='cartlist<%#Eval("Key.SupplierId") %>'>
                    <asp:Repeater ID="dataListShoppingCart" runat="server">
                        <ItemTemplate>
                            <div class="floor">
                                <div class="con">
                                    <div class="select" id="divCheck" runat="server">
                                        <input type="checkbox" checked="checked" class="icheck" id="ck_<%# Eval("SkuId") %>" onclick="jiesuan();" name="ck_productId" value="<%# Eval("SkuId") %>" />
                                    </div>
                                    <div class="select" id="divNoCheck" runat="server" style="width: 30px">
                                        &nbsp;
                            <input type="hidden" id="ck_<%# Eval("SkuId") %>" value="<%# Eval("SkuId") %>" />
                                    </div>
                                    <div class="quehuo" id="divIsStock" runat="server">
                                        缺货
                            <input type="hidden" id="ck_<%# Eval("SkuId") %>" value="<%# Eval("SkuId") %>" />
                                    </div>
                                    <div class="shixiao" id="divInValid" runat="server">
                                        失效
                            <input type="hidden" id="ck_<%# Eval("SkuId") %>" value="<%# Eval("SkuId") %>" />
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

                                    <div class="price"><span class="cart_commodit_price">￥<span id="divPrice<%#Eval("SkuId") %>"><%#Eval("AdjustedPrice").ToDecimal().F2ToString("f2") %></span></span></div>

                                    <div class="num">
                                        <div class="info1" id="divAmount" runat="server">
                                            <asp:HiddenField runat="server" ID="hidSkuId" Value='<%# Eval("SkuId")%>' ClientIDMode="Static"></asp:HiddenField>
                                            <span t="cp" m="pminus" class="num_minus">-</span>
                                            <asp:TextBox runat="server" ID="txtBuyNum" Text='<%# Eval("Quantity")%>' oldNumb='<%# Eval("Quantity")%>' CssClass="cart_txtbuynum" inputTagID='<%# Eval("SKU")%>' stock='<%#Hidistro.SaleSystem.Shopping.ShoppingCartProcessor.GetSkuStock(Eval("SkuId").ToNullString())%>' onblur="autoUpdateProductNum(this)" />
                                            <span t="cp" m="padd" class="num_add">+</span>
                                        </div>
                                        <div class="info1" id="divStockInfo_<%#Eval("SkuId") %>">
                                            <%--<asp:HiddenField runat="server" ID="hidSkuId" Value='<%# Eval("SkuId")%>' ClientIDMode="Static"></asp:HiddenField>--%>
                                            <%--<asp:Label runat="server" ID="lblBuyNum" Text="<%# Eval("Quantity")%>"></asp:Label>--%>
                                            <span style="color: red;" id="spanStock" runat="server" visible="false">此商品库存不足</span>
                                        </div>
                                        <div class="info1" id="divValidInfo" runat="server">
                                            <%--<asp:HiddenField runat="server" ID="hidSkuId" Value='<%# Eval("SkuId")%>' ClientIDMode="Static"></asp:HiddenField>--%>
                                            <%--<asp:Label runat="server" ID="Label1" Text="<%# Eval("Quantity")%>"></asp:Label>--%>
                                            <%# Eval("Quantity")%>
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
            </div>
        </ItemTemplate>
    </asp:Repeater>







    <script type="text/javascript">
        //供应商组复选框按钮 全选
        function setchksupplier(supplierId) {
            if ($('#chksupplier_' + supplierId).is(':checked')) {
                $("#cartlist" + supplierId + " input[name='ck_productId']").iCheck('check');
            } else {
                $("#cartlist" + supplierId + " input[name='ck_productId']").iCheck('uncheck');
            }
            jiesuan();//重新结算
        }

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
                    data: { action: 'reducedpromotion', Amount: totalprice, Quantity: productNum, client: "pc" },
                    async: true,
                    success: function (data) {
                        //totalprice = totalprice - parseFloat(data.ReducedPromotionAmount);
                        if (totalprice < 0) totalprice = 0;
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

            ischeckparent();//判断供应商和全选 复选框变为选中或不选中
        }

        //判断供应商和全选 复选框变为选中或不选中
        function ischeckparent() {
            var chknocheck_pro = $(":checkbox[name=ck_productId]").not(":checked");//没有被选中的商品
            var chknocheck_sup = $(":checkbox[name=chksupplier]").not(":checked");//没有被选中的供应商
            if (chknocheck_pro.length == 0 && chknocheck_sup.length == 0) {
                $("#checkall").attr("checked", true);//把全选 设为不选中
                $('#checkall').parent("div .icheckbox_flat-red").removeClass("checked").addClass("checked");//选中样式
            }
            else {
                $("#checkall").attr("checked", false);//把全选 设为不选中
                $('#checkall').parent("div .icheckbox_flat-red").removeClass("checked");

                //没选中的商品
                chknocheck_pro.each(function () {
                    //当前供应标题id=“chksupplier_”+获取当前最近list标签属性supplierid 值
                    var chksupplierid = "chksupplier_" + $(this).closest("div.list").attr("supplierid");
                    $("#" + chksupplierid).attr("checked", false);//当前供应商不选中
                    $("#" + chksupplierid).parent("div .icheckbox_flat-red").removeClass("checked");
                });

                //没选中的供应商列表
                chknocheck_sup.each(function () {
                    //"div[supplierid=" 是找到当前div，.find是找到它对应未选中的商品
                    var nocheck_pro = $("div[supplierid=" + $(this).val() + "]").find(":checkbox[name=ck_productId]").not(":checked");
                    if (nocheck_pro.length == 0) { //它子商品没有全选中了，把当前供应商选中
                        $(this).attr("checked", true);
                        $(this).parent("div .icheckbox_flat-red").removeClass("checked").addClass("checked");
                    }
                });

                chknocheck_sup = $(":checkbox[name=chksupplier]").not(":checked");//再次获取没有被选中的供应商
                if (chknocheck_sup.length == 0 && $(":checkbox[name=chksupplier]").length > 0) {//供应商全选中了，则全选按钮变为选中
                    $("#checkall").attr("checked", true);
                    $('#checkall').parent("div .icheckbox_flat-red").removeClass("checked").addClass("checked");
                }
            }
        }

        $(function () {
            $('.icheck').iCheck({
                checkboxClass: 'icheckbox_flat-red',
                radioClass: 'iradio_flat-red'
            });

            $('#checkall').on('ifChecked', function (event) {
                $("input[name='ck_productId']").iCheck('check');
                $("input[name='chksupplier']").iCheck('check');
            });
            $('#checkall').on('ifUnchecked', function (event) {
                $("input[name='ck_productId']").iCheck('uncheck');
                $("input[name='chksupplier']").iCheck('uncheck');
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
                            if (GoToCheck()) {
                                $("input").each(function (i, submitObj) {
                                    var submitTagObj = $(submitObj).attr("SubmitTagID");
                                    if (submitTagObj == inputTagObj && GoToCheck()) { $(submitObj).focus(); }
                                })
                            }
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
                    data: { action: 'delete', SkuId: sskuid, client: "pc" },
                    async: true,
                    success: function (data) {
                        if (data.status == 'true') {
                            $("#ck_" + sskuid).parents('.floor').remove();
                            jiesuan();
                            clearCart();

                            //判断供应商下如没商品时 供应商标题隐藏掉
                            $(":checkbox[name=chksupplier]").each(function () {
                                var chk_pros = $("div[supplierid=" + $(this).val() + "] .floor");
                                if (chk_pros.length == 0) {//它下是没商品 去掉标题
                                    $(this).closest("div .pc_carttitle").remove();
                                }
                            });
                            ///----------------------------
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
                data: { action: 'updateBuyNum', SkuId: sskuid, BuyNum: quantity, client: "pc" },
                async: true,
                success: function (data) {
                    if (data.status == 'true') {
                        txtBuyNum.attr('oldNumb', txtBuyNum.val());
                        $("span[gid=giftNum_" + sskuid + "]").text(txtBuyNum.val());
                        $("#divPrice" + sskuid).html(data.adjustedPrice.toFixed(2));
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
