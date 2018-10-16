<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SearchPromotionProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SearchPromotionProduct" %>

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
            padding: 20px 20px 90px 20px;
            width: 100% !important;
        }
    </style>
    <div class="dataarea mainwidth databody">
        <input type="hidden" id="hdIsMobileExclusive" clientidmode="Static" runat="server" value="0" />
        <input type="hidden" id="hdActivityId" clientidmode="Static" runat="server" value="0" />
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea clearfix">
                <ul>
                    <li><span>商品名称：</span><span>
                        <input type="text" id="txtSearchText" class="forminput form-control" value="<%=productName %>" /></span></li>
                    <li class="formselect">
                        <Hi:ProductCategoriesDropDownList ID="dropCategories" ClientIDMode="Static" runat="server" NullToDisplay="请选择商品分类" CssClass="iselect" /></li>
                    <li class="formselect">
                        <Hi:BrandCategoriesDropDownList runat="server" ClientIDMode="Static" ID="dropBrandList" NullToDisplay="请选择商品品牌" CssClass="iselect" /></li>
                    <li class="formselect">
                        <Hi:ProductTagsDropDownList runat="server" ID="dropTagList" ClientIDMode="Static" NullToDisplay="请选择商品标签" CssClass="iselect" />
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>
            <!--结束-->
            <div class="batchHandleArea mb_20 fl" style="width: 100%;">
                <div class="batchHandleButton ">
                    <!--分页功能-->
                    <span class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                </div>
                <div class="paginalNum">
                    <span>每页显示数量：</span>
                    <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                </div>
            </div>

            <!--S DataShow-->

            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th width="50"></th>
                        <th>商品名称</th>
                        <th width="100">成本价格</th>
                        <th width="100">商品价格</th>
                        <th width="80">库存</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
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


    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <div class="modal_iframe_footer">
        <Hi:ImageLinkButton ID="btnAdd" runat="server" Text="添 加" CssClass="btn btn-primary" OnClientClick="return VilaMobileExclusive()" />

    </div>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>
                        <input name="CheckBoxGroup" type="checkbox" value='{{item.ProductId}}' class="icheck" /></td>
                    <td>
                        <div style="float: left; margin-right: 10px; width: 13%;">
                            <a href='../../ProductDetails.aspx?productId={{item.ProductId}}' target="_blank">{{if item.ThumbnailUrl40}}
                                    <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; width: 40px; height: 40px;" alt="" />
                                {{else}}
                                    <img src="/utility/pics/none.gif" style="border-width: 0px; width: 40px; height: 40px;" alt="" />
                                {{/if}}
                            </a>
                        </div>
                        <div style="float: left; width: 82%;">
                            <a href='../../ProductDetails.aspx?productId={{item.ProductId}}' target="_blank">{{item.ProductName}}</a>
                            <br />
                            <span class="colorC">商家编码：{{item.ProductCode}}</span>
                        </div>
                        <div style="clear: both"></div>
                    </td>
                    <td>{{ item.CostPrice.toFixed(2)}}</td>
                    <td>一口价：{{ item.SalePrice.toFixed(2)}}</td>
                    <td>{{ item.Stock}}</td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/SearchPromotionProduct.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/SearchPromotionProduct.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">
    <script type="text/javascript">
        function VilaMobileExclusive() {
            var IsMobileExclusive = $("#hdIsMobileExclusive").val().replace(/\s/g, "");
            var activityId = $("#hdActivityId").val().replace(/\s/g, "");
            if (activityId != "" && parseInt(activityId) > 0) {
                if (IsMobileExclusive == "0") {
                    return true;
                }
                else {
                    var Iscontiniu = false;
                    var productids = "";
                    $.each($(":checkbox[name=CheckBoxGroup]:checked"), function () {
                        productids += $(this).val() + ",";
                    });
                    if (productids != "") {
                        productids = productids.substring(0, productids.length - 1);
                        $.ajax({
                            url: "SearchPromotionProduct",
                            type: "post",
                            dataType: "json",
                            timeout: 10000,
                            data: {
                                productids: productids,
                                activityId: activityId,
                                isCallback: "true"
                            },
                            async: false,
                            success: function (data) {
                                if (data.Status == "0") {
                                    Iscontiniu = true;
                                } else if (data.Status == "004") {
                                    if (confirm("您选择的商品中有部分商品优惠后价格为0,是否继续？")) {
                                        Iscontiniu = true;
                                    } else {
                                        Iscontiniu = false;
                                    }
                                }
                                else {
                                    ShowMsg("错误的请求信息！错误码：" + data.Status);
                                    Iscontiniu = false;
                                }
                            }
                        });

                        //ShowMsg(Iscontiniu);
                        return Iscontiniu;


                    } else {
                        ShowMsg("请先选择商品");
                        return false;
                    }
                }
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>
