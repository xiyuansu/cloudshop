<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        /**
 *    打印相关
*/
        @media print
        {
            .notprint
            {
                display: none;
            }
            .PageNext
            {
                page-break-after: always;
            }
        }
        @media screen
        {
            .notprint
            {
                display: inline;
                cursor: hand;
            }
        }
        .submit_DAqueding{ width:100px; height:38px;background:url(../images/icon.gif) no-repeat -159px -362px; color:#FFF; border:0;font-size:14px; font-weight:700;cursor:pointer;}
    
        #bg{ display: none;  position: absolute;  top: 0%;  left: 0%;  width: 100%;  height: 100%;  background-color: black;  z-index:1001;  -moz-opacity: 0.7;  opacity:.70;  filter: alpha(opacity=70);}
        #show{display: none;  position: absolute;  top: 35%;  left: 40%;  width: 25%;  height: 4%;  padding: 8px;  border: 8px solid #E8E9F7;  background-color: white;  z-index:1002;  overflow: auto;}
 
    </style>
    <script src="../../Utility/jquery-1.6.4.min.js"></script>
<script src="../../Utility/jquery.artDialog.js"></script>
<script src="../../Utility/iframeTools.js"></script>
    <script language="javascript">
        function closeclicks() {
            art.dialog.close();
            
        }
        function showdiv() {
            document.getElementById("bg").style.display = "block";
            document.getElementById("show").style.display = "block";
        }
        function hidediv() {
           
            document.getElementById("bg").style.display = "none";
            document.getElementById("show").style.display = "none";
           
        } 
</script>
</head>
<body style="text-align: center;margin:0 auto;">
 
<script language="javascript" src="../js/LodopFuncs.js"></script>
<object  id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width=0 height=0> 
       <embed id="LODOP_EM" type="application/x-print-lodop" width=0 height=0></embed>
</object>
   <div id="bg"></div>
   <div id="show"> 
  正在打印，请勿关闭窗口。。。 
</div>
    <div style="text-align: center; margin-top:0px; margin-left:0px; margin-right:0px; margin-bottom:50px;" id="divContent" runat="server">
      
    </div>
    
       <form id="Form1" runat="server">
   <div style=" width:100px; height:70px;  margin-left:400px;   " id="divprint">  
   <asp:Button ID="btprint" runat="server" Text="确认打印" CssClass="notprint submit_DAqueding"/></div> 
   <div id="divcloseprint" style=" display:none;  margin-top:100px;"><input type="button" value="关闭" class="btn btn-danger" id="printBtn" onclick="closeclicks()"/></div>
      </form>
  
    <form action="PrintComplete.aspx" name="printForm" method="post" id="PrintForm">
        <input type="hidden" name="orderIds" value="<%= UpdateOrderIds %>" />
        <input type="hidden" name="mailNo" value="<%= mailNo %>" />
        <input type="hidden" name="templateName" value="<%= templateName %>" />
        </form>
    
</body>
</html>
