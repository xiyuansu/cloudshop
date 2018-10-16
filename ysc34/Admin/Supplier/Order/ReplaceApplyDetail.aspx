<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReplaceApplyDetail.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.Order.ReplaceApplyDetail" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.Core" Assembly="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script src="../../sales/order.helper.js?v=3.4"></script>
<script src="../../../Utility/expressInfo.js"></script>
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
            if (adminShipAddress == "" || adminShipTo == "" || adminCellPhone == "") { alert("请输入收货地址和联系方式"); return false; }
            return true;
        }
        function refuseReplace() {
            var adminRemark = $("#txtAdminRemark").val();
            if (adminRemark == "") { ShowMsg("请在管理员备注中输入拒绝原因", false); return false; }
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
                                     <div class="name" style="line-height:30px">
                                       货号： <%# Eval("SKU") %> <%# Eval("SKUContent") %>
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
                    <li><b>订单编号：</b><em><asp:Literal ID="litOrderId" runat="server"></asp:Literal></em></li>
                    <li><b>换货原因：</b><em><asp:Literal ID="litRefundReason" runat="server"></asp:Literal></em></li>
                    <li><b>换货数量：</b><em><asp:Literal ID="litReturnQuantity" runat="server"></asp:Literal></em></li>
                    <li><b>买家备注：</b><em><asp:Literal ID="litRemark" runat="server"></asp:Literal></em></li>
                    <li><b>订单总金额：</b><em><Hi:FormatedMoneyLabel ID="litOrderTotal" runat="server" /></em></li>
                    <li id="divCredentials" runat="server" style="float:left;width:100%;"><b style="float:left;">售后凭证：</b>
                         <span class="after-service-img">
                             <asp:Literal ID="litImageList" runat="server"></asp:Literal></span>
                        <div class="preview-img" style="display: none;">
                            <img src="" />
                        </div>
                    </li>
                    <li><b>收货信息：</b><em><asp:Literal ID="litUserAddress" runat="server"></asp:Literal></em></li>
                    <li class="acceptRow" style="display: none"><b>收货地址：</b>
                        <em>
                            <asp:Literal ID="litAdminShipAddrss" runat="server" Visible="false"></asp:Literal><asp:TextBox ID="txtAdminShipAddress" placeholer="请填写收货地址" ClientIDMode="Static" runat="server" CssClass="forminput form-control" Width="350" /></em>
                    </li>
                    <li class="acceptRow" style="display: none"><b>收货人：</b>
                        <em>
                            <asp:Literal ID="litAdminShipTo" runat="server" Visible="false"></asp:Literal>
                            <asp:TextBox ID="txtAdminShipTo" placeholer="请填写收货人" runat="server" ClientIDMode="Static" CssClass="forminput form-control" /></em>
                    </li>
                    <li class="acceptRow" style="display: none"><b>收货人电话：</b>
                        <em>
                            <asp:Literal ID="litAdminCellPhone" runat="server" Visible="false"></asp:Literal><asp:TextBox ID="txtAdminCellPhone" ClientIDMode="Static" placeholer="请填写收货人电话" runat="server" CssClass="forminput form-control" /></em>
                    </li>                   
                    <li class="inputrow"><b>管理员备注：</b><em><asp:TextBox ID="txtAdminRemark" ClientIDMode="Static" placeholer="拒绝时,必须填写拒绝理由" runat="server" CssClass="forminput" Width="350px" /></em></li>
                </ul>
                <div class="modal_table_footer">
                    <asp:Button ID="btnAcceptReplace" runat="server" OnClientClick="return acceptReplace()" Visible="false" CssClass="btn btn-primary" Text="确认换货" />
                    <asp:Button ID="btnRefuseReplace" runat="server" OnClientClick="return refuseReplace()" Visible="false" CssClass="btn btn-default" Text="拒绝换货" />
                    
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
