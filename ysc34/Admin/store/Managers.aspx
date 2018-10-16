<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Managers.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Managers" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddManager.aspx">添加</a></li>
            </ul>

        </div>

        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li><span>用户名：</span> <span>
                        <input id="txtSearchText" type="text" class="forminput form-control" />
                    </span></li>
                    <li><span>所属部门：</span>
                        <abbr class="formselect">
                            <Hi:RoleDropDownList ID="dropRolesList" runat="server" ClientIDMode="Static" SystemAdmin="true" NullToDisplay="请选择部门" CssClass="iselect" />
                        </abbr>
                    </li>

                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>

                </div>
                <!--S warp-->
                <div class="dataarea mainwidth databody">
                    <!--S DataShow-->
                        <div class="datalist">
                            <table class="table table-striped">
                                <thead>
                                    <tr>
                                        <th style="width: 25%">用户名</th>
                                        <th  style="width: 25%">创建时间</th>
                                        <th  style="width: 25%">所属部门</th>
                                        <th   >操作</th>
                                    </tr>
                                </thead>
                                <tbody id="datashow"></tbody>
                            </table>
                            <div class="blank12 clearfix"></div>
                        </div>
                    </div>
                    <!--E DataShow-->
            
                <!--E warp-->
                <script id="datatmpl" type="text/html">
                    {{each rows as item index}}
                          <tr>
                              <td>{{item.UserName}}</td>
                              <td>{{item.CreateDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                              <td >{{item.RoleName}}</td>
                              <td class="operation">
                                  <span><a href='/Admin/store/EditManager.aspx?UserId={{item.ManagerId}}'>编辑</a></span>

                                  <span>

                                      <a href="javascript:bat_delete({{item.ManagerId}})">删除</a></span>
                              </td>
                          </tr>
                    {{/each}}
                </script>
            
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
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/store/ashx/Managers.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/store/scripts/Managers.js" type="text/javascript"></script>
</asp:Content>
