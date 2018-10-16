<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OrderPromotions.aspx.cs" Inherits="Hidistro.UI.Web.Admin.OrderPromotions" Title="无标题页" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="hidIsWholesale" ClientIDMode="Static" runat="server" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="AddOrderPromotion.aspx" ID="hlinkAddPromotion" Text="添加" /></li>
            </ul>
        </div>


        <div class="datalist clearfix">
            <div>
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th>活动名称</th>
                            <th>活动类型</th>
                            <th>开始时间</th>
                            <th>结束时间</th>
                            <th>操作</th>
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
                    <td>{{item.Name}}</td>
                    <td>{{item.PromoteTypeText}} {{item.PromoteInfo}}</td>
                    <td>{{item.StartDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.EndDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>
                        <span>
                            {{if item.PromoteType==13||item.PromoteType==14||item.PromoteType==4}}
                            <a href='/admin//promotion/EditOrderPromotion.aspx?ActivityId={{item.ActivityId}}&isWholesale=true'>编辑</a>
                            {{else}}
                            <a href='/admin//promotion/EditOrderPromotion.aspx?ActivityId={{item.ActivityId}}'>编辑</a>
                            {{/if}}

                        </span>
                        <span><a href="javascript:Post_Delete('{{item.ActivityId}}')">删除</a></span>
                    </td>

                </tr>
        {{/each}}
        
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/OrderPromotions.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/OrderPromotions.js" type="text/javascript"></script>
</asp:Content>
