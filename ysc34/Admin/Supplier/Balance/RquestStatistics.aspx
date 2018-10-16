<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" EnableViewState="false" AutoEventWireup="true" CodeBehind="RquestStatistics.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.Balance.RquestStatistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a id="all" href="javascript:return false;">财务管理</a></li>
            </ul>
        </div>
        <div class="datalist">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li><span>供应商名称：</span><span><input type="text" id="txtSearchText" class="forminput form-control" /></span></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                     <li>
                        <p><a href="javascript:Post_ExportExcel()">导出数据</a></p>
                    </li>
                </ul>
            </div>
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th scope="col" width="20%">供应商名称</th>
                        <th scope="col"  width="15%">联系电话</th>
                        <th scope="col"  width="15%">可提现余额</th>
                        <th scope="col"  width="15%">已冻结提现</th>
                        <th scope="col"  width="15%">已提现总额</th>
                        <th scope="col" >操作</th>
                    </tr>
                </thead>
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
                    <td>{{item.SupplierName}}</td>
                    <td>{{item.Tel}}</td>
                    <td>{{item.AvailableBalance.toFixed(2)}}</td>
                    <td>{{item.BalanceFozen.toFixed(2)}}</td>
                    <td>{{item.BalanceOut.toFixed(2)}}</td>
                    <td style="white-space: nowrap;">
                        <div class="operation">
                            <span><a href="BalanceOrder.aspx?BalanceOver=0&SupplierId={{item.SupplierId}}&Name={{item.SupplierName}}">结算明细</a></span>
                            <span><a href="BalanceDetail.aspx?SupplierId={{item.SupplierId}}&Name={{item.SupplierName}}">收支明细</a></span>
                        </div>
                    </td>
                </tr>
        {{/each}}
        </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/Supplier/Balance/ashx/RquestStatistics.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/Supplier/Balance/scripts/RquestStatistics.js?v=3.2" type="text/javascript"></script>
</asp:Content>
