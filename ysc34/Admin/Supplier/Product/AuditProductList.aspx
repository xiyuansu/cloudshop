<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AuditProductList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.AuditProductList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnAudit").click(function () {
                var v_str = GetSelectId();
                if (v_str.length == 0) {
                    ShowMsg("请选择商品");
                    return;
                }
                var products = v_str;
                ShowAuditDiv(products, 1);
            });
            if ($("#hidProductId").val() != "" && parseInt($("#hidProductId").val()) > 0)
                ShowAuditDiv($("#hidProductId").val(), 111);
            $("#hidProductId").val("");
        });
        //审核明细
        function ShowAuditDiv(productId, salePrice) {
            //if (parseFloat(salePrice) > 0) {
            DialogFrame("supplier/product/AuditProduct.aspx?ProductIds=" + productId, "商品审核", 500, 250, function (e) { ReloadPageData(); })
            //} else {
            //    ShowMsg("此商品需要先设置一口价，才能审核！");
            //}
        }
        function checkReson() {
            if ($("#tbxReson").val() == "") {
                ShowMsg("拒绝时必须填写拒绝理由");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="hidProductId" ClientIDMode="Static" runat="server" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a id="all" href="javascript:return false;">待审核商品</a></li>
            </ul>
        </div>
        <div class="datalist">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li><span>商品名称：</span><span>
                        <input type="text" id="txtSearchText" runat="server" clientidmode="Static" class="forminput form-control" /></span></li>
                    <li>
                        <span>供应商：</span>
                        <abbr class="formselect">
                            <Hi:SuplierDropDownList ID="dropSuplier" ClientIDMode="Static" runat="server" NullToDisplay="请选择供应商" CssClass="iselect"></Hi:SuplierDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <span>商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" ClientIDMode="Static" runat="server" NullToDisplay="请选择商品分类" CssClass="iselect" />
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
                    <input id="btnAudit" type="button" class="btn btn-default ml20" value="批量审核" />
                    <div class="paginalNum">
                        <span>每页显示数量：</span><Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>

            <!--数据列表区域-->
            <table class="table table-striped" cellspacing="0" border="0" style="border-collapse: collapse;">
                <thead>
                    <tr>
                        <th scope="col" style="width: 5%;">&nbsp;</th>
                        <th scope="col" style="width: 35%;">商品</th>
                        <th scope="col" style="width: 10%;">分类</th>
                        <th scope="col  ">供应商</th>
                        <th scope="col" style="width: 10%;">供货价</th>
                        <th scope="col" style="width: 10%;">一口价</th>
                        <th scope="col" style="width: 10%;">库存</th>
                        <th scope="col" style="width: 13%;">操作</th>
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
                    <td>{{if item.SalePrice!=0}}
                        <input type="checkbox" name="CheckBoxGroup" value="{{item.ProductId}}" class="icheck" />
                        {{else}}
                        &nbsp;
                        {{/if}}
                    </td>
                    <td style="width: 360px;text-align:left;">
                        <div style="margin-right: 10px;width:40px;display:inline-block;">
                            <a href="/ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">
                                <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; max-width:40px;">
                            </a>
                        </div>
                        <div class="p_list_fr" style="display:inline-block;vertical-align:middle;">
                            <a href="/ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">{{item.ProductName}}</a>
                        </div>
                    </td>
                    <td>{{item.CategoryName}}
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
                    <td>
                        <span>{{item.Stock}}</span>
                    </td>
                    <td style="white-space: nowrap;">
                        <div class="operation">
                            <span><a target="_blank" href="/ProductDetails.aspx?productId={{item.ProductId}}">预览</a></span>
                            <span><a href="javascript:void(0)" onclick="ToEdit({{item.ProductId}})">编辑</a></span>
                            <span><a href="javascript:ShowAuditDiv({{item.ProductId}},{{item.SalePrice}});">审核</a></span>
                        </div>
                    </td>
                </tr>
        {{/each}}
         
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/Supplier/Product/ashx/AuditProductList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/Supplier/Product/scripts/AuditProductList.js?v=3.35" type="text/javascript"></script>
</asp:Content>
