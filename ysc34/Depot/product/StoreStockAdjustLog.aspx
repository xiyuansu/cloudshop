<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="StoreStockAdjustLog.aspx.cs" Inherits="Hidistro.UI.Web.Depot.product.StoreStockAdjustLog" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;" id="divSearchBox" runat="server">
                <asp:HiddenField ID="hidProductId" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hidStoreId" ClientIDMode="Static" runat="server" />
                <ul>
                    <li>
                        <span>调整时间：</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldStartDate" Width="160"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldEndDate" Width="160" IsEndDate="true"></Hi:CalendarPanel>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>
            <!--S DataShow-->
            <table class="table table-striped" width="100%">
                <thead>
                    <tr>
                        <th scope="col" width="150">调整时间</th>
                        <th scope="col" width="50%">调整记录</th>
                        <th scope="col">备注</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <div class="blank12 clearfix"></div>
        </div>
        <!--E DataShow-->
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
                    <td>{{item.ChangeTime |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.Content}}</td>
                    <td>{{item.Remark}}</td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/product/ashx/StoreStockAdjustLog.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/product/scripts/StoreStockAdjustLog.js" type="text/javascript"></script>
</asp:Content>
