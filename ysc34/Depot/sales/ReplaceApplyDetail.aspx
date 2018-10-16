<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="ReplaceApplyDetail.aspx.cs" Inherits="Hidistro.UI.Web.Depot.Sales.ReplaceApplyDetail" %>

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
            if ($("#hidReplaceStatus").val() == "0") {
                $(".acceptRow").show();
            }
            if ($("#hidReplaceStatus").val() == "4") {
                $(".sendgoodsRow").show();
            }
        })

        function acceptReplace() {
            var adminRemark = $("#txtAdminRemark").val();
            var adminShipAddress = $("#txtAdminShipAddress").val();
            var adminShipTo = $("#txtAdminShipTo").val();
            var adminCellPhone = $("#txtAdminCellPhone").val();
            if (adminShipAddress == "" || adminShipTo == "" || adminCellPhone == "") { alert("请输入平台收货地址和联系方式"); return false; }
            return true;
        }
        function refuseReplace() {
            var adminRemark = $("#txtAdminRemark").val();
            if (adminRemark == "") { alert("请在管理员备注中输入拒绝原因"); return false; }
            return true;
        }

        ///提交确认收货并且发货
        function GetAndSendGoods() {
            var adminRemark = $("#txtAdminRemark").val();
            var express = $("#expressDropDownList").val();
            var shipOrderNumber = $("#txtShipOrderNumber").val();
            if (express == undefined || express == "") {
                alert("请选择发货快递公司");
                return false;
            }
            if (shipOrderNumber == "" || shipOrderNumber.length > 20) {
                alert("请输入快递编号,长度为1到20位");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="hidReplaceStatus" ClientIDMode="Static" runat="server" />
    <div class="dataarea mainwidth databody">
         <!--查看物流-->
        <div id="ViewLogistics" style="display: none">
            <div class="frame-content">
                <h1>快递单物流信息</h1>

                <div id="expressInfo">正在加载中....</div>
            </div>
        </div>
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">换货详情</a></li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="dingdanxx">
                <div class="dingdan_box3">
                    <div class="dingdanhao line_bottom">
                        <span>售后编号：<asp:Literal ID="txtAfterSaleId" runat="server" /></span>
                        <span>状态：<asp:Literal ID="txtStatus" runat="server" /></span>
                        <input type="button" runat="server" class="btn btn-default" id="btnViewUserLogistic" onclick="ViewReplaceUserLogistics(this)" clientidmode="Static" visible="false" value="用户物流跟踪" />
                        <input type="button" runat="server" class="btn btn-default" id="btnViewMallLogistic" onclick="ViewReplaceMallLogistics(this)" clientidmode="Static" visible="false" value="商家物流跟踪" />
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
                    <li>订单编号：<em><asp:Literal ID="litOrderId" runat="server"></asp:Literal></em></li>
                    <li>换货原因：<em><asp:Literal ID="litRefundReason" runat="server"></asp:Literal></em></li>
                    <li class="returnRow" style="display: none">换货数量：<em><asp:Literal ID="litReturnQuantity" runat="server"></asp:Literal></em></li>
                    <li>买家备注：<em><asp:Literal ID="litRemark" runat="server"></asp:Literal></em></li>
                    <li>订单总金额：<em><Hi:FormatedMoneyLabel ID="litOrderTotal" runat="server" /></em></li>
                    <li id="divCredentials" runat="server">售后凭证：<span><asp:Literal ID="litImageList" runat="server"></asp:Literal></span></li>
                    <li>用户收货信息：<em><asp:Literal ID="litUserAddress" runat="server"></asp:Literal></em></li>
                    <li class="acceptRow" style="display: none"><b>收货地址：</b>
                        <em>
                            <asp:Literal ID="litAdminShipAddrss" runat="server" Visible="false"></asp:Literal><asp:TextBox ID="txtAdminShipAddress" placeholer="请填写平台的收货地址" ClientIDMode="Static" runat="server" CssClass="forminput form-control" Width="350" /></em>
                    </li>
                    <li class="acceptRow" style="display: none"><b>收货人：</b>
                        <em>
                            <asp:Literal ID="litAdminShipTo" runat="server" Visible="false"></asp:Literal>
                            <asp:TextBox ID="txtAdminShipTo" placeholer="请填写平台的收货人" runat="server" ClientIDMode="Static" CssClass="forminput form-control" /></em>
                    </li>
                    <li class="acceptRow" style="display: none"><b>收货人电话：</b>
                        <em>
                            <asp:Literal ID="litAdminCellPhone" runat="server" Visible="false"></asp:Literal><asp:TextBox ID="txtAdminCellPhone" ClientIDMode="Static" placeholer="请填写平台的收货人电话" runat="server" CssClass="forminput form-control" /></em>
                    </li>
                    <li class="sendgoodsRow inputrow" style="display: none"><b>物流公司：</b>
                        <em>
                            <Hi:ExpressDropDownList runat="server" repeatcolumns="6" repeatdirection="Horizontal" ClientIDMode="Static" ID="expressDropDownList" />
                        </em>
                    </li>
                    <li class="sendgoodsRow inputrow" style="display: none"><b>运单号码：</b>
                        <em>
                            <asp:TextBox runat="server" ID="txtShipOrderNumber" ClientIDMode="Static" placeholer="运单号码不能为空，在1至20个字符之间." class="forminput form-control" Width="350" /></em>
                    </li>
                    <li class="inputrow"><b>管理员备注：</b><em><asp:TextBox ID="txtAdminRemark" ClientIDMode="Static" placeholer="拒绝时,必须填写拒绝理由" runat="server" CssClass="forminput" Width="350px" /></em></li>
                </ul>
                <div class="modal_table_footer">
                    <asp:Button ID="btnAcceptReplace" runat="server" OnClientClick="return acceptReplace()" Visible="false" CssClass="btn btn-primary" Text="确认换货" />
                    <asp:Button ID="btnRefuseReplace" runat="server" OnClientClick="return refuseReplace()" Visible="false" CssClass="btn btn-default" Text="拒绝换货" />
                    <asp:Button ID="btnGetAndSendGoods" runat="server" OnClientClick="return GetAndSendGoods()" CssClass="btn btn-primary" Visible="false" Text="确认收货并发货" />
                </div>
            </div>

        </div>

    </div>
</asp:Content>
