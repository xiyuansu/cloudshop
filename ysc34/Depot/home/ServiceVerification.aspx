<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="ServiceVerification.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.ServiceVerification" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a class="hover">服务类商品核销</a></li>
            </ul>
        </div>

        <div class="datafrom">
            <div class="formitem validator1 depot-p">
                <ul>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>请输入核销码：</span>
                        <input type="text" id="txtVerificationItems" class="forminput form-control" style="width: 160px" />
                    </li>

                </ul>
                <ul class="btntf Pa_198 clear">
                    <li>
                        <input type="button" value="核 销" class="btn btn-primary inbnt" id="btnAdd" onclick="doSubmit();" /></li>
                </ul>
            </div>
        </div>
    </div>

    <script src="/Depot/home/scripts/ServiceVerification.js?v=3.1" type="text/javascript"></script>
</asp:Content>
