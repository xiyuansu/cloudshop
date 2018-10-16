<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditMember.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditMember" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        .icheck_group label {
            margin-right: 20px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">
    <script type="text/javascript">
        //$(function () {
        //    var menu_left = window.parent.document.getElementById("menu_left");
        //    var aReturnTitle = $(".curent", menu_left);
        //    if (aReturnTitle) {
        //        $("#aReturnTitle").text($(aReturnTitle).text());
        //        var href = "/admin/" + $(aReturnTitle).attr("href");
        //        $("#aReturnTitle").attr("href", href);
        //    }
        //})

        function ToList() {
            var returnUrl = getParam("returnurl");
            if (returnUrl != "") {
                location.href = decodeURIComponent(returnUrl);
            } else {
                window.history.back();
            }
        }
    </script>
    <asp:HiddenField runat="server" ID="hidTagId" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidTagNames" Value="" ClientIDMode="Static" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="javascript:ToList()" id="aReturnTitle">会员列表</a></li>
                <li class="hover"><a>基本信息</a></li>
                <li><a href='<%="EditMemberLoginPassword.aspx?userId="+Page.Request.QueryString["userId"]+"&returnUrl="+Page.Request.QueryString["returnUrl"].ToNullString() %>'>登录密码</a></li>
                <li><a href='<%="EditMemberTransactionPassword.aspx?userId="+Page.Request.QueryString["userId"]+"&returnUrl="+Page.Request.QueryString["returnUrl"].ToNullString() %>'>交易密码</a></li>

            </ul>
        </div>
    </div>

    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="formitem">
                <ul>
                    <li class="mb_0"><span class="formitemtitle">会员名：</span>
                        <strong class="colorE">
                            <asp:Literal ID="lblLoginNameValue" runat="server"></asp:Literal></strong></li>
                    <li><span class="formitemtitle"><em>*</em>会员等级：</span>
                        <abbr class="formselect">
                            <Hi:MemberGradeDropDownList ID="drpMemberRankList" runat="server" CssClass="iselect" AllowNull="false" />
                        </abbr>
                    </li>
                    <li class="mb_0"><span class="formitemtitle">姓名：</span>
                        <asp:TextBox ID="txtRealName" runat="server" CssClass="forminput form-control" placeholder="20个字符以内"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtRealNameTip"></p>
                    </li>
                    <li><span class="formitemtitle">生日：</span>
                        <Hi:CalendarPanel runat="server" ID="calBirthday"></Hi:CalendarPanel>
                    </li>
                    <li><span class="formitemtitle">性别：</span>
                        <span class="icheck_group">
                            <Hi:GenderRadioButtonList runat="server" ID="gender" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="icheck" />
                        </span>
                    </li>
                    <li class="mb_0"><span class="formitemtitle">电子邮件地址：</span>
                        <asp:TextBox ID="txtprivateEmail" runat="server" CssClass="forminput form-control" placeholder="1-256个字符以内"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtprivateEmailTip"></p>
                    </li>
                    <li><span class="formitemtitle">所在区域：</span>
                        <Hi:RegionSelector runat="server" ID="rsddlRegion" />
                    </li>
                    <li class="mb_0"><span class="formitemtitle">详细地址：</span>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="forminput form-control" placeholder="0-100个字符以内"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtAddressTip"></p>
                    </li>
                    <%--<li class="mb_0"><span class="formitemtitle">旺旺：</span>
                        <asp:TextBox runat="server" ID="txtWangwang" CssClass="forminput form-control" placeholder="" />
                        <p id="ctl00_contentHolder_txtWangwangTip"></p>
                    </li>--%>
                    <li class="mb_0"><span class="formitemtitle">QQ：</span>
                        <asp:TextBox ID="txtQQ" runat="server" CssClass="forminput form-control" placeholder=""></asp:TextBox>
                        <p id="ctl00_contentHolder_txtQQTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle">昵称：</span>
                        <asp:TextBox ID="txtMSN" runat="server" CssClass="forminput form-control" placeholder=""></asp:TextBox>
                        <p id="ctl00_contentHolder_txtMSNTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle">手机号码：</span>
                        <asp:TextBox ID="txtCellPhone" runat="server" CssClass="forminput form-control" placeholder=""></asp:TextBox>
                        <p id="ctl00_contentHolder_txtCellPhoneTip"></p>
                    </li>
                    <li><span class="formitemtitle">注册日期：</span>
                        <Hi:FormatedTimeLabel ID="lblRegsTimeValue" runat="server" />
                    </li>
                    <li><span class="formitemtitle">总消费金额：</span>
                        <asp:Literal ID="lblTotalAmountValue" runat="server"></asp:Literal>
                    </li>
                    <li><span class="formitemtitle">会员标签：</span>
                        <div id="divTagContent" runat="server" class="m_info_tag" style="width: 500px; margin-top: 0;"></div>
                        <input type="button" value="编辑" class="btn btn-default float ml_10" onclick="EditTags()" />
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnEditUser" runat="server" Text="确 定" OnClientClick="return doSubmit();" CssClass="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function doSubmit() {
            if (!PageIsValid())
                return false;
            return true;
        }
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtRealName', 0, 20, true, '^[A-Za-z\\u4e00-\\u9fa5]+$', '姓名限制在20个字符以内,只能由中文或英文组成'));
            initValid(new InputValidator('ctl00_contentHolder_txtprivateEmail', 1, 256, true, '[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\.[\\w-]+)+', '请输入正确电子邮件，长度在1-256个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtAddress', 0, 100, true, null, '详细地址必须控制在100个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtQQ', 3, 20, true, '^[0-9]*$', 'QQ号长度限制在3-20个字符之间，只能输入数字'));
            initValid(new InputValidator('ctl00_contentHolder_txtMSN', 1, 20, true, null, '请输入正确的昵称，长度在1-20个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtTel', 3, 20, true, '^[0-9-]*$', '电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”'));
            initValid(new InputValidator('ctl00_contentHolder_txtCellPhone', 3, 20, true, '^[0-9]*$', '手机号码长度限制在3-20个字符之间,只能输入数字'));
        }
        $(document).ready(function () { InitValidators();CloseFrameWindow(); });

        function CloseFrameWindow() {
            var names = $("#hidTagNames").val();    
            if(names==""){
                return;
            }
            var tagNames = names.split(',');
            var tagContent = "";
            for(var i=0;i<tagNames.length;i++){
                tagContent+="<span style='margin-right:10px;'>"+tagNames[i]+"</span>";
            }
            $("#ctl00_contentHolder_divTagContent").html(tagContent);
        }

        //设置标签
        function EditTags() {
            DialogFrame("member/EditMemberTags.aspx?userTagIds=" + $("#hidTagId").val() + "&singleUserId="+<%= Page.Request.QueryString["userId"] %>, "设置标签", 600, 300, function (e) { CloseFrameWindow(); });
        }
        
    </script>
</asp:Content>
