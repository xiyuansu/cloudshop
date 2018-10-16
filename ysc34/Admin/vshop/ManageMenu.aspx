<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ManageMenu" CodeBehind="~/Admin/vshop/ManageMenu.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="hidClientType" ClientIDMode="Static" runat="server" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddMenu.aspx">添加</a></li>

            </ul>
        </div>
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <!--S DataShow-->
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th scope="col" style="width: 20%;">菜单名称</th>
                        <th scope="col" style="width: 40%;">绑定关键字</th>
                        <th scope="col">显示顺序</th>
                        <th class="td_left td_right_fff" scope="col" width="260">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--E DataShow-->
        </div>
        <div class="batchHandleArea">
            <ul>
                <li class="batchHandleButton">
                    <span>
                        <asp:Button ID="btnSubmit" runat="server" Text="保存到微信" CssClass="btn btn-primary" />
                    </span>
                </li>
            </ul>
        </div>
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{if item.ParentMenuId>0}}
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{{item.Name}}
                        {{else}}
                        <b>{{item.Name}}</b>
                        {{/if}}
                    </td>
                    <td>{{if item.Url && item.Url!=""}}
                        <span class="Name">{{item.BindTypeName}}&nbsp;{{if item.IsHttpStart}}{{item.Url}}{{/if}}
                        </span>
                        {{else}}
                        &nbsp;
                        {{/if}}
                    </td>
                    <td>
                        <input type="text" class="form-control txt-sort" onblur="Post_Sort('{{item.MenuId}}',this.value)" style="width: 60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" value="{{item.DisplaySequence}}" />
                    </td>
                    <td>
                        <div class="operation">
                            <span>
                                <a href="EditMenu.aspx?MenuID={{item.MenuId}}">编辑</a>
                            </span>
                            <span>
                                <a href="javascript:Post_Delete('{{item.MenuId}}')">删除</a></span>
                            {{if item.BindType==0}}
                            <span>
                                <a href="AddMenu.aspx?pid={{item.MenuId}}">添加子菜单</a>
                            </span>
                            {{/if}}
                        </div>
                    </td>

                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/vshop/ashx/ManageMenu.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/vshop/scripts/ManageMenu.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

