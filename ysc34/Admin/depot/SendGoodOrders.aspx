<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SendGoodOrders.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SendGoodOrders" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea clearfix">
                <blockquote class="blockquote-default blockquote-tip clearfix">
                    发货订单总数：<span class="f-bold" id="spanTotalCount" ></span> （门店中所有发货后正常完成的订单（不包括售后订单以及非正常关闭的订单）），订单总金额：<span>
                        <label ID="lblOrderSummaryTotal" class="f-bold"/>
                    </span>元，订单总利润：<span>
                       <label id="lblOrderSummaryProfit"   class="f-bold" />
                    </span> 元
                </blockquote>
                <ul>
                    <li><span>发货时间：</span>
                        <Hi:CalendarPanel runat="server" ID="startDate" Width="157"  ClientIDMode="Static"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <Hi:CalendarPanel runat="server" ID="endDate" Width="157" IsEndDate="true"  ClientIDMode="Static"></Hi:CalendarPanel>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:StoreDropDownList ID="ddlStores" runat="server" CssClass="iselect"  ClientIDMode="Static"></Hi:StoreDropDownList>
                        </abbr>
                    </li>
                    <li>
                         <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />    
                    </li>
                    <li>
                         <a href="javascript:ExportToExcel()">导出数据</a>
                    </li>
                </ul>
            </div>
              <div class="dataarea mainwidth databody">
                <div class="imageDataLeft" id="ImageDataList">
                    <!--S DataShow-->
                    <div class="datalist">
                         <table class="table table-striped">
                    <thead>
                        <tr>
                            <th width="14%">发货时间</th>
                            <th width="14%">下单时间</th>
                            <th width="14%">订单编号</th>
                            <th width="14%">用户名</th>
                            <th width="10%">收货人</th>
                            <th width="10%">订单金额</th>
                            <th width="10%">利润</th>
                            <th width="">操作</th>
                           </tr>
                    </thead>
                        <tbody id="datashow"></tbody>
                               </table>

                        <div class="blank12 clearfix"></div>
                    </div>
                </div>
                <!--E DataShow-->
             
                  <script id="datatmpl" type="text/html">
               
                    {{each rows as item index}}
                          <tr>
                              <td>{{item.ShippingDateStr}}</td>
                              <td>{{item.OrderDateStr}}</td>
                              <td>
                                  {{item.PayOrderId}}
                              </td>
                              <td>{{item.Username}}</td>
                              <td>{{item.ShipTo}}</td>
                              <td>{{item.OrderTotalStr}}</td>
                              <td>{{item.OrderProfitStr}}</td>
                                    <td>
                            <span class="submit_bianji"><a id='a_{{item.OrderId}}' href="/admin/sales/OrderDetails.aspx?OrderId={{item.OrderId}}" title="{{item.PayOrderId}}">查看详情</a></span>
                        </td>
                          </tr>
                    {{/each}}
              
            </script>
              <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
        </div>
    </div>
    
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/SendGoodOrders.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/SendGoodOrders.js" type="text/javascript"></script>
</asp:Content>
