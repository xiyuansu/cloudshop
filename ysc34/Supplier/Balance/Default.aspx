<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.Balance.Default" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a id="all" href="javascript:return false;">提现管理</a></li>
            </ul>
        </div>
        <div class="datalist">
            <blockquote class="blockquote-default blockquote-tip">
                <span>
                    <em>可提现余额</em>
                    <Hi:FormatedMoneyLabel ID="litAccountAmount" runat="server" />
                </span>
                <span>

                    <em>提现冻结金额</em>
                    <Hi:FormatedMoneyLabel ID="litRequestBalance" runat="server" /></span>
                <span>

                    <em>已提现金额</em>
                    <Hi:FormatedMoneyLabel ID="litOutBalance" runat="server" /></span>

                <span><a class="a_link" href="RequestBalanceDraw.aspx">申请提现</a> </span>
                <span>
                    <a href="javascript:ExportToExcelBalance()" class="a_link">生成报告</a>
                </span>

            </blockquote>
            <div class="searcharea">

                <ul>
                    <li><span>申请时间：</span><span>
                        <Hi:CalendarPanel runat="server" ID="calendarStartDate" ClientIDMode="Static"></Hi:CalendarPanel>
                    </span><span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarEndDate" IsEndDate="true" ClientIDMode="Static"></Hi:CalendarPanel>
                        </span></li>

                    <li><span>提现状态:</span><span>

                        <select class="iselect" id="ddlType">
                            <option value="0">请选择状态</option>
                            <option value="1">审核中</option>
                            <option value="2">已通过审核</option>
                            <option value="3">拒绝</option>
                        </select>
                    </span>

                    </li>
                    <li>

                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                </ul>
            </div>
            <!--数据列表区域-->
            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <tbody>
                            <thead>
                                <th scope="col" style="width: 10%">申请时间</th>
                                <th scope="col" style="width: 10%">提现金额</th>
                                <th scope="col" style="width: 15%">提现方式</th>
                                <th scope="col" style="width: 10%">收款人</th>
                                <th scope="col" style="width: 15%">收款账号</th>
                                <th scope="col" style="width: 10%">状态</th>
                                <th scope="col" style="width: 20%">拒绝理由</th>
                                <th scope="col" style="text-align: center; width: 10%;">放款时间</th>
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

        </div>
    </div>

    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.RequestTime | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}} 
                    </td>
                    <td>{{item.Amount.toFixed(2)}}   
                    </td>
                    <td>{{item.DrawType}}</td>
                    <td>{{item.ReceiverName}}</td>
                    <td>{{item.ReceiverID}}</td>
                    <td>{{item.StateStr}}</td>
                    <td style="width: 130px;">{{item.ManagerRemark}}</td>
                    <td style="text-align: center;">{{item.AccountDateStr}}</td>
                </tr>
        {{/each}}
                  
    </script>
    <input type="hidden" name="dataurl" id="dataurl" value="/Supplier/Balance/ashx/BalanceManage.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Supplier/Balance/scripts/BalanceManage.js" type="text/javascript"></script>
</asp:Content>
