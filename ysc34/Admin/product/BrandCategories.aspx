<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BrandCategories.aspx.cs" Inherits="Hidistro.UI.Web.Admin.BrandCategories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <%--<li><a href="SetBrandCategoryTemplate.aspx">模板设置</a></li>--%>
                <li><a href="AddBrandCategory.aspx">添加</a></li>
            </ul>
        </div>
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist">
            <div class="searcharea bd_0 mb_0">
                <ul>
                    <li><span>品牌名称：</span>
                        <span>
                            <input type="text" id="txtSearchText" class="forminput form-control" />
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>
            </div>
            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th style="width: 20%;">品牌Logo</th>
                        <th style="width: 14%;">品牌名称</th>
                        <th style="width: 14%;">URL重写</th>
                        <th style="width: 38%;">显示顺序</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--E DataShow-->

        </div>
        <!--S Pagination-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
        <!--E Pagination-->
        
    </div>

    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>
                        <a id="A1" href='{{item.CompanyUrl}}' runat="server" target="_blank">

                            <img src="{{item.Logo}}" class="Img100_30" />
                        </a>
                    </td>

                    <td>{{item.BrandName}}&nbsp;</td>
                    <td>{{item.RewriteName}}</td>
                    <td>
                        <input name="txtDisplaySequence" type="text" onblur="Post_Sort({{item.BrandId}},this)" class="forminput form-control" button="btnSearchButton" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" value="{{item.DisplaySequence}}" style="width: 60px;">
                    </td>
                    <td>
                        <input type="hidden" id="hidBrandId{{index}}" value="{{item.BrandId}}" />
                        <div class="operation">
                            <span>
                                <a href="EditBrandCategory.aspx?brandId={{item.BrandId}}">编辑</a>

                            </span>
                            <span>
                                <a href="javascript:post_delete({{item.BrandId}})">删除</a>
                            </span>
                        </div>
                    </td>
                </tr>
        {{/each}}
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/product/ashx/BrandCategories.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/product/scripts/BrandCategories.js" type="text/javascript"></script>
</asp:Content>
