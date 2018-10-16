<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditMemberPrices.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.EditMemberPrices" Title="无标题页" %>

<%@ Import Namespace="Hidistro.Core" %>
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
        .iselect {
            width:100px;
        }
        #ascrail2000-hr{
            bottom:75px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div style="color:red; margin-bottom:20px" runat="server" id="Msgtips">您选择的部分商品正在参与活动，活动期间无法调整价格，请结束活动后再修改!</div>
        <div class="searcharea mb_0 a_none">
            <ul>
                
                <li>

                    <span>直接调价：</span>
                    <Hi:MemberPriceDropDownList ID="ddlMemberPrice" runat="server" AllowNull="false" CssClass="iselect mywidth" Width="80"  />
                    <span>&nbsp;=&nbsp;</span>
                    <asp:TextBox ID="txtTargetPrice" CssClass="forminput form-control" runat="server" Width="80px" />
                </li>
                <li>
                    <asp:Button ID="btnTargetOK" runat="server" Text="确定" CssClass="btn btn-primary" /></li>
            </ul>
            <ul>
                <li><span>公式调价：</span>
                    <Hi:MemberPriceDropDownList ID="ddlMemberPrice2" runat="server" AllowNull="false" CssClass="iselect" Width="80" />
                    <span>&nbsp;=&nbsp;</span>
                    <Hi:MemberPriceDropDownList ID="ddlSalePrice" runat="server" AllowNull="false" CssClass="iselect" Width="80"  /> <span>&nbsp;&nbsp;</span>
                    <Hi:OperationDropDownList ID="ddlOperation" runat="server" AllowNull="false" CssClass="iselect"  Style="margin-right:5px;" /><span>&nbsp;&nbsp;</span>
                    <asp:TextBox ID="txtOperationPrice" CssClass="forminput form-control" runat="server" Width="80px" />
                </li>
                <li>
                    <asp:Button ID="btnOperationOK" runat="server" Text="确定" CssClass="btn btn-primary" /></li>
            </ul>
        </div>
        <div class="datalist clearfix" class="table-striped">
            <Hi:GridSkuMemberPriceTable runat="server" />
        </div>
        <div class="modal_iframe_footer">
            <Hi:TrimTextBox runat="server" ID="txtPrices" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
            <asp:Button ID="btnSavePrice" runat="server" OnClientClick="return loadSkuPrice();" Text="保存" CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function loadSkuPrice() {
            if (!checkPrice())
                return false;

            var skuPriceXml = "<xml><skuPrices>";
            $.each($(".SkuPriceRow"), function () {
                var skuId = $(this).attr("skuId");
                var costPrice = $("#tdCostPrice_" + skuId).val();
                var salePrice = $("#tdSalePrice_" + skuId).val();
                var itemXml = String.format("<item skuId=\"{0}\" costPrice=\"{1}\" salePrice=\"{2}\">", skuId, costPrice, salePrice);
                itemXml += "<skuMemberPrices>";

                $(String.format("input[type='text'][name='tdMemberPrice_{0}']", skuId)).each(function (rowIndex, rowItem) {
                    var id = $(this).attr("id");
                    var gradeId = id.substring(0, id.indexOf("_"));
                    var memberPrice = $(this).val();
                    if (memberPrice != "")
                        itemXml += String.format("<priceItme gradeId=\"{0}\" memberPrice=\"{1}\" \/>", gradeId, memberPrice);
                });

                itemXml += "<\/skuMemberPrices>";
                itemXml += "<\/item>";
                skuPriceXml += itemXml;
            });
            skuPriceXml += "<\/skuPrices><\/xml>";
            $("#ctl00_contentHolder_txtPrices").val(skuPriceXml);
            return true;
        }

        function checkPrice() {
            var validated = true;
            var exp = new RegExp("^(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)$", "i");

            $.each($(".SkuPriceRow"), function () {
                var skuId = $(this).attr("skuId");
                var costPrice = $("#tdCostPrice_" + skuId).val();
                var salePrice = $("#tdSalePrice_" + skuId).val();

                // 检查必填项是否填了
                if (salePrice.length == 0) {
                    alert("商品规格的一口价为必填项！");
                    $("#tdSalePrice_" + skuId).focus();
                    validated = false;
                    return false;
                }

                if (!exp.test(salePrice)) {
                    alert("商品规格的一口价输入有误");
                    $("#tdSalePrice_" + skuId).focus();
                    validated = false;
                    return false;
                }

                var num = parseFloat(salePrice);
                if (num > 10000000 || num <= 0) {
                    alert("商品规格的一口价超出了系统表示范围！");
                    $("#tdSalePrice_" + skuId).focus();
                    validated = false;
                    return false;
                }

                if (costPrice.length > 0) {
                    // 检查输入的是否是有效的金额
                    if (!exp.test(costPrice)) {
                        alert("商品规格的成本价输入有误！");
                        $("#tdCostPrice_" + skuId).focus();
                        validated = false;
                        return false;
                    }

                    // 检查金额是否超过了系统范围
                    var num = parseFloat(costPrice);
                    if (num > 10000000 || num < 0) {
                        alert("商品规格的成本价超出了系统表示范围！");
                        $("#tdCostPrice_" + skuId).focus();
                        validated = false;
                        return false;
                    }
                }

                $(String.format("input[type='text'][name='tdMemberPrice_{0}']", skuId)).each(function (rowIndex, rowItem) {
                    var id = $(this).attr("id");
                    var memberPrice = $(this).val();
                    if (memberPrice.length > 0) {
                        // 检查输入的是否是有效的金额
                        if (!exp.test(memberPrice)) {
                            alert("商品规格的会员等级价输入有误！");
                            $(this).focus();
                            validated = false;
                            return false;
                        }

                        // 检查金额是否超过了系统范围
                        var num = parseFloat(memberPrice);
                        if (!((num >= 0.01) && (num <= 10000000))) {
                            alert("商品规格的会员等级价超出了系统表示范围！");
                            $(this).focus();
                            validated = false;
                            return false;
                        }
                    }
                    if (validated == false)
                        return false;
                });
                if (validated == false)
                    return false;
            });

            return validated;
        }
    </script>
</asp:Content>
