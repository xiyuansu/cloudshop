<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReturnApplyDetail.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.ReturnApplyDetail" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.Core" Assembly="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="order.helper.js?v=3.4" type="text/javascript"></script>
    <script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function (e) {
            if ($("#hidAfterSaleType").val() != "3") {
                $(".returnRow").show();
            }
            if ($("#hidReturnStatus").val() == "0") {
                $(".acceptRow").show();
                if ($("#hidAfterSaleType").val() == "3") {
                    $(".returnRow").hide();
                }

            }
        });

        function acceptReturn() {
            var refundMoney = parseFloat($("#txtRefundMoney").val());
            var MaxRefundMoney = parseFloat($("#hidRefundMaxAmount").val().trim());
            if (!Reg_float_non_negative.test($("#txtRefundMoney").val().trim())) {
                ShowMsg("请输入正确的退款金额，必须为小数或者整数，且不能小于0,精确到2位小数", false);
                return false;
            }
            if (isNaN(refundMoney) || isNaN(MaxRefundMoney) || refundMoney > MaxRefundMoney || refundMoney < 0 || (Math.round(refundMoney * 100) / 100 != refundMoney)) {
                ShowMsg("请输入确认退款金额,金额不能大于订单/商品总额，且不能小于0,精确到2位小数。", false);
                return false;
            }
            var adminShipAddress = $("#txtAdminShipAddress").val();
            var adminShipTo = $("#txtAdminShipTo").val();
            var adminCellPhone = $("#txtAdminCellPhone").val();
            if (adminShipAddress == "" && $("#hidAfterSaleType").val() != "3") { ShowMsg("请输入平台收货地址和联系方式", false); return false; }
            var adminRemark = $("#txtAdminRemark").val();
            return true;
        }
        function refuseReturn() {
            var adminRemark = $("#txtAdminRemark").val();
            if (adminRemark == "") { ShowMsg("请在管理员备注中输入拒绝原因", false); return false; }
            return true;
        }

        ///确认退货的收货
        function FinishGetGoods() {
            var refundMoney = parseFloat($("#txtRefundMoney").val());
            var MaxRefundMoney = parseFloat($("#hidRefundMaxAmount").val().trim());
            if (!Reg_float_non_negative.test($("#txtRefundMoney").val().trim())) {
                ShowMsg("请输入正确的退款金额，必须为小数或者整数，且不能小于0,精确到2位小数", false);
                return false;
            }
            if (isNaN(refundMoney) || isNaN(MaxRefundMoney) || refundMoney > MaxRefundMoney || refundMoney < 0 || (Math.round(refundMoney * 100) / 100 != refundMoney)) {
                ShowMsg("请输入确认退款金额,金额不能大于订单/商品总额，且不能小于0,精确到2位小数。", false);
                return false;
            }
            return true;
        }
        ///完成退货
        function FinishReturn() {
            var refundMoney = parseFloat($("#txtRefundMoney").val());
            var MaxRefundMoney = parseFloat($("#hidRefundMaxAmount").val().trim());
            if (!Reg_float_non_negative.test($("#txtRefundMoney").val().trim())) {
                ShowMsg("请输入正确的退款金额，必须为小数或者整数，且不能小于0,精确到2位小数", false);
                return false;
            }
            if (isNaN(refundMoney) || isNaN(MaxRefundMoney) || refundMoney > MaxRefundMoney || refundMoney < 0 || (Math.round(refundMoney * 100) / 100 != refundMoney)) {
                ShowMsg("请输入确认退款金额,金额不能大于订单/商品总额，且不能小于0,精确到2位小数。", false);
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="hidRefundMaxAmount" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hidReturnStatus" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hidAfterSaleType" ClientIDMode="Static" runat="server" />
    <div class="dataarea mainwidth databody">
        <!--查看物流-->
        <div id="ViewLogistics" style="display: none">
            <div class="frame-content">
                <h1>快递单物流信息</h1>
                <div id="expressBaseInfo"></div>
                <div id="expressInfo">正在加载中....</div>
            </div>
        </div>
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void"><%=AfterSaleType %>详情</a></li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="dingdanxx">
                <div class="dingdan_box3">
                    <div class="dingdanhao line_bottom">
                        <span>售后编号：<asp:Literal ID="txtAfterSaleId" runat="server" /></span>
                        <span>状态：<asp:Literal ID="txtStatus" runat="server" /></span>
                        <input type="button" runat="server" class="btn btn-default" id="btnViewLogistic" onclick="ViewReturnLogistics(this)" clientidmode="Static" visible="false" value="物流跟踪" />
                    </div>
                </div>

                <div class="Refund_pro">
                    <asp:Repeater ID="listPrducts" runat="server">
                        <HeaderTemplate>
                            <div class="title">
                                <ul>
                                    <li style="width: 580px; padding-left: 20px;">商品名称</li>
                                    <li style="width: 250px;">购买数量</li>
                                    <li style="width: 90px;">单价</li>
                                </ul>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="con">
                                <div class="info">
                                    <div class="pic">
                                        <Hi:ProductDetailsLink ID='ProductDetailsLink' runat='server' ProductName='<%# Eval("ItemDescription") %>'
                                            ProductId='<%# Eval("ProductId")%>' ImageLink="true">
                        <Hi:HiImage DataField="ThumbnailsUrl" Width="60px" runat="server" ToolTip="" />
                                        </Hi:ProductDetailsLink>
                                    </div>
                                    <div class="name">
                                        <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ItemDescription") %>'
                                            ProductId='<%# Eval("ProductId")%>' ImageLink="false"></Hi:ProductDetailsLink>
                                    </div>
                                </div>
                                <div class="num">X  <b><%# Eval("Quantity") %></b></div>
                                <div class="price">￥<%# Eval("ItemAdjustedPrice").ToDecimal().F2ToString("f2") %></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

            </div>


            <div class="dingdan_box5">
                <ul>
                    <li class="acceptRow"><b>订单编号：</b><em><asp:Literal ID="litOrderId" runat="server"></asp:Literal></em></li>
                    <li class="acceptRow"><b><%=AfterSaleType %>原因：</b><em><asp:Literal ID="litRefundReason" runat="server"></asp:Literal></em></li>
                    <li class="acceptRow"><b>申请退款金额：</b><em><Hi:FormatedMoneyLabel ID="litRefundTotal" runat="server" />
                        (实付&yen;<asp:Literal ID="txtPayMoney" runat="server"></asp:Literal>)</em></li>
                    <li class="acceptRow"><b>退款途径：</b><em><asp:Literal ID="litType" runat="server"></asp:Literal></em></li>
                    <li class="acceptRow returnRow" style="display: none"><b>退货数量：</b><em><asp:Literal ID="litReturnQuantity" runat="server"></asp:Literal></em></li>
                    <li class="acceptRow"><b>买家备注：</b><em><asp:Literal ID="litRemark" runat="server"></asp:Literal></em></li>
                    <li class="acceptRow"><b>订单金额：</b><em><Hi:FormatedMoneyLabel ID="litOrderTotal" runat="server" /></em></li>
                    <li class="acceptRow column2" id="divCredentials" runat="server"><b>售后凭证：</b>
                        <span class="after-service-img">
                            <asp:Literal ID="litImageList" runat="server"></asp:Literal></span>
                        <div class="preview-img" style="display: none;">
                            <img src="" />
                        </div>
                    </li>
                    <li class="acceptRow"><b>确认退款金额：</b><em id="showPanel" runat="server"><asp:Literal ID="litRefundMoney" runat="server"></asp:Literal></em><em class="tips" id="inputPanel" runat="server"><asp:TextBox ID="txtRefundMoney" ClientIDMode="Static" runat="server" CssClass="form_input_s form-control" /><span class="input-group-addon">元</span></em></li>
                    <li class="acceptRow returnRow" style="display: none"><b>收货地址：</b>
                        <em>
                            <asp:Literal ID="litAdminShipAddrss" runat="server" Visible="false"></asp:Literal><asp:TextBox ID="txtAdminShipAddress" placeholer="请填写平台的收货地址" runat="server" CssClass="forminput form-control" Width="350" /></em>
                    </li>
                    <li class="acceptRow returnRow" style="display: none"><b>收货人：</b>
                        <em>
                            <asp:Literal ID="litAdminShipTo" runat="server" Visible="false"></asp:Literal>
                            <asp:TextBox ID="txtAdminShipTo" placeholer="请填写平台的收货人" runat="server" CssClass="forminput form-control" Width="350px" /></em>
                    </li>
                    <li class="acceptRow returnRow" style="display: none"><b>收货人电话：</b>
                        <em>
                            <asp:Literal ID="litAdminCellPhone" runat="server" Visible="false"></asp:Literal><asp:TextBox ID="txtAdminCellPhone" placeholer="请填写平台的收货人电话" runat="server" Width="350px" CssClass="forminput form-control" /></em>
                    </li>
                    <li class="acceptRow"><b>管理员备注：</b><em><asp:TextBox ID="txtAdminRemark" placeholer="拒绝时,必须填写拒绝理由" runat="server" CssClass="forminput" Width="350px" /></em></li>
                </ul>
                <div class="modal_table_footer">
                    <asp:Button ID="btnAcceptReturn" runat="server" OnClientClick="return acceptReturn()" Visible="false" CssClass="btn btn-primary" Text="确认退货" />
                    <asp:Button ID="btnRefuseReturn" runat="server" OnClientClick="return refuseReturn()" Visible="false" CssClass="btn btn-default" Text="拒绝退货" />
                    <asp:Button ID="btnGetGoods" runat="server" CssClass="btn btn-primary" Visible="false" OnClientClick="return FinishGetGoods()" Text="确认收货" />
                    <asp:Button ID="btnFinishReturn" runat="server" CssClass="btn btn-primary" Visible="false" OnClientClick="return FinishReturn()"  Text="完成退货" />
                </div>
            </div>

        </div>

    </div>
    <script type="text/javascript">
        $(function () {
            $('.after-service-img img').click(function () {
                var bigsrc = $(this).attr('src');
                var is_img = $(this).parents('.column2').find('.preview-img img').attr('src');
                $(this).addClass("active");
                $(this).siblings().removeClass("active");
                $(".preview-img img").attr("src", bigsrc);
                $(".preview-img").show(300);
            })
            $(".preview-img").click(function () {
                $(this).hide(300);
            })
        })
    </script>
    <style>
        .after-service-img img {
            position: relative;
            width: 50px;
            height: 50px;
            display: block;
            float: left;
            margin-right: 12px;
            border: 3px solid #ececec;
            margin-bottom: 10px;
        }

        .after-service-img img {
            cursor: zoom-in;
        }

        .after-service-img .active {
            border: 2px solid #f43a3e !important;
            position: relative;
            cursor: zoom-out;
        }

        .preview-img {
            width: 305px;
            clear: both;
            overflow: hidden;
            padding: 10px;
            border: 1px solid #ddd;
            position: relative;
            left: 0;
            display: none;
            margin: 20px 0 20px 110px;
            cursor:zoom-out;
        }

            .preview-img img {
                width: 300px;
                height: 300px;
            }
    </style>
</asp:Content>
