<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CashSetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.CashSetting" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <link href="/admin/css/Hishopv5.css" rel="stylesheet" />
    <script type="text/javascript" src="/admin/js/Hishopv5.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $.fn.bootstrapSwitch.defaults.size = 'small';
            $('#ctl00_contentHolder_ooEnableBulkPaymentAliPay').show();
            $('#ctl00_contentHolder_ctl00').bootstrapSwitch({});
        });
        $(function () {
            $.fn.bootstrapSwitch.defaults.size = 'small';
            $('#ctl00_contentHolder_ooEnableBulkPaymentWeixin').show();
            $('#ctl00_contentHolder_ctl01').bootstrapSwitch({});
        }); //]]>

        function InitValidators() {
            initValid(new InputValidator('txtMinimumSingleShot', 1, 11, false,null, '单次提现最小限额必须大于等于1元,限额在1-1000万以内！'));
            appendValid(new MoneyRangeValidator("txtMinimumSingleShot", 1, 10000000, '单次提现最小限额必须大于等于1元,限额在1-1000万以内！'));
        }

        function IsFlagDate() {
            if (!PageIsValid())
                return false;
            return true;
        }

        function InitValidatorsAlipay(bret) {
            if (bret) {
                initValid(new InputValidator('txtAlipayPartner', 1, 100, false, '', '合作者身份(PID)不能为空，且字符数限制在100以内'))
                initValid(new InputValidator('txtAlipayKey', 1, 100, false, '', '安全校验码(Key)不能为空，且字符数限制在100以内'))
                initValid(new InputValidator('txtAlipayEmail', 1, 100, false, '', '付款帐号不能为空，且字符数限制在100以内'))
                initValid(new InputValidator('txtAlipayAccountName', 1, 100, false, '', '付款帐号名不能为空，且字符数限制在100以内'))
            } else {
                $(".AlipayRequest").find("input").each(function () { if ($(this).val().length > 100) $(this).val(""); });
                initValid(new InputValidator('txtAlipayPartner', 1, 100, true, '', ''))
                initValid(new InputValidator('txtAlipayKey', 1, 100, true, '', ''))
                initValid(new InputValidator('txtAlipayEmail', 1, 100, true, '', ''))
                initValid(new InputValidator('txtAlipayAccountName', 1, 100, true, '', ''))
            }
        }
        function InitValidatorsWeixin(bret) {
            if (bret) {
                initValid(new InputValidator('txtWeixinMchAppid', 1, 100, false, '', '微信AppId不能为空，且字符数限制在100以内'));
                initValid(new InputValidator('txtWeixinMchid', 1, 100, false, '', '商户号不能为空，且字符数限制在100以内'));
                initValid(new InputValidator('txtWeixinKey', 32, 32, false, '', '商户密钥必须为32位您在商户中心自己设置的32位字符串'));
            } else {
                $(".WeixinRequest").find("input").each(function () { if ($(this).val().length > 100) $(this).val(""); });
                initValid(new InputValidator('txtWeixinMchAppid', 1, 100, true, '', ''));
                initValid(new InputValidator('txtWeixinMchid', 1, 100, true, '', ''));
                initValid(new InputValidator('txtWeixinKey', 1, 100, true, '', ''));
            }
        }

        function InitValidatorsAdvance(bret) {
            if (bret) {
                $("#isopenadvance").show();
            } else {
                $("#isopenadvance").hide();
                $(".AlipayRequest").hide()
                $(".WeixinRequest").hide();
                $('input[name="ctl00$contentHolder$ctl00"],input[name="ctl00$contentHolder$ctl01"]').bootstrapSwitch("state", false);
                
            }
        }

        $(document).ready(function () {
            InitValidators();
            InitValidatorsAdvance(true);

            //是否开启预付款提现
            //if ($('input[name="ctl00$contentHolder$ctl00"]').bootstrapSwitch('state') == false) { InitValidatorsAdvance(false); } else { InitValidatorsAdvance(true); }
            //$('input[name="ctl00$contentHolder$ctl00"]').on('switchChange.bootstrapSwitch', function (event, state) {
            //    var Advance = 0;
            //    if (state) {
            //        //InitValidatorsAdvance(true);
            //    } else {
            //        //InitValidatorsAdvance(true);
                 
            //    }

            //});


            //支付宝参数验证
            if ($('input[name="ctl00$contentHolder$ctl00"]').bootstrapSwitch('state') == false) { $(".AlipayRequest").hide(); InitValidatorsAlipay(false); } else { $(".AlipayRequest").show(); InitValidatorsAlipay(true); }
            $('input[name="ctl00$contentHolder$ctl00"]').on('switchChange.bootstrapSwitch', function (event, state) {
                var Alipay = 0;
                if (state) {
                    $(".AlipayRequest").show(); InitValidatorsAlipay(true);
                } else {
                    $(".AlipayRequest").hide(); InitValidatorsAlipay(false);
                }

            });
            //微信参数验证
            if ($('input[name="ctl00$contentHolder$ctl01"]').bootstrapSwitch('state') == false) { $(".WeixinRequest").hide(); InitValidatorsWeixin(false); } else { $(".WeixinRequest").show(); InitValidatorsWeixin(true); }
            $('input[name="ctl00$contentHolder$ctl01"]').on('switchChange.bootstrapSwitch', function (event, state) {
                var Weixin = 0;
                if (state) {
                    $(".WeixinRequest").show(); InitValidatorsWeixin(true);
                } else {
                    $(".WeixinRequest").hide(); InitValidatorsWeixin(false);
                }
            });
            //隐藏上传控件
            if ($("#divWeixinCertPathHtml").find("a").length > 0) {
                $("#fileWeixinCertPath").hide();
            }
            //删除文件
            $(".delCertFile").live("click", function (e) {
                var fileUrl = $(this).attr("fileUrl");
                var fileObj = $(this);
                $.ajax({
                    url: '/Admin/Admin.ashx?action=delCertFile',
                    type: 'post', dataType: 'json', timeout: 20000,
                    data: { action: "delCertFile", FilePath: fileUrl },
                    error: function (xm, msg) {
                        alert(msg);
                    },
                    success: function (data) {
                        if (data.status == "0") {
                            alert(data.msg);
                        }
                        $(fileObj).parent().prev().show();
                        $(fileObj).parent().hide();
                        $(fileObj).parent().find("input[type='text']").remove();
                    }
                });
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div id="formitem" runat="server" class="formitem">
                <ul>
                    <div id="isopenadvance">
                     <li><span class="formitemtitle"><big style="color: red;">*</big>单次提现最小限额：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtMinimumSingleShot" runat="server" ClientIDMode="Static" class="form_input_s form-control" Text="1.00"></asp:TextBox>
                            <span class="input-group-addon">元</span>
                        </div>
                        <p id="txtMinimumSingleShotTip">单次提现最小限额必须大于等于1元,限额在1-1000万以内</p>
                    </li>
                     <li>
                        <h2 class="colorE">支付宝</h2>
                    </li>
                    <li><span class="formitemtitle os-switch-text">支付宝批量转账：</span>
                        <Hi:OnOff runat="server" ID="ooEnableBulkPaymentAliPay"></Hi:OnOff>
                        <span class="ml_10">
                            开启以后可用支付宝批量发放提现，自动转账。
                        <a class="colorBlue" href="https://b.alipay.com/order/appInfo.htm?salesPlanCode=2011052500326597&channel=ent" target="_blank">开通支付宝批量付款</a>
                        </span>
                    </li>

                    <li class="AlipayRequest mb_0"><span class="formitemtitle"><big style="color: red;">*</big>合作者身份(PID)：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtAlipayPartner" runat="server" ClientIDMode="Static" class="form_input_m form-control" Text="" ></asp:TextBox>
                            <a href="https://b.alipay.com/order/pidKey.htm?pid=2088101600118305&amp;product=escrow" target="_blank" class="ml_10 colorBlue">获取PID、Key</a>
                        </div>
                        <p id="txtAlipayPartnerTip"></p>
                    </li>
                    <li class="AlipayRequest mb_0"><span class="formitemtitle"><big style="color: red;">*</big>安全校验码(Key)：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtAlipayKey" runat="server" ClientIDMode="Static" class="form_input_m form-control" Text="" ></asp:TextBox>
                        </div>
                        <p id="txtAlipayKeyTip"></p>
                    </li>
                    <li class="AlipayRequest mb_0"><span class="formitemtitle"><big style="color: red;">*</big>付款帐号：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtAlipayEmail" runat="server" ClientIDMode="Static" class="form_input_m form-control" Text="" ></asp:TextBox>
                        </div>
                        <p id="txtAlipayEmailTip"></p>
                    </li>
                    <li class="AlipayRequest mb_0"><span class="formitemtitle"><big style="color: red;">*</big>付款帐号名：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtAlipayAccountName" runat="server" ClientIDMode="Static" class="form_input_m form-control" Text="" ></asp:TextBox>
                        </div>
                        <p id="txtAlipayAccountNameTip"></p>
                    </li>
                    <li>
                        <h2 class="colorE">微信</h2>
                    </li>
                    <li><span class="formitemtitle os-switch-text">微信批量转账：</span>
                        <Hi:OnOff runat="server" ID="ooEnableBulkPaymentWeixin"></Hi:OnOff>
                        <span class="ml_10">开启以后可用微信批量发放提现，自动转账。</span>
                    </li>
                    <li class="WeixinRequest mb_0"><span class="formitemtitle"><big style="color: red;">*</big>AppId：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtWeixinMchAppid" runat="server" ClientIDMode="Static" class="form_input_m form-control" Text=""></asp:TextBox>
                        </div>
                        <p id="txtWeixinMchAppidTip"></p>
                    </li>
                    <li class="WeixinRequest mb_0"><span class="formitemtitle"><big style="color: red;">*</big>mch_id：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtWeixinMchid" runat="server" ClientIDMode="Static" class="form_input_m form-control" Text="" ></asp:TextBox>
                        </div>
                        <p id="txtWeixinMchidTip"></p>
                    </li>
                    <li class="WeixinRequest mb_0"><span class="formitemtitle"><big style="color: red;">*</big>Key：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtWeixinKey" runat="server" ClientIDMode="Static" class="form_input_m form-control" Text=""></asp:TextBox>
                        </div>
                        <p id="txtWeixinKeyTip">由商户在商户中心自己设置的32位字符串</p>
                    </li>
                    <li class="WeixinRequest mb_0"><span class="formitemtitle"><big style="color: red;">*</big>校验用户姓名方式：</span>
                        <div class="input-group" style="position:relative; z-index:9999;">
                            <abbr class="formselect">
                                <asp:DropDownList runat="server" ID="ddlWeixinCheckName" CssClass="iselect" />
                            </abbr>
                        </div>
                        <p id="ddlWeixinCheckNameTip"></p>
                    </li>
                    <li class="WeixinRequest"><span class="formitemtitle "><big style="color: red;">*</big>证书路径：</span>
                        <div class="input-group" style="position:relative;z-index:0;">
                            <asp:FileUpload ID="fileWeixinCertPath" ClientIDMode="Static" runat="server"  />
                            <div runat="server" id="divWeixinCertPathHtml" clientidmode="Static"></div>
                            <asp:HiddenField ID="hidWeixinCertPath" runat="server" />
                        </div>
                        <p id="fileWeixinCertPathTip">证书用于企业帐号支付以及退款原路返回，请使用扩展名为p12的证书文件</p>
                    </li>
                    <%--                    <li class="WeixinRequest"><span class="formitemtitle "><big style="color: red;">*</big>证书密码：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtWeixinCertPassword" runat="server" TextMode="Password" ClientIDMode="Static" class="forminput form-control" Text="" Width="230"></asp:TextBox>
                        </div>
                        <p id="txtWeixinCertPasswordTip">证书密码用于企业帐号支付以及退款原路返回</p>
                    </li>--%>


                   </div>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnSave" runat="server" OnClientClick="return IsFlagDate();" CssClass="btn btn-primary" Text="保存" />
                </div>
            </div>
        </div>
    </div>
       </div>
</asp:Content>
