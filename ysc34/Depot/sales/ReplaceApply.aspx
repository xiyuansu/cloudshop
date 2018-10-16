<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" EnableViewState="false" AutoEventWireup="true" CodeBehind="ReplaceApply.aspx.cs" Inherits="Hidistro.UI.Web.Depot.sales.ReplaceApply" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="order.helper.js?v=3.4" type="text/javascript"></script>
    <script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:void(0);" class="statusanchors hover" data-status="">换货申请单</a></li>
                <li>
                    <a href="javascript:void(0);" class="statusanchors" data-status="0">待处理</a></li>
                <li>
                    <a href="javascript:void(0);" class="statusanchors" data-status="3">待用户发货</a></li>
                <li>
                    <a href="javascript:void(0);" class="statusanchors" data-status="4">用户已发货</a></li>
                <li>
                    <a href="javascript:void(0);" class="statusanchors" data-status="6">商家已发货</a></li>
                <li>
                    <a href="javascript:void(0);" class="statusanchors" data-status="1">已完成</a></li>
                <li>
                    <a href="javascript:void(0);" class="statusanchors" data-status="2">已拒绝</a></li>
            </ul>
        </div>

        <div class="datalist clearfix">
            <div class="searcharea">
                <ul>
                    <li><span>订单编号：</span><span>
                        <input name="txtOrderId" type="text" id="txtOrderId" class="forminput form-control" />
                    </span></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                        &nbsp;&nbsp;<a href="javascript:ExportToExcel()" class="btn btn-primary">导出数据</a>
                    </li>
                </ul>
            </div>
            <div class="functionHandleArea">
                <div class="batchHandleArea">
                    <div class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                    </div>
                    
                     <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>
            <!--数据列表区域-->
            <table width="0" border="0" cellspacing="0" class="table table-striped">
                <tr>
                    <th width="50"></th>
                    <th>订单编号</th>
                    <th>会员名</th>
                    <th>申请时间</th>
                    <th>处理状态</th>
                    <th>处理时间</th>
                    <th>操作</th>
                </tr>
                <tbody id="datashow"></tbody>
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
                    <td>
                        <input name="CheckBoxGroup" type="checkbox" value='{{item.ReplaceId}}' class="icheck" /></td>
                    <td><a href="OrderDetails?OrderId={{item.OrderId}}">{{item.PayOrderId}}</a></td>
                    <td>{{item.UserName}}</td>
                    <td>&nbsp;{{ item.ApplyForTime | artex_formatdate:"yyyy-MM-dd"}}</td>
                    <td nowrap="nowrap">{{item.ReplaceStatusStr}}</td>
                    <td>&nbsp;{{item.handleTimeStr}}</td>
                    <td>
                        <span><a href="ReplaceApplyDetail?ReplaceId={{item.ReplaceId}}">{{item.OperText}}</a></span>
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <!--查看物流-->
    <div id="ViewLogistics" style="display: none">
        <div class="frame-content" style="margin-top: -20px;">
            <h1>快递单物流信息</h1>

            <div id="expressInfo">正在加载中....</div>
        </div>
    </div>

    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/sales/ashx/ReplaceApply.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/sales/scripts/ReplaceApply.js" type="text/javascript"></script>

</asp:Content>
