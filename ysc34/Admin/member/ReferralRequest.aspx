<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReferralRequest.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ReferralRequest" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>用户名：</span>
                        <span>

                            <input type="text" id="txtUserName" class="forminput form-control" />
                        </span>
                    </li>
                    <li>
                        <span>申请时间：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarStartDate" ClientIDMode="Static"></Hi:CalendarPanel>
                            <span class="Pg_1010">至</span>
                            <Hi:CalendarPanel runat="server" ID="calendarEndDate" Style="width: 160px;" IsEndDate="true" ClientIDMode="Static"></Hi:CalendarPanel>
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary">
                    </li>
                </ul>
            </div>
            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>用户名</th>
                        <th>下单量</th>
                        <th>累计消费</th>
                        <th>申请时间</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--E DataShow-->
            <div class="blank12 clearfix"></div>
        </div>
        <script id="datatmpl" type="text/html">
            {{each rows as item index}}
                <tr>
                    <td><a href="javascript:void(0)" onclick="DialogFrame('member/ReferralInfo.aspx?UserId={{item.UserId}}', '分销员信息', 610, 280, null);">{{item.UserName}}</a></td>
                    <td>{{item.OrderNumber}}   
                    </td>
                    <td>{{item.SaleTotalStr}}</td>
                    <td>{{item.RequetDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}} 
                    </td>
                    <td><a href="javascript:void(0)" onclick="return CheckReferral('{{item.UserId}}','{{item.UserName}}', '{{item.RequetReason}}')">审核</a></td>
                </tr>
            {{/each}}
                
        </script>
        <!--数据列表底部功能区域-->
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


    <div id="CheckReferral" style="display: none;">
        <div class="frame-content" style="margin-top: -20px;">
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat" style="margin-top: 10px">
                <tr>
                    <td align="right" width="30%">用户名:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblUserName" runat="server"></asp:Label></td>
                </tr>

                <tr>
                    <td align="right">提交信息:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblPersonMsg" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">备注:</td>
                    <td align="left" class="bd_td">&nbsp;<asp:TextBox ID="txtRefusalReason" ClientIDMode="Static" runat="server" CssClass="forminput form-control" TextMode="MultiLine" Height="60" Width="243" placeholder="拒绝时填写拒绝理由" /></td>
                </tr>
            </table>

            <div style="text-align: center; padding-top: 10px;">
                <input type="button" id="Button2" onclick="javascript: acceptRequest();" class="btn btn-primary" value="通过" />
                &nbsp;
                <input type="button" id="Button3" onclick="javascript: refuse();" class="btn btn-danger" value="拒绝" />

            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidUserId" runat="server" clientidmode="Static" />
        <input type="hidden" id="hidRefusalReason" runat="server" clientidmode="Static" />

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/MemberManage.ashx?action=getlistReferralRequest" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/ReferralRequestManage.js" type="text/javascript"></script>
</asp:Content>
