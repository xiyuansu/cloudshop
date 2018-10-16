<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ExpressTemplates.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ExpressTemplates" Title="无标题页" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddExpressTemplate.aspx">添加</a></li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th scope="col" width="200">单据编号</th>
                        <th scope="col">单据名称</th>
                        <th scope="col" width="16%">是否启用</th>
                        <th class="td_left td_right_fff" scope="col" width="40%">操作</th>
                    </tr>
                </thead>
                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>
            <div class="blank5 clearfix"></div>
        </div>
    </div>
    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.ExpressId}}</td>
                    <td>{{item.ExpressName}}</td>
                    <td>
                        <a href="javascript:Post_SetYesOrNo('{{item.ExpressId}}')">{{if item.IsUse}}
                                 <img src="../images/allright.gif" />
                            {{else}}
                                 <img src="../images/wrong.gif" />
                            {{/if}}    
                        </a>
                    </td>
                    <td>
                        <div class="operation">
                            <span><a href="EditExpressTemplate.aspx?ExpressId={{item.ExpressId}}&ExpressName={{item.ExpressName}}&XmlFile={{item.XmlFile}}">编辑</a></span>
                            <span><a href="AddSampleExpressTemplate.aspx?ExpressName={{item.ExpressName}}&XmlFile={{item.XmlFile}}">添加相似单据</a></span>
                            <span class="submit_shanchu"><a href="javascript:Post_Delete('{{item.ExpressId}}','{{item.XmlFile}}')">删除</a></span>
                        </div>

                    </td>
                </tr>
        {{/each}}
  
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/sales/ashx/ExpressTemplates.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/sales/scripts/ExpressTemplates.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
