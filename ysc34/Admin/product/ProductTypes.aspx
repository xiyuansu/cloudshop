<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ProductTypes" CodeBehind="ProductTypes.aspx.cs" MasterPageFile="~/Admin/Admin.Master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddProductType.aspx">添加</a></li>
            </ul>

        </div>
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist ">
            <div class="searcharea bd_0 mb_0">
                <ul>
                    <li><span>商品类型名称：</span>
                        <span>
                            <input type="text" id="txtSearchText" class="forminput form-control" />
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>
            </div>
            <div class="clearfix"></div>
            <!--S DataShow-->
            <table class="table table-striped">

                <thead>
                    <tr>
                        <th style="width: 40%;">商品类型名称</th>
                        <th style="width: 40%;">备注</th>
                        <th style="width: 20%;">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--E DataShow-->
            <div class="clearfix"></div>
        </div>

        <script id="datatmpl" type="text/html">
            {{each rows as item index}}
                <tr>

                    <td>{{item.TypeName}}&nbsp;</td>
                    <td>{{item.Remark}}</td>

                    <td>
                        <div class="operation">
                            <span>
                                <a href="EditProductType.aspx?TypeId={{item.TypeId}}">编辑</a>
                            </span>
                            <span>
                                <a href="javascript:post_delete({{item.TypeId}})">删除</a>
                            </span>
                        </div>
                    </td>
                </tr>
            {{/each}}
        </script>
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
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/product/ashx/ProductTypes.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/product/scripts/ProductTypes.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

