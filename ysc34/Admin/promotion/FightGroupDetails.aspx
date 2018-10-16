<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="FightGroupDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.FightGroupDetails" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.Core" Assembly="Hidistro.Core" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <input type="hidden" id="hidFightGroupActivityId" name="hidFightGroupActivityId" value="<%=fightGroupActivityId %>" />
    <!--选项卡-->

    <script type="text/javascript">
        function lookOrders(fightGroupId) {
            var url = "/admin/promotion/FightGroupOrders.aspx?fightGroupId=" + fightGroupId;
            DialogFrame(url, "活动订单", 500, 262);
        }

    </script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="FightGroupActivitiyList.aspx">管理</a></li>
                <li class="hover"><a>组团明细</a></li>
            </ul>
        </div>
        <!--搜索-->

        <!--结束-->

        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul class="a_none_left">
                    <li><span>组团状态：</span><span><asp:DropDownList ID="ddlStatus" ClientIDMode="Static" runat="server" CssClass="iselect"></asp:DropDownList></span></li>
                    <li><span>组团时间：</span><span>
                        <Hi:CalendarPanel runat="server" ID="CPStartTime" ClientIDMode="Static" Width="150"></Hi:CalendarPanel></span><span>-</span><span>
                            <Hi:CalendarPanel runat="server" ID="CPEndTime" ClientIDMode="Static" Width="150"></Hi:CalendarPanel></span></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>

            <!--S DataShow-->
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th>组团时间</th>
                        <th>组团状态</th>
                        <th>成团时间</th>
                        <th>成团人数/已参团人数</th>
                        <th>组团订单</th>
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
                    <td>{{item.StartTime | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.GroupStatusText}}</td>
                    <td>{{item.CreateTimeText |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.PersonNumber}}</td>
                    <td>
                        <span><a href="javascript:lookOrders('{{item.FightGroupId}}');">查看订单</a></span>
                    </td>

                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/FightGroupDetails.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/FightGroupDetails.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

