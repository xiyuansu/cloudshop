<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="AuditProductList.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.AuditProductList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="hidProductId" runat="server" />
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
                    <li>
                        <span>商品名称：</span><span>

                            <input type="text" id="txtSearchText" class="forminput form-control" />
                        </span></li>
                    <li>
                        <span>商家编码：</span><span>

                            <input type="text" id="txtSKU" class="forminput form-control" />
                        </span></li>

                    <li>
                        <span>商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ClientIDMode="Static" ID="dropCategories" runat="server" NullToDisplay="请选择商品分类" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>
                        <span>审核状态：</span>

                        <select class="iselect" id="ddlAuditStatus">
                            <option value="0">请选择类型</option>
                            <option value="1">审核中 </option>
                            <option value="3">未通过</option>
                        </select>

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

                    <a class="btn btn-default ml20" href="javascript:bat_delete()">删除</a>
                    <div class="paginalNum">
                        <div class="pageHandleArea">
                            <ul>
                                <li class="paginalNum"><span>每页显示数量：</span><Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <!--数据列表区域-->

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
                                <th scope="col" style="width: 5%;">库存</th>
                                <th scope="col" style="width: 10%;">状态</th>
                                <th scope="col" style="width: 10%;">备注</th>
                                <th scope="col" style="width: 17%;">操作</th>
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
                        <input name="CheckBoxGroup" type="checkbox" class="icheck" value="{{item.ProductId}}">
                    </td>

                    <td>

                        <div style="float: left; margin-right: 10px;">
                            <a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">
                                <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; width: 40px;">
                            </a>
                        </div>
                        <div class="p_list_fr" style="width: 80%; float: left;">
                            <span class="Name"><a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">{{item.ProductName}}  </a></span>
                        </div>
                    </td>
                    <td>{{item.ProductCode}}
                    </td>
                    <td><b>{{if item.CostPrice==null}}
                             {{item.CostPrice}} 
                             {{else}}
                               {{item.CostPrice.toFixed(2)}} 
                              {{/if}} </b></td>
                    <td>{{item.Stock}}  </td>
                    <td>{{item.AuditStatusStr}} </td>
                    <td>{{item.AuditReson}}</td>
                    <td>
                        <span><a target="_blank" href="/ProductDetails.aspx?productId={{item.ProductId}}">预览</a></span>
                        <span><a href="EditProduct.aspx?productId={{item.ProductId}}">{{if item.AuditStatus==3}}
                                    编辑提交
                                  {{else}}
                                    编辑
                                    {{/if}}

                        </a></span>
                        <span><a href="javascript:DeleteModel({{item.ProductId}})">删除</a></span>
                    </td>
                </tr>
                    {{/each}}
                        
                </script>
            </div>

        </div>

        <input type="hidden" name="dataurl" id="dataurl" value="/Supplier/product/ashx/AuditProductList.ashx" />
        <input type="hidden" name="dataurl" id="adjustStockurl" value="../../admin/product/EditStocks.aspx" />
        <script src="/Utility/artTemplate.js" type="text/javascript"></script>
        <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
        <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
        <script src="scripts/AuditProductList.js" type="text/javascript"></script>
</asp:Content>
