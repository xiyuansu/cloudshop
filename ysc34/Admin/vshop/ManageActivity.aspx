<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageActivity.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageActivity" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddActivity.aspx">添加</a></li>

            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--S DataShow-->
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th scope="col" width="300">活动名称</th>
                        <th scope="col">关键字</th>
                        <th scope="col" width="80">报名人数</th>
                        <th scope="col" width="200">活动时间</th>
                        <th class="td_left td_right_fff" scope="col" width="260">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--E DataShow-->
        </div>
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td style="word-break: break-all;">{{item.Name}}<br />
                        http://<%=Globals.DomainName%>/Vshop/Activity.aspx?id={{item.ActivityId}}
                    </td>
                    <td>{{item.Keys}}</td>
                    <td>
                        <a href="javascript:void(0);" onclick="javascript:DialogFrame('vshop/ActivityDetail.aspx?id={{item.ActivityId}}', '报名详细', 800, null);">{{item.CurrentValue}}</a>
                    </td>
                    <td>{{ item.StartDate | artex_formatdate:"yyyy-MM-dd"}}
                        至
                        {{ item.EndDate | artex_formatdate:"yyyy-MM-dd"}}
                    </td>
                    <td>
                        <div class="operation">
                            <span class="submit_bianji"><a href="/admin/vshop/EditActivity.aspx?id={{item.ActivityId}}">编辑</a></span>
                            <span><a href="javascript:void(0);" onclick="javascript:DialogFrame('vshop/ActivityDetail.aspx?id={{item.ActivityId}}', '报名详细', 800, null);">查看报名人数</a></span>
                            <span class="submit_shanchu">
                                <span><a href="javascript:Post_Delete('{{item.ActivityId}}')">删除</a></span>
                            </span>
                        </div>
                    </td>

                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/vshop/ashx/ManageActivity.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/vshop/scripts/ManageActivity.js" type="text/javascript"></script>
</asp:Content>
