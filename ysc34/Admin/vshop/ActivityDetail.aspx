<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityDetail.aspx.cs"
    Inherits="Hidistro.UI.Web.Admin.ActivityDetail" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:content id="Content1" contentplaceholderid="headHolder" runat="server">
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
            padding: 20px 20px 60px 20px;
            width: 100% !important;
        }

      
    </style>
</asp:content>
<asp:content id="Content3" contentplaceholderid="contentHolder" runat="server" clientidmode="Static">
    <div class="dataarea mainwidth databody">
    
    <!--结束-->
    <!--数据列表区域-->
    <div class="datalist clearfix">
          <asp:Repeater ID="rpt" runat="server">
                    <HeaderTemplate>
                        <table border="0" cellspacing="0" width="80%" cellpadding="0" class="table table-striped">
                            <tr class="table_title">
                                <td>报名日期</td>
                                <td><%= _act.Item1 %>&nbsp;</td>
                                <td><%= _act.Item2 %>&nbsp;</td>
                                <td><%= _act.Item3 %>&nbsp;</td>
                                <td><%= _act.Item4 %>&nbsp;</td>
                                <td><%= _act.Item5 %>&nbsp;</td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval("SignUpDate", "{0:yyyy-MM-dd}")%></td>
                            <td><%#Eval("Item1")%>&nbsp;</td>
                            <td><%#Eval("Item2")%>&nbsp;</td>
                            <td><%#Eval("Item3")%>&nbsp;</td>
                            <td><%#Eval("Item4")%>&nbsp;</td>
                            <td><%#Eval("Item5")%>&nbsp;</td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            <%if (nodata)
                { %><div class="dataNull"><img src="../images/data_null.png"><p>没有找到符合条件的数据!</p></div><%} %>
    </div>
    <!--数据列表底部功能区域-->
  </div>

</asp:content>
<asp:content id="Content2" contentplaceholderid="validateHolder" runat="server">
</asp:content>
