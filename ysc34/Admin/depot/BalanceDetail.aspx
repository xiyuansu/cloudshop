<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BalanceDetail.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.BalanceDetail" %>

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
                <li><a href="storeAccount.aspx">管理</a></li>
                <li class="hover"><a href="javascript:return false;">收支明细</a></li>
            </ul>
        </div>
        <div class="datalist">
            <div class="searcharea">
                <ul>
                     <li><span>门店：</span><span style="padding-left:7px;text-overflow: ellipsis;overflow: hidden;white-space: nowrap;"><asp:Label ID="lblName" ClientIDMode="Static" runat="server" Text="" CssClass="j_storeName"></asp:Label></span><asp:HiddenField runat="server" ID="hidStoreId" ClientIDMode="Static" />
                    </li>
                    <li><span>时间：</span><span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarStartDate"></Hi:CalendarPanel>
                    </span><span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarEndDate" IsEndDate="true"></Hi:CalendarPanel>
                        </span></li>

                    <li><span>类型:</span><span>
                        <asp:DropDownList ID="ddlType" ClientIDMode="Static" runat="server" CssClass="iselect">
                            <asp:ListItem Text="类型" Value="0"></asp:ListItem>
                            <asp:ListItem Text="提现" Value="1"></asp:ListItem>
                            <asp:ListItem Text="商品交易" Value="2"></asp:ListItem>
                        </asp:DropDownList></span></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                    <li>
                        <p><a href="javascript:Post_ExportExcel()">生成报告</a></p>
                    </li>

                </ul>
            </div>

            <!--S DataShow-->

            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th width="150">时间</th>
                        <th>类型</th>
                        <th>订单号</th>
                        <th>收入</th>
                        <th>支出</th>
                        <th>账户余额</th>
                        <th>操作</th>
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
                    <td>{{item.TradeDate |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{if item.TradeType==1}}
                        提现
                        {{else}}
                        商品交易
                        {{/if}}
                    </td>
                    <td>{{if item.TradeNo}}
                        {{if item.TradeType!=1}}
                        <a href="/admin/sales/OrderDetails.aspx?OrderId={{item.TradeNo}}">{{item.TradeNo}}</a>
                        {{else}}
                        {{item.TradeNo}}
                        {{/if}}
                        {{else}}
                        --
                        {{/if}}</td>
                    <td>{{if item.Income}}
                        {{item.Income.toFixed(2)}}
                        {{else}}
                        --
                        {{/if}}</td>
                    <td>{{if item.Expenses}}
                        {{item.Expenses.toFixed(2)}}
                        {{else}}
                        --
                        {{/if}}</td>
                    <td>{{item.Balance.toFixed(2)}}</td>
                    <td>{{if item.TradeType==1}}<a href='javascript:void(0);' onclick="javascript:DialogFrame('/admin/depot/BalanceDrawRequestDetail?Name={{item.UserName}}&OrderId={{item.TradeNo}}', '账户详情', 610, 280, null);">详情</a>{{/if}}</td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/depot/ashx/BalanceDetail.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/depot/scripts/BalanceDetail.js" type="text/javascript"></script>
</asp:Content>
