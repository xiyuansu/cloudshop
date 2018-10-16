<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageShippingTemplates.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.ManageShippingTemplates" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddShippingTemplate.aspx">添加</a></li>
            </ul>

        </div>

        <div class="datalist clearfix">

            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th>方式名称</th>
                                <th>数量</th>
                                <th>起步价</th>
                                <th>增加数量</th>
                                <th>加价</th>
                                <th>是否包邮</th>
                                <th>计价方式</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody id="datashow"></tbody>
                    </table>

                    <div class="blank12 clearfix"></div>
                </div>
                <!--E DataShow-->
            </div>
            <!--E warp-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <tr>
                    <td>{{item.TemplateName}} 
                    </td>
                    <td>{{=item.ShowNumberAndUnit}}</td>
                    <td>{{item.Price.toFixed(2)}}</td>
                    <td>{{#item.AddNumberStr}}</td>
                    <td>{{item.AddPrice.toFixed(2)}}</td>
                    <td>{{#item.IsFreeShippingImg}}</td>
                    <td>{{item.ValuationMethodStr}}</td>
                    <td style="text-align: center;">
                        <span><a href="EditShippingTemplate.aspx?TemplateId={{item.TemplateId}}" class="SmallCommonTextButton a_link">编辑</a></span>
                        <span><a class="SmallCommonTextButton" href="javascript:Delete({{item.TemplateId}})">删除</a></span>


                    </td>
                </tr>
                {{/each}}
           
            </script>

            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" id="showpager"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/sales/ashx/ManageShippingTemplates.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/sales/scripts/ManageShippingTemplates.js" type="text/javascript"></script>
</asp:Content>

