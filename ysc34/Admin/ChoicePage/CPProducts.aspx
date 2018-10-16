<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CPProducts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ChoicePage.CPProducts" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        html { background: #fff !important; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 20px 20px 60px 20px; width: 100% !important; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <textarea name="hidformData" id="hidformData" style="display:none;"><%=formData %></textarea>
    <input type="hidden" name="hidreturnUrl" id="hidreturnUrl" value="<%=returnUrl %>" />
    <input type="hidden" name="hidIsHasStock" id="hidIsHasStock" value="<%=IsHasStock %>" />
    <input type="hidden" name="hidProductCode " id="hidProductCode " value="<%=ProductCode  %>" />
    <input type="hidden" name="hidStartDate" id="hidStartDate" value="<%=StartDate %>" />
    <input type="hidden" name="hidTypeId" id="hidTypeId" value="<%=TypeId %>" />
    <input type="hidden" name="hidSaleStatus" id="hidSaleStatus" value="<%=SaleStatus %>" />
    <input type="hidden" name="hidEndDate" id="hidEndDate" value="<%=EndDate %>" />
    <input type="hidden" name="hidIsWarningStock" id="hidIsWarningStock" value="<%=IsWarningStock %>" />
    <input type="hidden" name="hidFilterProductIds" id="hidFilterProductIds" value="<%=FilterProductIds %>" />
    <input type="hidden" name="hidIsFilterFightGroupProduct" id="hidIsFilterFightGroupProduct" value="<%=IsFilterFightGroupProduct %>" />
    <input type="hidden" name="hidIsFilterBundlingProduct" id="hidIsFilterBundlingProduct" value="<%=IsFilterBundlingProduct %>" />
    <input type="hidden" name="hidIsFilterPromotionProduct" id="hidIsFilterPromotionProduct" value="<%=IsFilterPromotionProduct %>" />
    <input type="hidden" name="hidIsFilterCountDownProduct" id="hidIsFilterCountDownProduct" value="<%=IsFilterCountDownProduct %>" />
    <input type="hidden" name="hidIsFilterGroupBuyProduct" id="hidIsFilterGroupBuyProduct" value="<%=IsFilterGroupBuyProduct %>" />
    <input type="hidden" name="hidNotInCombinationMainProduct" id="hidNotInCombinationMainProduct" value="<%=NotInCombinationMainProduct %>" />
    <input type="hidden" name="hidNotInPreSaleProduct" id="hidNotInPreSaleProduct" value="<%=NotInPreSaleProduct %>" />
    <input type="hidden" name="hidNotInCombinationOtherProduct" id="hidNotInCombinationOtherProduct" value="<%=NotInCombinationOtherProduct %>" />


    <div class="dataarea mainwidth databody">

        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li><span>商品名称：</span><span>
                         <input type="text" id="txtSearchText" class="forminput form-control float"/></span></li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" ClientIDMode="Static" runat="server" NullToDisplay="请选择商品分类"
                                CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList runat="server" ClientIDMode="Static" ID="dropBrandList" NullToDisplay="请选择品牌"
                                CssClass="iselect">
                            </Hi:BrandCategoriesDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductTagsDropDownList runat="server" ClientIDMode="Static" ID="dropTagList" NullToDisplay="请选择标签"
                                CssClass="iselect">
                            </Hi:ProductTagsDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>
            

            <!--数据列表区域-->

            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>商品</th>
                        <th width="130">成本价</th>
                        <th width="130">商品价格</th>
                        <th width="50">操作</th>
                    </tr>
                </thead>
                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>
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
        {{each rows as item index}}
                <tr>
                    <td>
                        <div style="float: left; margin-right: 10px;">
                            <a href='../../ProductDetails.aspx?productId={{item.ProductId}}' target="_blank">
                                <img src="{{item.ThumbnailUrl40}}" width="40" height="40" />
                            </a>
                        </div>
                        <div class="p_list_fr">
                            <span class="Name"><a href='../../ProductDetails.aspx?productId={{item.ProductId}}' target="_blank">{{item.ProductName}}</a></span>
                            <span class="colorC">库存:{{item.Stock}}</span>
                        </div>
                        <div style="clear: both;"></div>
                    </td>
                    <td>{{if item.CostPrice}}
                        {{item.CostPrice.toFixed(2)}}
                        {{else}}
                        -
                        {{/if}}
                    </td>
                    <td>市场价：{{if item.MarketPrice}}
                        {{item.MarketPrice.toFixed(2)}}
                        {{else}}
                        -
                        {{/if}}</td>

                    <td>
                        <a class="a_select" href="javascript:closewindow('{{item.ProductId}}')">选择</a>
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="callback" id="callback" value="<%=JsCallBack%>" />
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/ChoicePage/ashx/CPProducts.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/ChoicePage/scripts/CPProducts.js" type="text/javascript"></script>


    <script type="text/javascript">
        function closewindow(id) {
            var win = art.dialog.open.origin;
            var callback = $("#callback").val();
            var returnUrl = $("#hidreturnUrl").val();
            var formData = $("#hidformData").val();
            if (callback && callback.length > 0) {
                artwin[callback]();
            } else {
                if (returnUrl && returnUrl.length > 0) {
                    if (returnUrl.indexOf("?") > -1) {
                        returnUrl += "&";
                    } else {
                        returnUrl += "?";
                    }
                    returnUrl += "productId=" + id + "&";
                    if (formData.length > 0) {
                        returnUrl += "formData=" + formData + "&";
                    }
                    win.location.href = returnUrl;
                } else {
                    win.location.reload();
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
