<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMenu.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AliOH.AddMenu" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>





<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="ManageMenu.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">添加</a></li>
            </ul>
        </div>
        <div class="columnright">

            <div class="formitem validator2">
                <ul>
                    <li class="mb_0"><span class="formitemtitle Pw_100"><em>*</em>菜单名称：</span>
                        <asp:TextBox ID="txtCategoryName" runat="server" CssClass="forminput form-control" placeholder="菜单名称不能为空，一级菜单最多4个汉字，二级菜单最多12个汉字。" />
                        <p id="ctl00_contentHolder_txtCategoryNameTip"></p>
                    </li>
                    <li runat="server" id="liParent"><span class="formitemtitle Pw_100">上级菜单：</span>
                        <asp:Literal runat="server" ID="lblParent"></asp:Literal>
                    </li>
                    <li><span class="formitemtitle Pw_100">绑定对象：</span>
                        <asp:DropDownList ID="ddlType" runat="server" Height="33" CssClass="form-control iselect" Width="230"
                            AutoPostBack="True" ClientIDMode="Static"
                            OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            <asp:ListItem Text="不绑定" Value="0"></asp:ListItem>
                            <asp:ListItem Text="自定义页面" Value="2"></asp:ListItem>
                            <asp:ListItem Text="首页" Value="3"></asp:ListItem>
                            <asp:ListItem Text="产品分类" Value="4"></asp:ListItem>
                            <asp:ListItem Text="购物车" Value="5"></asp:ListItem>
                            <asp:ListItem Text="会员中心" Value="6"></asp:ListItem>
                            <asp:ListItem Text="链接" Value="8"></asp:ListItem>
                        </asp:DropDownList>
                        <p id="P2">绑定后主菜单将不能再添加下属子菜单</p>
                    </li>
                    <li id="liValue" runat="server"><span class="formitemtitle Pw_100" id="liTitle" runat="server">&nbsp;</span>
                        <asp:DropDownList ID="ddlValue" runat="server" Height="33" CssClass="form-control iselect" Width="230" />
                    </li>
                    <li id="liUrl" runat="server"><span class="formitemtitle Pw_100">链接地址：</span>
                        <asp:TextBox ID="txtUrl" runat="server" Text="http://" CssClass="form-control forminput" Width="300px" />
                    </li>
                    <li>
                        <span class="formitemtitle Pw_100">&nbsp;</span>
                        <asp:Button ID="btnAddMenu" runat="server" OnClientClick="return PageIsValid();" Text="确 定" CssClass="btn btn-primary float" />
                    </li>
                </ul>
            </div>
        </div>

    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtCategoryName', 1, 12, false, null, '必填 菜单名称不能为空，一级菜单最多4个汉字，二级菜单最多12个汉字。'))
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>


