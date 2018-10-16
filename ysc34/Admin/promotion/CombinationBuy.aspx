<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CombinationBuy.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.CombinationBuy" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">


    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddCombinationBuy.aspx">添加</a></li>
            </ul>
        </div>
        <!--搜索-->

        <!--结束-->

        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul class="a_none_left">

                    <li><span>商品名称：</span><span>
                        <input type="text" id="txtProductName" class="forminput form-control" value="<%=productName %>" />
                    </span></li>
                    <li>
                        <span>活动状态：</span>
                        <span>
                            <select id="ddlStatus" class="iselect">
                                <option value="">请选择状态</option>
                                <option value="0" <%=status==0?"selected":"" %>>即将开始</option>
                                <option value="1" <%=status==1?"selected":"" %>>正在进行</option>
                                <option value="2" <%=status==2?"selected":"" %>>已结束</option>
                            </select>
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>

            <div class="functionHandleArea clearfix m_none">
                <!--分页功能-->


                <!--结束-->
                <div class="batchHandleArea">
                    <div class="pageHandleArea pull-right">
                        <ul>
                            <li class="paginalNum"><span>每页显示数量：</span><Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>

            <!--S DataShow-->

            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th>商品组合</th>
                        <th width="15%">状态</th>
                        <th width="15%">开始时间</th>
                        <th width="15%">结束时间</th>
                        <th width="15%" class="td_left td_right_fff" scope="col">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--E DataShow-->
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
    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td><a href='../../ProductDetails.aspx?productId={{item.MainProductId}}' title="{{item.ProductName}}" target="_blank">
                        <img src="{{item.ThumbnailUrl40}}" border="0" width="40" height="40" />
                    </a>
                        <img src="../images/icon_add.png" />
                        {{each item.OtherProductsImg as subitem index}}
                        <a href='../../ProductDetails.aspx?productId={{subitem.ProductId}}' title="{{subitem.ProductName}}" target="_blank">
                            <img src="{{subitem.ThumbnailUrl40}}" border="0" width="40" height="40" />
                        </a>
                        {{/each}}
                    </td>
                    <td>{{item.StatusText}}</td>
                    <td>{{ item.StartDate | artex_formatdate:"yyyy-MM-dd"}}</td>
                    <td>{{ item.EndDate | artex_formatdate:"yyyy-MM-dd"}}</td>
                    <td>{{if item.StatusText=="即将开始"}}
                        <span class="submit_bianji"><a href="EditCombinationBuy.aspx?editType=1&combinationId={{item.CombinationId}}">编辑</a></span>
                        {{else if item.StatusText=="正在进行"}}
                        <span class="submit_bianji"><a href="EditCombinationBuy.aspx?editType=2&combinationId={{item.CombinationId}}">编辑</a></span>
                        <span class="submit_bianji"><a href="javascript:Post_End('{{item.CombinationId}}')">立即结束</a></span>
                        {{/if}}
                        {{if item.StatusText!="正在进行"}}
                        <span class="submit_shanchu"><a href="javascript:Post_Delete('{{item.CombinationId}}')">删除</a></span>
                        {{/if}}
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/CombinationBuy.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/CombinationBuy.js" type="text/javascript"></script>
</asp:Content>
