<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetOrderOption.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetOrderOption" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <%--富文本编辑器start--%>
    <link rel="stylesheet" href="/Utility/Ueditor/css/dist/component-min.css" />
    <link rel="stylesheet" href="/Utility/Ueditor/plugins/uploadify/uploadify-min.css" />

    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function funCheckEnableTax(event, state) {
            if (state) {
                $("#litaxrate").show();
            }
            else {
                if ($("#OnOffTax input").is(':checked') || $("#OnOffE_Tax input").is(':checked'))
                    $("#litaxrate").show();
                else
                    $("#litaxrate").hide();
            }
        }

        function funCheckEnableVATTax(event, state) {
            if (state) {
                $("#litaxrate1").show();
                $("#litaxrate2").show();
            }
            else {
                $("#litaxrate1").hide();
                $("#litaxrate2").hide();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem validator1 p-100 setorder">
                <ul>
                    <li>
                        <h2 class="clear">自动化设置</h2>
                    </li>
                    <li class="mb_0"><span class="formitemtitle "><em>*</em>限时抢购订单超过</span>
                        <div class="input-group" style="font-size: 14px">
                            <asp:TextBox ID="txtCountDownTime" runat="server" CssClass="form_input_s form-control" />
                            <span class="input-group-addon input-radius-bor">分</span>
                            &nbsp;&nbsp;未付款，订单自动关闭
                        </div>
                        <p id="P2" runat="server"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>下单后超过</span>
                        <div class="input-group" style="font-size: 14px">
                            <asp:TextBox ID="txtCloseOrderDays" runat="server" CssClass="form_input_s form-control" />
                            <span class="input-group-addon input-radius-bor">天</span>
                            &nbsp;&nbsp;未付款，订单自动关闭
                        </div>
                        <p id="txtCloseOrderDaysTip" runat="server"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>发货超过</span>
                        <div class="input-group" style="font-size: 14px">
                            <asp:TextBox ID="txtFinishOrderDays" runat="server" CssClass="form_input_s form-control" />
                            <span class="input-group-addon input-radius-bor">天</span>
                            &nbsp;&nbsp;未收货，订单自动完成
                        </div>
                        <p id="txtFinishOrderDaysTip" runat="server"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>订单完成超过</span>
                        <div class="input-group" style="font-size: 14px">
                            <asp:TextBox ID="txtEndOrderDays" runat="server" CssClass="form_input_s form-control" />
                            <span class="input-group-addon input-radius-bor">天</span>
                            &nbsp;&nbsp;自动结束交易，不能申请售后
                        </div>
                        <p id="txtEndOrderDaysTip" runat="server"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>订单完成超过</span>
                        <div class="input-group" style="font-size: 14px">
                            <asp:TextBox ID="txtEndOrderDaysEvaluate" runat="server" CssClass="form_input_s form-control" />
                            <span class="input-group-addon input-radius-bor">天</span>
                            &nbsp;&nbsp;自动五星好评
                        </div>
                        <p id="txtEndOrderDaysEvaluateTip" runat="server"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>自动处理退款</span>
                        <div class="input-group" style="font-size: 14px; width: 498px;">
                            <Hi:OnOff runat="server" ID="OnOffAutoDealRefund" ClientIDMode="Static"></Hi:OnOff>
                            &nbsp;&nbsp;开启后,如果客户申请退款自动退钱给客户(仅对服务类订单有效)
                        </div>
                    </li>
                    <li>
                        <h2 class="clear">其它设置</h2>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>订单短信通知：</span>
                        <div class="input-group" style="font-size: 14px; width: 298px;">
                            <Hi:OnOff runat="server" ID="OnOffOrderPayToShipper" ClientIDMode="Static"></Hi:OnOff>
                            &nbsp;&nbsp;订单支付后系统发短信通知发货人
                        </div>
                    </li>

                    <li><span class="formitemtitle"><em>*</em>跨境商品实名认证：</span>
                        <div class="input-group" style="font-size: 14px; width: 498px;">
                            <Hi:OnOff runat="server" ID="OnOffCertification" ClientIDMode="Static"></Hi:OnOff>
                            &nbsp;&nbsp;开启后，商品可设置实名验证，提交订单时用户需要提交身份证信息
                        </div>
                    </li>
                    <li style="display: none;" id="liCertificationModel">
                        <span class="formitemtitle">&nbsp;</span>
                        <div class="input-group" style="font-size: 14px; width: 498px;">
                            <asp:RadioButtonList ID="radCertificationModel" runat="server" RepeatDirection="Horizontal" Width="100%">
                                <asp:ListItem Text="&nbsp;仅验证身份证号" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="&nbsp;验证身份证及身份证正反面照片" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </li>
                    <li>
                        <h2 class="clear">发票设置</h2>
                    </li>
                    <li class="clearfix">
                        <span class="formitemtitle">普通发票：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="OnOffTax" ClientIDMode="Static"></Hi:OnOff>
                        </abbr>
                        <span class="formitemtitle">电子发票：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="OnOffE_Tax" ClientIDMode="Static"></Hi:OnOff> &nbsp;&nbsp;开启后买家下单可以索取该类型发票
                        </abbr>
                    </li>
                    <li class="mb_0" id="litaxrate"><span class="formitemtitle"><em>*</em>税率：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtTaxRate" runat="server" CssClass="form_input_s form-control" placeholder="0表示不承担发票税金" />
                            <span class="input-group-addon">%</span>
                        </div>
                        <p id="txtTaxRateTip" runat="server"></p>
                    </li>
                    <li class="clearfix">
                        <span class="formitemtitle">增值税专用发票：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="OnOffVATTax" ClientIDMode="Static"></Hi:OnOff>&nbsp;&nbsp;开启后买家下单可以索取发票
                        </abbr>
                    </li>
                    <li class="mb_0" style="display: none;" id="litaxrate1"><span class="formitemtitle"><em>*</em>税率：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtVATTax" runat="server" CssClass="form_input_s form-control" placeholder="0表示不承担发票税金" />
                            <span class="input-group-addon">%</span>
                        </div>
                        <p id="txtVATTaxTip" runat="server"></p>
                    </li>
                      <li class="mb_0" style="display: none;" id="litaxrate2"><span class="formitemtitle"><em>*</em>订单结束交易超过多少</span>
                        <div class="input-group" style="font-size:14px;">
                            <asp:TextBox ID="txtVATInvoiceDays" runat="server" CssClass="form_input_s form-control" placeholder="请输入天数" />
                            <span class="input-group-addon">天</span>&nbsp;&nbsp;开具发票
                        </div>
                        <p id="txtVATInvoiceDaysTip" runat="server"></p>
                    </li>
                    <li>
                        <h2 class="clear">配送设置</h2>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>上门自提：</span>
                        <div class="input-group" style="font-size: 14px; width: 420px;">
                            <Hi:OnOff runat="server" ID="OnOffIsOpenPickeupInStore" ClientIDMode="Static"></Hi:OnOff>
                            &nbsp;&nbsp;开启后，买家下单可选择上门自提，且运费默认为0
                        </div>
                    </li>
                    <li><span class="formitemtitle">备注：</span>
                        <asp:TextBox ID="tbxPickeupInStoreRemark" Rows="6" Height="50px" MaxLength="250" Columns="76" TextMode="MultiLine" runat="server" CssClass="form-control form_input_l" placeholder=" 比如自提点地址或营业时间等" />
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button ID="btnOK" runat="server" Text="提交" CssClass="btn btn-primary" OnClientClick="return PageIsValid()" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function InitValidators() {

            initValid(new InputValidator('ctl00_contentHolder_txtCloseOrderDays', 1, 10, false, '-?[0-9]\\d*', '下单后过期几天系统自动关闭未付款订单'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCloseOrderDays', 0, 90, '下单后过期几天系统自动关闭未付款订单'));

            initValid(new InputValidator('ctl00_contentHolder_txtFinishOrderDays', 1, 10, false, '-?[0-9]\\d*', '发货几天后，系统自动把订单改成已完成状态'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtFinishOrderDays', 0, 90, '发货几天后，系统自动把订单改成已完成状态'));

            initValid(new InputValidator('ctl00_contentHolder_txtEndOrderDays', 1, 10, false, '-?[0-9]\\d*', '订单完成几天后，系统自动结束交易，不得再申请退换货服务'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtEndOrderDays', 0, 90, '订单完成几天后，系统自动结束交易，不得再申请退换货服务'));


            initValid(new InputValidator('ctl00_contentHolder_txtEndOrderDaysEvaluate', 1, 10, false, '-?[0-9]\\d*', '订单完成几天后，系统自动结束交易后自动好评'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtEndOrderDaysEvaluate', 0, 90, '订单完成几天后，系统自动结束交易后自动好评'));

            initValid(new InputValidator('ctl00_contentHolder_txtTaxRate', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '发票税率不能为空,必须在0-100之间'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtTaxRate', 0, 100, '发票税率必须在0-100之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtVATTax', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '发票税率不能为空,必须在0-100之间'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtVATTax', 0, 100, '发票税率必须在0-100之间'));

            initValid(new InputValidator('ctl00_contentHolder_txtVATInvoiceDays', 1, 10, false, '-?[0-9]\\d*', '开具发票的天数不能为空,必须在0-30之间'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtVATTax', 0, 100, '开具发票的天数必须在0-30之间'));

            //initValid(new InputValidator('ctl00_contentHolder_txtExpresskey', 0, 60, true, null, '快递100所需Key在物流跟踪时会用到，长度限制在60字符以内'))
        }

       
        function fuCheckCertification(event, state) {
            if (state) {
                $("#liCertificationModel").show();
            }
            else {
                $("#liCertificationModel").hide();
            }
        }
       
        $(document).ready(function () {
            InitValidators();

            if ($("#OnOffTax input").is(':checked') || $("#OnOffE_Tax input").is(':checked')) {
                $("#litaxrate").show();
            }
            else {
                $("#litaxrate").hide();
            }
            if ($("#OnOffVATTax input").is(':checked')) {
                $("#litaxrate1").show();
                $("#litaxrate2").show();

            }
            else {
                $("#litaxrate1").hide();
                $("#litaxrate2").hide();
            }
            if ($("#OnOffCertification input").is(':checked')) {
                $("#liCertificationModel").show();
            }
            else {
                $("#liCertificationModel").hide();
            }

            $("#ctl00_contentHolder_txtTaxRate").blur(function (e) {
                var tax = parseFloat($(this).val());
                if (isNaN(tax) || tax < 0) {
                    $(this).val(0);
                }
            })
            $("#ctl00_contentHolder_txtVATTax").blur(function (e) {
                var tax = parseFloat($(this).val());
                if (isNaN(tax) || tax < 0) {
                    $(this).val(0);
                }
            })
            $("#ctl00_contentHolder_txtVATInvoiceDays").blur(function (e) {
                var tax = parseFloat($(this).val());
                if (isNaN(tax) || tax < 0) {
                    $(this).val(0);
                }
            })
        });
    </script>
</asp:Content>

