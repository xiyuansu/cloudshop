<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="StoreProducts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.StoreProducts" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="hidStoreId" runat="server" ClientIDMode="Static" Value="0" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <asp:HyperLink ID="OnStore" runat="server" ClientIDMode="Static">门店管理</asp:HyperLink></li>
                <li class="hover"><a id="all" href="javascript:void">门店商品</a></li>
                <li>
                    <asp:HyperLink ID="OnSale" runat="server" ClientIDMode="Static">上架商品</asp:HyperLink></li>
            </ul>
        </div>

        <div class="datalist">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li><span>门店名称：</span><span style="padding-left:7px;text-overflow: ellipsis;overflow: hidden;white-space: nowrap;width: 150px;" class="j_storeName"><asp:Literal runat="server" ID="storeName"></asp:Literal></span></li>
                    <li><span>商品名称：</span><span><input type="text" id="txtSearchText" class="forminput form-control" /></span></li>
                    <li>
                        <span>商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" ClientIDMode="Static" NullToDisplay="请选择商品分类" CssClass="iselect" />
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

                    <a href="javascript:batDelete()"  class="btn btn-primary ">批量下架</a>
                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>
            <!--数据列表区域-->

            <div class="dataarea mainwidth databody">
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th width="5%"></th>
                                <th width="50%">商品</th>
                                <th width="15%">门店价格</th>
                                <th width="15%">门店库存</th>
                                <th width="15%">操作</th>
                            </tr>
                        </thead>
                        <tbody id="datashow"></tbody>
                    </table>
                    <div class="blank12 clearfix"></div>
                </div>
            </div>
            <!--E DataShow-->
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
                            <a href="/wapshop/{{item.ProductType==1?'ServiceProductDetails.aspx':'StoreProductDetails.aspx'}}?productId={{item.ProductId}}&StoreId={{item.StoreId}}" target="_blank">
                                <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; width: 40px;">
                            </a>
                        </div>
                        <div class="p_list_fr" style="width: 300px;">
                            <span class="Name" style="text-align: left;"><a href="/wapshop/{{item.ProductType==1?'ServiceProductDetails.aspx':'StoreProductDetails.aspx'}}?productId={{item.ProductId}}&StoreId={{item.StoreId}}" target="_blank"><i class="black">{{item.ProductType==1?"（服务）":""}}</i>{{item.ProductName}}</a></span>
                        </div>
                    </td>
                    <td>{{item.StoreSalePrice.toFixed(2)}}</td>
                    <td>{{item.Stock}}</td>
                    <td>
                        <span><a href='javascript:void(0);' onclick="javascript:DialogFrame('depot/StoreProductLink.aspx?productId={{item.ProductId}}'+'&StoreId={{item.StoreId}}', '门店商品', 385, 240, null)">链接</a></span>
                        <span><a href="javascript:void(0);" onclick="javascript:DialogFrame('depot/EditStoreProductInfo.aspx?productId={{item.ProductId}}'+'&StoreId={{item.StoreId}}+&callback=ShowSuccessAndCloseReload','编辑商品',1000, 600, null)">编辑</a></span>
                        <span><span class="submit_shanchu"><a href="javascript:void(0)" class="downshelf" data-id="{{item.ProductId}}" data-storeid="{{item.StoreId}}">下架</a></span></span>
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

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/StoreProducts.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/StoreProducts.js" type="text/javascript"></script>
</asp:Content>
