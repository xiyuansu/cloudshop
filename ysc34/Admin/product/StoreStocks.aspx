<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="StoreStocks.aspx.cs" Inherits="Hidistro.UI.Web.Admin.StoreStocks" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div id="divempty" runat="server" visible="false" style="text-align: center; font-size: 24px; color: red; padding: 20px 0px 20px 0px">
            没有任何门店有库存
        </div>
        <div class="datalist clearfix" id="divlist" runat="server">
            <asp:Repeater ID="repStoreStock" runat="server" OnItemDataBound="repStoreStock_ItemDataBound">
                <ItemTemplate>
                    <div style="padding: 10px 0px 10px 10px; font-weight: bold;"><%#Eval("StoreName") %></div>
                    <asp:Repeater ID="repStocks" runat="server">
                        <HeaderTemplate>
                            <table width="100%" border="0" cellspacing="0" class="table table-striped">
                                <tr class="table_title">
                                    <td width="80%">规格
                                    </td>
                                    <td width="20%">库存
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("SKUContent")%></td>
                                <td><%#Eval("Stock") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
