<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="EditOrder.aspx.cs" Inherits="Hidistro.UI.Web.Depot.EditOrder" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody ">
        <div class="title title_height m_none td_bottom">
            <em>
                <img src="../images/05.gif" width="32" height="32" /></em>
            <h1 class="title_line">修改订单</h1>
        </div>
        <div class="list">
            <h1>商品列表</h1>
            <asp:Repeater ID="grdProducts" runat="server">
                <HeaderTemplate>
                    <table width="0" border="0" cellspacing="0">
                        <tr class="table_title">
                            <td>商品</td>
                            <td width="14%" class="td_right td_left">商品单价
                            </td>
                            <td width="16%" class="td_right td_left">购买数量
                            </td>
                            <td width="12%" class="td_right td_left">发货数量
                            </td>
                            <td width="12%" class="td_right td_left">总价
                            </td>
                            <td width="18%" class="td_left td_right_fff">操作
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" width="100%" style="border: none;">
                                <tr>
                                    <td style="width: 80px; border: none;">
                                        <a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                                            <Hi:ListImage ID="HiImage2" runat="server" DataField="ThumbnailsUrl" Width="60px" />
                                        </a>
                                    </td>
                                    <td style="line-height: 22px; width: auto; border: none;">
                                        <span><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank"><%# Eval("ItemDescription") %></a>
                                            <br />
                                            货号：<asp:Literal ID="litSku" runat="server" Text='<%# Eval("Sku") %>' />
                                            <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <Hi:FormatedMoneyLabel ID="productPrice" runat="server" Money='<%# Eval("ItemListPrice") %>'></Hi:FormatedMoneyLabel></td>
                        <td>
                            <asp:TextBox Columns="6" ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="forminput leftR10" TagPrice="inputValue"></asp:TextBox>
                            <span class="submit_jiage">
                                <asp:LinkButton ID="setQuantity" runat="server" CommandArgument='<%# Eval("SkuId") %>' Text="修改" CommandName="setQuantity"></asp:LinkButton></span></td>
                        <td>
                            <asp:Literal ID="litShipmentQuantity" runat="server" Text='<%# Eval("ShipmentQuantity") %>'></asp:Literal></td>
                        <td>
                            <div class="color_36c">
                                <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(GetRouteUrl("FavourableDetails", new { activityId = Eval("PromotionId") }))%>'
                                    Text='<%# Eval("PromotionName")%>' Target="_blank"></asp:HyperLink>
                            </div>
                            <strong class="colorG">
                                <Hi:FormatedMoneyLabel ID="lblTotalPrice" runat="server" Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>' /></strong></td>
                        <td><span class="submit_shanchu">
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



          <div class="Price">
                <table width="100%" border="0" cellspacing="0">
                    <tr class="bg">
                        <td class="Pg_top td_none" width="88%" align="right">商品金额（元）：</td>
                        <td class="Pg_top td_none" width="11%" style="padding-left:1%;"><strong class="fonts colorG">
                            <Hi:FormatedMoneyLabel ID="lblAllPrice" runat="server" /></strong></td>
                    </tr>
                    <tr class="bg">
                        <td class="Pg_bot" align="right">商品总重量（克）：</td>
                        <td class="Pg_bot" style="padding-left:1%;"><strong class="fonts ">
                            <asp:Label ID="lblWeight" runat="server" /></strong></td>
                    </tr>
                </table>
            </div>



            <h1>礼品列表</h1>

           <asp:Repeater ID="grdOrderGift" runat="server">
                <HeaderTemplate>
                    <table width="0" border="0" cellspacing="0">
                        <tr class="table_title">
                            <td>礼品</td>
                            <td width="14%" class="td_right td_left">礼品单价
                            </td>
                            <td width="16%" class="td_right td_left">礼品赠送数量
                            </td>
                            <td width="12%" class="td_right td_left">礼品发货数量
                            </td>
                            <td width="12%" class="td_right td_left">礼品总金额
                            </td>
                            <td width="18%" class="td_left td_right_fff">操作
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <Hi:HiImage ID="HiImage1" AutoResize="true" Width="60" Height="60" runat="server" DataField="ThumbnailsUrl" />
                            <span>
                                <asp:Literal ID="giftName" runat="server" Text='<%# Eval("GiftName") %>'></asp:Literal></span>
                        </td>
                        <td>
                            <Hi:FormatedMoneyLabel ID="giftPrice" runat="server" Money='<%# Eval("CostPrice") %>'></Hi:FormatedMoneyLabel></td>
                        <td>
                            <asp:Literal ID="litQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litShipmentQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal></td>
                        <td>
                            <Hi:FormatedMoneyLabel ID="lblTotalPrice" runat="server" Money='<%# (decimal)Eval("CostPrice")*(int)Eval("Quantity") %>' /></td>
                        <td><span class="submit_shanchu">
                            <Hi:ImageLinkButton ID="Delete" runat="server" Text="删除" CommandArgument='<%# Eval("GiftId") %>'
                                CommandName="Delete" OnClientClick="return window.confirm('确认要删除该商品吗？')"
                                ForeColor="Red"></Hi:ImageLinkButton>
                        </span></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>



            <h1>订单实收款结算</h1>
            <div class="Settlement">
                <table width="200" border="0" cellspacing="0">
                    <tr>
                        <td width="15%" align="right">打折优惠(元)：<br />
                        </td>
                        <td width="11%" class="a_none"><span class="colorB">
                            <Hi:FormatedMoneyLabel ID="fullDiscountAmount" runat="server" /></span></td>
                        <td width="74%" class="a_none"><span class="Name">
                            <asp:HyperLink Target="_blank" ID="lkbtnFullDiscount" runat="server" /></span></td>
                    </tr>
                    <tr>
                        <td align="right">满额免费用活动(元)：</td>
                        <td colspan="2" class="a_none">
                            <asp:HyperLink Target="_blank" ID="lkbtnFullFree" runat="server" /></td>
                    </tr>
                    <tr>
                        <td align="right">运费(元)： </td>
                        <td class="a_none">
                            <asp:TextBox ID="txtAdjustedFreight" runat="server" CssClass="forminput" Width="70" /></td>
                        <td class="a_none">
                            <asp:Literal ID="litShipModeName" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">支付手续费(元)：</td>
                        <td class="a_none">
                            <asp:TextBox ID="txtAdjustedPayCharge" runat="server" CssClass="forminput" Width="70" /></td>
                        <td class="a_none">
                            <asp:Literal ID="litPayName" runat="server" /></td>
                    </tr>
                    <tr>
                        <td align="right">优惠券折扣(元)：</td>
                        <td colspan="2" class="a_none">
                            <Hi:FormatedMoneyLabel ID="couponAmount" runat="server" /></td>
                    </tr>
                    <tr>
                        <td align="right">涨价或减价(元)：</td>
                        <td class="a_none">
                            <asp:TextBox ID="txtAdjustedDiscount" runat="server" CssClass="forminput" Width="70" /></td>
                        <td class="a_none">为负代表折扣，为正代表涨价 </td>
                    </tr>
                    <asp:Literal ID="litTax" runat="server" />
                    <asp:Literal ID="litInvoiceTitle" runat="server" />
                    <tr>
                        <td align="right">订单可得积分：</td>
                        <td colspan="2" class="a_none">
                            <asp:Literal ID="litPoint" runat="server"></asp:Literal>&nbsp;<asp:HyperLink ID="hlkSentTimesPointPromotion" runat="server" Target="_blank" /></td>
                    </tr>
                    <tr class="bg">
                        <td align="right" class="colorG">订单实收款(元)：</td>
                        <td colspan="2" class="a_none"><strong class="colorG fonts">
                            <asp:Literal ID="litTotal" runat="server" /></strong></td>
                    </tr>
                </table>
            </div>
            <div class="bnt Pa_140 Pg_15 Pg_18">
                <span><asp:Button ID="btnUpdateOrderAmount" OnClientClick="return PageIsValid();" runat="server" Text="保存修改" CssClass="btn btn-primary" Style="float: left"></asp:Button></span>
            </div>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>






</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
function InitValidators()
{
initValid(new InputValidator('ctl00_contentHolder_txtAdjustedFreight', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '运费只能是数值，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAdjustedFreight', 0, 10000000, '运费只能是数值，不能超过10000000，且不能超过2位小数'));
initValid(new InputValidator('ctl00_contentHolder_txtAdjustedDiscount', 1, 10, false, '(0|^-?(0+(\\.[0-9]{1,2}))|^-?[1-9]\\d*(\\.\\d{1,2})?)', '订单折扣只能是数值，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAdjustedDiscount', -10000000, 10000000, '订单折扣只能是数值，不能超过10000000，且不能超过2位小数'));

}
$(document).ready(function(){ 
   //验证
   InitValidators(); 

  // 给输入值加限制
     $(".list table tr td input").each(function (index, domEle){
	   if($(this).attr("TagPrice")=="inputValue")
	     {
			$(this).keyup(function(e)
			{
			   //var key = window.event?e.keyCode:e.which;
			    var inputValue=$(this).val();
				inputValue=inputValue.replace(/[^\d]/g,'');
			   $(this).val(inputValue);
		    });		
	     }		
	 })						   
});
    </script>
</asp:Content>
