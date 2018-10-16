<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BalanceOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.Balance.BalanceOrder" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        #calendarStartDate { width: 160px; }
        #calendarEndDate { width: 160px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="RquestStatistics.aspx">管理</a></li>
                <li <%=BalanceOver==0?"class='hover'":"" %>>
                    <asp:HyperLink ID="hlkUnBalanceOver" runat="server" ClientIDMode="Static">待结算订单</asp:HyperLink></li>
                <li <%=BalanceOver==1?"class='hover'":"" %>>
                    <asp:HyperLink ID="hlkBalanceOver" runat="server" ClientIDMode="Static">已结算订单</asp:HyperLink></li>
            </ul>
        </div>
        <div class="datalist">
            <blockquote class="blockquote-default blockquote-tip clearfix">
                供应商：<asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblOrderNumText" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblOrderNumValue" ClientIDMode="Static" runat="server" CssClass="f-bold" Text="0"></asp:Label>
                <asp:Label ID="lblBalanceText" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblBalanceValue" ClientIDMode="Static" runat="server" CssClass="f-bold" Text="0"></asp:Label>元                

            </blockquote>
            <div class="searcharea">
                <ul>
                    <li><span>订单日期：</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarStartDate"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarEndDate" IsEndDate="true"></Hi:CalendarPanel>
                    </li>
                    <li><span>订单编号：</span><span>
                        <input type="text" id="txtOrderId" class="forminput form-control"/>
                    </span></li>
                    <li>
                        <asp:HiddenField runat="server" ID="hidSupplierId" ClientIDMode="Static" />
                        <input type="hidden" name="hidStatus" id="hidStatus" value="<%=BalanceOver %>" />
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                    <li>
                        <p><a href="javascript:Post_ExportExcel()">导出数据</a></p>
                    </li>
                </ul>
            </div>

            <!--S DataShow-->

            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th style="width: 180px;">下单时间</th>
                        <th style="width: 140px;">订单编号</th>
                        <th style="width: 80px;">支付方式</th>
                        <th>供货总价</th>
                        <th>运费</th>
                        <th style="width: 80px;">结算金额</th>
                        <th style="width: 80px;">状态</th>
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
                    <td>{{item.OrderDate |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td><a class="colorBlue" href="../OrderDetails.aspx?OrderId={{item.OrderId}}" data-toggle='tooltip' data-placement='top' title='查看详情'>{{item.OrderId}}</a></td>
                    <td><span class="colorC">{{item.PaymentType}}</span></td>
                    <td>{{item.OrderCostPrice.toFixed(2)}}</td>
                    <td>{{item.Freight.toFixed(2)}}</td>
                    <td>{{item.SettlementAmount.toFixed(2)}}</td>
                    <td>{{item.OrderStatusText}}</td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/Supplier/Balance/ashx/BalanceOrder.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/Supplier/Balance/scripts/BalanceOrder.js" type="text/javascript"></script>
</asp:Content>
