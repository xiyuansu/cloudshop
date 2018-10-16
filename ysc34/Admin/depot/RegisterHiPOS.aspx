<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="RegisterHiPOS.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.RegisterHiPOS" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
     <script type="text/javascript" src="/utility/jquery.form.js" ></script>
    <script type="text/javascript">
        window.history.go(1);
        $(function () {
            InitValidators();
        })

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtSellerName', 1, 50, false, null, '商户名称不能为空，长度必须小于或等于50个字'));
            initValid(new InputValidator('ctl00_contentHolder_txtContactName', 1, 10, false, null, '联系人姓名不能为空，长度必须小于或等于10个字'));
            initValid(new InputValidator('ctl00_contentHolder_txtContactPhone', 7, 20, false, '1\\d{10}$', '手机号码不能为空，请输入合法的手机号码'));
        }
    </script>
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem validator1">
                
                <div class="step" style="margin-bottom: 30px;">
                    <span class="setp_hover">1.注册商户</span>
                    <span>2.设置支付</span>
                    <span class="mr0">3.完成</span>
                </div>
                <div class="blockquote-default blockquote-tip mb_20">提示：商户信息填写注册成功后，不能修改。</div>
                <ul>
                    <li class="mb_0 mt_10"><span class="formitemtitle"><em>*</em>商户名称：</span>
                        <asp:TextBox ID="txtSellerName" runat="server" CssClass="forminput form-control" MaxLength="50"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtSellerNameTip"></p>

                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>联系人姓名：</span>
                        <asp:TextBox ID="txtContactName" runat="server" CssClass="forminput form-control" MaxLength="10"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtContactNameTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>联系人电话：</span>
                        <asp:TextBox ID="txtContactPhone" runat="server" CssClass="forminput form-control" MaxLength="20"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtContactPhoneTip"></p>
                    </li>
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-primary" OnClientClick="return PageIsValid();" OnClick="btnAdd_Click">下一步</asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
    </div>



</asp:Content>
