<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ProductReviews" CodeBehind="ProductReviews.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <link rel="stylesheet" href="/Utility/star/star-rating.css" rev="stylesheet" type="text/css">
    <link href="/Utility/hishopUpload/hishopUpload.css" rel="stylesheet" />
    <script src="/Utility/star/star-rating.js"></script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:void(0);" class="tab_status" data-filter="false">未回复</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-filter="true">已回复</a></li>
            </ul>
        </div>
    </div>
    <div class="dataarea mainwidth">
        <!--数据列表区域-->
        <div class="searcharea">
            <ul>
                <li><span>商品名称：</span><span>
                    <input type="text" id="txtSearchText" class="forminput form-control" />
                </span></li>
                <li><span>评论时间：</span>
                    <Hi:CalendarPanel ClientIDMode="Static" runat="server" ID="calendarStartDate"></Hi:CalendarPanel>
                    <span class="Pg_1010">至</span>
                    <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="calendarEndDate" IsEndDate="true"></Hi:CalendarPanel>
                </li>
                <li>
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />

                </li>
            </ul>
        </div>
        <div class="functionHandleArea clearfix m_none">

            <div class="batchHandleArea">

                <div class="batchHandleButton">
                    <span class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                    <a href="javascript:BatchReply();" class="btn btn-primary ml_10">批量回复</a>
                </div>
            </div>
        </div>
        <div class="datalist clearfix">
            <input type="hidden" id="hidType" value="false" />
            <!--S DataShow-->
            <div class="p_replyed">
                <div class="p_replyed_title">
                    <span class="p_reviews_user">评论用户</span>
                    <span class="p_reviews_por">评论商品</span>
                    <span class="p_reviews_score">评分</span>
                    <span class="p_reviews_time">评论时间</span>
                    <span class="p_reviews_operation">操作</span>
                </div>
                <div id="datashow">
                </div>

                <div class="blank12 clearfix"></div>
            </div>
            <!--E DataShow-->

            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}                       
                <div class="order_hover">
                    <div class="replyed_info_title">
                        <div class="reviews_info_use">
                              <span class="productreview_title_checkall">
                            <input type="checkbox" name="CheckBoxGroup" value="{{item.ReviewId}}" class="icheck" /></span>
                            <a class="text-decoration c-666" href="/Admin/member/MemberDetails?userId={{item.UserId}}">{{item.UserName}}
                            </a>
                        </div>

                        <div class="reviews_info_pname">
                            <a class="text-decoration c-666" title="{{item.ProductName}}" href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">{{item.ProductNameStr}}</a>
                        </div>
                        <div class="reviews_info_score">
                            &nbsp;<input id="input-2ba" type="number" class="rating" min="0" max="5" step="1" data-size="xs" data-stars="5" value="{{item.Score}}" data-symbol="&#xe005;" readonly="readonly">
                        </div>
                        <div class="reviews_info_time">
                            {{item.ReviewDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}  
                        </div>

                        <div class="reviews_info_operation">
                            {{if item.Type==true}}
                                <a href="javascript:DialogFrame('comment/ReplyProductReviews.aspx?ReviewId={{item.ReviewId}}','查看客户评论',null,null,function(e){ReloadPageData();})">查看</a>
                            {{else}}
                                <a href="javascript:DialogFrame('comment/ReplyProductReviews.aspx?callback=CloseDialogAndReloadData&ReviewId={{item.ReviewId}}','回复客户评论',null,null,function(e){ReloadPageData();})">回复</a>
                            {{/if}}
                           <b>|</b>
                            <a href="javascript:void(0)" onclick="post_delete('{{item.ReviewId}}');">删除</a>
                        </div>
                    </div>
                    <p class="comment_info_text">
                        <font>评论内容：</font>
                        <span class="ReviewText">{{item.ReviewTextStr}} {{item.Type}}
                        </span>
                    </p>
                    {{if item.Type==true}}
                       <p class="replyed_info_text">
                           <font>回复内容：</font>  <span class="ReplyText" style="word-break: break-word;">{{item.ReplyTextStr}}</span><span class="ReplyDate" style="float: right; color: #999; position: absolute; right: 15px; top: 5px;">{{item.ReplyDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}  
                           </span>
                       </p>
                    {{/if}}

                </div>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/comment/ashx/ProductReviews.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/comment/scripts/ProductReviews.js?v=3.40" type="text/javascript"></script>
</asp:Content>

