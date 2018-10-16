<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.HelpList" MasterPageFile="~/Admin/Admin.Master" CodeBehind="HelpList.aspx.cs" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="HelpList.aspx">管理</a></li>
                <li><a href="AddHelp.aspx">添加</a></li>
            </ul>
        </div>
    </div>
    <div class="dataarea mainwidth">
        <!--搜索-->

        <!--结束-->


        <div class="searcharea">
            <ul class="a_none_left">

                <li><span>关键字：</span>
                    <span>
                        <input type="text" id="txtkeyWords" class="forminput form-control" />
                    </span></li>
                <li>
                    <abbr class="formselect">
                        <Hi:HelpCategoryDropDownList ID="dropHelpCategory" ClientIDMode="Static" NullToDisplay="请选择分类" runat="server" CssClass="iselect" />
                    </abbr>
                </li>
                <li><span>选择时间：</span>
                    <span>
                        <Hi:CalendarPanel runat="server" ID="calendarStartDataTime" ClientIDMode="Static"></Hi:CalendarPanel>
                    </span>
                    <span class="Pg_1010">至</span>
                    <span>
                        <Hi:CalendarPanel runat="server" ID="calendarEndDataTime" IsEndDate="true" ClientIDMode="Static"></Hi:CalendarPanel>
                    </span></li>
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

                        <a href="javascript:bat_deletes()" class="btn btn-default ml0">删除</a>
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
                        <th width="50px;"></th>
                        <th width="25%">分类名称</th>
                        <th width="25%">主题</th>
                        <th width="25%">开始时间</th>

                        <th>操作</th>
                    </thead>
                    <tbody id="datashow"></tbody>

                </table>
                <div class="blank12 clearfix"></div>
            </div>

        </div>
        <!--E DataShow-->

        <!--E warp-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
        <script id="datatmpl" type="text/html">
            {{each rows as item index}}
                          <tr>
                              <td><span class="icheck">
                                  <input name="CheckBoxGroup" type="checkbox" value='{{item.HelpId}}' />
                              </span></td>
                              <td>{{item.Name}} </td>
                              <td><a href="../../HelpDetails.aspx?HelpId={{item.HelpId}}" target="_blank">{{item.Title}}   
                              </a></td>
                              <td>{{item.AddedDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>

                              <td class="operation">
                                         <div class="operation">
                                      <span><a href="Edithelp.aspx?HelpId={{item.HelpId}}">编辑</a></span>

                                      <span><a href="javascript:bat_delete({{item.HelpId}})">删除</a></span>
                                  </div>
                                 </td>
                          </tr>
            {{/each}}
           

        </script>


    </div>
    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/comment/ashx/HelpList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/comment/scripts/HelpList.js" type="text/javascript"></script>
</asp:Content>

