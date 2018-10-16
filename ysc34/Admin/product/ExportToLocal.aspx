<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ExportToLocal.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.ExportToLocal" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#ctl00_contentHolder_radCategory").bind("click", function () { $("#liCategory").show(); $("#liLine").hide(); });
            $("#ctl00_contentHolder_radLine").bind("click", function () { $("#liLine").show(); $("#liCategory").hide(); });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="exporttotb.aspx">导出为淘宝数据包</a></li>
                <li class="hover"><a href="javascript:void">导出为本地数据包</a></li>
                <%--  <li><a href="exporttopp.aspx">导出为拍拍数据包</a></li>--%>
            </ul>
        </div>
        <!--搜索-->
        <div class="searcharea ">
            <ul>
                <li><span>商品名称：</span><span><input type="text" id="txtSearchText" class="forminput form-control" /></span></li>
                <li><span>商家编码：</span><span>
                    <input type="text" id="txtSKU" class="forminput form-control" /></span></li>
                <li><span>添加时间：</span>
                    <Hi:CalendarPanel runat="server" ID="calendarStartDate" ClientIDMode="Static"></Hi:CalendarPanel>
                    <span class="Pg_1010">至</span>
                    <Hi:CalendarPanel runat="server" ID="calendarEndDates" IsEndDate="true" ClientIDMode="Static"></Hi:CalendarPanel>
                </li>
                <li>
                    <span>商家分类：</span>
                    <abbr class="formselect">
                        <Hi:ProductCategoriesDropDownList ClientIDMode="Static" ID="dropCategories" runat="server" NullToDisplay="请选择商品分类" CssClass="iselect" />
                    </abbr>
                </li>
                <li class="P_state">
                    <span>包含状态：</span>
                    <span class="icheck-label-5-10">

                        <input type="checkbox" class="icheck" id="chkOnSales" checked="true" />出售中
                         <input type="checkbox" class="icheck" id="chkUnSales" />已下架
                         <input type="checkbox" class="icheck" id="chkInStock" />仓库中
                    </span>
                </li>
                <li>
                    <input type="button" name="btnSearch" value="筛选" id="btnSearch" class="btn btn-primary" /></li>
            </ul>
        </div>
        <!--结束-->
        <div class="functionHandleArea">
            <!--分页功能-->
            <div class="pageHandleArea">
                <ul>

                    <li><span>导出数量：</span><span id="lblTotals"></span>件</li>
                    <li style="width: 210px">
                        <span class="formitemtitle">&nbsp;&nbsp;&nbsp;&nbsp;是否导出商品图片：</span><Hi:OnOff ClientIDMode="Static" runat="server" ID="chkExportImages"></Hi:OnOff>

                    </li>
                    <input type="hidden" id="RemoveProductId" />
                    <input type="button" value="导出" class="btn btn-primary" onclick="Export()" />
                    <li></li>
                </ul>
            </div>
            <div class="paginalNum">
                <span>每页显示数量：</span>
                <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
            </div>
        </div>
        <!--数据列表区域-->
        <div class="datalist">
            <table class="table table-striped">
                <tr>
                    <th scope="col" style="width: 50%;">商品</th>
                    <th scope="col" style="width: 15%;">商家编码</th>
                    <th scope="col" style="width: 15%;">库存</th>
                    <th scope="col" style="width: 10%;">价格</th>
                    <th class="td_left td_right_fff" scope="col" style="width: 10%;">操作</th>
                </tr>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--E DataShow-->

            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <tr>
                    <td>
                        <div style="float: left; margin-right: 10px;">
                            <a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">
                                <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; width:40px;">
                            </a>
                        </div>
                        <div class="pro_name">
                            <a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">{{item.ProductName}}</a>
                        </div>
                    </td>
                    <td>
                        <span>{{item.ProductCode}}</span>
                    </td>

                    <td>
                        <span class="Name">{{item.Stock}}</span>
                    </td>
                    <td>
                        <span>{{item.SalePrice.toFixed(2)}}</span>
                    </td>


                    <td style="white-space: nowrap;">
                        <span class="submit_shanchu"><a href="javascript:void(0)" onclick="post_delete('{{item.ProductId}}');">除外</a></span>

                    </td>
                </tr>
                {{/each}}
                   
            </script>
            <!--E Data Template-->

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
        <!--E Pagination-->
    </div>

    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/product/ashx/ExportToLocal.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/product/scripts/ExportToLocal.js" type="text/javascript"></script>
</asp:Content>
