<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="MarketingImageManage.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.MarketingImageManage" EnableViewState="false" %>

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
                <li><a class="hover">营销图片管理</a></li>
            </ul>
        </div>
        <div class="datalist clearfix">
            <!--搜索-->
            <table class="table table-striped" width="100%">
                <thead>
                    <tr>
                        <th width="10%">图片</th>
                        <th width="15%" nowrap="nowrap">图片名称</th>
                        <th>使用说明</th>
                        <th class="td_left td_right_fff" scope="col">操作</th>
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
                              <td>
                                  <img src="{{item.ImageUrl}}" width="40" height="40" />
                              </td>
                              <td>{{item.ImageName}}</td>
                              <td>{{item.Description}}</td>
                              <td nowrap="nowrap">
                                  <span><a href="/Depot/home/MdyMarketingImageProduct.aspx?ImageId={{item.ImageId}}" >配置商品</a></span>

                              </td>
                          </tr>
        {{/each}}
           

    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/home/ashx/MarketingImageManage.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/home/scripts/MarketingImageManage.js" type="text/javascript"></script>
</asp:Content>
