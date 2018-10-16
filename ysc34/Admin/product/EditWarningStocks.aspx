<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditWarningStocks.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.EditWarningStocks" Title="无标题页" %>

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
            padding: 20px 20px 60px 20px;
            width: 100% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="searcharea clearfix">
            <ul>
                <li><font>将原警戒库存改为：</font><input name="txtTagetStock" type="text" maxlength="20" id="txtTagetStock" class="forminput form-control" style="width:80px;"/></li>
                <li><input type="button" name="btnTargetOK" value="确定" id="btnTargetOK" class="btn btn-info"/></li>
                <li><font>将原警戒库存增加(输入负数为减少)：</font><input name="txtAddStock" type="text" maxlength="20" id="txtAddStock" class="forminput form-control" style="width:80px;"/></li>
                <li>
                    <input type="button" name="btnOperationOK" value="确定" id="btnOperationOK" class="btn btn-info" />
                </li>
            </ul>
        </div>

        <div class="datalist clearfix">
            <table class="table table-striped" cellspacing="0" border="0" style="width: 100%; border-collapse: collapse;">
                <thead>
                    <tr>
                        <th scope="col" width="20%">货号</th>
                        <th scope="col">商品</th>
                        <th scope="col" style="width: 15%;">库存</th>
                        <th scope="col" style="width: 20%;">警戒库存</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptSelectedProducts" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("SKU") %></td>
                                <td>
                            <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank" style="float:left; width:15%">
                                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                            </a>
                            <span style="width:75%; overflow:hidden; float:left;">
                            <%#Eval("ProductName") %> <%#Eval("SKUContent")%></span>
                                </td>
                                <td><%#Eval("Stock") %></td>
                                <td>
                                    <asp:TextBox ID="hidSkuId" runat="server" CssClass="hide ipt_skuid" Text='<%#Eval("SkuId") %>'></asp:TextBox>
                                    <asp:TextBox ID="txtWarningStock" runat="server" CssClass="forminput form-control ipt_stock" Text='<%#Eval("WarningStock") %>'></asp:TextBox>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </tbody>
            </table>
        </div>

        <div class="modal_iframe_footer">
            <asp:Button ID="btnSaveWarningStock" runat="server" OnClientClick="return PageIsValid();" Text="保存" CssClass="btn btn-primary" />
        </div>
    </div>
    
    <script type="text/javascript">
        $(function () {
            $("#btnTargetOK").click(function () {
                var num = $("#txtTagetStock").val();
                if (isNaN(num)) {
                    $("#txtTagetStock").val("");
                    ShowMsg("库存填写只能为数字！", false);
                    return;
                }
                var stocknum = parseInt(num);
                if (stocknum >= 0) {
                    $(".ipt_stock").val(stocknum);
                }
            });
            $("#btnOperationOK").click(function () {
                var stocknum = $("#txtAddStock").val();
                if (isNaN(stocknum)) {
                    $("#txtAddStock").val("");
                    ShowMsg("库存填写只能为数字！", false);
                    return;
                }
                stocknum = parseInt(stocknum);
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
