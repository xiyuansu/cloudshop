<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" EnableViewState="false" AutoEventWireup="true" CodeBehind="ManageLotteryTicket.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageLotteryActivity" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddLotteryTicket.aspx">添加</a></li>

            </ul>
        </div>
        <!-- 添加按钮-->
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist clearfix">

            <!--S DataShow-->
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th>活动名称</th>
                        <th width="100">活动关键字</th>
                        <th width="100">活动开始时间</th>
                        <th width="150">抽奖开始时间</th>
                        <th width="100">活动结束时间</th>
                        <th width="200">操作</th>
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

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.ActivityName}}<br />
                       <div style="width: 230px;text-overflow : ellipsis;
white-space : nowrap;
overflow : hidden; "> <%=BaseTicketUrl%>{{item.ActivityId}}</div></td>
                    <td>{{item.ActivityKey}}</td>
                    <td>{{ item.StartTime | artex_formatdate:"yyyy-MM-dd"}}</td>
                    <td>{{ item.OpenTime | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{ item.EndTime | artex_formatdate:"yyyy-MM-dd"}}</td>
                    <td class="operation">
                        <a href="EditLotteryTicket.aspx?ID={{item.ActivityId}}">编辑</a>
                        {{if item.CanStart}}
                        <span><a href="javascript:Post_SetStatus('{{item.ActivityId}}')">立即开始</a></span>
                        {{/if}}
                        <a href="javascript:Post_Delete('{{item.ActivityId}}')">删除</a>
                        <a href="PrizeRecord.aspx?typeid=4&id={{item.ActivityId}}">查看中奖信息</a>
                    </td>

                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/vshop/ashx/ManageLotteryTicket.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/vshop/scripts/ManageLotteryTicket.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
