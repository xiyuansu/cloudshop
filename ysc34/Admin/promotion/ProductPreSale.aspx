<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductPreSale.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.ProductPreSale" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddProductPreSale.aspx">添加</a></li>
            </ul>
        </div>
        <!--搜索-->

        <!--结束-->

        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul class="a_none_left">

                    <li><span>商品名称：</span><span>
                        <input type="text" id="txtProductName" class="forminput form-control" /></span></li>
                    <li>
                        <span>活动状态：</span>
                        <span>
                            <select id="ddlStatus" class="iselect">
                                <option value="">请选择状态</option>
                                <option value="1">进行中</option>
                                <option value="2">已结束</option>
                            </select>
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>
                 <table cellspacing="0" border="0" class="table table-striped table-fixed">
            <thead>
                <tr>
                    <th width="35%">商品名称</th>
                    <th width="10%">状态</th>
                    <th width="15%">预售结束时间</th>
                    <th width="20%">尾款支付时间</th>
                    <th width="15%" class="td_left td_right_fff" scope="col">操作</th>
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
    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
   
                {{each rows as item index}}
                <tr>
                    <td><a href="../../PreSaleProductDetails.aspx?PreSaleId={{item.PreSaleId}}" title="{{item.ProductName}}" target="_blank">{{item.ProductName}}
                    </a></td>
                    <td>{{if item.IsPreSaleEnd}}
                        已结束
                        {{else}}
                        进行中
                        {{/if}}
                    </td>
                    <td>{{ item.PreSaleEndDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{ item.PaymentStartDate | artex_formatdate:"yyyy-MM-dd"}}至{{ item.PaymentEndDate | artex_formatdate:"yyyy-MM-dd"}}</td>
                    <td class="operation">
                        <span><a href="/admin/promotion/ProductPreSaleView.aspx?preSaleId={{item.PreSaleId}}">查看</a></span>
                        <span><a href='javascript:void(0);' onclick="javascript:DialogFrame('promotion/ProductPreSaleLink.aspx?preSaleId={{item.PreSaleId}}', '活动二维码', 480, 300, null);">链接</a></span>
                        {{if item.IsPreSaleHasOrder}}
                        <span><a href="/admin/promotion/ProductPreSaleOrderList.aspx?preSaleId={{item.PreSaleId}}">预售记录</a></span>
                        {{if !item.IsPreSaleEnd }}
                        <span><a href="javascript:Post_SetOver('{{item.PreSaleId}}')">结束预售</a></span>
                        {{/if}}
                        {{else}}
                         {{if !item.IsPreSaleEnd }}
                         <span><a href="/admin/promotion/EditProductPreSale.aspx?preSaleId={{item.PreSaleId}}">编辑</a></span>
                        {{/if}}
                        <span><a href="javascript:Post_Delete('{{item.PreSaleId}}')">删除</a></span>
                        {{/if}}
                        
                    </td>

                </tr>
                {{/each}}
     
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/ProductPreSale.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/ProductPreSale.js" type="text/javascript"></script>
</asp:Content>
