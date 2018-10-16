<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageRedEnvelope.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.ManageRedEnvelope" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a id="AddRedEnvelope" runat="server">新增</a></li>
            </ul>
        </div>

        <div class="datalist clearfix">
            <!--数据列表区域-->
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th>红包名称</th>
                        <th width="80">已领个数</th>
                        <th width="95">使用条件</th>
                        <th width="95">红包金额</th>
                        <th width="130">开始时间</th>
                        <th width="130">结束时间</th>
                        <th width="130">红包失效时间</th>
                        <th width="60">状态</th>
                        <th width="110">操作</th>
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
                    <td>{{item.Name}}</td>
                    <td>{{item.ActualNumber}}</td>
                    <td>满{{item.EnableIssueMinAmount.toFixed(2)}}元</td>
                    <td>{{if item.MinAmount != item.MaxAmount}}
                        {{item.MinAmount.toFixed(2)}}-
                        {{/if}}
                        {{item.MaxAmount.toFixed(2)}}元
                    </td>
                    <td>{{ item.ActiveStartTime | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{ item.ActiveEndTime | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{ item.EffectivePeriodEndTime | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.StatusText}}</td>
                    <td class="operation">
                        <span class="submit_bianji"><a href="RedEnvelopeDetails.aspx?RedEnvelopeId={{item.Id}}">领取详情</a></span>
                        {{if item.StatusText=="已开启"}}
                        <span class="submit_bianji"><a href="javascript:Post_SetStatus('{{item.Id}}','close')">关闭</a></span>
                        {{/if}}
                        <span class="submit_bianji"><a href="javascript:Post_Delete('{{item.Id}}')">删除</a></span>
                        <span class="submit_bianji"><a href="ViewRedEnvelope?RedEnvelopeId={{item.Id}}">查看</a></span>
                    </td>

                </tr>
        {{/each}}
      
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/vshop/ashx/ManageRedEnvelope.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/vshop/scripts/ManageRedEnvelope.js" type="text/javascript"></script>
</asp:Content>
