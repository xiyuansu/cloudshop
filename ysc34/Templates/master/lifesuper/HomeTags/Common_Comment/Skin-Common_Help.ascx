<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>

<dl>
    <dt><a href='<%# GetRouteUrl("Helps", new {CategoryId =Eval("CategoryId")})%>'><%# Eval("Name") %> </a> </dt>
    <asp:Repeater ID="rptHelp" runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.Helps") %>'>
        <ItemTemplate>
            <dd>
                <a href='/help/show/<%# Eval("HelpId")%>' target="_blank" title='<%#Eval("Title") %>'>
                    <Hi:SubStringLabel ID="SubStringLabel" Field="Title" runat="server" />
                </a>
            </dd>
        </ItemTemplate>
    </asp:Repeater>
</dl>
