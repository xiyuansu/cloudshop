<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="MyStoreProducts.aspx.cs" Inherits="Hidistro.UI.Web.Depot.product.MyStoreProducts" EnableViewState="false" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="###" class="tab_status" data-filter="">我的商品</a></li>
                <li><a href="###" class="tab_status" data-filter="1">警戒库存</a></li>
            </ul>
            <i class="glyphicon glyphicon-question-sign" data-container="body" style="cursor: pointer" data-toggle="popover" data-placement="left" title="操作说明" data-content="我的门店中的所有商品，您可以对商品进行库存管理"></i>
        </div>
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;" id="divSearchBox" runat="server">
                <asp:HiddenField ID="hidIsWarning" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hidStoreId" ClientIDMode="Static" runat="server" />
                <ul>
                    <li><span>商品名称：</span><span><input name="txtSearchText" type="text" id="txtSearchText" class="forminput" /></span></li>
                    <li><span>商品编码：</span><span><input name="txtProductCode" type="text" id="txtProductCode" class="forminput" /></span></li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" Width="150" ClientIDMode="Static" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                        <asp:DropDownList ID="ddlProductType" ClientIDMode="Static" runat="server" CssClass="iselect">
                            <asp:ListItem Value="-1" Text="请选择商品种类"></asp:ListItem>
                            <asp:ListItem Value="0" Text="实物商品"></asp:ListItem>
                            <asp:ListItem Value="1" Text="服务商品"></asp:ListItem>
                        </asp:DropDownList>
                        </abbr>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary"></li>
                </ul>
            </div>
            <div class="functionHandleArea clearfix">
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span>
                            <span>
                                <a class="btn btn-primary" href="javascript:void(0)" onclick="SelectAll()">
                                    <i class="glyphicon glyphicon-ok" aria-hidden="true"></i>全选</a></span>
                            <span>
                                <a class="btn btn-primary" href="javascript:void(0)" onclick="ReverseSelect()">
                                    <i class="glyphicon glyphicon-fullscreen" aria-hidden="true"></i>反选</a></span>
                            <span runat="server" id="spbacthStock"><a class="btn btn-primary" href="javascript:void(0)" onclick="batchAdjustStore()">批量调整库存</a></span>
                            <span runat="server" id="spbacthWarningStock"><a class="btn btn-primary" href="javascript:void(0)" onclick="batchAdjustWarningStore()">批量调整警戒库存</a></span>
                            <span runat="server" id="spbacthSalePrice"><a class="btn btn-primary" href="javascript:void(0)" onclick="batchAdjustSalePrice()">批量调整商品售价</a></span>
                            <span class="btn btn-danger"><i class="glyphicon glyphicon-remove" aria-hidden="true"></i>
                                <a id="btnBatDelete" name="btnBatDelete">批量下架</a></span>
                        </li>
                    </ul>
                    <div class="pageHandleArea">
                        <ul>
                            <li class="paginalNum"><span>每页显示数量：</span><Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <table class="table table-striped" width="100%">
                <thead>
                    <tr>
                        <th scope="col" width="5%">选择</th>
                        <th scope="col" width="10%">排序</th>
                        <th scope="col" width="40%">商品</th>
                        <th scope="col" width="15%">商品价格</th>
                        <th scope="col" width="10%">商品状态</th>
                        <th class="td_left td_right_fff" scope="col">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <div class="blank12 clearfix"></div>
        </div>
        <!--E DataShow-->
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
    <!--E warp-->
    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>
                        <input name="CheckBoxGroup" type="checkbox" value="{{item.ProductId}}">
                    </td>
                    <td>{{item.DisplaySequence}}</td>
                    <td>
                        <div style="float: left; margin-right: 10px;">
                            <a href="../../wapshop/{{item.ProductType==1?'ServiceProductDetails.aspx':'StoreProductDetails.aspx'}}?productId={{item.ProductId}}&storeId={{item.StoreId}}" target="_blank">
                                <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px;">
                            </a>
                        </div>
                        <div style="float: left;">
                            <span class="Name"><a href="../../wapshop/{{item.ProductType==1?'ServiceProductDetails.aspx':'StoreProductDetails.aspx'}}?productId={{item.ProductId}}&storeId={{item.StoreId}}" target="_blank">{{item.ProductName}}</a>
                                {{#item.ProductType==1?"<font style='color:red;'>（服务）</font>":""}}
                            </span> 
                            <span class="colorC">商家编码：{{item.ProductCode}}&nbsp;平台库存：{{item.MainStock}}&nbsp;门店库存：{{item.Stock}}&nbsp;成本：{{if item.CostPrice}}{{item.CostPrice.toFixed(2)}}{{else}}-{{/if}}</span>
                        </div>
                    </td>
                    <td>
                        <span class="Name">平台价：{{if item.SalePrice}}{{item.SalePrice.toFixed(2)}}{{else}}-{{/if}}</span><span class="colorC">门店价：{{if item.StoreSalePrice}}{{item.StoreSalePrice.toFixed(2)}}{{else}}-{{/if}}</span>
                    </td>
                    <td>
                        <span>{{item.SaleStatusText}}</span>
                    </td>
                    <td style="white-space: nowrap;">
                        <span class="submit_bianji btb_stock hidden"><a href="javascript:void(0)" onclick="openAdjustStockPage(this.title)" title="{{item.ProductId}}">调整库存</a></span>
                        <span class="submit_bianji btb_warningstock hidden"><a href="javascript:void(0)" onclick="openAdjustWarningStockPage(this.title)" title="{{item.ProductId}}">调整警戒库存</a></span>
                        <span class="submit_bianji btb_saleprice hidden"><a href="javascript:void(0)" onclick="openAdjustSalePrice(this.title)" title="{{item.ProductId}}">调整售价</a></span>
                        <span class="submit_bianji btb_stocklog"><a href="javascript:void(0)" onclick="openAdjustStockLog(this.title)" title="{{item.ProductId}}">库存明细</a></span>
                        <span class="submit_shanchu"><a href="javascript:void(0)" class="downshelf" data-id="{{item.ProductId}}">下架</a></span>
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/product/ashx/MyStoreProducts.ashx" />
    <input type="hidden" name="adjustStockurl" id="adjustStockurl" value="/Depot/product/AdjustStock.aspx" />
    <input type="hidden" name="adjustWarningStockurl" id="adjustWarningStockurl" value="/Depot/product/AdjustWarningStock.aspx" />
    <input type="hidden" name="storeStockAdjustLogurl" id="storeStockAdjustLogurl" value="/Depot/product/StoreStockAdjustLog.aspx" />
    <input type="hidden" name="adjustSalePriceurl" id="adjustSalePriceurl" value="/Depot/product/AdjustStoreSalePrice.aspx" />
    <input type="hidden" runat="server" id="hidIsModifyPrice" value="1" clientidmode="Static" />
    <input type="hidden" runat="server" id="hidIsShelvesProduct" value="1" clientidmode="Static" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/product/scripts/MyStoreProducts.js" type="text/javascript"></script>
</asp:Content>
