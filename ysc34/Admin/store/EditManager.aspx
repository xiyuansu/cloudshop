<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditManager.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditManager" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
 
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="managers.aspx">管理</a></li>
                <li  class="hover"><a href="javascript:void">基本信息</a></li>
                <li><a href='<%="EditManagerPassword.aspx?userId="+Page.Request.QueryString["userId"] %>'>修改密码</a></li>

            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="formitem validator2">
                <ul class="managerInfoform">
                    <li><span class="formitemtitle">用户名：</span>
                        <strong class="colorG">
                            <asp:Literal ID="lblLoginNameValue" runat="server" /></strong></li>
                    <li><span class="formitemtitle"><em>*</em>所属部门：</span><abbr>
                        <Hi:RoleDropDownList ID="dropRole" runat="server" AllowNull="false" CssClass="iselect" />
                    </abbr>
                    </li>
                    <li><span class="formitemtitle">注册日期：</span><Hi:FormatedTimeLabel ID="lblRegsTimeValue" runat="server" />
                    </li>
                    <li style="display: none"><span class="formitemtitle">最后登录日期：</span>
                        <Hi:FormatedTimeLabel ID="lblLastLoginTimeValue" runat="server" />
                    </li>
                </ul>
                <div class="ml_198 ">
                    <asp:Button ID="btnEditProfile" runat="server" OnClientClick="return PageIsValid();" CssClass="btn btn-primary" Text="保 存" />
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

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function link() {
            window.location.href = 'Managers.aspx';
        }

        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>

