<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="RefundApply.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.RefundApply" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="order.helper.js?v=3.4" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:void(0);" class="tab_status" data-status="">全部退款申请单</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="0">待处理</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="3">异常</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="1">已处理</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="2">已拒绝</a></li>
            </ul>
        </div>
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul>
                    <li><span>订单编号：</span><span>
                        <input type="text" id="txtOrderId" class="forminput form-control" />
                        <asp:HiddenField ID="hidStatus" ClientIDMode="Static" runat="server" />
                    </span></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                    <li id="errPrompt" style="display:none;">
                        <span>退款账户余额不足会造成退款异常</span>
                    </li>
                </ul>
            </div>
            <!--结束-->
            <div class="functionHandleArea">
                <div class="batchHandleArea">
                    <span class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                    <a href="javascript:bat_delete();" class="btn btn-default">删除</a>&nbsp;&nbsp;
                    <a href="javascript:ExportToExcel()" class="btn btn-primary">导出数据</a>
                    <a href="javascript:BatchDeal()" id="btnBatchDeal" class="btn btn-default" style="display:none;">批量处理</a>
                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>
            <input type="hidden" id="hidOrderId" runat="server" />
            <input type="hidden" id="hidRefundId" runat="server" />
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th width="5%"></th>
                        <th width="18%">订单编号</th>
                        <th width="18%">会员名</th>
                        <th width="10%">退款金额(元)</th>
                        <th width="14%">申请时间</th>
                        <th width="10%">处理状态</th>
                        <th width="15%">处理时间</th>
                        <th width="10%">操作</th>
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
                    <td><span class="icheck">
                        <input type="checkbox" name="CheckBoxGroup" value="{{item.RefundId}}" /></span></td>
                    <td><a href="OrderDetails?OrderId={{item.OrderId}}">{{item.PayOrderId}}{{if item.IsServiceProduct}}<i style="color:red; margin-left:5px;">(服务)</i>{{/if}}</a></td>
                    <td style="text-overflow: ellipsis;overflow: hidden; white-space: nowrap;">{{item.UserName}}</td>
                    <td>{{item.RefundAmount.toFixed(2)}}</td>
                    <td>{{item.ApplyForTime |artex_formatdate:"yyyy-MM-dd"}}</td>
                    <td nowrap="nowrap" title="{{item.ExceptionInfo}}">{{item.StatusStr}}</td>
                    <td>{{item.HandleTime |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>
                         <a href="RefundApplyDetail?RefundId={{item.RefundId}}">{{item.OperText}}</a> 
                    </td>
                </tr>
        {{/each}}
  
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/sales/ashx/RefundApply.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/sales/scripts/RefundApply.js?v=3.3" type="text/javascript"></script>
</asp:Content>
