<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageLogs.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageLogs" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul class="a_none_left">
                    <li><span>操作人：</span>
                        <span>
                            <Hi:LogsUserNameDropDownList ID="dropOperationUserNames" ClientIDMode="Static" runat="server" CssClass="iselect" NullToDisplay="请选择操作人" />
                        </span>
                    </li>
                    <li><span>选择时间段：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calenderFromDate" ClientIDMode="Static"></Hi:CalendarPanel>
                        </span><span class="Pg_1010">至</span><span>
                            <Hi:CalendarPanel runat="server" ID="calenderToDate" IsEndDate="true" ClientIDMode="Static"></Hi:CalendarPanel>
                        </span></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary">
                    </li>
                </ul>
            </div>

            <div class="functionHandleArea clearfix">
                <!--分页功能-->

                <!--结束-->
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton">
                            <span class="checkall">
                                <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>

                            <a href="javascript:bat_deletes()" class="btn btn-default ml0 ">删除</a>

                            <a href="javascript:RemoveAll()" class="btn btn-default ml0 ">清空日志</a>
                        </li>

                    </ul>
                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>

            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <div class="imageDataLeft" id="ImageDataList">
                    <!--S DataShow-->
                    <div class="datalist">
                        <table class="table table-striped">
                            <thead>
                                <tr>
                                    <th width="5%"></th>
                                    <th width="15%">用户名</th>
                                    <th width="15%">IP地址</th>
                                    <th width="20%">操作时间</th>
                                    <th>操作内容</th>
                                    <th width="10%">操作</th>
                                </tr>
                            </thead>
                            <tbody id="datashow"></tbody>
                        </table>

                        <div class="blank12 clearfix"></div>
                    </div>
                </div>
                <!--E DataShow-->
            </div>
            <!--E warp-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                          <tr>
                              <td><span class="icheck">
                                  <input name="CheckBoxGroup" type="checkbox" value='{{item.LogId}}' />
                              </span></td>
                              <td>{{item.UserName}}</td>
                              <td>{{item.IPAddress}}</td>
                              <td>{{item.AddedTime | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                              <td>{{item.Description}}</td>
                              <td class="operation">
                                  <span>
                                      <a href="javascript:bat_delete({{item.LogId}})">删除</a></span>
                              </td>
                          </tr>
                {{/each}}
            

            </script>

        </div>
        <!--数据列表底部功能区域-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/store/ashx/ManageLogs.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/store/scripts/ManageLogs.js" type="text/javascript"></script>
</asp:Content>

