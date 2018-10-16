<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MemberDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.MemberDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">
    <style type="text/css">
       
    </style>
    <script type="text/javascript">
        //$(function () {
        //    var menu_left = window.parent.document.getElementById("menu_left");
        //    var aReturnTitle = $(".curent", menu_left);
        //    if (aReturnTitle) {
        //        $("#aReturnTitle").text($(aReturnTitle).text());
        //        var href = "/admin/" + $(aReturnTitle).attr("href");
        //        $("#aReturnTitle").attr("href", href);
        //    }
        //})
    </script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="javascript:ToList()" id="aReturnTitle">返回列表</a></li>
                <li class="hover">
                    <a href="javascript:void">查看“<asp:Literal runat="server" ID="litUserName" />”会员信息</a></li>
            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <h3 class="m_title">基本信息
        </h3>
        <div class="m_info">
            <div class="m_info_1">
                <asp:Image ID="userPicture" runat="server" ImageUrl="" />
                <div class="m_info_user">
                    <span>
                        <asp:Literal runat="server" ID="litShowUserName" /></span><i><asp:Literal runat="server" ID="litGrade" /></i>
                </div>
                <div class="m_info_tag" runat="server" id="divTags">
                </div>
            </div>
            <ul class="m_info_list">
                <li><span>姓名：</span><asp:Literal runat="server" ID="litRealName" /></li>
                <li><span>昵称：</span><asp:Literal runat="server" ID="litNickName" /></li>
                <li><span>性别：</span><asp:Literal runat="server" ID="litGender" /></li>
                <li><span>邮箱：</span><asp:Literal runat="server" ID="litEmail" /></li>
                <li><span>生日：</span><asp:Literal runat="server" ID="litBirthDate" /></li>
                <li><span>QQ：</span><asp:Literal runat="server" ID="litQQ" /></li>
                <li><span>手机：</span><asp:Literal runat="server" ID="litCellPhone" /></li>
                <li><span style="width: 80px">上级分销员：</span><asp:Literal runat="server" ID="litReferral" /></li>
                <li><span>注册于：</span><asp:Literal runat="server" ID="litCreateDate" /></li>
                <li><span>地址：</span><div style="width: 200px; float: right; padding-top: 6px; line-height: 1.8">
                    <asp:Literal runat="server" ID="litAddress" />
                </div>
                </li>
            </ul>
        </div>
        <h3 class="m_title mt_40">统计信息
        </h3>
        <div class="m_s">
            <ul>
                <li>
                    <span>
                        <asp:Literal runat="server" ID="litConsumeAmount" /></span>
                    <font>消费金额</font>
                </li>
                <li>
                    <span>
                        <asp:Literal runat="server" ID="litConsumeTimes" /></span>
                    <font>消费次数</font>
                </li>
                <li>
                    <span>
                        <asp:Literal runat="server" ID="litMoney" /></span>
                    <font>预付款</font>
                </li>
                <li>
                    <span>
                        <asp:Literal runat="server" ID="litPoints" /></span>
                    <font>可用积分</font>
                </li>
                <li>
                    <span>
                        <asp:Literal runat="server" ID="litCouponCount" /></span>
                    <font>优惠券（张）</font>
                </li>
                <li class="bd_0">
                    <span>
                        <asp:Literal runat="server" ID="litDormancyDay" /></span>
                    <font>休眠时间（天）</font>
                </li>
            </ul>
        </div>

        <h3 class="m_title mt_40">购买记录
        </h3>


        <div class="datalist clearfix">
            <table class="m_table mt_20">
                <thead>
                    <tr>
                        <th>订单编号</th>
                        <th width="180">下单时间</th>
                        <th width="180">订单完成时间</th>
                        <th width="160">支付方式</th>
                        <th width="120">订单实付金额</th>
                        <th width="120">消费金额</th>
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

        <input type="hidden" name="hidUserId" id="hidUserId" value="<%=userId %>" />
        <!--S Data Template-->
        <script id="datatmpl" type="text/html">
            {{each rows as item index}}
                <tr>
                    <td>{{item.OrderId}}</td>
                    <td>{{item.OrderDate |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.FinishDate |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.PaymentType}}</td>
                    <td>{{if item.OrderTotal}}
                        {{item.OrderTotal.toFixed(2)}}
                        {{else}}
                        0.00
                        {{/if}}
                    </td>
                    <td>{{item.ConsumptionAmount.toFixed(2)}}</td>
                </tr>
            {{/each}}
        </script>
        <!--E Data Template-->

        <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/MemberDetails.ashx" />
        <script src="/Utility/artTemplate.js" type="text/javascript"></script>
        <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
        <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
        <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
        <script src="/admin/member/scripts/MemberDetails.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="Server">
</asp:Content>
