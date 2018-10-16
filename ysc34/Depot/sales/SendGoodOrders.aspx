<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" EnableViewState="false" AutoEventWireup="true" CodeBehind="SendGoodOrders.aspx.cs" Inherits="Hidistro.UI.Web.Depot.SendGoodOrders" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:void" class="hover">发货统计</a></li>
            </ul>
            <i class="glyphicon glyphicon-question-sign" data-container="body" style="cursor: pointer" data-toggle="popover" data-placement="left" title="操作说明" data-content="我的门店中所有发货后正常完成的订单(不包括售后订单以及非正常关闭的订单)"></i>
        </div>

        <!--搜索-->
        <div class="searcharea clearfix br_search">
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li>
                        <span>结算时间：</span>
                        <input name="startDate" type="text" value="" id="startDate" class="forminput form-control" readonly="readonly" style="width: 140px; float: left;" />
                        <span class="Pg_1010">至</span>
                        <input name="endDate" type="text" value="" id="endDate" class="forminput form-control" readonly="readonly" style="width: 140px; float: left;" />
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                    <li>
                        <a href="javascript:ExportToExcel()">导出数据</a></li>
                </ul>
            </div>
            <div class="functionHandleArea clearfix" style="font-weight: bold;">
                发货订单总数：<span style="color: red; padding-left: 3px; padding-right: 3px;" id="spanTotalCount">0</span>,订单总金额<span style="color: red; padding-left: 3px; padding-right: 3px;" id="lblOrderSummaryTotal">0.00</span>元,订单总利润<span style="color: red; padding-left: 3px; padding-right: 3px;" id="lblOrderSummaryProfit">0.00</span>元
            </div>

            <!--数据列表区域-->
            <div class="datalist clearfix">
                <table width="100%" border="0" cellspacing="0">
                    <tr class="table_title">
                        <td width="14%" class="td_right td_left">发货时间</td>
                        <td width="14%" class="td_right td_left">下单时间</td>
                        <td width="14%" class="td_right td_left">订单编号</td>
                        <td width="14%" class="td_right td_left">用户名</td>
                        <td width="10%" class="td_right td_left">收货人</td>
                        <td width="10%" class="td_right td_left">订单金额</td>
                        <td width="10%" class="td_right td_left">利润</td>
                        <td width="10%" class="td_left td_right_fff">操作</td>
                    </tr>
                    <tbody id="datashow"></tbody>
                </table>
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
    </div>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
            <tr>
                <td>{{item.ShippingDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                <td>{{item.OrderDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                <td>{{item.PayOrderId}}</td>
                <td>{{item.Username}}</td>
                <td>{{item.ShipTo}}</td>
                <td>{{item.StatisticsOrderTotal.toFixed(2)}}</td>
                <td>{{item.StatisticsOrderProfit.toFixed(2)}}</td>
                <td>
                    <span class="submit_bianji"><a id='a_{{item.OrderId}}' href="OrderDetails.aspx?OrderId={{item.OrderId}}" title="{{item.PayOrderId}}">查看详情</a></span>
                </td>
            </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/sales/ashx/SendGoodOrders.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/sales/scripts/SendGoodOrders.js" type="text/javascript"></script>
</asp:Content>
