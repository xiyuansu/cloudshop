<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductPreSaleView.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.ProductPreSaleView" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField runat="server" ID="hidProductId" />
    <asp:HiddenField runat="server" ID="hidSelectProducts" />
    <asp:HiddenField runat="server" ID="hidPreSaleId" />
    <asp:HiddenField runat="server" ID="hidProductName" />
    <asp:HiddenField runat="server" ID="hidSalePrice" />
    <div class="areacolumn clearfix">
        <div class="dataarea mainwidth databody">
            <div class="title">
                <ul class="title-nav">
                    <li><a href="ProductPreSale.aspx">管理</a></li>
                    <li class="hover"><a href="javascript:void">查看</a></li>

                </ul>
            </div>
        </div>
        <div class="columnright">
            <div class="formitem validator2">
                <ul>
                    <li class="mb_0">
                        <span class="formitemtitle">预售商品：</span>
                        <asp:Label runat="server" ID="lblProductName"></asp:Label>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle">商品价格：</span>
                        <asp:Label runat="server" ID="lblSalePrice"></asp:Label>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle">定金：</span>
                        <asp:Label runat="server" ID="lblDeposit"></asp:Label>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle">预售结束时间：</span>
                        <asp:Label runat="server" ID="lblPreSaleEndDate"></asp:Label>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle">尾款支付时间：</span>
                        <asp:Label runat="server" ID="lblPaymentStartDate"></asp:Label><span> &nbsp;至&nbsp;</span><asp:Label runat="server" ID="lblPaymentEndDate"></asp:Label>
                    </li>
                    <li>
                        <span class="formitemtitle">发货时间：</span>
                        <asp:Label runat="server" ID="lblDelivery"></asp:Label>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
