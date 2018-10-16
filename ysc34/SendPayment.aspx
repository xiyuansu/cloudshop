<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendPayment.aspx.cs" Inherits="Hidistro.UI.Web.SendPayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择要支付的银行</title>
    <style type="text/css">
        table td { height: 30px; line-height: 30px; font-size: 15px; width: 16%; }

        h2 { color: #e6b650; width: 100%; height: 40px; font-size: 18px; background: #eeeeee; line-height: 40px; border-bottom: 2px solid #ff0000; margin-top: 50px; }

        .button { display: block; font-size: 12px; text-decoration: none !important; font-family: Helvetica, Arial, sans serif; padding: 8px 12px; border-radius: 3px; -moz-border-radius: 3px; box-shadow: inset 0px 0px 2px #fff; -o-box-shadow: inset 0px 0px 2px #fff; -webkit-box-shadow: inset 0px 0px 2px #fff; -moz-box-shadow: inset 0px 0px 2px #fff; }

            .button:active { box-shadow: inset 0px 0px 3px #999; -o-box-shadow: inset 0px 0px 3px #999; -webkit-box-shadow: inset 0px 0px 3px #999; -moz-box-shadow: inset 0px 0px 3px #999; }
        /* The styles for the yellow button */
        .yellow { color: #986a39; border: 1px solid #e6b650; background-image: -moz-linear-gradient(#ffd974, #febf4d); background-image: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#febf4d), to(#ffd974)); background-image: -webkit-linear-gradient(#ffd974, #febf4d); background-image: -o-linear-gradient(#ffd974, #febf4d); text-shadow: 1px 1px 1px #fbe5ac; background-color: #febf4d; }

            .yellow:hover { border: 1px solid #c1913d; background-image: -moz-linear-gradient(#febf4d, #ffd974); background-image: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#ffd974), to(#febf4d)); background-image: -webkit-linear-gradient(#febf4d, #ffd974); background-image: -o-linear-gradient(#febf4d, #ffd974); background-color: #ffd974; }

            .yellow:active { border: 1px solid #936b26; }

        .red { color: red; }
    </style>
    <script src="Utility/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function (e) {
            $("#btnSubmit").click(function (e) {
                var val = $('input:radio[name="radBankCode"]:checked').val();
                if (val == null || val == undefined || val == "") {
                    alert("请选择要支付的银行!");
                    return false;
                }
                else {
                    $("#txtBankCode").val(val);
                    return true;

                }
            });
        });
    </script>
</head>
<body>
    
    <div id="RedirectToPayDIV" runat="server">
        <p>支付失败,请联系商城客服</p>
    </div>
    <div id="ChoiceBankDIV" runat="server" style="width: 990px; margin: 0 auto;">
        <h2>请选择支付银行</h2>
        <asp:HiddenField ID="txtBankCode" runat="server" ClientIDMode="Static" />
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <input type="radio" name="radBankCode" value="BOCB2C" />中国银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="ICBCB2C" />中国工商银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="CMB" />招商银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="CCB" />中国建设银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="ABC" />中国农业银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="SPDB" />上海浦东发展银行</td>
            </tr>
            <tr>
                <td>
                    <input type="radio" name="radBankCode" value="CIB" />兴业银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="GDB" />广发银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="FDB" />富滇银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="HZCBB2C" />杭州银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="SHBANK" />上海银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="NBBANK" />宁波银行</td>
            </tr>
            <tr>
                <td>
                    <input type="radio" name="radBankCode" value="SPABANK" />平安银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="POSTGC" />中国邮政储蓄银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="SHRCB" />上海农商银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="WZCBBZC-BEBIT" />温州银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="SPABANK" />平安银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="BJBANK" />北京银行</td>
            </tr>
            <tr>
                <td>
                    <input type="radio" name="radBankCode" value="SHRCB" />上海农商银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="WZCBBZC-BEBIT" />温州银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="COMM" />交通银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="CMBC" />中国民生银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="BJRCB" />北京农村商业银行</td>
                <td>
                    <input type="radio" name="radBankCode" value="CITIC-DEBIT" />中信银行</td>
            </tr>
            <tr>
                <td colspan="5"></td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Button ID="btnSubmit" ClientIDMode="Static" runat="server" Text="确认支付" CssClass="button yellow" OnClick="btnSubmit_Click" /></td>
            </tr>
        </table>
    </div>

</body>
</html>
