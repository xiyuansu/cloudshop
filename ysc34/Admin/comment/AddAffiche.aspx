<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.AddAffiche" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix databody">
            <div class="title">
                <ul class="title-nav">
                    <li><a href="AfficheList.aspx">管理</a></li>
                    <li class="hover"><a href="javascript:void">添加</a></li>
                </ul>

            </div>
        <div class="columnright">

            <div class="formitem validator2 mt_10">
                <ul>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>标题：</span>
                        <asp:TextBox ID="txtAfficheTitle" runat="server" CssClass="form_input_l form-control" placeholder="长度限制在60个字符以内" />
                        <p id="ctl00_contentHolder_txtAfficheTitleTip"></p>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>公告内容：</span>
                        <span>
                            <Hi:Ueditor ID="fcContent" runat="server" Width="660" /></span>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnAddAffiche" runat="server" OnClientClick="return PageIsValid();" Text="添加" CssClass="btn btn-primary" />
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
            initValid(new InputValidator('ctl00_contentHolder_txtAfficheTitle', 1, 60, false, null, '公告标题不能为空，长度限制在60个字符以内'))
        }

        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>

