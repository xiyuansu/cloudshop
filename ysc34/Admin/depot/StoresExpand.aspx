<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="StoresExpand.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.StoresExpand" %>


<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Orders" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        table tr td, th {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>门店名称：</span><span>
                            <input type="text" id="txtStoresName" class="forminput form-control" /></span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                    <li id="clickTopDown" style="cursor: pointer;">
                        <a href="javascript:ExportToExcel()">导出数据</a>
                    </li>
                </ul>
            </div>

            <!--结束-->
            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th width="20%">门店名称</th>
                                <th width="20%">店员</th>
                                <th width="20%">累计发展会员数</th>
                                <th width="20%">累计会员消费</th>
                                <th width="20%">累计发展会员订单数</th>
                            </tr>
                        </thead>
                        <tbody id="datashow"></tbody>
                    </table>
                    <div class="blank12 clearfix"></div>
                </div>
                <!--E DataShow-->
            </div>
            <script id="datatmpl" type="text/html">
                <table class="table table-striped">
                    {{each rows as item index}}
                          <tr>
                              <td>{{item.StoreName}}
                              </td>
                              <td><a href="SalesclerkList.aspx?storeId={{item.StoreId}}">{{item.ManagerCount}}</a></td>
                              <td><a href="MemberDetails.aspx?storeId={{item.StoreId}}">{{item.MemberCount}}</a></td>
                              <td>{{item.ConsumeTotals}}</td>
                              <td>{{item.OrderNumbers}}</td>
                          </tr>
                    {{/each}}
                </table>
            </script>
        </div>
        <!--数据列表底部功能区域-->
        <div class="bottomBatchHandleArea clearfix">
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/StoresExpand.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/StoresExpand.js" type="text/javascript"></script>
</asp:Content>
