<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="StoreMoveProductList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.StoreMoveProductList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="hidStoreId" runat="server" Value="0" ClientIDMode="Static" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <asp:HyperLink ID="StoreList" runat="server" ClientIDMode="Static">门店管理</asp:HyperLink></li>
                <li>
                    <asp:HyperLink ID="StoreProducts" runat="server" ClientIDMode="Static">门店商品</asp:HyperLink></li>
                <li class="hover"><a id="all" href="javascript:void">上架商品</a></li>
            </ul>
        </div>

        <div class="datalist">
            <!--搜索-->
            <div class="searcharea">
                <ul>

                    <li><span>商品名称：</span><span> <input type="text" id="txtSearchText" class="forminput form-control"/></span></li>
                    <li>
                        <span>商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="请选择商品分类"
                                CssClass="iselect" ClientIDMode="Static" />
                        </abbr>
                    </li>
                    <li>
                        <span>商品品牌：</span>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList ClientIDMode="Static" runat="server" ID="dropBrandList" NullToDisplay="请选择品牌"
                                CssClass="iselect">
                            </Hi:BrandCategoriesDropDownList>
                        </abbr>
                    </li>

                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>

            </div>
            <div class="functionHandleArea clearfix">
                <div class="batchHandleArea">
                    <div class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                    </div>
                    <a class="btn btn-primary" href="javascript:void(0)" onclick="batchMoveToStore()">批量上架</a>
                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>

            <div class="dataarea mainwidth databody">
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th width="5%"></th>
                            <th width="40%">商品</th>
                            <th width="10%">商品价格</th>
                            <th>商品分类</th>
                            <th width="15%">操作</th>
                        </tr>
                    </thead>
                    <!--S DataShow-->
                    <tbody id="datashow"></tbody>
                </table>

                <div class="blank12 clearfix"></div>
            </div>

            <!--E DataShow-->
        </div>
        <script id="datatmpl" type="text/html">
            {{each rows as item index}}
                <tr>
                    <td>
                        <span class="icheck">
                            <input name="CheckBoxGroup" type="checkbox" value="{{item.ProductId}}" />
                        </span>

                    </td>

                    <td>

                        <div style="float: left; margin-right: 10px;">
                            <a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">
                                <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px;width: 40px;">
                            </a>
                        </div>
                        <div class="p_list_fr">
                            <span class="Name" style="text-align: left;"><a href="../../ProductDetails.aspx?productId={{item.ProductId}}"
                                target="_blank"><i class="black">{{item.ProductType==1?"（服务）":""}}</i>{{item.ProductName}}</a></span>
                        </div>
                    </td>
                    <td>{{item.SalePrice.toFixed(2)}}</td>
                    <td>{{item.categoryName}}
                    </td>
                    <td>
                        <div class="operation">
                            <span>
                                <a href="javascript:void(0)" onclick="openMoveFrame(this.title)" title="{{item.ProductId}}">上架</a>
                            </span>
                        </div>
                    </td>
                </tr>
            {{/each}}
                 
        </script>
    </div>
    <div class="page">
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" id="showpager"></div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/StoreMoveProducts.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/StoreMoveProducts.js" type="text/javascript"></script>
</asp:Content>
