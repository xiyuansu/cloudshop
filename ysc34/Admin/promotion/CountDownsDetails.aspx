<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="CountDownsDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.CountDownsDetails" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <input type="hidden" id="hidCountDownId" name="hidCountDownId" value="<%=countDownId %>" />
        <asp:HiddenField ID="hidOpenMultStore" Value="0" runat="server" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="CountDowns.aspx" id="aReturnUrl" runat="server">限时抢购</a></li>
                <li class="hover"><a>详情</a></li>
            </ul>
        </div>
         <asp:Panel ID="pnlStoreDetail" runat="server">
            <blockquote class="blockquote-default blockquote-tip">
                <asp:Literal ID="ltStoreNames" runat="server"></asp:Literal>
            </blockquote> 
             <div class="searcharea">
                <ul class="a_none_left">
                    <li><span>活动门店：</span><span>
                        <asp:DropDownList ID="ddlStores" runat="server" DataTextField="StoreName" DataValueField="StoreId"  CssClass="iselect"></asp:DropDownList></span></li>
                    <li><span>订单状态: </span>
                        <span><asp:DropDownList ID="ddlStatus" runat="server" CssClass="iselect">
                         <asp:ListItem Text="请选择" Value="0"></asp:ListItem>
                         <asp:ListItem Text="待付款" Value="1"></asp:ListItem>
                         <asp:ListItem Text="待发货" Value="2"></asp:ListItem>
                         <asp:ListItem Text="交易完成" Value="5"></asp:ListItem>
                         <asp:ListItem Text="交易关闭" Value="4"></asp:ListItem>
                              </asp:DropDownList>
                            </span>
                            </li>
                    <li>
                        <asp:HiddenField ClientIDMode="Static" ID="hidState" runat="server" />
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>
        </asp:Panel>

        <div class="datalist clearfix">
            <!--S DataShow-->
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th scope="col">会员名</th>
                        <th width="150">订单编号</th>
                        <th width="150">下单时间</th>
                        <th width="140">订单实收款(元)</th>
                        <th width="80">抢购数量</th>
                        <th width="100">订单状态</th>
                        <th class="thHead">门店</th>
                        <th width="80">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
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
    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td style="text-overflow: ellipsis; white-space: nowrap; overflow: hidden;">{{item.Username}}</td>
                    <td>{{item.PayOrderId}}</td>
                    <td>{{ item.OrderDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.OrderTotal.toFixed(2)}}</td>
                    <td>{{item.Quantity}}</td>
                    <td>{{item.OrderStatusText}}</td>
                    <td  class="thHead">{{item.StoreName}}</td>
                    <td><span class="submit_shanchu">
                        <a href="/admin/sales/OrderDetails.aspx?OrderId={{item.OrderId}}">查看订单</a>
                    </span>
                    </td>

                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/CountDownsDetails.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/CountDownsDetails.js?v=2" type="text/javascript"></script>
</asp:Content>
