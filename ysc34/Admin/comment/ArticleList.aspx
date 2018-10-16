<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ArticleList" CodeBehind="ArticleList.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="databody mainwidth">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="ArticleList.aspx">管理</a></li>
                <li><a href="AddArticle.aspx">添加</a></li>
            </ul>
        </div>
    </div>

    <!--选项卡-->

    <div class="dataarea mainwidth">
        <!--结束-->
        <div class="searcharea">
            <ul class="a_none_left">
                <li><span>关键字：</span>
                    <input type="text" id="txtKeywords" class="forminput form-control" />
                    <li>
                        <abbr class="formselect">
                            <Hi:ArticleCategoryDropDownList ID="dropArticleCategory" NullToDisplay="请选择分类" ClientIDMode="Static" runat="server" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li><span>选择时间：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarStartDataTime" ClientIDMode="Static"></Hi:CalendarPanel>

                        </span><span class="Pg_1010">至</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarEndDataTime" IsEndDate="true" ClientIDMode="Static"></Hi:CalendarPanel>

                        </span>
                        <span style="margin-left: 15px;">
                            <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                        </span>

                    </li>
            </ul>
        </div>

        <div class="functionHandleArea clearfix m_none">
            <div class="batchHandleArea">
                <ul>
                    <li class="batchHandleButton">
                        <span class="checkall">
                            <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                        <a href="javascript:bat_deletes()" class="btn btn-default ml0">删除</a>
                </ul>
                <div class="paginalNum">
                    <span>每页显示数量：</span>
                    <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                </div>
            </div>
        </div>


        <!--S warp-->
        <div class="dataarea mainwidth databody">
            <div class="datalist">
                <table class="table table-striped">
                    <thead>
                        <th width="50px;"></th>
                        <th width="12%">图片</th>
                        <th width="10%">文章分类</th>
                        <th>文章标题</th>
                        <th width="8%">点击量</th>
                        <th width="12%">立即发布</th>
                        <th width="12%">添加时间</th>
                        <th width="15%">操作</th>
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
        <!--E warp-->


    </div>
    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                          <tr>
                              <td><span class="icheck">
                                  <input name="CheckBoxGroup" type="checkbox" value='{{item.ArticleId}}' />
                              </span></td>
                              <td>
                                  <img src="{{item.IconUrls}}" class="Img100_30" style="border: none;"></td>
                              <td>{{item.Name}}</td>
                              <td><a href="../../ArticleDetails.aspx?ArticleId={{item.ArticleId}}" target="_blank">
                                {{item.Title}}    </a></td>
                              <td>{{item.Hits}}</td>
                              <td>
                           
                                      {{if item.IsRelease}}
                                 <a href="javascript:Release({{item.ArticleId}},{{item.IsRelease.toString()}})">
                                     <img alt='点击取消' src='../images/iconaf.gif' style="    margin-left: 5px;"  /></a>
                                      {{else}}
                                 <a href="javascript:Release({{item.ArticleId}},{{item.IsRelease.toString()}})">
                                     <img alt='点击发布' src='../images/ta.gif' /></a>
                                      {{/if}}
                           
                              </td>
                              <td>{{item.AddedDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                              <td class="operation">
                                  <div class="operation">
                                      <span><a href="../../admin/comment/EditArticle.aspx?ArticleId={{item.ArticleId}}" class="SmallCommonTextButton">编辑</a></span>
                                      <span>
                                          <a href="javascript:bat_delete({{item.ArticleId}})">删除</a></span>
                                      <span><a href="javascript:DialogFrame('comment/RelatedArticleProduct.aspx?ArticleId={{item.ArticleId}}','文章相关商品',null,null,function(e){ReloadPageData();});">相关商品</a></span>
                                  </div>
                              </td>
                          </tr>
        {{/each}}
       

    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/comment/ashx/ArticleList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/comment/scripts/ArticleList.js" type="text/javascript"></script>
</asp:Content>


