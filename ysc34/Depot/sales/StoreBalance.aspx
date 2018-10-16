<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" EnableViewState="false" AutoEventWireup="true" CodeBehind="StoreBalance.aspx.cs" Inherits="Hidistro.UI.Web.Depot.StoreBalance" %>

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
                <li><a href="javascript:void" class="hover">门店结算</a></li>
            </ul>
            <i class="glyphicon glyphicon-question-sign" data-container="body" style="cursor: pointer" data-toggle="popover" data-placement="left" title="操作说明" data-content="门店收款订单是指的由门店收款的已完成状态的订单，主要用于门店与平台进行对账"></i>
        </div>
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <%
                    DateTime sdate = DateTime.Now.AddDays(-(DateTime.Now.Day - 1));
                    DateTime edate = sdate.AddMonths(1);
                %>
                <ul>
                    <li>
                        <span>结算时间：</span>
                        <input name="startDate" type="text" value="<%=sdate.ToString("yyyy-MM-dd") %>" id="startDate" class="forminput form-control" readonly="readonly" style="width: 140px; float: left;" />
                        <span class="Pg_1010">至</span>
                        <input name="endDate" type="text" value="<%=edate.ToString("yyyy-MM-dd") %>" id="endDate" class="forminput form-control" readonly="readonly" style="width: 140px; float: left;" />
                    </li>
                    <li>
                        <abbr class="formselect">
                            <select name="ddlReceivables" id="ddlReceivables" class="forminput form-control">
                                <option selected="selected" value="">请选择收款方</option>
                                <option value="0">平台</option>
                                <option value="1">门店</option>
                            </select>
                        </abbr>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                    <li>
                        <a href="javascript:ExportToExcel()">导出数据</a></li>
                </ul>
            </div>
            <div class="functionHandleArea clearfix" style="font-weight: bold;">
                结算订单总数：<span style="color: red; padding-left: 3px; padding-right: 3px;" id="spanTotalCount">0</span>,总结算金额<span style="color: red; padding-left: 3px; padding-right: 3px;" id="lblOrderSummaryTotal">0.00</span>元
            </div>

            <!--数据列表区域-->
            <table width="100%" border="0" cellspacing="0">
                <tr class="table_title">
                     <th width="12%">结算时间</th>
                                <th width="12%">订单编号</th>
                                <th width="8%">订单实付</th>
                                <th width="8%">退款金额</th>
                                <th width="8%">平台佣金</th>
                                <th width="10%">结算金额</th>
                                <th width="5%">收款方</th>       
                                <th width="5%">操作 </th>       
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

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.CreateTime}}</td>
                              <td>{{if item.TradeType==2}}
                                    <a href="OrderDetails.aspx?OrderId={{item.TradeNo}}">{{item.TradeNo}}</a>
                                  {{else}}
                                     线下订单
                                  {{/if}}
                              </td>
                              <td>{{item.OrderTotal.toFixed(2)}}</td>
                              <td>{{ if item.RefundAmount== 0}}
                              --
                              {{else}}
                              {{item.RefundAmount}}
                                  {{/if}}
                              </td>
                              <td>{{item.PlatCommission.toFixed(2)}}</td>                            
                              <td>{{item.Income.toFixed(2)}}</td>
                                <td>{{if item.CollectByStore==1}}
                                  门店
                                  {{else}}
                                  平台
                                  {{/if}}
                              </td>
                              <td>{{if item.TradeType==2}}
                                  <span class="submit_bianji"><a href="OrderDetails.aspx?OrderId={{item.TradeNo}}" title="{{item.TradeNo}}">查看详情</a></span>
                                   {{else}}
                                    --
                                  {{/if}}
                              </td>                   
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/sales/ashx/StoreBalance.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/sales/scripts/StoreBalance.js" type="text/javascript"></script>
</asp:Content>
