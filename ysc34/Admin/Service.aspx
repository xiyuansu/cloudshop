<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Service.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Service" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

 <div  class="areacolumn clearfix" style="width:980px;">
 <iframe src="http://www.shopefx.com/zengzhi.html" style="border:0px;background-color:Transparent; height:1500px;width:980px;" scrolling="no" allowTransparency="true" frameborder="0"></iframe></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
        <script type="text/javascript" language="javascript">
          function gotoWeb(src)
          {
            window.open(src,"_blank")
          }
        </script>
</asp:Content>
