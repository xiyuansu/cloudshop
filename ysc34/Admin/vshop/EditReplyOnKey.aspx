<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false"
    Inherits="Hidistro.UI.Web.Admin.EditReplyOnKey"
    CodeBehind="EditReplyOnKey.aspx.cs"
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
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="ReplyOnKey.aspx">管理</a></li>
                <li  class="hover"><a href="javascript:void">编辑文本</a></li>                
            </ul>
        </div>
        <div class="areacolumn clearfix">
            <div class="columnright">
                <div class="formitem validator2">
                    <ul>
                        <li><span class="formitemtitle ">回复内容：</span>
                            <span>
                                <asp:TextBox ID="fcContent" runat="server" CssClass="form_input_l form-control"  Height="150px" TextMode="MultiLine" /></span>
                        </li>
                        <li><span class="formitemtitle">回复类型：</span>
                            <asp:CheckBox ID="chkKeys" runat="server" Text="关键字回复" CssClass="icheck mr_20" />
                            <asp:CheckBox ID="chkSub" runat="server" Text="关注时回复" CssClass="icheck mr_20" />
                            <asp:CheckBox ID="chkNo" runat="server" Text="无匹配回复" CssClass="icheck" />
                        </li>
                        <li class="likey"><span class="formitemtitle"><em>*</em>关键字：</span>
                            <asp:TextBox ID="txtKeys" runat="server" CssClass="form_input_m form-control" placeholer="用户可通过该关键字搜到到这个内容"></asp:TextBox>
                        </li>
                        <li class="likey"><span class="formitemtitle ">匹配模式：</span>
                            <Hi:YesNoRadioButtonList ID="radMatch" runat="server" RepeatLayout="Flow" YesText="模糊匹配" NoText="精确匹配" CssClass="icheck icheck-label-10" />
                        </li>
                        <li><span class="formitemtitle">状态：</span>
                            <Hi:YesNoRadioButtonList ID="radDisable" runat="server" CssClass="icheck icheck-label-10" RepeatLayout="Flow" YesText="启用" NoText="禁用" />
                        </li>
                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnAdd" runat="server" OnClientClick="return CheckKey();;" Text="保存" CssClass="btn btn-primary" />
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
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="../js/ReplyOnKey.js"></script>
</asp:Content>
