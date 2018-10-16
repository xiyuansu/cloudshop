<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditOrder" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        html { background: #fff !important; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 20px 20px 60px 20px; width: 100% !important; }
        #producttb{
            float:left;
            width:100%;
        }
        #producttb tbody{
            float:left;
            width:100% !important;
        }
        #producttb tbody tr{
            float:left;
            width:100%;
        }
    </style>
    <script>
        $(function () {
            var num = $("#producttb tr").length;
            if (num < 2) {
                $("#producttb").css("display", "none");
            } else {
                $("#producttb").css("display", "block");
            }


        }) 

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody ">
        <div class="list">
            <asp:Repeater ID="grdProducts" runat="server">
                <HeaderTemplate>
                    <table class="table table-striped" id="producttb">
                        <tr>
                            <td width="400px">商品</td>
                            <td width="80px">商品单价
                            </td>
                            <td width="128px">购买数量
                            </td>
                            <td width="80px">发货数量
                            </td>
                            <td width="80px">总价
                            </td>
                            <td width="80px">操作
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td width="400px">
                            <a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                                <Hi:ListImage ID="HiImage2" runat="server" DataField="ThumbnailsUrl" Width="60px" />
                            </a>

                            <span style="text-align:left;float:right; width: 300px;"><a class="text-ellipsis c-666" href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank"><%# Eval("ItemDescription") %></a>
                                货号：<asp:Literal ID="litSku" runat="server" Text='<%# Eval("Sku") %>' />
                                <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                            </span>
                        </td>
                        <td width="80px">
                            <Hi:FormatedMoneyLabel ID="productPrice" runat="server" Money='<%# Eval("ItemListPrice") %>'></Hi:FormatedMoneyLabel></td>
                        <td width="128px">
                            <asp:TextBox Columns="6" ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="form-control forminput leftR10" TagPrice="inputValue" Width="70"></asp:TextBox>
                            <span class="submit_jiage lh32">
                                <asp:LinkButton ID="setQuantity" runat="server" CommandArgument='<%# Eval("SkuId") %>' Text="修改" CommandName="setQuantity"></asp:LinkButton></span></td>
                        <td width="80px">
                            <asp:Literal ID="litShipmentQuantity" runat="server" Text='<%# Eval("ShipmentQuantity") %>'></asp:Literal></td>
                        <td width="80px">
                            <div class="color_36c">
                                <asp:HyperLink ID="hlinkPurchase" Visible="false" runat="server" NavigateUrl='<%# string.Format(GetRouteUrl("FavourableDetails", new { activityId = Eval("PromotionId") }))%>'
                                    Text='<%# Eval("PromotionName")%>' Target="_blank"></asp:HyperLink>
                            </div>
                            <strong class="colorG">
                                <Hi:FormatedMoneyLabel ID="lblTotalPrice" runat="server" Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>' /></strong></td>
                        <td width="80px"><span class="submit_shanchu">
                            <Hi:ImageLinkButton ID="Delete" runat="server" Text="删除" CommandArgument='<%# Eval("SkuId") %>'
                                CommandName="Delete" OnClientClick="return window.confirm('确认要删除该商品吗？')"
                                ForeColor="Red"></Hi:ImageLinkButton>
                        </span></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Repeater ID="grdOrderGift" runat="server">
                <HeaderTemplate>
                    <table width="0" border="0" cellspacing="0" class="table table-striped">
                        <tr>
                            <td>礼品</td>
                            <td>成本价
                            </td>
                            <td>数量
                            </td>
                            <td>礼品类型
                            </td>
                            <td>操作
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td width="400px" style="text-align:left">
                            <Hi:HiImage ID="HiImage1" AutoResize="true" Width="60" Height="60" runat="server" DataField="ThumbnailsUrl" />
                            <span style="float: left; position: absolute; left: 115px; width: 300px;">
                                <a class="text-ellipsis c-666" href="../../GiftDetails.aspx?GiftId=<%#Eval("GiftId") %>">
                                    <asp:Literal ID="giftName" runat="server" Text='<%# Eval("GiftName") %>'></asp:Literal></a></span>
                        </td>
                        <td>
                            <Hi:FormatedMoneyLabel ID="giftPrice" runat="server" Money='<%# Eval("CostPrice") %>'></Hi:FormatedMoneyLabel></td>
                        <td>
                            <asp:Literal ID="litQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal></td>
                        <td>
                            <%# Eval("PromoteType").ToInt() == (int)Hidistro.Entities.Promotions.PromoteType.SentGift?"商品赠送":(Eval("PromoteType").ToInt() == (int)Hidistro.Entities.Promotions.PromoteType.FullAmountSentGift?"订单促销":(Eval("PromoteType").ToInt() ==-1?"中奖":"积分兑换")) %>    
                        </td>

                        <td><span class="submit_shanchu">
                            <Hi:ImageLinkButton ID="Delete" runat="server" Text="删除" CommandArgument='<%# Eval("GiftId") %>'
                                CommandName="Delete" OnClientClick="return window.confirm('确认要删除该礼品吗？')"
                                ForeColor="Red"></Hi:ImageLinkButton>
                        </span></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>


            <div class="Price">
                <table width="100%" border="0" cellspacing="0">
                    <tr class="bg">
                        <td class="Pg_top td_none" width="88%" align="right">商品金额（元）：</td>
                        <td class="Pg_top td_none" width="10%" style="padding-left: 1%;"><strong class="fonts colorG">
                            <Hi:FormatedMoneyLabel ID="lblAllPrice" runat="server" /></strong></td>
                    </tr>
                    <tr class="bg" id="trDeopt" runat="server" visible="false">
                        <td class="Pg_top td_none" width="88%" align="right">定金（元）：</td>
                        <td class="Pg_top td_none" width="10%" style="padding-left: 1%;"><strong class="fonts colorG">
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyDeopt" runat="server" /></strong></td>
                    </tr>
                    <tr class="bg" id="trFinal" runat="server" visible="false">
                        <td class="Pg_top td_none" width="88%" align="right">尾款（元）：</td>
                        <td class="Pg_top td_none" width="10%" style="padding-left: 1%;"><strong class="fonts colorG">
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyFinal" runat="server" /></strong></td>
                    </tr>
                    <%-- <tr class="bg">
                        <td class="Pg_bot" align="right">商品总重量（克）：</td>
                        <td class="Pg_bot" style="padding-left: 1%;"><strong class="fonts ">
                            <asp:Label ID="lblWeight" runat="server" /></strong></td>
                    </tr>--%>
                </table>
            </div>






            <div class="Settlement">

                <table border="0" cellspacing="0" class="table table_editorder float">
                    <tr>
                        <td align="right" class="" style="width: 150px;"><span id="tdDiscount" runat="server">打折优惠：</span>
                            &nbsp;
                        </td>
                        <td class="a_none text-ellipsis" nowrap="nowrap">
                            <span class="color_price float">
                                <Hi:FormatedMoneyLabel ID="fullDiscountAmount" runat="server" />
                            </span>
                            <span class="Name colorBlue float">
                                <asp:HyperLink Target="_blank" ID="lkbtnFullDiscount" runat="server" CssClass="text-ellipsis" Width="320" />
                            </span>
                            &nbsp;
                        </td>
                        <td class="a_none" align="right">
                            <span style="line-height: 32px;">运费：</span>
                            <div class="input-group float_r">
                                <span class="input-group-addon">￥</span>
                                <asp:TextBox ID="txtAdjustedFreight" runat="server" CssClass="forminput form-control" Width="70" />
                            </div>
                        </td>
                    </tr>
                    <tr id="trFullFree" runat="server">
                        <td align="right">满额免费用活动：</td>
                        <td colspan="2" class="a_none color_price text-ellipsis float" style="width: 500px; line-height: 50px;">
                            <asp:HyperLink Target="_blank" ID="lkbtnFullFree" runat="server" CssClass="colorBlue" /></td>
                    </tr>


                    <tr id="trCouponAmount" runat="server">
                        <td align="right">优惠券折扣：</td>
                        <td colspan="2" class="a_none color_price">
                            <Hi:FormatedMoneyLabel ID="couponAmount" runat="server" Width="320" CssClass="text-ellipsis" /></td>
                    </tr>
                    <tr>
                        <td align="right">涨价或减价：</td>
                        <td class="a_none">
                            <div class="input-group">
                                <span class="input-group-addon">￥</span>
                                <asp:TextBox ID="txtAdjustedDiscount" runat="server" CssClass="forminput form-control" Width="70" />
                            </div>
                        </td>
                        <td align="right">订单实收款：
                                <span class="color_price"><b>￥
                                    <asp:Literal ID="litTotal" runat="server" /></b></span>

                        </td>
                    </tr>
                    <asp:Literal ID="litTax" runat="server" />
                    <asp:Literal ID="litInvoiceTitle" runat="server" />
                    <tr>
                        <td align="right"><span id="tdPoint" runat="server">订单可得积分：</span></td>
                        <td class="a_none color_price float" style="line-height: 50px;">
                            <div class="float">
                                <asp:Literal ID="litPoint" runat="server"></asp:Literal>
                            </div>
                            &nbsp;<asp:HyperLink ID="hlkSentTimesPointPromotion" CssClass="colorBlue text-ellipsis float" runat="server" Target="_blank" Width="300" /></td>
                        <td align="right">余额抵扣：
                                <span class="color_price"><b>￥
                                    <asp:Literal ID="litBalance" runat="server" /></b></span>

                        </td>
                    </tr>
                </table>
            </div>
            <div class="modal_iframe_footer">
                <asp:Button ID="btnUpdateOrderAmount" OnClientClick="return PageIsValid();" runat="server" Text="保存" CssClass="btn btn-primary"></asp:Button>
            </div>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>






</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {

            initValid(new InputValidator('ctl00_contentHolder_txtAdjustedFreight', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '运费只能是数值，且不能超过2位小数'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAdjustedFreight', 0, 10000000, '运费只能是数值，不能超过10000000，且不能超过2位小数'));
            initValid(new InputValidator('ctl00_contentHolder_txtAdjustedDiscount', 1, 10, false, '(0|^-?(0+(\\.[0-9]{1,2}))|^-?[1-9]\\d*(\\.\\d{1,2})?)', '订单折扣只能是数值，且不能超过2位小数'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAdjustedDiscount', -10000000, 10000000, '订单折扣只能是数值，不能超过10000000，且不能超过2位小数'));

        }
        $(document).ready(function () {
            //验证
            InitValidators();

            // 给输入值加限制
            $(".list table tr td input").each(function (index, domEle) {
                if ($(this).attr("TagPrice") == "inputValue") {
                    $(this).keyup(function (e) {
                        //var key = window.event?e.keyCode:e.which;
                        var inputValue = $(this).val();
                        inputValue = inputValue.replace(/[^\d]/g, '');
                        $(this).val(inputValue);
                    });
                }
            })
        });
    </script>
</asp:Content>
