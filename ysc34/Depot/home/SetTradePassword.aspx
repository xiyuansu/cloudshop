<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="SetTradePassword.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.SetTradePassword" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a class="hover">交易密码设置</a></li>
            </ul>
        </div>

        <div class="datafrom">
            <div class="formitem validator1 depot-p">
                <ul>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>交易密码：</span>
                        <input type="password" id="txtPassword" class="forminput form-control" style="width: 160px" maxlength="6" />
                        <p>
                            密码必须为6位!
                        </p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>重复交易密码：</span>
                        <input type="password" id="txtRePassword" class="forminput form-control" style="width: 160px" maxlength="6" />
                        <p>
                            密码必须为6位!
                        </p>
                    </li>

                </ul>
                <ul class="btntf Pa_198 clear">
                    <li>
                        <input type="button" value="保 存" class="btn btn-primary inbnt" id="btnAdd" onclick="doSubmit();" /></li>
                </ul>
            </div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hidSelectProducts" />
    <asp:HiddenField runat="server" ID="hidProductIds" />
    <asp:HiddenField runat="server" ID="hidAllSelectedProducts" />


    <script src="/Depot/home/scripts/SetTradePassword.js?v=3.1" type="text/javascript"></script>
</asp:Content>
