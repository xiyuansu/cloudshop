<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.ProductList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <asp:HiddenField ID="hidFilter" ClientIDMode="Static" runat="server" />
            <ul class="title-nav">
                <li><a id="all" href="javascript:void" class="tab_status" data-filter="-1">全部商品</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-filter="1">出售中</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-filter="2">下架中</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-filter="3">仓库中</a></li>
            </ul>
            </div>

        <div class="datalist">
            <!--搜索-->

            <div class="searcharea">
                <ul>
                    <li>
                        <span>商品名称：</span><span>
                            <input type="text" clientidmode="Static" id="txtSearchText" class="forminput form-control" />
                        </span></li>
                    <li>
                        <span>商家编码：</span><span>

                            <input type="text" clientidmode="Static" id="txtSKU" class="forminput form-control" />
                        </span></li>
                    <li>
                        <span>商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="请选择商品分类" CssClass="iselect" ClientIDMode="Static" />
                        </abbr>
                    </li>

                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                        <asp:CheckBox ID="chkIsWarningStock" runat="server" Text="库存报警" CssClass="icheck kc-danger" ClientIDMode="Static" />

                    </li>
                </ul>   
            </div>
            <div class="functionHandleArea clearfix">
                <div class="batchHandleArea">
                    <div class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                    </div>
                    <input type="button" id="btnSetStock" class="btn btn-default ml20" value="批量调整库存" onclick="batchAdjustSupplier();" />
                    <div class="paginalNum">
                        <ul>
                            <li class="paginalNum"><span>每页显示数量：</span><Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th scope="col" style="width: 5%;"></th>
                                <th scope="col" style="width: 33%;">商品</th>
                                <th scope="col" style="width: 10%;">商家编码</th>
                                <th scope="col" style="width: 10%;">供货价</th>
                                <th scope="col" style="width: 10%;">商品状态</th>
                                <th scope="col" style="width: 10%;">库存</th>
                                <th scope="col" style="width: 10%;">销量</th>
                                <th class="td_left td_right_fff" scope="col" style="width: 17%;">操作</th>
                            </tr>
                        </thead>
                        <tbody id="datashow"></tbody>
                    </table>
                    <div class="blank12 clearfix"></div>
                </div>
                <!--S Pagination-->
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
                    <td>
                        <span class="icheck">
                            <input name="CheckBoxGroup" type="checkbox" class="" value="{{item.ProductId}}" /></span>

                    </td>

                    <td>

                        <div style="float: left; margin-right: 10px;">
                            <a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">
                                <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px;width: 40px;">
                            </a>
                        </div>
                        <div class="p_list_fr" style="width: 80%; float: left;">
                            <span class="Name"><a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">{{item.ProductName}}  </a></span>
                        </div>
                    </td>
                    <td>
                        <span class="colorC">{{item.ProductCode}} </span>
                    </td>
                    <td><span class="Name"><b>{{if item.CostPrice==null}}
                             {{item.CostPrice}} 
                             {{else}}
                               {{item.CostPrice.toFixed(2)}} 
                              {{/if}} </b></span><span class="colorC"></span></td>
                    <td>
                        <span>{{item.SaleStatus | SaleStatus:"integer"}}</span>
                    </td>
                    <td>
                        <span class="colorC">{{item.Stock}} 
                             
                        </span>
                    </td>
                    <td><span>{{item.SaleNum}}  </span></td>
                    <td style="white-space: nowrap;">
                        <span><a href="EditProduct.aspx?productId={{item.ProductId}}">编辑</a></span>
                        <span class="submit_bianji"><a href="javascript:void(0)" onclick="openAdjustStockPage(this.title)" title="{{item.ProductId}}">调整库存</a></span>
                        <%--<span><a href="javascript:void(0);" onclick="javascript: DialogFrame('../../admin/product/EditStocks.aspx?ProductIds=<%#  Eval("ProductId")%>', '调整库存', 1000, null, function (e) { location.reload(); })">库存修改</a></span>      --%>

                    </td>
                </tr>
                    {{/each}}
            
                </script>
            </div>
        </div>


        <div style="display: none">

            <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine"></Hi:TrimTextBox>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
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

    <input type="hidden" name="dataurl" id="dataurl" value="/Supplier/product/ashx/ProductManage.ashx" />
    <input type="hidden" name="dataurl" id="adjustStockurl" value="../../admin/product/EditStocks.aspx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="scripts/productmanage.js" type="text/javascript"></script>
</asp:Content>

