<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<div>
<asp:Repeater ID="dataListStoreShoppingCart" runat="server">
                        <ItemTemplate>
                            <div class="floor">
                                <div class="con">
                                    <div class="select" style="width:30px">&nbsp;
                                        <input type="hidden" id="ck_<%# Eval("SkuId") %><%# Eval("StoreId") %>" value="<%# Eval("SkuId") %><%# Eval("StoreId") %>" />
                                    </div>
                                    <div class="shixiao" id="divInValid" runat="server">失效</div>
                                    <div class="mendshix" id="divStore" runat="server" >门店</div>
                                    <div class="name">
                                        <div class="cart_commodit_span">
                                           <a href="javascript:;"><Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl100" runat="server" /></a>
                                        </div>
                                        <div class="info">
                                            <div class="cart_commodit_name">
                                               <a href="javascript:;"><%# Eval("Name")%></a>
                                            </div>
                                            <div class="cart_commodit_para"><%# Eval("SKUContent")%></div>
                                        </div>
                                    </div>

                                    <div class="price"><span class="cart_commodit_price">￥<span><%#Eval("AdjustedPrice").ToDecimal().F2ToString("f2") %></span></span></div>

                                    <div class="num">
                                        <div class="info1" id="divValidInfo" runat="server">
                                            <%# Eval("Quantity")%>
                                        </div>
                                    </div>

                                    <div class="total">
                                        <button type="button" id="delCartItem" class="cart_commodit_del" value="<%# Eval("SkuId")%>,<%# Eval("StoreId")%>" onclick="removeStoreCartItem('<%# Eval("SkuId")%>','<%# Eval("StoreId")%>')">删除</button>
                                    </div>

                                </div>
                                <div class="line"></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
     </div>   
    <script type="text/javascript">
   
        function removeStoreCartItem(sskuid, sstoreid) {
            if (confirm('确定删除？')) {
                $.ajax({
                    url: "API/ShoppingCartHandler.ashx",
                    type: "post",
                    dataType: "json",
                    timeout: 10000,
                    data: { action: 'deletestore', SkuId: sskuid, StoreId: sstoreid, client: "pc" },
                    async: true,
                    success: function (data) {
                        if (data.status == 'true') {
                            $("#ck_" + sskuid + sstoreid).parents('.floor').remove();
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
    </script>
