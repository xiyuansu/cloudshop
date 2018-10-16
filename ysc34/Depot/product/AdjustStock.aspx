<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="AdjustStock.aspx.cs" Inherits="Hidistro.UI.Web.Depot.product.AdjustStock" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="searcharea clearfix" style="padding-left: 10px;">
            <ul>
                <li><span>将原库存改为：</span><span><asp:TextBox ID="txtTagetStock" ClientIDMode="Static" runat="server" Width="80px" CssClass="forminput" /></span></li>
                <li>
                    <input type="button" name="btnTargetOK" value="确定" id="btnTargetOK" class="btn btn-primary" />
                </li>
                <li><span>将原库存增加(输入负数为减少)：</span><span><asp:TextBox ID="txtAddStock" ClientIDMode="Static" runat="server" Width="80px" CssClass="forminput" /></span></li>
                <li>
                    <input type="button" name="btnOperationOK" value="确定" id="btnOperationOK" class="btn btn-primary" />
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
                            <th scope="col" style="width: 70px;">平台库存</th>
                            <th scope="col" style="width: 70px;">门店库存</th>
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
                              <%#GetMianStock(Eval("ProductId").ToNullString(), Eval("SkuId").ToNullString()) %>
                        </td>
                        <td>
                             <asp:TextBox ID="txtStock" runat="server" CssClass="forminput ipt_stock" Width="100%" Text='<%#Eval("Stock") %>'></asp:TextBox>
                            <asp:HiddenField ID="txtOldStock" runat="server" Value='<%#Eval("Stock") %>'></asp:HiddenField>                           
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
            <asp:Button ID="btnSaveStock" runat="server" Text="保存" OnClick="btnSaveStock_Click" ClientIDMode="Static" CssClass="btn btn-primary hide" />
        </div>
    </div>

    <script type="text/javascript">
        $(function () {
            $("#btnTargetOK").click(function () {
                var stocknum = parseInt($("#txtTagetStock").val());
                if (isNaN(stocknum)) {
                    $("#txtTagetStock").val("");
                    ShowMsg("请输入正确的数字", false);
                    return;
                }
                if (stocknum >= 0) {
                    $(".ipt_stock").val(stocknum);
                }
            });
            $("#btnOperationOK").click(function () {
                var stocknum = parseInt($("#txtAddStock").val());
                if (isNaN(stocknum)) {
                    $("#txtAddStock").val("");
                    ShowMsg("请输入正确的数字", false);
                    return;
                }
                $(".ipt_stock").each(function () {
                    var _t = $(this);
                    var _ov = parseInt(_t.val());
                    var _v = _ov + stocknum;
                    if (_v < 0) _v = 0;
                    _t.val(_v);
                });
            });
        })
    </script>
</asp:Content>
