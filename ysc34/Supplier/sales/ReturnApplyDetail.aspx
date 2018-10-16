<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="ReturnApplyDetail.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.sales.ReturnApplyDetail" %>

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

        //检测验证(它是window.js里DialogShow方法调用)，这个页面特别验证，直接返回true就行
        function validatorForm() { return true; }
        ////供应商改变状态------结束------
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
                                    <li style="width: 90px;">供货价</li>
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
                                <div class="price">￥<%# Eval("ItemCostPrice").ToDecimal().F2ToString("f2") %></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

            </div>


            <div class="dingdan_box5">
                <ul>
                    <li class="acceptRow"><b>订单编号：</b><em><asp:Literal ID="litOrderId" runat="server"></asp:Literal></em></li>
                    <li class="acceptRow"><b><%=AfterSaleType %>原因：</b><em><asp:Literal ID="litRefundReason" runat="server"></asp:Literal></em></li>
                    <li class="acceptRow returnRow" style="display: none"><b>退货数量：</b><em><asp:Literal ID="litReturnQuantity" runat="server"></asp:Literal></em></li>
                    <li class="acceptRow"><b>买家备注：</b><em><asp:Literal ID="litRemark" runat="server"></asp:Literal></em></li>
                    <li class="acceptRow column2" id="divCredentials" runat="server"><b>售后凭证：</b>
                        <span class="after-service-img">
                            <asp:Literal ID="litImageList" runat="server"></asp:Literal></span>
                        <div class="preview-img" style="display: none;">
                            <img src="" />
                        </div>
                    </li>
                    <li class="acceptRow returnRow" style="display: none"><b>收货地址：</b>
                        <em>
                            <asp:Literal ID="litAdminShipAddrss" runat="server"></asp:Literal></em>
                    </li>
                    <li class="acceptRow returnRow" style="display: none"><b>收货人：</b>
                        <em>
                            <asp:Literal ID="litAdminShipTo" runat="server"></asp:Literal></em>
                    </li>
                    <li class="acceptRow returnRow" style="display: none"><b>收货人电话：</b>
                        <em>
                            <asp:Literal ID="litAdminCellPhone" runat="server"></asp:Literal></em>
                    </li>
                    <li class="acceptRow"><b>管理员备注：</b>
                        <em>
                            <asp:Literal ID="liAdminRemark" runat="server" /></em>
                    </li>
                </ul>
                <div class="modal_table_footer" style="float: right; padding-right: 200px;">
                    <asp:Button ID="btnGetGood2" runat="server" CssClass="btn btn-primary" Visible="false" Text="确认收货" OnClientClick="return PnGoods()" />
                </div>

                <div style="display: none;">
                    <div id="divGetGoods">
                        确认已收货，待平台退款!
                    </div>
                    <asp:Button ID="btnGetGoods" runat="server" CssClass="btn btn-primary" Visible="false" Text="确认收货" />
                </div>
                <script type="text/javascript">
                    function PnGoods() {
                        DialogShow("确认收货", "suppliergoods", "divGetGoods", "ctl00_contentHolder_btnGetGoods");//确认收货 弹窗
                        return false;
                    }
                </script>

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
            cursor: zoom-out;
        }

            .preview-img img {
                width: 300px;
                height: 300px;
            }
    </style>
</asp:Content>
