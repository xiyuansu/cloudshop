<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="FriendlyLinks.aspx.cs" Inherits="Hidistro.UI.Web.Admin.FriendlyLinks" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddFriendlyLink.aspx">添加</a></li>
            </ul>

        </div>
        <div class="datalist clearfix">
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th scope="col" width="160">友情链接Logo</th>
                        <th scope="col">网站名称</th>
                        <th scope="col">网站地址</th>
                        <th scope="col" width="160">排列顺序</th>
                        <th scope="col" width="160">是否显示</th>
                        <th class="td_left td_right_fff" scope="col" width="160">操作</th>
                    </tr>
                </thead>
                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>
        </div>
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td><img class="Img100_30" src="{{item.ImageUrl}}" style="border-width:0px;" alt=""/></td>
                    <td>{{item.Title}}</td>
                    <td>{{item.LinkUrl}}</td>
                    <td><input type="text" class="forminput form-control txt-sort" onblur="Post_Sort('{{item.LinkId}}',this.value)" onkeyup="this.value=this.value.replace(/\D/g,'')"
                         onafterpaste="this.value=this.value.replace(/\D/g,'')" value="{{item.DisplaySequence}}" style="width: 60px;"/></td>
                    <td>
                        <a href="javascript:Post_SetShow('{{item.LinkId}}')">{{if item.Visible}}
                                 <img src="../images/allright.gif" />
                            {{else}}
                                 <img src="../images/wrong.gif" />
                            {{/if}}    
                        </a></td>
                    <td>
                        <div class="operation">
                            <span><a href="EditFriendlyLink.aspx?linkId={{item.LinkId}}">编辑</a></span>
                            <span><a href="javascript:Post_Delete('{{item.LinkId}}')">删除</a></span>
                        </div>
                    </td>
                </tr>
        {{/each}}
  
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/store/ashx/FriendlyLinks.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/store/scripts/FriendlyLinks.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
