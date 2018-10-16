<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CustomPrintItems.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.CustomPrintItems" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul>
                    <li class="pull-right mr0">
                        <a class="btn btn-primary" href="javascript:DialogFrame('sales/AddPrintDataItem.aspx','添加自定义打印项',600,300)">添加</a>
                    </li>
                </ul>
            </div>

            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th scope="col" width="200">名称</th>
                        <th scope="col">内容</th>
                        <th class="td_left td_right_fff" scope="col" width="10%">操作</th>
                    </tr>
                </thead>
                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>
            <div class="blank5 clearfix"></div>
        </div>
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.Name}}</td>
                    <td>{{item.Content}}</td>
                    <td>
                        <span class="submit_shanchu"><a href="javascript:Post_Delete('{{item.Name}}')">删除</a></span></td>
                </tr>
        {{/each}}
  
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/sales/ashx/CustomPrintItems.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/sales/scripts/CustomPrintItems.js" type="text/javascript"></script>
</asp:Content>
