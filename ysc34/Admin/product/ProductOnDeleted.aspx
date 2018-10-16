<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductOnDeleted.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ProductOnDeleted" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
       .red{ color:red;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea ">
                <ul>

                    <li><span>商品名称：</span><span>
                        <input type="text" id="txtSearchText" class="forminput form-control" /></span></li>
                    <li><span>商家编码：</span><span>
                        <input type="text" id="txtSKU" class="forminput form-control" /></span></li>
                    <li><span>添加时间：</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarStartDate"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <Hi:CalendarPanel runat="server" Width="160px" ID="calendarEndDate" ClientIDMode="Static" IsEndDate="true"></Hi:CalendarPanel>
                    </li>
                    <li>
                        <span>商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" ClientIDMode="Static" runat="server" NullToDisplay="请选择商品分类" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>
                        <span>商品品牌：</span>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList runat="server" ClientIDMode="Static" ID="dropBrandList" NullToDisplay="请选择品牌" CssClass="iselect"></Hi:BrandCategoriesDropDownList>
                        </abbr>
                    </li>

                    <li>
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
                    <div class="btn-group pull-left" role="group" aria-label="...">
                        <span class="btn btn-default">
                            <a href="javascript:post_ChageStatus('UpShelf')">还原到出售中</a>
                        </span>

                        <span class="btn btn-default">


                            <a href="javascript:post_ChageStatus('OffShelf')">还原到下架区</a>
                        </span>

                        <span class="btn btn-default">

                            <a href="javascript:post_ChageStatus('InStock')">还原到仓库里</a>
                        </span>
                    </div>
                    <a href="javascript:void(0)" onclick="deleteProducts();" class="btn btn-default ml20 pull-left">彻底删除</a>


                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>

                </div>
            </div>
            <!--S DataShow-->
            <table class="table table-striped">
                <tr>
                    <th style="width: 5%;"></th>
                    <th scope="col" style="width: 5%;">排序</th>
                    <th scope="col" style="width: 40%">商品</th>
                    <th scope="col" style="width: 8%;">库存</th>
                    <th scope="col" style="width: 10%;">市场价</th>
                    <th scope="col" style="width: 8%">成本价</th>
                    <th scope="col" style="width: 8%;">一口价</th>
                    <th class="td_left td_right_fff" scope="col">操作</th>
                </tr>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--E DataShow-->
            <div class="blank12 clearfix"></div>

            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <tr>
                    <td>
                        <span class="icheck">
                            <input name="CheckBoxGroup" type="checkbox" value="{{item.ProductId}}" />
                        </span>
                    </td>
                    <td>{{item.DisplaySequence}}</td>
                    <td>
                        <div style="float: left; margin-right: 10px">
                            <a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">
                                <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px;" width="40">
                            </a>
                        </div>
                        <div class="p_list_fr">
                            <a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">{{item.ProductName}}</a>
                            <span class="colorC">商家编码：{{item.ProductCode}}</span>
                        </div>
                        <p class="red">{{item.ProductType==1?"（服务）":""}}</p>

                    </td>
                    <td>
                        <span>{{item.Stock}}</span>
                    </td>

                    <td>{{item.MarketPrice}}
                    </td>
                    <td>
                        <span>{{item.CostPrice.toFixed(2)}}</span>
                    </td>
                    <td>
                        <span>{{item.SalePrice.toFixed(2)}}</span>
                    </td>

                    <td style="white-space: nowrap;">
                        <span class="submit_shanchu"><a href="javascript:void(0)" onclick="deleteProduct('{{item.ProductId}}');">彻底删除</a></span>

                    </td>
                </tr>
                {{/each}}
                   
            </script>
            <!--E Data Template-->
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

    <%--彻底删除--%>
    <div id="deleteProduct" style="display: none;">
        <div class="frame-content">
            是否删除图片：<asp:CheckBox ID="chkDeleteImage" Text="删除图片" Checked="true" runat="server" onclick="javascript:SetPenetrationStatus(this)" />
        </div>
    </div>

    <div style="display: none">
        <input type="button" value="删除" id="btnOKs" onclick="bat_delete()" />
        <input type="button" value="删除" id="btndelete" onclick="deleteModel()" />
        <input type="hidden" id="hdPenetrationStatus" value="1" />
        <input type="hidden" id="currentProductId" />
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/product/ashx/ProductOnDeleted.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/product/scripts/ProductOnDeleted.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function deleteProduct(productId) {
            $("#currentProductId").val(productId);
            DialogShow("彻底删除商品", "productdel", "deleteProduct", "btndelete");
        }


        function deleteProducts() {

            DialogShow("彻底删除商品", "productdel", "deleteProduct", "btnOKs");
        }


        function validatorForm() {

            return true;
        }

        function SetPenetrationStatus(checkobj) {
  
            if (checkobj.checked) {
                $("#hdPenetrationStatus").val("1");
            } else {
                $("#hdPenetrationStatus").val("0");
            }
        }
    </script>


</asp:Content>
