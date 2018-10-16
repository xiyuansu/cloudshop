<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FightGroupActivitiyList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.FightGroupActivitiyList" MasterPageFile="~/Admin/Admin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <script type="text/javascript">
        function openF(fightGroupActivityId) {
            var url = "/admin/promotion/FightGroupShareLink.aspx?fightGroupActivityId=" + fightGroupActivityId;
            DialogFrame(url, "拼团链接", 800, 260);
        }


    </script>

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddFightGroupActivitiy.aspx">添加</a></li>
                <li><a href="FightGroupActivitiySeeting.aspx">拼团设置</a></li>
                <%--<li><a href="SetFightGroupActivitiy.aspx">设置</a></li>--%>
            </ul>
        </div>
        <!--搜索-->

        <!--结束-->

        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul class="a_none_left">

                    <li><span>商品名称：</span><span>
                        <input type="text" id="txtProductName" class="forminput form-control" /></span></li>

                    <li><span>活动状态：</span><span><asp:DropDownList ID="ddlStatus" ClientIDMode="Static" runat="server" CssClass="iselect">
                    </asp:DropDownList></span></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th>商品名称</th>
                        <th width="8%">活动状态</th>
                        <th width="10%">活动时间</th>
                        <th width="8%">组团次数</th>
                        <th width="8%">成团次数</th>
                        <th width="8%">排序</th>
                        <th width="16%">操作</th>
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
                    <td>{{item.ProductName}}</td>
                    <td>{{item.StatusText}}</td>
                    <td>{{item.StartDate |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}至<br />
                        {{item.EndDate |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td><a href='FightGroupDetails.aspx?fightGroupActivityId={{item.FightGroupActivityId}}'>{{item.CreateGroupCount}}</a></td>
                    <td>{{item.CreateGroupSuccessCount}}</td>
                     <td>
                        <input name="txtSequence" type="text" value="{{item.DisplaySequence}}" data-id="{{item.FightGroupActivityId}}" class="forminput form-control txtSequence" style="width: 50px;" />
                    </td>
                    <td>{{if item.StatusText!="已结束"}}
                        <span><a href="javascript:openF({{item.FightGroupActivityId}});">链接</a></span>
                        {{/if}}
                        {{if item.StatusText=="即将开始"}}
                        <span><a href="EditFightGroupActivitiy.aspx?fightGroupActivityId={{item.FightGroupActivityId}}">编辑</a></span>
                        {{else if item.StatusText=="正在进行"}}
                        <span><a href="EditFightGroupActivitiyBeingCarried.aspx?fightGroupActivityId={{item.FightGroupActivityId}}">编辑</a></span>
                        {{/if}}
                        <span><a href="javascript:Post_Delete('{{item.FightGroupActivityId}}')">删除</a></span>
                        <span><a href="ViewFightGroupActivitiy.aspx?fightGroupActivityId={{item.FightGroupActivityId}}">查看</a></span>
                    </td>

                </tr>
        {{/each}}
      
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/FightGroupActivitiyList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/FightGroupActivitiyList.js?v=3.4" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

