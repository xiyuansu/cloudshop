<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditReferralGrade.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.EditReferralGrade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
     <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtGradeName', 1, 8, false, null, '分销员等级名称不能为空，长度限制在8个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtCommissionThreshold', 1, 5, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '设置佣金门槛达到多少佣金以后自动升级到此等级，为0-99999之间的数字'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCommissionThreshold', 0, 99999, '设置佣金门槛达到多少佣金以后自动升级到此等级，为0-99999之间的数字'));
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="ReferralGrades">管理</a></li>
                <li class="hover"><a href="javascript:void">编辑</a></li>
            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">

            <div class="formitem">
                <ul>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>分销员等级名称：</span>
                        <asp:TextBox ID="txtGradeName" CssClass="form_input_m form-control" runat="server" placeholder="限制在8字符以内" />
                        <p id="ctl00_contentHolder_txtGradeNameTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>佣金门槛：</span>
                        <asp:TextBox ID="txtCommissionThreshold" CssClass="form_input_m form-control" runat="server" placeholder="为0-99999之间的数字" />
                        <p id="ctl00_contentHolder_txtCommissionThresholdTip"></p>
                    </li>
                </ul>

            </div>
            <div class="ml_198">
                <asp:Button ID="btnSubmitReferralGrades" OnClientClick="return PageIsValid();" Text="确 定" CssClass="btn btn-primary" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
