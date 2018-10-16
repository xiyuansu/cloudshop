<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="MoveToStore.aspx.cs" Inherits="Hidistro.UI.Web.Depot.product.MoveToStore" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <style type="text/css">
         .focus{  
    border:1px solid #fe5722;  
 } 
    </style>
    <script type="text/javascript">
        function bacthSetting() {
            var Stock = $("#txtBatchTagetStock").val();
            if (Stock.trim() != "" && isInteger(Stock.trim()))
                $('input[id$="txtStock"]').val(Stock);

            var WarningStock = $("#txtBatchWarningStock").val();
            if (WarningStock.trim() != "" && isInteger(WarningStock.trim()))
                $('input[id$="txtWarningStock"]').val(WarningStock);

            var storesaleprice = $("#txtBatchStoreSalePrice").val();
            if (storesaleprice.trim() != "" && isDecimal(storesaleprice.trim()))
                $('input[id$="txtStoreSalePrice"]').val(storesaleprice);
        }

        function isInteger(obj) {
            reg = new RegExp("^(0|[1-9][0-9]*)$");
            return reg.test(obj);
        }

        function isDecimal(obj) {
            reg = new RegExp("^[0-9]+\.{0,1}[0-9]{0,2}$");
            return reg.test(obj);
        }

        function validateStorePrice() {
            var isModifyPrice = $("#ctl00_contentHolder_hidIsModifyPrice").val();
            if (parseInt(isModifyPrice) == 1) {//如果开启了修改商品价格
                var minPriceRate = $("#ctl00_contentHolder_hidMinPriceRate").val();
                var maxPriceRate = $("#ctl00_contentHolder_hidMaxPriceRate").val();
                var errorCount = 0;
                $(".j_salePrice").each(function () {
                    var salePrice = $(this).attr("salePrice");//当前的平台价格
                    var currentStoreSalePrice = $(this).val();//当前输入的门店价格
                    var min = salePrice * minPriceRate;
                    var max = salePrice * maxPriceRate;
                    if (currentStoreSalePrice >= min && currentStoreSalePrice <= max) {
                        $(this).removeClass("focus");
                    } else {
                        errorCount++;
                        $(this).addClass("focus");
                        ShowMsg('可设置价格区间为商品价格的' + minPriceRate + '倍-' + maxPriceRate + '倍', false);
                        return;
                    }
                });
                if (errorCount > 0) {
                    return false;
                }
            }
        }

        $(function () {
            var isModifyPrice = $("#ctl00_contentHolder_hidIsModifyPrice").val();
            if (parseInt(isModifyPrice) == 1) {//如果开启了修改商品价格
                var minPriceRate = $("#ctl00_contentHolder_hidMinPriceRate").val();
                var maxPriceRate = $("#ctl00_contentHolder_hidMaxPriceRate").val();
                $(".j_salePrice").blur(function () {
                    var salePrice = $(this).attr("salePrice");//当前的平台价格
                    var currentStoreSalePrice = $(this).val();//当前输入的门店价格
                    var min = salePrice * minPriceRate;
                    var max = salePrice * maxPriceRate;
                    if (currentStoreSalePrice >= min && currentStoreSalePrice <= max) {
                        $(this).removeClass("focus");
                    } else {
                        $(this).addClass("focus");
                        ShowMsg('可输入价格范围' + min + '-' + max + '', false);
                        return;
                    }
                });
 
            } else {
                $(".l_salePrice").show();
                $(".j_salePrice").hide();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="searcharea clearfix" style="padding-left: 10px;">
            <ul>
                <li><span>批量设置库存：</span><input type="text" id="txtBatchTagetStock" style="width: 80px" class="forminput form-control" /></li>
                <li><span>批量设置警戒库存：</span><input type="text" id="txtBatchWarningStock" style="width: 80px" class="forminput form-control" /></li>
                <li runat="server" id="lisaleprice"><span>批量设置门店售价：</span><input type="text" id="txtBatchStoreSalePrice" style="width: 80px" class="forminput form-control" /></li>
                <li>
                    <input type="button" id="btnTargetOK" value="确定" class="btn btn-primary" onclick="bacthSetting()" />
                </li>
                <li runat="server" id="priceTip" visible="false"><div class="priceTip"><font style="color:red"><asp:Literal runat="server" ID="priceTipMessage"></asp:Literal></font>&nbsp;&nbsp;</div></li>
            </ul>
        </div>
        <div class="datalist clearfix">
           
            <asp:Repeater ID="grdSelectedProducts" runat="server">
                <HeaderTemplate>
                    <table class="table table-striped grdSelectedProducts">
                        <thead>
                            <th scope="col">货号</th>
                            <th scope="col">商品</th>
                            <th scope="col" style="width: 70px;">平台库存</th>
                            <th scope="col" style="width: 70px;">平台售价</th>
                            <th scope="col" style="width: 100px;">门店售价</th>
                            <th scope="col" style="width: 70px;">门店库存</th>
                            <th scope="col" style="width: 70px;">警戒库存</th>
                        </thead>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="trData">
                        <td>&nbsp;<%#Eval("SKU") %></td>
                        <td>
                            <asp:HiddenField ID="HidProductId" runat="server" Value='<%#Eval("ProductId") %>'></asp:HiddenField>
                            <asp:HiddenField ID="HidSkuId" runat="server" Value='<%#Eval("SkuId") %>'></asp:HiddenField>
                            <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                            </a>
                            <%#Eval("ProductName") %>
                            <br />
                            <%#Eval("SKUContent")%>
                            <asp:HiddenField ID="hidSKUContent" runat="server" Value='<%#Eval("SKUContent")%>' />
                        </td>
                        <td>
                            <%#Eval("Stock") %>
                        </td>
                        <td>
                            <%#Eval("SalePrice","{0:f2}") %>
                            <asp:HiddenField ID="hidSalePrice" runat="server" Value='<%#Eval("SalePrice")%>' />
                        </td>
                        <td>
                            <asp:TextBox ID="txtStoreSalePrice" runat="server" CssClass="j_salePrice forminput" salePrice='<%#Eval("SalePrice")%>' Width="100%" Text='<%#Eval("SalePrice","{0:f2}") %>'></asp:TextBox>
                            <div  class="l_salePrice" style=" display:none;"><%#Eval("SalePrice","{0:f2}") %></div>
                        </td>
                        <td>

                            <asp:TextBox ID="txtStock" runat="server" CssClass="forminput" Width="100%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWarningStock" runat="server" CssClass="forminput" Width="100%"></asp:TextBox>

                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>       
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div style="text-align: right; margin-top: 5px; margin-right: 20px;">
            <asp:Button ID="SaveStock" ClientIDMode="Static" runat="server" Text="确认上架" OnClick="btnSaveStock_Click" OnClientClick="return validateStorePrice();" CssClass="btn btn-primary hide" />
        </div>
    </div>
    <asp:HiddenField ID="hidMinPriceRate" runat="server" Value="" />
    <asp:HiddenField ID="hidMaxPriceRate" runat="server" Value="" />
    <asp:HiddenField ID="hidIsModifyPrice" runat="server" Value="" />
</asp:Content>
