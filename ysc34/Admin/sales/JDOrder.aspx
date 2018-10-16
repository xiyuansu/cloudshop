<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="JDOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.JDOrder" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth">
        <!--搜索-->
        <div class="searcharea">
            <ul>
                <li><span>选择时间：</span>
                    <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldStartDate"></Hi:CalendarPanel>
                    <span class="Pg_1010">至</span>
                    <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldEndDate" IsEndDate="true"></Hi:CalendarPanel>
                </li>
                <li>
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                </li>
            </ul>
            <div style="clear: both;">默认返回前一个月等待出库订单；查询时间不能超过一个月</div>
        </div>
        <!--结束-->
        <div class="functionHandleArea clearfix m_none">


            <div class="batchHandleArea">

                <div class="batchHandleButton">
                    <span class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                    <a class="btn btn-warning ml20" href="javascript:bat_Down()">下载所选京东订单</a>
                </div>

                <!--分页功能-->
                <div class="paginalNum">
                    <span>每页显示数量：</span>
                    <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                </div>
            </div>
        </div>
        <input type="hidden" id="hidOrderId" runat="server" />
        <!--数据列表区域-->
        <div class="datalist clearfix">

            <!--S DataShow-->

            <div class="order">
                <div class="order_title">
                    <table style="width: 100%; margin: 1px;">
                        <tr style="height: 48px;">
                            <td style="width: 60%; padding-left: 15px;">商品名</td>
                            <td style="width: 10%">商品金额</td>
                            <td style="width: 10%">数量</td>
                            <td style="width: 10%">支付方式</td>
                            <td style="width: 10%">订单金额</td>
                        </tr>
                    </table>
                </div>
                <div id="datashow"></div>
            </div>
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
        <div class="order_hover">
            <div class="order_info_title">
                <span class="order_title_checkall">{{if item.IsExsit}}
                                <input type="checkbox" class="icheck" name="CheckBoxGroup" value="{{item.OrderId}}" />
                    {{/if}}
                                <span style="color: red; font-weight: bold;">{{item.Status}}</span>
                </span>
                <span class="order_title_usename">订单号：{{item.OrderId}}（{{item.Products.length}}）</span>
                <span class="order_title_con_1">提交时间：{{item.CreatedAt |artex_formatdate:"yyyy:MM:dd HH:mm:ss"}}</span>
            </div>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 80%;">
                        <table style="width: 100%;">
                            {{each item.Products as pitem pi}}
                                <tr style="height: 48px;">
                                    <td style="width: 75%; padding-left: 10px;">{{pitem.SkuName}}</td>
                                    <td style="width: 12.5%;">¥{{pitem.Price}}</td>
                                    <td style="width: 12.5%;">{{pitem.Total}}</td>
                                </tr>
                            {{/each}}
                        </table>
                    </td>
                    <td style="width: 10%;">{{item.PayType}}</td>
                    <td style="width: 10%;">¥{{item.OrderPayment}}</td>
                </tr>
            </table>

        </div>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/sales/ashx/JDOrder.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/sales/scripts/JDOrder.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
