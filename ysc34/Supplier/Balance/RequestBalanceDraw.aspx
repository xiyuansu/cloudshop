<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="RequestBalanceDraw.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.Balance.RequestBalanceDraw" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        var minDraws = 0;
        function checkData() {
            var Amount = $("#ctl00_contentHolder_txtAmount");
            if (Amount.val() == "") {
                alert("请输入提现金额"); Amount.focus(); return false;
            }
            else if (!(/^(([1-9]\d{0,6})|\d)(\.\d{1,2})?$/).test(Amount.val())) {
                alert("提现金额格式错误，请输入最小提现金额至1000万以内的数字(仅可保留两位小数)"); Amount.focus(); Amount.select(); return false;
            } else if (parseFloat($.trim($("#spanMindraws").html()) == "" ? "0" : $.trim($("#spanMindraws").html())) > parseFloat(Amount.val())) { alert('请输入提现金额大于或者等于单次提现最小限额'); return false; }

            if ($("#ctl00_contentHolder_IsAlipay").is(':checked')) {
                var AlipayRealName = $("#ctl00_contentHolder_txtAlipayRealName");
                if (AlipayRealName.val() == "") {
                    alert("请输入支付宝真实姓名"); AlipayRealName.focus(); return false;
                }
                var AlipayCode = $("#ctl00_contentHolder_txtAlipayCode");
                if (AlipayCode.val() == "") {
                    alert("请输入支付宝账号"); AlipayCode.focus(); return false;
                }
            }

            if ($("#ctl00_contentHolder_IsDefault").is(':checked')) {
                var BankName = $("#ctl00_contentHolder_txtBankName");
                if (BankName.val() == "") {
                    alert("请输入开户银行名称"); BankName.focus(); return false;
                }
                var AccountName = $("#ctl00_contentHolder_txtAccountName");
                if (AccountName.val() == "") {
                    alert("请输入银行开户名"); AccountName.focus(); return false;
                }
                var MerchantCode = $("#ctl00_contentHolder_txtMerchantCode");
                if (MerchantCode.val() == "") {
                    alert("请输入提现账号"); MerchantCode.focus(); return false;
                }
                else if (!(/^(\d{10,21})$/).test(MerchantCode.val())) {
                    alert("提现账号只可以为10到21位的数字"); MerchantCode.focus(); MerchantCode.select(); return false;
                }
            }
            var TradePassword = $("#ctl00_contentHolder_txtTradePassword");
            if (TradePassword.val() == "") {
                alert("请输入交易密码"); TradePassword.focus(); return false;
            }
            return true;
        }
        $(document).ready(function () {
          
            if ($("#ctl00_contentHolder_btnDrawNext").val()) { } else { $(".yzyouxiang_box1").hide(); }

            if ($("#ctl00_contentHolder_IsDefault").is(':checked')) { $(".alipay").hide(); $(".IsDefault").show(); }
            if ($("#ctl00_contentHolder_IsWeixin").is(':checked')) {
                $(".alipay").hide(); $(".IsWeixin").show();
            }
            if ($("#ctl00_contentHolder_IsAlipay").is(':checked')) { $(".alipay").show(); $(".IsAlipay").show(); }

            $("#ctl00_contentHolder_IsDefault").click(function () {
                $(".RInfo").each(function () {
                    var Txt = $(this).find("input");
                    if (Txt.val() == "") { Txt.val("0"); }
                })
                $(".IsDefault").each(function () {
                    var Txt = $(this).find("input");
                    if (Txt.val() == "0") { Txt.val(""); }
                })
                $(".bank").show(); $(".alipay").hide();
                if (minDraws > 0) {
                    $("#spanMindraws").html(minDraws);
                }
            })
            $("#ctl00_contentHolder_IsWeixin").click(function () {
                $(".RInfo").each(function () {
                    var Txt = $(this).find("input");
                    if (Txt.val() == "") { Txt.val("0"); }
                })
                $(".IsWeixin").each(function () {
                    var Txt = $(this).find("input");
                    if (Txt.val() == "0") { Txt.val(""); }
                })

                $(".RInfo").hide(); $(".IsWeixin").show();
                var Amount = parseFloat($("#spanMindraws").html());
                if (isNaN(Amount)) {
                    Amount = 0;
                }
                minDraws = Amount;
                $("#spanMindraws").html(Amount > 1 ? Amount.toFixed(2) : "1.00");
            })
            $("#ctl00_contentHolder_IsAlipay").click(function () {
                $(".RInfo").each(function () {
                    var Txt = $(this).find("input");
                    if (Txt.val() == "") { Txt.val("0"); }
                })
                $(".IsAlipay").each(function () {
                    var Txt = $(this).find("input");
                    if (Txt.val() == "0") { Txt.val(""); }
                })

                $(".alipay").show();
                $(".bank").hide();
                if (minDraws > 0) {
                    $("#spanMindraws").html(minDraws);
                }
            })

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li>
                        <h2 class="colorE">申请提现</h2>
                    </li>
                    <li><span class="formitemtitle">用户名：</span>
                        <asp:Literal ID="litUserName" runat="server" />
                    </li>
                    <li><span class="formitemtitle">可提现余额：</span>
                        <Hi:FormatedMoneyLabel ID="lblBanlance" runat="server" />&nbsp&nbsp 元
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>提现金额：</span>
                        <asp:TextBox ID="txtAmount" runat="server" class="forminput form-control" /></span>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle">&nbsp;</span> <em>&nbsp;</em><samp>注意：</samp>每次最少提现
                        <samp id="spanMindraws">
                            <asp:Literal ID="lblminDraws" runat="server" /></samp>
                        元
                    </li>

                    <li><span class="formitemtitle">提现方式：</span>
                        <asp:RadioButton runat="server" ID="IsDefault" GroupName="DrawRequestType" Text="银行卡转账" />
                        <asp:RadioButton runat="server" ID="IsAlipay" GroupName="DrawRequestType" Text="支付宝支付" />

                    </li>
                    <li class="alipay">
                        <div class="RInfo IsAlipay"><span class="formitemtitle"><em>*</em>真实姓名：</span><asp:TextBox ID="txtAlipayRealName" runat="server" class="forminput form-control" MaxLength="30" /></div>
                    </li>
                    <li class="alipay">
                        <div class="RInfo IsAlipay">
                            <span class="formitemtitle"><em>*</em>收款账号：</span><asp:TextBox ID="txtAlipayCode" runat="server" class="forminput form-control" />
                        </div>
                    </li>
                    <li class="bank">
                        <div class="RInfo IsDefault">
                            <span class="formitemtitle"><em>*</em>开户银行：</span><asp:TextBox ID="txtBankName" runat="server" class="forminput form-control" MaxLength="60" />
                        </div>
                    </li>
                    <li class="bank">
                        <div class="RInfo IsDefault">
                            <span class="formitemtitle"><em>*</em>银行开户名：</span><asp:TextBox ID="txtAccountName" runat="server" class="forminput form-control" MaxLength="30" />
                        </div>
                    </li>
                    <li class="bank">
                        <div class="RInfo IsDefault">
                            <span class="formitemtitle"><em>*</em>提现账号：</span><asp:TextBox ID="txtMerchantCode" runat="server" MaxLength="100" class="forminput form-control" />
                        </div>
                    </li>
                    <li><span class="formitemtitle">备注：</span><asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" autocomplete="off" class="forminput form-control" MaxLength="100" />
                    </li>
                    <li>
                        <input style="display: none" /></li>
                    <!--阻止谷歌浏览器自动填充密码-->
                    <li><span class="formitemtitle"><em>*</em>交易密码：</span>
                        <asp:TextBox ID="txtTradePassword" runat="server" autocomplete="off" TextMode="Password" class="forminput form-control" />
                    </li>
                    <li>
                        <asp:Button ID="btnDrawNext" runat="server" Text="下一步" CssClass="btn btn-primary ml_198" OnClick="btnDrawNext_Click" OnClientClick="return checkData()" />

                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
