<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MemberGrades.aspx.cs" Inherits="Hidistro.UI.Web.Admin.MemberGrades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="addmembergrade.aspx">添加</a></li>
            </ul>
        </div>

        <!-- 添加按钮-->

        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th style="width: 15%;">等级名称</th>
                        <th style="width: 10%;">默认会员等级</th>
                        <th style="width: 20%;">积分满足点</th>
                        <th style="width: 10%;">折扣</th>
                        <th style="width: 30%;">备注</th>
                        <th style="width: 15%;">操作</th>
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

                    <td>{{if item.IsDefault}}
                                 <a href="javascript:IsDefault({{item.GradeId}})">
                                     <img alt='' src='../images/allright.gif' /></a>
                        {{else}}
                                 <a href="javascript:IsDefault({{item.GradeId}})">
                                     <img alt='' src='../images/wrong.gif' /></a>
                        {{/if}}
                    </td>
                    <td>{{item.Points}}</td>
                    <td>{{item.Discount}}</td>
                    <td>{{item.Description}}</td>
                    <td>
                        <div class="operation">
                            <span><a href="EditMemberGrade.aspx?GradeId={{item.GradeId}}">编辑</a></span>
                            <span>
                                <a href="javascript:Post_Deletes({{item.GradeId}})">删除</a>
                            </span>
                        </div>
                    </td>
                </tr>
                {{/each}}
            </script>
            <!--E Data Template-->
        </div>
        <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/MemberGrades.ashx" />
        <script src="/Utility/artTemplate.js" type="text/javascript"></script>
        <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
        <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
        <script src="/admin/member/scripts/MemberGrades.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">
    <style>
        .dataarea .datalist .table-striped th { padding: 15px 0px !important; }
    </style>
</asp:Content>
