<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RestoreDatabase.aspx.cs" Inherits="Hidistro.UI.Web.Admin.store.RestoreDatabase" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">数据恢复</a></li>
                <li><a href="backup.aspx">备份</a></li>
            </ul>
        </div>
        <div class="datalist clearfix">
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>备份文件名</th>
                        <th>版本号</th>
                        <th>大小(字节)</th>
                        <th>备份时间</th>
                        <th>操作</th>
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
            <td>{{item.BackupName}}</td>
            <td>{{item.Version}}</td>
            <td>{{item.FileSize}}</td>
            <td>{{item.BackupTime | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
            <td>
                <span class="submit_shanchu"><a href="javascript:Post_Restore('{{item.BackupName}}')">恢复</a></span>
                <span class="submit_shanchu"><a href="javascript:Post_Delete('{{item.BackupName}}')">删除</a></span>
            </td>
        </tr>
        {{/each}}
        
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/tools/ashx/RestoreDatabase.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/tools/scripts/RestoreDatabase.js" type="text/javascript"></script>
</asp:Content>
