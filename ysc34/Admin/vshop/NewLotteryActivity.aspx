<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="NewLotteryActivity.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.NewLotteryActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        .title { clear: both; }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">
                    <asp:Literal ID="LitTitle" runat="server"></asp:Literal></a></li>
                <li><a id="addactivity" runat="server">添加</a></li>

            </ul>
        </div>
        <div class="searcharea">
            <ul>
                <li>
                    <span>活动名称：</span>
                    <span>
                        <input type="text" id="txtSearchText" class="forminput form-control float" value="<%=ActivityName %>" />
                    </span>
                </li>
                <li id="liStoreFilter" runat="server">
                    <abbr class="formselect">
                        <select id="ddlStateus" class="iselect">
                            <option value="">请选择状态</option>
                            <option value="1" <%=OrderStatus==1?"selected":"" %>>进行中</option>
                            <option value="2" <%=OrderStatus==2?"selected":"" %>>未开始</option>
                            <option value="3" <%=OrderStatus==3?"selected":"" %>>已结束</option>
                        </select>
                    </abbr>

                </li>
                <li>
                    <input type="hidden" name="hidType" id="hidType" value="<%=type %>" />
                    <span>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary"></span>
                </li>
            </ul>
        </div>

        <!--数据列表区域-->
        <div class="datalist clearfix top_10">
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th width="20%">活动名称</th>
                        <th width="30%">奖项</th>
                        <th width="15%" style="text-align: center;">状态</th>
                        <th width="15%">活动时间</th>
                        <th>操作</th>
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
                    <td>{{item.ActivityName}}</td>
                    <td>{{each item.AwardItem as aitem aindex}}
                        {{if aindex>0}}<br />
                        {{/if}}{{#aitem.AwardGradeText}}:{{#aitem.PrizeType}}
                        {{/each}}
                    </td>
                    <td style="text-align: center;">{{item.StatusText}}</td>
                    <td>{{ item.StartDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}至<br />
                        {{ item.EndDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td class="operation">{{if item.StatusText!="未开始"}}
                        <a href="PrizeRecordShow.aspx?ID={{item.ActivityId}}&typeid={{item.ActivityType}}">中奖信息</a>
                        {{/if}}
                        <a href="javascript:DialogFrame('vshop/BigWheelLink.aspx?ActivityID={{item.ActivityId}}&typeid={{item.ActivityType}}', '活动二维码', 320, 200, null);">链接</a>
                        {{if item.StatusText!="已结束"}}
                        <a href="EidVLotteryActivity.aspx?ID={{item.ActivityId}}&typeid={{item.ActivityType}}">编辑</a>
                        {{/if}}
                        {{if item.StatusText=="进行中"}}
                        <span><a href="javascript:Post_SetOver('{{item.ActivityId}}')">结束活动</a></span>
                        {{/if}}
                        {{if item.StatusText=="未开始"}}
                        <a href="javascript:Post_Delete('{{item.ActivityId}}')">删除</a>
                        {{/if}}
                        
                    </td>

                </tr>
        {{/each}}
           
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/vshop/ashx/NewLotteryActivity.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/vshop/scripts/NewLotteryActivity.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
