<%@ Page Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="SendOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.sales.SendGoods" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Supplier/Ascx/Order_ItemsList.ascx" %>
<%--<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Supplier/Ascx/Order_ShippingAddress.ascx" %>--%>
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
.modal_iframe_footer {
    position: fixed;
    bottom: 0;
    left: 0;
    z-index: 1;
    border-top: 1px solid #b8b8b8;
    width: 100%;
    padding: 20px 0 20px 0;
    background: #fff;
}
.modal_iframe_footer .btn {
    float: right;
    margin-right: 20px;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	   <script type="text/javascript" src="/utility/jquery.cssforms.js"></script>
	   	<script type="text/javascript">
		$(function(){	
			$('.beautify_input').cssSelect();
		});
	</script>
    <%--<div class="dataarea mainwidth databody">--%>
    <div class="dataarea mainwidth">
        
        <div class="Purchase" style="margin-bottom:10px;">
            <div class="info1">
            	<ul>
            		<%--<li><em>买家选择：</em><b> <asp:Literal runat="server" ID="litShippingModeName" /></b></li>--%>
            		<li><em>收货时间：</em><b> <asp:Label ID="litShipToDate" runat="server" Style="word-wrap: break-word; word-break: break-all;" /></b></li>
            	</ul>
            	
            </div>
            <div class="info2"><em>收货信息：</em>
            	<b>    
                            <asp:Literal runat="server" ID="litReceivingInfo" />
                    <!--    <td width="10%"><span class="Name"><a href="javascript:UpdateShippAddress('<%=Page.Request.QueryString["OrderId"] %>')">修改收货地址</a></span></td>-->
            	</b></div>

            <div class="info3"><em>买家留言：</em> <b><asp:Label ID="litRemark" runat="server" Style="word-wrap: break-word; word-break: break-all;" /></b></div>
            	
            	
                <!--<table width="100%" class="table table-striped">
                    <tr>
                        <td >订单编号：</td>
                        <td >
                            <asp:Label ID="lblOrderId" runat="server"></asp:Label></td>
                        <td >创建时间：</td>
                        <td >
                            <Hi:FormatedTimeLabel runat="server" ID="lblOrderTime"></Hi:FormatedTimeLabel></td>
                        <td >&nbsp;</td>
                        <td >&nbsp;</td>
                    </tr>
                </table>-->
            
        </div>

        <div class="list">
            <div class="Settlement">              
                <table width="100%" border="0" cellspacing="0" class="table">
                    <tr>
                        <td width="13%">发货方式：</td>
                        <td>
                            <asp:RadioButtonList runat="server" ClientIDMode="Static" CellPadding="150" RepeatDirection="Horizontal" ID="radio_sendGoodType">
                                <asp:ListItem Value="1" Selected="True"><span style="margin-right: 30px;">需要物流</span></asp:ListItem>
                                <asp:ListItem Value="0">无需物流</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="tr_wuliu_1">
                        <td width="13%"><em style="display:none;">*</em>物流公司：</td>
                        <td>
                            <Hi:ExpressDropDownList  ClientIDMode="Static" runat="server" CssClass="beautify_input" ID="expressRadioButtonList"></Hi:ExpressDropDownList>
                        	
                        </td>
                    </tr>
                    <tr id="tr_wuliu_2">
                        <td width="13%"><em style="display:none;">*</em>快递单号：</td>
                        <td >&nbsp;
                            <asp:TextBox runat="server" ID="txtShipOrderNumber"  Width="280px" class="forminput form-control" />
                            <span id="txtShipOrderNumberTip" runat="server"></span></td>
                    </tr>

                </table>
            </div>
        </div>

        <div class="blank12 clearfix"></div>
        <div class="list">
            <cc1:Order_ItemsList runat="server" ID="itemsList" ShowAllItem="false" />
        </div>
     
   <div class="modal_iframe_footer">
       <asp:Button ID="btnSendGoods" runat="server" Text="发货" OnClientClick="return checkNeedWuliu();" class="btn btn-primary" />
   </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        //PageIsValid();
        function checkNeedWuliu() {
            var need = $("#radio_sendGoodType_0").is(":checked");
            if (need) {
                var wuliuName = $("#select_ui_id_expressRadioButtonList .selectLt").html();
                var wuliuCode = $.trim($("#ctl00_contentHolder_txtShipOrderNumber").val());
                if (wuliuName == "请选择快递公司") {
                    alert("请选择快递公司");
                    return false;
                }
                if (wuliuCode == "") {
                    alert("运单号码不能为空，在1至20个字符之间");
                    return false;
                }
                return true;
            }
            return true;
        }
        function check() {
            return 
        }
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtShipOrderNumber', 1, 20, false, null, '运单号码不能为空，在1至20个字符之间'));
        }
        $(document).ready(function () {
            InitValidators();
            $('.list .table-striped').css("margin-top", "0px");

            $('input:radio[name="ctl00$contentHolder$radio_sendGoodType"]').change(function () {
                var need = $("input[name='ctl00$contentHolder$radio_sendGoodType']:checked").val();
                if (need == 1) {
                    $("#tr_wuliu_1").show();
                    $("#tr_wuliu_2").show();
                } else {
                    $("#tr_wuliu_1").hide();
                    $("#tr_wuliu_2").hide();
                }
            });

        });


        function UpdateShippAddress(ordernumber) {
            var pathurl = "sales/ShippAddress.aspx?action=update&orderId=" + ordernumber;
            DialogFrame(pathurl, "修改收货地址", 750, 400);
        }
    </script>
</asp:Content>
 