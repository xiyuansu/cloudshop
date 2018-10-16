<%@ Page Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="MemberDetails.aspx.cs" Inherits="Hidistro.UI.Web.Depot.MemberDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">

    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <ul class="title-nav">
                    <li>
                        <a href="javascript:void" class="hover">查看“<asp:Literal runat="server" ID="litUserName" />”会员信息</a></li>
                </ul>
                <i class="glyphicon glyphicon-question-sign" data-container="body" style="cursor: pointer" data-toggle="popover" data-placement="left" title="操作说明" data-content="会员账户的详细信息"></i>
            </div>
            <div class="formitem clearfix top_10 left">
                <ul>
                    <li><span class="formitemtitle">会员等级：</span>
                        <asp:Literal runat="server" ID="litGrade" /></li>
                    <li><span class="formitemtitle">姓名：</span><asp:Literal runat="server" ID="litRealName" /></li>
                    <li><span class="formitemtitle">生日：</span><asp:Literal runat="server" ID="litBirthDate" /></li>
                    <li><span class="formitemtitle">性别：</span><asp:Literal runat="server" ID="litGender" /></li>
                    <li><span class="formitemtitle">电子邮件地址：</span><asp:Literal runat="server" ID="litEmail" /></li>
                    <li><span class="formitemtitle">详细地址：</span>
                        <asp:Literal runat="server" ID="litAddress" /></li>
                    <%--<li><span class="formitemtitle">旺旺：</span><asp:Literal runat="server" ID="litWangwang" /></li>--%>
                    <li><span class="formitemtitle">QQ：</span><asp:Literal runat="server" ID="litQQ" /></li>
                    <li><span class="formitemtitle">昵称：</span><asp:Literal runat="server" ID="litMSN" /></li>
                    <li><span class="formitemtitle">手机号码：</span><asp:Literal runat="server" ID="litCellPhone" /></li>
                    <li><span class="formitemtitle">注册日期：</span><asp:Literal runat="server" ID="litCreateDate" />
                    </li>
                </ul>
            </div>

        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="Server">
</asp:Content>
