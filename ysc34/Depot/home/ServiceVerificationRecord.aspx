<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="ServiceVerificationRecord.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.ServiceVerificationRecord" EnableViewState="false" %>

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
                <li><a class="hover">服务类核销记录</a></li>
            </ul>
        </div>
        <div class="searcharea clearfix br_search">
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li>
                        <span>订单编号/核销码：</span>

                        <span>
                            <input name="txtkeyword" type="text" id="txtkeyword" class="forminput" /></span>
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
                            <th>订单ID</th>
                            <th>商品</th>
                            <th>价格</th>
                            <th>数量</th>
                            <th>核销人</th>
                            <th>核销日期</th>
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
                              <td>{{item.OrderId}}</td>
                              <td>
                                  <img src="{{item.ThumbnailsUrl}}" style="border-width: 0px; width: 40px; height: 40px; margin-right:10px;" alt="" />{{item.ProductName}}</td>
                              <td>{{item.Price}}</td>
                              <td>{{item.num}}</td>
                              <td>{{item.UserName}}</td>
                              <td>{{item.VerificationDate}}</td>
                          </tr>
        {{/each}}
           

    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/home/ashx/ServiceVerification.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/home/scripts/ServiceVerificationRecord.js" type="text/javascript"></script>
</asp:Content>
