<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MemberDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.MemberDetails" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Orders" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        table tr td, th {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--查询列表-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>登录帐号：</span>
                        <span>
                            <input type="text" id="txtUserName" class="forminput form-control"/>
                        </span>
                    </li>
                    <li>
                        <span>店员：</span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="ManagerList" class="iselect" runat="server">
                            </asp:DropDownList>
                        </abbr>
                    </li>
                    <li>
                        <span>注册时间：</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="startDate"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="endDate"></Hi:CalendarPanel>
                    </li>
                </ul>
                <ul style="float:right;margin-right:50px;">
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                    <li>
                        <a href="javascript:ExportToExcel()">导出数据</a>
                    </li>
                </ul>
            </div>
            <!--结束-->
            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th width="20%">登录账号</th>
                                <th width="15%">店员</th>
                                <th width="25%">注册时间</th>
                                <th width="20%">累计消费金额</th>
                            </tr>
                        </thead>
                        <tbody id="datashow"></tbody>
                    </table>
                    <div class="blank12 clearfix"></div>
                </div>
                <!--E DataShow-->
            </div>
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                          <tr>
                              <td>{{item.UserName}}</td>
                              <td>{{item.ManagersName}}</td>
                              <td>{{item.CreateDate}}</td>
                              <td>
                                  <a href="/admin/member/MemberDetails?userId={{item.UserId}}">{{item.Expenditure}}</a>
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

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/MemberDetails.ashx" />
    <input type="hidden" name="butstoreId" id="butstoreId" runat="server" value="0" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/MemberDetails.js" type="text/javascript"></script>
</asp:Content>
