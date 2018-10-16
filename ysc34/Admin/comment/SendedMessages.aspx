<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.SendedMessages" MasterPageFile="~/Admin/Admin.Master" CodeBehind="SendedMessages.aspx.cs" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>






<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a>发件箱</a></li>
                <li><a href="SendMessage.aspx">发送站内信</a></li>
            </ul>
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
                        <th width="15%">收件人</th>
                        <th width="15%">时间</th>
                        <th>内容</th>
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
                <td><a class="c-666" href="/admin/member/MemberDetails?userId={{item.AccepterUserId}}">{{item.Accepter}}</a></td>
                <td>{{item.Date | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                <td>{{item.Content}}</td>
            </tr>
        {{/each}}

    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/comment/ashx/SendedMessages.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/comment/scripts/SendedMessages.js" type="text/javascript"></script>
</asp:Content>
