<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="LotteryDrawSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.LotteryDrawSet" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            InitValidators();
            function setShowHide() {
                var t = $(this).val();
                index = $(this).attr("index");
                if (t == "0") {
                    $("#ctl00_contentHolder_txtPoints" + index).closest('.input-group').show();
                    $("#ctl00_contentHolder_txtPoints" + index).show();
                    $("#ctl00_contentHolder_ddlCoupons" + index).closest('.iselect').hide();
                }
                if (t == "1") {
                    $("#ctl00_contentHolder_txtPoints" + index).closest('.input-group').show();
                    $("#ctl00_contentHolder_txtPoints" + index).show();
                    $("#ctl00_contentHolder_ddlCoupons" + index).closest('.iselect').hide();
                }
                if (t == "2") {
                    $("#ctl00_contentHolder_txtPoints" + index).closest('.input-group').hide()
                    $("#ctl00_contentHolder_ddlCoupons" + index).closest('.iselect').show();
                    $("#ctl00_contentHolder_ddlCoupons" + index).show();
                }
            }
            $("select").each(setShowHide);
            $("select").change(setShowHide);
        });
        function InitValidators() {
            <%--initValid(new InputValidator('<%=txtPoints.ClientID%>', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));--%>
            initValid(new InputValidator('<%=txtDepletePoints.ClientID%>', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '消耗积分数必须是数并且数值在1-10000000之间'));
            appendValid(new NumberRangeValidator('<%=txtDepletePoints.ClientID%>', 1, 10000000, '消耗积分数必须在1-10000000之间'));
            initValid(new InputValidator('<%=txtPercent1.ClientID%>', 1, 10, false, '[0-9]\\d*', '数据类型错误，只能输入正整数型数值'));
            initValid(new InputValidator('<%=txtPercent2.ClientID%>', 1, 10, false, '[0-9]\\d*', '数据类型错误，只能输入正整数型数值'));
            initValid(new InputValidator('<%=txtPercent3.ClientID%>', 1, 10, false, '[0-9]\\d*', '数据类型错误，只能输入正整数型数值'));
            initValid(new InputValidator('<%=txtPercent4.ClientID%>', 1, 10, false, '[0-9]\\d*', '数据类型错误，只能输入正整数型数值'));
        }
    </script>
    <script type="text/javascript">
        function fuOnOffEnableShake(event, state) {
            if (state) {
                $("#divContent").show();
            }
            else {
                $("#divContent").hide();
            }
        }

        $(document).ready(function () {
            if ($("#ctl00_contentHolder_onoffEnableShake input").is(':checked')) {
                $("#divContent").show();
            }
            else {
                $("#divContent").hide();
            }

            //$('#ctl00_contentHolder_onoffEnableShake').on('switch-change', function (e, data) {
            //    alert(data);
            //});
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
            <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <div class="dataarea mainwidth databody">


        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                    <li><span class="formitemtitle" style="width:115px;">开启摇一摇：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="onoffEnableShake"></Hi:OnOff>
                        </abbr>
                    </li>
                </ul>
            </div>
            <div class="formitem validator1">
                <div class="formitem ld-set" id="divContent">
                    <%--<dl>
                            <dt>每日签到所得积分：</dt>
                            <dd><asp:TextBox ID="txtPoints" runat="server" Width="150" CssClass="forminput"></asp:TextBox></dd>
                        </dl>--%>
                    <dl>
                        <dt>每次摇奖消耗积分：</dt>
                        <dd>
                            <asp:TextBox ID="txtDepletePoints" runat="server" Width="150" placeholder="请填写消耗积分数" CssClass="forminput form-control"></asp:TextBox></dd>
                    </dl>
                    <dl>
                        <dt><strong>奖项设置</strong></dt>
                    </dl>

                    <dl>
                        <dt>一等奖：</dt>
                        <dd>
                            <abbr>
                                <asp:DropDownList ID="ddlDrawType1" index="1" runat="server" CssClass="forminput iselect" Height="32">
                                    <asp:ListItem Text="请选择奖品类型" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="积分" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="优惠券" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </abbr>
                        </dd>
                        <dd class="J_select_dd">
                            <div class="input-group">
                                <asp:TextBox ID="txtPoints1" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <span class="input-group-addon">分</span>
                            </div>
                            <Hi:CouponDropDownList ID="ddlCoupons1" runat="server" CssClass="forminput form-control iselect" Height="32" Width="160"></Hi:CouponDropDownList>
                        </dd>
                        <dt class="w-auto">概率：</dt>
                        <dd class="input-group">
                            <asp:TextBox ID="txtPercent1" runat="server" CssClass="forminput form-control"></asp:TextBox>
                            <span class="input-group-addon">%</span>
                        </dd>
                    </dl>
                    <dl>
                        <dt>二等奖：</dt>
                        <dd>
                            <abbr>
                                <asp:DropDownList ID="ddlDrawType2" index="2" runat="server" CssClass="forminput iselect" Height="32">
                                    <asp:ListItem Text="请选择奖品类型" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="积分" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="优惠券" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </abbr>
                        </dd>
                        <dd class="J_select_dd">
                            <div class="input-group">
                                <asp:TextBox ID="txtPoints2" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <span class="input-group-addon">分</span>
                            </div>
                            <Hi:CouponDropDownList ID="ddlCoupons2" runat="server" CssClass="forminput form-control iselect" Height="32" Width="160"></Hi:CouponDropDownList>
                        </dd>
                        <dt class="w-auto">概率：</dt>
                        <dd class="input-group">
                            <asp:TextBox ID="txtPercent2" runat="server" CssClass="forminput form-control"></asp:TextBox>
                            <span class="input-group-addon">%</span>
                        </dd>
                    </dl>
                    <dl>
                        <dt>三等奖：</dt>
                        <dd>
                            <abbr>
                                <asp:DropDownList ID="ddlDrawType3" index="3" runat="server" CssClass="forminput iselect" Height="32">
                                    <asp:ListItem Text="请选择奖品类型" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="积分" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="优惠券" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </abbr>
                        </dd>
                        <dd class="J_select_dd">
                            <div class="input-group">
                                <asp:TextBox ID="txtPoints3" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <span class="input-group-addon">分</span>
                            </div>
                            <Hi:CouponDropDownList ID="ddlCoupons3" runat="server" CssClass="forminput form-control iselect" Height="32" Width="160"></Hi:CouponDropDownList>
                        </dd>
                        <dt class="w-auto">概率：</dt>
                        <dd class="input-group">
                            <asp:TextBox ID="txtPercent3" runat="server" CssClass="forminput form-control"></asp:TextBox>
                            <span class="input-group-addon">%</span>
                        </dd>
                    </dl>
                    <dl>
                        <dt>优秀奖：</dt>
                        <dd>
                            <abbr>
                                <asp:DropDownList ID="ddlDrawType4" index="4" runat="server" CssClass="forminput iselect" Height="32">
                                    <asp:ListItem Text="请选择奖品类型" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="积分" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="优惠券" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </abbr>
                        </dd>
                        <dd class="J_select_dd">
                            <div class="input-group">
                                <asp:TextBox ID="txtPoints4" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <span class="input-group-addon">分</span>
                            </div>
                            <Hi:CouponDropDownList ID="ddlCoupons4" runat="server" CssClass="forminput form-control iselect" Height="32" Width="160"></Hi:CouponDropDownList>
                        </dd>
                        <dt class="w-auto">概率：</dt>
                        <dd class="input-group">
                            <asp:TextBox ID="txtPercent4" runat="server" CssClass="forminput form-control"></asp:TextBox>
                            <span class="input-group-addon">%</span>
                        </dd>
                    </dl>
                    </div>
                <div class="formitem ld-set">
                    <dl>
                        <dt>&nbsp;</dt>
                        <dd>
                            <asp:Button ID="btnOK" runat="server" Text="确定" CssClass="btn btn-primary" OnClick="btnOK_Click" />
                        </dd>
                    </dl>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
