<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" EnableViewState="false" AutoEventWireup="true" CodeBehind="GroupBuys.aspx.cs" Inherits="Hidistro.UI.Web.Admin.GroupBuys" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddGroupBuy.aspx">添加</a></li>
            </ul>
        </div>
        <!--搜索-->

        <!--结束-->

        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul class="a_none_left">
                    <li><span>商品名称：</span><span><input type="text" id="txtProductName" class="forminput form-control" />
                    </span></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>

            <div class="functionHandleArea clearfix m_none">
                <!--分页功能-->


                <!--结束-->
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton">
                            <span class="checkall">
                                <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                            <span class="btn btn-default ml0"><a href="javascript:bat_delete()">删除</a></span>
                        </li>
                    </ul>
                    <div class="pageHandleArea pull-right">
                        <ul>
                            <li class="paginalNum"><span>每页显示数量：</span>
                                <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th width="5%"></th>
                        <th width="29%">商品名称</th>
                        <th width="10%">状态</th>
                        <th width="20%">活动时间</th>
                        <th width="5%">限购</th>
                        <th width="5%">订单</th>
                        <th width="8%">当前价格</th>
                        <th width="8%">排序</th>
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
    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td><span class="icheck">
                        <input type="checkbox" name="CheckBoxGroup" value="{{item.GroupBuyId}}" /></span></td>
                    <td width="29%">
                        <a class="c-666 text-ellipsis" href='../../GroupBuyProductDetails.aspx?groupBuyId={{item.GroupBuyId}}' target="_blank">{{item.ProductName}}</a>
                    </td>
                    <td width="10%"><span>{{item.StatusText}}</span></td>
                    <td width="20%">开始时间：{{ item.StartDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}<br />
                        结束时间：{{ item.EndDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td width="5%">{{item.MaxCount}}</td>
                    <td width="5%">
                        <a href='{{if item.SupplierId=="0"}}../sales/ManageOrder.aspx{{else}}../supplier/order/ManageOrder.aspx{{/if}}?orderStatus=0&GroupBuyId={{item.GroupBuyId}}'>{{item.OrderCount}}</a>
                    </td>
                    <td width="8%">{{item.CurrentPrice.toFixed(2)}}</td>
                    <td width="8%">
                        <input name="txtSequence" type="text" value="{{item.DisplaySequence}}" data-id="{{item.GroupBuyId}}" class="forminput form-control txtSequence" style="width: 50px;" /></td>

                    <td width="10%" class="operation">
                        <span><a href='EditGroupBuy.aspx?GroupBuyId={{item.GroupBuyId}}'>编辑</a></span>
                        {{if item.CanDelete}}
                        <span><a href="javascript:Post_Delete('{{item.GroupBuyId}}')">删除</a></span>
                        {{/if}}
                    </td>

                </tr>
        {{/each}}
     
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/GroupBuys.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/GroupBuys.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

