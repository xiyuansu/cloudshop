<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="BatchSendOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Depot.sales.BatchSendOrderGoods" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div></div>
    <div class="dataarea mainwidth databody">
        <div class="searcharea clearfix sendod" style="padding-bottom: 0;">
            <ul>
                <li>
                    <span>发货方式：</span>
                    <span>
                         <asp:HiddenField ID="txtSendGoodType" runat="server" ClientIDMode="Static" Value="1" />
                        <input type="radio" name="sendGoodType" id="radio_sendGoodType_1" value="1" checked="checked" /><label for="radio_sendGoodType_1" style="margin-right: 40px;">需要物流</label>
                        <label id="labSameCity" runat="server">
                            <input type="radio" name="sendGoodType" id="radio_sendGoodType_3" value="2" /><label for="radio_sendGoodType_3" style="margin-right: 40px;">同城物流</label></label>
                        <input type="radio" name="sendGoodType" id="radio_sendGoodType_2" value="0" /><label for="radio_sendGoodType_2">无需物流</label>
                    </span>
                </li>
            </ul>
        </div>
        <div class="searcharea clearfix sendod" id="div_wuliu">
            <ul>
                <li><span>物流公司：</span>
                    <asp:DropDownList ID="dropExpressComputerpe" CssClass="forminput" ClientIDMode="Static" runat="server" /></li>
                <li><span>起始发货单号：</span><asp:TextBox Width="157" ID="txtStartShipOrderNumber" CssClass="forminput form-control" ClientIDMode="Static" runat="server" /></li>
                <li>
                    <asp:Button ID="btnSetShipOrderNumber" runat="server" OnClientClick="return CheckShipNumber()" Text="确定" CssClass="btn btn-warning" /></li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
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
                    <tr>
                        <td>

                            <asp:HiddenField ID="hidorderId" Value='<%#Eval("OrderId")%>' runat="server" />
                            <a href='<%# "OrderDetails.aspx?OrderId="+Eval("OrderId") %>'><%#Eval("OrderId") %></a>
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
                            <span class="showFreight" orderid="<%# Eval("OrderId") %>"></span>
                            <asp:HiddenField runat="server" ID="txtDeliveryNo" Value="" />
                            <asp:TextBox runat="server" ID="txtShippOrderNumber" CssClass="forminput form-control txt-orderNumber" Text='<%# Eval("ShipOrderNumber") %>' /></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>       
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div class="blank5 clearfix"></div>
        <div style="padding-left: 380px;">
            <asp:Button runat="server" ID="btnBatchSendGoods" OnClientClick="return CheckSendGoods();" Text="批量发货" CssClass="btn btn-primary" />
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input:radio[name="sendGoodType"]').change(function () {
                var need = $("input[name='sendGoodType']:checked").val();
                $("#txtSendGoodType").val(need);
                if (need == 1) {
                    $("#div_wuliu").show();
                    $(".iselect_one").attr("disabled", false);

                    $(".txt-orderNumber").css("background-color", "#fff");
                    $(".txt-orderNumber").attr("disabled", false);
                    $(".showFreight").hide();
                    $(".txt-orderNumber").show();
                } else {
                    if (need == 2) {
                        $(".txt-orderNumber").css("background-color", "#fff");
                        $(".txt-orderNumber").attr("disabled", false);
                        $(".txt-orderNumber").hide();
                        $(".showFreight").show();
                        var orderIds = "";
                        $(".showFreight").each(function (index, element) {
                            if ($(this).text() == "") {
                                orderIds += (orderIds == "" ? "" : ",") + $(this).attr("orderid");
                            }
                        });
                        if (orderIds != "") {
                            var url = '/Depot/Sales/ashx/SendGoodOrders.ashx';
                            var expressData;
                            $.ajax({
                                type: "get",
                                url: url,
                                data: { action: 'QueryDeliverFees', orderIds: orderIds },
                                dataType: "json",
                                async: false,
                                success: function (data) {
                                    if (data.Result.Status == "SUCCESS") {
                                        var list = data.Result.List;
                                        for (var i = 0; i < list.length; i++) {
                                            var el = list[i];
                                            if (el.Message == "") {
                                                $("span[orderid='" + el.OrderId + "']").text(el.fee);
                                            }
                                            else {
                                                $("span[orderid='" + el.OrderId + "']").text(el.Message);
                                                $("span[orderid='" + el.OrderId + "']").next().text(el.deliveryNo);
                                            }
                                        }
                                    }
                                    else {
                                        ShowMsg(data.Result.Message);
                                    }

                                }
                            });
                        }
                    }
                    else {
                        $(".txt-orderNumber").css("background-color", "#E0E0E0");
                        $(".txt-orderNumber").attr("disabled", true);
                        $(".txt-orderNumber").show();
                        $(".showFreight").hide();
                    }
                    $(".iselect_one").attr("disabled", true);
                    $("#div_wuliu").hide();

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
                alert('请先选择物流公司!');
                return false;
            }
            if ($("#txtStartShipOrderNumber").val() == "") {
                alert('请填写起始单号!');
                return false;
            }
            var end = no.substr(no.length - 1, 1);
            if (!is_num(end)) {
                alert('请输入正确的快递单号!');
                return false;
            }
            else if ($("#dropExpressComputerpe").val() == "EMS" && !isEMSNo(no)) {
                alert('请输入正确的EMS单号!');
                return false;
            }
            else if ($("#dropExpressComputerpe").val() == "顺丰快递" && !isSFNo(no)) {
                alert('请输入正确的顺丰单号!');
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
