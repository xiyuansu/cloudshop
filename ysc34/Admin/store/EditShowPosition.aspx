<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditShowPosition.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditShowPosition" %>

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
        <div class="columnright">
            <div class="formitem">
                <ul>
                    <li><span class="formitemtitle " style="width: 128px;"><em>*</em>显示位置：</span>
                        <abbr class="formselect">
                            <asp:RadioButtonList ID="radShowPosition" RepeatDirection="Horizontal" ClientIDMode="Static" RepeatLayout="Flow" runat="server" CssClass="icheck">
                                <asp:ListItem Value="1">左上</asp:ListItem>
                                <asp:ListItem Value="2">左下</asp:ListItem>
                                <asp:ListItem Value="3">右上</asp:ListItem>
                                <asp:ListItem Value="4">右下</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:HiddenField runat="server" ID="hidShowPosition" ClientIDMode="Static" />
                        </abbr>
                    </li>
                    <li><span class="formitemtitle " style="width: 128px;"><em>*</em>与网站顶部间隔：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtYPosition" runat="server" CssClass="forminput form-control" ClientIDMode="Static" Text="120" Width="150" placeholder="请输入数字"></asp:TextBox>
                            <span class="input-group-addon">PX</span>
                        </div>
                        <p id="txtYPositionTip" style="width: 180px; margin-left: 128px"></p>
                    </li>
                </ul>
                <div class="modal_iframe_footer">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="btn btn-primary inbnt" OnClientClick="return checkSubmit()" />
                </div>
            </div>
        </div>
    </div>

    <div id="divAddAccount" style="display: none;">
        <div class="areacolumn clearfix">
            <div class="columnright" style="width: 500px;">

                <div class="formitem">
                    <ul>
                        <li><span class="formitemtitle" style="width: 100px;"><em>*</em>客服类型：</span>
                            <abbr class="formselect">
                                <Hi:ServiceTypeDropDownList runat="server" ClientIDMode="Static" AllowNull="true" ID="dropServiceType" CssClass="pull-left" Height="32" Style="border: 1px solid #e4e4e4"></Hi:ServiceTypeDropDownList>
                                <asp:HiddenField runat="server" ID="hidServiceType" Value="1" ClientIDMode="Static" />
                            </abbr>
                        </li>
                        <li class="mb_0"><span class="formitemtitle " style="width: 100px;"><em>*</em>帐号：</span>
                            <asp:TextBox ID="txtAccount" runat="server" ClientIDMode="Static" CssClass="forminput form-control" placeholder="请填写正确的QQ/阿里巴巴账号"></asp:TextBox>
                            <p id="txtAccountTip" runat="server">&nbsp;</p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle " style="width: 100px;"><em>*</em>昵称：</span>
                            <asp:TextBox ID="txtNickName" runat="server" ClientIDMode="Static" CssClass="forminput form-control" placeholder="不可为空"></asp:TextBox>
                            <p id="txtNickNameTip" runat="server">&nbsp;</p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle " style="width: 100px;"><em>*</em>排序：</span>
                            <asp:TextBox ID="txtOrderId" ClientIDMode="Static" runat="server" CssClass="forminput form-control" placeholder="不可为空，数字越小越靠前"></asp:TextBox>
                            <p id="txtOrderIdTip" runat="server">&nbsp;</p>
                        </li>
                        <li style="display: none;">
                            <span class="formitemtitle " style="width: 100px;">图片类型：</span>
                            <asp:TextBox ID="txtImageType" ClientIDMode="Static" runat="server" Text="1" CssClass="forminput form-control"></asp:TextBox>
                        </li>
                        <li><span class="formitemtitle " style="width: 100px;">是否显示：</span>
                            <Hi:YesNoRadioButtonList ID="radioShowStatus" ClientIDMode="Static" RepeatLayout="Flow" runat="server" />
                            <%--<Hi:OnOff runat="server" ID="radioShowStatus"></Hi:OnOff>--%>
                            <asp:HiddenField runat="server" ID="hidShowStatus" Value="True" ClientIDMode="Static" />
                        </li>
                    </ul>
                </div>

            </div>
        </div>
    </div>

    <div style="display: none;">
        <asp:HiddenField runat="server" ID="hidID" ClientIDMode="Static" />
        <asp:Button ID="btnSubmit" runat="server" Text="添 加" OnClientClick="return checkSubmit()" CssClass="btn btn-primary" />
    </div>
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>

    <script language="javascript" type="text/javascript">

        $(document).ready(function (e) {
            AdjustPositionInitValidators();
        });
        function AdjustPositionInitValidators() {
            var chkval = $('input:radio[name="ctl00$contentHolder$radShowPosition"]:checked').val();
            chkval = parseInt(chkval);
            $("#hidShowPosition").val(chkval);
            if (chkval == 1 || chkval == 3)
                $("#yPositionSpan").html("与网站顶部间隔：");
            else
                $("#yPositionSpan").html("与网站底部间隔：");
            $("#radShowPosition input").click(function (e) {
                $("#hidShowPosition").val($(this).val());
                switch (parseInt($(this).val())) {
                    case 1:
                    case 3:
                        $("#yPositionSpan").html("与网站顶部间隔：");
                        $("#txtYPosition").val("120");
                        break;
                    case 2:
                    case 4:
                        $("#yPositionSpan").html("与网站底部间隔：");
                        $("#txtYPosition").val("90");
                        break;
                }
            });
            initValid(new InputValidator('txtYPosition', 1, 5, false, '^[0-9]*$', '不能为空，且与网站顶部间隔只能输入数字'));
        }


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
        function validatorForm() {
            if (!PageIsValid())
                return false;

            return true;
        }
    </script>
</asp:Content>
