<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddOnlineService.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddOnlineService" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="OnlineService.aspx" >管理</a></li>
                <li class="hover"><a href="javascript:void" >添加</a></li>
            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">

            <div class="formitem validator4">
                <ul>
                    <li><span class="formitemtitle"><em>*</em>在线客服类型：</span>
                        <abbr class="formselect">
                            <Hi:ServiceTypeDropDownList runat="server" AllowNull="true" ID="dropServiceType" CssClass="float"></Hi:ServiceTypeDropDownList>
                        </abbr>
                        <p id="dropServiceTypeTip" runat="server" ></p>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>帐号：</span>
                        <asp:TextBox ID="txtAccount" runat="server" CssClass="forminput form-control"></asp:TextBox>
                        <p id="txtAccountTip" runat="server"></p>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>昵称：</span>
                        <asp:TextBox ID="txtNickName" runat="server" CssClass="forminput form-control"></asp:TextBox>
                        <p id="txtNickNameTip" runat="server"  ></p>
                    </li>
                    <li><span class="formitemtitle">排序：</span>
                        <asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput form-control"></asp:TextBox>
                        <p id="txtOrderIdTip" runat="server" > </p>
                    </li>
                    <li style="display: none;">
                        <span class="formitemtitle">图片类型：</span>
                        <asp:TextBox ID="txtImageType" runat="server" Text="1" CssClass="forminput form-control"  ></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle">是否显示：</span>
                         <Hi:OnOff runat="server" ID="radioShowStatus"></Hi:OnOff>
                        <p id="radioShowStatusTip" runat="server" ></p>
                    </li>
                </ul>
                <div class="ml_198">
                    <li>
                        <asp:Button ID="btnSubmit" runat="server" Text="添 加" OnClientClick="return SubmitCheck()" CssClass="btn btn-primary" />

                    </li>
                </div>
            </div>

        </div>
    </div>
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <style type="text/css">
        .validator4 li p
        {
            height:auto;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtAccount', 0, 50, false, null, '客服帐号不能为空，且长度限制在50个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtNickName', 0, 50, false, null, '昵称不能为空，且长度限制在50个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtOrderId', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，只能输入整数型数值'));
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtOrderId', -100000000, 100000000, '输入的数值超出了系统表示范围'));

        }

        function SubmitCheck() {
            if (PageIsValid()) {
                var ServiceType = parseInt($("#ctl00_contentHolder_dropServiceType").val());

                if (isNaN(ServiceType) || ServiceType == 0) {
                    //$("#ctl00_contentHolder_dropServiceType").addClass("errorFocus");
                    $("#ctl00_contentHolder_dropServiceTypeTip").html("请选择在线客服类型").removeClass().addClass("msgError");
                    return false;
                }
                else {
                    $("#ctl00_contentHolder_dropServiceTypeTip").removeClass();
                }
                var Account = $("#ctl00_contentHolder_txtAccount").val();
                var re = /^[1-9][0-9]{5,10}$/;
                if (ServiceType == 2) {
                    re = /^[_a-zA-Z0-9\u4e00-\u9fa5]{3,50}$/;
                }
                if (!re.test(Account)) {
                    $("#ctl00_contentHolder_txtAccount").addClass("errorFocus");
                    $("#ctl00_contentHolder_txtAccountTip").removeClass().addClass("msgError");
                    if (ServiceType == 1)
                        $("#ctl00_contentHolder_txtAccountTip").html("请输入正确的QQ帐号").removeClass().addClass("msgError");
                    else
                        $("#ctl00_contentHolder_txtAccountTip").html("请输入正确的旺旺帐号").removeClass().addClass("msgError");
                    return false;
                }
                else {
                    $("#ctl00_contentHolder_txtAccount").removeClass("errorFocus");
                    $("#ctl00_contentHolder_txtAccountTip").removeClass("msgError");
                }
                return true;

            }
            else {
                return false;
            }
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>
