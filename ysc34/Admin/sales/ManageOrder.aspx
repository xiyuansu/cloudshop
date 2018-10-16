<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ManageOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageOrder" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Context" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="javascript:void(0);" class="tab_status" data-status="0">所有订单</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="1">待付款</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="999">待自提/待核销</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="2">等待发货</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="3">已发货</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="5">成功订单</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="4">已关闭</a></li>
                <li><a href="javascript:void(0);" class="tab_status" data-status="99">历史订单</a></li>
            </ul>
        </div>
    </div>
    <!--选项卡-->
    <div class="dataarea mainwidth">
        <!--搜索-->

        <div class="searcharea">
            <ul>
                <li><span>订单编号：</span><span>
                    <input type="text" id="txtOrderId" class="forminput form-control" clientidmode="Static" runat="server" />
                    <asp:HiddenField ID="hidStatus" ClientIDMode="Static" runat="server" />
                </span></li>
                <li><span>会员名称：</span><span>
                    <input type="text" id="txtUserName" class="forminput form-control" clientidmode="Static" runat="server" />
                </span></li>
                <li><span>选择时间：</span>
                    <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldStartDate"></Hi:CalendarPanel>
                    <span class="Pg_1010">至</span>
                    <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldEndDate" IsEndDate="true"></Hi:CalendarPanel>
                </li>
                <li>
                    <span>订单类型：</span>
                    <abbr class="formselect">
                        <Hi:OrderTypeDrowpDownList ClientIDMode="Static" runat="server" ID="ddlOrderType" CssClass="iselect"></Hi:OrderTypeDrowpDownList>
                    </abbr>
                </li>
                <li id="listore" runat="server">
                    <span>选择门店：</span>
                    <abbr class="formselect">
                        <Hi:StoreDropDownList ClientIDMode="Static" ID="ddlSearchStore" runat="server" CssClass="iselect"></Hi:StoreDropDownList>
                    </abbr>
                </li>
                <li>
                    <span>订单来源：</span>
                    <abbr class="formselect">
                        <Hi:SourceOrderDrowpDownList ClientIDMode="Static" runat="server" ID="dropsourceorder" NullToDisplay="请选择订单来源" CssClass="iselect"></Hi:SourceOrderDrowpDownList>
                    </abbr>
                </li>
                <li>
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    <a href="javascript:;" id="so_more">更多搜索条件&nbsp;<i class="glyphicon glyphicon-menu-down"></i></a>
                    <input type="text" value="" id="so_more_input" style="display: none;" runat="server" clientidmode="Static" />
                </li>
            </ul>
            <ul id="so_more_none" style="display: none;" runat="server" clientidmode="Static">
                <li><span>商品名称：</span><span>
                    <input type="text" id="txtProductName" class="forminput form-control" clientidmode="Static" runat="server" />
                </span></li>
                <li><span>收货人：</span><span>
                    <input type="text" id="txtShopTo" class="forminput form-control" clientidmode="Static" runat="server" />
                </span></li>
                <li><span>打印状态：</span><span>
                    <abbr class="formselect">
                        <select name="ddlIsPrinted" id="ddlIsPrinted" class="iselect">
                            <option value="">请选择打印状态</option>
                            <option value="1" <%=(IsPrinted==1?"selected":"") %>>已打印</option>
                            <option value="0" <%=(IsPrinted==0?"selected":"") %>>未打印</option>
                        </select>
                    </abbr>
                </span></li>
                <li id="liStoreFilter" runat="server"><span>门店配送：</span><span>
                    <abbr class="formselect">
                        <select name="ddlStoreDistribution" id="ddlStoreDistribution" class="iselect">
                            <option value="">请选择门店配送</option>
                            <option value="1" <%=(IsAllotStore==1?"selected":"") %>>未分配到门店订单</option>
                            <option value="2" <%=(IsAllotStore==2?"selected":"") %>>已分配到门店订单</option>
                        </select>
                    </abbr>
                </span>
                </li>
                <li><span>联系地址：</span>
                    <abbr class="formselect">
                        <Hi:RegionSelector runat="server" ClientIDMode="Static" ID="dropRegion" />
                    </abbr>
                </li>
                <li><span>发票类型：</span>
                    <abbr class="formselect">
                        <asp:DropDownList ID="dropInvoiceType" ClientIDMode="Static" CssClass="iselect" runat="server">
                            <asp:ListItem Value="">请选择发票类型</asp:ListItem>
                            <asp:ListItem Value="0">普通发票</asp:ListItem>
                            <asp:ListItem Value="2">电子发票</asp:ListItem>
                            <asp:ListItem Value="4">增值发票</asp:ListItem>
                        </asp:DropDownList>
                    </abbr>
                </li>
            </ul>
        </div>
        <!--结束-->
        <div class="functionHandleArea clearfix m_none">

            <div class="batchHandleArea">

                <div class="batchHandleButton">
                    <span class="checkall">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                    <div class="btn-group  btn-group-all" role="group" aria-label="...">
                        <a href="javascript:downOrder()" class="btn btn-default">下载配货单</a>
                        <div class="btn-group">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                批量打印 <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu">
                                <li><a href="javascript:printPosts()">批量打印快递单</a></li>
                                <li><a href="javascript:printGoods()">批量打印发货单</a></li>
                            </ul>
                        </div>
                        <div class="btn-group">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                更多操作 <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu">
                                <li><a href="javascript:batchSend()" onclick="">批量发货</a></li>
                                <li>
                                    <input type="button" name="btnExportExcel" value="导出数据" id="btnExportExcel" style="width: 100%; background: none; border: 0;" /></li>
                            </ul>
                        </div>

                    </div>
                    <a href="javascript:bat_delete();" class="btn btn-default ml20">删除</a>

                    <asp:HyperLink ID="hplinkprint" CssClass="btn btn-warning ml20" runat="server" Text="批量打印电子面单" Target="_blank"></asp:HyperLink>

                    <a href="javascript:verifications();" class="btn btn-primary ml_10">服务核销</a>
                </div>

                <!--分页功能-->
                <div class="paginalNum">
                    <span>每页显示数量：</span>
                    <select name="pagesize_dropdown" id="pagesize_dropdown">
                        <option value="10" <%=(Page_CurrentPageSize==10?"selected":"") %>>10</option>
                        <option value="20" <%=(Page_CurrentPageSize==20?"selected":"") %>>20</option>
                        <option value="40" <%=(Page_CurrentPageSize==40?"selected":"") %>>40</option>
                        <option value="200" <%=(Page_CurrentPageSize==200?"selected":"") %>>200</option>
                        <option value="500" <%=(Page_CurrentPageSize==500?"selected":"") %>>500</option>
                        <option value="1000" <%=(Page_CurrentPageSize==1000?"selected":"") %>>1000</option>
                        <option value="2000" <%=(Page_CurrentPageSize==2000?"selected":"") %>>2000</option>
                    </select>
                </div>
            </div>
        </div>
        <input type="hidden" id="hidOrderId" clientidmode="Static" runat="server" />
        <!--数据列表区域-->
        <div class="datalist clearfix">

            <!--S DataShow-->
            <div class="order">
                <div class="order_title">
                    <span class="order_title_checkall"></span>
                    <span class="order_title_usename">会员名</span>
                    <span class="order_title_pay">支付方式</span>
                    <span class="order_title_price">订单金额(元)</span>
                    <span class="order_title_state pd_0">订单状态</span>
                    <span class="order_title_store" style="display: none;">匹配门店</span>
                    <span class="order_title_operation">操作</span>
                </div>
                <div id="datashow"></div>
            </div>
            <!--E DataShow-->
            <div class="blank5 clearfix"></div>
        </div>

        <!--S Pagination-->
        <div class="flbotpage">
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" id="showpager"></div>
                    </div>
                </div>
            </div>
        </div>
        <!--E Pagination-->
    </div>
    <!--关闭订单--->
    <div id="closeOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <em>关闭交易?请您确认已经通知买家,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</em>
            </p>
            <p>
                <span class="frame-span frame-input110">关闭理由：</span>
                <Hi:CloseTranReasonDropDownList runat="server" ClientIDMode="Static" ID="ddlCloseReason" CssClass="forminput form-control" Width="200px" />
            </p>
        </div>
    </div>
    <!--编辑备注--->

    <div id="RemarkOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input100">订单编号：</span><span id="spanOrderId"></span>
            </p>
            <p>
                <span class="frame-span frame-input100">提交时间：</span><span id="lblOrderDateForRemark"></span>
            </p>
            <p>
                <span class="frame-span frame-input100">订单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel
                    ID="lblOrderTotalForRemark" runat="server" /></strong>
            </p>
            <span class="frame-span frame-input100"><em>*</em>标志：</span>
            <Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" />
            <p>
                <span class="frame-span frame-input100">备忘录：</span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server"
                    Width="300" Height="65" CssClass="forminput form-control" />
            </p>
        </div>
    </div>
    <div id="DownOrder" style="display: none;">
        <div class="frame-content" style="text-align: center;">
            <input type="button" id="btnorderph" onclick="javascript: Setordergoods();" class="btn btn-primary"
                value="订单配货表" />
            &nbsp;
            <input type="button" id="Button1" onclick="javascript: Setproductgoods();" class="btn btn-primary"
                value="商品配货表" />
            <p>
                导出内容只包括等待发货状态的订单
            </p>
            <p>
                订单配货表不会合并相同的商品,商品配货表则会合并相同的商品。
            </p>
        </div>
    </div>
    <div id="verifications_div" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input110">核销码：</span>
                <input type="text" id="txtCode" class="forminput form-control" />
            </p>
            <p>
                <span class="frame-span frame-input110">&nbsp;</span>
                <em id="codeErr"></em>
            </p>
        </div>
    </div>
    <div id="myTab_Content1" style="display: none">
        <div id="spExpressData">
            正在加载中....
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidOrderTotal" runat="server" />
        <input type="hidden" id="hidExpressCompanyName" clientidmode="static" runat="server" />
        <input type="hidden" id="hidShipOrderNumber" clientidmode="Static" runat="server" />
        <input type="button" name="btnCloseOrder" value="关闭订单" id="btnCloseOrder" class="btn btn-primary" />
        <asp:Button runat="server" ID="btnRemark" Text="编辑备注信息" CssClass="btn btn-primary" />
        <asp:Button ID="btnOrderGoods" runat="server" CssClass="btn btn-primary" Text="订单配货表" />&nbsp;
        <asp:Button runat="server" ID="btnProductGoods" Text="商品配货表" CssClass="btn btn-primary" />
        <input type="button" id="btnVerifications" value="验证核销" />
    </div>
    <!--查看物流-->
    <div id="ViewLogistics" style="display: none">
        <div class="frame-content">
            <h1>快递单物流信息</h1>
            <div id="expressInfo">
                正在加载中....
            </div>
        </div>
    </div>

    <style>
        .iselect_one { width: 100%; }
    </style>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                    <div class="order_hover">
                        <div class="order_info_title">
                            <span class="order_title_checkall">
                                <input type="checkbox" name="CheckBoxGroup" value="{{item.OrderId}}" class="icheck" /></span>
                            <span class="order_title_usename">{{if item.IsError}}
                            <img src="/Supplier/images/orderError.png" width="17" height="17" title="{{item.ErrorMessage}}" />
                                {{/if}}
                                订单号：<a class="colorBlue" href="javascript:void(0)" onclick="ToDetail('{{item.OrderId}}');" data-toggle='tooltip' data-placement='top' title='查看详情'>{{item.PayOrderId}}</a>
                                {{if item.SourceOrder==1}}
                                 <i class="iconfont" data-toggle="tooltip" data-placement="top" data-original-title="PC订单">&#xe606;</i>
                                {{else if item.SourceOrder==2}}
                        <i class="iconfont" data-toggle="tooltip" data-placement="top" data-original-title="淘宝订单">&#xe613;</i>
                                {{else if item.SourceOrder==3}}
                    <i class="iconfont" data-toggle="tooltip" data-placement="top" data-original-title="微信订单">&#xe614;</i>
                                {{else if item.SourceOrder==4}}
                    <i class="iconfont" data-toggle="tooltip" data-placement="top" data-original-title="WAP订单">&#xe605;</i>
                                {{else if item.SourceOrder==5}}
                    <i class="iconfont" data-toggle="tooltip" data-placement="top" data-original-title="生活号订单">&#xe60a;</i>
                                {{else if item.SourceOrder==6}}
                    <i class="iconfont" data-toggle="tooltip" data-placement="top" data-original-title="APP订单">&#xe600;</i>
                                {{else if item.SourceOrder==7}}
                    <i class="iconfont" data-toggle="tooltip" data-placement="top" data-original-title="京东订单">&#xe604;</i>
                                {{else if item.SourceOrder==8}}
                    <i class="iconfont" data-toggle="tooltip" data-placement="top" data-original-title="微信小程序">&#xe614;</i>
                                {{else}}
                        {{/if}}
                        {{if item.GroupBuyId && item.GroupBuyId>0}}(团){{/if}}
                        {{if item.CountDownBuyId && item.CountDownBuyId>0}}(抢){{/if}}
                        {{if item.PreSaleId && item.PreSaleId>0}}(预){{/if}}
                        {{if item.IsAwardOrder}}(奖){{/if}}
                        {{if item.IsGiftOrder}}(礼){{/if}}
                        {{if item.OrderType==6}}(服务){{/if}}
                        {{if item.FightGroupId&&item.FightGroupId>0}}<a href="/Admin/promotion/FightGroupDetails.aspx?fightGroupActivityId={{item.FightGroupActivityId}}">(拼)</a>{{/if}}
                            </span>
                            <span class="order_title_con_1">提交时间：{{ item.OrderDate | artex_formatdate:"yyyy-MM-dd"}}
                                {{if item.IsShowRefundIcon}}
                                <a href="{{item.RefundOperUrl}}"><i class="iconfont" style="color: red" data-toggle="tooltip" data-placement="top" title="" data-original-title="订单中有商品正在进行退货/退款"></i></a>
                                {{/if}}
                            </span>
                            <span class="order_title_price_1">{{if item.ExpressCompanyName=="同城物流配送"}}
                               <span style="color: red"><a href="/AppDepot/OrderLogistics?OrderId={{item.OrderId}}" target="_blank">达达物流:{{item.dadaState}}</a></span>
                                {{else if item.ShipOrderNumber}}
                                物流单号：<a href="javascript:void(0)" onclick="GetLogisticsInformation('{{item.OrderId}}','{{item.ShipOrderNumber}}','{{item.ExpressCompanyName}}')">
                                    {{item.ShipOrderNumber}}</a>
                                {{/if}}
                            </span>
                            <span class="order_title_store" style="color: #ff6600; display: none;">{{if item.ShippingModeId && item.ShippingModeId==-2}}(上门自提){{/if}}</span>
                            <span class="order_title_invoice" style="color: #0091ea;"><a class="colorD" href="javascript:void(0)" onclick="ToDetail('{{item.OrderId}}');" data-toggle='tooltip' data-placement='top' title='查看详情'>{{item.InvoiceTypeText==""?"":item.InvoiceTypeText}}</a></span>
                            <span class="order_title_operation">
                                <a href="javascript:RemarkOrder('{{item.OrderId}}');">
                                    <span class="icon iconfont" data-toggle="tooltip" data-placement="top" data-original-title='{{if item.ManagerRemark && item.ManagerRemark!=""}}{{item.ManagerRemark}}{{else}}备注{{/if}}' style="color: #999;">{{if item.ManagerRemark && item.ManagerRemark!=""}}
                                        {{if item.ManagerMark==1}}
                                        <img border="0" src="/Admin/images/iconaf.png" />
                                        {{else if item.ManagerMark==2}}
                                        <img border="0" src="/Admin/images/iconb.png" />
                                        {{else if item.ManagerMark==3}}
                                        <img border="0" src="/Admin/images/iconc.png" />
                                        {{else if item.ManagerMark==4}}
                                        <img border="0" src="/Admin/images/icona.png" />
                                        {{else if item.ManagerMark==5}}
                                        <img border="0" src="/Admin/images/iconad.png" />
                                        {{else if item.ManagerMark==6}}
                                        <img border="0" src="/Admin/images/iconae.png" />
                                        {{else}}
                                        <img border="0" src="/Admin/images/xi.gif" />
                                        {{/if}}
                                        {{else}}
                                        &#xe603;
                                        {{/if}}
                                    </span>

                                </a>
                            </span>
                        </div>
                        <div class="order_info">
                            <span style="width: 5%; float: left; height: 60px;"></span>
                            {{if item.RealName}}
                                <span class="order_title_usename" style="line-height: 18px; margin-top: 10px;">{{else}}
                                 <span class="order_title_usename">{{/if}}
                                <a href="/Admin/member/MemberDetails?userId={{item.UserId}}">{{item.Username}}</a>
                                     {{if item.Wangwang}}<a target="_blank" href="http://www.taobao.com/webww/ww.php?ver=3&touid={{item.Wangwang}}&siteid=cntaobao&status=1&charset=utf-8"><img border="0" src="http://amos.alicdn.com/realonline.aw?v=2&uid={{item.Wangwang}}&site=cntaobao&s=1&charset=utf-8" alt="点击这里给我发消息" /></a>{{/if}}
                                {{if item.RealName}}
                                  <br />
                                     <a href="javascript:void(0)" onclick="ToMemberDetail({{item.UserId}})">{{item.RealName}}</a>
                                     {{/if}}
                                 </span>

                                    <span class="order_title_pay">{{item.PaymentType}}
                                    </span>
                                    <div class="order_title_price">
                                        <label>{{if item.PreSaleId && item.PreSaleId>0 }}总价：{{/if}}</label>
                                        <span style="color: #666; font-weight: 700;">{{item.OrderTotal.toFixed(2)}}</span>
                                        {{if item.CanEditPrice}}
                                <a href="javascript:DialogFrame('/Admin/sales/EditOrder.aspx?OrderId={{item.OrderId}}','修改订单价格',null,null,function(e){ReloadPageData();})">
                                    <img src='/admin/images/icon_modify.png' data-toggle='tooltip' data-placement='top' title='修改价格' /></a>
                                        {{/if}}
                                {{if item.PreSaleId && item.PreSaleId>0 }}
                                <font>定金：{{item.Deposit.toFixed(2)}}</font>
                                        <font>尾款：{{item.FinalPayment.toFixed(2)}}</font>
                                        {{/if}}
                                    </div>
                                    <span class="order_title_state">
                                        <span>{{ item.OrderStatusText}}</span>
                                        {{if item.SendGoodsTips}}{{#item.SendGoodsTips}}{{/if}}
                                {{if item.FightGrouping}}成团中{{/if}}
                                    </span>
                                    <span class="order_title_store" style="display: none;">
                                        <span class="text-ellipsis" style="width: 140px;">{{if item.StoreId==-1}}
                                    多门店
                                    {{else if item.StoreId>0}}
                                    {{item.StoreName}}
                                    {{else}}
                                    平台
                                    {{/if}}
                                        </span>
                                    </span>
                                    <span class="order_title_operation">{{if item.canOfflineReceipt&&item.OrderType!=6}}
                                    <a href="javascript:void(0)" onclick="Post_ConfirmPay('{{item.OrderId}}');" style="color: #0091ea;">确认收款</a>
                                        {{/if}}
                                    {{if item.canCloseOrder}}
                                    <a href="javascript:CloseOrder('{{item.OrderId}}');" title="{{item.OrderId}}">关闭订单</a>
                                        {{/if}}
                                    {{if item.CanAllotStore&&item.OrderType!=6}}
                                    <a href="javascript:divideStore('{{item.OrderId}}');" title='{{item.OrderId}}'>{{if item.StoreId>0}}更改门店{{else}}分配门店{{/if}}</a>
                                        {{/if}}
                                    {{if item.canSendGoods&&item.OrderType!=6}}
                                    <a href="javascript:sendorderGoods('{{item.OrderId}}')" title="{{item.OrderId}}">发货</a>
                                        {{/if}}
                                    {{if item.IsOpenCertification}}
                                    <a href="javascript:RealNameCertification('{{item.OrderId}}')" title="{{item.OrderId}}">{{if item.IDStatus==1}}验证详情{{else}}实名验证{{/if}}</a>
                                        {{/if}}
                                          {{if item.canCancelSendGoods}}                           
                                    <a href="javascript:void(0)" onclick="Post_CancelSendGoods('{{item.OrderId}}');" style="color: Red;">取消发单</a>
                                        {{/if}}
                                    {{if item.canFinishTrade}}
                                    <a href="javascript:void(0)" onclick="return Post_FinishTrade('{{item.OrderId}}')">完成订单</a>
                                        {{/if}}     
                                    {{if item.IsCheckRefund}}                           
                                    <a style="color: Red"></a><a href="RefundApplyDetail?RefundId={{item.RefundId}}">处理退款</a>
                                        {{/if}}
                                    </span>
                        </div>
                    </div>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/sales/ashx/ManageOrder.ashx" />
    <input type="hidden" name="curpageindex" id="curpageindex" value="<%=Page_CurrentPageIndex %>" />
    <input type="hidden" name="isshowstore" id="isshowstore" value="<%=Page_IsShowStore?"1":"0" %>" />
    <asp:HiddenField ID="hidGroupId" ClientIDMode="Static" runat="server" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/sales/scripts/ManageOrder.js?v=3.4" type="text/javascript"></script>
    <script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript">
        $(function () {
            $('[data-toggle="tooltip"]').tooltip();
        })

        var formtype = "";

        function ConfirmPayOrder() {
            return confirm("如果客户已经通过其他途径支付了订单款项，您可以使用此操作修改订单状态\n\n此操作成功完成以后，订单的当前状态将变为已付款状态，确认客户已付款？");
        }

        //备注信息
        function RemarkOrder(OrderId, OrderDate, OrderTotal, managerMark, managerRemark) {
            DialogFrame('sales/OrderRemark.aspx?OrderId=' + OrderId, '修改订单备注', 550, 350, function (e) {
                ReloadPageData();
            });
        }

        function CloseOrder(orderId) {
            arrytext = null;
            formtype = "close";
            $("#hidOrderId").val(orderId);
            DialogShow("关闭订单", 'closeframe', 'closeOrder', 'btnCloseOrder');
        }

        function ValidationCloseReason() {
            var reason = document.getElementById("ddlCloseReason").value;
            if (reason == "请选择关闭的理由") {
                alert("请选择关闭的理由");
                return false;
            }
            setArryText("ddlCloseReason", reason);
            return true;
        }

        // 批量打印发货单
        function printGoods() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要打印的订单");
            }
            else {
                var url = "/Admin/sales/BatchPrintSendOrderGoods.aspx?callback=ShowMsgAndReloadData&OrderIds=" + orderIds;
                window.open(url, "批量打印发货单", "width=700, top=0, left=0, toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no");
            }
        }

        //批量发货
        function batchSend() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要发货的订单");
            }
            else {
                DialogFrame("sales/BatchSendOrderGoods.aspx?callback=ShowMsgAndReloadData&OrderIds=" + orderIds, "批量发货", 980, null, function () { ReloadPageData(); });
            }
        }
        function Setordergoods() {
            $("#ctl00_contentHolder_btnOrderGoods").trigger("click");
        }
        function Setproductgoods() {
            $("#ctl00_contentHolder_btnProductGoods").trigger("click");
        }
        //批量打印快递单
        function printPosts() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要打印的订单");
            }
            else {
                var url = "sales/BatchPrintData.aspx?callback=ShowMsgAndReloadData&OrderIds=" + orderIds;
                DialogFrame(url, "批量打印快递单", null, null, function (e) {
                    ReloadPageData();
                });
            }
        }

        //验证
        function validatorForm() {
            switch (formtype) {
                case "remark":
                    arrytext = null;
                    $radioId = $("input[type='radio'][name='ctl00$contentHolder$orderRemarkImageForRemark']:checked")[0];
                    if ($radioId == null || $radioId == "undefined") {
                        alert('请先标记备注');
                        return false;
                    }
                    setArryText($radioId.id, "true");
                    setArryText("ctl00_contentHolder_txtRemark", $("#ctl00_contentHolder_txtRemark").val());
                    break;
                case "shipptype":
                    return ValidationShippingMode();
                    break;
                case "close":
                    return ValidationCloseReason();
                    break;
                case "verifications":
                    if ($("#txtCode").val() == "") {
                        $("#codeErr").text("请输入核销码");
                        return false;
                    }
                    var ck = CheckVerification($("#txtCode").val());
                    if (ck) {
                        DialogFrame('sales/CheckVerification.aspx?Code=' + $("#txtCode").val(), '验证核销', 800, 500, function (e) {
                            ReloadPageData();
                        });
                    }
                    return ck;
                    break;
            };
            return true;
        }
        // 下载配货单
        function downOrder() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("请选要下载配货单的订单");
            }
            else {
                ShowMessageDialog("下载配货批次表", "downorder", "DownOrder");
            }
        }
        $(function () {
            $(".datalist img[src$='tui.gif']").each(function (item, i) {
                $parent_link = $(this).parent();
                $parent_link.attr("href", "javascript:DialogFrame('sales/" + $parent_link.attr("href") + "','退款详细信息',null,null,function(e){ReloadPageData();});");
            });
        });

        // 分配门店
        function divideStore(orderId) {
            DialogFrame("sales/ChangeOrderStore.aspx?orderId=" + orderId, "订单匹配门店", 550, 280, function () { ReloadPageData(); });
        }

        $(document).ready(function () {
            var oderlist = $('.order_title').offset().top - 58;
            $(window).scroll(function () {
                if ($(document).scrollTop() >= oderlist) {
                    $('.order_title').css({
                        position: 'fixed',
                        top: '0',
                        borderBottom: '1px solid #ccc',
                        boxShadow: '0 1px 3px #ccc',
                        width: '960px',
                    })
                }
                if ($(document).scrollTop() + $('.order_title').height() + 58 <= oderlist) {
                    $('.order_title').removeAttr('style');
                }
            });
            $("div.order_hover").each(function () {
                var fightGroupValue = $("[id$=hidFightGroup]", this).val();
                if (fightGroupValue == "1") {
                    $("[id$=hidFightGroup]", this).parent().html("成团中");
                }

            });
        });

        //发货
        function sendorderGoods(strorderId) {
            var str = location.href; //取得整个地址栏
            var num = str.indexOf("?");
            var paramstr = "";
            if (num > 0) {
                //取得所有参数   stringvar.substr(start [, length ]
                paramstr = str.substr(num + 1).toLowerCase().replace("orderid=", "searchorderid=");
            }

            var vsendhref = "/admin/sales/SendOrderGoods.aspx?callback=ShowMsgAndReloadData&OrderId=" + strorderId;
            if (paramstr != null && paramstr.length > 0) {
                vsendhref = vsendhref + "&" + paramstr;
            }
            DialogFrame(vsendhref, '订单发货', null, 580, function () { ReloadPageData(); });
        }

        //实名认证
        function RealNameCertification(strorderId) {
            var str = location.href; //取得整个地址栏
            var num = str.indexOf("?");
            var paramstr = "";
            if (num > 0) {
                //取得所有参数   stringvar.substr(start [, length ]
                paramstr = str.substr(num + 1).toLowerCase().replace("orderid=", "searchorderid=");
            }
            var vsendhref = "/admin/sales/RealNameCertification.aspx?OrderId=" + strorderId;
            if (paramstr != null && paramstr.length > 0) {
                vsendhref = vsendhref + "&" + paramstr;
            }
            DialogFrame(vsendhref, '实名认证', 640, 400, function () { location.reload(); });
        }

        //服务核销
        function verifications() {
            formtype = "verifications";
            $("#codeErr").text("");
            DialogShow("验证核销", 'verifications', 'verifications_div', 'btnVerifications');
        }

        function GetLogisticsInformation(orderId, ShipOrderNumber, ExpressCompanyName) {
            if (ExpressCompanyName != "同城物流配送") {
                hidHeadHtml = "<ul id='hidheard' class='buyer_information'><li><span>物流公司：</span>" + ExpressCompanyName + "</li>"
                           + "<li><span>物流单号：</span>" + ShipOrderNumber + "</li>";
                if (ExpressCompanyName.indexOf("顺丰") > -1) {
                    hidHeadHtml += "<li style='text-align:right'><a href=\"http://www.sf-express.com/cn/sc/dynamic_function/waybill/#search/bill-number/" + ShipOrderNumber + "\"  target='_blank'>顺丰官网查询></a></li></ul>";
                }
                else {
                    hidHeadHtml += "<li style='text-align:right'><a href=\"https://www.kuaidi100.com/chaxun?nu=" + ShipOrderNumber + "\"  target='_blank'>快递100查询></a></li></ul>";
                }
                $('#spExpressData').expressInfo(orderId, 'OrderId', hidHeadHtml);
                ShowMessageDialog("物流详情", "Exprass", "myTab_Content1");
            }
            else {

            }
        }
    </script>
</asp:Content>

