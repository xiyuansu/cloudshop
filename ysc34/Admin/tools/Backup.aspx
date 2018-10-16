<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Backup.aspx.cs" Inherits="Hidistro.UI.Web.Admin.store.Backup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="restoredatabase.aspx">数据恢复</a></li>
                <li  class="hover"><a href="javascript:void">备份</a></li>
                
            </ul>
         
        </div>
        
        <div class="datafrom">
                 
            <div class="formitem validator1">
               <asp:Button ID="btnBackup" runat="server" Text="开始备份" CssClass="btn btn-primary inbnt" />
            </div>
        </div>
    </div>
</asp:Content>
