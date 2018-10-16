<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReferralInfo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.ReferralInfo" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
     <style>
        html {
            background: #fff !important;
        }

        body {
            padding: 0;
            width: 100%;
        }

        #mainhtml {
            margin: 0;
            padding: 20px 20px 90px 20px;
            width: 100% !important;
        }
        #btnCopy{
            float:right;
        }
      
    </style>
    <div class="areacolumn clearfix">
        <div class="columnright">
            <asp:Literal ID="litRefferralInfo" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>

