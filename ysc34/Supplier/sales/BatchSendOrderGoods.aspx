<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="BatchSendOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.sales.BatchSendOrderGoods" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        html {
            background: #fff !important;
        }

        body {
            padding: 0;
            width: 100%;
        }

        #mainhtml {
            margin: 0;
            padding: 20px 20px 60px 20px;
            width: 100% !important;
        }

        .carat {
            margin-right: 0 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <%--<div class="title">
            <em>
                <img src="../images/05.gif" width="32" height="32" /></em>
            <h1>订单批量发货</h1>
            <span>这里的订单将执行批量发货操作</span>
        </div>--%>
        <div class="searcharea clearfix" style="border-bottom: none;margin-bottom: 0;padding-bottom: 0;">
            <ul>
                <li style="margin-bottom: 10px;">
                    <span>发货方式：</span>
                    <span>
                        <input type="radio" name="sendGoodType" id="radio_sendGoodType_1" value="1" checked="checked" /><label for="radio_sendGoodType_1" style="margin-right: 40px;">需要物流</label>
                        <input type="radio" name="sendGoodType" id="radio_sendGoodType_2" value="0" /><label for="radio_sendGoodType_2">无需物流</label>
                    </span>
                </li>
            </ul>
        </div>
        <div class="searcharea clearfix" id="div_wuliu">
            <ul>
                <li><span>物流公司： </span><span>
                    <asp:DropDownList ID="dropExpressComputerpe" CssClass="iselect" ClientIDMode="Static" runat="server" NullToDisplay="请选择物流公司" /></span></li>
                <li><span>起始单号： </span><span>
                    <asp:TextBox ID="txtStartShipOrderNumber" placeholder="长度为1到20位" CssClass="forminput form-control" Width="160px" ClientIDMode="Static" runat="server" /></span></li>
                <li>
                    <asp:Button ID="btnSetShipOrderNumber" runat="server" OnClientClick="return CheckShipNumber()" Text="确定" CssClass="btn btn-primary" /></li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <asp:Repeater ID="repOrderGoods" runat="server">
                <HeaderTemplate>
                    <table width="0" border="0" cellspacing="0">
                        <tr class="table_title">
                            <td width="15%" class="td_right td_left">订单编号</td>
                            <td width="14%" class="td_right td_left">收货人</td>
                            <td width="14%" class="td_right td_left">地区</td>
                            <td width="18%" class="td_right td_left">详细地址</td>
                            <td width="12%" class="td_right td_left">配送方式</td>
                            <td width="12%" class="td_left td_right_fff">物流公司</td>
                            <td width="12%" class="td_left td_right_fff">发货单号</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="td_bg">
                        <td><%#Eval("OrderId") %></td>
                        <td>
                            <asp:Literal runat="server" Text='<%# Eval("ShipTo") %>' /></td>
                        <td>
                            <asp:Literal runat="server" Text='<%# Eval("ShippingRegion") %>' /></td>
                        <td>
                            <asp:Literal runat="server" Text='<%# Eval("Address") %>' /></td>

                        <td>
                            <Hi:ExpressDropDownList runat="server" ID="dropExpress"></Hi:ExpressDropDownList></td>
                        <td>
                            <asp:TextBox runat="server" ID="txtShippOrderNumber" CssClass="forminput form-control txt-orderNumber" Text='<%# Eval("ShipOrderNumber") %>' placeholder="不能为空" /></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater> 
            <asp:Repeater ID="grdOrderGoods" runat="server">
                <HeaderTemplate>
                    <table class="table table-striped grdOrderGoods">
                        <tr>
                            <th scope="col">订单编号</th>
                            <th scope="col">收货人</th>
                            <th scope="col">地区</th>
                            <th scope="col">详细地址</th>
                            <th scope="col">物流公司</th>
                            <th scope="col">发货单号</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <!--项目模板，会进行循环显示，放置表格第二行-->
                    <tr>
                        <td>

                            <asp:HiddenField ID="hidorderId" Value='<%#Eval("OrderId")%>' runat="server" />
                            <%#Eval("OrderId") %>
                        </td>
                        <td>
                            <%# Eval("ShipTo") %>
                        </td>
                        <td><%# Eval("ShippingRegion") %> </td>
                        <td>
                            <%# Eval("Address") %>
                        </td>
                        <td>
                            <Hi:ExpressDropDownList runat="server" CssClass="iselect_one" ID="expressList1"></Hi:ExpressDropDownList></td>
                        <td>
                            <asp:TextBox runat="server" ID="txtShippOrderNumber" CssClass="forminput form-control txt-orderNumber" Text='<%# Eval("ShipOrderNumber") %>' /></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>       
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div class="modal_iframe_footer">
            <asp:Button runat="server" ID="btnBatchSendGoods" OnClientClick="return CheckSendGoods();" Text="批量发货" CssClass="btn btn-primary" />
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input:radio[name="sendGoodType"]').change(function () {
                var need = $("input[name='sendGoodType']:checked").val();
                if (need == 1) {
                    $("#div_wuliu").show();
                    $(".iselect_one").attr("disabled", false);

                    $(".txt-orderNumber").css("background-color", "#fff");
                    $(".txt-orderNumber").attr("disabled", false);
                } else {
                    $("#div_wuliu").hide();
                    $(".iselect_one").attr("disabled", true);

                    $(".txt-orderNumber").css("background-color", "#E0E0E0");
                    $(".txt-orderNumber").attr("disabled", true);
                }
            });
        });

        function CheckSendGoods() {
            var need = $("input[name='sendGoodType']:checked").val();
            if (need == 1) {
                var selectArray = $(".iselect_one");
                var orderNumberArray = $(".txt-orderNumber");
                for (var i = 0; i < selectArray.length; i++) {
                    var selectVal = $(selectArray[i]).val();
                    if (selectVal == "") {
                        alert("请选择快递公司");
                        $(selectArray[i]).focus();
                        return false;
                    }
                }

                for (var i = 0; i < orderNumberArray.length; i++) {
                    var orderNumber = $.trim($(orderNumberArray[i]).val());
                    if (orderNumber == "") {
                        alert("请输入快递单号");
                        $(orderNumberArray[i]).focus();
                        return false;
                    }
                }

            }
            return true;
        }

        function CheckShipNumber() {
            var no = $("#txtStartShipOrderNumber").val();
            if ($("#dropExpressComputerpe").val() == "") {
                ShowMsg('请先选择物流公司!', false);
                return false;
            }
            if ($("#txtStartShipOrderNumber").val() == "") {
                ShowMsg('请填写起始单号!', false);
                return false;
            }
            var end = no.substr(no.length - 1, 1);
            if (!is_num(end)) {
                ShowMsg('请输入正确的快递单号!', false);
                return false;
            }
            else if ($("#dropExpressComputerpe").val() == "EMS" && !isEMSNo(no)) {
                ShowMsg('请输入正确的EMS单号!', false);
                return false;
            }
            else if ($("#dropExpressComputerpe").val() == "顺丰快递" && !isSFNo(no)) {
                ShowMsg('请输入正确的顺丰单号!', false);
                return false;
            }
            return true;
        }

        function isSFNo(no) {

            if (!is_num(no) || no.length != 12) {
                return false;
            } else {
                return true;
            }
        }

        function is_num(str) {
            var pattrn = /^[0-9]+$/;
            if (pattrn.test(str)) {
                return true;
            } else {
                return false;
            }
        }
        function isEMSNo(no) {
            if (no.length != 13) {
                return false;
            }

            if (getEMSLastNum(no) == no.substr(10, 1)) {
                return true;
            } else {
                return false;
            }
        }
        function getEMSLastNum(no) {
            var v = new Number(no.substr(2, 1)) * 8;
            v += new Number(no.substr(3, 1)) * 6;
            v += new Number(no.substr(4, 1)) * 4;
            v += new Number(no.substr(5, 1)) * 2;
            v += new Number(no.substr(6, 1)) * 3;
            v += new Number(no.substr(7, 1)) * 5;
            v += new Number(no.substr(8, 1)) * 9;
            v += new Number(no.substr(9, 1)) * 7;
            v = 11 - v % 11;
            if (v == 10)
                v = 0;
            else if (v == 11)
                v = 5;
            return v.toString();
        }
    </script>
</asp:Content>
