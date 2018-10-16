<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="Balancedetail.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.Balance.Balancedetail" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a id="all" href="javascript:return false;">收支明细</a></li>
            </ul>
        </div>
        <div class="datalist">
            <div class="searcharea">
                <ul>
                    <li><span>时间：</span><span>
                        <Hi:CalendarPanel runat="server" ID="calendarStartDate" ClientIDMode="static"></Hi:CalendarPanel>
                    </span><span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarEndDate" IsEndDate="true" ClientIDMode="static"></Hi:CalendarPanel>
                        </span>
                    </li>

                    <li><span>类型：</span><span>
                        <select class="iselect" id="ddlType">
                            <option value="0">请选择类型</option>
                            <option value="1">提现</option>
                            <option value="2">商品交易</option>
                        </select>
                    </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                        <li>
                            <p>
                                <a href="javascript:ExportToExcelBalancedetail()">生成报告</a>
                            </p>
                        </li>
                </ul>
            </div>

            <div class="dataarea mainwidth databody">
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th scope="col" style="width: 15%;">时间</th>
                                <th scope="col" style="width: 15%;">类型</th>
                                <th scope="col" style="width: 15%;">单号</th>
                                <th scope="col" style="width: 10%;">收入</th>
                                <th scope="col" style="width: 10%;">支出</th>
                                <th scope="col" style="width: 15%;">账户余额</th>
                                <th scope="col" style="text-align: center; width: 20%">备注</th>
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
                    <td>{{item.TradeDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}} </td>
                    <td>{{item.TradeTypeStr}}   
                    </td>
                    <td>{{item.OrderIdStr}}</td>
                    <td>{{item.IncomeStr}}</td>
                    <td>{{item.ExpensesStr}}</td>
                    <td>{{item.Balance.toFixed(2)}}</td>

                    <td style="text-align: center;">{{item.Remark}}</td>
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
    <script src="/Supplier/Balance/scripts/BalancedetailManage.js" type="text/javascript"></script>
</asp:Content>
