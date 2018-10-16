<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SplittinDrawRecord.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SplittinDrawRecord" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>分销员：</span>
                        <span>
                            <input type="text" id="txtUserName" class="forminput form-control" /></span>
                    </li>
                    <li>
                        <span>选择时间段：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarStart" ClientIDMode="Static"></Hi:CalendarPanel></span>
                        <span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarEnd" IsEndDate="true" ClientIDMode="Static"></Hi:CalendarPanel></span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                    <li id="clickTopDown" onclick="javascript:Post_ExportExcel()" style="cursor: pointer;">
                        <i class="glyphicon glyphicon-save c-666" aria-hidden="true"></i>导出数据</li>
                </ul>
            </div>
            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th scope="col" style="width: 20%;">分销员</th>
                        <th scope="col" style="width: 25%;">申请时间</th>
                        <th scope="col" style="width: 12%;">提现金额</th>
                        <th scope="col" style="width: 10%;">操作人</th>
                        <th scope="col">备注</th>
                        <th scope="col">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>

            <div class="blank12 clearfix"></div>
            <!--E DataShow-->
            <!--E warp-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <tr>
                    <td>
                        <a href="/admin/member/EditMember.aspx?userId={{item.UserId}}">{{item.UserName}}</a>
                    </td>
                    <td>{{item.RequestDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.Amount.toFixed(2)}}</td>
                    <td>{{item.ManagerUserName}}</td>
                    <td>{{item.Remark}}</td>
                    <td>{{if item.IsWeixin}}{{else}}<a href='javascript:void(0);' onclick="javascript:DialogFrame('/admin/member/SplittinDrawsDetail?JournalNumber={{item.JournalNumber}}&Name={{item.UserName}}', '账户详情', 610, 280, null);">详情</a>{{/if}}</td>
                </tr>
                {{/each}}
            
            </script>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/SplittinDrawRequestManage.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/SplittinDrawRecordManage.js?v=3.2" type="text/javascript"></script>
</asp:Content>
