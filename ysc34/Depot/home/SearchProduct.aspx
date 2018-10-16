<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SearchProduct.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.SearchProduct" Title="选择商品" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <style>
        html {
            background: #fff !important;
        }

        body {
            padding: 0;
            width: 100%;
        }

        #mainhtml {
            margin: 0;
            padding: 20px 20px 80px 20px;
            width: 100% !important;
        }
    </style>
    <div class="dataarea mainwidth databody">
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea clearfix">
                <ul>
                    <li><span>商品名称：</span><span>
                        <input type="text" id="txtSearchText" class="forminput form-control" value="<%=productName %>" /></span></li>
                    <li class="formselect">
                        <Hi:ProductCategoriesDropDownList ID="dropCategories" ClientIDMode="Static" runat="server" NullToDisplay="请选择商品分类" CssClass="iselect" /></li>
                    <li class="formselect" style="display:none">
                        <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" ClientIDMode="Static" NullToDisplay="请选择商品品牌" CssClass="iselect" /></li>
                    <li>
                        <input type="hidden" name="hidProductType" id="hidProductType" value="<%=productType %>" />
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>
            </div>
            <!--结束-->
            <div class="functionHandleArea clearfix">

                <div class="batchHandleArea">
                    <div class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                    </div>

                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>



            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th width="50px;"></th>
                        <th width="50%">商品名称</th>
                        <th>成本价格</th>
                        <th>商品价格</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
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
    <input type="hidden" id="hidAllProductIds" />
    <div class="modal_iframe_footer">
        <a href="javascript:void(0)" class="btn btn-primary" onclick="return AddBindProduct()">添加</a>
        <%--<a href="javascript:void(0)" class="btn btn-danger" onclick="return GetAllProductIds()" title="一键添加当前所有筛选结果">一键添加</a>--%>
    </div>


    <script id="datatmpl" type="text/html">
        {{each rows as item index}}                
                <tr>
                    <td>
                        <input name="CheckBoxGroup" type="checkbox" value='{{item.ProductId}}|||{{item.ProductName}}' class="icheck" /></td>
                    <td>
                        <div style="float: left; margin-right: 10px; width: 13%;">
                            <a href='../../ProductDetails.aspx?productId={{item.ProductId}}' target="_blank">{{if item.ThumbnailUrl40}}
                                    <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; width: 40px; height: 40px;" alt="" />
                                {{else}}
                                    <img src="/utility/pics/none.gif" style="border-width: 0px; width: 40px; height: 40px;" alt="" />
                                {{/if}}
                            </a>
                        </div>
                        <div style="float: left; width: 80%;">
                            <a href='../../ProductDetails.aspx?productId={{item.ProductId}}' target="_blank">{{item.ProductName}}</a>{{#item.ProductType==1?"<font style='color: red;'>（服务）</font>":""}}
                        </div>
                        <div style="clear: both"></div>
                    </td>
                    <td>{{ item.CostPrice.toFixed(2)}}</td>
                    <td>一口价：{{ item.SalePrice.toFixed(2)}}</td>
                </tr>
        {{/each}}
    </script>
    <input type="hidden" id="hidFilterProductIds" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidIsIncludeHomeProduct" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidIsIncludeAppletProduct" runat="server" clientidmode="Static" />
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/home/ashx/SearchProduct.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/home/scripts/SearchProduct.js?v=3.1" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">
    <script language="javascript" type="text/javascript">

        function AddBindProduct() {
            var chks = $("input[name='CheckBoxGroup']:checked");
            if (chks.length <= 0) {
                alert("请选择商品");
                return false;
            }
            var origin = artDialog.open.origin;
            var arr = new Array();
            $(chks).each(function (i, item) {
                arr.push($(item).val());
            });
            if (origin.document.getElementById("ctl00_contentHolder_hidSelectProducts") != null && origin.document.getElementById("ctl00_contentHolder_hidSelectProducts") != undefined) {
                $(origin.document.getElementById("ctl00_contentHolder_hidSelectProducts")).val(arr.join(",,,"));
            }
            if (origin.document.getElementById("hidSelectProducts") != null && origin.document.getElementById("hidSelectProducts") != undefined) {
                $(origin.document.getElementById("hidSelectProducts")).val(arr.join(",,,"));
            }
            art.dialog.close();
        }

        function AddAllProduct() {
            GetAllProductIds();
        }
    </script>
</asp:Content>
