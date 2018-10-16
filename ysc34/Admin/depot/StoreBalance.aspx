<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="StoreBalance.aspx.cs" Inherits="Hidistro.UI.Web.Admin.StoreBalance" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea clearfix">
                <blockquote class="blockquote-default blockquote-tip clearfix">
                    说明：<br />
                    1、若门店订单采用线上付款或到店支付采用支付宝微信收银<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;应结金额 = 订单实付+积分抵扣部分+优惠券抵扣部分-税金-退款部分 - （订单实付+积分抵扣部分+优惠券抵扣部分-运费-退款部分-退款优惠券返还-税金）*门店抽成佣金比例
                    <br />
                    其中： 退款优惠券返还=[退款金额/(订单实付-税费-运费）]*优惠劵金额
                    <br />
                    2、若门店订单采用到店现金支付<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;应结金额 = 优惠券抵扣部分+积分抵扣部分-税金- （订单实付+积分抵扣部分+优惠券抵扣部分-退款部分-退款优惠券返还-运费-税金）*门店抽成佣金比例
                    <br />
                    其中： 退款优惠券返还=[退款金额/(订单实付-税费-运费）]*优惠劵金额
                </blockquote>

                <ul>
                    <li>
                        <Hi:CalendarPanel runat="server" ID="startDate" Width="157" ClientIDMode="Static"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <Hi:CalendarPanel runat="server" ID="endDate" Width="157" IsEndDate="true" ClientIDMode="Static"></Hi:CalendarPanel>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:StoreDropDownList ID="ddlStores" runat="server" CssClass="iselect" ClientIDMode="Static"></Hi:StoreDropDownList>
                        </abbr>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                    <li>

                        <a href="javascript:ExportToExcel()">导出数据</a>
                    </li>
                </ul>
                <ul>
                    <li>
                        <span>【结算总额：￥</span><span id="lblOrderSummaryTotal" class="f-bold">0.00</span>】
                    </li>
                </ul>
            </div>
            <div class="dataarea mainwidth databody">
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th width="12%">门店</th>
                                <th width="12%">结算时间</th>
                                <th width="12%">订单编号</th>
                                <th width="8%">订单实付</th>
                                <th width="8%">退款金额</th>
                                <th width="8%">平台佣金</th>
                                <th width="10%">结算金额</th>
                                <th width="5%">收款方</th>                                
                                <th width="8%">操作</th>
                            </tr>
                        </thead>
                        <tbody id="datashow"></tbody>
                    </table>

                    <div class="blank12 clearfix"></div>
                </div>
            </div>
            <!--E DataShow-->

            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                          <tr>
                              <td>{{item.StoreName}}</td>
                              <td>{{item.CreateTime}}</td>
                              <td>{{if item.TradeType==2}}
                                    <a href="/admin/sales/OrderDetails.aspx?OrderId={{item.TradeNo}}">{{item.TradeNo}}</a>
                                  {{else}}
                                     线下订单
                                  {{/if}}
                              </td>
                              <td>{{item.OrderTotal.toFixed(2)}}</td>
                              <td>{{ if item.RefundAmount== 0}}
                              --
                              {{else}}
                              {{item.RefundAmount}}
                                  {{/if}}
                              </td>
                              <td>{{item.PlatCommission.toFixed(2)}}</td>                            
                              <td>{{item.Income.toFixed(2)}}</td>
                                <td>{{if item.CollectByStore==1}}
                                  门店
                                  {{else}}
                                  平台
                                  {{/if}}
                              </td>
                              <td>
                                  <span class="submit_bianji"><a href="javascript:OpenDetail({{item.TradeType}},{{item.JournalNumber}})">查看详情</a></span>
                              </td>
                          </tr>
                {{/each}}
            </script>
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" id="showpager"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="button" id="btnId" />
    </div>
    <div id="divDetail" style="display: none" class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <div class="dataarea mainwidth databody">
                <div class="datalist" id="balanceDetail">
                   
                </div>
            </div>
        </div>
         <script id="balanceDetailTmpl" type="text/html">
              <ul>
                        <li>订单号:{{OrderId}}</li>
                        <li>订单创建时间:{{OrderDate}}</li>
                        <li>结算时间:{{OverBalanceDate}}</li>
                        <li>订单实付金额:{{OrderTotal}}</li>
                        <li>运费:{{Freight}}</li>
                        <li>积分抵扣 :{{DeductionMoney}}</li>
                        <li>优惠券抵扣 :{{CouponValue}}</li>
                        <li>退款金额 :{{RefundAmount}}</li>
                        <li>平台佣金 :{{PlatCommission.toFixed(2)}}</li>
                        <li>税金(元) :{{Tax.toFixed(2)}}</li>
                    </ul>
             </script> 
        <script id="balanceDetail2Tmpl" type="text/html">
              <ul>
                        <li>订单支付时间:{{PayTime}}</li>
                        <li>结算时间:{{OverBalanceDate}}</li>
                        <li>支付方式:{{PaymentTypeName}}</li>
                        <li>订单金额:{{OrderTotal}}</li>
                    </ul>
             </script>
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/StoreBalance.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/StoreBalance.js" type="text/javascript"></script>
    <script>
        function OpenDetail(type, balaneId) {
            if (type == 2) {
                getDetailInfo(balaneId);
            }
            else {
                getDetailInfo2(balaneId);
            }
        }
        function getDetailInfo(Id) {
            $.ajax({
                url: "ashx/StoreBalance.ashx",
                data: { action: "GetBalanceOnLineDetailById", id: Id},
                dataType: "json",
                success: function (data) {
                    var databox = $("#balanceDetail");
                    if (data) {
                        if (data) {
                            databox.html(template("balanceDetailTmpl", data));
                            var dlg = art.dialog({
                                id: "balanceDetail",
                                title: "结算详情",
                                content: $("#divDetail").html(),
                                resize: true,
                                fixed: true,
                                button: [ {
                                    name: "确定"
                                }]
                            });
                        } else {
                            databox.append("无数据");
                        }
                    }
                },
                error: function () {
                    ShowMsg(actionName + "获取数据异常", false);
                }
            });
        }
        function getDetailInfo2(Id) {
            $.ajax({
                url: "ashx/StoreBalance.ashx",
                data: { action: "GetBalanceOffLineDetailById", id: Id },
                dataType: "json",
                success: function (data) {
                    var databox = $("#balanceDetail");
                    if (data) {
                        if (data) {
                            databox.html(template("balanceDetail2Tmpl", data));
                            var dlg = art.dialog({
                                id: "balanceDetail2",
                                title: "线下订单结算详情",
                                content: $("#divDetail").html(),
                                resize: true,
                                fixed: true,
                                button: [{
                                    name: "确定"
                                }]
                            });
                        } else {
                            databox.append("无数据");
                        }
                    }
                },
                error: function () {
                    ShowMsg(actionName + "获取数据异常", false);
                }
            });
        }
    </script>
</asp:Content>
