<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="PrizeRecord.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.PrizeRecord" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                 <li><a href="ManageLotteryTicket.aspx" runat="server" id="alist">
                    <asp:Literal ID="LitListTitle" runat="server">微抽奖</asp:Literal>活动管理</a></li>
                <li  class="hover"><a href="javascript:void(0)">
                    <asp:Literal ID="LitTitle" runat="server"></asp:Literal>中奖名单</a></li>
            </ul>
        </div>
        <!-- 添加按钮-->
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <asp:Repeater ID="rpMaterial" runat="server">
                <HeaderTemplate>
                    <table border="0" cellspacing="0"  cellpadding="0" class="table table-striped" >
                        <tr>
                            <td>昵称</td>
                            <td>状态</td>
                            <td>奖品</td>
                            <td>领奖人姓名</td>
                            <td>联系电话</td>
                            <td>中奖时间</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("UserName")%>&nbsp;</td>
                        <td><%#Eval("Prizelevel")%>&nbsp;</td>
                        <td><%#Eval("PrizeName")%>&nbsp;</td>
                        <td><%#Eval("RealName")%>&nbsp;</td>
                        <td><%#Eval("CellPhone")%>&nbsp;</td>
                        <td><%#Eval("PrizeTime")%>&nbsp;</td>
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

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
