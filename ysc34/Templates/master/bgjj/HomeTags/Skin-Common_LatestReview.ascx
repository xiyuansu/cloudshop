<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<li>
    <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                    <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl180" isdelayedloading="true" /></Hi:ProductDetailsLink>
    <div class="ueser-txt">
        <span><i></i>
        <%# string.IsNullOrEmpty(Eval("UserName").ToString())?"":(Eval("UserName").ToString().Substring(0,1)+"***"+Eval("UserName").ToString().Substring(Eval("UserName").ToString().Length-1,1)) %>
		</span>
        <b><em><%# Eval("ReviewText") %></em></b>
    </div>
</li>
