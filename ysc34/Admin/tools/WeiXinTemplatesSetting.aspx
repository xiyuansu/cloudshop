<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="WeiXinTemplatesSetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXinTemplatesSetting" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="blank12 clearfix">
    </div>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="sendmessagetemplets.aspx">消息提醒</a></li>
                <li class="hover"><a href="javascript:void">微信模板设置</a></li>
                <li>
                    <a href="WeixinSettings.html" target="_blank">手动设置帮助</a></li>

            </ul>

        </div>
        <div class="functionHandleArea clearfix m_none">

            <div class="batchHandleArea">

                <div class="batchHandleButton float_r">
                    <input type="button" id="btnQuickSet" class="btn btn btn-primary mr_20" value="一键设置" />
                    <input type="button" id="btnSaveSendSetting"  value="保存设置" class="btn btn-default" />
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
                            <%#Eval("Name") %>
                            <a name="<%# Eval("MessageType")%>"></a>
                        </td>
                        <td>
                            <%# Eval("WeiXinName") %>
                        </td>
                        <td>
                            <%# Eval("WeiXinTemplateNo") %>
                        </td>
                        <td>
                            <asp:TextBox runat="server" Text='<%# Eval("WeixinTemplateId") %>' messagetype='<%# Eval("MessageType") %>' Width="350px" CssClass="forminput form-control"></asp:TextBox>
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
    <input type="hidden" id="dataurl" value="/Admin/tools/ashx/WeiXinTemplatesSetting.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Admin/tools/scripts/WeiXinTemplatesSetting.js"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
