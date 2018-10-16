<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddMemberGrade.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddMemberGrade" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="membergrades.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">添加</a></li>
            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">

            <div class="formitem">
                <ul>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>会员等级名称：</span>
                        <asp:TextBox ID="txtRankName" CssClass="form_input_m form-control" runat="server" placeholder="限制在20字符以内" />
                        <p id="ctl00_contentHolder_txtRankNameTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>积分满足点数：</span>
                        <asp:TextBox ID="txtPoint" CssClass="form_input_m form-control" runat="server" placeholder="为大于等于0的整数" />
                        <p id="ctl00_contentHolder_txtPointTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>会员等级价格：</span>

                        <div class="input-group">
                            <span class="input-group-addon">一口价 x</span>
                            <asp:TextBox ID="txtValue" CssClass="form_input_s form-control" runat="server" placeholder="折扣为正数" />
                            <span class="input-group-addon">%</span>
                        </div>
                        <p id="ctl00_contentHolder_txtValueTip"></p>
                    </li>
                    <li><span class="formitemtitle">设为默认：</span>
                        <Hi:YesNoRadioButtonList ID="chkIsDefault" runat="server" RepeatLayout="Flow" CssClass="icheck" />
                    </li>
                    <li class="mb_0"><span class="formitemtitle">备注：</span>
                        <asp:TextBox ID="txtRankDesc" runat="server" TextMode="MultiLine" CssClass="forminput form-control" Width="450" Height="120"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtRankDescTip"></p>
                    </li>
                </ul>

            </div>
            <div class="ml_198">
                <asp:Button ID="btnSubmitMemberRanks" OnClientClick="return PageIsValid();" Text="确 定" CssClass="btn btn-primary" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtRankName', 1, 20, false, null, '会员等级名称不能为空，长度限制在20个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtPoint', 1, 8, false, '-?[0-9]\\d*', '设置会员的积分达到多少分以后自动升级到此等级，为0-9999万的整数'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtPoint', 0, 2147483647, '设置会员的积分达到多少分以后自动升级到此等级，为大于等于0的整数'));
            initValid(new InputValidator('ctl00_contentHolder_txtValue', 1, 3, false, '-?[0-9]\\d*', '等级折扣为不能为空，且必须为0-999的整数'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtValue', 1, 1000, '等级折扣必须在1-1000之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtRankDesc', 0, 100, true, null, '备注的长度限制在100个字符以内'));
        }
        $(document).ready(function () { InitValidators(); });
    </script>

</asp:Content>
