<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="ManageOrder.aspx.cs" Inherits="Hidistro.UI.Web.Depot.sales.ManageOrder" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="/Depot/Sales/order.helper.js?v=3.4" type="text/javascript"></script>
    <script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>
    <script type="text/javascript">
        function GetLogisticsInformation(orderId, dadaStatus) {
            DialogFrame('/AppDepot/OrderLogistics.aspx?OrderId=' + orderId, '查看物流', null, null, function () {
                //ReloadPageData();
            });
        }
        function Post_CancelSendGoods(orderId) {
            if (orderId == "") {
                alert("请选择要取消发货的订单");
                return;
            }
            DialogFrame('sales/CancelSendOrderGoods.aspx?OrderId=' + orderId, '取消订单发货', null, null, function () {
                ReloadPageData();
            });
        }
        var formtype = "";

        function toConfirmPayOrder(OrderId) {
            if (confirm("如果客户已经通过其他途径支付了订单款项，您可以使用此操作修改订单状态\n\n此操作成功完成以后，订单的当前状态将变为已付款状态，确认客户已付款？")) {

            }
        }

        //备注信息
        function RemarkOrder(OrderId, OrderDate, OrderTotal, managerMark, managerRemark) {
            arrytext = null;
            formtype = "remark";
            $("#lblOrderTotalForRemark").html(OrderTotal);
            $("#hidOrderId").val(OrderId);
            $("#spanOrderId").html(OrderId);
            $("#lblOrderDateForRemark").html(OrderDate);

            for (var i = 0; i <= 5; i++) {
                if (document.getElementById("orderRemarkImageForRemark_" + i).value == managerMark) {
                    setArryText("orderRemarkImageForRemark_" + i, "true");
                    $("#orderRemarkImageForRemark_" + i).attr("check", true);
                }
                else {
                    $("#" + i).attr("check", false);
                }
            }
            setArryText("txtRemark", managerRemark);
            DialogShow("修改备注", 'updateremark', 'RemarkOrder', 'btnRemark');
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
                var url = "BatchPrintSendOrderGoods.aspx?OrderIds=" + orderIds;
                window.open(url, "批量打印发货单", "width=700, top=0, left=0, toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=n o, status=no");
            }
        }

        //批量发货
        function batchSend() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            });
            viewBatchSendGoods(orderIds);
        }
        function viewBatchSendGoods(orderId) {
            if (orderId == "") {
                alert("请选要发货的订单");
                return;
            }
            DialogFrame("sales/BatchSendOrderGoods.aspx?OrderIds=" + orderId, "批量发货", null, null, function () {
                ReloadPageData();
            });
        }

        function viewSendGoods(orderId) {
            if (orderId == "") {
                alert("请选要发货的订单");
                return;
            }
            DialogFrame('sales/SendOrderGoods.aspx?OrderId=' + orderId, '订单发货', null, null, function () {
                ReloadPageData();
            });
        }

        function Setordergoods() {
            $("#btnOrderGoods").trigger("click");
        }
        function Setproductgoods() {
            $("#btnProductGoods").trigger("click");
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
                var url = "sales/BatchPrintData.aspx?OrderIds=" + orderIds;
                DialogFrame(url, "批量打印快递单", null, null);
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
                $parent_link.attr("href", "javascript:DialogFrame('sales/" + $parent_link.attr("href") + "','退款详细信息',null,null);");
            });
        });

        //验证提货码
        function checkTakeCode() {
            DialogShowOfCalback("验证提货码", "checktakecode", "divcheckcode", "btnCheckCode", function () {
                var takeCode = $.trim($("#txtTakeCode").val());
                if (takeCode.length == 0) {
                    alert("提货码不能为空");
                    $("#txtTakeCode").focus();
                } else {
                    $.ajax({
                        type: "Post",
                        url: "/API/DepotHandler.ashx",
                        data: "action=validateTakeCodeNew&code=" + takeCode,
                        success: function (result) {
                            if (result == "1") {
                                DialogFrame("sales/ConfirmTake.aspx?code=" + takeCode, "确认提货", 800, 600, function () { ReloadPageData(); });
                            } else if (result == "-1") {
                                alert("提货码不正确");
                            } else if (result == "-2") {
                                alert("该提货码已被使用，无法再次提货");
                            } else if (result == "0") {
                                alert("提货码不能为空");
                            } else if (result == "-3") {
                                alert("订单不属于本门店");
                            }
                        }
                    });
                }

            });
        }

        //跳转到确认提货页面
        function toConfirmTakeCode(el) {
            var orderId = $(el).attr("title");
            DialogFrame("sales/ConfirmTake.aspx?orderId=" + orderId, "确认提货", 800, 600, function () {
                ReloadPageData();
            });
        }
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">

        <div id="divcheckcode" style="display: none;">
            <table style="width: 400px; height: 100px; margin-top: 25px;">
                <tr>
                    <td style="text-align: right; width: 90px; font-size: 16px;"><span style="color: red;">*</span>提货码：</td>
                    <td>
                        <input type="text" id="txtTakeCode" class="forminput form-control" style="width: 90%; height: 40px; font-size: 24px; color: #0b76c1; font-weight: bold;" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center; height: 80px; display: none;">
                        <input type="button" value="验证提货码" class="btn btn-primary" id="btnCheckCode" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="title">
            <ul class="title-nav">
                <li id="anchors0">
                    <a href="javascript:void(0);" class="statusanchors" data-staus="0">所有订单</a></li>
                <li id="anchors2">
                    <a href="javascript:void(0);" class="statusanchors" data-staus="2">等待发货</a></li>
                <li id="anchors999">
                    <a href="javascript:void(0);" class="statusanchors" data-staus="999">待自提/消费</a></li>
                <li id="anchors3">
                    <a href="javascript:void(0);" class="statusanchors" data-staus="3">已发货</a></li>
                <li id="anchors5">
                    <a href="javascript:void(0);" class="statusanchors" data-staus="5">成功订单</a></li>
                <li id="anchors4">
                    <a href="javascript:void(0);" class="statusanchors" data-staus="4">已关闭</a></li>
            </ul>
            <i class="glyphicon glyphicon-question-sign" data-container="body" style="cursor: pointer" data-toggle="popover" data-placement="left" title="操作说明" data-content="此处的订单编号查询不受订单状态限制，历史订单列表只显示三个月前的订单"></i>
        </div>
    </div>
    <!--选项卡-->
    <div class="dataarea mainwidth">
        <!--搜索-->
        <style>
            .searcharea ul li { padding: 5px 0px; }
        </style>
        <div class="searcharea clearfix br_search">
            <ul>
                <li><span>订单编号：</span><span>
                    <input name="txtOrderId" type="text" id="txtOrderId" class="forminput form-control" clientidmode="Static" runat="server"/>
                    <input type="hidden" name="ordStatus" id="ordStatus" value="<%=OrderStatusID %>" />
                </span></li>
                <li><span>收货人：</span><span>
                    <input name="txtShopTo" type="text" id="txtShopTo" class="forminput form-control" clientidmode="Static" runat="server"/>
                </span></li>
                <li><span>下单时间：</span></li>
                <li>
                    <input name="orderStartDate" type="text" id="orderStartDate" class="forminput form-control" clientidmode="Static" runat="server" readonly="readonly" style="width: 160px; float: left;" />
                    <span class="Pg_1010">至</span>
                    <input name="orderEndDate" type="text" id="orderEndDate" class="forminput form-control" clientidmode="Static" runat="server" readonly="readonly" style="width: 160px; float: left;" />
                </li>
                <li>
                    <span>订单类型：</span>
                    <abbr class="formselect">
                        <Hi:OrderTypeDrowpDownList ClientIDMode="Static" runat="server" ID="ddlOrderType" CssClass="iselect"></Hi:OrderTypeDrowpDownList>
                    </abbr>
                </li>
                <li><span>提货码：</span><span>
                    <input name="txtCode" type="text" id="txtCode" class="forminput form-control" clientidmode="Static" runat="server"/>
                </span></li>
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
                <li>
                    <span>是否开具发票：</span><span>
                        <abbr class="formselect">
                            <select name="ddlIsOpenReceipt" id="ddlIsOpenReceipt" class="forminput form-control" style="width: 107px;">
                                <option value="">全部</option>
                                <option value="1" <%=(isTickit==1?"selected":"") %>>是</option>
                                <option value="0" <%=(isTickit==0?"selected":"") %>>否</option>
                            </select>
                        </abbr>
                    </span>
                    &nbsp;&nbsp;
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                </li>
            </ul>
        </div>
        <!--结束-->
        <div class="functionHandleArea clearfix m_none">
            <div class="blank8 clearfix">
            </div>
            <div class="batchHandleArea">
                <ul>
                    <li class="batchHandleButton"><span class="signicon"></span><span>
                        <a href="javascript:void(0)" class="btn btn-primary" onclick="SelectAll()"><i class="glyphicon glyphicon-ok" aria-hidden="true"></i>全选</a></span> <span>
                            <a href="javascript:void(0)" class="btn btn-primary" onclick=" ReverseSelect()"><i class="glyphicon glyphicon-fullscreen" aria-hidden="true"></i>反选</a></span>
                        <span><a class="btn btn-primary" href="javascript:printPosts()">批量打印快递单</a></span> <span><a class="btn btn-primary" href="javascript:printGoods()">批量打印发货单</a></span> <span>
                            <a href="javascript:downOrder()" class="btn btn-primary">下载配货单</a></span> <span><a class="btn btn-primary" href="javascript:batchSend()"
                                onclick="">批量发货</a></span>
                        <span><a class="btn btn-primary" href="javascript:ExportToExcel()"
                                onclick="">导出数据</a></span>
                        <span><a class="btn btn-warning" href="javascript:checkTakeCode()"
                            onclick="">验证提货码</a></span>
                    </li>
                </ul>
                <!--分页功能-->
                <div class="pageHandleArea">
                    <ul>
                        <li class="paginalNum"><span>每页显示数量：</span>                            
                        <select name="pagesize_dropdown" id="pagesize_dropdown">
                            <option value="10" <%=(Page_CurrentPageSize==10?"selected":"") %>>10</option>
                            <option value="20" <%=(Page_CurrentPageSize==20?"selected":"") %>>20</option>
                            <option value="40" <%=(Page_CurrentPageSize==40?"selected":"") %>>40</option>
                            <option value="200" <%=(Page_CurrentPageSize==200?"selected":"") %>>200</option>
                            <option value="500" <%=(Page_CurrentPageSize==500?"selected":"") %>>500</option>
                            <option value="1000" <%=(Page_CurrentPageSize==1000?"selected":"") %>>1000</option>
                            <option value="2000" <%=(Page_CurrentPageSize==2000?"selected":"") %>>2000</option>
                        </select>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <input name="hidOrderId" type="hidden" id="hidOrderId" />
        <div class="datalist clearfix">
            <table width="0" border="0" cellspacing="0">
                <tr class="table_title">
                    <td width="5%">选择</td>
                    <td class="td_right td_left">会员名
                    </td>
                    <td width="24%" class="td_right td_left">收货人
                    </td>
                    <td width="16%" class="td_right td_left">支付方式
                    </td>
                    <td width="14%" class="td_right td_left">订单实收款(元)
                    </td>
                    <td width="18%" class="td_right td_left">订单状态
                    </td>
                    <td width="18%" class="td_left td_right_fff">操作
                    </td>
                </tr>
                <tbody id="datashow"></tbody>
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
                <tr class="td_bg">
                    <td rowspan="2" align="left">
                        <input name="CheckBoxGroup" type="checkbox" value="{{item.OrderId}}" /></td>
                    <td>{{if item.IsError}}
                            <img src="/Supplier/images/orderError.png" width="17" height="17" title="{{item.ErrorMessage}}" />
                            {{/if}}
                        订单编号：{{item.PayOrderId}}
                        {{if item.SourceOrder==2}}
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
                        {{else}}
                        {{/if}}
                        {{if item.GroupBuyId && item.GroupBuyId>0}}(团){{/if}}
                        {{if item.CountDownBuyId && item.CountDownBuyId>0}}(抢){{/if}}
                        {{if item.PreSaleId && item.PreSaleId>0}}(预){{/if}}                        
                        {{if item.FightGroupId}}(拼){{/if}} 
                        {{if item.OrderType==6}}(服务){{/if}}
                    </td>
                    <td>提交时间：{{ item.OrderDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}
                    </td>
                    <td>{{if item.IsPrinted}}已打印{{else}}未打印{{/if}}</td>
                    <td></td>
                    <td>&nbsp;
                       {{if item.ExpressCompanyName=="同城物流配送"}}
                                <span style="color: red"><a href="/AppDepot/OrderLogistics?OrderId={{item.OrderId}}" target="_blank">达达物流:{{item.dadaState}}</a></span>
                                {{else if item.ShipOrderNumber}}
                                物流单号：<a href="javascript:void(0)" onclick="GetLogisticsInformation('{{item.OrderId}}','{{item.ShipOrderNumber}}','{{item.ExpressCompanyName}}')">
                                    {{item.ShipOrderNumber}}</a>
                                {{/if}}
                        {{if item.ShippingModeId=="-2"}}<font style="color: red;">上门自提</font>{{/if}}
                    </td>
                   <td>&nbsp;<a class="colorD" href="javascript:void(0)" onclick="ToDetail('{{item.OrderId}}');" data-toggle='tooltip' data-placement='top' title='查看详情'>{{item.InvoiceTypeText==""?"":item.InvoiceTypeText}}</a></td>
                    <td align="right">
                        <a style="float: right;" href="javascript:RemarkOrder('{{item.OrderId}}','{{item.OrderDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}','{{item.OrderTotal}}','{{item.ManagerMark}}','{{item.ManagerRemark}}');">
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
                    </td>
                </tr>

        <tr>
            <td>
                <a href="/depot/member/MemberDetails?userId={{item.UserId}}">{{item.Username}}</a>
                {{if item.Wangwang}}<a target="_blank" href="http://www.taobao.com/webww/ww.php?ver=3&touid={{item.Wangwang}}&siteid=cntaobao&status=1&charset=utf-8"><img border="0" src="http://amos.alicdn.com/realonline.aw?v=2&uid={{item.Wangwang}}&site=cntaobao&s=1&charset=utf-8" alt="点击这里给我发消息" /></a>{{/if}}
                {{if item.RealName}}
                      <br/>
                      <a href="/depot/member/MemberDetails?userId={{item.UserId}}">{{item.RealName}}</a>
                {{/if}}
            </td>
            <td>{{item.ShipTo}}
            </td>
            <td>{{item.PaymentType}}</td>
            <td>
                <span style="font-weight: bold; font-family: Arial;">{{item.OrderTotal.toFixed(2)}}</span>
            </td>
            <td>
                <span>{{item.OrderStatusText}}</span>
                <span class="Name"><a  href="javascript:void(0)" onclick="ToDetail('{{item.OrderId}}');">详情</a></span>
                {{if item.FightGrouping}}成团中{{/if}}
            </td>
            <td>
                <div class="Name">
                    {{if item.CanConfirmOrder}}
                                    <a href="javascript:Post_ConfirmOrder('{{item.OrderId}}');" class="btnConfirm" style="color: #0091EA;">确认订单</a>
                    {{/if}}
                                    {{if item.canCheckTake}}
                            <a href="javascript:void(0)" id="btnCheckTake" onclick="toConfirmTakeCode(this)" title="{{item.PayOrderId}}">确认提货</a>
                    {{/if}}
                                    {{if item.canOfflineReceipt && item.OrderType!=6}}
                            <div>
                                <a href="javascript:void(0)" onclick="Post_ConfirmPay('{{item.OrderId}}');" style="color: Red;">确认线下收款</a>
                            </div>
                    {{/if}}
                     {{if item.canCancelSendGoods}}
                            <div>
                                <a href="javascript:void(0)" onclick="Post_CancelSendGoods('{{item.OrderId}}');" style="color: Red;">取消发单</a>
                            </div>
                    {{/if}}
                    
                                    {{if item.canCloseOrder}}
                        <a href="javascript:void(0)" onclick="CloseOrder('{{item.OrderId}}');" title="{{item.PayOrderId}}">关闭订单</a>
                    {{/if}}
                                    {{if item.canSendGoods && item.OrderType!=6}}
                        <a class="btn btn-primary" style="color: white;" href="javascript:viewSendGoods('{{item.OrderId}}')" title="{{item.PayOrderId}}">发货</a>
                    {{/if}}
                                    {{if item.canFinishTrade}}
                        <a href="javascript:void(0)" onclick="return Post_FinishTrade('{{item.OrderId}}')">完成订单</a>
                    {{/if}}
                </div>
            </td>
        </tr>
        {{/each}}
        
    </script>
    <!--E Data Template-->

    <!--关闭订单--->
    <div id="closeOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <em>关闭交易?请您确认已经通知买家,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</em>
            </p>
            <p>
                <span class="frame-span frame-input110">关闭理由:</span>
                <Hi:CloseTranReasonDropDownList runat="server" ClientIDMode="Static" ID="ddlCloseReason" />
            </p>
        </div>
    </div>
    <!--编辑备注--->
    <style>
        .frame-content { margin-top: -20px; }
    </style>
    <div id="RemarkOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input100">订单号：</span><span id="spanOrderId"></span>
            </p>
            <p>
                <span class="frame-span frame-input100">提交时间：</span><span id="lblOrderDateForRemark"></span>
            </p>
            <p>
                <span class="frame-span frame-input100">订单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel
                    ID="lblOrderTotalForRemark" ClientIDMode="Static" runat="server" /></strong>
            </p>
            <span class="frame-span frame-input100">标志：<em>*</em></span><Hi:OrderRemarkImageRadioButtonList
                runat="server" ID="orderRemarkImageForRemark" ClientIDMode="Static" RepeatDirection="Horizontal" />
            <p>
                <span>备忘录：</span><asp:TextBox ID="txtRemark" CssClass="forminput form-control" ClientIDMode="Static" TextMode="MultiLine" runat="server"
                    Width="300" Height="50" />
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

    <div style="display: none">
        <input type="hidden" id="hidOrderTotal" runat="server" />
        <input type="hidden" id="hidExpressCompanyName" clientidmode="static" runat="server" />
        <input type="hidden" id="hidShipOrderNumber" clientidmode="Static" runat="server" />
        <input type="button" name="btnCloseOrder" value="关闭订单" id="btnCloseOrder" class="btn btn-primary" />
        <input type="button" name="btnRemark" value="编辑备注信息" id="btnRemark" class="btn btn-primary" />
        <asp:Button ID="btnOrderGoods" ClientIDMode="Static" runat="server" CssClass="btn btn-primary" Text="订单配货表" />&nbsp;
        <asp:Button runat="server" ID="btnProductGoods" ClientIDMode="Static" Text="商品配货表" CssClass="btn btn-primary" />
    </div>
    <!--查看物流-->
    <div id="ViewLogistics" style="display: none">
        <div class="frame-content" style="margin-top: -20px;">
            <h1>快递单物流信息</h1>

            <div id="expressInfo">正在加载中....</div>
        </div>
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/Depot/sales/ashx/ManageOrder.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Depot/sales/scripts/ManageOrder.js" type="text/javascript"></script>
</asp:Content>
