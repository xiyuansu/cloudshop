<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="StoresList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.StoresList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">门店管理</a></li>
                <li><a href="AddStores.aspx">新增</a></li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>门店名称：</span><span>
                            <input type="text" id="txtStoresName" class="forminput form-control" /></span>
                    </li>
                    <li>
                        <span>用户名：</span><span><input type="text" id="txtUserName" class="forminput form-control" /></span>
                    </li>
                </ul>
                <ul id="so_more_none">
                    <li>
                        <span>门店区域：</span><span><Hi:RegionSelector ID="dropRegion" runat="server" ClientIDMode="Static" />
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                    <li>

                        <a href="javascript:Post_ExportExcel()">导出数据</a>
                    </li>
                </ul>
            </div>

            <div class="functionHandleArea clearfix">
                <div class="batchHandleArea">
                    <div class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                    </div>
                    <a class="btn btn-default" href="javascript:void(0);" onclick="batchEdit()">批量设置佣金比例</a>
                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>
            <!--结束-->
            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th width="5%"></th>
                                <th>门店名称</th>
                                <th width="10%">用户名</th>
                                <th width="10%">联系电话</th>
                                <th width="25% ">配送范围</th>
                                <th width="8%">营业状态</th>
                                <th width="8%">门店开关</th>
                                <th width="10%">操作</th>
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
                              <td><span class="icheck">
                                  <input name="CheckBoxGroup" type="checkbox" value='{{item.StoreId}}' />
                              </span></td>
                              <td>{{item.StoreName}}</td>
                              <td>{{item.UserName}}</td>
                              <td>{{item.Tel}}</td>
                              <td>{{item.StoreDeliveryScop}}</td>
                              <td>
                                  <span class="Name">{{if item.CloseStatus==0}}
                                 <img src="../images/wrong.gif" />
                                      {{else}}
                                 <img src="../images/allright.gif" />
                                      {{/if}}
                                  </span>
                              </td>
                              <td>
                                  <span class="Name"><a href="javascript:SetStoreState({{item.StoreId}})">{{if item.State==0}}
                                 <img src="../images/wrong.gif" />
                                      {{else}}
                                 <img src="../images/allright.gif" />
                                      {{/if}}
                                  </a>
                                  </span>
                              </td>
                              <td>
                                  <div class="operation">
                                      <span><a href="EditStores.aspx?StoreId={{item.StoreId}}">编辑</a> </span>
                                      <span><a href="StoreProducts.aspx?StoreId={{item.StoreId}}">商品</a> </span>
                                      <span><a href="StoresPermissions.aspx?StoreId={{item.StoreId}}">权限</a> </span>
                                      <span><a href='javascript:void(0);' onclick="javascript:DialogFrame('depot/StoresLink.aspx?StoreId={{item.StoreId}}', '链接', 610, 280, null);">链接</a></span>
                                  </div>
                              </td>
                          </tr>
                {{/each}}
            </script>
        </div>
        <!--数据列表底部功能区域-->
        <div class="bottomBatchHandleArea clearfix">
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/StoresList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/StoresList.js?v=3.2" type="text/javascript"></script>
</asp:Content>
