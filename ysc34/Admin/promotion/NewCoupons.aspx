<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="NewCoupons.aspx.cs" Inherits="Hidistro.UI.Web.Admin.NewCoupons" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddCoupon.aspx">添加</a></li>
            </ul>
        </div>

        <!-- 添加按钮-->

        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul class="a_none_left">
                    <li>
                        <span>优惠券名称：</span><span>
                            <input type="text" id="txtCouponName" class="forminput form-control" /></span>
                    </li>
                    <li>
                        <span>状态：</span>
                        <span>
                            <select id="ddlStatus" class="iselect">
                                <option value="">请选择状态</option>
                                <option value="0">未生效</option>
                                <option value="1">进行中</option>
                                <option value="2">已过期</option>
                            </select>
                        </span>
                    </li>
                    <li>
                        <span>领取方式：</span>
                        <span>
                            <select id="ddlGetType"  class="iselect">
                                <option value="">请选择领取方式</option>
                                <option value="0">主动领取</option>
                                <option value="1">指定发放</option>
                                <option value="2">积分兑换</option>
                            </select>
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>
            </div>
              <table cellspacing="0" border="0" class="table table-striped table-fixed">
             <thead>
                <tr>
                    <th>优惠券名称</th>
                    <th>面值（元）</th>
                    <th>使用条件</th>
                    <th>有效期</th>
                    <th>剩余数量</th>
                    <th>领取人/张</th>
                    <th>已使用</th>
                    <th width="210" class="operation">操作</th>
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
                    <td>{{item.CouponName}}</td>
                    <td>{{item.Price.toFixed(2)}}</td>
                    <td>
                        {{if item.OrderUseLimit>0}}
                        满{{item.OrderUseLimit.toFixed(2)}}
                        {{else}}
                        无限制
                        {{/if}}
                    </td>
                    <td>{{ item.StartTime | artex_formatdate:"yyyy-MM-dd"}}至{{ item.ClosingTime | artex_formatdate:"yyyy-MM-dd"}}</td>
                    <td>{{item.LastCount}}</td>
                    <td>{{item.UserCount}}/{{item.UserGetCount}}</td>
                    <td>{{item.UseCount}}</td>
                    <td>
                        {{if  item.IsCouponEnd}}
                        <span><a href='/admin/promotion/CouponLink.aspx?couponId={{item.CouponId}}'>链接</a></span>
                        {{/if}}
                        <span><a href='/admin/promotion/CouponDetails.aspx?couponId={{item.CouponId}}'>活动详情</a></span>
                        {{if  item.IsCouponEnd}}
                        <span><a href='/admin/promotion/EditCoupon.aspx?couponId={{item.CouponId}}'>编辑</a></span>
                        {{/if}}
                        {{if !item.IsCouponEnd}}
                        <span><a href="/admin/promotion/CouponView.aspx?couponId={{item.CouponId}}">查看</a></span>
                        {{/if}}
                        {{if !item.IsCouponEnd}}
                        <span><a href="javascript:Post_Delete('{{item.CouponId}}')">删除</a></span>
                        {{/if}}
                        {{if item.IsCouponEnd}}
                        <span><a href="javascript:Post_SetOver('{{item.CouponId}}')">使失效</a></span>
                        {{/if}}
                    </td>

                </tr>
                {{/each}}
 
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/NewCoupons.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/NewCoupons.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

</asp:Content>
