<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditPaymentType" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="paymenttypes.aspx">PC端</a></li>
                <li><a href="MobilePaymentSet">移动端</a></li>
                <li><a href="WxPaySetting">微信支付</a></li>
                <li class="hover"><a href="javascript:void">编辑</a></li>

            </ul>
        </div>
    </div>
    <div class="areacolumn  ">
        <div class="columnright">

            <div class="datafrom">
                <div class="formitem ">
                    <div class="blockquote-default blockquote-tip" style="margin: -10px 0px 30px 0px;">
                        <span style="color: red;">注意：</span>
                        因官方已取消 财付通担保交易、财付通双接口交易、支付宝标准双接口、支付宝担保交易、支付宝纯网关接口等支付方式，之前签约过这些支付方式的客户，如果遇到无法使用的情况请与支付官方联系。
                    </div>
                    <div class="clear">
                        <br />
                    </div>
                    <ul>
                        <li>
                            <span class="formitemtitle"><em>*</em>支付接口类型：</span>
                            <abbr class="formselect">
                                <select id="ddlPayments" name="ddlPayments" class="iselect_one"></select>
                            </abbr>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>支付方式名称：</span>
                            <asp:TextBox ID="txtName" runat="server" CssClass="forminput form-control" placeholder="名称不能为空，在1至60个字符之间"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtNameTip">&nbsp;</p>
                        </li>
                        <li style="display: none;" id="licert"><span class="formitemtitle"><em>*</em>上传证书：</span>
                            <asp:FileUpload ID="fileBankUnionCert" runat="server" />
                        </li>
                    </ul>
                </div>
            </div>
            <div class="datafrom2">
                <div class="formitem mt0">
                    <ul id="pluginContainer" class="attributeContent2">
                        <li rowtype="attributeTemplate" style="display: none;">
                            <span class="formitemtitle" style="display: block; float: left; font-size: 14px; text-align: right">$Name$：</span>
                            $Input$
                        </li>
                    </ul>
                </div>
            </div>
            <div class="datafrom">
                <div class="formitem mt0">
                    <ul>                    
                        <li id="liUsePrePay">
                            <span class="formitemtitle "><em>*</em>用于预付款充值：</span>
                            <Hi:OnOff runat="server" ID="radiIsUseInpour"></Hi:OnOff>
                            <p id="P1">用于预付款充值的支付方式最好是即时到帐的支付方式，方便立即查看预付款</p>
                        </li>
                        <li class="clearfix"><span class="formitemtitle">备注：</span>
                            <span>
                                <Hi:Ueditor ID="fcContent" runat="server" Width="660" />
                            </span>
                        </li>
                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnUpdate" runat="server" OnClientClick="return Save();" Text="保 存" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </div>
   <uc1:ImageList ID="ImageList" runat="server" />
    <asp:HiddenField runat="server" ID="txtSelectedName" />
    <asp:HiddenField runat="server" ID="txtConfigData" />
    <script type="text/javascript" src="/utility/plugin.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            pluginContainer = $("#pluginContainer");
            templateRow = $(pluginContainer).find("[rowType=attributeTemplate]");
            dropPlugins = $("#ddlPayments");
            selectedNameCtl = $("#<%=txtSelectedName.ClientID %>");
            configDataCtl = $("#<%=txtConfigData.ClientID %>");

            // 绑定支付接口类型列表
            $(dropPlugins).append($("<option value=\"\">-请选择接口类型-</option>"));
            $.ajax({
                url: "/PluginHandler?type=PaymentRequest&action=getlist",
                type: 'GET',
                async: false,
                dataType: 'json',
                timeout: 10000,
                success: function (resultData) {
                    if (resultData.qty == 0)
                        return;

                    $.each(resultData.items, function (i, item) {
                        if (item.FullName == $(selectedNameCtl).val())
                            $(dropPlugins).append($(String.format("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.FullName, item.DisplayName)));
                        else
                            $(dropPlugins).append($(String.format("<option value=\"{0}\">{1}</option>", item.FullName, item.DisplayName)));
                    });
                }
            });
            $(dropPlugins).attr("disabled", "disabled");

            if ($(selectedNameCtl).val().length > 0) {
                SelectPlugin("PaymentRequest");
            }
        });

        function Save() {
            if (PageIsValid()) {
                $(dropPlugins).removeAttr("disabled");
                return true;
            }
            return false;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

    <%--富文本编辑器start--%>
    <link rel="stylesheet" href="/Utility/Ueditor/css/dist/component-min.css" />
    <link rel="stylesheet" href="/Utility/Ueditor/plugins/uploadify/uploadify-min.css" />

    <script type="text/javascript" src="/Utility/Ueditor/js/dist/lib-min.js"></script>
    <script type="text/javascript" src="/Utility/Ueditor/js/config.js"></script>
    <script type="text/javascript" src="/Utility/Ueditor/plugins/uploadify/jquery.uploadify.min.js?ver=940"></script>
    <script type="text/javascript" src="/Utility/Ueditor/plugins/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="/Utility/Ueditor/plugins/ueditor/ueditor.all.min.js?v=3.0"></script>
    <script type="text/javascript" src="/Utility/Ueditor/plugins/ueditor/diy_imgpicker.js"></script>
    <script type="text/javascript" src="/Utility/Ueditor/js/dist/componentindex-min.js"></script>

    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <%--富文本编辑器end--%>
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            // 支付方式名称
            initValid(new InputValidator('ctl00_contentHolder_txtName', 1, 60, false, null, '支付方式名称不能为空，长度限制在1-60个字符之间'));
            
        }
        $(document).ready(function () { InitValidators(); });
    </script>

</asp:Content>
