<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReferralsSplittin.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ReferralsSplittin" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="Referrals.aspx">分销员列表</a></li>
                <li class="hover"><a href="javascript:void">佣金明细</a></li>
            </ul>
        </div>

        <div class="VIPbg fonts">
            <ul>
                <li class="colorQ">分销员：<strong style="color: black"><asp:Literal runat="server" ID="litUserName" /></li>
                <li class="colorQ">当前可提现佣金：<strong style="color: black"><asp:Literal runat="server" ID="litUseSplittin" /></li>
                <li class="colorQ">未结算佣金：<strong style="color: black"><asp:Literal runat="server" ID="litNoUseSplittin" /></li>
            </ul>
            <ul>
                <li class="colorQ">直接下级数：<strong style="color: black"><asp:Literal runat="server" ID="litLowerNum" /></li>
                <li class="colorQ">直接下级成交额：<strong style="color: black"><asp:Literal runat="server" ID="litLowerMoney" /></li>
                <li class="colorQ">累计获得佣金：<strong style="color: black"><asp:Literal runat="server" ID="litAllSplittin" /></li>
            </ul>
            <ul>
                <li class="colorQ">上级分销员：<strong style="color: black"><asp:Literal runat="server" ID="litSuperior" /></li>
                <li class="colorQ">上二级分销员：<strong style="color: black"><asp:Literal runat="server" ID="litSuperior2" /></li>
            </ul>
        </div>

        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>订单编号：</span><span>
                            <input type="text" id="txtOrderId" class="forminput form-control" /></span>
                    </li>
                    <li>
                        <span>订单支付时间：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarPayStart"></Hi:CalendarPanel></span>
                        <span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarPayEnd"></Hi:CalendarPanel></span>
                    </li>
                </ul>
                <ul>
                    <li>
                        <span>会员：</span><span><input type="text" id="txtUserName" class="forminput form-control" /></span>
                    </li>
                    <li>
                        <span>佣金类型：</span>
                        <span>
                            <asp:DropDownList ClientIDMode="Static" runat="server" ID="ddlSplittinTypes" CssClass="iselect">
                            </asp:DropDownList>
                        </span>
                    </li>
                </ul>
                <ul>
                    <li>
                        <span>佣金结算时间：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarSetStart"></Hi:CalendarPanel></span>
                        <span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarSetEnd"></Hi:CalendarPanel></span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                     <li id="clickTopDown" onclick="javascript:Post_ExportExcel()" style="cursor: pointer;">
                        <i class="glyphicon glyphicon-save c-666" aria-hidden="true"></i>导出数据</li>
                </ul>
            </div>
            <!--S DataShow-->
            <table class="table table-striped">
                <tr>
                    <th>订单编号</th>
                    <th>会员</th>
                    <th>支付时间</th>
                    <th>佣金结算时间</th>
                    <th>订单金额</th>
                    <th>佣金</th>
                    <th>佣金类型</th>
                </tr>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--E DataShow-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <tr>
                    <td>{{item.OrderId}}
                    </td>
                    <td>{{item.UserNameStr}}&nbsp;</td>
                    <td>{{item.TradeDateStr | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}  </td>
                    <td>{{item.FinishDateStr | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}  
                    </td>
                    <td>{{item.OrderTotal}}&nbsp;</td>
                    <td>{{item.Money}}
                    </td>
                    <td>{{item.TradeTypeStr}}
                    </td>
                </tr>
                {{/each}}
            </script>
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/ReferralsSplittin.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/ReferralsSplittin.js" type="text/javascript"></script>
</asp:Content>
