<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddAccount.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddAccount" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="Server">
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
    </style>
    <div class="areacolumn clearfix">
        <div class="columnright" style="width: 500px;">

            <div class="formitem">
                <ul>
                    <li><span class="formitemtitle" style="width: 100px;"><em>*</em>客服类型：</span>
                        <abbr class="formselect">
                            <Hi:ServiceTypeDropDownList runat="server" ClientIDMode="Static" AllowNull="true" ID="dropServiceType" CssClass="iselect" Height="32" Style="border: 1px solid #e4e4e4"></Hi:ServiceTypeDropDownList>
                            <asp:HiddenField runat="server" ID="hidServiceType" Value="1" ClientIDMode="Static" />
                        </abbr>
                    </li>
                    <li class="mb_0"><span class="formitemtitle " style="width: 100px;"><em>*</em>帐号：</span>
                        <asp:TextBox ID="txtAccount" runat="server" ClientIDMode="Static" CssClass="forminput form-control" placeholder="请填写正确的QQ/阿里巴巴账号"></asp:TextBox>
                        <p id="txtAccountTip" style="margin-left:101px;">&nbsp;</p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle " style="width: 100px;"><em>*</em>昵称：</span>
                        <asp:TextBox ID="txtNickName" runat="server" ClientIDMode="Static" CssClass="forminput form-control" placeholder="不可为空"></asp:TextBox>
                        <p id="txtNickNameTip" style="margin-left:101px;">&nbsp;</p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle " style="width: 100px;"><em>*</em>排序：</span>
                        <asp:TextBox ID="txtOrderId" ClientIDMode="Static" runat="server" CssClass="forminput form-control" placeholder="不可为空，数字越大越靠前"></asp:TextBox>
                        <p id="txtOrderIdTip" style="margin-left:101px;">&nbsp;</p>
                    </li>
                    <li><span class="formitemtitle " style="width: 100px;">是否显示：</span>
                        <Hi:OnOff runat="server" ID="ooShowStatus"></Hi:OnOff>
                        <asp:HiddenField runat="server" ID="hidShowStatus" Value="True" ClientIDMode="Static" />
                    </li>
                </ul>
                <div class="modal_iframe_footer">
                    <asp:HiddenField runat="server" ID="hidID" ClientIDMode="Static" />
                    <asp:Button ID="btnSubmit" runat="server" Text="添 加" OnClientClick="return checkSubmit()" CssClass="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>

    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function (e) {
            AddAccountInitValidators();
        });
        function AddAccountInitValidators() {
            initValid(new InputValidator('txtAccount', 0, 50, false, null, '客服帐号不能为空，且长度在50字内'));
            initValid(new InputValidator('txtNickName', 0, 50, false, null, '昵称不能为空，且长度在50字内'));
            initValid(new InputValidator('txtOrderId', 1, 10, false, '-?[0-9]\\d*', '排序不可为空，且只能输入整数'));
            appendValid(new MoneyRangeValidator('txtOrderId', -100000000, 100000000, '输入的数值超出了系统表示范围'));

            $("#dropServiceType").change(function () {
                $("#hidServiceType").val($(this).val());
            })
            $("#radioShowStatus input").click(function (e) {
                $("#hidShowStatus").val($(this).val());
            });
        }

        function checkSubmit() {
            if (!PageIsValid())
                return false;

            return true;
        }

    </script>
</asp:Content>
