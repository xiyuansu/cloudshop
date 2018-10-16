<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Depot/Admin.Master" CodeBehind="AdjustStoreSalePrice.aspx.cs" Inherits="Hidistro.UI.Web.Depot.product.AdjustStoreSalePrice" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
     <style type="text/css">
         .focus{  
    border:1px solid #fe5722;  
 } 
    </style>
    <div class="dataarea mainwidth databody">
        <div class="searcharea clearfix" style="padding-left: 10px;">
            <ul>
                <li><span>将原售价改为：</span><span><asp:TextBox ID="txtStoreSalePrice" ClientIDMode="Static" runat="server" Width="80px" CssClass="forminput" /></span></li>
                <li>
                    <input type="button" name="btnTargetOK" value="确定" id="btnTargetOK" class="btn btn-primary" />
                </li>
                <li><span>将原售价增加(输入负数为减少)：</span><span><asp:TextBox ID="txtAddStoreSalePrice" ClientIDMode="Static" runat="server" Width="80px" CssClass="forminput" /></span></li>
                <li>
                    <input type="button" name="btnOperationOK" value="确定" id="btnOperationOK" class="btn btn-primary" />
                </li>
                <li runat="server" id="priceTip" visible="false">
                    <div class="priceTip"><font style="color: red">
                        <asp:Literal runat="server" ID="priceTipMessage"></asp:Literal></font>&nbsp;&nbsp;</div>
                </li>
            </ul>
        </div>
        <div class="datalist clearfix">

            <asp:Repeater ID="grdSelectedProducts" runat="server">
                <HeaderTemplate>
                    <table class="table table-striped grdSelectedProducts">
                        <thead>
                            <th scope="col">货号</th>
                            <th scope="col">商品</th>
                            <th scope="col" style="width: 100px;">平台售价</th>
                            <th scope="col" style="width: 140px;">门店售价</th>
                            <th scope="col">备注</th>
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
                            <%#Eval("SalePrice","{0:f2}") %>
                            <asp:HiddenField ID="hidOldSalePrice" runat="server" Value='<%#Eval("SalePrice","{0:f2}") %>'></asp:HiddenField>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStoreSalePrice" runat="server" CssClass="j_salePrice forminput ipt_stock" Width="100%" salePrice='<%#Eval("SalePrice","{0:f2}") %>' Text='<%# Eval("StoreSalePrice").ToDecimal() > 0 ? Eval("StoreSalePrice").ToDecimal().F2ToString("f2") :Eval("SalePrice").ToDecimal().F2ToString("f2") %>'></asp:TextBox>
                            <asp:HiddenField ID="txtOldStoreSalePrice" runat="server" Value='<%#Eval("StoreSalePrice","{0:f2}") %>'></asp:HiddenField>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemark" runat="server" CssClass="forminput" Width="100%"></asp:TextBox>
                        </td>

                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>       
                </FooterTemplate>
            </asp:Repeater>

            <div style="margin-top: 10px; margin-bottom: 5px;">统一备注：</div>
            <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Width="99%" Height="50" CssClass="forminput"></asp:TextBox>
        </div>
        <div style="text-align: right; margin-top: 5px; margin-right: 20px;">
            <asp:Button ID="btnSaveSalePrice" runat="server" Text="保存" ClientIDMode="Static" CssClass="btn btn-primary hide" OnClick="btnSaveSalePrice_Click" OnClientClick="return validateStorePrice();" />
        </div>
    </div>
    <asp:HiddenField ID="hidIsModifyPrice" runat="server" Value="" />
    <asp:HiddenField ID="hidMinPriceRate" runat="server" Value="" />
    <asp:HiddenField ID="hidMaxPriceRate" runat="server" Value="" />
    <script type="text/javascript">
        $(function () {
            $("#btnTargetOK").click(function () {
                var b = isDecimal($("#txtStoreSalePrice").val());
                if (!b) {
                    $("#txtStoreSalePrice").val("");
                    ShowMsg("请输入正确的数字", false);
                    return;
                }
                var stocknum = parseFloat($("#txtStoreSalePrice").val());
                if (stocknum >= 0) {
                    $(".ipt_stock").val(stocknum.toFixed("2"));
                }
            });
            $("#btnOperationOK").click(function () {
                var b = IsDecimal2($("#txtAddStoreSalePrice").val());
                if (!b) {
                    $("#txtAddStoreSalePrice").val("");
                    ShowMsg("请输入正确的数字", false);
                    return;
                }
                var stocknum = parseFloat($("#txtAddStoreSalePrice").val());
                $(".ipt_stock").each(function () {
                    var _t = $(this);
                    var _ov = parseFloat(_t.val());
                    var _v = _ov + stocknum;
                    if (_v < 0) _v = 0;
                    _t.val(_v.toFixed("2"));
                });
            });
        })

        function isDecimal(obj) {
            reg = new RegExp("^[0-9]+\.{0,1}[0-9]{0,2}$");
            return reg.test(obj);
        }

        function IsDecimal2(obj) {
            reg = new RegExp(/^(\-|\+)?\d+(\.\d+)?$/);
            return reg.test(obj);
        }

        function validateStorePrice() {
            var minPriceRate = $("#ctl00_contentHolder_hidMinPriceRate").val();
            var maxPriceRate = $("#ctl00_contentHolder_hidMaxPriceRate").val();
            var isModifyPrice = $("#ctl00_contentHolder_hidIsModifyPrice").val();
            if (parseInt(isModifyPrice) == 1) {//如果开启了修改商品价格
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
            var minPriceRate = $("#ctl00_contentHolder_hidMinPriceRate").val();
            var maxPriceRate = $("#ctl00_contentHolder_hidMaxPriceRate").val();
            var isModifyPrice = $("#ctl00_contentHolder_hidIsModifyPrice").val();
            if (parseInt(isModifyPrice) == 1) {//如果开启了修改商品价格
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

        });
    </script>
</asp:Content>
