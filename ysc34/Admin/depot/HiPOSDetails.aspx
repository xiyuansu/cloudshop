<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="HiPOSDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.HiPOSDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <style type="text/css">
        .p { margin: 0px 200px 0px 0px; height: 30px; }

            .p div { width: 30%; float: left; }
    </style>
    <div class="dataarea mainwidth databody">
        <div class="dataarea mainwidth">

            <div class="datalist clearfix">
                <div class="searcharea ">
                    <div class="blockquote-default blockquote-tip clearfix">
                        <div>交易总额：<strong style="color: #333; font-size: 16px;" class="mr_20">￥<span id="lblSumTransactions">0.00</span>&nbsp;</strong> 交易笔数：<strong class="colorE" style="color: #333; font-size: 16px;"><span id="lblNumberTransactions">0</span></strong></div>
                    </div>
                    <ul class="mt_20">
                        <li><span>门店名称：</span>
                            <span>
                                <input type="text" id="txtStoreName" class="forminput form-control" value="<%=storeName %>" />
                            </span>
                        </li>
                        <li></li>
                        <li><span>交易时间：</span> <span>
                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="caStartDate"></Hi:CalendarPanel></span>
                            <span style="margin: 0px 5px;">至</span>
                            <span>
                                <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="caEndDate"></Hi:CalendarPanel>
                            </span></li>
                        <li>
                            <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                        </li>
                    </ul>

                </div>
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped" id="TabOrders">
                        <thead>
                            <tr>
                                <th width="25%">门店名称</th>
                                <th width="20%">pos数量</th>
                                <th width="20%">交易总额</th>
                                <th width="20%">交易笔数</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody id="datashow"></tbody>
                    </table>
                    <div class="blank12 clearfix"></div>
                </div>
                <!--E DataShow-->

                <div class="page">
                    <div class="bottomPageNumber clearfix">
                        <div class="pageNumber">
                            <div class="pagination" id="showpager"></div>
                        </div>
                    </div>
                </div>
                <script id="datatmpl" type="text/html">
                    {{each rows as item index}}
                          <tr>

                              <td>{{item.name}}</td>
                              <td>{{item.DeviceCount}}</td>
                              <td>{{item.total}}
                              </td>
                              <td>{{item.count}}</td>
                              <td>
                                  <a href="javascript:Link('{{item.id}}','{{item.systemStoreId}}')">查看详情</a>
                              </td>
                          </tr>

                    <tr class="c_hidden">
                        <td colspan="5" style="padding: 0px;">
                            <table width="100%" name="tbItem">
                                {{each item.devices as dev devind}}
                            <tr>
                                <td width="25%"></td>
                                <td width="20%">{{dev.device_id}}</td>
                                <td width="20%">{{dev.total}}</td>
                                <td width="20%">{{dev.count}}</td>
                                <td>
                                    <a href="javascript:Link('{{item.id}}','{{item.systemStoreId}}','{{dev.device_id}}')">查看详情</a>
                                </td>
                            </tr>
                                {{/each}}
                            </table>
                        </td>
                    </tr>
                    {{/each}}
                </script>
            </div>

        </div>
        <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/HiPOSDetails.ashx" />
        <script src="/Utility/artTemplate.js" type="text/javascript"></script>
        <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
        <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
        <script src="/admin/depot/scripts/HiPOSDetails.js?v=3.3" type="text/javascript"></script>
</asp:Content>
