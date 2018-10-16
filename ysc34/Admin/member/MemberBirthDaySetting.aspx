<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MemberBirthDaySetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.MemberBirthDaySetting" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<!--引用，样式，Javascript-->
    <script language="javascript" type="text/javascript">
        function ToList() {
            var returnUrl = getParam("returnurl");
            if (returnUrl != "") {
                location.href = decodeURIComponent(returnUrl);
            } else {
                window.history.back();
            }
        }
 
    </script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="javascript:ToList()">会员列表</a>
                </li>
                <li>
                    <a href="javascript:GoMemberSearch()">购买力筛选</a>
                </li>
                <li class="hover"><a href="javascript:GoMemberBirthDaySetting()">生日提醒设置</a></li>
            </ul>
        </div>
    </div>
    <div class="dataarea mainwidth databody">
        <div class="areacolumn clearfix">
            <div class="columnright">
                <div id="formitem" runat="server" class="formitem">
                    <div class="input-group">
                        <span class="formitemtitle"><big style="color: red;">*</big>提醒时间设置：</span>
                        <span class="">距当天时间</span>
                        <asp:TextBox ID="txtMinDayRemind" runat="server" ClientIDMode="Static" class="form_input_s form-control" Text="1"></asp:TextBox>
                        <span class="">天做生日提醒。<%--(设置为-1则关闭提醒)--%></span>
                    </div>
                    <div class="input-group">
                        <p id="txtMinDayRemindTip">&nbsp;</p>
                    </div>
                    <div class="ml_198">
                        <asp:Button ID="btnSave" runat="server" OnClientClick="return IsFlagDate();" CssClass="btn btn-primary" Text="保存" />
                    </div>
                </div>
            </div>
        </div>
    </div>

  <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/ManageMembers.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/ManageMembers.js?v=3.310" type="text/javascript"></script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new MoneyRangeValidator("txtMinDayRemind", 1, 366, '天数最小限额必须大于等于0元,限额在0-366以内！'));
        }
        $(document).ready(function () {
            InitValidators();
        });

        function IsFlagDate() {
            if (!PageIsValid())
                return false;
            return true;
        }
    </script>
</asp:Content>
