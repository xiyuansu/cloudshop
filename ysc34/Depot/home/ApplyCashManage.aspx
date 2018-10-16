<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="ApplyCashManage.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.ApplyCashManage" EnableViewState="false" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a class="hover">提现明细管理</a></li>
                <li><a href="AddApplyCash.aspx" >申请提现</a></li>
            </ul>
        </div>
        <div class="datalist clearfix">
            <!--搜索-->
            <table class="table table-striped" width="100%">
                <thead>
                    <tr>
                        <th>提现时间</th>
                        <th>提现金额</th>
                        <th>备注</th>
                        <th>提现状态</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <div class="blank12 clearfix"></div>
        </div>
        <!--E DataShow-->
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
    <!--E warp-->
    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each List as item index}}
                          <tr>
                              <td>{{item.RequestTime}}</td>
                              <td>{{item.Amount}}</td>
                              <td>{{item.Remark}}</td>
                              <td>{{item.StatusText}}</td>
                          </tr>
        {{/each}}
           

    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/home/ashx/ApplyCashManage.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/home/scripts/ApplyCashManage.js" type="text/javascript"></script>
</asp:Content>
