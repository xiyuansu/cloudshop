<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true"
    CodeBehind="ManageOrder.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.sales.ManageOrder" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li id="anchors0" class="statusanchors" data-staus="0"><a href="javascript:void(0);">所有订单</a></li>
                <li id="anchors1" class="statusanchors" data-staus="1"><a href="javascript:void(0);">等待买家付款</a></li>
                <li id="anchors2" class="statusanchors" data-staus="2"><a href="javascript:void(0);">等待发货</a></li>
                <li id="anchors3" class="statusanchors" data-staus="3"><a href="javascript:void(0);">已发货</a></li>
                <li id="anchors5" class="statusanchors" data-staus="5"><a href="javascript:void(0);">成功订单</a></li>
                <li id="anchors4" class="statusanchors" data-staus="4"><a href="javascript:void(0);">已关闭</a></li>
                <li id="anchors99" class="statusanchors" data-staus="99"><a href="javascript:void(0);">历史订单</a></li>
            </ul>
        </div>
        <%--选项卡--%>

        <div class="dataarea mainwidth">
            <%--搜索--%>
            <div class="searcharea">
                <ul>
                    <li><span>订单编号：</span><span>
                        <input name="txtOrderId" type="text" id="txtOrderId" class="forminput form-control" clientidmode="Static" runat="server"/>
                        <input type="hidden" name="ordStatus" id="ordStatus" clientidmode="Static" runat="server"/>
                    </span></li>
                    <li><span>商品名称：</span><span>
                        <input name="txtProductName" type="text" id="txtProductName" class="forminput form-control" clientidmode="Static" runat="server"/>
                    </span></li>
                    <li><span>选择时间：</span>
                        <input name="orderStartDate" type="text" id="orderStartDate" class="forminput form-control" clientidmode="Static" runat="server" readonly="readonly" style="width: 160px; float: left;" />
                        <span class="Pg_1010">至</span>
                        <input name="orderEndDate" type="text" id="orderEndDate" class="forminput form-control" clientidmode="Static" runat="server" readonly="readonly" style="width: 160px; float: left;" />
                    </li>
                    <li><span>&nbsp;收&nbsp;货&nbsp;人：</span><span>
                        <input name="txtShopTo" type="text" id="txtShopTo" class="forminput form-control" clientidmode="Static" runat="server"/>
                    </span></li>
                    <li><span>收货地区：</span><span>
                        <Hi:RegionSelector runat="server" ClientIDMode="Static" ID="dropRegion" />
                    </span></li>

                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>
            </div>
            <%--结束--%>

            <div class="functionHandleArea clearfix m_none">

                <div class="batchHandleArea">

                    <div class="batchHandleButton">
                        <span class="checkall">
                            <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                        <div class="btn-group  btn-group-all" role="group" aria-label="...">
                            <a href="javascript:downOrder()" class="btn btn-default">下载配货单</a>
                            <div class="btn-group">
                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    批量打印 <font class="caret"></font>
                                </button>
                                <ul class="dropdown-menu">
                                    <li><a href="javascript:printPosts()">批量打印快递单</a></li>
                                    <li><a href="javascript:printGoods()">批量打印发货单</a></li>
                                </ul>
                            </div>
                            <div class="btn-group">
                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    更多操作 <font class="caret"></font>
                                </button>
                                <ul class="dropdown-menu">
                                    <li><a href="javascript:batchSend()" onclick="">批量发货</a></li>
                                    <li>
                                        <input type="button" name="btnExportExcel" value="导出数据" id="btnExportExcel" style="width: 100%; background: none; border: 0; padding: 3px 20px;" /></li>
                                </ul>
                            </div>

                        </div>
                    </div>

                    <%--分页功能--%>
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
            <input type="hidden" id="hidOrderId" runat="server" />
            <%--数据列表区域--%>
            <div class="datalist clearfix">
                

                <div class="order">
                    <div class="order_title">
                        <span class="order_title_checkall"></span>
                        <span class="order_title_usename">会员名</span>
                        <span class="order_title_pay">支付方式</span>
                        <span class="order_title_price">结算价(元)</span>
                        <span class="order_title_state pd_0">订单状态</span>
                        <span class="order_title_operation">操作</span>
                    </div>
                    <div class="datashow" id="datashow"></div>
                </div>
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

            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <div class="order_hover">
                    <div class="order_info_title">
                        <span class="order_title_checkall">
                            <input name="CheckBoxGroup" type="checkbox" value='{{item.OrderId}}' class="icheck" /></span>

                        <span class="order_title_usename">{{if item.IsError}}
                            <img src="/Supplier/images/orderError.png" width="17" height="17" title="{{item.ErrorMessage}}" />
                            {{/if}}
                            订单号：<a class="colorBlue"  href="javascript:void(0)" onclick="ToDetail('{{item.OrderId}}');" data-toggle='tooltip' data-placement='top' title='查看详情'>{{item.PayOrderId}}</a>
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
                        {{if item.isGiftOrder}}(礼){{/if}}
                        {{if item.FightGroupId}}<a href="/Supplier/vshop/FightGroupDetails.aspx?fightGroupActivityId={{item.FightGroupActivityId}}">(拼)</a>{{/if}}
                        </span>
                        <span class="order_title_con_1">提交时间：{{ item.OrderDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}
                            {{if item.isShowRefund}}
                            <a {{if item.AfterSaleType}}
                            href="{{if item.AfterSaleType==1}}ReturnApplyDetail?ReturnId={{item.ReturnId}}{{else}}ReplaceApplyDetail?ReplaceId={{item.ReplaceId}}{{/if}}"
                            {{else}}{{/if}}>
                            <i class="iconfont" style="color: red" data-toggle="tooltip" data-placement="top" title="" data-original-title="{{item.RefundStatus}}">&#xe612;</i></a>
                            {{/if}}
                        </span>
                        <span class="order_title_price_1">{{if item.ShipOrderNumber}}物流单号：<a href="javascript:void(0)" onclick="GetLogisticsInformation('{{item.OrderId}}','{{item.ShipOrderNumber}}','{{item.ExpressCompanyName}}')">{{item.ShipOrderNumber}}</a>{{/if}}</span>
                        <span class="order_title_store" style="color: #ff6600; <%= StoreShowStyle%>">{{if item.ShippingModeId=="-2"}}上门自提{{/if}}
                        </span>
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
                                <span class="order_title_usename" style="line-height: 18px;margin-top: 10px;">
                         {{else}}
                                 <span class="order_title_usename">
                         {{/if}}
                         {{item.Username}}
                         {{if item.Wangwang}}<a target="_blank" href="http://amos1.taobao.com/msg.ww?v=2&uid={{item.Wangwang}}&s=1"><img border="0" src="http://amos1.taobao.com/online.ww?v=2&uid={{item.Wangwang}}&s=1" alt="点击这里给我发消息" /></a>{{/if}}
                         {{if item.RealName}}<br/>{{item.RealName}}{{/if}}

                        </span>
                        <span class="order_title_pay">{{item.PaymentType}}
                        </span>
                        <div class="order_title_price">
                            <span style="color: #666; font-weight: 700;">{{item.SupplierOrderTotals.toFixed(2)}}</span>
                        </div>
                        <span class="order_title_state">
                            <span>{{item.OrderStatusText}}</span>
                            {{if item.FightGrouping}}成团中{{/if}}
                        </span>
                        <span class="order_title_operation">
                            {{if item.canSendGoods}}<span class="Name">
                        <a href="javascript:viewSendGoods('{{item.OrderId}}')" title="{{item.PayOrderId}}">发货</a>
                                </span>
                            {{/if}}
                            </span>
                    </div>
                </div>
                {{/each}}       
            </script>
            <!--E Data Template-->

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
            <div style="display: none">
                <input type="hidden" id="hidOrderTotal" runat="server" />
                <input type="hidden" id="hidExpressCompanyName" clientidmode="static" runat="server" />
                <input type="hidden" id="hidShipOrderNumber" clientidmode="Static" runat="server" />
                <asp:Button ID="btnOrderGoods" runat="server" CssClass="btn btn-primary" Text="订单配货表" />&nbsp;
        <asp:Button runat="server" ID="btnProductGoods" Text="商品配货表" CssClass="btn btn-primary" />

            </div>

            <%--查看物流--%>
            <div id="ViewLogistics" style="display: none">
                <div class="frame-content">
                    <h1>快递单物流信息</h1>

                    <div id="expressInfo">正在加载中....</div>
                </div>
            </div>
             <div id="myTab_Content1" style="display: none">
            <div id="spExpressData">
                正在加载中....
            </div>
        </div>

            <input type="hidden" name="dataurl" id="dataurl" value="/Supplier/sales/ashx/ManageOrder.ashx" />
            <script src="/Utility/artTemplate.js" type="text/javascript"></script>
            <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
            <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
            <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
            <script src="/Supplier/sales/scripts/ManageOrder.js" type="text/javascript"></script>
            <script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })

        //备注信息
        function RemarkOrder(OrderId, OrderDate, OrderTotal, managerMark, managerRemark) {
            DialogFrame('sales/OrderRemark.aspx?callback=SuccessAndCloseReload&OrderId=' + OrderId, '修改订单备注', 550, 350, function (e) {
                ReloadPageData();
            });
        }

        var formtype = "";

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
                var url = "/Supplier/sales/BatchPrintSendOrderGoods.aspx?OrderIds=" + orderIds;
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
                DialogFrame("sales/BatchSendOrderGoods.aspx?OrderIds=" + orderIds, "批量发货", 980, null, function () {
                    ReloadPageData();
                });
            }
        }

        function viewSendGoods(orderId) {
            if (orderId == "") {
                alert("请选要发货的订单");
                return;
            }
            DialogFrame('sales/SendOrderGoods.aspx?OrderId=' + orderId, '订单发货', null, 580, function () {
                ReloadPageData();
            });
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
                var url = "sales/BatchPrintData.aspx?OrderIds=" + orderIds;
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
        //查看物流信息
        function GetLogisticsInformation(orderId, ShipOrderNumber, ExpressCompanyName) {
            hidHeadHtml = "<ul id='hidheard' class='buyer_information'><li><span>物流公司：</span>" + ExpressCompanyName + "</li>"
                       + "<li><span>物流单号：</span>" + ShipOrderNumber + "</li>"
            if (ExpressCompanyName.indexOf("顺丰") > -1) {
                hidHeadHtml += "<li style='text-align:right'><a href=\"http://www.sf-express.com/cn/sc/dynamic_function/waybill/#search/bill-number/" + ShipOrderNumber + "\"  target='_blank'>顺丰官网查询></a></li></ul>";
            }
            else {
                hidHeadHtml += "<li style='text-align:right'><a href=\"https://www.kuaidi100.com/chaxun?nu=" + ShipOrderNumber + "\"  target='_blank'>快递100查询></a></li></ul>";
            }

            $('#spExpressData').expressInfo(orderId, 'OrderId', hidHeadHtml);
            ShowMessageDialog("物流详情", "Exprass", "myTab_Content1");
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
        })
    </script>
</asp:Content>
