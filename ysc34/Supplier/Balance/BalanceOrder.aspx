<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="BalanceOrder.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.Balance.BalanceOrder" %>

<%@ Import Namespace="Hidistro.Core" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">

                <li><a href="BalanceOrder.aspx?&BalanceOver=0" id="anchors0">待结算订单</a></li>
                <li><a href="BalanceOrder.aspx?&BalanceOver=1" id="anchors1">已结算订单</a></li>
            </ul>
        </div>
        <div class="datalist">
            <blockquote class="blockquote-default blockquote-tip clearfix">
                <asp:Label ID="lblOrderNumText" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblOrderNumValue" runat="server" CssClass="f-bold" Text="0"></asp:Label>
                &nbsp;
                <asp:Label ID="lblBalanceText" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblBalanceValue" runat="server" CssClass="f-bold" Text="0"></asp:Label>元                

            </blockquote>
            <div class="searcharea">
                <ul>
                    <li><span>订单日期：</span>
                        <Hi:CalendarPanel runat="server" ID="calendarStartDate" ClientIDMode="Static"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <Hi:CalendarPanel runat="server" ID="calendarEndDate" IsEndDate="true" ClientIDMode="Static"></Hi:CalendarPanel>
                    </li>
                    <li><span>订单编号：</span><span>
                        <input type="text" id="txtOrderId" class="forminput form-control" />
                    </span></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                    <li>
                        <p>

                            <a href="javascript:ExportToExcelBalanceOrder()">生成报告</a>
                        </p>
                    </li>

                </ul>
            </div>
            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th scope="col" style="width: 15%;">下单时间</th>
                                <th scope="col" style="width: 15%;">订单编号</th>
                                <th scope="col" style="width: 15%;">支付方式</th>
                                <th scope="col" style="width: 15%;">供货总价</th>
                                <th scope="col" style="width: 15%;">运费</th>
                                <th scope="col" style="width: 15%;">结算金额</th>
                                <th scope="col" style="text-align: center; width: 10%">状态</th>
                            </tr>
                        </thead>
                        <tbody id="datashow"></tbody>
                    </table>
                    <div class="blank12 clearfix"></div>
                </div>
                <!--E DataShow-->
            </div>
            <!--E warp-->

            <!--S Pagination-->
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" id="showpager"></div>
                    </div>
                </div>
            </div>
            <!--E Pagination-->

            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <tr>
                    <td>{{item.OrderDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}} 
                    </td>
                    <td><a class="colorBlue" href="../sales/OrderDetails.aspx?OrderId={{item.OrderId}}" data-toggle='tooltip' data-placement='top' title='查看详情'>{{item.PayOrderId}}</a></td>
                    <td>{{item.PaymentType}}</td>
                    <td>{{item.OrderCostPrice.toFixed(2)}}</td>
                    <td>{{item.Freight.toFixed(2)}}</td>
                    <td>{{(item.OrderCostPrice+item.Freight).toFixed(2)}}</td>
                    <td style="text-align: center;">{{item.OrderStatusText}}</td>
                </tr>
                {{/each}}
              
            </script>

        </div>
    </div>

    <input type="hidden" name="dataurl" id="dataurl" value="/Supplier/Balance/ashx/BalanceManage.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Supplier/Balance/scripts/BalanceOrderManage.js" type="text/javascript"></script>
</asp:Content>
