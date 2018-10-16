<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MemberPointDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.MemberPointDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:ToList()">会员积分</a></li>
                <li class="hover"><a href="javascript:void">积分明细</a></li>
            </ul>
        </div>
        <div class="VIPbg fonts">
            <ul>
                <li class="colorQ">会员：<strong style="color: black"><asp:Literal ID="litUserName" runat="server" /></strong></li>
                <li class="colorQ">可用积分：<strong class="colorB"><asp:Literal ID="litCanUsePoints" runat="server" /></strong></li>
                <li class="colorQ">历史积分：<strong class="colorG"><asp:Literal ID="litHistoryPoints" runat="server" /></strong></li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea a_none mb_0">
                <ul>
                    <li>
                        <span>积分来源：</span>
                        <abbr class="formselect">
                            <Hi:SourcePointDrowpDownList runat="server" ClientIDMode="Static" ID="dropPointSource" NullToDisplay="请选择积分来源" CssClass="iselect"></Hi:SourcePointDrowpDownList>
                        </abbr>
                    </li>
                    <li>
                        <input type="hidden" name="hidUserId" id="hidUserId" value="<%=userId %>" />
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                        &nbsp;&nbsp;<a href="javascript:ExportToExcel()" class="btn btn-primary">导出数据</a>
                    </li>
                </ul>
            </div>
            

            <!--S DataShow-->
            <table class="table table-striped">
                <tr>
                    <th style="width: 220px">积分来源</th>
                    <th style="width: 120px;">积分变化</th>
                    <th>时间</th>
                    <th style="width: 300px">备注</th>
                </tr>
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
                    <td>{{item.TradeTypeName}}
                        {{if item.SignInSource==2}}
                    <img src="/Utility/pics/wap.gif" title="触屏版签到"/>
                        {{else if item.SignInSource==3}}
                    <img src="/Utility/pics/wx.gif" title="微信签到"/>
                        {{else if item.SignInSource==4}}
                    <img src="/Utility/pics/tao.gif" title="生活号签到"/>
                        {{else if item.SignInSource==5}}
                    <img src="/Utility/pics/androi.gif" title="APP签到"/>
                        {{/if}}
                    </td>
                    <td>{{if item.Increased && item.Increased>0}}
                        +{{item.Increased}}
                        {{else}}
                        -{{item.Reduced}}
                        {{/if}}</td>
                    <td>{{item.TradeDate |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.Remark}}</td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/MemberPointDetails.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/MemberPointDetails.js" type="text/javascript"></script>
</asp:Content>
