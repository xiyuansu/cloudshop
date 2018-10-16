<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="JDSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.JDSettings" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Context" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <%--富文本编辑器start--%>
    <link rel="stylesheet" href="/Utility/Ueditor/plugins/uploadify/uploadify-min.css" />

    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem validator1 p-100 setorder">
                <ul>
                    <li>
                        <h2 class="clear">京东设置</h2>
                    </li>
                    <li>
                <div class="blockquote-default blockquote-tip">
                    1.Appkey和AppSecret请商家去京东接入自研应用后获得。请阅读<a target="_blank" href="https://jos.jd.com/doc/channel.htm?id=68">获取指南</a><br />
                    2.保存AppKey和AppSecret后，再点击“前往生成”，再按操作提示生成AccessToken。
                </div>

                    </li>
                    <li class="mb_10"><span class="formitemtitle "><em>*</em>AppKey：</span>
                        <div class="input-group" style="font-size: 14px">
                            <asp:TextBox ID="JdAppKeyTextBox" runat="server" CssClass="form-control" />
                        </div>
                    </li>
                    <li class="mb_10"><span class="formitemtitle"><em>*</em>AppSecret：</span>
                        <div class="input-group" style="font-size: 14px">
                            <asp:TextBox ID="JdAppSecretTextBox" runat="server" CssClass="form-control" />
                        </div>
                    </li>
                    <li class="mb_10"><span class="formitemtitle"><em>*</em>AccessToken：</span>
                        <div class="input-group" style="font-size: 14px">
                            <asp:TextBox ID="JdAccessTokenTextBox" runat="server" CssClass="form-control" />
                            &nbsp;&nbsp;<asp:HyperLink ID="createAccessTokenHyperLink" Target="_blank" runat="server" Text="前往生成" />
                        </div>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button ID="btnOK" runat="server" Text="提交" CssClass="btn btn-primary" OnClick="btnOK_Click" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>

