<%@ Page Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="ShopDecoration.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.ShopDecoration" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a class="hover">楼层管理</a></li>
                <li><a href="AddShopDecoration.aspx" >新增楼层</a></li>
            </ul>
            <i class="glyphicon glyphicon-question-sign" data-container="body" style="cursor: pointer" data-toggle="popover" data-placement="left" title="操作说明" data-content="我的门店中所有发货后正常完成的订单(不包括售后订单以及非正常关闭的订单)"></i>
        </div>

        <!--搜索-->
        <div class="datalist clearfix">
            <!--搜索-->
            <table class="table table-striped" width="100%">
                <thead>
                    <tr>
                        <th class="td_right td_left">楼层名称</th>
                        <th class="td_right td_left">关联商品</th>
                        <th class="td_right td_left">楼层排序</th>
                        <th width="10%" class="td_left td_right_fff">操作</th>
                    </tr>
                </thead>
                <tbody id="tbList"></tbody>
            </table>
        </div>
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/sales/ashx/SendGoodOrders.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/home/scripts/ShopDecoration.js?v=3.3" type="text/javascript"></script>
</asp:Content>
