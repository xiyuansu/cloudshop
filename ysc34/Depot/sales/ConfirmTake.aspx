<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="ConfirmTake.aspx.cs" Inherits="Hidistro.UI.Web.Depot.sales.ConfirmTake" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Context" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">
        function validateTakeCode() {
            var orderId = $("#<%=hidOrderId.ClientID%>").val().trim();
            var code = $("#<%=txtTakeCode.ClientID%>").val().trim();
            var notempty = "<%=NeedTakeCode.ToString().ToLower()%>";
            if (code.length == 0 && notempty == "true") {
                alert("请输入提货码");
                return;
            }
            var statue = "<%=(int)Hidistro.Entities.Orders.OrderStatus.WaitBuyerPay%>";
            $.ajax({
                type: "Post",
                url: "/API/DepotHandler.ashx",
                data: "action=validateTakeCode&code=" + code + "&orderId=" + orderId,
                success: function (result) {
                    if (result == "1") {
                        if ($("#<%=hidOrderStatue.ClientID%>").val().trim() == statue) {
                            if (confirm("订单状态为\"等待买家付款\",请确认用户是否已经付款")) {
                                $("#<%=btnCheck.ClientID%>").click();
                            }
                        } else {
                            $("#<%=btnCheck.ClientID%>").click();
                        }

                    } else if (result == "-1") {
                        alert("提货码不正确");
                    } else if (result == "-2") {
                        alert("该提货码已被使用，无法再次提货");
                    } else if (result == "0") {
                        alert("提货码不能为空");
                    } else if (result == "-3") {
                        alert("订单不属于本门店");
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div id="divorderinfo" runat="server" class="dataarea mainwidth databody" style="height: 100%;">
        <div>
            <table width="100%">
                <tr>
                    <td style="width: 90px; text-align: right;">订单编号：
                    </td>
                    <td>
                        <asp:Literal ID="ltlOrderId" runat="server"></asp:Literal>
                        <asp:HiddenField ID="hidOrderId" runat="server" />
                    </td>
                    <td style="width: 90px; text-align: right;">支付方式：
                    </td>
                    <td>
                        <asp:Literal ID="ltlPaymentType" runat="server"></asp:Literal>
                    </td>
                    <td style="text-align: right">提货人信息：
                    </td>
                    <td>
                        <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>&nbsp;&nbsp;
                    <asp:Literal ID="ltlTelPhone" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">订单状态：</td>
                    <td style="color: #ff6600;">
                        <Hi:OrderStatusLabel ID="lblOrderStatus" runat="server" />
                        <asp:HiddenField ID="hidOrderStatue" runat="server" />
                    </td>
                    <td style="text-align: right">是否开具发票：</td>
                    <td colspan="3">
                        <asp:Literal ID="ltlIsTickt" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">发票类型：</td>
                    <td style="color: #ff6600;">
                        <asp:Literal runat="server" ID="litInvoiceType" />
                    </td>
                    <td style="text-align: right">发票抬头：</td>
                    <td colspan="3">
                     <asp:Literal runat="server" ID="litInvoiceTitle" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">买家留言：
                    </td>
                    <td colspan="5">
                        <asp:Literal ID="ltlComments" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
            &nbsp;&nbsp;
        </div>
        <div class="list">
            <asp:Repeater ID="dlstOrders" runat="server" OnItemDataBound="dlstOrders_ItemDataBound">
                <HeaderTemplate>
                    <table width="0" border="0" cellspacing="0">
                        <caption style="color: #0b5ba5;">商品列表</caption>
                        <tr class="table_title">
                            <td width="60%" class="td_right td_left">商品
                            </td>
                            <td width="10%" class="td_right td_left">商品单价
                            </td>
                            <td width="10%" class="td_right td_left">购买数量
                            </td>
                            <td width="10%" class="td_right td_left">小计
                            </td>
                            <td width="10%" class="td_right td_left">状态
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <span class="Name"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                                <%# Eval("ItemDescription")%></a></span> <span class="colorC">货号：<asp:Literal runat="server" ID="litCode" Text='<%#Eval("sku") %>' />
                                    <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></span>
                        </td>
                        <td>
                            <Hi:FormatedMoneyLabel ID="lblItemListPrice" runat="server" Money='<%# Eval("ItemListPrice") %>' />
                        </td>
                        <td>
                            <asp:Literal runat="server" ID="litQuantity" Text='<%#Eval("Quantity") %>' />
                        </td>
                        <td><strong class="colorG">
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" runat="server" Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>' /></strong></td>
                        <td>
                            <asp:Literal ID="litStatusText" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Repeater ID="grdOrderGift" runat="server">
                <HeaderTemplate>
                    <table width="200" border="0" cellspacing="0">
                        <caption style="color: #0b5ba5;">礼品列表</caption>
                        <tr class="table_title">
                            <td width="90%" class="td_right td_left">礼品名称</td>
                            <td width="10" class="td_right td_left">数量 </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <Hi:HiImage ID="HiImage1" AutoResize="true" Width="60" Height="60" runat="server" DataField="ThumbnailsUrl" />
                            <span>
                                <asp:Literal ID="giftName" runat="server" Text='<%# Eval("GiftName") %>'></asp:Literal></span> </td>

                        <td>×<asp:Literal ID="litQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal></td>

                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div style="margin-top: 10px; padding-right: 10px; text-align: right;">
                <span class="colorG">订单实收款(元)：</span><span class="colorG fonts"><Hi:FormatedMoneyLabel ID="lblOrderTotal" runat="server" /></span>
            </div>
        </div>
        <div style="text-align: center;">
            <table width="100%">
                <tr>
                    <td style="text-align: right; width: 250px;"><%if (NeedTakeCode)
                                                                     { %><span style="color: red;">*</span><%} %>买家提货码：</td>
                    <td>
                        <asp:TextBox ID="txtTakeCode" runat="server" class="forminput form-control" Style="width: 300px; height: 40px; font-size: 24px; color: #0b76c1; font-weight: bold;"></asp:TextBox></td>
                </tr>
                <%if (!NeedTakeCode)
                    { %>
                <tr><td style="text-align: right; width: 250px;">&nbsp;</td>
                    <td style="text-align:left;">可以不填写提货码直接确认提货</td>
                </tr>
                <%} %>
            </table>
        </div>
        <div style="margin-top: 20px; text-align: center;">
            <input type="button" value="确认提货" class="btn btn-primary" onclick="validateTakeCode();" />
            <div style="display: none;">
                <asp:Button ID="btnCheck" runat="server" Text="确认提货" CssClass="btn btn-primary" OnClick="btnCheck_Click" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</asp:Content>
