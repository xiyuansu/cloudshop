<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CountDowns.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CountDowns" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="hidOpenMultStore" Value="0" runat="server" ClientIDMode="Static" />
    <script type="text/javascript">
        $(function () {
            $("a[name=aDetails]").each(function () {
                var href = $(this).attr("href");
                href += "&returnUrl=" + encodeURIComponent(location.href);
                $(this).attr("href", href);
                
            });
            hidTitle();
        })
        function hidTitle() {
            if ($("#hidOpenMultStore").val() == "1") {
                $(".theadAllNum").hide();
            }
        }
    </script>


    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:void(0);" class="statusanchors" data-status="0">正在进行</a></li>
                <li><a href="javascript:void(0);" class="statusanchors" data-status="1">即将开始</a></li>
                <li><a href="javascript:void(0);" class="statusanchors" data-status="2">历史抢购</a></li>
                <li><a href="AddCountDown.aspx" id="aWhoAdd">添加</a></li>
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
                        <asp:HiddenField ClientIDMode="Static" ID="hidState" runat="server" />
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                </ul>
            </div>

            <div class="functionHandleArea clearfix">
                <div class="batchHandleArea">
                    <div class="checkall" id="divCheckAll" runat="server">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                    </div>
                    <span id="deleteSpan"><a class="btn btn-default" href="javascript:bat_delete()">删除</a></span>

                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>


                </div>
            </div>
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th width="5%"></th>
                        <th width="25%">商品名称</th>
                        <th width="14%">开始时间</th>
                        <th width="14%">结束时间</th>
                        <th width="10%">抢购价格</th>
                        <th width="8%" class="theadAllNum">抢购总量</th>
                        <th width="8%">已抢总量</th>
                        <th width="8%">排序</th>
                        <th width="8%">操作</th>
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
                    <td><span class="icheck">
                        <input type="checkbox" name="CheckBoxGroup" value="{{item.CountDownId}}" /></span></td>
                    <td>
                        <a class="c-666 text-ellipsis" href='../../CountDownProductsDetails.aspx?countDownId={{item.CountDownId}}' target="_blank">{{item.ProductName}}</a>
                    </td>
                    <td>{{ item.StartDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{ item.EndDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{if item.CountDownPrice}}
                        {{item.CountDownPrice.toFixed(2)}}
                        {{else}}
                        0.00
                        {{/if}}
                    </td>
                    <td class="theadAllNum">{{item.TotalCount}}</td>
                    <td>{{item.BoughtCount}}</td>
                    <td>
                        <input name="txtSequence" type="text" value="{{item.DisplaySequence}}" data-id="{{item.CountDownId}}" class="forminput form-control txtSequence" style="width: 50px;" />
                    </td>
                    <td class="operation">{{if  item.State!="1"}}
                        <span><a href='CountDownsDetails.aspx?countDownId={{item.CountDownId}}'>活动详情</a></span>
                        {{/if}}
                        {{if item.State=="1"}}
                        <span><a href="EditCountDown.aspx?CountDownId={{item.CountDownId}}">编辑</a></span>
                        {{/if}}
                        {{if item.State!="0"}}
                        <span><a href="javascript:Post_Delete('{{item.CountDownId}}')">删除</a></span>
                        {{/if}}
                        {{if  item.State=="0"}}
                        <span><a href="javascript:Post_SetOver('{{item.CountDownId}}')">提前结束</a></span>
                        <span><a href="EditCountDown.aspx?CountDownId={{item.CountDownId}}">编辑</a></span>
                        {{/if}}
                    </td>

                </tr>
        {{/each}}
        
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/CountDowns.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/CountDowns.js" type="text/javascript"></script>
</asp:Content>
