<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="StoresPermissions.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.StoresPermissions" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />

    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="StoresList.aspx">管理</a></li>
                <li class="hover"><a>权限</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem validator1 p-100 setorder">
                <ul>
                    <li class="mb_0"><span class="formitemtitle">门店名称：</span>
                        <div class="input-group">
                            <Hi:HiLabel runat="server" ID="lblStoreName" ClientIDMode="Static" style="font-size: 14px;"></Hi:HiLabel>
                        </div>
                        <p></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle">自行上架商品：</span>
                        <div class="input-group">
                            <Hi:OnOff runat="server" ID="IsShelvesProduct" ClientIDMode="Static"></Hi:OnOff>
                        </div>
                        <p style="font-size: 14px; margin-top: 5px; margin-bottom: 10px; color: #999;">开启后该门店可自行同步上架商城内的商品并设置库存</p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle">修改商品价格：</span>
                        <div class="input-group">
                            <Hi:OnOff runat="server" ID="IsModifyPrice" ClientIDMode="Static"></Hi:OnOff>
                        </div>
                        <p style="font-size: 14px; margin-top: 5px; margin-bottom: 10px; padding-bottom: 15px; color: #999;">开启后该门店将具备商品价格的修改权限，可自行修改该门店对商品的售卖价格</p>
                    </li>
                    <li class="mb_0" style="display: none;" id="liprice"><span class="formitemtitle">价格修改区间：</span>
                        <div class="input-group" style="font-size: 14px;">
                            <font style="float: left;">&nbsp;&nbsp;平台价格的&nbsp;</font>
                            <asp:TextBox ID="txtMinPriceRate" runat="server" Width="60px" CssClass="form-control" />
                            <font style="float: left;">&nbsp;倍&nbsp&nbsp--&nbsp&nbsp</font>
                            <asp:TextBox ID="txtMaxPriceRate" runat="server" Width="60px" CssClass="form-control" />
                            <font style="float: left;">&nbsp;倍</font>
                        </div>
                        <p style="font-size: 14px; margin-top: 5px; margin-bottom: 10px; color: #999;">对平台修改门店商品价格同样有限制，可设置小数</p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle">允许门店提现：</span>
                        <div class="input-group">
                            <Hi:OnOff runat="server" ID="IsRequestBlance" ClientIDMode="Static"></Hi:OnOff>
                        </div>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button ID="btnOK" runat="server" Text="保存" CssClass="btn btn-primary" OnClientClick="return doSubmit();" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function doSubmit() {
            if ($("#IsModifyPrice input").is(':checked')) {
                var minPriceRate = $("#ctl00_contentHolder_txtMinPriceRate").val();
                if (isNaN(minPriceRate) || parseFloat(minPriceRate) > 100 || parseFloat(minPriceRate) < 0) {
                    $("#ctl00_contentHolder_txtMinPriceRate").focus();
                    ShowMsg("请输入正确的最小价格倍数！", false);
                    return false;
                }
                var maxPriceRate = $("#ctl00_contentHolder_txtMaxPriceRate").val();
                if (isNaN(maxPriceRate) || parseFloat(maxPriceRate) > 100 || parseFloat(maxPriceRate) < 0) {
                    $("#ctl00_contentHolder_txtMaxPriceRate").focus();
                    ShowMsg("请输入正确的最大价格倍数！", false);
                    return false;
                }
                if (parseFloat(minPriceRate) > parseFloat(maxPriceRate)) {
                    $("#ctl00_contentHolder_txtMaxPriceRate").focus();
                    ShowMsg("最大价格倍数需大于最小价格倍数！", false);
                    return false;
                }
                var regPoint = /^\d+(\.\d{1,2})?$/;
                if (minPriceRate>0&&(!regPoint.test(minPriceRate))) {
                    ShowMsg("最多只能保留两位小数！", false);
                    return false;
                }
                if (maxPriceRate>0&&(!regPoint.test(maxPriceRate))) {
                    ShowMsg("最多只能保留两位小数！", false);
                    return false;
                }
            }
            return true;
        }
        function fuCheckEnablePrice(event, state) {
            if (state) {
                $("#liprice").show();
            }
            else {
                $("#liprice").hide();
            }
        }
        $(document).ready(function () {
            if ($("#IsModifyPrice input").is(':checked')) {
                $("#liprice").show();
            }
            else {
                $("#liprice").hide();
            }
        });
    </script>
</asp:Content>

