<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.AddHelp" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <div class="areacolumn clearfix databody">
        <div class="title">
                <ul class="title-nav">
                    <li><a href="HelpList.aspx">管理</a></li>
                    <li  class="hover"><a href="javascript:void">添加</a></li>                  
                </ul>
            </div>
        <div class="columnright">
            
            <div class="formitem clearfix validator3">
                <ul>
                    <li><span class="formitemtitle"><em>*</em>所属分类：</span><abbr class="formselect">
                        <Hi:HelpCategoryDropDownList ID="dropHelpCategory" AllowNull="false" runat="server" CssClass="iselect" NullToDisplay="请选择所属分类"/>

                    </abbr>
                        <a class="colorBlue ml_10" href="javascript:DialogFrame('comment/AddHelpCategory.aspx?source=add','添加帮助分类',null,null,function(e){location.reload();})">添加帮助分类</a></li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>主题：</span>
                        <asp:TextBox ID="txtHelpTitle" runat="server" CssClass="form_input_m form-control" placeholder="限制在60个字符以内"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtHelpTitleTip"></p>
                    </li>
                    <li><span class="formitemtitle">搜索描述：</span>
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtMetaDescription" placeholder="限制在260个字符以内" />
                    </li>
                    <li><span class="formitemtitle">搜索关键字：</span>
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtMetaKeywords" placeholder="限制在160个字符以内" />
                    </li>
                    <li><span class="formitemtitle">摘要：</span>
                        <asp:TextBox ID="txtShortDesc" TextMode="MultiLine" CssClass="form_input_l form-control"  Height="70px" runat="server" placeholder="限制在300个字符以内" ></asp:TextBox>
                    </li>
                    <li><span class="formitemtitle">显示在底部帮助：</span>
                        <Hi:OnOff runat="server" ID="ooShowFooter"></Hi:OnOff>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>内容：</span>
                        <span>
                            <Hi:Ueditor ID="fcContent" runat="server" Width="660" />
                        </span>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnAddHelp" runat="server" OnClientClick="return PageIsValid();" Text="添 加" CssClass="btn btn-primary" />
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
    <uc1:ImageList ID="ImageList" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtHelpTitle', 1, 60, false, null, '帮助主题不能为空，长度限制在60个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtShortDesc', 0, 300, true, null, '摘要的长度限制在300个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 1, 260, true, null, '搜索描述不能为空，长度限制在260个字符以内'))
            initValid(new InputValidator('ctl00_contentHolder_txtMetaKeywords', 0, 160, true, null, '搜索关键字的长度限制在160个字符以内'))
        }
        $(document).ready(function () { InitValidators(); });
    </script>

</asp:Content>

