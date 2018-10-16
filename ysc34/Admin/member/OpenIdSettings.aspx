<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OpenIdSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.OpenIdSettings" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Register src="../Ascx/ImageList.ascx" tagname="ImageList" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
  
    <script type="text/javascript">
        function InitValidators() {
            // 显示名称
            initValid(new InputValidator('ctl00_contentHolder_txtName', 1, 50, false, null, '名称不能为空，长度限制在50个字符以内'));
        }
        $(document).ready(function () {
            pluginContainer = $("#pluginContainer");
            templateRow = $(pluginContainer).find("[rowType=attributeTemplate]");
            selectedNameCtl = $("#<%=txtSelectedName.ClientID %>");
            configDataCtl = $("#<%=txtConfigData.ClientID %>");
            ResetContainer($(selectedNameCtl).val(), "OpenIdService");
            InitValidators();
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void" >信任登录-<asp:Literal ID="lblDisplayName" runat="server"></asp:Literal></a></li>
            </ul>
            <%--<i runat="server" id="span1" class="glyphicon glyphicon-question-sign" data-container="body" style="cursor: pointer" data-toggle="popover" data-placement="left" title="操作说明" data-content=""></i>--%>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="formitem">
                <div style="float: left">
                    <ul>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>显示名称：</span>
                            <asp:TextBox ID="txtName" runat="server" CssClass="forminput form-control" placeholder="设置此信任登录方式在前台的显示名称"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtNameTip"></p>
                        </li>
                    </ul>

                    <ul id="pluginContainer" class="attributeContent2 fl">
                        <li rowtype="attributeTemplate" style="display: none;">
                            <span class="formitemtitle"><em>*</em>$Name$：</span>
                            $Input$
                        </li>
                    </ul>


                    <ul>
                        <li class="clearfix"><span class="formitemtitle">备注：</span>
                            <span>
                                <Hi:Ueditor ID="fcContent" runat="server" Width="660" />
                            </span>
                        </li>
                    </ul>
                    <div class="ml_198">
                            <asp:Button ID="btnSave" runat="server" OnClientClick="return PageIsValid();" Text="保 存" CssClass=" btn btn-primary"  />
                    </div>
                </div>
            </div>
        </div>

    </div>
    <div class="areacolumn clearfix  td_top_ccc">
        <asp:Literal ID="lblDisplayName2" runat="server"></asp:Literal>

    </div>
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <!-- start ImgPicker -->
       <uc1:ImageList ID="ImageList" runat="server" />


  
    <asp:HiddenField runat="server" ID="txtSelectedName" />
    <asp:HiddenField runat="server" ID="txtConfigData" />
    <script type="text/javascript" src="/utility/plugin.js" ></script>
</asp:Content>
