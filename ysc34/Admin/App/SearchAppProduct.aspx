<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SearchAppProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SearchAppProduct" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="AppProductSetting.aspx">配置</a></li>
                <li class="hover"><a>添加</a></li>
            </ul>
        </div>

        <!--数据列表区域-->
        <div class="datalist datalist-img">
            <div class="searcharea clearfix">
                <ul>
                    <li><span>商品名称：</span><span>

                        <input name="txtSearchText" type="text" id="txtSearchText" class="form-control">
                    </span></li>
                    <li><span>商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ClientIDMode="Static" ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>
                        <span>商品品牌：</span>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList ClientIDMode="Static" runat="server" ID="dropBrandList" NullToDisplay="--请选择商品品牌--" CssClass="iselect" />
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
                    <a class="btn btn-primary" href="javascript:AddProduct()">选择</a>
                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>
            <!--结束-->


            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <div class="imageDataLeft" id="ImageDataList">
                    <!--S DataShow-->
                    <div class="datalist">
                        <table class="table table-striped">
                            <thead>
                                <tr>
                                    <th style="width: 5%;"></th>
                                    <th style="width: 60%;">商品名称</th>
                                    <th style="width: 20%;">价格</th>
                                    <th style="width: 25%;">库存</th>
                                </tr>
                                <tbody id="datashow"></tbody>
                            </thead>
                        </table>

                        <div class="blank12 clearfix"></div>
                    </div>
                </div>
                <!--E DataShow-->
            </div>
            <!--E warp-->
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" id="showpager"></div>
                    </div>
                </div>
            </div>
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                          <tr>
                              <td><span class="icheck">
                                  <input name="CheckBoxGroup" type="checkbox" value='{{item.ProductId}}' />
                              </span></td>
                              <td>
                                  <div style="float: left; margin-right: 10px;">
                                          <a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">
                                              <img src="{{item.ThumbnailUrl40}}" width="40" height="40" />
                                          </a>
                                      </div>
                                      <div style="float: left; width: 200px;">
                                          <span class="Name"><a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">{{item.ProductName}}</a></span>
                                          <span class="colorC">商家编码：{{item.ProductCode}}</span>
                                      </div>
                              </td>
                              <td>{{item.SalePrice}}</td>

                              <td>{{item.Stock}}
                                 
                              </td>
                          </tr>
                {{/each}}
              
            </script>
        </div>

    </div>


    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/App/ashx/SearchAppProduct.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/App/scripts/SearchAppProduct.js" type="text/javascript"></script>
</asp:Content>
