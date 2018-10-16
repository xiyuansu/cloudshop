<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="ActiveMember.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.ActiveMember" EnableViewState="false" %>

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
                <li><a class="hover">活跃会员列表</a></li>
            </ul>
        </div>
        <div class="searcharea clearfix br_search">
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li>
                        <span>活跃时间：</span>

                        <abbr class="formselect">
                            <select class="iselect" id="selTimeScope">
                                <option value="1">1个月</option>
                                <option value="3">3个月</option>
                                <option value="6">6个月</option>
                            </select>
                        </abbr>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>
            <div class="datalist clearfix">
                <!--搜索-->
                <table class="table table-striped" width="100%">
                    <thead>
                        <tr>
                            <th>用户昵称/姓名</th>
                            <th>最后消费日期</th>
                            <th>累积消费次数</th>
                            <th>累积消费金额</th>
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
        </div>
        <!--E Pagination-->
    </div>
    <!--E warp-->
    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each List as item index}}
                          <tr>
                              <td>{{item.NickName}}</td>
                              <td>{{item.LastConsumeDate}}</td>
                              <td>{{item.ConsumeTimes}}</td>
                              <td>{{item.ConsumeTotal}}</td>
                          </tr>
        {{/each}}
           

    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/home/ashx/StoreUsersStatistics.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/home/scripts/ActiveMember.js" type="text/javascript"></script>
</asp:Content>
