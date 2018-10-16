<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReferralGrades.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.ReferralGrades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <style>
        .dataarea .datalist .table-striped th { padding: 15px 0px !important; }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddReferralGrade.aspx">添加</a></li>
            </ul>
        </div>

        <!-- 添加按钮-->

        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="blockquote-default blockquote-tip" style="margin: 10px 0px 30px 0px;">
                <span style="color: red;">注意：</span>最多可以有10个分销员等级，佣金门槛金额上限为¥99999。
            </div>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th style="width: 30%;">分销员等级</th>
                        <th style="width: 20%;">分销员人数</th>
                        <th style="width: 20%;">佣金门槛</th>
                        <th style="width: 30%;">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                 
                <tr>
                    <td>{{item.Name}}</td>
                    <td><a href="referrals?gradeId={{item.GradeId}}">{{item.ReferralCount}}</a></td>
                    <td>{{item.CommissionThreshold}}</td>
                    <td style="text-align: center;">
                        <a href="EditReferralGrade.aspx?GradeId={{item.GradeId}}">编辑</a>&nbsp;&nbsp;
                        <a href="javascript:Post_Delete({{item.GradeId}})">删除</a>


                    </td>
                </tr>
                {{/each}}
            </script>
            <!--E Data Template-->
        </div>
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/ReferralGrades.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/ReferralGrades.js" type="text/javascript"></script>
</asp:Content>

