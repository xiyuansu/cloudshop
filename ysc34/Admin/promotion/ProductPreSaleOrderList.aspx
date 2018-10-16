<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductPreSaleOrderList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.ProductPreSaleOrderList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <input type="hidden" id="hidPreSaleId" name="hidPreSaleId" value="<%=preSaleId %>" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="ProductPreSale.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">预售记录</a></li>
            </ul>
        </div>
        <div class="VIPbg fonts">
            <ul>
                <li class="colorQ">预售商品销售<strong style="color: black"><asp:Literal ID="litProductSaleAmount" runat="server" /></strong>件</li>
                <li class="colorQ">已付尾款商品数<strong class="colorB"><asp:Literal ID="litPayAllProductAmount" runat="server" /></strong>件</li>
                <li class="colorQ">已付定金<strong class="colorG"><asp:Literal ID="litPayDepositTotal" runat="server" /></strong>元</li>
                <li class="colorQ">已付尾款<strong class="colorG"><asp:Literal ID="litFinalPaymentTotal" runat="server" /></strong>元</li>
                <li class="colorQ">金额总计<strong class="colorG"><asp:Literal ID="litAllTotal" runat="server" /></strong>元</li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">

            <!--S DataShow-->
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th>用户名</th>
                        <th>下单时间</th>
                        <th>订单编号</th>
                        <th>商品数量</th>
                        <th>定金</th>
                        <th>尾款</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--E DataShow-->
        </div>
        <!--S Pagination-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
        <!--E Pagination-->
    </div>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td style="text-overflow: ellipsis;white-space: nowrap;overflow: hidden;">{{item.UserName}}</td>
                    <td>{{ item.OrderDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td><a class="colorBlue" href="/admin/sales/OrderDetails.aspx?OrderId={{item.OrderId}}">{{item.OrderId}}</a></td>
                    <td>{{item.ProductSum}}</td>
                    <td>{{item.Deposit}}</td>
                    <td>{{if item.PayDate && item.PayDate!=""}}
                        {{item.FinalPayment}}
                        {{else}}
                        未支付
                        {{/if}}
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/ProductPreSaleOrderList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/ProductPreSaleOrderList.js" type="text/javascript"></script>
</asp:Content>
