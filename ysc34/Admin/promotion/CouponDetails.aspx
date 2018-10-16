<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CouponDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CouponDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="newcoupons.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">领取详情</a></li>
            </ul>
        </div>

        <!-- 添加按钮-->
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul class="a_none_left">
                    <li>
                        <span>订单编号：</span><span>
                            <input type="text" id="txtOrderId" class="forminput form-control" value="<%=orderId %>" /></span>
                    </li>
                    <li>
                        <span>使用状态：</span>
                        <span>
                            <select id="ddlStatus" class="iselect">
                                <option value="">请选择使用状态</option>
                                <option value="1" <%=couponStatus==1?"selected":"" %>>未使用</option>
                                <option value="2" <%=couponStatus==2?"selected":"" %>>已使用</option>
                            </select>
                        </span>
                    </li>
                    <li>
                        <input type="hidden" name="hidCouponId" id="hidCouponId" value="<%=couponId %>" />
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>
            </div>
            <!--S DataShow-->

            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th width="280">优惠码</th>
                        <th>领取会员</th>
                        <th width="80">面值</th>
                        <th width="120">领取日期</th>
                        <th width="120">使用日期</th>
                        <th>订单编号</th>
                        <th>状态</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--E DataShow-->
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
                    <td>{{item.ClaimCode}}</td>
                    <td>{{item.UserName}}</td>
                    <td>{{item.Price.toFixed(2)}}</td>
                    <td>{{ item.GetDate | artex_formatdate:"yyyy-MM-dd"}}</td>
                    <td>{{ item.UsedTime | artex_formatdate:"yyyy-MM-dd"}}</td>
                    <td>{{item.OrderId}}</td>
                    <td>{{if item.OrderId && item.OrderId!=""}}
                        已使用
                        {{else}}
                        未使用
                        {{/if}}
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/CouponDetails.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/CouponDetails.js" type="text/javascript"></script>
</asp:Content>
