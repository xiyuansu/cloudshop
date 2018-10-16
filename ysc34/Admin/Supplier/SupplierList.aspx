<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" EnableViewState="false" CodeBehind="SupplierList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.SupplierList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .dataarea .datalist .table-striped th {
            text-align:left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <%--我就是供应商列表呢--%>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a id="all" href="javascript:void">管理</a></li>
                <li><a href="AddSupplier.aspx" title="添加供应商">添加</a></li>
            </ul>
        </div>

        <div class="datalist">
            <%--搜索--%>
            <div class="searcharea">
                <ul>
                    <li><span>供应商名称：</span><span>
                        <input type="text" id="txtSupplierName" class="forminput form-control" /></span></li>
                    <li><span>用户名：</span><span>
                        <input type="text" id="txtUserName" class="forminput form-control" />
                    </span></li>
                    <li>
                        <span>状态：</span>
                        <abbr class="formselect">
                            <select name="ddlStatus" id="ddlStatus" class="iselect">
                                <option value="">请选择状态</option>
                                <option value="1" <%=(Status==1?"selected":"") %>>正常</option>
                                <option value="2" <%=(Status==2?"selected":"") %>>冻结</option>
                            </select>
                        </abbr>
                    </li>

                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                    <li id="clickTopDown" onclick="javascript:Post_ExportExcel()" style="cursor: pointer;">
                        <i class="glyphicon glyphicon-save c-666" aria-hidden="true"></i>导出数据</li>
                </ul>
            </div>
            <table class="table table-striped" cellspacing="0" border="0" style="border-collapse: collapse;">
                <thead>
                    <tr>
                        <th scope="col" width="20%">用户名</th>
                        <th scope="col" width="20%">供应商名称</th>
                        <th scope="col" width="10%">联系电话</th>
                        <th scope="col" width="10%">上架商品数</th>
                        <th scope="col" width="10%">订单数</th>
                        <th scope="col" width="10%">状态</th>
                        <th scope="col">操作</th>
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
                        <span class="Name">{{item.UserName}}</span>
                    </td>
                    <td>
                        <span class="colorC">{{item.SupplierName}}</span>
                    </td>
                    <td>
                        <span class="colorC">{{item.Tel}}</span>
                    </td>
                    <td align="center">
                        <span class="colorC">{{item.ProductNums}}</span>
                    </td>
                    <td align="center">
                        <span class="colorC">{{item.OrderNums}}</span>
                    </td>
                    <td align="center">
                        <span>{{item.StateName}}</span>
                    </td>
                    <td style="width: 200px; white-space: nowrap;">
                        <div class="operation">
                            <span><a href="SupplierDetails.aspx?supplierId={{item.SupplierId}}">查看</a></span>
                            <span><a href="EditSupplier.aspx?supplierId={{item.SupplierId}}">编辑</a></span>
                        </div>
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/Supplier/ashx/SupplierList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/Supplier/scripts/SupplierList.js?v=3.2" type="text/javascript"></script>
</asp:Content>
