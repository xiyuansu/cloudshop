<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Gifts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Gifts" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a>列表</a></li>
                <li><a href="addgift.aspx">添加</a></li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul>
                    <li>
                        <span>关键字：</span>
                        <span>
                            <input type="text" id="txtSearchText" class="forminput form-control float" /></span>
                    </li>
                    <li>
                        <span>
                            <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></span>
                    </li>
                    <li class="icheck-label-5-10">
                        <span>
                            <input type="checkbox" id="chkPromotion" clientidmode="static" runat="server" class="icheck pull-left" />
                            <label class="mb_0">参与促销赠送的礼品</label>
                        </span>
                    </li>
                </ul>
            </div>
            <div class="advanceSearchArea clearfix">
                <!--预留显示高级搜索项区域-->
            </div>
            <!--结束-->

            <div class="functionHandleArea m_none clearfix">
                <!--分页功能-->

                <!--结束-->
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton">
                            <span class="checkall">
                                <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                            <span class="btn btn-default ml0"><a href="javascript:bat_delete()">删除</a></span>
                        </li>
                    </ul>
                    <div class="pageHandleArea pull-right">
                        <ul>
                            <li class="paginalNum"><span>每页显示数量：</span>
                                <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th style="width: 50px;"></th>
                        <th>礼品图片</th>
                        <th width="300">礼品名称</th>
                        <th>成本价</th>
                        <th>市场参考价</th>
                        <th>兑换所需积分</th>
                        <th>参与促销</th>
                        <th>操作</th>
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
                        <input type="checkbox" name="CheckBoxGroup" value="{{item.GiftId}}" /></span></td>
                    <td><a href='../../GiftDetails.aspx?GiftId={{item.GiftId}}' target="_blank">
                        <img src="{{item.ThumbnailUrl40}}" /></a></td>
                    <td><a href='../../GiftDetails.aspx?GiftId={{item.GiftId}}' target="_blank"><span>{{item.Name}}</span></a></td>
                    <td>{{if item.CostPrice}}
                        {{item.CostPrice.toFixed(2)}}
                        {{else}}
                        0.00
                        {{/if}}
                    </td>
                    <td>{{if item.MarketPrice}}
                        {{item.MarketPrice.toFixed(2)}}
                        {{else}}
                        0.00
                        {{/if}}</td>
                    <td>{{ if item.NeedPoint == 0}}-{{ else}}{{item.NeedPoint}}{{/if}}</td>
                    <td>{{if item.IsPromotion}}
                        参与促销赠送
                        {{else}}
                        不参与促销赠送
                        {{/if}}
                    </td>
                    <td>
                        <span><a href='/admin/promotion/EditGift.aspx?giftId={{item.GiftId}}'>编辑</a></span>
                        <span><a href="javascript:Post_Delete('{{item.GiftId}}')">删除</a></span>
                    </td>

                </tr>
        {{/each}}
        
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/Gifts.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/Gifts.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
