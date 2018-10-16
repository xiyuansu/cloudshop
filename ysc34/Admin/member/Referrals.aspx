<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Referrals.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Referrals" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>分销员：</span><span>
                            <input type="text" id="txtUserName" class="forminput form-control" />
                        </span>
                    </li>
                    <li>
                        <span>选择时间段：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarStart" ClientIDMode="Static"></Hi:CalendarPanel></span>
                        <span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarEnd" ClientIDMode="Static"></Hi:CalendarPanel></span>
                    </li>
                    <li><span>
                        <input type="checkbox" id="chkIsRepeled" class="icheck kc-danger" />已清退的
                        </span>
                    </li>
                    <li>
                        <span>店铺名称：</span><span>
                            <input type="text" id="txtShopName" class="forminput form-control" />
                        </span>
                    </li>
                    <li>
                        <span>分销员等级：</span><span>
                            <asp:DropDownList ID="dropGradeList" runat="server" ClientIDMode="Static" CssClass="iselect">
                            </asp:DropDownList>
                        </span>
                    </li>
                    <li>
                        <span>
                            <asp:DropDownList ID="dropSortBy" runat="server" ClientIDMode="Static" CssClass="iselect">
                                <asp:ListItem Value="time_d">按成为分销员时间降序排序</asp:ListItem>
                                <asp:ListItem Value="time_a">按成为分销员时间升序排序</asp:ListItem>
                                <asp:ListItem Value="lowerusers_d">按直接下级数降序排序</asp:ListItem>
                                <asp:ListItem Value="tradetotal_d">按直接下级成交额降序排序</asp:ListItem>
                                <asp:ListItem Value="commissiontotal_d">按累计获得佣金数降序排序</asp:ListItem>
                            </asp:DropDownList>
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                    <li id="clickTopDown" onclick="javascript:Post_ExportExcel()" style="cursor: pointer;">
                        <i class="glyphicon glyphicon-save c-666" aria-hidden="true"></i>导出数据</li>
                </ul>
            </div>
            <!--结束-->
            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th style="width: 20%">分销员</th>
                        <th style="width: 16%">店铺名称</th>
                        <th style="width: 10%">分销员等级</th>
                        <th style="width: 10%">直接下级数</th>
                        <th style="width: 12%">直接下级成交额</th>
                        <th style="width: 10%">累计获得佣金</th>
                        <th style="width: 12%">成为分销员时间</th>
                        <th style="width: 10%">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--E DataShow-->
        </div>
        <script id="datatmpl" type="text/html">
            {{each rows as item index}}
                <tr>
                    <td>
                        <a href="javascript:void(0)" onclick="DialogFrame('member/ReferralInfo.aspx?UserId={{item.UserId}}', '分销员信息', 610, 280, null);">{{item.UserName}}</a>
                        {{if item.IsRepeled}}
                        <lable class="cleared">已清退</lable>
                        {{/if}} 
                    </td>
                    <td>{{item.ShopName}}</td>
                    <td>{{item.GradeName}}</td>
                    <td><a href="/admin/member/ReferralsLower.aspx?userId={{item.UserId}}">{{item.SubNumber}}</a>&nbsp;</td>
                    <td>{{item.LowerSaleTotal}}&nbsp;</td>
                    <td><a href="/admin/member/ReferralsSplittin.aspx?userId={{item.UserId}}">{{item.UserAllSplittin}}</a></td>
                    <td>{{item.AuditDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{if item.IsRepeled}}
                        <a href='javascript:void(0);' onclick="javascript:DialogFrame('/admin/member/RepelView?UserId={{item.UserId}}', '查看清退情况', 450, 280, null);">查看</a>&nbsp;
                            <a href='javascript:void(0);' onclick="javascript:DialogFrame('/admin/member/ReferralRestore?UserId={{item.UserId}}&callback=CloseDialogAndReloadData', '恢复分销员身份', 450, 280,null);">恢复</a>
                        {{else}}
                      <a href='javascript:void(0);' onclick="javascript:DialogFrame('/admin/member/ReferralsLink?UserId={{item.UserId}}', '分销码', 610, 280, null);">分销码</a>&nbsp;
                            <a href='javascript:void(0);' onclick="javascript:DialogFrame('/admin/member/ReferralRepel?UserId={{item.UserId}}&callback=CloseDialogAndReloadData', '清退分销员', 450, 280,null);">清退</a>&nbsp;
                        {{/if}} 
                    </td>
                </tr>
            {{/each}}
        </script>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/MemberManage.ashx?action=getlistReferral" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/ReferralManage.js?v=3.4" type="text/javascript"></script>
</asp:Content>
