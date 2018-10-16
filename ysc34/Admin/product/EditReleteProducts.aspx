<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditReleteProducts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditReleteProducts" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        html { background: #fff !important; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 20px 20px 60px 20px; width: 100% !important; }
        .btn-clear { float: right; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="Goodsgifts">

        <div class="left">
            <asp:Panel ID="Panel1" runat="server" Style="line-height: 35px;">
                <p style="width: 100%; float: left; margin: 10px 0;">
                    <input name="txtSearchText" type="text" value="" id="txtSearchText" class="forminput form-control" style="height: 32px; width: 136px; margin-right: 5px">
                    <Hi:ProductCategoriesDropDownList ClientIDMode="Static" ID="dropCategories" runat="server" CssClass="iselect" Width="145" NullToDisplay="请选择分类" />
                    <input name="hidFilterProductIds" id="hidFilterProductIds" type="hidden" value="<%=hasRelatedId %>" />
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" style="margin-left: 5px;" class="btn btn-primary">
                </p>
            </asp:Panel>
            <div class="content">
                <div class="youhuiproductlist">

                    <!--S DataShow-->
                    <div class="datalist">
                        <div id="datashow"></div>
                    </div>
                    <!--E DataShow-->
                </div>
                <div class="r">
                    <div>
                        <input type="button" name="btnAddSearch" value="全部添加" id="btnAddSearch" onclick="Post_AddSearch()" class="btn btn-warning" />
                    </div>
                    <!--S Pagination-->
                    <div class="pagination" id="showpager"></div>
                    <!--E Pagination-->
                </div>


                <!--S Data Template-->
                <script id="datatmpl" type="text/html">
                    {{each rows as item index}}
                    <table width="100%" border="0" cellspacing="0" class="conlisttd">
                        <tr>
                            <td width="14%" rowspan="2" class="img">{{if item.ThumbnailUrl40}}
                                    <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; width: 40px; height: 40px;">
                                {{else}}
                                    <img src="/utility/pics/none.gif" style="border-width: 0px; width: 40px; height: 40px;">
                                {{/if}}
                            </td>
                            <td height="27" colspan="5" class="br_none">
                                <span class="Name"><a class="text-overflow" href="/ProductDetails.aspx?ProductId={{item.ProductId}}" target="_blank">{{item.ProductName}}</a></span>
                            </td>
                        </tr>
                        <tr>
                            <td width="27%" height="28" valign="top"><span class="colorC">一口价：{{item.SalePrice.toFixed(2)}}</span></td>
                            <td width="19%" valign="top">库存：{{item.Stock}}</td>
                            <td width="15%" valign="top" class="pr20 text-right"><span class="submit_tianjia">
                                <a href="javascript:Post_Add({{item.ProductId}})">添加</a></span></td>
                        </tr>
                    </table>
                    {{/each}}
                </script>
                <!--E Data Template-->

            </div>
        </div>

        <div class="right">
            <p style="width: 100%; float: left; margin: 9px 0;">
                <span style="float: left; line-height: 33px; font-size: 14px; font-weight: bold;">相关商品</span>
                <a class="btn btn-warning btn-clear" href="javascript:Post_Clear()">清空列表</a>
            </p>
            <div class="content">
                <div class="youhuiproductlist">


                    <!--S DataShow-->
                    <div class="datalist">
                        <div id="datashow2"></div>
                    </div>
                    <!--E DataShow-->
                </div>
                <div class="r">
                    <div>&nbsp;</div>

                    <!--S Pagination-->
                    <div class="pagination" id="showpager2"></div>
                    <!--E Pagination-->
                </div>


                <!--S Data Template-->
                <script id="datatmpl2" type="text/html">
                    {{each rows as item index}}
                    <table width="100%" border="0" cellspacing="0" class="conlisttd">
                        <tr>
                            <td width="14%" rowspan="2" class="img">{{if item.ThumbnailUrl40}}
                                    <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; width: 40px; height: 40px;">
                                {{else}}
                                    <img src="/utility/pics/none.gif" style="border-width: 0px; width: 40px; height: 40px;">
                                {{/if}}
                            </td>
                            <td height="27" colspan="5" class="br_none">
                                <span class="Name"><a class="text-overflow" href="/ProductDetails.aspx?ProductId={{item.ProductId}}" target="_blank">{{item.ProductName}}</a></span>
                            </td>
                        </tr>
                        <tr>
                            <td width="27%" height="28" valign="top"><span class="colorC">一口价：{{item.SalePrice.toFixed(2)}}</span></td>
                            <td width="19%" valign="top">库存：{{item.Stock}}</td>
                            <td width="15%" valign="top" class="pr20 text-right"><span class="submit_tianjia">
                                <a href="javascript:Post_Delete({{item.ProductId}})">删除</a></span></td>
                        </tr>
                    </table>
                    {{/each}}
                </script>
                <!--E Data Template-->
            </div>
        </div>
    </div>

    <input type="hidden" name="hidCurProductId" id="hidCurProductId" value="<%=productId %>" />
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/product/ashx/EditReleteProducts.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/product/scripts/EditReleteProducts.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
