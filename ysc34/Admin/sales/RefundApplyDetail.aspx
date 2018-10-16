<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RefundApplyDetail.aspx.cs" Inherits="Hidistro.UI.Web.Admin.RefundApplyDetail" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.Core" Assembly="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function acceptRefund() {
            var adminRemark = $("#txtAdminRemark").val();
            var refundMoney = parseFloat($("#txtRefundMoney").val());
            var MaxRefundMoney = parseFloat($("#hidRefundMaxAmount").val().trim());
            if (!Reg_float_non_negative.test($("#txtRefundMoney").val().trim())) {
                ShowMsg("请输入正确的退款金额，必须为小数或者整数，且不能小于0,精确到2位小数", false);
                return false;
            }
            if (isNaN(refundMoney) || isNaN(MaxRefundMoney) || refundMoney > MaxRefundMoney || refundMoney < 0 || (Math.round(refundMoney * 100) / 100 != refundMoney)) {
                ShowMsg("请输入确认退款金额,金额不能大于可退款金额，且不能小于0,精确到2位小数。", false);
                return false;
            }

            return true;
        }
        function refuseRefund() {
            var adminRemark = $("#txtAdminRemark").val();
            if (adminRemark == "") { ShowMsg("请在管理员备注中输入拒绝原因", false); return false; }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <asp:HiddenField ID="hidRefundMaxAmount" ClientIDMode="Static" runat="server" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">退款详情</a></li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="dingdanxx">
                <div class="dingdan_box3">
                    <div class="dingdanhao line_bottom">
                        <span>售后编号：<asp:Literal ID="txtAfterSaleId" runat="server" /></span>
                        <span>状态：<asp:Literal ID="txtStatus" runat="server" /></span>
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
                    <li class="acceptRow"><b>订单编号：</b><asp:Literal ID="litOrderId" runat="server"></asp:Literal></li>
                    <%if (RefundData.IsServiceProduct && !string.IsNullOrWhiteSpace(RefundData.ValidCodes))
                        { %><li class="acceptRow"><b>退款密码：</b><span class="refund_password"><%=RefundData.ValidCodes %></span></li>
                    <%} %>
                    <%if (!string.IsNullOrWhiteSpace(RefundData.ExceptionInfo))
                        { %><li class="acceptRow"><b>退款异常：</b><%=RefundData.ExceptionInfo %></li>
                    <%} %>
                    <li class="acceptRow"><b>退款原因：</b><asp:Literal ID="litRefundReason" runat="server"></asp:Literal></li>
                    <li class="acceptRow"><b>申请退款金额：</b><Hi:FormatedMoneyLabel ID="litRefundTotal" runat="server" />
                        (实付&yen;<asp:Literal ID="txtPayMoney" runat="server"></asp:Literal>)</li>
                    <li class="acceptRow"><b>期望退款途径：</b><asp:Literal ID="litType" runat="server"></asp:Literal></li>
                    <li class="acceptRow"><b>买家备注：</b><asp:Literal ID="litRemark" runat="server"></asp:Literal></li>
                    <li class="acceptRow"><b>订单总金额：</b><Hi:FormatedMoneyLabel ID="litOrderTotal" runat="server" /></li>
                    <li class="acceptRow"><b>确认退款金额：</b><em id="showPanel" runat="server"><asp:Literal ID="litRefundMoney" runat="server"></asp:Literal></em><em class="tips" id="inputPanel" runat="server"><asp:TextBox ID="txtRefundMoney" ClientIDMode="Static" runat="server" CssClass="form_input_s form-control" /><span class="input-group-addon"></span></em></li>
                    <li class="acceptRow"><b>管理员备注：</b><asp:TextBox ID="txtAdminRemark" ClientIDMode="Static" placeholer="拒绝退款时,必须填写拒绝理由" runat="server" CssClass="forminput" Width="350px" /></li>

                </ul>
                <div class="modal_table_footer">
                    <asp:Button ID="btnAcceptRefund" runat="server" OnClientClick="return acceptRefund()" CssClass="btn btn-primary mr10" Text="确认退款" />
                    <asp:Button ID="btnRefuseRefund" runat="server" OnClientClick="return refuseRefund()" CssClass="btn btn-default" Text="拒绝退款" />
                </div>
            </div>

        </div>

    </div>

    <!--查看物流-->
    <div id="ViewLogistics" style="display: none">
        <div class="frame-content">
            <h1>快递单物流信息</h1>

            <div id="expressInfo">正在加载中....</div>
        </div>
    </div>
</asp:Content>
