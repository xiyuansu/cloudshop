<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.DrawRequestStatistics" MasterPageFile="~/Admin/Admin.Master" CodeBehind="DrawRequestStatistics.aspx.cs" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <div class="searcharea clearfix ">
                <ul class="a_none_left">
                    <li><span>用户名：</span><span>
                        <input type="text" id="txtUserName" class="forminput form-control" /></span></li>
                    <li>
                        <span>提现时间从：</span>
                        <Hi:CalendarPanel runat="server" ID="calendarStart" ClientIDMode="Static"></Hi:CalendarPanel>
                        <span class="Pg_1010">至：</span>
                        <Hi:CalendarPanel runat="server" ID="calendarEnd" ClientIDMode="Static" IsEndDate="true"></Hi:CalendarPanel>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                    <li>
                        <p><a href="javascript:Post_ExportExcel()">导出数据</a></p>
                    </li>
                </ul>
            </div>

            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th scope="col">用户名</th>
                        <th scope="col" width="15%">交易时间</th>
                        <th scope="col" width="15%">业务摘要</th>
                        <th scope="col" width="15%">转出金额</th>
                        <th scope="col" width="15%">当前余额</th>
                        <th scope="col" width="15%">操作人</th>
                        <th scope="col" width="15%">操作</th>
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
                    <td>{{item.UserName}}</td>
                    <td>{{item.TradeDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.TradeTypeText}}</td>
                    <td>{{item.Expenses.toFixed(2)}}</td>
                    <td>{{item.Balance.toFixed(2)}}</td>
                    <td>{{item.ManagerUserName}}</td>
                    <td>{{if item.IsWeiXin}}{{else}}  <a href='javascript:void(0);' onclick="javascript:DialogFrame('/admin/member/BalanceDrawRequestDetail?InpourId={{item.InpourId}}&Name={{item.UserName}}', '账户详情', 610, 280, null);">详情</a>{{/if}}</td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/DrawRequestStatistics.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/DrawRequestStatistics.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

