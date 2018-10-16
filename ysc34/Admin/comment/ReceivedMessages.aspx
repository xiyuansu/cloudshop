<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ReceivedMessages" MasterPageFile="~/Admin/Admin.Master" CodeBehind="ReceivedMessages.aspx.cs" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <input type="hidden" name="hidFilter" id="hidFilter" value="<%=MessageStatus %>" />

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:void(0);" class="tab_status" data-filter="1">收件箱</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-filter="2">已回复</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-filter="3">未回复</a></li>
            </ul>
            <%--<i class="glyphicon glyphicon-question-sign" data-container="body" style="cursor: pointer" data-toggle="popover" data-placement="left" title="操作说明" data-content="你可以查看回复删除会员发送给你的站内消息."></i>--%>
        </div>
        <!--搜索-->
        <div class="functionHandleArea m_none pb30">
            <!--分页功能-->


            <!--结束-->
            <div class="batchHandleArea">
                <ul>
                    <li class="batchHandleButton">
                        <span class="checkall">
                            <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                        <a href="javascript:bat_delete()" class="btn btn-default ml0 submit66">删除</a>

                    </li>
                </ul>
                <div class="pageHandleArea pull-right">
                    <ul>
                        <li class="paginalNum">
                            <span>每页显示数量：</span>
                            <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                        </li>
                    </ul>
                </div>

            </div>

        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th width="5%"></th>
                        <th width="20%">标题</th>
                        <th width="15%">发件人</th>
                        <th width="15%">时间</th>
                        <th>内容</th>
                        <th width="5%" class="text-center">操作</th>
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
                <td><span class="icheck">
                    <input name="CheckBoxGroup" type="checkbox" value='{{item.MessageId}}' />
                </span></td>
                <td>{{item.Title}}</td>
                <td><a class="c-666" href="/admin/member/MemberDetails?userId={{item.UserId}}">{{item.Sernder}}</a></td>
                <td>{{item.Date | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                <td>{{item.Content}}</td>
                <td width="5%" class="text-center"><a href='/admin/comment/ReplyReceivedMessages.aspx?MessageId={{item.MessageId}}'>回复</a></td>
            </tr>
        {{/each}}

    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/comment/ashx/ReceivedMessages.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/comment/scripts/ReceivedMessages.js" type="text/javascript"></script>
</asp:Content>

