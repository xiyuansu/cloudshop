<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageMenu.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AliOH.ManageMenu" MasterPageFile="~/Admin/Admin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
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
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th scope="col">菜单名称</th>
                        <th scope="col" style="width: 40%;">绑定关键字</th>
                        <th scope="col" style="width: 15%;">显示顺序</th>
                        <th scope="col" style="width: 25%;">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                               
                <tr>
                    <td>{{=item.Name}}</td>
                    <td>{{if item.Url!=""}}
                            <span class="Name">{{item.BindTypeName}}{{item.ulrs}}</span>
                        {{/if}} </td>
                    <td>
                        <input type="text" class="form-control txtdisplay" value='{{item.DisplaySequence}}' data-id="{{item.MenuId}}" data-oldvalue="{{item.DisplaySequence}}" style="width: 60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" /></td>
                    <td>
                        <div class="operation">
                            <span>
                                <a href="EditMenu.aspx?MenuID={{item.MenuId}}">编辑</a>
                            </span>
                            <span>
                                <a href="javascript:Post_Deletes({{item.MenuId}})">删除</a>
                            </span>
                            <span>{{if item.BindType=="0"}}
                                 <a href="AddMenu.aspx?pid={{item.MenuId}}">添加子菜单</a>
                                {{/if}}
                            </span>
                        </div>

                    </td>
                </tr>
                {{/each}}
            </script>
            <!--E Data Template-->

            <div style="margin-top: 10px;">
                <asp:Button ID="btnSubmit" runat="server" Text="保存到生活号" CssClass="btn btn-primary" />
            </div>

        </div>


        <input type="hidden" name="dataurl" id="dataurl" value="/admin/AliOH/ashx/ManageMenu.ashx" />
        <script src="/Utility/artTemplate.js" type="text/javascript"></script>
        <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
        <script src="/admin/AliOH/scripts/ManageMenu.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

