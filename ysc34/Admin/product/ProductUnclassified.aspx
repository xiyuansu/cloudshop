<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ProductUnclassified" CodeBehind="ProductUnclassified.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        samp.extend {
            width: auto;
            margin: 2px 4px 2px 0;
            line-height: 20px;
            border: 0px solid Blue;
            display: inline-block;
            position: relative;
            border: 1px solid #eee;
            background: #fff;
            padding: 1px 14px 1px 4px;
        }

        samp i {
            position: absolute;
            right: 0;
            top: 0;
            background: #eee url(../images/del.png) no-repeat center;
            background-size: 60%;
            width: 12px;
            height: 22px;
            display: block;
            cursor: pointer;
        }

        samp.nocontent {
            color: #999;
        }

        samp.extend1 {
            border: 1px solid #ff6a00;
            padding: 1px 14px 1px 4px;
        }

        samp.extend2 {
            border: 1px solid #ff0000;
            padding: 1px 14px 1px 4px;
        }

        samp.extend3 {
            border: 1px solid #b6ff00;
            padding: 1px 14px 1px 4px;
        }

        samp.extend4 {
            border: 1px solid #b200ff;
            padding: 1px 14px 1px 4px;
        }

        samp.extend5 {
            border: 1px solid #4800ff;
            padding: 1px 14px 1px 4px;
        }

        .iselect-100 .iselect {
            width: 100px;
        }

        .iselect-120 .iselect {
            width: 120px;
            margin-left: 10px;
        }

        #calendarEndDate {
            width: 160px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li><span>商品名称：</span><span>
                        <input type="text" id="txtSearchText" class="forminput form-control" />
                    </span></li>
                    <li><span>商家编码：</span><span><input type="text" id="txtSKU" class="forminput form-control" /></span></li>
                    <li>
                        <span>添加时间：</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarStartDate"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <Hi:CalendarPanel runat="server" ID="calendarEndDate" ClientIDMode="Static" IsEndDate="true"></Hi:CalendarPanel>
                    </li>
                    <li>
                        <span>商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ClientIDMode="Static" ID="dropCategories" IsUnclassified="true" NullToDisplay="请选择商品分类" runat="server" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>
                        <span>商品类型：</span>
                        <abbr class="formselect">
                            <Hi:ProductTypeDownList ID="dropType" ClientIDMode="Static" runat="server" NullToDisplay="请选择商品类型" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>
                        <span>商品品牌：</span>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList ClientIDMode="Static" runat="server" ID="dropBrandList" NullToDisplay="请选择品牌" CssClass="iselect"></Hi:BrandCategoriesDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>
            </div>

        </div>
        <div class="advanceSearchArea ">
            <!--预留显示高级搜索项区域-->
        </div>
        <!--结束-->

        <div class="functionHandleArea ">
            <div class="batchHandleArea">
                <div class="checkall" style="margin: 0;">
                    <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                </div>

                <a href="javascript:bat_delete()" class="btn btn-default ml_20">删除</a>
                <div class="paginalNum">
                    <span>每页显示数量：</span>
                    <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="txtCurrentExtendIndex" runat="server" ClientIDMode="Static" />
        <!--数据列表区域-->
        <!--S DataShow-->
        <div class="datalist">

            <table class="table table-striped">
                <thead>
                    <th style="width: 5%;"></th>
                    <th scope="col" style="width: 30%;">商品名称</th>
                    <th scope="col" style="width: 10%;">商家编码</th>
                    <th scope="col" style="width: 30%;">所属分类</th>
                    <th class="td_left td_right_fff" scope="col" style="width: 30%;">设置扩展分类</th>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>

            <div class="blank12 clearfix"></div>
        </div>
        <!--E DataShow-->
        <!--S Pagination-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
        <!--E Pagination-->

        <!--S Data Template-->
        <script id="datatmpl" type="text/html">
            {{each rows as item index}}
                <tr>

                    <td>

                        <span class="icheck">
                            <input name="CheckBoxGroup" type="checkbox" value="{{item.ProductId}}" />
                        </span>
                    </td>

                    <td>
                        <span class="Name">

                            <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; float: left; width:40px;">
                            <a class="pod-name-180 text-ellipsis" href='../../ProductDetails.aspx?productId={{item.ProductId}}' target="_blank">{{item.ProductName}}</a></span>

                    </td>
                    <td>
                        <span>{{item.ProductCode}}</span>
                    </td>

                    <td>
                        <span class="Name">
                            <div style="margin-bottom: 2px; overflow: hidden; text-overflow: ellipsis;">
                                <nobr><span style="font-size:13px;" >主分类：<abbr style=" color:#0091EA">{{item.CategoryStr}}</abbr></span></nobr>
                            </div>
                            <div>
                                <span>扩展分类：
                                
                                {{item.ExtendCategoryPathStr}}
                                {{item.ExtendCategoryPathStr1}}
                                {{item.ExtendCategoryPathStr2}}
                                {{item.ExtendCategoryPathStr3}}
                                {{item.ExtendCategoryPathStr4}}
                               
                                </span>
                            </div>

                        </span>
                    </td>
                    <td>
                        <div style="margin-bottom:10px;overflow:hidden">
                            <input type="hidden" class="hidSelectIndex" value="{{item.selectedExtIndex}}" />
                            <select name="drop_ExtendIndex" class="iselect dropExtendIndex" onchange="return checkIndex(this)" style="height: 30px;">
                                <option value="0" >请选择</option>
                                <option value="1">第一分类</option>
                                <option value="2">第二分类</option>
                                <option value="3">第三分类</option>
                                <option value="4">第四分类</option>
                                <option value="5">第五分类</option>
                            </select>
                        </div>
                        <div class="b_excate" data-pid="{{item.ProductId}}">

                        </div>
                    </td>



                </tr>
            {{/each}}
                   
        </script>
        <!--E Data Template-->
        <div class="blank12 clearfix"></div>
    </div>
    <div class=" br_search fl " style="width: 100%; margin: 30px 0 0 0; border-top: 1px solid #ddd; padding-top: 20px; border-bottom: 0;">
        <div style="float: left; width: 400px;">
            <ul style="width: 100%">
                <li>
                    <span class="formselect">
                        <Hi:ProductCategoriesDropDownList ID="dropMoveToCategories" ClientIDMode="Static" runat="server" CssClass="iselect_one" NullToDisplay="移动商品到分类" />
                    </span>

                    <a href="javascript:MoveCategory()" class="btn btn-primary  ml_10">转移主类</a>

                    <p class="mt_10 ">转移主类或者将商品转移到未分类，在转移以前请先选择要转移的商品。</p>
                </li>
                <li></li>
            </ul>


        </div>

        <div style="float: right; width: 500px;">
            <ul style="width: 100%">
                <li>
                    <span class="formselect " style="float: left; margin-right: 10px;">


                        <select class="iselect_one fl m" id="drop_ExtendIndex">
                            <option value="0">请选择</option>
                            <option value="1">第一分类</option>
                            <option value="2">第二分类</option>
                            <option value="3">第三分类</option>
                            <option value="4">第四分类</option>
                            <option value="5">第五分类</option>


                        </select>
                    </span>

                    <Hi:ProductCategoriesDropDownList ClientIDMode="Static" ID="dropAddToAllCategories" runat="server" CssClass="iselect_one  fl" NullToDisplay="选择分类" />

                    <a href="javascript:bat_AddToCategorie()" class="btn btn-primary  ml_10">设置扩展</a>
                    <p class="mt_10 ">批量设置商品的扩展分类，在设置以前请先选择要设置扩展分类的商品。</p>

                </li>

            </ul>

        </div>
    </div>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/product/ashx/ProductUnclassified.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/product/scripts/ProductUnclassified.js" type="text/javascript"></script>
</asp:Content>

