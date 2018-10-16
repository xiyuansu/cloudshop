<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false"
    Inherits="Hidistro.UI.Web.Admin.AddReplyOnKey"
    CodeBehind="AddReplyOnKey.aspx.cs"
    MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            ShowKey();
            initValid(new InputValidator('txtKeys', 1, 30, false, null, '不能为空，且必须在1-30个字符之间'));
            $("#chkKeys").bind("ifChanged", function () { ShowKey() });
        });
        function ShowKey() {
            if ($("#chkKeys:checked").length > 0) {
                $(".likey").show();
            }
            else {
                $(".likey").hide();
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="areacolumn clearfix databody">
            <div class="title">
                <ul class="title-nav">
                    <li><a href="ReplyOnKey.aspx">管理</a></li>
                    <li class="hover"><a href="javascript:void">添加文本</a></li>
                    <li><a href="AddSingleArticle.aspx">添加单图文</a></li>
                    <li><a href="AddMultiArticle.aspx">添加多图文</a></li>
                </ul>
            </div>
        <div class="columnright">
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle">回复内容：</span>
                        <span>
                            <asp:TextBox ID="fcContent" runat="server" class="form-control form_input_l"  Height="150px" TextMode="MultiLine" /></span>
                    </li>
                    <li><span class="formitemtitle">回复类型：</span>
                        <asp:CheckBox ID="chkKeys" runat="server" Text="关键字回复" CssClass="icheck icheck-label-10 mr5 mb_0" />
                        <asp:CheckBox ID="chkSub" runat="server" Text="关注时回复" CssClass="icheck icheck-label-10 mr5 mb_0" />
                        <asp:CheckBox ID="chkNo" runat="server" Text="无匹配回复" CssClass="icheck icheck-label-10 mb_0" />
                    </li>
                    <li class="likey mb_0"><span class="formitemtitle "><em>*</em>关键字：</span>
                        <asp:TextBox ID="txtKeys" runat="server" CssClass="form_input_m form-control" placeholder="用户可通过该关键字搜到到这个内容"></asp:TextBox>
                        <p id="txtKeysTip"></p>
                    </li>
                    <li class="likey"><span class="formitemtitle ">匹配模式：</span>
                        <Hi:YesNoRadioButtonList ID="radMatch" runat="server" RepeatLayout="Flow" YesText="模糊匹配" CssClass="icheck icheck-label-10" NoText="精确匹配" />
                    </li>
                    <li><span class="formitemtitle">状态：</span>
                        <Hi:YesNoRadioButtonList ID="radDisable" runat="server" RepeatLayout="Flow" YesText="启用" NoText="禁用" CssClass="icheck icheck-label-10" />
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnAdd" runat="server" OnClientClick="return CheckKey();" Text="保 存" CssClass="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="../js/ReplyOnKey.js" type="text/javascript"></script>
</asp:Content>
