<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<li>
<h2 class="font16"><%#Eval("IconUrl").ToString()=="" ? Eval("Name") : ""%> </h2>
<asp:Repeater ID="rptHelp" runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.Helps") %>'>
       <ItemTemplate>
           <p>
             <a href='/help/show/<%# Eval("HelpId")%>' target="_blank"  Title='<%#Eval("Title") %>'>
                <Hi:SubStringLabel ID="SubStringLabel" Field="Title" runat="server"  />
            </a>  
           </p>
      </ItemTemplate>
    </asp:Repeater>

</li>
 