<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AppletTemplateSetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AppletTemplateSetting" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="blank12 clearfix">
    </div>
    <div class="dataarea mainwidth databody">
        <div class="functionHandleArea clearfix m_none">

            <div class="batchHandleArea">
                <a href="../Applet/小程序模板消息配置指南.docx" class="btn btn-default" target="_blank">下载帮助指南</a>
                <div class="batchHandleButton float_r">
                    <input type="button" id="btnAppletSaveSendSetting"  value="保存设置" class="btn btn-primary" />
                </div>

            </div>
        </div>
        <div class="datalist clearfix">
            <asp:Repeater ID="grdWxTempletsNew" runat="server">
                <HeaderTemplate>
                    <table cellspacing="0" border="0" id="grdWxTempletsNew" class="table table-striped">
                        <tbody>
                            <tr>
                                <th width="18%">消息类型</th>
                                <th width="18%">微信模板名称</th>
                                <th width="24%">模板编号</th>
                                <th>模板ID</th>
                            </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hfMessageType" runat="server" Value='<%#Eval("MessageType") %>' />
                            <%#Eval("MessageType") %>
                            <a name="<%# Eval("MessageType")%>"></a>
                        </td>
                        <td>
                            <%# Eval("AppletTemplateName") %>
                        </td>
                        <td>
                            <%# Eval("AppletTemplateNo") %>
                        </td>
                        <td>
                            <asp:TextBox runat="server" Text='<%# Eval("WxAppletTemplateId") %>' messagetype='<%# Eval("MessageType") %>' Width="350px" CssClass="forminput form-control"></asp:TextBox>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></tbody></table></FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <input type="hidden" id="dataurl" value="/Admin/Applet/ashx/AppletTemplatesSetting.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Admin/tools/scripts/WeiXinTemplatesSetting.js?v=3.2"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

