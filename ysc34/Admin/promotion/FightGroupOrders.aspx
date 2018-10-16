<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="FightGroupOrders.aspx.cs" Inherits="Hidistro.UI.Web.Admin.FightGroupOrders" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>





<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <style>
        html { background: #fff !important; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 20px 20px 60px 20px; width: 100% !important; }
        li { display: inline; float: left; }
    </style>

    <div class="dataarea mainwidth databody">

        <!--搜索-->

        <!--结束-->

        <!--数据列表区域-->
        <ul style="width: 100%;">
            <%foreach (var item in GroupOrders)
                { %>
            <li style="width: 33%;">
                <a href="javascript:goOrderDetail('<%=item.OrderId%>')"><%=item.OrderId%></a>
            </li>
            <%} %>
        </ul>
        <!--数据列表底部功能区域-->

    </div>

    <script type="text/javascript" language='JavaScript' defer='defer'>
        function goOrderDetail(orderId){
            var win = art.dialog.open.origin; 
            win.location.href="/admin/sales/OrderDetails.aspx?orderId="+orderId;

        }          
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

