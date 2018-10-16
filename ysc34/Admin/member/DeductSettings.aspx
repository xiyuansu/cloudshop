<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DeductSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.DeductSettings" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtRegReferralDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '设置的注册佣金不正确'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtRegReferralDeduct', 0, 10000, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtSubMemberDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '设置的会员直接上级抽佣比例不正确'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSubMemberDeduct', 0, 100, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtSecondLevelDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '设置的会员上二级抽佣比例不正确'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSecondLevelDeduct', 0, 100, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtThreeLevelDeduct', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '设置的会员上三级抽佣比例不正确'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtThreeLevelDeduct', 0, 100, '输入的数值超出了系统表示范围'));
        }
        $(document).ready(function () {
            InitValidators();
            //if ($("#ctl00_contentHolder_OnOffDeduct input").is(':checked')) {
            //    $("#second").show();
            //    $("#three").show();
            //}
            //else {
            //    $("#second").hide();
            //    $("#three").hide();
            //}
            if ($("#ctl00_contentHolder_OnOffSecondCommission input").is(':checked')) {
                $("#second").show();
                $("#threeUL").show();
            }
            else {
                $("#second").hide();
                $("#three").hide();
                $("#threeUL").hide();
            }
            if ($("#ctl00_contentHolder_OnOffThirdCommission input").is(':checked')) {
                $("#three").show();
            }
            else {
                $("#three").hide();
            }
        });

        function fuOnOffSecond(event, state) {
            if (state) {
                $("#second").show();
                $("#threeUL").show();
                if ($("#ctl00_contentHolder_OnOffThirdCommission input").is(':checked')) {
                    $("#three").show();
                }
            }
            else {
                $("#second").hide();
                $("#three").hide();
                $("#threeUL").hide();
            }
        }

        function fuOnOffThird(event, state) {
            if (state) {
                if ($("#ctl00_contentHolder_OnOffSecondCommission input").is(':checked')) {
                    $("#three").show();
                }
                else {
                    $("#threeUL").hide();
                    $("#three").hide();
                }
            }
            else {
                $("#three").hide();
            }
        }

        //function fuOnOffDeduct(event, state) {
        //    if (state) {
        //        $("#second").show();
        //        $("#three").show();
        //    }
        //    else {
        //        $("#second").hide();
        //        $("#three").hide();
        //    }
        //}

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void" >分佣规则设置</a></li>
                <li><a href="ReferralSettings.aspx">分销员申请设置</a></li>
                <li><a href="ReferralPosterSet.aspx">分销海报设置</a></li>
                <li><a href="ExtendShareSettings.aspx">分销分享设置</a></li>
                <li><a href="RecruitPlanSettings.aspx">招募计划设置</a></li>
            </ul>
        </div>
        <div class="datafrom">
            <div class="formitem">
                <ul>
                    <li ><span class="formitemtitle">注册佣金：</span>
                        <div class="input-group">
                            <span class="input-group-addon">￥</span>
                            <asp:TextBox ID="txtRegReferralDeduct" CssClass="form_input_s form-control" runat="server" />
                        </div>
                        <p id="txtRegReferralDeductTip" runat="server">A邀请一个新会员B注册后，A所能获得的佣金</p>
                    </li>
                </ul>
                 <ul style="display:none;">
                    <li class="clearfix"><span class="formitemtitle">开启多级分佣：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="OnOffDeduct"></Hi:OnOff>
                        </abbr>
                        <span>&nbsp;使用多级分佣及高额佣金，有关停微信支付的风险，请妥善使用!</span>
                    </li>
                </ul>
                <ul>
                    <li ><span class="formitemtitle">会员直接上级抽佣比例：</span>
                        <div class="input-group">
                            
                            <asp:TextBox ID="txtSubMemberDeduct" CssClass="form_input_s form-control" runat="server" />
                            <span class="input-group-addon">%</span>
                        </div>
                        <p id="txtSubMemberDeductTip" runat="server">A邀请了B，B后续消费A所能获得的佣金</p>
                    </li>
                </ul>
                 <ul id="secondUL">
                    <li class="clearfix"><span class="formitemtitle">开启二级分销：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="OnOffSecondCommission"></Hi:OnOff>
                        </abbr>
                        <span>&nbsp;</span>
                    </li>
                </ul>
                <ul id="second">
                    <li ><span class="formitemtitle">会员上二级抽佣比例：</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtSecondLevelDeduct" CssClass="form_input_s form-control" runat="server" />
                            <span class="input-group-addon">%</span>
                        </div>
                        <p id="txtSecondLevelDeductTip" runat="server">A邀请的会员B后续发展了其它会员C，C消费时，A所能获得的佣金</p>
                    </li>
                </ul>
                 <ul id="threeUL">
                    <li class="clearfix"><span class="formitemtitle">开启三级分销：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="OnOffThirdCommission"></Hi:OnOff>
                        </abbr>
                        <span>&nbsp;</span>
                    </li>
                </ul>
                <ul id="three">
                    <li ><span class="formitemtitle">会员上三级抽佣比例：</span>
                        <div class="input-group">
                            
                            <asp:TextBox ID="txtThreeLevelDeduct" CssClass="form_input_s form-control" runat="server" />
                            <span class="input-group-addon">%</span>
                        </div>
                        <p id="txtThreeLevelDeductTip" runat="server">A邀请的会员B后续发展了会员C，C又发展了会员D，D消费时，A所能获得的佣金</p>
                    </li>
                </ul>
                <ul>
                    <li class="clearfix"><span class="formitemtitle">分销员自己购买是否计算佣金：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radSelfBuyDeduct"></Hi:OnOff>
                        </abbr>
                        <span>&nbsp;分销员购买商品将获得直接上级抽佣</span>
                    </li>
                </ul>
                <ul>
                    <li class="clearfix"><span class="formitemtitle">在商品详情页显示佣金：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radShowDeductInProductPage"></Hi:OnOff>
                        </abbr>
                    </li>
                </ul>
                <div class="ml_198 mt0">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="btn btn-primary" OnClientClick="return PageIsValid();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
