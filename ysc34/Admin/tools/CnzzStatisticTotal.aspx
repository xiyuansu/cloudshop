<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CnzzStatisticTotal.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CnzzStatisticTotal" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    
  <div class="dataarea mainwidth databody">
</div>
<div class="Tempimg">
<iframe style="border:0px;background-color:Transparent; height:1500px;width:100%;" scrolling="no" allowTransparency="true" frameborder="0" id="framcnz" runat="server"></iframe>
</div>
<style>
.Tempimg { width:980px; margin:0 auto; overflow:hidden;}
</style> 

  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
        <script type="text/javascript" language="javascript">
          function gotoWeb(src)
          {
            window.open(src,"_blank")
          }
        </script>
</asp:Content>



