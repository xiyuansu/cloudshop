<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.BalanceStatistics" MasterPageFile="~/Admin/Admin.Master" CodeBehind="BalanceStatistics.aspx.cs" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>





<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix" id="datalist">
            <!--搜索-->
            <!--结束-->
            <div class="searcharea clearfix ">
                <ul class="a_none_left">
                    <li>
                        <span>用户名：</span>
                        <input type="text" id="txtUserName" class="forminput form-control" />
                    </li>
                    <li>
                        <span>交易时间从：</span>
                        <Hi:CalendarPanel runat="server" ID="calendarStart" ClientIDMode="Static"></Hi:CalendarPanel>
                        <span class="Pg_1010">至：</span>
                        <Hi:CalendarPanel runat="server" ID="calendarEnd" ClientIDMode="Static" IsEndDate="true"></Hi:CalendarPanel>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                    <li>
                        <p><a href="javascript:Post_ExportExcel()">导出数据</a></p>
                    </li>
                </ul>
            </div>


            <table class="table table-striped">
                <tr>
                    <th scope="col" width="180">用户名</th>
                    <th scope="col">交易时间</th>
                    <th scope="col" width="130">业务摘要</th>
                    <th scope="col" width="140">转入金额</th>
                    <th scope="col" width="140">转出金额</th>
                    <th scope="col" width="140">当前余额</th>
                </tr>

                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
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
                    <td>{{item.UserName}}</td>
                    <td>{{item.TradeDate |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.TradeTypeText}}</td>
                    <td>{{if item.Income}}
                        {{item.Income.toFixed(2)}}
                        {{else}}
                        -
                        {{/if}}
                    </td>
                    <td>{{if item.Expenses}}
                        {{item.Expenses.toFixed(2)}}
                        {{else}}
                        -
                        {{/if}}</td>
                    <td>{{if item.Balance}}
                        {{item.Balance.toFixed(2)}}
                        {{else}}
                        -
                        {{/if}}</td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/BalanceStatistics.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/BalanceStatistics.js" type="text/javascript"></script>
</asp:Content>

