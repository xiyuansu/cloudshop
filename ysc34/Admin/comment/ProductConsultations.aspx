<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ProductConsultations" MasterPageFile="~/Admin/Admin.Master" CodeBehind="ProductConsultations.aspx.cs" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:void(0);" class="tab_status" data-filter="NoReply">未回复</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-filter="Replyed">已回复</a></li>
            </ul>
        </div>
    </div>
    <!--选项卡-->

    <div class="dataarea mainwidth">
        <!--搜索-->
        <div class="searcharea">
            <ul>
                <li><span>商品名称：</span> <span>
                    <input type="text" id="txtSearchText" class="forminput form-control" /></span> </li>
                <li>
                    <span>商品分类：</span>
                    <abbr class="formselect">
                        <Hi:ProductCategoriesDropDownList ClientIDMode="Static" ID="dropCategories" runat="server" CssClass="iselect" NullToDisplay="请选择分类" /></abbr>
                </li>
                <li><span>商家编码：</span> <span>
                    <input type="text" id="txtSKU" class="forminput form-control" /></span> </li>
                <li>
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                </li>
            </ul>
        </div>

        <!--数据列表区域-->
        <input type="hidden" id="hidType" value="NoReply" />
        <div class="datalist clearfix">
            <!--S DataShow-->

            <div class="p_replyed">
                <div class="p_replyed_title">
                    <span class="p_replyed_user">咨询用户</span>
                    <span class="p_replyed_por">咨询商品</span>
                    <span class="p_replyed_time">咨询时间</span>
                    <span class="p_replyed_operation">操作</span>
                </div>
                <div id="datashow"></div>
                <div class="blank12 clearfix"></div>
            </div>
            <!--E DataShow-->

            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}                       
                <div class="order_hover">

                    <div class="replyed_info_title">
                        <div class="replyed_info_use marleft10">
                            <a>{{item.UserName}}
                            </a>
                        </div>
                        <div class="replyed_info_pname">
                            <a class="text-ellipsis" href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">{{item.ProductName}}</a>
                        </div>
                        <div class="replyed_info_time">
                            {{item.ConsultationDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}
                        </div>
                        <div class="replyed_info_operation">
                            {{if item.Type==1}}
                                <a href="javascript:DialogFrame('comment/ReplyProductConsultations.aspx?callback=CloseDialogAndReloadData&ConsultationId={{item.ConsultationId}}','回复客户咨询',null,null,function(e){ReloadPageData()})">回复</a><b>|</b>
                            {{/if}}
                                <a href="javascript:void(0)" onclick="post_delete('{{item.ConsultationId}}');">删除</a>
                        </div>

                    </div>
                    <p class="replyed_info_text">
                        <font>咨询内容：</font>
                        {{item.ConsultationText}}{{item.Typez}}
                    </p>
                    {{if item.Type==2}}
                      <p class="replyed_info_text">
                          <font>回复内容：</font>

                          <span class="line">{{item.ReplyText}}</span>
                          <em class="colorD">{{item.ReplyDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}   </em>
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

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/comment/ashx/ProductConsultations.ashx" />
        <script src="/Utility/artTemplate.js" type="text/javascript"></script>
        <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
        <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
        <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
        <script src="/admin/comment/scripts/ProductConsultations.js" type="text/javascript"></script>
</asp:Content>

