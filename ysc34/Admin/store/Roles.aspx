<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Roles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">

        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="functionHandleArea">
                <a class="btn btn-primary float_r" href="javascript:ShowEditDiv('','','');">添加部门</a>
                <input  type="hidden" id="txtRoleId" />
                <input runat="server" type="hidden" id="txtRoleName" />
            </div>

            <table class="table table-striped">
                <thead>
                    <tr>
                        <th width="15%">部门名称</th>
                        <th>职能说明</th>
                        <th width="15%">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <tr>

                    <td>{{item.RoleName}}</td>
                    <td>{{item.Description}}</td>

                    <td class="operation">
                        <span>
                            <a href="RolePermissions.aspx?roleId={{item.RoleId}}">部门权限</a>
                        </span>
                        <span><a href="javascript:ShowEditDiv('{{item.RoleId}}','{{item.RoleName}}','{{item.Description}}');">编辑</a></span>
                        <span>
                            <a href="javascript:Delete('{{item.RoleId}}')" class="SmallCommonTextButton">删除</a></span></td>
                </tr>
                {{/each}}
                
            </script>
        </div>
        <!--数据列表底部功能区域-->

    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

    <!--添加部门-->
    <div id="divaddroles" style="display: none;">
        <div class="frame-content">
            <p><span class="frame-span frame-input90"><em>*</em>&nbsp;部门名称：</span>
                <input type="text" id="txtAddRoleName" class="forminput form-control"/>
            </p>
            <%--<b id="ctl00_contentHolder_txtAddRoleNameTip">部门名称不能为空,长度限制在60个字符以内</b>--%>
            <p><span class="frame-span frame-input90">职能说明：</span>
              <input type="text" id="txtRoleDesc" class="forminput form-control"/>
                </p>
              <%--<b id="ctl00_contentHolder_txtRoleDescTip">说明部门具备哪些职能，长度限制在100个字符以内</b>--%>
        </div>
    </div>
    <!--编辑部门-->
    <div id="EditRole" style="display: none;">
        <div class="frame-content">
            <p><span class="frame-span frame-input90"><em>*</em>&nbsp;部门名称：</span>
             <input type="text" id="txtEditRoleName" class="forminput form-control"/>
            </p>
            <p>
                <span class="frame-span frame-input90">职能说明：</span>
                 <input type="text" id="txtEditRoleDesc" class="forminput form-control"/>
            </p>
         </div>
    </div>
    <div style="display: none">
         <input type="button" id="btnEditRoles"  value="编辑部门"/>
        <input type="button" id="btnSubmitRoles"  value="添加部门"/>
    </div>
 
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/store/ashx/Roles.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/store/scripts/Roles.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
