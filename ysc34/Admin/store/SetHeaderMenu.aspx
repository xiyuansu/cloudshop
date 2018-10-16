<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetHeaderMenu.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetHeaderMenu" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="ManageThemes.aspx">“<asp:Literal runat="server" ID="litThemName"></asp:Literal>”模板的页头菜单</a></li>
                <li>
                    <asp:HyperLink ID="hlinkAddHeaderMenu" Text="添加页头菜单" runat="server" /></li>
            </ul>

        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <div class="float">
                </div>
                <div class="float_r">
                    <span class="float" style="line-height: 32px;">商品分类显示个数：</span>
                    <input name="txtCategoryNum" type="text" id="txtCategoryNum" class="forminput form-control" style="width: 60px;" />
                    <input type="button" name="btnSaveNum" value="保存" id="btnSaveNum" class="btn btn-primary ml_10" />
                </div>


            </div>
            <div class="clearfix">
                <table cellspacing="0" border="0" class="table table-striped table-fixed">
                    <thead>
                        <tr>
                            <th scope="col" width="20%">显示顺序</th>
                            <th scope="col">菜单名称</th>
                            <th scope="col" width="20%">菜单类别</th>
                            <th scope="col" width="15%">是否显示</th>
                            <th class="td_left td_right_fff" scope="col" width="15%">操作</th>
                        </tr>
                    </thead>
                    <!--S DataShow-->
                    <tbody id="datashow"></tbody>
                    <!--E DataShow-->
                </table>
            </div>
        </div>
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>
                        <input type="text" class="forminput form-control txt-sort" onblur="Post_Sort('{{item.Id}}',this.value)" onkeyup="this.value=this.value.replace(/\D/g,'')"
                            onafterpaste="this.value=this.value.replace(/\D/g,'')" value="{{item.DisplaySequence}}" style="width: 60px;" /></td>
                    <td>{{item.Title}}</td>
                    <td>{{item.CategoryName}}</td>
                    <td>
                        <a href="javascript:Post_SetShow('{{item.Id}}')">{{if item.Visible=="true"}}
                                 <img src="../images/allright.gif" />
                            {{else}}
                                 <img src="../images/wrong.gif" />
                            {{/if}}
                        </a></td>
                    <td>
                        <div class="operation">
                            <span><a href="/Admin/store/EditHeaderMenu.aspx?Id={{item.Id}}&ThemName=<%=themName %>">编辑</a></span>
                            <span><a href="javascript:Post_Delete('{{item.Id}}')">删除</a></span>
                        </div>
                    </td>
                </tr>
        {{/each}}
  
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/store/ashx/SetHeaderMenu.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/store/scripts/SetHeaderMenu.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
