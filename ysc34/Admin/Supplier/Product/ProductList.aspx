<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.ProductList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:void(0);" class="tab_status" data-status="-1">全部商品</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="<%=Hidistro.Entities.Commodities.ProductSaleStatus.OnSale.GetHashCode() %>">出售中</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="<%=Hidistro.Entities.Commodities.ProductSaleStatus.UnSale.GetHashCode() %>">下架中</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="<%=Hidistro.Entities.Commodities.ProductSaleStatus.OnStock.GetHashCode() %>">仓库中</a></li>
            </ul>
        </div>

        <div class="datalist">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li><span>商品名称：</span><span>
                        <input type="text" id="txtSearchText" runat="server" clientidmode="Static" class="forminput form-control" /></span></li>
                    <li><span>商家编码：</span><span>
                        <input type="text" id="txtSKU" class="forminput form-control"  runat="server" clientidmode="Static"/>
                    </span>
                    </li>
                    <li>
                        <span>商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" ClientIDMode="Static" runat="server" NullToDisplay="请选择商品分类" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>
                        <span>供应商：</span>
                        <abbr class="formselect">
                            <Hi:SuplierDropDownList ID="dropSuplier" ClientIDMode="Static" runat="server" NullToDisplay="请选择供应商" CssClass="iselect"></Hi:SuplierDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <span>商品品牌：</span>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" ClientIDMode="Static" NullToDisplay="请选择品牌"
                                CssClass="iselect">
                            </Hi:BrandCategoriesDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <span>商品标签：</span>
                        <abbr class="formselect">
                            <Hi:ProductTagsDropDownList runat="server" ID="dropTagList" ClientIDMode="Static" NullToDisplay="请选择标签"
                                CssClass="iselect">
                            </Hi:ProductTagsDropDownList>
                        </abbr>
                    </li>

                    <li>
                        <input type="hidden" name="hidStatus" id="hidStatus"  value="<%=saleStatus.GetHashCode() %>" />
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                        <a href="javascript:;" id="so_more">更多搜索条件&nbsp;<i class="glyphicon glyphicon-menu-down"></i></a>
                        <input type="text" value="" id="so_more_input" style="display: none;" runat="server" clientidmode="Static" />
                    </li>
                </ul>
                <ul id="so_more_none" style="display: none;" runat="server" clientidmode="Static">
                    <li>
                        <span>商品类型：</span>
                        <abbr class="formselect">
                            <Hi:ProductTypeDownList ID="dropType" ClientIDMode="Static" runat="server" NullToDisplay="请选择类型" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>
                        <label><input type="checkbox" id="chkIsWarningStock"  class="icheck kc-danger" <%=isWarningStock?"checked":"" %>/>库存报警</label>
                    </li>

                </ul>
            </div>
            <div class="functionHandleArea clearfix">
                <div class="batchHandleArea">
                    <div class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                    </div>
                    <select id="dropBatchOperation" class="iselect">
                        <option value="">批量操作</option>
                        <option value="1">商品上架</option>
                        <option value="2">商品下架</option>
                        <option value="3">商品入库</option>
                        <option value="10">调整基本信息</option>
                        <option value="11">调整显示销售数量</option>
                        <option value="13">调整会员零售价</option>
                        <option value="15">调整商品关联标签</option>
                    </select>
                    <a class="btn btn-default ml20" href="javascript:bat_delete()">删除</a>
                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>
            <table class="table table-striped" cellspacing="0" border="0" id="ctl00_contentHolder_grdProducts" style="border-collapse: collapse;">
                <thead>
                    <tr>

                        <th scope="col" style="width:50px;">&nbsp;</th>
                        <th scope="col" style="width:50px;">排序</th>
                        <th scope="col" style="width: 38%;">商品</th>
                        <th scope="col">供应商</th>
                        <th scope="col" style="width: 10%;">供货价</th>
                        <th scope="col" style="width: 8%;">一口价</th>
                        <th scope="col" style="width: 8%;">商品状态</th>
                        <th scope="col" style="width: 50px;">库存</th>
                        <th scope="col" style="width: 11%;">操作</th>
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
    <%-- 上架商品--%>
    <div id="divOnSaleProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要上架商品？上架后商品将前台出售</em>
            </p>
        </div>
    </div>
    <%-- 下架商品--%>
    <div id="divUnSaleProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要下架商品？</em>
            </p>
            <p>
                <em>（友情提示:正在参加活动的商品不能被下架的哦！）</em>
            </p>
        </div>
    </div>
    <%-- 入库商品--%>
    <div id="divInStockProduct" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定要将商品入库？</em>
            </p>
            <p>
                <em>（友情提示:正在参加活动的商品不能被入库的哦！）</em>
            </p>
        </div>
    </div>
    <div style="display: none">
        <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine"></Hi:TrimTextBox>
        <input type="button" name="btnInStock" value="入库商品" id="btnInStock" class="btn btn-primary" />
        <input type="button" name="btnUnSale" value="下架商品" id="btnUnSale" class="btn btn-primary" />
        <input type="button" name="btnUpSale" value="上架商品" id="btnUpSale" class="btn btn-primary" />
    </div>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{if item.SalePrice!=0}}
                        <input type="checkbox" name="CheckBoxGroup" value="{{item.ProductId}}" class="icheck" />
                        {{else}}
                        &nbsp;
                        {{/if}}
                    </td>
                    <td>{{item.DisplaySequence}}</td>
                    <td>
                        <div style="float: left; margin-right: 10px;">
                            <a href="/ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">
                                <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px;max-width:40px;">
                            </a>
                        </div>
                        <div class="p_list_fr">
                            <span class="Name"><a href="/ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">{{item.ProductName}}</a></span>
                        </div>
                    </td>
                    <td>{{item.SupplierName}}
                    </td>
                    <td>
                        <b>{{if item.CostPrice}}
                            {{item.CostPrice.toFixed(2)}}
                            {{else}}
                            0.00
                            {{/if}}
                        </b>
                    </td>
                    <td>
                        <b>{{if item.SalePrice && item.SalePrice!=0}}{{item.SalePrice.toFixed(2)}}{{/if}}</b>
                    </td>
                    <td style="width: 70px;">
                        <span>{{item.SaleStatusText}}</span>
                    </td>
                    <td>
                        <span>{{item.Stock}}</span>
                    </td>
                    <td style="white-space: nowrap;">
                        <div class="operation">
                            <span><a href="javascript:void(0)" onclick="ToEdit({{item.ProductId}})">编辑</a></span>
                            <span><a href="javascript:CollectionProduct('EditReleteProducts.aspx?productId={{item.ProductId}}')">相关商品</a></span>
                            <span>
                                <a href="javascript:Post_Delete('{{item.ProductId}}')">删除</a></span>
                        </div>
                    </td>
                </tr>
        {{/each}}
         
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/Supplier/Product/ashx/ProductList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/Supplier/Product/scripts/ProductList.js?v=3.32" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">


    <script type="text/javascript">
        $(document).ready(function () {
            $("#dropBatchOperation").bind("change", function () { SelectOperation(); });
        });

        function SelectOperation() {
            var Operation = $("#dropBatchOperation").val();
            var productIds = GetProductId();
            if (productIds.length > 0) {
                switch (Operation) {
                    case "1":
                        formtype = "onsale";
                        arrytext = null;
                        DialogShow("商品上架", "productonsale", "divOnSaleProduct", "btnUpSale");
                        break;
                    case "2":
                        formtype = "unsale";
                        arrytext = null;
                        DialogShow("商品下架", "productunsale", "divUnSaleProduct", "btnUnSale");
                        break;
                    case "3":
                        formtype = "instock";
                        arrytext = null;
                        DialogShow("商品入库", "productinstock", "divInStockProduct", "btnInStock");
                        break;
                    case "10":
                        DialogFrame("product/EditBaseInfo?ProductIds=" + productIds + "&callback=ShowSuccessAndReloadData", "调整商品基本信息", 1000, null, function () { ReloadPageData() });
                        break;
                    case "11":
                        DialogFrame("product/EditSaleCounts?ProductIds=" + productIds + "&callback=ShowSuccessAndReloadData", "调整前台显示的销售数量", null, null, function () { ReloadPageData() });
                        break;
                    case "13":
                        DialogFrame("product/EditMemberPrices?SupplierId=1&ProductIds=" + productIds + "&callback=ShowSuccessAndReloadData", "调整会员零售价", 1200, null, function () { ReloadPageData() });
                        break;
                    case "15":
                        DialogFrame("product/EditProductTags?ProductIds=" + productIds + "&callback=ShowSuccessAndReloadData", "调整商品关联标签", 800, null, function () { ReloadPageData() });
                        break;
                }
            }
            $("#dropBatchOperation").val("");
        }

        function GetProductId() {
            var v_str = "";

            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });

            if (v_str.length == 0) {
                ShowMsg("请选择商品");
                return "";
            }
            return v_str.substring(0, v_str.length - 1);
        }

        function CollectionProduct(url) {
            DialogFrame("product/" + url, "相关商品");
        }

        function ShowStoreStock(url) {
            DialogFrame("product/" + url, "门店存库", 900, 600);
        }

        function validatorForm() {
            switch (formtype) {
                case "tag":
                    if ($("#ctl00_contentHolder_txtProductTag").val().replace(/\s/g, "") == "") {
                        ShowMsg("请选择商品标签");
                        return false;
                    }
                    break;
                case "onsale":
                case "unsale":
                case "instock":
                case "setFreeShip":
                case "cancelFreeShip":
                    setArryText('ctl00_contentHolder_hdPenetrationStatus', $("#ctl00_contentHolder_hdPenetrationStatus").val());
                    break;
            };
            return true;
        }
    </script>
</asp:Content>

