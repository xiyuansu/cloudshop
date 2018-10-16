<%@ Page Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductOnSales.aspx.cs" Inherits="Hidistro.UI.Web.Depot.ProductOnSales" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:void" class="hover">平台商品库</a></li>
            </ul>
            <i class="glyphicon glyphicon-question-sign" data-container="body" style="cursor: pointer" data-toggle="popover" data-placement="left" title="操作说明" data-content="平台中所有的销售中的商品，您可以选择将一些商品上架到您的门店库存中"></i>
        </div>
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li><span>商品名称：</span><span>
                        <input type="text" id="txtSearchText" class="forminput form-control" /></span></li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" ClientIDMode="Static" runat="server" NullToDisplay="--请选择商品分类--"
                                Width="150" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList runat="server" ClientIDMode="Static" ID="dropBrandList" NullToDisplay="--请选择品牌--"
                                Width="153">
                            </Hi:BrandCategoriesDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductTagsDropDownList runat="server" ClientIDMode="Static" ID="dropTagList" NullToDisplay="--请选择标签--"
                                Width="153">
                            </Hi:ProductTagsDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductTypeDownList ID="dropType" ClientIDMode="Static" runat="server" NullToDisplay="--请选择类型--" Width="153" />
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
                </ul>
            </div>
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span>商家编码：</span><span>
                        <input name="txtSKU" type="text" id="txtSKU" class="forminput"></span></li>
                    <li><span>添加时间：</span></li>
                    <li>
                        <input name="startDate" type="text" id="startDate" class="forminput" readonly="readonly" style="float: left;">
                        <span class="Pg_1010">至</span>
                        <input name="endDate" type="text" id="endDate" class="forminput" readonly="readonly" style="float: left;">
                    </li>
                    <li></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>
            <!--结束-->
            <div class="functionHandleArea clearfix">
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton">
                            <%if (IsShelvesProduct)
                                { %>
                            <span class="signicon"></span>
                            <span>
                                <a class="btn btn-primary" href="javascript:void(0)" onclick="SelectAll()"><i class="glyphicon glyphicon-ok" aria-hidden="true"></i>全选</a></span>
                            <span>
                                <a class="btn btn-primary" href="javascript:void(0)" onclick="ReverseSelect()"><i class="glyphicon glyphicon-fullscreen" aria-hidden="true"></i>反选</a></span>
                            <span>
                                <a class="btn btn-primary" id="bacthOnsale" href="javascript:void(0)" onclick="batchMoveToStore()">批量上架</a>
                            </span>
                            <%} %>
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
            <!--数据列表区域-->
            <!--S DataShow-->
            <table class="table table-striped">
                <tr>
                    <th scope="col" style="width: 30px;">选择</th>
                    <th scope="col" style="width: 35px;">排序</th>
                    <th scope="col" style="width: 42%;">商品</th>
                    <th scope="col" style="width: 16%;">商品价格</th>
                    <th scope="col" style="width: 80px;">商品状态</th>
                    <th class="td_left td_right_fff" scope="col">操作</th>
                </tr>
                <tbody id="datashow"></tbody>
            </table>
            <div class="blank12 clearfix"></div>
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
    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}<tr>
            <td>
                <input name="CheckBoxGroup" type="checkbox" value="{{item.ProductId}}" />
            </td>
            <td>{{item.DisplaySequence}}</td>
            <td>
                <div style="float: left; margin-right: 10px;">
                    <a href="../../{{item.ProductType==1?'wapshop/ServiceProductDetails.aspx':'ProductDetails.aspx'}}?productId={{item.ProductId}}" target="_blank">
                        <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px;">
                    </a>
                </div>
                <div style="float: left;">
                    <span class="Name"><a href="../../{{item.ProductType==1?'wapshop/ServiceProductDetails.aspx':'ProductDetails.aspx'}}?productId={{item.ProductId}}" target="_blank">{{item.ProductName}}</a>
                        {{#item.ProductType==1?"<font style='color:red;'>（服务）</font>":""}}
                    </span>
                    <span class="colorC">商家编码：{{item.ProductCode}}，库存：{{item.Stock}}，成本：{{if item.CostPrice}}{{item.CostPrice.toFixed(2)}}{{else}}-{{/if}}</span>
                </div>
            </td>
            <td>
                <span class="Name">一口价：{{if item.SalePrice}}{{item.SalePrice.toFixed(2)}}{{else}}-{{/if}}</span><span class="colorC">市场价：{{item.MarketPrice}}</span>
            </td>
            <td>
                <span>{{item.SaleStatus}}</span>
            </td>
            <td style="white-space: nowrap;">
                 <%if (IsShelvesProduct)
                                { %>
                <span class="submit_bianji"><a href="javascript:void(0)" onclick="openMoveFrame(this.title)" title="{{item.ProductId}}">上架</a></span>
                     <%} %>
            </td>
        </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/product/ashx/ProductOnSales.ashx" />
    <input type="hidden" name="MoveToStoreurl" id="MoveToStoreurl" value="/Depot/product/MoveToStore.aspx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/product/scripts/ProductOnSales.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
