<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="setShoppingScore.aspx.cs" Inherits="Hidistro.UI.Web.Admin.setShoppingScore" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="/Admin/Member/MemberPointList.aspx">积分管理</a></li>
                <li>
                    <a href="#" class="hover">积分规则设置</a></li>
            </ul>
        </div>
    </div>
    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem">
                <blockquote class="blockquote-default blockquote-tip">注意：积分抵扣活动不与团购、限时抢购和火拼团同时使用  </blockquote>

                <ul>
                    <li>
                        <h2 class="colorE">积分获取规则</h2>
                    </li>
                    <li>
                        <span class="formitemtitle">注册会员奖励：&nbsp;</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtMemberRegistrationPoint" runat="server" placeholder="新会员注册赠送多少积分" CssClass="form_input_s form-control" MaxLength="5"></asp:TextBox>
                            <span class="input-group-addon">分</span>
                        </div>
                    </li>

                    <li><span class="formitemtitle">每日签到奖励：&nbsp;</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtSignInPoint" runat="server" placeholder="" CssClass="form_input_s form-control" MaxLength="5"></asp:TextBox>
                            <span class="input-group-addon">分</span>
                        </div>
                    </li>

                    <li>
                        <span class="formitemtitle">每连续签到：</span>
                        <div class="input-group fl">
                            <asp:TextBox ID="txtContinuousDays" runat="server" CssClass="form_input_s form-control" MaxLength="5"></asp:TextBox>
                            <span class="input-group-addon">天</span>
                        </div>
                        <span class="c-666 pull-left font-14">&nbsp;&nbsp;额外奖励：</span>
                        <div class="input-group fl">
                            <asp:TextBox ID="txtContinuousPoint" runat="server" placeholder="连续签到多少天后，当天除奖励每日签到积分外，额外再奖励积分" CssClass="form_input_s form-control" MaxLength="5"></asp:TextBox>
                            <span class="input-group-addon">分</span>
                        </div>
                    </li>
                </ul>
                <ul>
                    <li><span class="formitemtitle">购物消费：&nbsp;</span>
                        <div class="input-group">
                            <span class="input-group-addon">￥ </span>
                            <asp:TextBox ID="txtShoppingBounty" runat="server" placeholder="购物积分不能为空,必须在0.01-10000000之间" CssClass="form_input_s form-control" MaxLength="8"></asp:TextBox>

                            <span class="c-666 pull-left font-14">&nbsp;&nbsp;奖励一积分（不含运费）</span>
                        </div>
                    </li>
                      <li><span class="formitemtitle">评论商品奖励：&nbsp;</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtProductCommentPoint" runat="server" placeholder="" CssClass="form_input_s form-control" MaxLength="5"></asp:TextBox>
                            <span class="input-group-addon" style="padding: 8px 5px !important">分</span>
                            <span class="c-666 pull-left font-14">&nbsp;&nbsp;（每一个有内容的评价都会获得相应的积分）</span>
                        </div>
                    </li>
                </ul>
                <ul>
                    <li>
                        <h2 class="colorE">积分抵扣规则</h2>
                    </li>
                    <li><span class="formitemtitle">抵扣订单金额每：&nbsp;</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtShoppingDeduction" runat="server" placeholder="0表示积分不能抵扣订单金额" CssClass="form_input_s form-control" MaxLength="5"></asp:TextBox>
                            <span class="input-group-addon">分</span>
                            <span class="c-666 pull-left font-14">&nbsp;&nbsp;抵扣一元</span>
                        </div>
                    </li>
                    <li><span class="formitemtitle">最高可抵扣比例：&nbsp;</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtShoppingDeductionRatio" runat="server" placeholder="单笔订单最高可抵扣比例，比例必须小于或等于100%" CssClass="form_input_s form-control" MaxLength="3"></asp:TextBox>
                            <span class="input-group-addon">%</span>
                        </div>
                    </li>
                </ul>
                <ul>
                    <li><span class="formitemtitle">与优惠券同时使用：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radCanPointUseWithCoupon"></Hi:OnOff>

                        </abbr>

                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="btn btn-primary" OnClientClick="return PageIsValid();" />
                </div>

            </div>
        </div>
    </div>
    <script type="text/javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtMemberRegistrationPoint', 1, 5, false, '[0-9]\\d*', '注册会员奖励分数不能为空且是整数'));
            initValid(new InputValidator('ctl00_contentHolder_txtSignInPoint', 1, 5, false, '[0-9]\\d*', '每日签到奖励分数不能为空且是整数'));
            initValid(new InputValidator('ctl00_contentHolder_txtContinuousDays', 1, 5, false, '[0-9]\\d*', '连续签到天数不能为空且是整数'));
            initValid(new InputValidator('ctl00_contentHolder_txtContinuousPoint', 1, 5, false, '[0-9]\\d*', '连续签到奖励分数不能为空且是整数'));
            initValid(new InputValidator('ctl00_contentHolder_txtShoppingBounty', 1, 10, false, "(([0-9]+.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*.[0-9]+)|([0-9]*[1-9][0-9]*))", '购物消费金额不能为空且是数字,必须在0.1-10000000之间'));
            //initValid(new InputValidator('ctl00_contentHolder_txtShoppingBounty', 1, 10, false, "(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)", '购物消费金额不能为空且是数字,必须在0.01-10000000之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtShoppingDeduction', 1, 10, false, '[0-9]\\d*', '订单抵扣金额分数不能为空且是整数'));
            initValid(new InputValidator('ctl00_contentHolder_txtShoppingDeductionRatio', 1, 5, false, '(^[1-9][0-9]$)|(^100)|(^[0-9]$)$', '可使用积分抵扣的比例必须为整数，且小于或等于100%'));
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>
