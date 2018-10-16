<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="PushRecords.aspx.cs" Inherits="Hidistro.UI.Web.Admin.App.PushRecords" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a>管理</a></li>
                <li><a href="ManualPush.aspx">推送</a></li>
            </ul>

        </div>
        <div class="datalist clearfix">

            <div class="searcharea">
                <ul>
                    <li>
                        <span>推送类型：</span><span>
                            <Hi:PushTypeDropDownList ID="ddlPushType" ClientIDMode="Static" runat="server" CssClass="iselect" NullToDisplay="请选择推送类型"></Hi:PushTypeDropDownList>
                        </span>
                    </li>
                    <li>
                        <span>选择时间：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarStart" ClientIDMode="Static"></Hi:CalendarPanel>
                        </span>
                        <span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarEnd" IsEndDate="true" ClientIDMode="Static"></Hi:CalendarPanel>
                        </span>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </li>
                    <li>
                        <span>推送状态：</span><span>
                            <Hi:PushStatusDropDownList ID="ddlPushStatus" ClientIDMode="Static" runat="server" CssClass="iselect" NullToDisplay="请选择推送状态"></Hi:PushStatusDropDownList>
                        </span>
                    </li>
                    <li>

                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>
            </div>

            <div class="functionHandleArea">
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton">
                            <span class="checkall">
                                <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                            <span class="btn btn-default ml0">
                                <a href="javascript:bat_delete()">删除</a>
                            </span>
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

                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th style="width: 5%;"></th>
                                <th style="width: 20%;">推送标题</th>
                                <th style="width: 10%;">类型</th>
                                <th style="width: 15%;">推送对象</th>
                                <th style="width: 20%;">推送时间</th>
                                <th style="width: 15%;">推送状态</th>
                                <th>操作</th>
                            </tr>
                          </thead>
                            <tbody id="datashow"></tbody>
                 
                    </table>
                    <div class="blank12 clearfix"></div>

                </div>
                <!--E DataShow-->
            </div>
            <!--E warp-->
          
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                          <tr>
                              <td><span class="icheck">
                                  <input name="CheckBoxGroup" type="checkbox" value='{{item.PushRecordId}}' />
                              </span></td>
                              <td>{{item.PushTitle}}</td>
                              <td>{{item.PushTypeText}}</td>
                              <td>{{item.PushTagText}}</td>
                              <td>{{item.PushSendDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                              <td>{{item.PushStatusText}}</td>
                              <td>
                                  <span>
                                      <a href="javascript:deleteModel({{item.PushRecordId}})">删除</a>
                                  </span></td>
                          </tr>
                {{/each}}
            </script>
              <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" id="showpager"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/App/ashx/PushRecords.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/App/scripts/PushRecords.js" type="text/javascript"></script>
</asp:Content>

