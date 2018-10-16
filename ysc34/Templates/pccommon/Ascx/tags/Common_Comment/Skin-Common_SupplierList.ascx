<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a href='/SupplierAbout?SupplierId=<%# Eval("SupplierId")%>'><img src="<%# Eval("Picture") %>" onerror="this.src='/Utility/pics/none.gif'" alt="<%# Eval("SupplierName") %>"/><br /><Hi:SubStringLabel ID="lblSupplierName" StrLength="10" Field="SupplierName" runat="server" /></a>