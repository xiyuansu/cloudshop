<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManualPushAgain.aspx.cs" Inherits="Hidistro.UI.Web.Admin.App.ManualPushAgain" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="PushRecords.aspx">管理</a></li>
                <li class="hover"><a  id="sendTitle" runat="server">重新推送</a></li>
            </ul>
            
        </div>
    </div>

    <div class="areacolumn clearfix">

        <div class="columnright">

            <div class="formitem validator2">
                <ul>
                    <li>
                        <span class="formitemtitle ">自定义推送：</span>
                        <asp:Label ID="lblPushTypeText" runat="server" Text="Label" ></asp:Label>

                    </li>
                    <li>
                        <span class="formitemtitle " id="spanPushTypeText" runat="server">链接：</span>
                        <asp:Label ID="lblPushTypeContext" runat="server" Text="Label" ></asp:Label>

                    </li>
                    <li>
                        <span class="formitemtitle ">推送标题：</span>
                        <asp:Label ID="lblPushTitle" runat="server" Text="Label" ></asp:Label>
                    </li>
                    <li>
                        <span class="formitemtitle ">推送内容：</span>
                        <asp:Label ID="lblPushContent" runat="server" Text="Label" ></asp:Label>

                    </li>
                    <li>
                        <span class="formitemtitle ">推送给：</span>
                        <asp:Label ID="lblPushTag" runat="server" Text="Label" ></asp:Label>
                    </li>
                    <li>
                        <span class="formitemtitle ">推送时间：</span>
                        <asp:Label ID="lblSendType" runat="server" Text="Label" ></asp:Label>
                    </li>
                    <li>
                        <span class="formitemtitle ">推送状态：</span>
                        <asp:Label ID="lblState" runat="server" Text="Label" ></asp:Label>
                    </li>
                     <li>
                        <span class="formitemtitle ">&nbsp;</span>
                    <asp:Button ID="btnSend" runat="server" Text="确 定" CssClass="btn btn-primary float" OnClick="btnSend_Click" />    
                    </li>
                </ul>
               
                <input id="locationUrl" runat="server" clientidmode="Static" type="hidden" value="" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfPushTypeLi" runat="server" />
    <asp:HiddenField ID="hfPushRecordId" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">   

</asp:Content>

