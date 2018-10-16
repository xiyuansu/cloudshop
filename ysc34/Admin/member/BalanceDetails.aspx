<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BalanceDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.BalanceDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:ToList()">预付款管理</a></li>
                <li class="hover"><a href="javascript:void">账户明细</a></li>
            </ul>
        </div>
        <br />
        <div class="VIPbg fonts">
            <ul>
                <li>
                    <asp:Literal ID="litUser" runat="server"></asp:Literal>
                </li>
                <li>预付款总额：<strong class="colorG"><asp:Literal ID="litBalance" runat="server" /></strong></li>
                <li>可用余额：<strong class="colorB"><asp:Literal ID="litUserBalance" runat="server" /></strong></li>
                <li>冻结金额：<strong class="colorQ"><asp:Literal ID="litDrawBalance" runat="server" /></strong></li>
                <li><a href="/admin/member/BalanceDrawRequest.aspx?userId=<%=userId %>">查看提现记录</a></li>
            </ul>
        </div>
        <div class="searcharea clearfix">
            <ul>
                <li>
                    <span>选择时间段：</span>
                    <span>
                        <Hi:CalendarPanel runat="server" ID="calendarStart" ClientIDMode="Static"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarEnd" ClientIDMode="Static" IsEndDate="true"></Hi:CalendarPanel>
                </li>
                <li>
                    <span>类型：</span>
                    <Hi:TradeTypeDropDownList ID="dropTradeType" ClientIDMode="Static" runat="server" CssClass="iselect" NullToDisplay="请选择类型" />
                </li>
                <li>
                    <input type="hidden" name="hidUserId" id="hidUserId" value="<%=userId %>" />
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
            </ul>
        </div>
        <!--结束-->
        <div class="batchHandleArea">
            <ul>
                <li class="batchHandleButton"><a href="javascript:ExportToExcel()" class="btn btn-primary">导出数据</a></li>
            </ul>
            <div class="pageHandleArea pull-right">
                <ul>
                    <li class="paginalNum"><span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList1" runat="server"></Hi:PageSizeDropDownList>
                    </li>
                </ul>
            </div>
        </div>

        <!--数据列表区域-->
        <div class="datalist clearfix" id="datalist">

            <table class="table table-striped">
                <tr>
                    <th scope="col" width="80">流水号</th>
                    <th scope="col" width="100">用户名</th>
                    <th scope="col" width="140">时间</th>
                    <th scope="col" width="100">类型</th>
                    <th scope="col" width="100">收入</th>
                    <th scope="col" width="100">支出</th>
                    <th scope="col" width="100">账户余额</th>
                    <th class="td_left td_right_fff" scope="col">备注</th>
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
                    <td>{{item.JournalNumber}}</td>
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
                    <td>{{item.Remark}}</td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/BalanceDetails.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/BalanceDetails.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
