<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageThemes.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageThemes" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .Template ul li span img { width: 235px; height: 145px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">您正在使用“<asp:Literal ID="litThemeName" runat="server"></asp:Literal>”模板</a></li>
            </ul>
             <ul class="title-nav">
                <li><a href="editethems">编辑模板</a></li>
            </ul>
        </div>

        <div class="Tempimg">
            <table width="98%" border="0" cellspacing="0">
                <tr>
                    <td width="25%" rowspan="4">
                        <asp:Image runat="server" ID="imgThemeImgUrl" Width="235" Height="145" /></td>
                    <td width="2%" rowspan="4">&nbsp;</td>
                    <td width="54%" rowspan="4" align="right">
                        <asp:Image runat="server" ID="Image1" Width="510" Height="145" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        <div class="blank12 clearfix"></div>
        <div class="datafrom">
            <div class="Template">
                <h1>可供您选择的模板（总数为：<asp:Literal ID="lblThemeCount" runat="server" />）</h1>
                <asp:Repeater runat="server" ID="dtManageThemes">
                    <ItemTemplate>
                        <ul>
                            <li>
                                <span>
                                    <Hi:DisplayThemesImages ID="themeImg" runat="server" Src='<%#  Eval("ThemeImgUrl") %>' ThemeName='<%# Eval("ThemeName") %>' />
                                </span>
                                <em>
                                    <p><%# Eval("Name") %></p>
                                    <asp:LinkButton ID="btnManageThemesOK" runat="server" CommandName="btnUse" CssClass="btn btn-primary" Text="应用" />
                                    <asp:LinkButton ID="btnDownload" runat="server" CommandName="download" CssClass="btn btn-default" Text="下载" />
                                </em>
                            </li>
                        </ul>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </div>
    </div>

    <div class="Pop_up" id="div_templeteupload" style="display: none;">
        <h1>上传模板
        </h1>
        <div class="img_datala">
            <span class="glyphicon glyphicon-trash"></span>
        </div>
        <div class="mianform" style="text-align: center;">
            <p>
                上传模板文件：<asp:FileUpload ID="fileTemplate" runat="server" />
                <br />
                <em style="font-style: normal;">注意:上传文件必须为zip格式，并将会覆盖原来模板</em>
            </p>
            <p>
                <asp:Button ID="btnUpload2" runat="server" Text="上传" CssClass="btn btn-primary" OnClientClick="javascript:return CheckUpload('fileTemplate');" /></p>
            <input type="hidden" id="hdtempname" runat="server" />
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function ShowUpload(themname) {
            if (themname.replace(/\s/g, "").length <= 0) {
                alert("请选择要上传的模板");
                return false;
            }
            $("#ctl00_contentHolder_hdtempname").val(themname);
            DivWindowOpen(500, 280, 'div_templeteupload');
        }

        function CheckUpload(filecontrol) {
            if ($("#ctl00_contentHolder_" + filecontrol).val().replace(/\s/g, "").length <= 0) {
                alert("请选择要上传的文件");
                return false;
            }
        }
    </script>
</asp:Content>

