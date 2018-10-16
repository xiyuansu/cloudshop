<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditStoreProductInfo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.EditStoreProductInfo" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        html {
            background: #fff !important;
        }

        body {
            padding: 0;
            width: 100%;
        }

        #mainhtml {
            margin: 0;
            padding: 0 20px 60px 20px;
            width: 100% !important;
        }

        .searcharea {
            padding: 20px 0 0 0;
        }

            .searcharea li span {
                float: left;
                margin: 0 5px;
            }
               .priceTip {
    border: 1px #fe5722 solid;
    line-height: 28px;
    overflow: hidden;
    padding-left: 15px;
    width:260px;
}
               .focus{  
    border:1px solid #fe5722;  
 } 
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="hidStoreId" runat="server" />
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <div class="datafrom">
                <div class="info_group">
                    <ul class="editPro">
                        <li style="margin-left: 20px; display: none">
                            <span class="formitemtitle">门店：</span>
                            <asp:Literal runat="server" ID="litStoreName"></asp:Literal>
                        </li>
                        <li class="mb_20" style="margin-left: 20px; display: none">
                            <span class="formitemtitle">商品：</span>
                            <asp:Image ID="ImgProduct" runat="server" />&nbsp;&nbsp;&nbsp;
                            <asp:Literal runat="server" ID="litProductName"></asp:Literal>
                        </li>
                        <li>
                            <div class="batch">
                                <dl>
                                    <dd runat="server" id="priceTip" visible="false"><div class="priceTip"><font style="color: red">
                                <asp:Literal runat="server" ID="priceTipMessage"></asp:Literal></font>&nbsp;&nbsp;</div></dd>
                                    <dd>
                                        <span style="color: #666; font-size: 14px;">批量填充：</span>
                                    </dd>
                                    <dd>
                                        <input type="text" id="txtBatchStoreSalePrice" class="form-control" placeholder="门店价格" onkeyup="this.value=this.value.replace(/^[^0-9\.]+$/g,'')" />
                                    </dd>
                                    <dd>
                                        <input type="text" id="txtBatchStock" class="form-control" placeholder="库存" onkeyup="this.value=this.value.replace(/\D/g,'')" /></dd>
                                    <dd>
                                        <input type="text" id="txtBatchWarningStock" class="form-control" placeholder="警戒库存" onkeyup="this.value=this.value.replace(/\D/g,'')" />
                                    </dd>
                                    <dd>
                                        <input type="button" id="btnBacthSave" class="btn btn-primary" value="确定" onclick="bacthSave()" />
                                    </dd>
                                </dl>
                            </div>
                        </li>
                        <li>
                            <asp:Repeater ID="grdSelectedProducts" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-striped grdOrderGoods">
                                        <thead>
                                            <th scope="col">规格</th>
                                            <th scope="col">货号</th>
                                            <th scope="col" style="width: 100px;">平台价格</th>
                                            <th scope="col" style="width: 100px;">门店价格</th>
                                            <th scope="col" style="width: 100px;">门店库存</th>
                                            <th scope="col">警戒库存</th>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="trData">
                                        <td>
                                            <%# string.IsNullOrWhiteSpace(Eval("SKUContent").ToNullString())?"默认规格":Eval("SKUContent")%>
                                            <asp:HiddenField ID="hidSKUContent" runat="server" Value='<%#Eval("SKUContent")%>' />
                                            <asp:HiddenField ID="hidSkuId" runat="server" Value='<%#Eval("SkuId")%>' />
                                            <asp:HiddenField ID="hidSKU" runat="server" Value='<%#Eval("SKU")%>' />
                                            <asp:HiddenField ID="hidProductId" runat="server" Value='<%#Eval("ProductId")%>' />
                                        </td>
                                        <td>
                                            <%#Eval("SKU")%>
                                        </td>
                                        <td>
                                            <%#Eval("SalePrice","{0:f2}") %>
                                            <asp:HiddenField ID="hidOldSalePrice" runat="server" Value='<%#Eval("SalePrice","{0:f2}") %>'></asp:HiddenField>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStoreSalePrice" runat="server" CssClass="j_salePrice forminput" salePrice='<%#Eval("SalePrice","{0:f2}") %>' Width="100%" Text='<%# Eval("StoreSalePrice").ToDecimal() > 0 ? Eval("StoreSalePrice").ToDecimal().F2ToString("f2") :Eval("SalePrice").ToDecimal().F2ToString("f2") %>' onkeyup="this.value=this.value.replace(/^[^0-9\.]+$/g,'')"></asp:TextBox>
                                            <asp:HiddenField ID="hidOldStoreSalePrice" runat="server" Value='<%#Eval("StoreSalePrice","{0:f2}") %>'></asp:HiddenField>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStock" runat="server" CssClass="j_stock forminput" Width="100%" Text='<%#Eval("Stock")%>' onkeyup="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
                                            <asp:HiddenField ID="hidOldStock" runat="server" Value='<%#Eval("Stock") %>'></asp:HiddenField>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWarningStock" runat="server" CssClass="j_stock forminput" Width="100%" Text='<%#Eval("WarningStock") %>' onkeyup="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
                                            <asp:HiddenField ID="hidOldWarningStock" runat="server" Value='<%#Eval("WarningStock") %>'></asp:HiddenField>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>       
                                </FooterTemplate>
                            </asp:Repeater>
                        </li>
                    </ul>
                </div>
                <div style="text-align: right; margin-top: 5px; margin-right: 20px;">
                    <asp:Button ID="btnSaveStock" runat="server" Text="保存" CssClass="btn btn-primary" OnClick="btnSaveStock_Click" OnClientClick="return validateStorePrice();" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidMinPriceRate" runat="server" Value="" />
    <asp:HiddenField ID="hidMaxPriceRate" runat="server" Value="" />
    <asp:HiddenField ID="hidIsModifyPrice" runat="server" Value="" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function bacthSave() {
            var storesaleprice = $("#txtBatchStoreSalePrice").val();
            if (storesaleprice.trim() != "" && isDecimal(storesaleprice.trim()))
                $('input[id$="txtStoreSalePrice"]').val(storesaleprice);

            var Stock = $("#txtBatchStock").val();
            if (Stock.trim() != "" && isInteger(Stock.trim()))
                $('input[id$="txtStock"]').val(Stock);

            var WarningStock = $("#txtBatchWarningStock").val();
            if (WarningStock.trim() != "" && isInteger(WarningStock.trim()))
                $('input[id$="txtWarningStock"]').val(WarningStock);
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
            var errorNum = 0;
            $(".j_stock").each(function () {
                if (parseInt($(this).val()) <= 99999) {
                    $(this).removeClass("focus");
                } else {
                    $(this).addClass("focus");
                    ShowMsg('允许输入的最大值为99999', false);
                    errorNum++;
                    return;
                }
            });
            if (errorNum > 0) {
                return false;
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
            }

            $(".j_stock").blur(function () {
                if (parseInt($(this).val())<=99999) {
                    $(this).removeClass("focus");
                } else {
                    $(this).addClass("focus");
                    ShowMsg('允许输入的最大值为99999', false);
                    return;
                }
            });
        });
    </script>
</asp:Content>
